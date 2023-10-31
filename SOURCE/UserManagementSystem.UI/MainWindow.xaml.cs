using Serilog;
using System;
using System.Windows;
using UserManagementSystem.Database;
using UserManagementSystem.UI.Method;
using UserManagementSystem.UI.View;

namespace UserManagementSystem.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ILogger Logger { get; set; } = new SerilogWrapper().GetLogger("UI.log");

        public MainWindow()
        {
            Logger.Information("Start UserManagementSystem");

            //데이터베이스 연결 여부 체크
            databaseConnectCheck();

            InitializeComponent();
            MainBorder.Child = new UserListView();
        }

        //데이터베이스 연결 여부를 체크하기 위해 만든 메소드
        private void databaseConnectCheck()
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    Logger.Information("Database Connect");
                };

            }
            catch (Exception ex)
            {
                Logger.Error("Database Connect Error");
                Logger.Error(ex.ToString());
            }
        }
    }
}
