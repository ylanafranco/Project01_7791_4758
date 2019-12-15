using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BE
{
    public class GuestRequest : Enumeration
    {
        public int GuestRequestKey { get; set; }
        //= Configuration.NumStaticGuestRequest;
        public string FamilyName { get; set; }
        public string PrivateName { get; set; }
        public string MailAddress { get; set; }
        public GuestRequestStatus Status { get; set; }
        //public List<Order> AllOrdersProposed { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int NumberofDays { get; set; }
        public Area Area { get; set; }
        //public string SubArea { get; set; }
        public Type Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Room { get; set; }
        public Response Pool { get; set; }
        public Response KidClub { get; set; }
        public Response Parking { get; set; }
        public Response PetsAccepted { get; set; }
        public Response WifiIncluded { get; set; }
        public Response SmokingRoom { get; set; }
        public Response HandicapAccess { get; set; }
        public Response Restaurant { get; set; }
        //public Response Jacuzzi { get; set; }
        //public Response Garden { get; set; }
        public Response ChildrenAttractions { get; set; }
        public bool WorkTrip { get; set; }

        public override string ToString()
        {
            string s = ("The Guest Request key is " + GuestRequestKey 
                + " , and the client name is " + FamilyName + " " + PrivateName 
                + " and his mail adress is " + MailAddress);
            return s;
        }

        // pas sur de mon coup
        public GuestRequest()
        {
            this.GuestRequestKey = Configuration.NumStaticGuestRequest++;
        }



    }
}
