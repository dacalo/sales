using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Sales.Helpers
{
    public static class RegexHelper
    {
        public static bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                var email = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
