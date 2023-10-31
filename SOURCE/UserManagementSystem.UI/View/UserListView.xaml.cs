using System.Windows;
using System.Windows.Controls;
using UserManagementSystem.UI.ViewModel;

namespace UserManagementSystem.UI.View
{
    /// <summary>
    /// UserListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserListView : UserControl
    {
        public UserListView()
        {
            InitializeComponent();
            
            DataContext = new UserListViewModel();
        }
    }
}
