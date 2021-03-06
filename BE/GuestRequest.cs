﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;


namespace BE
{
    [Serializable]
    public class GuestRequest : Enumeration
    {
        private long GuestRequestkey;
        public long GuestRequestKey
        {
            get { return GuestRequestkey; }
            set { GuestRequestkey = value; }

        }

        private string familyName { get; set; }
        public string FamilyName
        {
            get { return familyName; }
            set
            {
                familyName = value;
            }
        }

        private string privateName;
        public string PrivateName { get { return privateName; }
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
                    throw new FormatException("Email address must be a valid gmail address!");
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
                set {entryDate = value; }
            }

        private DateTime releaseDate;
        public DateTime ReleaseDate
        {
            get { return releaseDate; }
            set { releaseDate = value; }
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

        private Response handicapAccess;
        public Response HandicapAccess { get { return handicapAccess; } set { handicapAccess = value; } }

        private Response foodIncluded;
        public Response FoodIncluded { get { return foodIncluded; } set { foodIncluded = value; } }
        
        public override string ToString()
        {
            string s = ("Guest Request Details : " +
                "\nGuest Request key : " + GuestRequestkey
                + "\nName :" + familyName + " " + privateName
                + "\nMail : " + mailAddress
                + "\nPhone Number : " + "0" + phoneNumber
                + "\nStatus : " + Status
                + "\nEntry Date : " + entryDate
                + "\nRelease Date : " + releaseDate
                + "\nType : " + type
                + "\nArea : " + area
                + "\nNum of people :" + numTotalPersons);
               // + "\n Pool :" + Pool.ToString()
                //+ "\n Food :" + FoodIncluded.ToString()
                //+"\n Wifi :" + WifiIncluded.ToString());
            return s;
        }

    }
}
