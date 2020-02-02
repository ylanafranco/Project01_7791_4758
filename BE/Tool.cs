using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BE
{
    public static class Tool
    {
        //public static T Clone<T>(this T source)
        //{
        //    if (source == null)
        //        return default(T);
        //    if (!typeof(T).IsSerializable)
        //        throw new ArgumentException("The type must be serializable.", "source");
        //    IFormatter formatter = new BinaryFormatter();
        //    Stream stream = new MemoryStream();
        //    using (stream)
        //    {
        //        formatter.Serialize(stream, source);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return (T)formatter.Deserialize(stream);
        //    }
        //}

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
