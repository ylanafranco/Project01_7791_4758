using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;


namespace BL
{
    public class BL_imp : IBL
    {
        DAL.IDAL dal;

        public BL_imp()
        {
            dal = DAL.FactoryDAL.GetDAL();

        }


        #region ADD
        public void AddGuestRequest(GuestRequest gs)
        {
            if (gs.FamilyName == "" || gs.PrivateName == "" || gs.EntryDate == null || gs.ReleaseDate == null || !(Tool.isValidID(gs.ID)) || !(Tool.IsValidEmail(gs.MailAddress) || !(Tool.isValidNumber(gs.PhoneNumber))))
            {
                throw new ArgumentException("You need to fill all this information, please.");
            }
            gs.RegistrationDate = DateTime.Now;
            string diff = (gs.ReleaseDate - gs.EntryDate).TotalDays.ToString();
            int num = int.Parse(diff);
            if (num <= 0 || (gs.RegistrationDate > gs.EntryDate))
            {
                throw new ArgumentException("You need to recheck your dates. This is not valid.");
            }
            else
            {   gs.NumTotalPersons = gs.Adults + gs.Children;
                gs.Status = Enumeration.GuestRequestStatus.Open;
            }

            dal.AddGuestRequest(gs);
        }

        public void AddOrder(Order or)
        {
            GuestRequest guestreq = GetGuestRequest(or.GuestRequestKey);
            HostingUnit myhosting = GetHostingUnit(or.HostingUnitKey);
            if (guestreq != null && myhosting!=null )
            {
                bool flag = CheckMatrice(guestreq.EntryDate, guestreq.ReleaseDate, myhosting);
                if (flag == true)
                {
                    throw new ArgumentException("The dates are not available for this hosting unit");

                }
                or.CreateDate = DateTime.Now;
                or.Status = Enumeration.OrderStatus.NotAddressed;
                dal.AddOder(or);
            }
            else
            {
                if (guestreq == null && myhosting != null)
                {
                    throw new KeyNotFoundException("This request does not exist.");
                }
                else if (myhosting == null)
                {
                    throw new KeyNotFoundException("This Hosting Unit does not exist.");
                }
                
            }
            
        }

        public void AddHost(Host h)
        {
            if (h.FamilyName == "" || h.PrivateName == "" || !(Tool.isValidID(h.HostKey)) || !(Tool.IsValidEmail(h.MailAddress) || !(Tool.isValidNumber(h.PhoneNumber))))
            {
                throw new ArgumentException("You need to fill all this information, please.");
            }
            dal.AddHost(h);
        }

        public void AddHostingUnit(HostingUnit hu)
        {
            // verifier que son owner il existe dans le systeme
            if (GetHost(hu.Owner.HostKey) == null)
            {
                throw new KeyNotFoundException("The Owner of the Hosting Unit is not in the system. Please add him before.");
            }
            if (hu.HostingUnitName == "" || hu.Room == 0)
            {
                throw new ArgumentException("You need to fill all this information, please.");
            }
            initMatrice(hu);
            dal.AddHostingUnit(hu);
        }
        #endregion

