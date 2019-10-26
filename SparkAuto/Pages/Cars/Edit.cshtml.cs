using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Cars
{
    public class EditModel : PageModel
    {
        private ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Car Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, string userId = null)
        {
            Car = await _db.Car.FirstOrDefaultAsync(m => m.Id == id);
            //Car = await _db.Car.FindAsync(Id);
            return Page();
        }

        //Post - create
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (ModelState.IsValid)
            {
                Car CarFromDb = await _db.Car.FirstOrDefaultAsync(m => m.Id == id);
                //var CarFromDb = await _db.Car.FindAsync(Id);
                CarFromDb.VIN = Car.VIN;
                CarFromDb.Make = Car.Make;
                CarFromDb.Model = Car.Model;
                CarFromDb.Style = Car.Style;
                CarFromDb.Year = Car.Year;
                CarFromDb.Miles = Car.Miles;
                CarFromDb.Color = Car.Color;   
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("Index", new { userId = Car.UserId });
        }
    }
}