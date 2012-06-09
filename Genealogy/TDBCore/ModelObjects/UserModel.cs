using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;



namespace TDBCore.ModelObjects
{
    public class UserModel
    {




        public void Login()
        { 
        
        }

        public void LogOut()
        { 
        
        }

        public void CreateUser()
        { 
        
        }

        public void EditUser()
        { 
        
        }

        public void DeleteUser()
        { 
        
        }




        public void AddRole()
        { 
        
        }

        public void EditRole()
        { 
        
        }

        public void DeleteRole()
        { 
        
        }





        public void AddUserToRole(string user, string role)
        {
            Roles.AddUserToRole(user, role);
        }

        public void RemoveUserFromRole(string user, string role)
        {
            if (Roles.IsUserInRole(role))
            {
                Roles.RemoveUserFromRole(user, role);
            }
        }




        public MembershipUserCollection GetUserList()
        {
            MembershipUserCollection muc = Membership.GetAllUsers();

            return muc;
        }

        public List<string> GetRoleList()
        {
            return new List<string>(Roles.GetAllRoles());
        }


        public List<string> GetUserRole(string user)
        {
            return new List<string>(Roles.GetRolesForUser(user));
        }

        

    }
}
