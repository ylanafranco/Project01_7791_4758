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
            init();
        }

        public void init()
        {
        }

        #region ADD
        public void AddGuestRequest(GuestRequest gs)
        {
            string diff = (gs.ReleaseDate - gs.EntryDate).TotalDays.ToString();
            int num = int.Parse(diff);
            if (num <= 0)
            {
                throw new ArgumentException("You need to recheck your dates. This is not valid.");
            }
            else
                dal.AddGuestRequest(gs);
        }

        public void AddOrder(Order or)
        {
            GuestRequest guestreq = dal.GetGuestRequest(or.HostingUnitKey);
            HostingUnit myhosting = dal.GetHostingUnit(or.HostingUnitKey);
            bool flag = CheckMatrice(guestreq.EntryDate, guestreq.ReleaseDate, myhosting);
            if (flag == true)
            {
                throw new ArgumentException("The dates are not available for this hosting unit");

            }
            dal.AddOder(or);
        }

        public void AddHost(Host h)
        {
            dal.AddHost(h);
        }

        public void AddHostingUnit(HostingUnit hu)
        {
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

                GuestRequest guestreq = dal.GetGuestRequest(or.GuestRequestKey);
                int NumDays = CommissionCost(guestreq);
                Console.WriteLine("The cost of the comission is " + NumDays);

                //MaJ du statut de tous les orders avec cette guest request pour les fermer
                UpdateOrderAndGuestReq(or);
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
                // faire une fonction pour envoyer un mail 
                Console.WriteLine("MAIL SENT");
            }
            dal.UpdateOrder(or);
        }

        public void UpdateGuestRequest(GuestRequest gs)
        {

            dal.UpdateGuestRequest(gs);

        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
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
            int count=0;
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


    }
}
