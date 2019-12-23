using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BE
{
    public class Tool
    {

        public static bool IsValidEmail(string mail)
        {
            //try
            //{
            //    var addr = new System.Net.Mail.MailAddress(email);
            //    return addr.Address == email;
            //}
            //catch
            //{
            //    return false;
            //}

            Regex r = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (r.IsMatch(mail) && mail.EndsWith("@gmail.com"))
            {
                return true;

            }
            else
                return false;
                
        }


        public static bool isValidNumber(int num)
        {
            string str = num.ToString();
            if (str.Length == 9)
                return true;
            return false;
        }

        public static bool isValidID(int num)
        {
            string str = num.ToString();
            if (str.Length == 9)
                return true;
            return false;
        }
    }
}
