﻿using System;
using TDBCore.EntityModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace TDBCore.BLL
{
    public class BaseBll
    {
      
        protected SqlConnection GetConnection()
        {
            var sqlConnection = new SqlConnection();

            try
            {

                //sqlConnection.ConnectionString = Properties.Settings.Default.ThackrayDBConnectionString;
                sqlConnection.ConnectionString = Properties.Settings.Default.gendbnet;

                sqlConnection.Open();

                // You can get the server version 
                // SQLConnection.ServerVersion
            }
            catch (Exception Ex)
            {
                // Try to close the connection
                sqlConnection.Dispose();              
            }
            return sqlConnection;
        }
    }
}
