using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    public interface IBL
    {
        #region GuestRequestFonction
        void AddGuestRequest(GuestRequest gs);
        void UpdateGuestRequest(GuestRequest gs);
        GuestRequest GetGuestRequest(int key);
        #endregion

        #region HostFonction
        void AddHost(Host h);
        void UpdateHost(Host h);
        void EraseHost(int key);
        Host GetHost(int key);
        #endregion

        #region HostingUnitFonction
        void AddHostingUnit(HostingUnit hu);
        void EraseHostingUnit(int Key);
        void UpdateHostingUnit(HostingUnit hu);
        HostingUnit GetHostingUnit(int key);
        #endregion

        #region OrderFonction
        void AddOrder(Order or);
        void UpdateOrder(Order or);
        Order GetOrder(int key);
        #endregion

        #region List
        IEnumerable<HostingUnit> GetAllHostingUnitCollection(Func<HostingUnit, bool> predicate = null);
        IEnumerable<Host> GetAllHost(Func<Host, bool> predicate = null);
        IEnumerable<Order> GetAllOrder(Func<Order, bool> predicate = null);
        IEnumerable<GuestRequest> GetAllGuestRequest(Func<GuestRequest, bool> predicate = null);
        IEnumerable<int> GetAllBranch(Func<int, bool> predicate = null);
        #endregion

        #region GROUPING
        IGrouping<Enumeration.Area, GuestRequest> GetGuestReqGroupByArea(bool sorted = false);
        IGrouping<int, GuestRequest> GetGuestRequestGroupByPersons(bool sorted = false);
        IGrouping<int, Host> GetHostGroupByNumofHU(bool sorted = false);
        IGrouping<Enumeration.Area, HostingUnit> GetHUGroupByArea(bool sorted = false);
        #endregion

        void UpdateMatriceHostingunit(Order or);
        void MatriceUptoZero(HostingUnit HU);
        void UpdateOrderAndGuestReq(Order or);
        bool CheckMatrice(DateTime EntryD, DateTime ReleaseD, HostingUnit myhosting);
        void initMatrice(HostingUnit HU);

        int CommissionCost(GuestRequest guestreq);
        double PriceOftheStay(Order or);
        double Reduction(Order or);
        int DifferenceDays(DateTime[] list);

        List<HostingUnit> HostingUnitPerHost(Host h);
        List<Order> OrderSelonHostingUnit(List<HostingUnit> listHU);
        List<HostingUnit> HostingUnitFree(DateTime EntryD, int num);
        List<Order> OrderSelonTime(int num);

        int NumOfPropositionGR(GuestRequest guestreq);
        int NumofPropositionHU(HostingUnit hu);
        void NumTotalPersonGR(GuestRequest gs);

        void testYourChance();
        bool checkCollectionClearance(Host H);
    }
}
