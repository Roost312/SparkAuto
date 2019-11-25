using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Models.ViewModels;

namespace SparkAuto.Pages.Services
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public CarServiceViewModel CarServiceVM { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGet(int CarId)
        {
            CarServiceVM = new CarServiceViewModel
            {
                Car = await _db.Car.Include(c => c.ApplicationUser)
                    .FirstOrDefaultAsync(c => c.Id == CarId),
                ServiceHeader = new Models.ServiceHeader()
            };

            List<String> ListServiceTypeInShoppingCart = _db.ShoppingCart
                .Include(c => c.ServiceType)
                .Where(c => c.CarId == CarId)
                .Select(c => c.ServiceType.Name)
                .ToList();

            IQueryable<ServiceType> ListServices =
                from s in _db.ServiceType
                where !(ListServiceTypeInShoppingCart.Contains(s.Name))
                select s;

            CarServiceVM.ServiceType = ListServices.ToList();

            CarServiceVM.ShoppingCart =
                _db.ShoppingCart
                    .Include(c => c.ServiceType)
                    .Where(c => c.CarId == CarId)
                    .ToList();

            CarServiceVM.ServiceHeader.TotalPrice = 0;

            foreach (var item in CarServiceVM.ShoppingCart)
            {
                CarServiceVM.ServiceHeader.TotalPrice += item.ServiceType.Price;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCart()
        {
            ShoppingCart objShoppingCart = new ShoppingCart
            {
                CarId = CarServiceVM.Car.Id,
                ServiceTypeId = CarServiceVM.ServiceDetails.ServiceTypeId
                
            };
            _db.ShoppingCart.Add(objShoppingCart);
            await _db.SaveChangesAsync();
            return RedirectToPage("Create", new { carId = CarServiceVM.Car.Id });
        }

        public async Task<IActionResult> OnPostRemoveFromCart(int serviceTypeId)
        {
            ShoppingCart objShoppingCart = _db.ShoppingCart.FirstOrDefault(u => u.CarId == CarServiceVM.Car.Id && u.ServiceTypeId == serviceTypeId);
            _db.ShoppingCart.Remove(objShoppingCart);
            await _db.SaveChangesAsync();
            return RedirectToPage("Create", new { carId = CarServiceVM.Car.Id });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CarServiceVM.ServiceHeader.DateAdded = DateTime.Now;
            CarServiceVM.ShoppingCart = _db.ShoppingCart.Include(c => c.ServiceType).Where(u => u.CarId == CarServiceVM.Car.Id).ToList();

            CarServiceVM.ServiceHeader.CarId = CarServiceVM.Car.Id;
            _db.ServiceHeader.Add(CarServiceVM.ServiceHeader);
            await _db.SaveChangesAsync();

            foreach(var detail in CarServiceVM.ShoppingCart)
            {
                ServiceDetails serviceDetails = new ServiceDetails
                {
                    ServiceHeaderId = CarServiceVM.ServiceHeader.Id,
                    ServiceName = detail.ServiceType.Name,
                    ServicePrice = detail.ServiceType.Price,
                    ServiceTypeId = detail.ServiceTypeId
                };
                _db.ServiceDetail.Add(serviceDetails);
            }
            // erase the shopping cart
            _db.ShoppingCart.RemoveRange(CarServiceVM.ShoppingCart);
            await _db.SaveChangesAsync();
            return RedirectToPage("../Cars/Index", new { UserId = CarServiceVM.Car.UserId });
        }

    }
}