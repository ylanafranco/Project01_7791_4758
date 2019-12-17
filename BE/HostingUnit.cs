using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit : Enumeration
    {
        public int HostingUnitKey = Configuration.NumStaticHostingUnit;
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }
        public Area Areaa { get; set; }
        public Type Typee { get; set; }
        public int Room { get; set; }
        public bool[,] Diary { get; set; } = new bool[12, 31];
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool Spa { get; set; }
        public bool SportRoom { get; set; }
        public bool Parking { get; set; }
        public bool Lounge { get; set; }
        public bool KidsClub { get; set; }
        public bool RoomService { get; set; }
        public bool PetsAccepted { get; set; }
        public bool WifiIncluded { get; set; }
        public bool SmokingRoom { get; set; }
        public bool HandicapAccess { get; set; }
        public bool Television { get; set; }
        public bool Restaurant { get; set; }
        public bool AirConditionner { get; set; }
        public bool ChildrenAttraction { get; set; }
        public double PricePerNight { get; set; }
        public override string ToString()
        {
            string s = ("The Hosting Unit Key and it name are " + HostingUnitKey + " and " + HostingUnitName 
                + ", his owner is " + Owner.ToString());
            return s;
        }



    }
}
