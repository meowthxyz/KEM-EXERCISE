using KEM_WPF.Models;
using KEM_WPF.Notifications;
using System;
using KEM_WPF.Views;
using KEM_WPF.Models.Entity;
using System.Collections.Generic;

namespace KEM_WPF.ViewModels
{
    public class UsersViewModel : TableModel<UserEntity>
    { 
        public UsersViewModel()
        {
            ItemName = "user";
            TableName = "Users";
        }
        protected override void DeleteItem(object parameter)
        {
            try
            {
                string UserID = SelectedItem.user_name;
                UserManager.RemoveUser(UserID);
                NotificationProvider.Info("User deleted", String.Format("Username: {0}", UserID));
                RefreshList(parameter);
            }
            catch (ArgumentException e)
            {

                switch (e.ParamName)
                {
                    case "CurrentUser":
                        NotificationProvider.Error("Delete User", "Unable to delete current user.");
                        break;

                    default:
                        NotificationProvider.Error("Delete User", "Unable to delete selected user");
                        break;
                }
            }
        }

        protected override void EditItem(object parameter)
        {
            EditUserViewModel EUVM = new EditUserViewModel(SelectedItem.user_name, SelectedItem.first_name, SelectedItem.last_name, SelectedItem.email_address, SelectedItem.user_type);
            EditUserWindow EUV = new EditUserWindow() { DataContext = EUVM };
            if((UserManager._LoggedUser.user_type == "SuperAdmin") || (UserManager._LoggedUser.user_name == SelectedItem.user_name))
            {
                EUVM.EditWindow = EUV;
                EUV.ShowDialog();
            }

            RefreshList(parameter);
        }

        protected override void NewItem(object parameter)
        {
            NewUserWindow NUW = new NewUserWindow();
            NUW.ShowDialog();
            RefreshList(parameter);
        }

        protected override void RefreshList(object parameter)
        {
            var users = UserManager.GetUsers();
            var userEntities = new List<UserEntity>();
            foreach(var u in users)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                userEntities.Add(new UserEntity()
                {
                    email_address = u.email_address,
                    first_name = u.first_name,
                    last_name = u.last_name,
                    user_id = u.user_id,
                    password = "*****", //u.password, //replaced to encrypt password
                    user_name = u.user_name,
                    user_type = u.user_type
                });
#pragma warning restore CS8604 // Possible null reference argument.
            }
              
            List = userEntities;
        }
    }
}
