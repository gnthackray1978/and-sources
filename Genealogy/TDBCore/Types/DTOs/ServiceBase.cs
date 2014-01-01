namespace TDBCore.Types.DTOs
{
    public class ServiceBase
    {
        
       
        private int _userId = 1 ;
        private string _errorStatus = "";

        public string ErrorStatus 
        {
            get
            {
                return _errorStatus;
            }

            set { _errorStatus = value; }
        }

        public int UserId
        {
            get
            {
                return _userId;
            }

            set { _userId = value; }
        }
    }
}