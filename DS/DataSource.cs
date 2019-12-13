using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class DataSource
    {
        public static List<HostingUnit> GetAllHostingUnitCollection = new List<HostingUnit>();
        public static List<Host> GetAllHost = new List<Host>();
        public static List<Order> GetAllOrder = new List<Order>();
        public static List<GuestRequest> GetAllGuestRequest = new List<GuestRequest>();
        public static List<int> GetAllBranch = new List<int>();
    }
}
