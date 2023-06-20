using System;
using System.ComponentModel;
using System.Windows;
using ToDoList.Model;

namespace ToDoList.ViewModel
{
    public class EditTaskViewModel : INotifyPropertyChanged
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
        public Task Task { get; private init; }
        public Action? CloseView { get; set; } 

        private BaseCommand? editTaskCommand;
        public BaseCommand EditTaskCommand
        {
            get
            {
                if (editTaskCommand != null)
                    return editTaskCommand;
                else
                {
                    Action<object?> execute = o =>
                    {
                        Task.Name = TaskName;
                        Task.Description = TaskDescription;
                        CloseView?.Invoke();
                    };
                    Predicate<object?> canExecute = o => TaskName != null && TaskName.Length >= 3;
                    editTaskCommand = new BaseCommand(execute, canExecute);
                    return editTaskCommand;
                }
            }
        }

        public EditTaskViewModel(Task task)
        {
            Task = task;
            TaskName = task.Name;
            TaskDescription = task.Description;
            OnPropertyChanged(nameof(Task));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
