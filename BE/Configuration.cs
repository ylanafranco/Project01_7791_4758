using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public static class Configuration
    {
        public static long NumStaticGuestRequest = 10000000;
        public static long NumStaticHostingUnit = 10000000;
        public static int NumStaticOrder = 1;
        public static int Commission = 10;
        public static string MAIL = "befitbynana@gmail.com";
        public static string MAIL_PASSWORD = "ylanaetclara";
    }
}
