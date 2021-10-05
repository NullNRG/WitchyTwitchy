using System;
using System.Collections.Generic;
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
        Owner
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
        static Dictionary<string, User> _users= new Dictionary<string, User>(); 
        public UserPermissions Permission { get; set; }

        public string UserName {  get; set; }
        public int Points { get; set; }
        public int MessageCount { get; set; }
        public ConsoleColor UserColor { get; set; }
        bool UserNameColorSet = false;
        bool NewUser = true;

        User()
        {
            if (!_users.ContainsKey(this.UserName)) // if the username doesnt exist in the dictionary, then set them up as a new user
            {
                if (this.NewUser == true)
                {

                    if (Permission <= UserPermissions.Standard) //TODO: Check a whitelist for users with defined permissions
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
                //TODO: Also add checks for if a user permissions  had been changed, and update the user permissions and color to reflaect that
            }


        }

        private void SetPermissions(User user, UserPermissions permission) 
        {
            user.Permission = permission; //TODO: Change this behavior to check the specified permission for validation

        }
        private void SetUserNameColor(UserPermissions permission)
        {
            switch (permission)
            {
                case UserPermissions.Standard:
                    this.UserColor = ConsoleColor.Gray;
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
                default:
                    break;
            }
        }


    }
}
