using System.Collections.ObjectModel;
using System.Windows;
using ToDoList.Model;
using ToDoList.ViewModel;

namespace ToDoList.View
{
    public partial class NewTaskListView : Window
    {
        public NewTaskListView(ObservableCollection<TaskList> taskLists)
        {
            InitializeComponent();
            NewTaskListViewModel viewModel = new NewTaskListViewModel(taskLists);
            viewModel.CloseView = () => { Close(); };
            DataContext = viewModel;
        }
    }
}
