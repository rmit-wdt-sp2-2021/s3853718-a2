﻿using Assignment_2.Data;
using Assignment_2.Models;
using Assignment_2.ViewModels;
using DataValidator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Assignment_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Assignment2DbContext _context;

        public HomeController(ILogger<HomeController> logger, Assignment2DbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            var customer = await _context.Customers.FindAsync(2100);
            return View(customer);
        }
        public async Task<IActionResult> Deposit(int accountNumber)
        {
            return View(
                new SingleAccountTransactionViewModel
                {
                    AccountNumber = accountNumber,
                    Account = await _context.Accounts.FindAsync(accountNumber)
                });
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(SingleAccountTransactionViewModel viewModel)
        {
            viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);
            if (!viewModel.Amount.ToString().IsDollarAmount())
            {
                ModelState.AddModelError(nameof(viewModel.Amount), "Enter a dollar amount");
                return View(viewModel);
            }


            viewModel.Account.Balance += viewModel.Amount;
            viewModel.Account.Transactions.Add(
                new Transaction
                {
                    TransactionType = TransactionType.D,
                    Amount = viewModel.Amount,
                    Comment = viewModel.Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                    
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Withdraw(int accountNumber)
        {
            return View(
                new SingleAccountTransactionViewModel
                {
                    AccountNumber = accountNumber,
                    Account = await _context.Accounts.FindAsync(accountNumber)
                });
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(SingleAccountTransactionViewModel viewModel)
        {
            viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);
            if (!viewModel.Amount.ToString().IsDollarAmount())
            {
                ModelState.AddModelError(nameof(viewModel.Amount), "Enter a dollar amount");
                return View(viewModel);
            }
            decimal fees = AccountChecks.GetATMFee();
            if (viewModel.Account.FreeTransactions > 0)
            {
                fees = 0;
                viewModel.Account.FreeTransactions -= 1;
            }

            if (viewModel.Account.Balance - viewModel.Amount - fees < 0)
            {
                ModelState.AddModelError(nameof(viewModel.Amount), "Insufficient Funds for Withdrawal");
                return View(viewModel);
            }


            viewModel.Account.Balance = viewModel.Account.Balance - viewModel.Amount - fees;
            viewModel.Account.Transactions.Add(
            new Transaction
            {
                TransactionType = TransactionType.W,
                Amount = viewModel.Amount,
                Comment = viewModel.Comment,
                TransactionTimeUtc = DateTime.UtcNow
            });
            if (fees > 0)
                viewModel.Account.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.S,
                    Amount = fees,
                    Comment = "Withdrawal Service Fee",
                    TransactionTimeUtc = DateTime.UtcNow
                });


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
