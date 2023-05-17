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
                MessageBox.Show("Неверные данные для входа");
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (btnSwitch.Content.ToString() == "Зарегистрироваться")
            {
                btnSwitch.Content = "Авторизоваться";             
                txtLogin.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Collapsed;
                RegLogin.Visibility = Visibility.Visible;
                RegMail.Visibility = Visibility.Visible;
                RegPassword.Visibility = Visibility.Visible;
                RegRepeatPassword.Visibility = Visibility.Visible;
                DPBirthday.Visibility = Visibility.Visible;
                btnConfimRegistration.Visibility = Visibility.Visible;
                btnLogin.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSwitch.Content = "Зарегистрироваться";
                txtLogin.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Visible;
                RegLogin.Visibility = Visibility.Collapsed;
                RegMail.Visibility = Visibility.Collapsed;
                RegPassword.Visibility = Visibility.Collapsed;
                RegRepeatPassword.Visibility = Visibility.Collapsed;
                DPBirthday.Visibility = Visibility.Collapsed;
                btnConfimRegistration.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Visible;

            }
        }

        public bool CheckRegistration()
        {
            if (String.IsNullOrWhiteSpace(RegLogin.Text))
            {
                RegLogin.Foreground = Brushes.Red;
                return false;
            }
            if (String.IsNullOrWhiteSpace(RegMail.Text))
            {
                RegMail.Foreground = Brushes.Red;
                return false;
            }
            if (String.IsNullOrWhiteSpace(RegPassword.Password))
            {
                RegPassword.Foreground = Brushes.Red;
                return false;
            }
            if (String.IsNullOrWhiteSpace(RegRepeatPassword.Password))
            {
                RegRepeatPassword.Foreground = Brushes.Red;
                return false;
            }
            if (String.IsNullOrWhiteSpace(DPBirthday.Text))
            {
                DPBirthday.Foreground = Brushes.Red;
                return false;
            }
            return true;
        }
        private async void btnConfimRegistration_Click(object sender, RoutedEventArgs e)
        {
            bool res = CheckRegistration();
            if (res ==true)
            {
                User newUser = new User();
                newUser.NickName = RegLogin.Text;
                newUser.Email = RegMail.Text;
                newUser.Password = RegPassword.Password;
                newUser.Birthday = DPBirthday.DisplayDate;
                User loginUser = await ApiController.CheckUser(newUser);
                if (loginUser == null)
                {                
                    await ApiController.AddUser(newUser);
                    GlobalInformation.currentUser = newUser;
                    MainFrame mainFrame = new MainFrame();
                    mainFrame.Show();
                    this.Close();
                }

                else
                    MessageBox.Show("Неверные данные для входа");
            }
        }

        private void RegMail_GotFocus(object sender, RoutedEventArgs e)
        {
            if(sender is TextBlock)
            {
                (sender as TextBox).Foreground = Brushes.Green;
            }
            else if (sender is PasswordBox)
            {
                (sender as PasswordBox).Foreground = Brushes.Green;
            }
            else
            {
                (sender as DatePicker).Foreground = Brushes.Green;
            }
        }
    }
}
