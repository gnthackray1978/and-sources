using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Security;
using Facebook;
using GenOnline.Helpers;
using GenOnline.Services.Contracts;
using TancWebApp.Helpers;
using TDBCore.BLL;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;
using System.Net;

namespace GenOnline.Services
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

        public string DoNothing() 
        {
          //  Console.WriteLine("test");
           // WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
          //  WebOperationContext.Current.OutgoingResponse.StatusDescription = "DoNothing failed";

            return WebHelper.MakeJSONReturn(Guid.Empty.ToString(), true);
        }


        // misc

        public string GetLoggedInUser()
        {
            string user;

            try
            {
                user = WebHelper.GetUserName();
            }
            catch (Exception e)
            {
                user = e.Message;
                 
            }
       
            return user;
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






        public string GetLoggedInUserName()
        {
            string user;

            try
            {
                user = WebHelper.GetUserName();
            }
            catch (Exception e)
            {
                user = e.Message;

            }

            return user;
        }

        public string GetLoggedInUserId()
        {
            string user;

            try
            {
                user = WebHelper.GetUser();
            }
            catch (Exception e)
            {
                user = e.Message;

            }

            return user;
        }
    }
}







