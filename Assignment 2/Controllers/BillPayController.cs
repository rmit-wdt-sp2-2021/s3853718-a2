﻿using Assignment_2.Authorise;
using Assignment_2.Data;
using Assignment_2.Models;
using Assignment_2.ViewModels;
using DataValidator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_2.Controllers
{
    public class BillPayController : Controller
    {
        private readonly Assignment2DbContext _context;
        public BillPayController(Assignment2DbContext context) => _context = context;

        public async Task<IActionResult> List()
        {
            var customer = await _context.Customers.FindAsync(HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value);
            //var billpay = await _context.BillPays.FromSqlRaw(@"select b.* from BillPay b where AccountNumber").ToListAsync();
            var billpay = await _context.BillPays.Where(x => x.AccountNumber == customer.Accounts[0].AccountNumber).ToListAsync();
            return View(billpay);
        }
        public IActionResult PayeeList()
        {
            var payees = _context.Set<Payee>().ToList();
            return View(payees);
        }

        public IActionResult NewPayee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewPayee([Bind("PayeeID,Name,Address,Suburb,State,PostCode,Phone")] PayeeViewModel payeeModel)
        {
            if (ModelState.IsValid)
            {
                var payee = new Payee
                {
                    Name = payeeModel.Name,
                    Address = payeeModel.Address,
                    Suburb = payeeModel.Suburb,
                    State = Enum.Parse<AusStates>(payeeModel.State),
                    PostCode = payeeModel.PostCode,
                    Phone = payeeModel.Phone,
                };
                _context.Add(payee);
                 await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PayeeList));
            }
            return View(payeeModel);
        }
    }
}