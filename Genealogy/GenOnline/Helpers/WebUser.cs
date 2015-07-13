using GenOnline.Helpers;
using TDBCore.Types.security;

namespace GenOnline
{
    public class WebUser : IUser
    {
        public string GetUser()
        {
            return WebHelper.GetUser();
        }
    }
}