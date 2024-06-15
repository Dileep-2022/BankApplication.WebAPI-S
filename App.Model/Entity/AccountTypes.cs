using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.Entity
{
    public class AccountTypes
    {
        [Key]
        public int Id { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }=true;
    }
}
