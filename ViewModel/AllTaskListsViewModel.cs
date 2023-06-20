using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using ToDoList.Model;
using ToDoList.Service;
using ToDoList.View;

namespace ToDoList.ViewModel
{
    public class AllTaskListsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TaskList> TaskLists { get; private set; }
        private TaskList? selectedTaskList;
        public TaskList? SelectedTaskList
        {
            get => selectedTaskList;
            set
            {
                selectedTaskList = value;
                OnPropertyChanged(nameof(SelectedTaskList));
                OnPropertyChanged(nameof(SelectedTaskListVisibility));
                OnPropertyChanged(nameof(ActiveTasksVisibility));
                OnPropertyChanged(nameof(FinishedTasksVisibility));
                OnPropertyChanged(nameof(SelectedTaskListVisibilityInversed));
            }
        }
        private TaskList finishedTaskList;
        public TaskList FinishedTaskList
        {
            get 
            {
                return finishedTaskList;
            }
            private set
            {                
                finishedTaskList = value;
                OnPropertyChanged(nameof(FinishedTaskList));
            }
        }        
        public Visibility SelectedTaskListVisibility
        { 
            get => SelectedTaskList != null ? Visibility.Visible : Visibility.Collapsed; 
        }
        public Visibility SelectedTaskListVisibilityInversed
        {
            get => SelectedTaskList == null ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility ActiveTasksVisibility
        {
            get => SelectedTaskList != null && SelectedTaskList.ActiveTasks != null ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility FinishedTasksVisibility
        { 
            get => SelectedTaskList != null && SelectedTaskList.IsFinishedTasksStored ? Visibility.Visible : Visibility.Collapsed; 
        }
        private Task? selectedTask;
        public Task? SelectedTask
        {
            get => selectedTask;
            set
            {
                if(value != null)
                {
                    if (selectedTaskList == null)
                        throw new Exception("Selected task can't be specified as no task list is selected!");
                    else if ((selectedTaskList.ActiveTasks != null && !selectedTaskList.ActiveTasks.Contains(value)) && (selectedTaskList.IsFinishedTasksStored && !selectedTaskList.FinishedTasks.Contains(value)))
                        throw new Exception("Selected task can't be specified as is doesn't belong to selected task list");
                }               
                selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
                OnPropertyChanged(nameof(SelectedTaskVisibility));
                OnPropertyChanged(nameof(SelectedTaskVisibilityInversed));
                OnPropertyChanged(nameof(FinishTaskButtonVisibility));
                OnPropertyChanged(nameof(DeleteTaskButtonVisibility));
            }
        }
        
        public Visibility SelectedTaskVisibility
        { 
            get => SelectedTaskList != null && SelectedTask != null ? Visibility.Visible : Visibility.Collapsed; 
        }
        public Visibility SelectedTaskVisibilityInversed
        {
            get => !(SelectedTaskList != null && SelectedTask != null) ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility FinishTaskButtonVisibility
        {
            get => SelectedTaskList != null && SelectedTask != null && SelectedTaskList.ActiveTasks != null && SelectedTaskList.ActiveTasks.Contains(SelectedTask) ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility DeleteTaskButtonVisibility
        {
            get => SelectedTaskList != null && SelectedTask != null && SelectedTaskList.IsFinishedTasksStored ? Visibility.Visible : Visibility.Collapsed;
        }
        public string? NewTaskName { get; set; }
        #region Commands
        private BaseCommand? openNewTaskViewCommand;
        public BaseCommand OpenNewTaskViewCommand
        {
            get
            {
                if (openNewTaskViewCommand != null)
                    return openNewTaskViewCommand; 
                else
                {
                    Action<object?> execute = o =>
                    {
                        NewTaskView newTaskView = new NewTaskView(SelectedTaskList);
                        newTaskView.Show();
                    };
                    Predicate<object?> canExecute = o => SelectedTaskList != null && SelectedTaskList.ActiveTasks != null;
                    openNewTaskViewCommand = new BaseCommand(execute, canExecute);
                    return openNewTaskViewCommand;
                }
            }
        }
        
        private BaseCommand? openEditTaskViewCommand;
        public BaseCommand OpenEditTaskViewCommand
        {
            get
            {
                if (openEditTaskViewCommand != null)
                    return openEditTaskViewCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        EditTaskView editTaskView = new EditTaskView(SelectedTask);
                        editTaskView.Show();
                    };
                    Predicate<object?> canExecute = o => SelectedTask != null;
                    openEditTaskViewCommand = new BaseCommand(execute, canExecute);
                    return openEditTaskViewCommand;
                }
            }
        }
        private BaseCommand? openEditTaskListViewCommand;
        public BaseCommand OpenEditTaskListViewCommand
        {
            get
            {
                if (openEditTaskListViewCommand != null)
                    return openEditTaskListViewCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        EditTaskListView editTaskListView = new EditTaskListView(SelectedTaskList);
                        editTaskListView.Closed += (o, e) => OnPropertyChanged(nameof(FinishedTasksVisibility));
                        editTaskListView.Show();
                    };
                    Predicate<object?> canExecute = o => SelectedTaskList != null;
                    openEditTaskListViewCommand = new BaseCommand(execute, canExecute);
                    return openEditTaskListViewCommand;
                }
            }
        }

