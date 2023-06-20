using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    public class NewTaskListViewModel : INotifyPropertyChanged
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
        public ObservableCollection<TaskList> TaskLists { get; private init; }
        public Action? CloseView { get; set; }

        private BaseCommand? addTaskListCommand;
        public BaseCommand AddTaskListCommand
        {
            get
            {
                if (addTaskListCommand != null)
                    return addTaskListCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        TaskList newTaskList = new TaskList(TaskListName, true, TaskListStoreFinishedTasks);
                        TaskLists.Add(newTaskList);
                        CloseView?.Invoke();
                    };
                    Predicate<object?> canExecute = o => TaskListName != null && TaskListName.Length >= 3;
                    addTaskListCommand = new BaseCommand(execute, canExecute);
                    return addTaskListCommand;
                }
            }
        }

        public NewTaskListViewModel(ObservableCollection<TaskList> taskLists)
        {
            TaskLists = taskLists;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
