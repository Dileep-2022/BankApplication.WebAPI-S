using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.RequestModel.AccountRequestModel
{
    public  class AmountTransactionRequest
    {
        public  string SenderAccountNumber { get; set; }

        public string RecieverAccountNumber { get; set; }

        public float Amount { get; set; }

    }
}
