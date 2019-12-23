using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
        public static int NumStaticGuestRequest = 10000000;
        public static int NumStaticHostingUnit = 10000000;
        public static int NumStaticOrder = 1;
        public static int Commission = 10;
        public static string WEB_LICENSE_MAIL;
        public static string MAIL_PASSWORD;
    }
}
