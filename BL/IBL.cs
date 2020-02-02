using BE;
using System;
using System.Collections;
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
        GuestRequest GetGuestRequest(long key);
        #endregion

        #region HostFonction
        void AddHost(Host h);
        void UpdateHost(Host h);
        void EraseHost(int key);
        Host GetHost(int key);
        #endregion

        #region HostingUnitFonction
        void AddHostingUnit(HostingUnit hu);
        void EraseHostingUnit(long Key);
        void UpdateHostingUnit(HostingUnit hu);
        HostingUnit GetHostingUnit(long key);
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
        IEnumerable<BankAccount> GetAllBranch();
        IEnumerable<string> GetBankName();
        #endregion

        #region GROUPING
        IEnumerable<IGrouping<int, Host>> GetHostGroupByNumofHU(IEnumerable<Host> hosts);
        IEnumerable<IGrouping<Enumeration.Area, GuestRequest>> GetGuestReqGroupByArea(IEnumerable<GuestRequest> guestReq);
        IEnumerable<IGrouping<int, GuestRequest>> GetGuestRequestGroupByPersons(IEnumerable<GuestRequest> guestReq);
        IEnumerable<IGrouping<Enumeration.Area, HostingUnit>> GetHUGroupByArea(IEnumerable<HostingUnit> hostingunits);
        #endregion

        void UpdateMatriceHostingunit(Order or);
        void MatriceUptoZero(HostingUnit HU);
        void UpdateOrderAndGuestReq(Order or);
        bool CheckMatrice(GuestRequest mygr, HostingUnit myhosting);
        void initMatrice(bool[,] arr);

        int CommissionCost(GuestRequest guestreq);
        double PriceOftheStay(Order or);
        double Reduction(Order or);
        int DifferenceDays(DateTime[] list);

        List<HostingUnit> HostingUnitPerHost(Host h);
        List<Order> OrderSelonHostingUnit(List<HostingUnit> listHU);
        //List<HostingUnit> HostingUnitFree(DateTime EntryD, int num);
        List<Order> OrderSelonTime(int num);
        List<Order> OrderSelonGuestRequest(GuestRequest gr);

        int NumOfPropositionGR(GuestRequest guestreq);
        int NumofPropositionHU(HostingUnit hu);
        int NumTotalPersonGR(int a, int b);

        bool testYourChance(int mynum);
        bool checkCollectionClearance(Host H);
        bool comparaison(GuestRequest gs, HostingUnit hu);

        IEnumerable<int> GetBranchNumbers(string BankName);
        BankAccount GetMyBranch(int branchnum, string bankname);
    }
}
