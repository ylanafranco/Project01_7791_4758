//using BL;
//using BE;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PL
//{
//    class Program
//    {
//        static IBL bl = FactoryBL.GetBL();
//        public static Host Createhost()
//        {

//            Host host = new Host();
//            Console.WriteLine("New Host");
//            Console.WriteLine("Enter Your ID");
//            host.HostKey = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Name");
//            host.FamilyName= Console.ReadLine();
//            Console.WriteLine("Enter Your FIRSTNAME");
//            host.PrivateName=Console.ReadLine();
//            Console.WriteLine("Enter Your Mail");
//            host.MailAddress=Console.ReadLine();
//            Console.WriteLine("Enter Your Num");
//            host.PhoneNumber = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Acount number");
//            host.BankAccountNumber = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Bank Number");
//            host.BankAccount = new BankAccount();
//            host.BankAccount.BankNumber = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Branch Number");
//            host.BankAccount.BranchNumber = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Bank Name ");
//            host.BankAccount.BankName = Console.ReadLine();
//            Console.WriteLine("Enter Your Bank Adress");
//            host.BankAccount.BranchAddress = Console.ReadLine();
//            Console.WriteLine("Enter Your Bank City");
//            host.BankAccount.BranchCity = Console.ReadLine();

//            return host;
//        }

//        public static GuestRequest CreateGuestReq()
//        {

//            GuestRequest gs = new GuestRequest();
//            Console.WriteLine("New Guest Req");
//            gs.GuestRequestKey = Configuration.NumStaticGuestRequest;
//            Console.WriteLine("Enter Your Name");
//            gs.FamilyName = Console.ReadLine();
//            Console.WriteLine("Enter Your ID");
//            gs.ID = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your FIRSTNAME");
//            gs.PrivateName = Console.ReadLine();
//            Console.WriteLine("Enter Your Mail");
//            gs.MailAddress = Console.ReadLine();
//            Console.WriteLine("Enter Your Tel");
//            gs.PhoneNumber = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Entry Date");
//            gs.EntryDate = DateTime.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Release Date");
//            gs.ReleaseDate = DateTime.Parse(Console.ReadLine());
//            Console.WriteLine("Enter Your Area : choose between Jerusalem,North, South, Center");
//            string area = Console.ReadLine();
//            if (area == "Jerusalem")
//            {
//                gs.Area = Enumeration.Area.Jerusalem;
//            }
//            else if (area == "North")
//            {
//                gs.Area = Enumeration.Area.North;
//            }
//            else if (area == "South")
//            {
//                gs.Area = Enumeration.Area.South;
//            }
//            else
//            {
//                gs.Area = Enumeration.Area.Center;
//            }
            
//            Console.WriteLine("Enter Your Type of location : choose between Zimmer, Hotel, Camping, Hostel, GuestHouse");
//            string type = Console.ReadLine();
//            if (type == "Zimmer")
//            {
//                gs.Type = Enumeration.Type.Zimmer;
//            }
//            else if (type == "Hotel")
//            {
//                gs.Type = Enumeration.Type.Hotel;
//            }
//            else if (type == "Camping")
//            {
//                gs.Type = Enumeration.Type.Camping;
//            }
//            else if (type == "Hostel")
//            {
//                gs.Type = Enumeration.Type.Hostel;
//            }
//            else if (type == "Appartment")
//            {
//                gs.Type = Enumeration.Type.Appartment;
//            }
//            Console.WriteLine("Enter the number of adults");
//            gs.Adults = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter the number of children");
//            gs.Children = int.Parse(Console.ReadLine());

//            return gs;
//        }

//        public static HostingUnit CreateHostingUnit(Host owner)
//        {
//            HostingUnit HU = new HostingUnit();
//            Console.WriteLine("New Hosting Unit");
//            HU.HostingUnitKey = Configuration.NumStaticHostingUnit;
//            Console.WriteLine("Enter a name");
//            HU.HostingUnitName = Console.ReadLine();
//            HU.Owner = owner;
//            Console.WriteLine("Enter Your Area : choose between Jerusalem,North, South, Center");
//            string area = Console.ReadLine();
//            if (area == "Jerusalem")
//            {
//                HU.Areaa = Enumeration.Area.Jerusalem;
//            }
//            else if (area == "North")
//            {
//                HU.Areaa = Enumeration.Area.North;
//            }
//            else if (area == "South")
//            {
//                HU.Areaa = Enumeration.Area.South;
//            }
//            else
//            {
//                HU.Areaa = Enumeration.Area.Center;
//            }

