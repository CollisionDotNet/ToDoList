using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    public class NewTaskViewModel : INotifyPropertyChanged
    {
        private string? taskName;
        public string? TaskName
        {
            get => taskName;
            set
            {
                taskName = value;
                OnPropertyChanged(nameof(TaskName));
            }
        }
        private string? taskDescription;
        public string? TaskDescription
        {
            get => taskDescription;
            set
            {
                taskDescription = value;
                OnPropertyChanged(nameof(TaskDescription));
            }
        }
        public TaskList TaskList { get; private init; }
        public Action? CloseView { get; set; }

        private BaseCommand? addTaskCommand;
        public BaseCommand AddTaskCommand
        {
            get
            {
                if (addTaskCommand != null)
                    return addTaskCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        Task newTask = new Task(TaskName, TaskDescription);
                        newTask.Owner = TaskList;
                        TaskList.ActiveTasks.Add(newTask);
                        CloseView?.Invoke();
                    };
                    Predicate<object?> canExecute = o => TaskName != null && TaskName.Length >= 3;
                    addTaskCommand = new BaseCommand(execute, canExecute);
                    return addTaskCommand;
                }
            }
        }

        public NewTaskViewModel(TaskList taskList)
        {
            TaskList = taskList;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
