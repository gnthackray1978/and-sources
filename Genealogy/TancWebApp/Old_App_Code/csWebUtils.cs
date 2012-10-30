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

namespace TancWebApp
{
    public class CsWebUtils
    {
        public CsWebUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void ShowError(string title, string message, Page _page, Type _type)
        {


            string _message = title + "<br />" + message + "<br />";


            ShowStatus(_message, _page, _type);

        }

        public static void ShowStatus(string message, Page _page, Type _type)
        {
            ScriptManager sm = ScriptManager.GetCurrent(_page);

            if (sm.IsInAsyncPostBack)
            {
                string script = @"
                $(document).ready(function() {
                $.jGrowl.defaults.position = 'center';
                $.jGrowl.defaults.theme = 'iphone';
                $.jGrowl('" + message + "', {theme: 'iphone',header: 'Notification',life: 8000});});";

                ScriptManager.RegisterStartupScript(_page, _type, "notification", script, true);

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


             //   long.TryParse(idVal, out facebookId);

               // Guid userGuid = facebookId.ToGuid();

                retVal = idVal;// userGuid.ToString();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            return retVal;
        }

    

    }



 

}