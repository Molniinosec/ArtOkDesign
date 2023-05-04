using ArtOkDesign.Classes;
using ArtOkDesign.Windowses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArtOkDesign
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            ApiController.RunAsync().GetAwaiter().GetResult();
        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            User user = new User();
            user.NickName = txtLogin.Text;
            user.Password = txtPassword.Password;
            User loginUser = await ApiController.CheckUser(user);
            if (loginUser != null)
            {
                GlobalInformation.currentUser = loginUser;
                MainFrame mainFrame = new MainFrame();
                mainFrame.Show();
                this.Close();
            }

            else
                MessageBox.Show("false");
        }
    }
}
