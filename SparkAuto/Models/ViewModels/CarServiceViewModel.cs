using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkAuto.Models.ViewModels
{
    public class CarServiceViewModel
    {
        public List<ShoppingCart> ShoppingCart { get; set; }
        public List<ServiceType> ServiceType { get; set; }
        public Car Car { get; set; }
        public ServiceHeader ServiceHeader { get; set; }
        public ServiceDetails ServiceDetails { get; set; }
    }
}