        #region UPDATE
        public void UpdateOrder(Order or)
        {
            if (or.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
            {
                // on met a jour la maarehet comme quoi c reserve , on envoie la reservation 
                UpdateMatriceHostingunit(or);

                GuestRequest guestreq = GetGuestRequest(or.GuestRequestKey);
                int NumDays = CommissionCost(guestreq);
                Console.WriteLine("The cost of the commission is " + NumDays);

                //MaJ du statut de tous les orders avec cette guest request pour les fermer
                UpdateOrderAndGuestReq(or);

                or.PaymentConfirmation = true;
            }

            else if (or.Status == Enumeration.OrderStatus.NotAddressed)
            {
                // check si le host a une autorisation de prelev pour la commission
                HostingUnit myhosting = dal.GetHostingUnit(or.HostingUnitKey);
                Host myHost = GetHost(myhosting.Owner.HostKey);
                if (myHost.CollectionClearance == true)
                {
                    or.Status = Enumeration.OrderStatus.SentEmail;
                }
            }
            else if (or.Status == Enumeration.OrderStatus.SentEmail)
            {
                or.OrderDate = DateTime.Now;
                GuestRequest GR = GetGuestRequest(or.GuestRequestKey);
                or.PriceOfTheStay = PriceOftheStay(or);
                bool flag = false;
                if (GR.NumTotalPersons >= 5 || (int)(((GR.ReleaseDate - GR.EntryDate).TotalDays)-1) > 7)
                {
                    flag = true;
                    or.PriceOfTheStay = Reduction(or);
                }
                if (flag == true)
                {
                    Console.WriteLine("The cost of the stay is " + or.PriceOfTheStay +
                        "\n, we are delighted to announce that we have given you a 5% reduction on the price of the stay.");

                }
                else 
                    Console.WriteLine("The cost of the stay is " + or.PriceOfTheStay);
                // faire une fonction pour envoyer un mail 
                Console.WriteLine("MAIL SENT");
            }
            dal.UpdateOrder(or);
        }

        public void UpdateGuestRequest(GuestRequest gs)
        {
            if (gs.EntryDate == DateTime.Now)
            {
                gs.Status = Enumeration.GuestRequestStatus.ClosedBecauseItExpired;
                Console.WriteLine("Your request is closed because you didn't close any deal.");
            }
            dal.UpdateGuestRequest(gs);
        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            if (DateTime.Now.Day == 1)
                MatriceUptoZero(hu);
            dal.UpdateHostingUnit(hu);
        }

        public void UpdateHost(Host h)
        {
            List<HostingUnit> listHostingUnit = HostingUnitPerHost(h);
            List<Order> Orders = OrderSelonHostingUnit(listHostingUnit);
            if (Orders.Count() != 0)
            {
                h.CollectionClearance = true;
            }

            dal.UpdateHost(h);
        }
        #endregion

        #region ERASE
        public void EraseHostingUnit(int hostingunitkey)
        {
            HostingUnit myhosting = GetHostingUnit(hostingunitkey);
            bool flag = false;
            foreach (var item in dal.GetAllOrder())
            {
                if (item.Status == Enumeration.OrderStatus.SentEmail)
                {
                    flag = true;
                }
            }
            if (flag == true)
                throw new Exception("There is still Guest Request open with this Hosting Unit. You can't delete it.");
            else
                dal.EraseHostingUnit(hostingunitkey);
        }

        public void EraseHost(int key)
        {
            Host hostt = GetHost(key);
            //bool flag = false;
            List<HostingUnit> listHostingUnit = HostingUnitPerHost(hostt);

            if (listHostingUnit != null) 
            {
                foreach (var item in listHostingUnit)
                {
                    EraseHostingUnit(item.HostingUnitKey);
                    
                }
            }
            // en gros si ya thr des trucs dedans psk ya eu un pb en enlevant une des HU
            if (listHostingUnit != null)
            {
                throw new ArgumentException("It seems there is still one order open with one or your hosting Unit. We can't delete you from the system.");
            }
            dal.EraseHost(key);
        }
        #endregion

        #region GET 
        public GuestRequest GetGuestRequest(int key)
        {
            return dal.GetGuestRequest(key);
        }

        public Host GetHost(int key)
        {
            return dal.GetHost(key);
        }

        public HostingUnit GetHostingUnit(int key)
        {
            return dal.GetHostingUnit(key);
        }

        public Order GetOrder(int key)
        {
            return dal.GetOrder(key);
        }
        #endregion

        #region GET ALL LIST
        public IEnumerable<HostingUnit> GetAllHostingUnitCollection(Func<HostingUnit, bool> predicate = null)
        {
            return dal.GetAllHostingUnitCollection();
        }

        public IEnumerable<Host> GetAllHost(Func<Host, bool> predicate = null)
        {
            return dal.GetAllHost();
        }

        public IEnumerable<Order> GetAllOrder(Func<Order, bool> predicate = null)
        {
            return dal.GetAllOrder();
        }

        public IEnumerable<GuestRequest> GetAllGuestRequest(Func<GuestRequest, bool> predicate = null)
        {
            return dal.GetAllGuestRequest();
        }

        public IEnumerable<int> GetAllBranch(Func<int, bool> predicate = null)
        {
            return dal.GetAllBranch();
        }
        #endregion


        // fonction en plus pour mettre a jour le diary du hosting unit
        public void UpdateMatriceHostingunit(Order or)
        {
            HostingUnit myhosting = GetHostingUnit(or.HostingUnitKey);
            GuestRequest myguestreq = GetGuestRequest(or.GuestRequestKey);
            DateTime i = myguestreq.EntryDate;
            while (i != myguestreq.ReleaseDate)
            {
                myhosting.Diary[i.Month - 1, i.Day - 1] = true;
                i = i.AddDays(1);

            }

        }

        // fonction pour calculer la commission du sejour
        public int CommissionCost(GuestRequest guestreq)
        {
            string NumDays = (guestreq.ReleaseDate - guestreq.EntryDate).TotalDays.ToString();
            int x = (int.Parse(NumDays) - 1);
            return Configuration.Commission * x;
        }

        // fonction qui va changer le statut de tous les order et de la guestreq quand une confirmation est faite
        public void UpdateOrderAndGuestReq(Order or)
        {
            GuestRequest guestreq = dal.GetGuestRequest(or.GuestRequestKey);
            if (or.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
            {
                IEnumerable<Order> OrderList = dal.GetAllOrder();
                guestreq.Status = Enumeration.GuestRequestStatus.ClosedThroughTheSite;
                dal.UpdateGuestRequest(guestreq);
                foreach (Order item in OrderList)
                {
                    if (item.GuestRequestKey == guestreq.GuestRequestKey && (item.Status == Enumeration.OrderStatus.NotAddressed || item.Status == Enumeration.OrderStatus.SentEmail))
                    {
                        item.Status = Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness;
                        dal.UpdateOrder(item);
                    }
                }
            }
        }

        public List<HostingUnit> HostingUnitPerHost(Host h)
        {
            List<HostingUnit> HostingUnitHost = new List<HostingUnit>();
            // peut etre datasource.
            foreach (var item in dal.GetAllHostingUnitCollection())
            {
                if (item.Owner.HostKey == h.HostKey)
                {
                    HostingUnitHost.Add(item);
                }
            }
            return HostingUnitHost;

        }

        public List<Order> OrderSelonHostingUnit(List<HostingUnit> listHU)
        {
            List<Order> OrderSelonHU = new List<Order>();
            foreach (var item in dal.GetAllOrder())
            {
                foreach (var truc in listHU)
                {
                    if ((truc.HostingUnitKey == item.HostingUnitKey) && (item.Status == Enumeration.OrderStatus.SentEmail))
                    {
                        OrderSelonHU.Add(item);
                    }
                }
            }
            return OrderSelonHU;
        }

        public bool CheckMatrice(DateTime EntryD, DateTime ReleaseD, HostingUnit myhosting)
        {
            bool flag = false;
            DateTime i = EntryD;
            // verif le truc avec le -1
            if ((myhosting.Diary[EntryD.Month - 1, EntryD.Day - 1]) == false)
            {
                while (i != ReleaseD)
                {

                    if (myhosting.Diary[i.Month - 1, i.Day - 1] == true)
                    {
                        return (flag = true);
                        // cest occupe
                    }
                    i = i.AddDays(1);

                }

            }
            return flag;
        }

        // Check all the hosting unit free for this dates
        public List<HostingUnit> HostingUnitFree(DateTime EntryD, int num)
        {
            DateTime ReleaseD = EntryD.AddDays(num);
            List<HostingUnit> HUselonDates = new List<HostingUnit>();
            var list = from item in dal.GetAllHostingUnitCollection()
                       let temp = CheckMatrice(EntryD, ReleaseD, item)
                       where temp == false
                       select item;
            HUselonDates = list.ToList();
            return HUselonDates;
        }


        public int DifferenceDays(DateTime[] list)
        {
            int x = list.Length;
            int diff;
            if (x == 1)
            {
                diff = (int)(DateTime.Now - list[0]).TotalDays;
                return diff;

            }
            else if (x == 2)
            {
                diff = (int)(list[0] - list[1]).TotalDays;
                if (diff < 0)
                {
                    diff *= -1;
                }
                return diff;
            }
            return -1;
        }

        public List<Order> OrderSelonTime(int num)
        {
            List<Order> OrderTime = new List<Order>();
            var list = from item in dal.GetAllOrder()
                       let temp = (int)(item.OrderDate - item.CreateDate).TotalDays
                       where temp >= num
                       select item;
            OrderTime = list.ToList();
            return OrderTime;
        }

        // fonction qui recoit un client et compte le nombre de propositions qu'on lui a envoyées
        public int NumOfPropositionGR(GuestRequest guestreq)
        {
            int count = 0;
            foreach (var item in dal.GetAllOrder())
            {
                if (item.GuestRequestKey == guestreq.GuestRequestKey)
                {
                    count++;
                }
            }
            return count;
        }

        // fonction qui recoit un hostingunit et qui recoit le nombre de prop sur ce hostingunit
        public int NumofPropositionHU(HostingUnit hu)
        {
            int count = 0;
            foreach (var item in dal.GetAllOrder())
            {
                if ((item.HostingUnitKey == hu.HostingUnitKey) && (item.Status == Enumeration.OrderStatus.SentEmail || item.Status == Enumeration.OrderStatus.ClosedForCustomerResponse))
                {
                    count++;
                }
            }
            return count;
        }

        public void NumTotalPersonGR(GuestRequest gs)
        {
            gs.NumTotalPersons = gs.Adults;
            gs.NumTotalPersons += gs.Children;
            dal.UpdateGuestRequest(gs);

        }

        #region GROUPING
        public IGrouping<Enumeration.Area, GuestRequest> GetGuestReqGroupByArea(bool sorted = false)
        {
            return (IGrouping<Enumeration.Area, GuestRequest>)from gs in dal.GetAllGuestRequest()
                                                              group gs by gs.Area;
        }

        public IGrouping<int, GuestRequest> GetGuestRequestGroupByPersons(bool sorted = false)
        {
            return (IGrouping<int, GuestRequest>)from gs in dal.GetAllGuestRequest()
                                                 group gs by gs.NumTotalPersons;
        }

        public IGrouping<int, Host> GetHostGroupByNumofHU(bool sorted = false)
        {
            return (IGrouping<int, Host>)from host in dal.GetAllHost()
                                         group host by HostingUnitPerHost(host).Count();
        }

        public IGrouping<Enumeration.Area, HostingUnit> GetHUGroupByArea(bool sorted = false)
        {
            return (IGrouping<Enumeration.Area, HostingUnit>)from hu in dal.GetAllHostingUnitCollection()
                                                             group hu by hu.Areaa;
        }
        #endregion


        public delegate bool condition(GuestRequest guestreq);
        public List<GuestRequest> AllGuestRequestThat(condition Mycondition)
        {
            List<GuestRequest> myList = new List<GuestRequest>();
            List<GuestRequest> allGR = dal.GetAllGuestRequest().ToList();
            foreach (GuestRequest gs in allGR)
            {
                if (Mycondition(gs))
                    myList.Add(gs);
            }
            return myList;

        }

        // calculate the total cost of the stay 
        public double PriceOftheStay(Order or)
        {
            HostingUnit HU = GetHostingUnit(or.HostingUnitKey);
            GuestRequest GR = GetGuestRequest(or.GuestRequestKey);
            double numDays = (GR.ReleaseDate - GR.EntryDate).TotalDays - 1;
            double price = numDays * HU.PricePerNight;
            return price;
        }

        // when we finish a month , zero the month in the matrix
        // to allowed order for the next year
        public void MatriceUptoZero(HostingUnit HU)
        {            
            int month = (DateTime.Now.Month - 1);
            if (DateTime.Now.Day == 1)
            {
                for (int i = 0; i < 31; i++)
                {
                    HU.Diary[month, i] = false;
                }

            }
            dal.UpdateHostingUnit(HU);
        }

        // reduction of the price if the num of people >= 5 or num of the days are >
        public double  Reduction(Order or)
        {            
            double price = or.PriceOfTheStay;
            double reduc = 5 / 100 * or.PriceOfTheStay;
            price -= reduc;
            return price;
        }

        public void initMatrice(HostingUnit HU)
        {
            for (int i = 0; i < HU.Diary.GetLength(0); i++)
            {
                for (int j = 0; j < HU.Diary.GetLength(1); j++)
                {
                    HU.Diary[i, j] = false;
                }
            }
        }

        public void testYourChance()
        {
            Random r = new Random();
            int num = r.Next(1, 50);
            Console.WriteLine("Better to miss a chance than not to have tried it: enter a number between 1 and 50 included");
            string s = Console.ReadLine();
            int x = int.Parse(s);
            if (x == num)
            {
                Console.WriteLine("Omg, you just win your stay for free. Congrats!!");
            }

        }

    }
}
