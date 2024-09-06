using LokatyWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LokatyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly LokatyContext _LokatyDbContext;

        public DepositController(LokatyContext lokatyDbContext)
        {
            _LokatyDbContext = lokatyDbContext;
        }

        [HttpGet]
        [Route("GetDeposits")]
        public async Task<IEnumerable<Deposit>> GetDeposits()
        {
            return await _LokatyDbContext.Deposits.Include(d => d.Client).ToListAsync();
        }

        [HttpPost]
        [Route("AddDeposit")]
        public async Task<ActionResult<Deposit>> AddDeposit(Deposit deposit)
        {
            // Check if the provided ClientId exists
            var existingClient = await _LokatyDbContext.Clients.FindAsync(deposit.ClientId);

            // If the client does not exist, return a NotFound response
            if (existingClient == null)
            {
                return NotFound($"Client with ID {deposit.ClientId} not found");
            }

            // Assign the existing client to the deposit
            deposit.Client = existingClient;

            // Add the deposit to the context and save changes
            _LokatyDbContext.Deposits.Add(deposit);
            await _LokatyDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeposits), new { id = deposit.DepositId }, deposit);
        }


        [HttpPatch]
        [Route("UpdateDeposit/{id}")]
        public async Task<ActionResult<Deposit>> UpdateDeposit(int id, Deposit deposit)
        {
            if (id != deposit.DepositId)
            {
                return BadRequest("Invalid ID");
            }

            var existingDeposit = await _LokatyDbContext.Deposits.FindAsync(id);
            if (existingDeposit == null)
            {
                return NotFound();
            }

            existingDeposit.Amount = deposit.Amount;
            existingDeposit.InterestRate = deposit.InterestRate;
            existingDeposit.DurationMonths = deposit.DurationMonths;
            existingDeposit.EstimatedProfit = deposit.EstimatedProfit;
            existingDeposit.ClientId = deposit.ClientId;

            try
            {
                await _LokatyDbContext.SaveChangesAsync();
                return Ok(existingDeposit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_LokatyDbContext.Deposits.Any(d => d.DepositId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteDeposit/{id}")]
        public async Task<ActionResult<Deposit>> DeleteDeposit(int id)
        {
            var deposit = await _LokatyDbContext.Deposits.FindAsync(id);
            if (deposit == null)
            {
                return NotFound();
            }

            _LokatyDbContext.Deposits.Remove(deposit);
            await _LokatyDbContext.SaveChangesAsync();

            return Ok(deposit);
        }
    }
}
