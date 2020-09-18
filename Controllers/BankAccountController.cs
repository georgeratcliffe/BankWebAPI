using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using BankWebAPI.ViewModels;

namespace BankWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly APIDBContext _context;

        public BankAccountController(APIDBContext context)
        {
            _context = context;
        }

        // GET: api/BankAccount
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            return await _context.BankAccounts.Where(ba => ba.UserID == userId).ToListAsync();
        }

        //GET: api/BankAccount/GetAllBankAccounts
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllBankAccounts")]
        public async Task<ActionResult<IEnumerable<BankAccountViewModel>>> GetAllBankAccounts()
        {
            var rawlist = await _context.BankAccounts.Include(ba => ba.Bank).ToListAsync();

            var list = from ba in rawlist
                       select new BankAccountViewModel
                       {
                           BankAccountID = ba.BankAccountID,
                           AccountNumber = ba.AccountNumber,
                           AccountHolder = ba.AccountHolder,
                           IFSC = ba.IFSC,
                           Bank = ba.Bank.BankName
                       };

            return new List<BankAccountViewModel>(list);
        }

        // GET: api/BankAccount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return bankAccount;
        }

        // PUT: api/BankAccount/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, BankAccount bankAccount)
        {
            if (id != bankAccount.BankAccountID)
            {
                return BadRequest();
            }

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bankAccount.UserID = userId;

            _context.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BankAccount
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bankAccount.UserID = userId;

            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankAccount", new { id = bankAccount.BankAccountID }, bankAccount);
        }

        // DELETE: api/BankAccount/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BankAccount>> DeleteBankAccount(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();

            return bankAccount;
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.BankAccountID == id);
        }
    }
}
