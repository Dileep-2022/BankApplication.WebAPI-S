using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.RequestModel.Customer
{
    public class CustomerRequestModel
    {
        public string Name { get; set; } = null!;
        public DateTime Dateofbirth { get; set; }
        public string Address { get; set; } = null!;
        public string PanNumber { get; set; } = null!;
    }
}
