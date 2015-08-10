using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RasPi_Controller.Helpers
{
    public static class SecureStringHelper
    {
        public static string GetString(SecureString securePassword)
        {
            string insecurePassword = string.Empty;

            try
            {
                insecurePassword = Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(securePassword));
            }
            catch (Exception)
            {
                insecurePassword = string.Empty;
            }

            return insecurePassword;
        }
    }
}
