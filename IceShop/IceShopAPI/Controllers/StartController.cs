using IceShopBL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IceShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private readonly IStartService _startService;

        public StartController(IStartService startService)
        {
            _startService = startService;
        }

        [HttpGet("get")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _startService.GetUserByEmailAsync(email);
                
                return Ok(user);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }




    }
}
