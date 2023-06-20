using System.Collections.ObjectModel;
using System.Windows;
using ToDoList.Model;
using ToDoList.ViewModel;

namespace ToDoList.View
{
    public partial class NewTaskView : Window
    {
        public NewTaskView(TaskList taskList)
        {
            InitializeComponent();
            NewTaskViewModel viewModel = new NewTaskViewModel(taskList);
            viewModel.CloseView = () => { Close(); };
            DataContext = viewModel;
        }
    }
}
