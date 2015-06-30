using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace TDBCore.BLL
{
    public class UserRole
    {
        public string FacebookUserId { get; set; }
        public int roleId { get; set; }
    }

    public class UserDal : BaseBll, IUserDal
    {


        public int GetUserRole(string user)
        {
            List<UserRole> userList = new List<UserRole>();
            int returnVal=0;

            string sql = @"Select FacebookUserId, RoleId FROM dbo.UserRoles";
        
            SqlConnection myConnection = this.GetConnection();
            SqlCommand myCommand = new SqlCommand(sql, myConnection);
         
            if(myConnection.State != ConnectionState.Open)
                myConnection.Open();

            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
            while (myReader.Read())
            {
                

                userList.Add(new UserRole() { FacebookUserId = myReader.GetString(0), roleId = myReader.GetInt32(1) });

            }



            myReader.Close();
            myConnection.Close();

            var role = userList.FirstOrDefault(o=>o.FacebookUserId.Trim() == user.Trim());

            if (role != null)
                returnVal = role.roleId;

        

            return returnVal;

        }

    }
}
