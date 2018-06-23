using System;
using System.Linq;
using System.Text;

namespace Asopalav.Helpers
{
    public static class SecurityHelper
    {
        public static string Base64Encode(this string toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes = Encoding.Unicode.GetBytes(toEncode);
                string returnValue = Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }

        public static string Base64Decode(this string encodedData)
        {
            try
            {
                byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
                string returnValue = Encoding.Unicode.GetString(encodedDataAsBytes);
                if (returnValue.Any(c => c > 255))
                    returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);
                return returnValue;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }
    }
}