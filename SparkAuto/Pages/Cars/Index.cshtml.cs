using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models.ViewModels;

namespace SparkAuto.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //binding to our view model for car and cust
        [BindProperty]
        public CarAndCustomerViewModel CarAndCustVM { get; set; }

        //once page first created, inject our database into local _db
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGet(string userId = null)
        {
            if (userId == null)
            {
                //grab current log in
                //Casting as a claimsidentity to grab unique security token
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }

            CarAndCustVM = new CarAndCustomerViewModel()
            {
                Cars = await _db.Car
                                .Where(c => c.UserId == userId)
                                .ToListAsync(),
                UserObj = await _db.ApplicationUser
                                .FirstOrDefaultAsync(u => u.Id == userId)
            };
            return Page();
        }
    }
}