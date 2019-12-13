using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order : Enumeration
    {
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int PriceOfTheStay { get; set; }
        // ichour comme quoi c'est cet order la qui est voulu
        //public bool flagConfirmation { get; set; }

        public override string ToString()
        {
            string s = ("The Order number is " + OrderKey
                + " , the Hosting Unit Key is " + HostingUnitKey
                + " and the Guest Request key is " + GuestRequestKey);
            return s;
        }


    }
}
