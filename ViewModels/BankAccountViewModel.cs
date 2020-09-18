using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebAPI.ViewModels
{
    public class BankAccountViewModel
    {
        public int BankAccountID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string IFSC { get; set; }
        public string Bank { get; set; }
    }
}
