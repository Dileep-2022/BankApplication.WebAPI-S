using App.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.RequestModel.Account
{
    public class AccountRequest
    {
        public int CustomerId { get; set; }
        public int AccountTypeId { get; set; }
        public float Balance { get; set; } = 500;

    }
}
