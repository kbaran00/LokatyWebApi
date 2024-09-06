using LokatyWebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LokatyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly LokatyContext _LokatyDbContext;

        public ClientController(LokatyContext lokatyDbContext)
        {
            _LokatyDbContext = lokatyDbContext;
        }

        [HttpGet]
        [Route("GetClient")]
        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _LokatyDbContext.Clients.ToListAsync();
        }

        [HttpPost]
        [Route("AddClient")]
        public async Task<Client> AddClient(Client objClient)
        {
            _LokatyDbContext.Clients.Add(objClient);
            await _LokatyDbContext.SaveChangesAsync();
            return objClient;
        }

        [HttpPatch]
        [Route("UpdateClient/{id}")]
        public async Task<ActionResult<Client>> UpdateClient(int id, Client objClient)
        {
            if (id != objClient.ClientId)
            {
                return BadRequest("Invalid ID");
            }

            var existingClient = await _LokatyDbContext.Clients.FindAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.FirstName = objClient.FirstName;
            existingClient.LastName = objClient.LastName;
            existingClient.Email = objClient.Email;
            existingClient.Password = objClient.Password;
            existingClient.AccountBalance = objClient.AccountBalance;

            try
            {
                await _LokatyDbContext.SaveChangesAsync();
                return Ok(existingClient);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_LokatyDbContext.Clients.Any(c => c.ClientId == id))
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
        [Route("DeleteClient/{id}")]
        public bool DeleteClient(int id)
        {
            bool result = false;
            var client = _LokatyDbContext.Clients.Find(id);
            if (client != null)
            {
                result = true;
                _LokatyDbContext.Entry(client).State = EntityState.Deleted;
                _LokatyDbContext.SaveChanges();
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
