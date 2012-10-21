using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;

using System.Web.UI;

using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;

using Facebook;
using System.Net;
using System.IO;
using System.ServiceModel.Web;
using TDBCore.EntityModel;
using TDBCore.Types;
using GedItter.BLL;


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
        public static string GetUser()
        {

            string retVal = "x x x ";
            string idVal = "";
            long facebookId = 0;


            try
            {


                string token = WebOperationContext.Current.IncomingRequest.Headers["fb"];

                if (token != null)
                {

                    Facebook.FacebookClient fbc = new FacebookClient(token);

                    var me2 = (IDictionary<string, object>)fbc.Get("/me");


                    if (me2.ContainsKey("name"))
                    {
                        retVal = (string)me2["name"];

                    }

                    if (me2.ContainsKey("id"))
                    {
                        idVal = (string)me2["id"];

                    }


                    retVal = idVal;// userGuid.ToString();
                }
                else
                {
                    retVal = "invalid token";
                }
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            return retVal;
        }

    }


}