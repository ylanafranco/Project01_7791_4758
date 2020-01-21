using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace BE
{
    public class Host
    {
        private int hostKey;
        public int HostKey { get { return hostKey; } 
            set {
                if (!(Tool.isValidID(value)))
                {
                    throw new ArgumentException(string.Format("This {0} is not valid", value));
                }
                hostKey = value;
            } }

        private string familyName;
        public string FamilyName
        {
            get { return familyName; }
            set
            {
                familyName = value;
            }
        }

        private string privateName;
        public string PrivateName
        {
            get { return privateName; }
            set { privateName = value; }
        }

        private string mailAddress;
        public string MailAddress
        {
            get { return mailAddress; }
            set
            {
                if (!(Tool.IsValidEmail(value)))
                    throw new FormatException("Email address must be a valid gmail address!");
                mailAddress = value;
            }
        }

        private int phoneNumber;
        public int PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (!(Tool.isValidNumber(value)))
                {
                    throw new ArgumentException(string.Format("This {0} is not valid.", value));
                }
                phoneNumber = value;

            }
        }

        private BankAccount bankAccount;
        public BankAccount BankAccount { get { return bankAccount; }  set { bankAccount = value; } }

        private int bankAccountNumber;
        public int BankAccountNumber { get { return bankAccountNumber; } set { bankAccountNumber = value; } }


        private bool collectionClearance;
        public bool CollectionClearance { get { return collectionClearance; } set { collectionClearance = value; } }

        public override string ToString()
        {
            string s = ("Host Details :\n" +
                "The Host Key : " + hostKey
                + ",\nName : " + familyName + " " + privateName
                + ",\nMail Address : " + mailAddress
                + "\nPhone Number :" + "0" +phoneNumber
                + "\nDetails Bank Account :" + BankAccount.ToString()
                + "\nBank Account Number : " + bankAccountNumber) ;
            return s;
        }
    }
}
