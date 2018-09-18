using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace CompanyAPI.Helper
{
    public class Authentication
    {
        string userName= "Hussain";
        string passWord = "Ruhullah";

        public bool Check( string AuthKey)
        {
            byte[] data = Convert.FromBase64String(AuthKey.Substring(6));
            string[] decodedString = Encoding.UTF8.GetString(data).Split(":");

            if ((userName == decodedString[0]) && (passWord == decodedString[1]))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}
