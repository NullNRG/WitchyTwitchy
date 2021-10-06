using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WitchyTwitchy
{
    enum UserPermissions
    {
        Standard,
        VIP,
        Mod,
        SuperMod,
        Admin,
        Owner,
        Custom
    }
    enum UserColor
    {
        Standard,
        VIP,
        Mod,
        SuperMod,
        Admin,
        Owner,
        Custom
    }
    internal class User
    {
        public static Dictionary<string, User> _users= new Dictionary<string, User>(); 
        public UserPermissions Permission { get; set; }

        public string UserName {  get; set; }
        public int Points { get; set; }
        public int MessageCount { get; set; }
        public ConsoleColor UserColor { get; set; }
        
        bool UserNameColorSet = false;
        bool NewUser = true;

        public User()
        {

        }
        public User(string userName, UserPermissions permission = UserPermissions.Standard)
        {
            if (!_users.ContainsKey(userName)) // if the username doesnt exist in the dictionary, then set them up as a new user
            {
                this.UserName = userName;
                if (this.NewUser == true)
                {
                    //Console.WriteLine($"New user found: {userName}");

                    if (permission <= UserPermissions.Standard) //TODO: Check a whitelist for users with defined permissions
                    {
                        try
                        {
                            SetPermissions(this, UserPermissions.Standard); // set the standard user defaults
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Cant set permissions for " + this.UserName);
                        }
                    }

                    if (this.UserNameColorSet == false)
                    {
                        try
                        {
                            SetUserNameColor(this.Permission); //TODO: Check a whitelist for users with defined permissions
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Cant set usename Color for " + this.UserName);
                        }
                    }
                }
                
                UserNameColorSet = true; 

                this.NewUser = false;

                _users.Add(userName, this);
                SaveUser();
                //TODO: Also add checks for if a user permissions  had been changed, and update the user permissions and color to reflaect that
            }


        }

        private void SetPermissions(User user, UserPermissions permission) 
        {
            user.Permission = permission; //TODO: Change this behavior to check the specified permission for validation

        }
        private void SetUserNameColor(UserPermissions permission)
        {
            this.Permission = permission;
            foreach (var user in File.ReadAllLines(Environment.CurrentDirectory + @"\Config\topstreamers.txt"))
            {
                var userName = user.ToLower();

                if (this.UserName == userName)
                {
                    SetPermissions(this,UserPermissions.Custom);
                }

            }
            switch (this.Permission)
            {
                case UserPermissions.Standard:
                    this.UserColor = ConsoleColor.Red;
                    break;
                case UserPermissions.VIP:
                    this.UserColor = ConsoleColor.Magenta;
                    break;
                case UserPermissions.Mod:
                    this.UserColor = ConsoleColor.Blue;
                    break;
                case UserPermissions.SuperMod:
                    this.UserColor = ConsoleColor.Cyan;
                    break;
                case UserPermissions.Admin:
                    this.UserColor = ConsoleColor.Yellow;
                    break;
                case UserPermissions.Owner:
                    this.UserColor = ConsoleColor.Green;
                    break;
                case UserPermissions.Custom:
                    this.UserColor = ConsoleColor.Blue;
                    break;
                default:
                    break;
            }
        }

        private void SaveUser()
        {

        }


    }
}
