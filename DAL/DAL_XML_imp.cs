using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using DS;

namespace DAL
{
    class DAL_XML_imp : IDAL
    {
        
        XElement ConfigRoot;
        string ConfigPath = @"Config.xml";
        string GuestRequestPath = @"GuestReq.xml";
        XElement HostRoot;
        string HostPath = @"HostUnit.xml";
        string HostingUnitPath = @"HostingUnit.xml";
        string OrderPath = @"Order.xml";

        XElement BankRoot;
        string BankPath = @"atm.xml";
        BackgroundWorker bwrk = new BackgroundWorker();

        private static DAL_XML_imp instance = null;
        public static DAL_XML_imp getDal_XML()
        {
            if (instance == null)
            {
                instance = new DAL_XML_imp();
            }
            return instance;
        }

        public DAL_XML_imp()
        {
            try
            {
                if (!(File.Exists(ConfigPath)))
                {
                    CreateConfig();
                    LoadConfig();
                }
                else LoadConfig();
                if (!(File.Exists(GuestRequestPath)))
                {
                    SaveToXML<GuestRequest>(DS.DataSource.GetAllGuestRequest, GuestRequestPath);

                }
                else DS.DataSource.GetAllGuestRequest = (LoadFromXML<GuestRequest>(GuestRequestPath));
                if (!(File.Exists(OrderPath)))
                {
                     SaveToXML<Order>(DS.DataSource.GetAllOrder, OrderPath);


                }
                else DS.DataSource.GetAllOrder = (LoadFromXML<Order>(OrderPath));
                if (!(File.Exists(HostPath)))
                {
                    CreateHost();
                }
                else LoadDataForHost();
                if (!(File.Exists(HostingUnitPath)))
                {
                   SaveToXML <HostingUnit>(DataSource.GetAllHostingUnitCollection, HostingUnitPath);
                }
                else DS.DataSource.GetAllHostingUnitCollection = (LoadFromXML<HostingUnit>(HostingUnitPath));
                bwrk.DoWork += bwrk_DoWork;
                bwrk.RunWorkerAsync();


            }
            catch (Exception)
            {

                throw new Exception("File Upload Problem");
            }
        }

        private void bwrk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                LoadBankData();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

        private void CreateHost()
        {
            HostRoot = new XElement("hosts");
            HostRoot.Save(HostPath);
        }

        private void CreateConfig()
        {
            XElement NumGuestRequest = new XElement("NumStaticGuestRequest", 10000000);
            XElement NumHostingUnit = new XElement("NumStaticHostingUnit", 10000000);
            XElement NumOrder = new XElement("NumStaticOrder", 1);
            XElement Commission = new XElement("Commission", 10);
            XElement Mail = new XElement("MAIL", "befitbynana@gmail.com");
            XElement Password = new XElement("MAIL_PASSWORD", "ylanaetclara");
            ConfigRoot = new XElement("Configuration", NumGuestRequest, NumHostingUnit, NumOrder, Commission, Mail, Password);
            ConfigRoot.Save(ConfigPath);
        }

        private void LoadConfig()
        {
            ConfigRoot = XElement.Load(ConfigPath);
        }

        #region BANK ACCOUNT
        const string xmlLocalPath = @"atm.xml";
        WebClient wc = new WebClient();
        void LoadBankData()
        {
            if (!File.Exists(BankPath))
            {
                try
                {
                    string xmlServerPath = @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                    wc.DownloadFile(xmlServerPath, xmlLocalPath);

                }
                catch (Exception)
                {
                    string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                    wc.DownloadFile(xmlServerPath, xmlLocalPath);
                }
                finally
                {
                    wc.Dispose();
                }
            }

            BankRoot = XElement.Load(xmlLocalPath);
        }

