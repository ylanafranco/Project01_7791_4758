using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    public static class Tools
    {
        public static IEnumerable<T> copy<T>(this IEnumerable<T> obj)
        {
            T[] arr = new T[obj.Count()];
            obj.ToList().CopyTo(arr);
            return arr.ToList();
        }
    }

    public class Dal_imp : IDAL
    {
        static DataSource DS = new DataSource();

        #region ADD FUNCTION
        public void AddGuestRequest(GuestRequest gs)
        {
            GuestRequest guestR = GetGuestRequest(gs.GuestRequestKey); 
            if (guestR != null) 
                throw new Exception("Guest Request already exists in the systeme...");
            DataSource.GetAllGuestRequest.Add(gs);
        }

        public void AddHost(Host h)
        {
            Host host = GetHost(h.HostKey);
            if (host != null)
                throw new Exception("Host with the same id already exists...");
            DataSource.GetAllHost.Add(h);
        }

        public void AddHostingUnit(HostingUnit hu)
        {
            HostingUnit hostU = GetHostingUnit(hu.HostingUnitKey);
            if (hostU != null)
                throw new Exception("This Hosting Unit already exists in the systeme...");
            DataSource.GetAllHostingUnitCollection.Add(hu);
        }

        public void AddOder(Order or)
        {
            Order order = GetOrder(or.OrderKey);
            if (order != null)
                throw new Exception("This order already exists in the systeme...");
            DataSource.GetAllOrder.Add(or);
        }
        #endregion

        #region ERASE FUNCTION
        public void EraseHost(int key)
        {
            Host host = GetHost(key);

            if (host == null)
                throw new Exception("This host is not found in the systeme...");
            
            DataSource.GetAllHostingUnitCollection.RemoveAll(hu => hu.Owner.HostKey == key);
            DataSource.GetAllHost.Remove(host);
        }

        public void EraseHostingUnit(int key)
        {
            HostingUnit hu = GetHostingUnit(key);
            if (hu == null)
                throw new Exception("This hosting unit is not found in the systeme...");
            DataSource.GetAllHostingUnitCollection.Remove(hu);
        }
        #endregion

        #region LIST FUNCTION
        public IEnumerable<int> GetAllBranch()
        {
            //if (predicate == null)
            //    return DataSource.GetAllBranch.AsEnumerable();
            //return DataSource.GetAllBranch.Where(predicate).copy();
            return new List<int> { 26, 7, 18, 55, 1 };
        }

        public IEnumerable<GuestRequest> GetAllGuestRequest(Func<GuestRequest, bool> predicate = null)
        {
            if (predicate == null) 
                return DataSource.GetAllGuestRequest.AsEnumerable();
            return DataSource.GetAllGuestRequest.Where(predicate).copy();
        }

        public IEnumerable<Host> GetAllHost(Func<Host, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.GetAllHost.AsEnumerable();
            return DataSource.GetAllHost.Where(predicate).copy();
        }

        public IEnumerable<HostingUnit> GetAllHostingUnitCollection(Func<HostingUnit, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.GetAllHostingUnitCollection.AsEnumerable();
            return DataSource.GetAllHostingUnitCollection.Where(predicate).copy();
        }

        public IEnumerable<Order> GetAllOrder(Func<Order, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.GetAllOrder.AsEnumerable();
            return DataSource.GetAllOrder.Where(predicate).copy();
        }
        #endregion

        #region GET FUNCTION
        public GuestRequest GetGuestRequest(int key)
        {
            return DataSource.GetAllGuestRequest.copy().FirstOrDefault(gs => gs.GuestRequestKey == key);
        }

        public Host GetHost(int key)
        {
            return DataSource.GetAllHost.copy().FirstOrDefault(h => h.HostKey == key);
        }

        public HostingUnit GetHostingUnit(int key)
        {
            return DataSource.GetAllHostingUnitCollection.copy().FirstOrDefault(hu => hu.HostingUnitKey == key);
        }

        public Order GetOrder(int key)
        {
            return DataSource.GetAllOrder.copy().FirstOrDefault(or => or.OrderKey == key);
        }
        #endregion

        #region UPDATE FUNCTION
        public void UpdateGuestRequest(GuestRequest gs)
        {
            int index = DataSource.GetAllGuestRequest.FindIndex(guestr => guestr.GuestRequestKey ==gs.GuestRequestKey); 
            if (index == -1) 
                throw new Exception("Guest Request is not found...");
            DataSource.GetAllGuestRequest[index] = gs;
        }

        public void UpdateHost(Host h)
        {
            int index = DataSource.GetAllHost.FindIndex(host => host.HostKey == h.HostKey);
            if (index == -1)
                throw new Exception("This Host is not found...");
            DataSource.GetAllHost[index] = h;
        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            int index = DataSource.GetAllHostingUnitCollection.FindIndex(hostinhu => hostinhu.HostingUnitKey == hu.HostingUnitKey);
            if (index == -1)
                throw new Exception("This Hosting unit is not found...");
            DataSource.GetAllHostingUnitCollection[index] = hu;
        }

        public void UpdateOrder(Order or)
        {
            int index = DataSource.GetAllOrder.FindIndex(order => order.OrderKey == or.OrderKey);
            if (index == -1)
                throw new Exception("This order is not found...");
            DataSource.GetAllOrder[index] = or;
        }
        #endregion
    }
}
