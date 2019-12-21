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
        public int hostKey;
        public int HostKey { get { return hostKey; } 
            set {
                if (!(Tool.isValidID(value)))
                {
                    throw new ArgumentException(string.Format("This {0} is not valid", value));
                }
                hostKey = value;
            } }

        public string familyName;
        public string FamilyName
        {
            get { return familyName.ToUpper(); }
            set
            {
                familyName = value;
            }
        }

        public string privateName;
        public string PrivateName
        {
            get { return privateName.ToUpper(); }
            set { privateName = value; }
        }

        public string mailAddress;
        public string MailAddress
        {
            get { return mailAddress; }
            set
            {
                if (!(Tool.IsValidEmail(value)))
                    throw new ArgumentException(string.Format("This {0} is not valid", value));
                mailAddress = value;
            }
        }

        public int phoneNumber;
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

        public BankAccount bankAccount;
        public BankAccount BankAccount { get; set; }

        public bool collectionClearance;
        public bool CollectionClearance { get { return collectionClearance; } set { collectionClearance = value; } }

        public override string ToString()
        {
            string s = ("The Host Request key is " + hostKey 
                + ",\nhis name is " + familyName + " " + privateName 
                + "\nand his mail adress is " + mailAddress);
            return s;
        }
    }
}
