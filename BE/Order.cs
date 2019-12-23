using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order : Enumeration
    {
        //private int HostingUnitkey;
        public long HostingUnitKey { get; set; }

        //private int GuestRequestkey;
        public long GuestRequestKey { get; set; }

        private int orderKey;
        public int OrderKey { get { return orderKey; } set { orderKey = Configuration.NumStaticOrder++; } }

        private OrderStatus status;
        public OrderStatus Status { get { return status; } set { status = value; } }

        private DateTime createDate;
        public DateTime CreateDate { get { return createDate; } set { createDate = value; } }

        private DateTime orderDate;
        public DateTime OrderDate { get { return orderDate; } set { orderDate = value; } }

        private double priceOfTheStay;
        public double PriceOfTheStay { get { return priceOfTheStay; } set { priceOfTheStay = value; } }

        // ichour de paiment 
        private bool paymentConfirmation;
        public bool PaymentConfirmation { get { return paymentConfirmation; } set { paymentConfirmation = value; } }

        public override string ToString()
        {
            string s = ("Order Details : " +
                "\nOrder Number : " + orderKey
                + "\nHosting Unit Key : " + HostingUnitKey
                + "\nGuest Request Key : " + GuestRequestKey
                + "\nCreate Date : " + createDate
                + "\nOrder Date : " + orderDate
                + "\nStatus : " + status
                + "\nPrice of the Stay : " + priceOfTheStay);
            return s;
        }


    }
}
