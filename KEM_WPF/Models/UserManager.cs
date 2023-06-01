using KEM_WPF.Data;
using KEM_WPF.Notifications;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;

namespace KEM_WPF.Models
{
    public class UserManager
    {
        public static ObservableCollection<User> _DatabaseUsers = new ObservableCollection<User>();
        public static User _LoggedUser = new User();
        public static List<User> GetUsers()
        {
            var users = new List<User>();
            using (var context = new KEMDbContext())
            {
                users = context.Users.ToList();
            }   
            return users;
        }

        public static bool AuthenticateUser(string username, string password)
        {
            var validUser = false;

            using (var context = new KEMDbContext())
            {
                var user = context.Users.FirstOrDefault(s => s.user_name == username && s.password == password);
                if(user != null)
                {
#pragma warning disable CS8601 // Possible null reference assignment.
                    _LoggedUser = user;
#pragma warning restore CS8601 // Possible null reference assignment.
                    validUser = true;
                }
            }

            return validUser;
        }

        public static void ClearLoggedUser()
        {
            _LoggedUser = new User();
        }

        public static bool AddUser(string userName, string password, string firstName, string lastName, string emailAddress, string userType)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new System.ArgumentException("Wrong username.", "userID");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                throw new System.ArgumentException("Wrong password.", "userID");
            }

            try
            {
                using (var context = new KEMDbContext())
                {
                    var checkUser = context.Users.FirstOrDefault(f => f.user_name == userName);
                    if(checkUser != null)
                    {
                        //throw new System.ArgumentException("Username already exists.", "recordExist");
                        return false;
                    }
                    var user = new User();
                    user.user_name = userName;
                    user.password = password;
                    user.first_name = firstName;
                    user.last_name = lastName;
                    user.email_address = emailAddress;
                    user.user_type = userType;
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsEmptyUserDatabase()
        {
            var noUsers = false;
            using (var context = new KEMDbContext())
            {
                var users = context.Users.ToList();
                noUsers = users.Count() == 0 ? true : false;
            }
            return noUsers;
        }

        public static bool ModifyUser(string oldUserName, string oldPassword, string userName, string password, string confirm, string firstName, string lastName, string emailAddress, string userType)
        {
            //if (!string.IsNullOrWhiteSpace(oldPassword) && string.IsNullOrWhiteSpace(password))
            //{
            //    throw new System.ArgumentException("Wrong password.", "password");

            //}
            //else
            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirm) && (password != confirm))
            { 
                throw new System.ArgumentException("Wrong confirm password.", "confirm");
            }

            try
            {
                using (var context = new KEMDbContext())
                {
                    var user = context.Users.FirstOrDefault(f => f.user_name == userName);
                    // && f.password == oldPassword);

                    //if(!string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(password)
                    //        && !string.IsNullOrWhiteSpace(confirm) )
                    //{
                    //    user = context.Users.FirstOrDefault(f => f.user_name == userName && f.password == oldPassword);

                    //    if(user == null)
                    //    {
                    //        throw new System.ArgumentException("Wrong password.", "oldPassword");
                    //    }
                    //}

                    if (user == null)
                    {
                        throw new System.ArgumentException("No user record.", "noRecord");
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(password) 
                            && !string.IsNullOrWhiteSpace(confirm) 
                            && (password == confirm))
                        {
                            user.password = confirm;
                        }

                        //user.user_name = userName;

                        user.first_name = firstName;
                        user.last_name = lastName;
                        user.email_address = emailAddress;
                        user.user_type = userType;
                        context.Users.Update(user);
                        context.SaveChanges();
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public static void RemoveUser(string userName)
        {
            if(_LoggedUser.user_name != userName)
            {
                try
                {
                    using(var context = new KEMDbContext())
                    {
                        var user = context.Users.FirstOrDefault(f => f.user_name == userName);
                        if(user != null)
                        {
                            //before deleting make sure to delete all related data.
                            var removeItems = context.Sites.Where(f => f.user_id == user.user_id);
                            if (removeItems != null)
                            {
                                foreach(var removeItem in removeItems)
                                {
                                    var removeReItems = context.RegisteredEquipments.Where(f => f.site_id == removeItem.site_id);
                                    foreach (var reItem in removeReItems)
                                    {
                                        context.RegisteredEquipments.Remove(reItem);
                                        context.SaveChanges();
                                    }
                                    context.Sites.Remove(removeItem);
                                    context.SaveChanges();
                                }
                            }

                            //delete equipments
                            var eq = EquipmentManager.GetList(user.user_id);
                            if(eq != null)
                            {
                                foreach(var e in eq)
                                {
                                    EquipmentManager.DeleteEquipment(e);
                                }
                            }
                            //delete user
                            context.Users.Remove(user);
                            context.SaveChanges();
                        }
                    }
                }
                catch (System.Exception)
                {
                    throw new System.ArgumentException("Remove User.", "DatabaseError.");
                }
            }
            else
            {
                throw new System.ArgumentException("Remove User.", "CurrentUser.");
            }
        }
    }
}
