﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TDBCore.BLL;

namespace TDBCore.Types.security
{
  

    public class Security:ISecurity
    {
       
        private string _persmission = "";
        private readonly bool _isSecurityEnabled = true;
        readonly UserDal _users = new UserDal();
        private readonly IUser _user;

        public Security(IUser user, bool isSecurityEnabled = false)
        {
            
            _user = user;

            _isSecurityEnabled = isSecurityEnabled;
        }


        public bool IsValidEdit()
        {

            if (!_isSecurityEnabled)
            {
                Debug.WriteLine("security bypassed for test");
                return true;
            }

          
            var role = _users.GetUserRole(_user.GetUser());

            if (role == 1 || role == 2)
            {
                _persmission = "";
                return true;
            }

            _persmission = "User not in required role group";
            return false;
        }

        public bool IsValidDelete()
        {
            if (!_isSecurityEnabled)
            {
                Debug.WriteLine("security bypassed for test");
                return true;
            }

            var role = _users.GetUserRole(_user.GetUser());

            if (role == 1 || role == 2)
            {
                _persmission = "";
                return true;
            }
            else
            {
                _persmission = "User not in required role group";
                return false;
            }
        }

        public bool IsValidInsert()
        {
            if (!_isSecurityEnabled)
            {

                Debug.WriteLine("security bypassed for test");
                return true;

            }

            int userId = _users.GetUserRole(_user.GetUser());
            if (userId == 1 || userId == 2)
            {
                _persmission = "";
                return true;
            }
            else
            {
                _persmission = "User not in required role group";
                return false;
            }
        }

        public bool IsvalidSelect()
        {
            return true;
        }

        public int UserId()
        {
             
            return 1;
        }


        public string PermissionState()
        {
            return _persmission;
        }
    }
}
