using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Cars
{
    public class DeleteModel : PageModel
    {
        private ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Car Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, string userId = null)
        {
            Car = await _db.Car.FirstOrDefaultAsync(m => m.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (Car == null)
            {
                return NotFound();
            }

            Car = await _db.Car.FirstOrDefaultAsync(m => m.Id == id);

            _db.Car.Remove(Car);

            await _db.SaveChangesAsync();
            return RedirectToPage("Index", new { userId = Car.UserId });
        }
    }
}