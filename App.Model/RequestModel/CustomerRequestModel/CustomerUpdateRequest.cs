using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.RequestModel.CustomerRequestModel
{
    public class CustomerUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Dateofbirth { get; set; }
        public string Address { get; set; } = null!;
        public string PanNumber { get; set; } = null!;
    }
}
