using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Models.ViewModels;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Users
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        //list for multiple users
        [BindProperty]
        public UsersListViewModel UsersListViewModel { get; set; }

        public async Task<IActionResult> OnGet(int userPage = 1, string searchEmail = null, string searchName = null, string searchPhone = null)
        {
            //constructor
            UsersListViewModel = new UsersListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Users?userPage=:");
            param.Append("&searchName=");

            if (searchName != null)
            {
                param.Append(searchName);
                UsersListViewModel.ApplicationUserList = await _db.ApplicationUser
                   .Where(u => u.Name.ToLower()
                   .Contains(searchName.ToLower())).ToListAsync();
            }

            param.Append("&searchEmail=");

            if (searchEmail != null)
            {
                param.Append(searchEmail);
                UsersListViewModel.ApplicationUserList = await _db.ApplicationUser
                    .Where(u => u.Email.ToLower()
                    .Contains(searchEmail.ToLower())).ToListAsync();
            }

            param.Append("&searchPhone=");

            if (searchPhone != null)
            {
                param.Append(searchPhone);
                UsersListViewModel.ApplicationUserList = await _db.ApplicationUser
                    .Where(u => u.PhoneNumber.ToLower()
                    .Contains(searchPhone.ToLower())).ToListAsync();
            }

            //if (searchEmail != null)
            //{
            //    UsersListViewModel.ApplicationUserList = await _db.ApplicationUser
            //        .Where(u => u.Email.ToLower()
            //        .Contains(searchEmail.ToLower())).ToListAsync();
            //}
            //if (searchName != null)
            //{
            //    UsersListViewModel.ApplicationUserList = await _db.ApplicationUser
            //        .Where(u => u.Name.ToLower()
            //        .Contains(searchName.ToLower())).ToListAsync();
            //}
            //if (searchPhone != null)
            //{
            //    UsersListViewModel.ApplicationUserList = await _db.ApplicationUser
            //        .Where(u => u.PhoneNumber.ToLower()
            //        .Contains(searchPhone.ToLower())).ToListAsync();
            //}

            var count = UsersListViewModel.ApplicationUserList.Count;

            UsersListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = userPage,
                ItemsPerPage = SD.PaginationUserPageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

            UsersListViewModel.ApplicationUserList = UsersListViewModel.ApplicationUserList
                .OrderBy(u => u.Email)
                .Skip((userPage - 1) * SD.PaginationUserPageSize)
                .Take(SD.PaginationUserPageSize).ToList();

            return Page();
        }
    }
}