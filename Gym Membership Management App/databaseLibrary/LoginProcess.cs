using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace databaseLibrary
{
    public class LoginProcess
    {
        public Boolean blnValidateUserInput(string strUserID, string strPassword)
        {
            // Validate that the user has input both a username and password
            if (strUserID == "" || strPassword == "")
            {
                return false;
            }

            // Validate the len of the username
            if (strUserID.Length < 3 || strUserID.Length > 15)
            {
                return false;
            }

            // Validate the len of the password
            if (strPassword.Length < 7 || strPassword.Length > 20)
            {
                return false;
            }


            return true;
        }
    }
}
