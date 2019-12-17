using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using DS;

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
            //  check if there is one night to stay
            int diff = DateTime.Compare(gs.EntryDate, gs.ReleaseDate);
            // meme date entree et sortie
            if (diff == 0)
                throw new Exception("You need to recheck your dates");
            // entrydate + grande que releasedate
            else if (diff == 1)
                throw new Exception("You need to recheck your dates");
            else dal.AddGuestRequest(gs);
        }

        public void AddOrder(Order or)
        {
            GuestRequest guestreq = dal.GetGuestRequest(or.HostingUnitKey);
            HostingUnit myhosting = dal.GetHostingUnit(or.HostingUnitKey);
            DateTime i = guestreq.EntryDate;
            // verif le truc avec le -1
            if ((myhosting.Diary[guestreq.EntryDate.Month-1, guestreq.EntryDate.Day-1]) == false)
            {
                while (i != guestreq.ReleaseDate)
                {

                    if (myhosting.Diary[i.Month-1, i.Day-1] == true)
                    {
                        throw new Exception("The dates are not available for this hosting unit");
                    }
                    i = i.AddDays(1);

                }
                dal.AddOder(or);
            }

        }

        public void AddHost(Host h)
        {
            throw new NotImplementedException();
        }

        public void AddHostingUnit(HostingUnit hu)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region UPDATE
        public void UpdateOrder(Order or)
        {
            bool flagStatus = false;
            if (or.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
            {
                flagStatus = true; // pour empecher le ch de statut

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
                Host myHost = dal.GetHost(myhosting.Owner.HostKey);
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

            if (flagStatus == true)
                or.Status = Enumeration.OrderStatus.ClosedForCustomerResponse;
            dal.UpdateOrder(or);
        }

        public void UpdateGuestRequest(GuestRequest gs)
        {
          
            dal.UpdateGuestRequest(gs);

        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            throw new NotImplementedException();
        }

        public void UpdateHost(Host h)
        {
            bool flagIchour = false;
            IEnumerable<Order> listOrder = dal.GetAllOrder();
            var Orders = from or in listOrder
                         where or.Status == Enumeration.OrderStatus.SentEmail
                         select or;
            if (h.CollectionClearance == true)
            {                
                if (Orders != null)
                {
                    flagIchour = true;
                }
            }

            if (flagIchour == true)
            {
                h.CollectionClearance = true;
            }
            dal.UpdateHost(h);
        }
        #endregion

        #region ERASE
        public void EraseHostingUnit(int hostingunitkey)
        {
            HostingUnit myhosting = dal.GetHostingUnit(hostingunitkey);
            bool flag = false;
            foreach (var item in dal.GetAllOrder())
            {
                if (item.Status == Enumeration.OrderStatus.NotAddressed || item.Status == Enumeration.OrderStatus.SentEmail)
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
            throw new NotImplementedException();
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
            HostingUnit myhosting = dal.GetHostingUnit(or.HostingUnitKey);
            GuestRequest myguestreq = dal.GetGuestRequest(or.GuestRequestKey);
            DateTime i = myguestreq.EntryDate;
            while (i != myguestreq.ReleaseDate)
            {
                myhosting.Diary[i.Month-1, i.Day-1] = true;
                i = i.AddDays(1);

            }

        }

        // fonction pour calculer la commission du sejour
        public int CommissionCost(GuestRequest guestreq)
        {
            string NumDays = (guestreq.ReleaseDate - guestreq.EntryDate).TotalDays.ToString();
            int x= (int.Parse(NumDays) - 1);
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

    }
}
