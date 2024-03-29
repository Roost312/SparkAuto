﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Services
{
    public class HistoryModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public HistoryModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<ServiceHeader> ServiceHeader { get; set; }

        public string UserId { get; set; }

        public async Task<IActionResult> OnGet(int carId)
        {
            ServiceHeader = await _db.ServiceHeader.Include(c => c.Car).Include(s => s.Car.ApplicationUser).Where(u => u.CarId == carId).ToListAsync();
            UserId = _db.Car.Where(u => u.Id == carId).ToList().FirstOrDefault().UserId;

            return Page();
        }
    }
}