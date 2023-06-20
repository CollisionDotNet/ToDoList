using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    public class EditTaskListViewModel : INotifyPropertyChanged
    {
        private string? taskListName;
        public string? TaskListName
        {
            get => taskListName;
            set
            {
                taskListName = value;
                OnPropertyChanged(nameof(TaskListName));
            }
        }
        private bool taskListStoreFinishedTasks;
        public bool TaskListStoreFinishedTasks
        {
            get => taskListStoreFinishedTasks;
            set
            {
                taskListStoreFinishedTasks = value;
                OnPropertyChanged(nameof(TaskListStoreFinishedTasks));
            }
        }
        public TaskList TaskList { get; private init; }
        public Action? CloseView { get; set; }

        private BaseCommand? editTaskListCommand;
        public BaseCommand EditTaskListCommand
        {
            get
            {
                if (editTaskListCommand != null)
                    return editTaskListCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        TaskList.Name = TaskListName;
                        if(TaskList.IsFinishedTasksStored != TaskListStoreFinishedTasks)
                        {
                            if (TaskListStoreFinishedTasks)
                                TaskList.FinishedTasks = new ObservableCollection<Task>();
                            else
                                TaskList.FinishedTasks = null;
                            TaskList.IsFinishedTasksStored = TaskListStoreFinishedTasks;
                        }
                        CloseView?.Invoke();
                    };
                    Predicate<object?> canExecute = o => TaskListName != null && TaskListName.Length >= 3;
                    editTaskListCommand = new BaseCommand(execute, canExecute);
                    return editTaskListCommand;
                }
            }
        }

        public EditTaskListViewModel(TaskList taskList)
        {
            TaskList = taskList;
            TaskListName = taskList.Name;
            TaskListStoreFinishedTasks = taskList.IsFinishedTasksStored;
            OnPropertyChanged(nameof(TaskList));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
