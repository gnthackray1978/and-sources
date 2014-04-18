﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
 
using TDBCore.BLL;
using System.Web.Security;
using System.ServiceModel.Web;
using System.Diagnostics;
using System.Reflection;
using Facebook;
using TDBCore.Types.DTOs;
using TDBCore.Types.domain;
using TDBCore.Types.libs;
using PersonService = PersonService.PersonService;


namespace ANDServices
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class APIServices : IAnd
    {
        private readonly IFilesDal _filesDal;

        public APIServices()
        {
            //
            // TODO: Add constructor logic here
            //
            _filesDal = new FilesDal();
        }


        public string TestLogin(string testParam)
        {
            string retVal = "could not get login ";
            string token = WebOperationContext.Current.IncomingRequest.Headers["fb"];

            if (token != null && token.Length != 0)
            {

                Facebook.FacebookClient fbc = new FacebookClient(token);

                var me2 = (IDictionary<string, object>)fbc.Get("/me");


                if (me2.ContainsKey("name"))
                {
                    retVal = (string)me2["name"];

                }

                Debug.WriteLine("user id" + WebHelper.GetUser());
            }


            return retVal;
        }


        public void Upload(string fileName, Stream stream)
        {

            //MultipartParser parser = new MultipartParser(stream);
            //if (parser.Success)
            //{
            //    // Save the file
            //   // SaveFile(parser.Filename, parser.ContentType, parser.FileContents);

            //   System.IO.File.WriteAllBytes("C:\\Temp\\" + fileName, parser.FileContents);
            //}

            //FileStream fileToupload = new FileStream("C:\\Temp\\" + fileName, FileMode.Create);

            //byte[] bytearray = new byte[10000];
            //int bytesRead, totalBytesRead = 0;
            //do
            //{
            //    bytesRead = stream.Read(bytearray, 0, bytearray.Length);
            //    totalBytesRead += bytesRead;
            //} while (bytesRead > 0);

            //fileToupload.Write(bytearray, 0, bytearray.Length);
            //fileToupload.Close();
            //fileToupload.Dispose();

        }


      


        // events

        public ServiceEventObject GetEvents(string chkIncludeBirths, string chkIncludeDeaths, string chkIncludeWitnesses, string chkIncludeParents, string chkIncludeMarriages, string chkIncludeSpouses, string chkIncludePersonWithSpouses,
            string christianName, string surname, string lowerDateRange, string upperDateRange, string location,
            string page_number, string page_size, string sort_col)
        {


            return new ServiceEventObject();
        }




        // misc

        public string GetLoggedInUser()
        {

            MembershipUser muser = null;
            string returnValue = "";

            try
            {
                muser = Membership.GetUser();
                returnValue = muser.UserName;

            }
            catch (Exception ex1)
            {
                returnValue = "Guest,Error: " + ex1.Message;
            }


            return returnValue;
        }

       

       



   

        public ServiceFileObject GetFilesForSource(string sourceId, string page_number, string page_size)
        {
            ServiceFileObject sfo = new ServiceFileObject();

            var filesMappedDataTable = _filesDal.GetFilesByParentId2(sourceId.ToGuid());



            sfo.serviceFiles = sfo.serviceFiles.Select(f => new ServiceFile()
            {
                FileDescription = f.FileDescription,
                FileId = f.FileId,
                FileLocation = f.FileLocation,
                FileThumbLocation = f.FileThumbLocation
            }).ToList();


            sfo.Batch = page_number.ToInt32();
            sfo.BatchLength = page_size.ToInt32();
            sfo.Total = sfo.serviceFiles.Count;

            sfo.serviceFiles = sfo.serviceFiles.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();


            return sfo;
        }




       
    }





}




