using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Enumeration
    {
        public enum Area { North, South, Center, Jerusalem}
        public enum SubArea_North { Tiberiade, Haifa, Golan, KineretLake, Tzfat }
        public enum SubArea_South { Eilat, Ashkelon, BeerCheva, Ashdod, MitzpeRamon, DeadSea, }
        public enum SubArea_Center { TelAviv, Netanya, Hertzilia, BatYam, RichonLeZion }
        public enum Type { Zimmer, Hotel, Camping, Hostel, Appartment, House }
        public enum Response { Necessary, Possible, NotInteressed}
        public enum OrderStatus { NotAddressed, SentEmail, ClosedForCustomerUnresponsiveness, ClosedForCustomerResponse }
        public enum GuestRequestStatus { Open, ClosedThroughTheSite, ClosedBecauseItExpired }

        }
}
