using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.RequestModel.AccountRequestModel
{
    public class AccountCreditDebitRequest
    {
        public string AccountNumber { get; set; }

        public float Amount { get; set; }
    }
}
