using App.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.ResponseModel
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType{ get; set; }
        public float Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string CustomerName {  get; set; }
    }
}
