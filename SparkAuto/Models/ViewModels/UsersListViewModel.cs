using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkAuto.Models.ViewModels
{
    public class UsersListViewModel
    {
        //view models allow multiple models to a page
        //attaching to two models
        public List<ApplicationUser> ApplicationUserList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
