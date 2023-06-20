using System.Windows;
using ToDoList.Model;
using ToDoList.ViewModel;

namespace ToDoList.View
{
    /// <summary>
    /// Interaction logic for EditTaskListView.xaml
    /// </summary>
    public partial class EditTaskListView : Window
    {
        public EditTaskListView(TaskList taskList)
        {
            InitializeComponent();
            EditTaskListViewModel viewModel = new EditTaskListViewModel(taskList);
            viewModel.CloseView = () => { Close(); };
            DataContext = viewModel;
        }
    }
}