        public IEnumerable<BankAccount> GetAllBranch()
        {
            try
            {
                if (BankRoot == null)
                {
                    throw new InvalidOperationException("List is charging");

                }
                var x = (from item in BankRoot.Elements()
                         select ConvertBank(item));
                return x;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
        }

        private BankAccount ConvertBank(XElement element)
        {
            BankAccount account = new BankAccount();
            account.BankName = element.Element("שם_בנק").Value;
            account.BankNumber = int.Parse(element.Element("קוד_בנק").Value);
            account.BranchNumber = int.Parse(element.Element("קוד_סניף").Value);
            account.BranchAddress = element.Element("כתובת_ה-ATM").Value;
            account.BranchCity = element.Element("ישוב").Value;
            return account;
        }
        #endregion

        private void LoadDataForHost()
        {
            HostRoot = XElement.Load(HostPath);

        }

        public static void SaveToXML<T>(List<T> list, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }

        public static List<T> LoadFromXML<T>(string path)
        {
            if (File.Exists(path))
            {
                List<T> list;
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                FileStream file = new FileStream(path, FileMode.Open);
                list = (List<T>)x.Deserialize(file);
                file.Close();
                return list;
            }
            else return new List<T>();
        }

        #region Host

        private XElement ConvertHost(Host host)
        {
            XElement hostElement = new XElement("host");
            hostElement.Add(new XElement("HostKey", host.HostKey));
            hostElement.Add(new XElement("FamilyName", host.FamilyName));
            hostElement.Add(new XElement("PrivateName", host.PrivateName));
            hostElement.Add(new XElement("Mail", host.MailAddress));
            hostElement.Add(new XElement("PhoneNumber", host.PhoneNumber));
            hostElement.Add(new XElement("BankAccountDetails", new XElement("BankNumber", host.BankAccount.BankNumber), new XElement("BankName", host.BankAccount.BankName), new XElement("BranchNumber", host.BankAccount.BranchNumber), new XElement("BranchAddress", host.BankAccount.BranchAddress), new XElement("BranchCity", host.BankAccount.BranchCity)));
            hostElement.Add(new XElement("BankAccountNumber", host.BankAccountNumber));
            hostElement.Add(new XElement("CollectionClearance", host.CollectionClearance));
            return hostElement;

        }

        private Host ConvertHost(XElement element)
        {
            Host host = new Host();
            host.BankAccount = new BankAccount();
            host.HostKey = int.Parse(element.Element("HostKey").Value);
            host.FamilyName = element.Element("FamilyName").Value;
            host.PrivateName = element.Element("PrivateName").Value;
            host.MailAddress = element.Element("Mail").Value;
            host.PhoneNumber = int.Parse(element.Element("PhoneNumber").Value);
            host.BankAccount.BankName = element.Element("BankAccountDetails").Element("BankName").Value;
            host.BankAccount.BankNumber = int.Parse(element.Element("BankAccountDetails").Element("BankNumber").Value);
            host.BankAccount.BranchAddress = element.Element("BankAccountDetails").Element("BranchAddress").Value;
            host.BankAccount.BranchCity = element.Element("BankAccountDetails").Element("BranchCity").Value;
            host.BankAccount.BranchNumber = int.Parse(element.Element("BankAccountDetails").Element("BranchNumber").Value);
            host.BankAccountNumber = int.Parse(element.Element("BankAccountNumber").Value);
            host.CollectionClearance = bool.Parse(element.Element("CollectionClearance").Value);
            return host;
        }

        public void AddHost(Host host)
        {
            
            try
            {
                if (GetHost(host.HostKey) != null)
                {
                    throw new InvalidOperationException("Guest Request already exists in the systeme...");
                }
                Host myhost = new Host();
                myhost = host;
                HostRoot.Add(ConvertHost(host));
                HostRoot.Save(HostPath);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("BE Problem :" + ex.Message);
            }
            catch (FormatException ex1)
            {
                throw new FormatException("BE Problem :" + ex1.Message);
            }

        }

        public void UpdateHost(Host host)
        {

            XElement toUpdate = (from item in HostRoot.Elements()
                                 where int.Parse(item.Element("HostKey").Value) == host.HostKey
                                 select item).FirstOrDefault();
            if (toUpdate == null)
                throw new KeyNotFoundException("The host is not found...");

            XElement UpdateHost = ConvertHost(host);
            toUpdate.ReplaceNodes(UpdateHost.Elements());
            HostRoot.Save(HostPath);
        }

        public void EraseHost(int key)
        {
            XElement toRemove = (from item in HostRoot.Elements()
                             where int.Parse(item.Element("HostKey").Value) == key
                             select item).FirstOrDefault();

            
            if (toRemove == null)
            {
                throw new InvalidOperationException("This host is not found in the systeme...");
            }
            DataSource.GetAllHostingUnitCollection = LoadFromXML<HostingUnit>(HostingUnitPath);
            DataSource.GetAllHostingUnitCollection.RemoveAll(hu => hu.Owner.HostKey == key);
            SaveToXML<HostingUnit>(DataSource.GetAllHostingUnitCollection, HostingUnitPath);
            toRemove.Remove();
            HostRoot.Save(HostPath);
        }

        public Host GetHost(int key)
        {
            XElement host = null;
            try
            {
                host = (from item in HostRoot.Elements()
                        where int.Parse(item.Element("HostKey").Value) == key
                        select item).FirstOrDefault();
            }
            catch
            {
                return null;
            }

            if (host == null)
                return null;

            return ConvertHost(host);
        }

        public IEnumerable<Host> GetAllHost(Func<Host, bool> predicate = null)
        {
            if (predicate == null)
            {
                return from item in HostRoot.Elements()
                       select ConvertHost(item);
            }
            return from item in HostRoot.Elements()
                   let x = ConvertHost(item)
                   where predicate(x)
                   select x;

        }

        #endregion

        #region GuestRequest
        public void AddGuestRequest(GuestRequest gs)
        {
            DataSource.GetAllGuestRequest = (LoadFromXML<GuestRequest>(GuestRequestPath));
            XElement ConfigElement = XElement.Load(ConfigPath);
            long myval = long.Parse(ConfigElement.Element("NumStaticGuestRequest").Value);
            gs.GuestRequestKey = myval;
            myval++;
            ConfigElement.Element("NumStaticGuestRequest").Value = (myval).ToString();
            ConfigElement.Save(ConfigPath);

            GuestRequest gs1 = DataSource.GetAllGuestRequest.FirstOrDefault(g => g.GuestRequestKey == gs.GuestRequestKey);
            if (gs1 != null)
            {
                throw new Exception("DAL: GuestRequest with the same guestrequest key already exist...");

            }
            
            DataSource.GetAllGuestRequest.Add(gs);
            SaveToXML(DataSource.GetAllGuestRequest, GuestRequestPath);
        }

        public void UpdateGuestRequest(GuestRequest gs)
        {
            DataSource.GetAllGuestRequest = (LoadFromXML<GuestRequest>(GuestRequestPath));
            int index = DataSource.GetAllGuestRequest.FindIndex(t => t.GuestRequestKey == gs.GuestRequestKey);
            if (index == -1)
            {
                throw new Exception("DAL: Guest request with the same key is not found...");
            }
            DataSource.GetAllGuestRequest[index] = gs;
            SaveToXML(DataSource.GetAllGuestRequest, GuestRequestPath);
        }

        public GuestRequest GetGuestRequest(long key)
        {
            DataSource.GetAllGuestRequest = LoadFromXML<GuestRequest>(GuestRequestPath);
            return DataSource.GetAllGuestRequest.FirstOrDefault(gs => gs.GuestRequestKey == key);
        }


        public IEnumerable<GuestRequest> GetAllGuestRequest(Func<GuestRequest, bool> predicate = null)
        {
            if (predicate == null)
                return LoadFromXML<GuestRequest>(GuestRequestPath).AsEnumerable();
            return LoadFromXML<GuestRequest>(GuestRequestPath).Where(predicate);
        }

        #endregion

        #region HostingUnit
        public void AddHostingUnit(HostingUnit hu)
        {
            DataSource.GetAllHostingUnitCollection = (LoadFromXML<HostingUnit>(HostingUnitPath));
            XElement ConfigElement = XElement.Load(ConfigPath);
            long myval = long.Parse(ConfigElement.Element("NumStaticHostingUnit").Value);
            hu.HostingUnitKey = myval;
            myval++;
            ConfigElement.Element("NumStaticHostingUnit").Value = (myval).ToString();
            ConfigElement.Save(ConfigPath);
            
            HostingUnit h1 = DataSource.GetAllHostingUnitCollection.FirstOrDefault(h => h.HostingUnitKey == hu.HostingUnitKey);
            
            if (h1 != null)
            {
                throw new Exception("DAL: HostingUnit with the same hostingunit key  already exists...");
            }

            
            DataSource.GetAllHostingUnitCollection.Add(hu);
            
            SaveToXML<HostingUnit>(DataSource.GetAllHostingUnitCollection, HostingUnitPath);

        }

        public void EraseHostingUnit(long Key)
        {
            DataSource.GetAllHostingUnitCollection = LoadFromXML<HostingUnit>(HostingUnitPath);
            HostingUnit myhosting = GetHostingUnit(Key);
            DataSource.GetAllHostingUnitCollection.Remove(myhosting);
            SaveToXML<HostingUnit>(DataSource.GetAllHostingUnitCollection, HostingUnitPath);

        }

        public void UpdateHostingUnit(HostingUnit hu)
        {
            DataSource.GetAllHostingUnitCollection = (LoadFromXML<HostingUnit>(HostingUnitPath));
            int index = DataSource.GetAllHostingUnitCollection.FindIndex(t => t.HostingUnitKey == hu.HostingUnitKey);
            if (index == -1)
            {
                throw new Exception("DAL: Hosting unit with the same hostingunit key not found...");
            }
            DataSource.GetAllHostingUnitCollection[index] = hu;
            SaveToXML(DataSource.GetAllHostingUnitCollection, HostingUnitPath);
        }

        public HostingUnit GetHostingUnit(long key)
        {
            DataSource.GetAllHostingUnitCollection = LoadFromXML<HostingUnit>(HostingUnitPath);
            return DataSource.GetAllHostingUnitCollection.FirstOrDefault(hu => hu.HostingUnitKey == key);
        }

        public IEnumerable<HostingUnit> GetAllHostingUnitCollection(Func<HostingUnit, bool> predicate = null)
        {
            if (predicate == null)//if there isnt condition we return all list that was converted to enumerable from file
                return LoadFromXML<HostingUnit>(HostingUnitPath).AsEnumerable();
            return LoadFromXML<HostingUnit>(HostingUnitPath).Where(predicate);
        }

        #endregion

        #region Order
        public void AddOder(Order or)
        {
            DataSource.GetAllOrder = (LoadFromXML<Order>(OrderPath));
            XElement ConfigElement = XElement.Load(ConfigPath);
            int myval = int.Parse(ConfigElement.Element("NumStaticOrder").Value);
            or.OrderKey = myval;
            myval++; 
            ConfigElement.Element("NumStaticOrder").Value = (myval).ToString();
            ConfigElement.Save(ConfigPath);

            Order order = DataSource.GetAllOrder.FirstOrDefault(o => o.OrderKey == or.OrderKey);
            if (order != null)
            {
                throw new Exception("DAL: Order with the same order key  already exist...");
            }

            DataSource.GetAllOrder.Add(or);
            SaveToXML<Order>(DS.DataSource.GetAllOrder, OrderPath);

        }

        public void UpdateOrder(Order or)
        {
            DataSource.GetAllOrder = (LoadFromXML<Order>(OrderPath));
            int index = DataSource.GetAllOrder.FindIndex(t => t.OrderKey == or.OrderKey);
            if (index == -1)
            {
                throw new Exception("DAL: Order with the same order  key not found...");
            }
            DataSource.GetAllOrder[index] = or;
            SaveToXML<Order>(DS.DataSource.GetAllOrder, OrderPath);

        }

        public Order GetOrder(int key)
        {
            DataSource.GetAllOrder = LoadFromXML<Order>(OrderPath);
            return DataSource.GetAllOrder.FirstOrDefault(or => or.HostingUnitKey == key);

        }

        public IEnumerable<Order> GetAllOrder(Func<Order, bool> predicate = null)
        {
            if (predicate == null)//if there isnt condition we return all list that was converted to enumerable from file
                return LoadFromXML<Order>(OrderPath).AsEnumerable();
            return LoadFromXML<Order>(OrderPath).Where(predicate);
        }

        #endregion




    }
}
