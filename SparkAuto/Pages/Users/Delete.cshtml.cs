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
    public class DeleteModel : PageModel
    {
        private ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public async Task OnGet(string id)
        {
            ApplicationUser = await _db.ApplicationUser.FindAsync(id);
        }

        public async Task<IActionResult> OnPost(string id)
        {
            var user = await _db.ApplicationUser.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _db.ApplicationUser.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}