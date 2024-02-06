using System;
using System.ComponentModel;

namespace ToDoList.Model
{
    public class Task : INotifyPropertyChanged
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
        private string? description;
        public string? Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }       
        public Task(string name, string? description)
        {
            this.name = name;
            this.description = description;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
