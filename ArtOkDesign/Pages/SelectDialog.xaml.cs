using ArtOkDesign.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ArtOkDesign.Pages
{
    /// <summary>
    /// Логика взаимодействия для SelectDialog.xaml
    /// </summary>
    public partial class SelectDialog : Page
    {
        List<Messages> GlobMessages= new List<Messages>();
        int CurrentDialogID= 0;
        public SelectDialog()
        {
            InitializeComponent();
            GetDialogs();
        }
        public async void GetDialogs()
        {
            Dialog[] dialogs = await ApiController.GetUserDialogs(GlobalInformation.currentUser.ID);
            foreach (Dialog dialog in dialogs)
            {
                //GetMessages(dialog.ID);
                dialog.DialogMessages = GlobMessages;         
            }

            lvDialogs.ItemsSource= dialogs; 
        }
        public async void GetDialogs(int IDDialog)
        {
            Dialog[] dialogs = await ApiController.GetUserDialogs(GlobalInformation.currentUser.ID);
            foreach (Dialog dialog in dialogs)
            {
                //GetMessages(dialog.ID);
                if (dialog.ID == IDDialog)
                {
                    dialog.DialogMessages = GlobMessages;
                }
                else
                {
                    dialog.DialogMessages = null;
                }
            }
            
            lvDialogs.ItemsSource = dialogs;
        }
        public async void GetMessages(int IDDialog)
        {
            DialogUser[] dialogUsers = await ApiController.GetAllUsersInDialog(IDDialog);
            List<Messages> messages = new List<Messages>();
            foreach (DialogUser user in dialogUsers)
            {
                Messages[] messages1 = await ApiController.GetAllMessageInDialog(user.ID);
                
                foreach (Messages message in messages1)
                {
                    User user1 = await ApiController.GetUserAsync($"https://localhost:2222/api/User/{user.IDUser}");
                    message.MessPP = await ApiController.GetProfilePicture(user.IDUser);
                    message.MessNickName = user1.NickName;
                    message.gridWidth = "600";
                    if (user.IDUser == GlobalInformation.currentUser.ID)
                    {

                        message.MessPPVisibility = "Collapsed";
                        message.GridHorizont = "Right";
                        message.GridBack = "LightYellow";
                    }
                    else
                    {
                        message.MessPPVisibility = "Visible";
                        message.GridHorizont = "Left";
                        message.GridBack = "LightGray";
                    }
                    messages.Add(message);
                }
            }
            messages = messages.OrderBy(message => message.TimeOfSend).OfType<Messages>().ToList();
            GlobMessages = messages;
            txtSend.Visibility = Visibility.Visible;
           GetDialogs(IDDialog);
        }

        private void lvDialogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvDialogs.SelectedItem is Dialog)
            {
                CurrentDialogID = (lvDialogs.SelectedItem as Dialog).ID;
                GetMessages(CurrentDialogID);

            }
        }

        private async void txtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogUser[] dialogUsers = await ApiController.GetAllUsersInDialog(CurrentDialogID);
                foreach (DialogUser user in dialogUsers)
                {
                    if (user.IDUser == GlobalInformation.currentUser.ID)
                    {
                        Messages newMessages = new Messages();
                        newMessages.IDUserDialog = user.ID;
                        newMessages.Message = txtSend.Text;
                        newMessages.TimeOfSend = DateTime.Now;
                        await ApiController.PushMessage(newMessages);
                        break;
                    }
                }             
                GetMessages(CurrentDialogID);
                txtSend.Text = "";
            }
        }
    }
}