        private BaseCommand? openNewTaskListViewCommand;
        public BaseCommand OpenNewTaskListViewCommand
        {
            get
            {
                if (openNewTaskListViewCommand != null)
                    return openNewTaskListViewCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        NewTaskListView newTaskListView = new NewTaskListView(TaskLists);
                        newTaskListView.Show();
                    };
                    openNewTaskListViewCommand = new BaseCommand(execute, null);
                    return openNewTaskListViewCommand;
                }
            }
        }

        private BaseCommand? completeTaskCommand;
        public BaseCommand CompleteTaskCommand
        {
            get
            {
                if (completeTaskCommand != null)
                    return completeTaskCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        Task completedTask = (Task)o;
                        SelectedTaskList.ActiveTasks.Remove(completedTask);
                        if (SelectedTaskList.IsFinishedTasksStored)
                        {
                            SelectedTaskList.FinishedTasks.Add(completedTask);
                            GetFinishedTasks();
                        }
                    };
                    Predicate<object?> canExecute = o => o != null && SelectedTaskList != null && SelectedTaskList.ActiveTasks != null && SelectedTaskList.ActiveTasks.Contains((Task)o);
                    completeTaskCommand = new BaseCommand(execute, canExecute);
                    return completeTaskCommand;
                }
            }
        }

        private BaseCommand? deleteTaskCommand;
        public BaseCommand DeleteTaskCommand
        {
            get
            {
                if (deleteTaskCommand != null)
                    return deleteTaskCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        if (MessageBox.Show("Удалить задачу?", "Удаление задачи", MessageBoxButton.YesNo, MessageBoxImage.None) == MessageBoxResult.Yes)
                        {
                            Task taskToDelete = (Task)o;
                            SelectedTaskList.ActiveTasks.Remove(taskToDelete);
                            if (SelectedTaskList.IsFinishedTasksStored)
                            { 
                                SelectedTaskList.FinishedTasks.Remove(taskToDelete);
                                GetFinishedTasks();
                            }
                        }
                        
                    };
                    Predicate<object?> canExecute = o => o != null && SelectedTaskList != null;
                    deleteTaskCommand = new BaseCommand(execute, canExecute);
                    return deleteTaskCommand;
                }
            }
        }
        private BaseCommand? deleteTaskListCommand;
        public BaseCommand DeleteTaskListCommand
        {
            get
            {
                if (deleteTaskListCommand != null)
                    return deleteTaskListCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        if (MessageBox.Show("Удалить группу задач?", "Удаление группы задач", MessageBoxButton.YesNo, MessageBoxImage.None) == MessageBoxResult.Yes)
                        {
                            TaskList taskListToDelete = (TaskList)o;
                            TaskLists.Remove(taskListToDelete);
                            //SelectedTaskList = null;
                        }

                    };
                    Predicate<object?> canExecute = o => o != null;
                    deleteTaskListCommand = new BaseCommand(execute, canExecute);
                    return deleteTaskListCommand;
                }
            }
        }

        private BaseCommand? saveDataCommand;
        public BaseCommand SaveDataCommand
        {
            get
            {
                if (saveDataCommand != null)
                    return saveDataCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        string path = (string)o;
                        JsonSerializerOptions options = new JsonSerializerOptions() 
                        { 
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                        };
                        var jsonSerializer = new JsonSerializer<ObservableCollection<TaskList>>(options);
                        jsonSerializer.Serialize(path, TaskLists);
                    };
                    Predicate<object?> canExecute = o => o is string;
                    saveDataCommand = new BaseCommand(execute, canExecute);
                    return saveDataCommand;
                }
            }
        }
        private BaseCommand? saveDataToFileCommand;
        public BaseCommand SaveDataToFileCommand
        {
            get
            {
                if (saveDataToFileCommand != null)
                    return saveDataToFileCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();

                        saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
                        saveFileDialog.Filter = "Data Files(*.JSON)|*.JSON";
                        saveFileDialog.RestoreDirectory = true;
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            saveDataCommand.Execute(saveFileDialog.FileName);
                        }
                    };
                    saveDataToFileCommand = new BaseCommand(execute, null);
                    return saveDataToFileCommand;
                }
            }
        }
        private BaseCommand? loadDataCommand;
        public BaseCommand LoadDataCommand
        {
            get
            {
                if (loadDataCommand != null)
                    return loadDataCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        string path = (string)o;
                        JsonSerializerOptions options = new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                        };
                        var jsonSerializer = new JsonSerializer<ObservableCollection<TaskList>>(options);
                        try
                        {
                            TaskLists = jsonSerializer.Deserialize(path) ?? new ObservableCollection<TaskList>();
                            OnPropertyChanged(nameof(TaskLists));
                        }
                        catch (Exception)
                        {
                            TaskLists = new ObservableCollection<TaskList>();
                        }
                        TaskList finished = TaskLists.FirstOrDefault(taskList => taskList.Name == "Завершенные");
                        if (finished == null)
                        {
                            finished = new TaskList("Завершенные", false, true);
                            TaskLists?.Add(finished);
                        }
                        FinishedTaskList = finished;
                        GetFinishedTasks();
                    };
                    Predicate<object?> canExecute = o => o is string;
                    loadDataCommand = new BaseCommand(execute, canExecute);
                    return loadDataCommand;
                }
            }
        }
        private BaseCommand? loadDataFromFileCommand;
        public BaseCommand LoadDataFromFileCommand
        {
            get
            {
                if (loadDataFromFileCommand != null)
                    return loadDataFromFileCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        
                            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                            openFileDialog.Filter = "Data Files(*.JSON)|*.JSON";
                            openFileDialog.RestoreDirectory = true;
                            if (openFileDialog.ShowDialog() == true)
                            {
                                loadDataCommand.Execute(openFileDialog.FileName);                              
                            }                                                       
                    };
                    loadDataFromFileCommand = new BaseCommand(execute, null);
                    return loadDataFromFileCommand;
                }
            }
        }
        #endregion
        public AllTaskListsViewModel()
        {
            LoadDataCommand.Execute("active.json");           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void GetFinishedTasks()
        {
            FinishedTaskList.FinishedTasks = new ObservableCollection<Task>(TaskLists
                .Where(taskList => taskList.FinishedTasks != null && taskList != FinishedTaskList)
                .SelectMany(taskList => taskList.FinishedTasks)
                .ToList());
            OnPropertyChanged(nameof(FinishedTaskList));
        }
    }
}
