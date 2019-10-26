using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Car Car { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet(string userId = null)
        {
            Car = new Car();
            if (userId == null)
            {
                //grab current log in
                //Casting as a claimsidentity to grab unique security token
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }
            Car.UserId = userId;
            return Page();
        }

        //Post - create
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //the bindproperty defines the servicetype being added
            _db.Car.Add(Car);
            await _db.SaveChangesAsync();
            StatusMessage = "Car has been added successfully.";
            return RedirectToPage("Index", new { userId = Car.UserId });
        }

    }
}