﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ToDoList.Model
{
    public class TaskList : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private bool isFinishedTasksStored;
        public bool IsFinishedTasksStored
        {
            get => isFinishedTasksStored;
            set
            {
                isFinishedTasksStored = value;
                OnPropertyChanged(nameof(IsFinishedTasksStored));
            }
        }
        public ObservableCollection<Task>? ActiveTasks { get; private init; }
        private ObservableCollection<Task>? finishedTasks;
        public ObservableCollection<Task>? FinishedTasks
        {
            get => finishedTasks;
            set
            {
                finishedTasks = value;
                OnPropertyChanged(nameof(FinishedTasks));
            }
        }
        public TaskList(string name, bool isActiveTasksStored, bool isFinishedTasksStored)
        {
            this.name = name;
            this.isFinishedTasksStored = isFinishedTasksStored;
            if(isActiveTasksStored)
                ActiveTasks = new ObservableCollection<Task>();
            else
                ActiveTasks = null;
            if (isFinishedTasksStored)
                FinishedTasks = new ObservableCollection<Task>();
            else
                FinishedTasks = null;
            if (ActiveTasks != null)
            {
                foreach (Task task in ActiveTasks)
                {
                    task.Owner = this;
                }
            }
            if (FinishedTasks != null)
            {
                foreach (Task task in FinishedTasks)
                {
                    task.Owner = this;
                }
            }
        }
        [JsonConstructor]
        public TaskList(string name, bool isFinishedTasksStored, ObservableCollection<Task>? activeTasks, ObservableCollection<Task>? finishedTasks)
        {
            this.name = name;
            this.isFinishedTasksStored = isFinishedTasksStored;
            ActiveTasks = activeTasks;
            if (isFinishedTasksStored)
                this.finishedTasks = finishedTasks;
            else
                this.finishedTasks = null;
            if (ActiveTasks != null)
            {
                foreach (Task task in ActiveTasks)
                {
                    task.Owner = this;
                }
            }
            if (FinishedTasks != null)
            {
                foreach (Task task in FinishedTasks)
                {
                    task.Owner = this;
                }
            }
        }      
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
