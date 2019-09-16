using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.ServiceTypes
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ServiceType ServiceType { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        //Post - create
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            //the bindproperty defines the servicetype being added
            _db.ServiceType.Add(ServiceType);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }

    }
}