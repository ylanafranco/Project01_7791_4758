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

            if ((myhosting.Diary[guestreq.EntryDate.Month, guestreq.EntryDate.Day]) == false)
            {
                while (i != guestreq.ReleaseDate)
                {

                    if (myhosting.Diary[i.Month, i.Day] == true)
                    {
                        throw new Exception("The dates are not available for this hosting unit");
                    }
                    i = i.AddDays(1);

                }
                dal.AddOder(or);
            }

        }

        public void AddOder(Order or)
        {
            throw new NotImplementedException();
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
            if (or.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
            {
                // on met a jour la maarehet comme quoi c reserve 
                UpdateMatriceHostingunit(or);
                // on met a jour la guest req qui correspond a cette order
                GuestRequest guestreq = dal.GetGuestRequest(or.GuestRequestKey);
                UpdateGuestRequest(guestreq);
                var list = from item in dal.GetAllOrder()
                           where item.GuestRequestKey == guestreq.GuestRequestKey
                           select item;
                var result = from item in list
                             where item.Status == Enumeration.OrderStatus.NotAddressed // verifier si c'est ca ou sent email
                             select item;
                foreach (var item in result)
                {
                    item.Status = Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness;
                }

                //trouver qq chose pour bloquer
                throw new Exception("You can't change the status, it's already closed");
            }

            else if (or.Status == Enumeration.OrderStatus.NotAddressed)
            {
                // en gros on verfie que le host a recu un ichour comme quoi il peut prelever le client 
                // si oui en gros ca veut dire qu'on a paye alors la azmana elle est vraiment effectue 
                HostingUnit myhosting = dal.GetHostingUnit(or.HostingUnitKey);
                Host myHost = dal.GetHost(myhosting.Owner.HostKey);
                //DataSource.GetAllHost.FirstOrDefault(h => h.HostKey == myhosting.Owner.HostKey);
                if (myHost.CollectionClearance == true)
                {
                    // ichour azmana
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
            // changement du statut de la guest req sil a ete envoye par le order qui a ete sagour
            // + fermeture de tous les propositions concernant cette guest req
            if (gs.Status == Enumeration.GuestRequestStatus.Open)
            {
                Order myorder = dal.GetOrder(gs.GuestRequestKey);
                if (myorder.Status == Enumeration.OrderStatus.ClosedForCustomerResponse)
                {
                    gs.Status = Enumeration.GuestRequestStatus.ClosedThroughTheSite;


                }
            }

            dal.UpdateGuestRequest(gs);

        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            throw new NotImplementedException();
        }

        public void UpdateHost(Host h)
        {
            throw new NotImplementedException();
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

                myhosting.Diary[i.Month, i.Day] = true;
                i = i.AddDays(1);

            }

        }
    }
}
