using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankAccount
    {
        public int BankNumber { get; set; }
        public string BankName { get; set; }
        public int BranchNumber { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public int BankAccountNumber { get; set; }
        public int NumOfPayment { get; set; }
        public override string ToString()
        {
            string s = ("the bank name is " + BankName 
                + " , the bank account number is " + BankAccountNumber
                + " , and the branch number is " + BranchNumber);
            return s;
        }


    }
}
