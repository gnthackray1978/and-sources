using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Facebook;
using TancWebApp.Helpers;

namespace GenOnline.Helpers
{
    /// <summary>
    /// Summary description for WebExtensions
    /// </summary>
    public static class WebExtensions
    {

        public static Guid ToGuid(this long value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static string ToCSVString<T>(this List<T> list)
        {
            return string.Join(",", list.ConvertAll<string>(delegate(T i) { return i.ToString(); }).ToArray());
        }







    }

    public class WebHelper
    {
        public static string GetRequestIp()
        {
            string requestAddress = "";

            try
            {
                OperationContext context = OperationContext.Current;
                MessageProperties messageProperties = context.IncomingMessageProperties;
                var endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                if (endpointProperty != null) requestAddress = endpointProperty.Address + ":" + endpointProperty.Port;
            }
            catch (Exception e)
            {
                ///todo tidy this up
                requestAddress = e.Message;
            }
           
            return requestAddress;
        }

        public static string MakeReturn(string recordId, string error)
        {
            return "Id=" + recordId + "&Error=" + error;
        }

        public static string MakeJSONReturn(string recordId, bool success) 
        { 
            return "{ \"SUCCESS\": "+ success.ToString() + ", \"ID\": " + recordId + " }";
        }

        public static void WriteParams(params string[] parameters)
        {

            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();


            List<string> names = method.GetParameters().Select(s => s.Name).ToList();

            int idx = 0;

            foreach (var p in parameters)
            {

                Debug.WriteLine(names[idx] + " " + p);
                idx++;
            }


        }


        private static IDictionary<string, object> GetFbDic()
        {
            var f = new FacebookHelper("205401136237103", "e2bae4f7b2ffa301366c119107df79b1");

            var token = f.AccessToken;

            if (string.IsNullOrEmpty(token)) return null;

            var fbc = new FacebookClient(token);

            var me2 = (IDictionary<string, object>)fbc.Get("/me");

            return me2;
        }

        public static string GetUser()
        {

            string retVal = "x x x ";


            try
            {
           
                var fbdic = GetFbDic();

                if (fbdic == null) return retVal;

                if (fbdic.ContainsKey("id"))
                {
                    retVal = (string)fbdic["id"];

                }

            }
            catch (Exception ex1)
            {
                throw ex1;
               
            }

            return retVal;
        }


        public static string GetUserName()
        {

            string retVal = "x x x ";

            try
            {

                var fbdic = GetFbDic();

                if (fbdic == null) return retVal;

                if (fbdic.ContainsKey("name"))
                {
                    retVal = (string)fbdic["name"];

                }
            }
            catch (Exception ex1)
            {
                throw ex1;

            }

            return retVal;
        }
    }


}