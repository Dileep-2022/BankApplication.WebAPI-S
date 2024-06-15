using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.Entity
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Dateofbirth { get; set; }
        public string Address { get; set; } = null!;
        public string PanNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive {  get; set; }=true;
        
    }
}
