using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit : Enumeration
    {
        private long HostingUnitkey;
        public long HostingUnitKey
        {
            get { return HostingUnitkey; }
            set { HostingUnitkey = Configuration.NumStaticHostingUnit++; }
        }

        private Host owner;
        public Host Owner { get { return owner; } set { owner = value; } }

        private string HostingUnitname;
        public string HostingUnitName { get { return HostingUnitname; } set { HostingUnitname = value; } }

        private Area areaa;
        public Area Areaa { get { return areaa; } set { areaa = value; } }

        private Type typee;
        public Type Typee { get { return typee; } set { typee = value; } }

        private int room;
        public int Room { get { return room; } set { room = value; } }

        private bool[,] diary;
        public bool[,] Diary
        {
            get { return diary; }
            set { diary = new bool[12, 31]; }
        }

        public List<string> Uris { get; set; }


        private int bed;
        public int Bed { get { return bed; } set { bed = value; } }


        public bool Pool { get; set; }
        public bool Spa { get; set; }
        public bool SportRoom { get; set; }
        public bool Parking { get; set; }
        public bool KidsClub { get; set; }
        public bool PetsAccepted { get; set; }
        public bool WifiIncluded { get; set; }
        public bool HandicapAccess { get; set; }
        public bool FoodIncluded { get; set; }
        public bool AirConditionner { get; set; }

        private double pricePerNight;
        public double PricePerNight { get { return pricePerNight; } set { pricePerNight = value; } }

        

        public override string ToString()
        {
            string s = ("Hosting Unit Details : " +
                "\nHosting Unit Key : " + HostingUnitKey +
                "\n Hosting Unit Name : " + HostingUnitName
                + "\nOwner : \n" + Owner.ToString()
                + "\nType : " + typee
                + "\nArea : " + areaa
                + "\nNumber of rooms :" + room
                + "\nNumber of beds :" + bed
                + "\nPrice per night :" + pricePerNight);
              //  + "\n pool :" + Pool.ToString());
            return s;
        }



    }
}
