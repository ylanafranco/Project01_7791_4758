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
        public int HostKey { get; set; }
        public string FamilyName { get; set; }
        public string PrivateName { get; set; }
        public MailAddress MailAddress { get; set; }
        public int PhoneNumber { get; set; }
        public BankAccount BankAccount { get; set; }
        public bool CollectionClearance { get; set; }
        public override string ToString()
        {
            string s = ("The Host Request key is " + HostKey 
                + " , and the host name is " + FamilyName + " " + PrivateName 
                + " and his mail adress is " + MailAddress);
            return s;
        }
    }
}