//            Console.WriteLine("Enter Your Type of location : choose between Zimmer, Hotel, Camping, Hostel, GuestHouse");
//            string type = Console.ReadLine();
//            if (type == "Zimmer")
//            {
//                HU.Typee = Enumeration.Type.Zimmer;
//            }
//            else if (type == "Hotel")
//            {
//                HU.Typee = Enumeration.Type.Hotel;
//            }
//            else if (type == "Camping")
//            {
//                HU.Typee = Enumeration.Type.Camping;
//            }
//            else if (type == "Hostel")
//            {
//                HU.Typee = Enumeration.Type.Hostel;
//            }
//            else if (type == "Appartment")
//            {
//                HU.Typee = Enumeration.Type.Appartment;
//            }
//            Console.WriteLine("enter num of rooms");
//            HU.Room = int.Parse(Console.ReadLine());
//            Console.WriteLine("enter price per night");
//            HU.PricePerNight = double.Parse(Console.ReadLine());

//            return HU;

//        }

//        public static Order CreateOrder(long guestreqkey, long hostingunitkey)
//        {
//            Order OR = new Order();
//            OR.GuestRequestKey = guestreqkey;
//            OR.HostingUnitKey = hostingunitkey;
//            OR.OrderKey = Configuration.NumStaticOrder;
//            OR.CreateDate = DateTime.Now;
//            return OR;
//        }

//        static void Main(string[] args)
//        {
//            Host host = new Host();
//            host = Createhost();
//            bl.AddHost(host);
//            host.CollectionClearance = false;
//            bl.UpdateHost(host);
//            foreach (var item in bl.GetAllHost())
//            {
//                Console.WriteLine(host.ToString());
//            }
//            //host.PhoneNumber = 0542234632;
//            //bl.UpdateHost(host);
//            //foreach (var item in bl.GetAllHost())
//            //{
//            //    Console.WriteLine(host.ToString());
//            //}
//            Console.WriteLine("\n");
//            GuestRequest gs = new GuestRequest();
//            gs = CreateGuestReq();
//            bl.AddGuestRequest(gs);
//            Console.WriteLine("\n");
//            foreach (var item in bl.GetAllGuestRequest())
//            {
//                Console.WriteLine(gs.ToString());
//            }
//            Console.WriteLine("\n");
//            HostingUnit HU = new HostingUnit();
//            HU = CreateHostingUnit(host);
//            bl.AddHostingUnit(HU);
//            Console.WriteLine("\n");
//            foreach (var item in bl.GetAllHostingUnitCollection())
//            {
//                Console.WriteLine(HU.ToString());

//            }
//            Console.WriteLine("\n");
//            Order Or = new Order();
//            Or = CreateOrder(gs.GuestRequestKey, HU.HostingUnitKey);
//            bl.AddOrder(Or);
//            Console.WriteLine("\n");
//            foreach (var item in bl.GetAllOrder())
//            {
//                Console.WriteLine(Or.ToString());

//            }
//            Console.WriteLine("\n");
//            bl.UpdateOrder(Or);
//            Console.WriteLine("\n");
//            foreach (var item in bl.GetAllOrder())
//            {
//                Console.WriteLine(Or.ToString());

//            }
//            Console.WriteLine("\n");
//            //Or.Status = Enumeration.OrderStatus.ClosedForCustomerResponse;
//            //bl.UpdateOrder(Or);
//            //Console.WriteLine("\n");
//            //foreach (var item in bl.GetAllOrder())
//            //{
//            //    Console.WriteLine(Or.ToString());

//            //}
//            //Console.WriteLine("\n");
//            //bl.UpdateOrder(Or);
//            //Console.WriteLine("\n");
//            //foreach (var item in bl.GetAllOrder())
//            //{
//            //    Console.WriteLine(Or.ToString());

//            //}
//            Console.WriteLine("\n");
//            //elle est ok
//            bl.testYourChance();

//            Console.WriteLine("enter key");
//            Console.ReadKey();

//        }
//    }
//}
