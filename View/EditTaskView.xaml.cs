using System.Windows;
using ToDoList.Model;
using ToDoList.ViewModel;

namespace ToDoList.View
{
    public partial class EditTaskView : Window
    {
        public EditTaskView(Task task)
        {
            InitializeComponent();
            EditTaskViewModel viewModel = new EditTaskViewModel(task);
            viewModel.CloseView = () => { Close(); };
            DataContext = viewModel;
        }
    }
}
