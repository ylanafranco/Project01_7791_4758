using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;


namespace BE
{
    public class GuestRequest : Enumeration
    {
        private int GuestRequestkey;
        public int GuestRequestKey
        {
            get { return GuestRequestkey; }
            set { GuestRequestkey = Configuration.NumStaticGuestRequest++; }

        }

        private string familyName { get; set; }
        public string FamilyName
        {
            get { return familyName.ToUpper(); }
            set
            {
                familyName = value;
            }
        }

        private string privateName;
        public string PrivateName { get { return privateName.ToUpper(); }
            set { privateName = value; } }

        private int id;
        public int ID { get { return id; }
            set { if(!Tool.isValidID(value))
                    throw new ArgumentException(string.Format("This {0} is not valid", value));
                id = value;
            }
        }

        private string mailAddress;
        public string MailAddress { get {return mailAddress; }
            set {
                if (!(Tool.IsValidEmail(value)))
                    throw new ArgumentException(string.Format("This {0} is not valid", value));
                mailAddress = value;
            } }

        private int phoneNumber;
        public int PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (!(Tool.isValidNumber(value)))
                {
                    throw new ArgumentException(string.Format("This {0} is not valid.", value));
                }
                phoneNumber = value;

            }
        }

        private GuestRequestStatus status;
        public GuestRequestStatus Status { get {return status; } set { status = value; } }

        private DateTime registrationDate;
        public DateTime RegistrationDate 
        { get { return registrationDate; }
            set { registrationDate = value; }
         }

        private DateTime entryDate;
        public DateTime EntryDate { 
                get { return entryDate; }
                set {
                if (value < DateTime.Now)
                {
                    throw new ArgumentException(string.Format("This date {0} has already passed.", value));
                }
                entryDate = value; }
            }

        private DateTime releaseDate;
        public DateTime ReleaseDate
        {
            get { return releaseDate; }
            set {
                if (value < this.EntryDate)
                {
                    throw new ArgumentException(string.Format("This date {0} has already passed.", value));
                }
                releaseDate = value; }
        }

        private int numberofDays;
        public int NumberofDays
        {
            get { return numberofDays; }
            set { numberofDays = value; }
        }

        private Area area;
        public Area Area { get { return area; } set { area = value; } }


        //public string SubArea { get; set; }

        private Type type;
        public Type Type { get { return type; } set { type = value; } }

        private int adults;
        public int Adults { get { return adults; } set { adults = value; } }

        private int children;
        public int Children { get { return children; } set { children = value; } }

        private int numTotalPersons;
        public int NumTotalPersons { get{ return numTotalPersons; } set { numTotalPersons = value; } }

        private int room;
        public int Room { get { return room;  } set { room = value; } }

        private Response pool;
        public Response Pool { get { return pool; } set { pool = value; } }

        private Response kidClub;
        public Response KidClub { get { return kidClub; } set { kidClub = value; } }

        private Response parking;
        public Response Parking { get { return parking; } set { parking = value; } }

        private Response petsAccepted;
        public Response PetsAccepted { get { return petsAccepted; } set { petsAccepted = value; } }

        private Response wifiIncluded;
        public Response WifiIncluded { get { return wifiIncluded; } set { wifiIncluded = value; } }

        private Response smokingRoom;
        public Response SmokingRoom { get { return smokingRoom; } set { smokingRoom = value; } }

        private Response handicapAccess;
        public Response HandicapAccess { get { return handicapAccess; } set { handicapAccess = value; } }

        private Response restaurant;
        public Response Restaurant { get { return restaurant; } set { restaurant = value; } }

        //public Response Jacuzzi { get; set; }
        //public Response Garden { get; set; }

        private Response childrenAttractions;
        public Response ChildrenAttractions { get { return childrenAttractions; } set { childrenAttractions = value; } }

        private bool workTrip;
        public bool WorkTrip { get { return workTrip; } set { workTrip = value; } }

        public override string ToString()
        {
            string s = ("The Guest Request key is " + GuestRequestkey 
                + " , the client name is " + familyName + " " + privateName 
                + " and his mail adress is " + mailAddress);
            return s;
        }

    }
}
