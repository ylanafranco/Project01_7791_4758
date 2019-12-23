using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankAccount
    {
        private int bankNumber;
        public int BankNumber { get { return bankNumber; } set { bankNumber = value; } }

        private string bankName;
        public string BankName { get { return bankName; } set { bankName = value; } }

        private int branchNumber;
        public int BranchNumber { get { return branchNumber; } set { branchNumber = value; } }

        private string branchAddress;
        public string BranchAddress { get { return branchAddress; } set { branchAddress = value; } }

        private string branchCity;
        public string BranchCity { get { return branchCity; } set { branchCity = value; } }

        public override string ToString()
        {
            string s = ("Bank Account Details :\n" +
                "Bank Name : " + bankName 
                + "\n Branch Number : " + branchNumber
                + "\nBank Number : " + bankName
                + "\nBranch Address : " + branchAddress + " , " + branchCity);
            return s;
        }


    }
}
