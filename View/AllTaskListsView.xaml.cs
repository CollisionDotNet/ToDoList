using System.Windows;
using ToDoList.ViewModel;

namespace ToDoList.View
{
    public partial class AllTaskListsView : Window
    {
        public AllTaskListsView()
        {
            InitializeComponent();
            AllTaskListsViewModel viewModel = new AllTaskListsViewModel();
            Closed += (o, e) => viewModel.SaveDataCommand.Execute("active.json");
            DataContext = viewModel;
        }
    }
}
