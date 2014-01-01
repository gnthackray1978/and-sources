using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Facebook;
using System.ServiceModel.Web;

namespace ANDServices
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

         
        public static string MakeReturn(string recordId, string error)
        {
            return "Id=" + recordId + "&Error=" + error;
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


        public static string GetUser()
        {

            string retVal = "x x x ";
            string idVal = "";
            long facebookId = 0;


            try
            {


                string token = WebOperationContext.Current.IncomingRequest.Headers["fb"];


                if (!WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.Host.Contains("local.gnthackray.net"))
                {

                    if (token != null && token.Length > 0)
                    {

                        Facebook.FacebookClient fbc = new FacebookClient(token);

                        var me2 = (IDictionary<string, object>) fbc.Get("/me");


                        if (me2.ContainsKey("name"))
                        {
                            retVal = (string) me2["name"];

                        }

                        if (me2.ContainsKey("id"))
                        {
                            idVal = (string) me2["id"];

                        }


                        retVal = idVal; // userGuid.ToString();
                    }
                    else
                    {
                        throw new Exception("Token Not Received");
                    }
                }
                else
                {
                    retVal = "localaccess";
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