using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Users
{
    public class EditModel : PageModel
    {
   
        private ApplicationDbContext _db;
      
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task OnGet(string id)
        {
            ApplicationUser = await _db.ApplicationUser.FindAsync(id);
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var UserFromDb = await _db.ApplicationUser.FindAsync(ApplicationUser.Id);
                UserFromDb.Name = ApplicationUser.Name;
                UserFromDb.PhoneNumber = ApplicationUser.PhoneNumber;
                UserFromDb.Address = ApplicationUser.Address;
                UserFromDb.City = ApplicationUser.City;
                UserFromDb.PostalCode = ApplicationUser.PostalCode;

                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}