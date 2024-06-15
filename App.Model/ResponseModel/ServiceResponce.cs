using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.ResponseModel
{
    public class ServiceResponce<T>
    {
        public bool IsSuccess { get; set; }
        public string? message { get; set; }
        public T? Data { get; set; }


    }
}
