﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Thread thread = new Thread(() => CheckExpiration());
            thread.Start();
        }

        void CheckExpiration()
        {
            List<GuestRequest> requests = new List<GuestRequest>();
            foreach (var item in GetAllGuestRequest())
            {
                if (item.ReleaseDate <= DateTime.Now)
                {
                    item.Status = Enumeration.GuestRequestStatus.ClosedBecauseItExpired;

                    foreach (var truc in OrderSelonGuestRequest(item))
                    {
                        truc.Status = Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness;
                    }
                }

                
            }
        
        }

        #region ADD
        public void AddGuestRequest(GuestRequest gs)
        {
            try
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
                    throw new ArgumentException("You need to recheck your dates. This is not valid. entry date is " + gs.EntryDate + " release date is " + gs.ReleaseDate + "registration date is " + gs.RegistrationDate);
                }
                else
                {
                    //gs.NumTotalPersons = gs.Adults + gs.Children;
                    gs.Status = Enumeration.GuestRequestStatus.Open;
                }

                dal.AddGuestRequest(gs);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("DAL PROBLEM : " + ex.Message);
            }

        }

        public void AddOrder(Order or)
        {
            try
            {
                GuestRequest guestreq = GetGuestRequest(or.GuestRequestKey);
                HostingUnit myhosting = GetHostingUnit(or.HostingUnitKey);
                if (guestreq != null && myhosting != null)
                {
                    bool flag = CheckMatrice(guestreq, myhosting);
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
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("DAL PROBLEM : " + ex.Message);
            }

        }

        public void AddHost(Host h)
        {
            try
            {
                if (h.FamilyName == "" || h.PrivateName == "" || !(Tool.isValidID(h.HostKey)) || !(Tool.IsValidEmail(h.MailAddress) || !(Tool.isValidNumber(h.PhoneNumber))))
                {
                    throw new ArgumentException("You need to fill all this information, please.");
                }
                h.CollectionClearance = checkCollectionClearance(h);
                dal.AddHost(h);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("DAL PROBLEM : " + ex.Message);
            }

        }

        public void AddHostingUnit(HostingUnit hu)
        {
            try {
                // verifier que son owner il existe dans le systeme
                if (GetHost(hu.Owner.HostKey) == null)
                {
                    throw new KeyNotFoundException("The Owner of the Hosting Unit is not in the system. Please add him before.");
                }
                if (hu.HostingUnitName == "" || hu.Room == 0)
                {
                    throw new ArgumentException("You need to fill all this information, please.");
                }
                hu.Diary = new bool[12, 31];
                initMatrice(hu.Diary);
                dal.AddHostingUnit(hu);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("DAL PROBLEM : " + ex.Message);
            }
        }
        #endregion

        #region UPDATE
        public void UpdateOrder(Order or)
        {
            try {
                if (or.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
                {
                    // on met a jour la maarehet comme quoi c reserve , on envoie la reservation 
                    UpdateMatriceHostingunit(or);

                    GuestRequest guestreq = GetGuestRequest(or.GuestRequestKey);
                    int NumDays = CommissionCost(guestreq);
                    // Console.WriteLine("The cost of the commission is " + NumDays);

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
                    else
                    {
                        or.Status = Enumeration.OrderStatus.NotAddressed;
                        throw new Exception("Sorry, the host doesn't have ichour gviya for the commission");

                    }
                }
                else if (or.Status == Enumeration.OrderStatus.SentEmail)
                {
                    or.OrderDate = DateTime.Now;
                    HostingUnit myhosting = GetHostingUnit(or.HostingUnitKey);
                    GuestRequest GR = GetGuestRequest(or.GuestRequestKey);
                    or.PriceOfTheStay = PriceOftheStay(or);
                    bool flag = CheckMatrice(GR, myhosting);
                    if (flag == true)
                    {
                        throw new ArgumentException("The dates are not available anymore for this hosting unit");

                    }
                    bool flag2 = false;
                    if (GR.NumTotalPersons >= 5 || (int)(((GR.ReleaseDate - GR.EntryDate).TotalDays) - 1) > 7)
                    {
                        flag2 = true;
                        or.PriceOfTheStay = Reduction(or);
                    }
                    //if (flag == true)
                    //{
                    //    Console.WriteLine("The cost of the stay is " + or.PriceOfTheStay +
                    //        "\n, we are delighted to announce that we have given you a 5% reduction on the price of the stay.");

                    //}
                    //else 
                    //    Console.WriteLine("The cost of the stay is " + or.PriceOfTheStay);
                    // faire une fonction pour envoyer un mail 
                    //Console.WriteLine("MAIL SENT");
                }
                dal.UpdateOrder(or);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("DAL PROBLEM : " + ex.Message);
            }
        }

        public void UpdateGuestRequest(GuestRequest gs)
        {
            try {
                if (gs.EntryDate == DateTime.Now)
                {
                    gs.Status = Enumeration.GuestRequestStatus.ClosedBecauseItExpired;
                    Console.WriteLine("Your request is closed because you didn't close any deal.");
                }
                dal.UpdateGuestRequest(gs);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("DAL PROBLEM : " + ex.Message);
            }
        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            try {
                //if (DateTime.Now.Day == 1)
                //    MatriceUptoZero(hu);
                dal.UpdateHostingUnit(hu);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("DAL PROBLEM : " + ex.Message);
            }
        }

        public void UpdateHost(Host h)
        {
            try {
                List<HostingUnit> listHostingUnit = HostingUnitPerHost(h);
                List<Order> Orders = OrderSelonHostingUnit(listHostingUnit);
                if (Orders.Count() != 0)
                {
                    h.CollectionClearance = true;
                }

                dal.UpdateHost(h);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("DAL PROBLEM : " + ex.Message);
            }
        }
        #endregion

        #region ERASE
        public void EraseHostingUnit(long hostingunitkey)
        {
            try {
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
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("DAL PROBLEM : " + ex.Message);
            }
        }

        public void EraseHost(int key)
        {
            try {
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
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("DAL PROBLEM : " + ex.Message);
            }
        }
        #endregion

        #region GET 
        public GuestRequest GetGuestRequest(long key)
        {
            return dal.GetGuestRequest(key);
        }

        public Host GetHost(int key)
        {
            return dal.GetHost(key);
        }

        public HostingUnit GetHostingUnit(long key)
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

        public IEnumerable<BankAccount> GetAllBranch()
        {
            var mylist = (from item in dal.GetAllBranch()
                          orderby item.BankNumber
                          select item);
            var list = mylist.GroupBy(x => new { x.BranchNumber, x.BankNumber, x.BankName }).Select(x => x.First());
            return list; 
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
            UpdateHostingUnit(myhosting);
        }

        // fonction pour calculer la commission du sejour
        public int CommissionCost(GuestRequest guestreq)
        {
            string NumDays = (guestreq.ReleaseDate - guestreq.EntryDate).TotalDays.ToString();
            int x = int.Parse(NumDays);
            return Configuration.Commission * x;
        }

        // fonction qui va changer le statut de tous les order et de la guestreq quand une confirmation est faite
        public void UpdateOrderAndGuestReq(Order or)
        {
            GuestRequest guestreq = dal.GetGuestRequest(or.GuestRequestKey);
            HostingUnit hu = dal.GetHostingUnit(or.HostingUnitKey);
            if (or.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
            {
                List<Order> OrderList = dal.GetAllOrder().ToList();
                guestreq.Status = Enumeration.GuestRequestStatus.ClosedThroughTheSite;
                dal.UpdateGuestRequest(guestreq);
                for (int i = 0; i < OrderList.Count(); i++)
                {
                    if (OrderList[i].HostingUnitKey == or.HostingUnitKey && (OrderList[i].Status == Enumeration.OrderStatus.NotAddressed || OrderList[i].Status == Enumeration.OrderStatus.SentEmail))
                    {
                        OrderList[i].Status = Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness;
                        UpdateOrder(OrderList[i]);

                    }
                }

                //foreach (Order item in OrderList)
                //    {
                //        if (item.HostingUnitKey == or.HostingUnitKey && (item.Status == Enumeration.OrderStatus.NotAddressed || item.Status == Enumeration.OrderStatus.SentEmail))

                //        {

                //            item.Status = Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness;
                //            UpdateOrder(item);
                //        }
                //    }  
            }
        }

        #region MORE LIST
        [Obsolete]
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

        
        //Check all the hosting unit free for this dates
        //public List<HostingUnit> HostingUnitFree(DateTime EntryD, int num)
        //{
        //    DateTime ReleaseD = EntryD.AddDays(num);
        //    List<HostingUnit> HUselonDates = new List<HostingUnit>();
        //    var list = from item in dal.GetAllHostingUnitCollection()
        //               let temp = CheckMatrice(EntryD, ReleaseD, item)
        //               where temp == false
        //               select item;
        //    HUselonDates = list.ToList();
        //    return HUselonDates;
        //}

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

        public List<Order> OrderSelonGuestRequest(GuestRequest gs)
        {
            List<Order> myorders = GetAllOrder().ToList();
            List<Order> OrderSelonGR = new List<Order>();
            foreach (var item in myorders)
            {
                if (item.GuestRequestKey == gs.GuestRequestKey)
                {
                    OrderSelonGR.Add(item);
                } 
            }
            return OrderSelonGR;
        }

        #endregion

        // check is the matrice is free between dates
        public bool CheckMatrice(GuestRequest mygr, HostingUnit myhosting)
        {
            
            DateTime i = mygr.EntryDate;
            // verif le truc avec le -1
            if ((myhosting.Diary[mygr.EntryDate.Month - 1, mygr.EntryDate.Day - 1]) == false)
            {
                while (i != mygr.ReleaseDate)
                {

                    if (myhosting.Diary[i.Month - 1, i.Day - 1] == true)
                    {
                        return true;
                        
                        // cest occupe
                    }
                    i = i.AddDays(1);

                }

            }
            else
            {
                return true;
                
            }
            return false;
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

        [Obsolete]
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

        [Obsolete]
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

        
        public int NumTotalPersonGR(int a, int b)
        {
            int temp = 0;
            temp += a;
            temp += b;
            //gs.NumTotalPersons = gs.Adults;
            //gs.NumTotalPersons += gs.Children;
            return temp;

        }

        #region GROUPING
        public IEnumerable<IGrouping<Enumeration.Area, GuestRequest>> GetGuestReqGroupByArea(IEnumerable <GuestRequest> guestReq)
        {
           var guestReq_group = from gs in guestReq
                                     group gs by gs.Area into g1
                                    select g1;
            return guestReq_group;
            
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GetGuestRequestGroupByPersons(IEnumerable<GuestRequest> guestReq)
        {
            var guestReq_group = from gs in guestReq
                                 group gs by gs.NumTotalPersons into g1
                                 select g1;
            return guestReq_group;

        }

        public IEnumerable<IGrouping<int, Host>> GetHostGroupByNumofHU(IEnumerable<Host> hosts)
        {
            var host_group = from host in hosts
                                 group host by HostingUnitPerHost(host).Count() into g1
                                 select g1;
            return host_group;
            
        }

        public IEnumerable<IGrouping<Enumeration.Area, HostingUnit>> GetHUGroupByArea(IEnumerable <HostingUnit> hostingunits)
        {
            var hostingunit_group = from hostingu in hostingunits
                                    group hostingu by hostingu.Areaa into g1
                             select g1;
            return hostingunit_group;
            
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
            double numDays = (GR.ReleaseDate - GR.EntryDate).TotalDays;
            double price = numDays * HU.PricePerNight;
            return price;
        }

        // when we finish a month , zero the month in the matrix
        // to allowed order for the next year
        public void MatriceUptoZero(HostingUnit HU)
        {            
            int month = (DateTime.Now.Month - 1);
            for (int i = 0; i < 31; i++)
            {
                    HU.Diary[month -1, i] = false;
            }
            dal.UpdateHostingUnit(HU);
        }

        // reduction of the price if the num of people >= 5 or num of the days are >
        public double  Reduction(Order or)
        {            
            double price = or.PriceOfTheStay;
            double reduc = or.PriceOfTheStay;
            reduc = reduc * 5;
            reduc = reduc / 100;
            price -= reduc;
            return price;
        }

        public void initMatrice(bool[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = false;
                }
            }
        }

        public bool testYourChance(int mynum)
        {
            Random r = new Random();
            int num = r.Next(1, 30);
            //Console.WriteLine("Better to miss a chance than not to have tried it: enter a number between 1 and 30 included");
            //string s = Console.ReadLine();
            //int x = int.Parse(s);
            if (mynum == num)
            {
                return true;
                //Console.WriteLine("Omg, you just win your stay for free. Congrats!!");
            }
            else
            {
                return false;
               // Console.WriteLine(String.Format("Oh you lose! Test your chance in your next trip. The right number was {0}.", num));
            }

        }

        public bool checkCollectionClearance(Host H)
        {
            if ((H.BankAccount.BankNumber != 0) && (H.BankAccount.BankName != "") && (H.BankAccount.BranchAddress != "") && (H.BankAccount.BranchCity != "") && (H.BankAccount.BranchNumber != 0) && (H.BankAccountNumber != 0))
            {
                return true;
            }
            return false;
        }

        public bool comparaison(GuestRequest gs, HostingUnit hu)
        {

            GuestRequest myrequest = GetGuestRequest(gs.GuestRequestKey);
            HostingUnit myhosting = GetHostingUnit(hu.HostingUnitKey);
            if ((myrequest.Type == myhosting.Typee) && (myrequest.Area == myhosting.Areaa) && (myhosting.Bed >= myrequest.NumTotalPersons))
            {
                if (((myrequest.Pool == Enumeration.Response.Necessary || myrequest.Pool == Enumeration.Response.Possible) && myhosting.Pool == true) || (myrequest.Pool == Enumeration.Response.NotInteressed && myhosting.Pool == false))
                {
                    if (((myrequest.KidClub == Enumeration.Response.Necessary || myrequest.KidClub == Enumeration.Response.Possible) && myhosting.KidsClub == true) || (myrequest.KidClub == Enumeration.Response.NotInteressed && myhosting.KidsClub == false))
                    {
                        if (((myrequest.Parking == Enumeration.Response.Necessary || myrequest.Parking == Enumeration.Response.Possible) && myhosting.Parking == true) || (myrequest.Parking == Enumeration.Response.NotInteressed && myhosting.Parking == false))
                        {
                            if (((myrequest.PetsAccepted == Enumeration.Response.Necessary || myrequest.PetsAccepted == Enumeration.Response.Possible) && myhosting.PetsAccepted == true) || (myrequest.PetsAccepted == Enumeration.Response.NotInteressed && myhosting.PetsAccepted == false))
                            {
                                if (((myrequest.FoodIncluded == Enumeration.Response.Necessary || myrequest.FoodIncluded == Enumeration.Response.Possible) && myhosting.FoodIncluded == true) || (myrequest.FoodIncluded == Enumeration.Response.NotInteressed && myhosting.FoodIncluded == false))
                                {
                                    if (((myrequest.WifiIncluded == Enumeration.Response.Necessary || myrequest.WifiIncluded == Enumeration.Response.Possible) && myhosting.WifiIncluded == true) || (myrequest.WifiIncluded == Enumeration.Response.NotInteressed && myhosting.WifiIncluded == false))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }

                    }
                }
            }
                return false;
        }

 

        public IEnumerable<int> GetBranchNumbers(string BankName)
        {
            var x = from item in GetAllBranch()
                    where item.BankName == BankName
                    select item.BranchNumber;

            List<int> mylist = x.ToList().Distinct().ToList();
            mylist.Sort();
            return mylist;


        }

       

        //Return all the bank detail by it branch number and it name
        public BankAccount GetMyBranch(int branchnum, string bankname)
        {
            foreach (BankAccount item in GetAllBranch())
            {
                if ((item.BranchNumber == branchnum) && (item.BankName == bankname))
                {
                    return item;
                }
            }
            return new BankAccount();
        }

        //IEnumerable<int> IBL.GetAllBranch()
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<string> GetBankName()
        {
            var list = from item in GetAllBranch()
                       select item.BankName;
            return list.Distinct().ToList();
        }
    }
}
