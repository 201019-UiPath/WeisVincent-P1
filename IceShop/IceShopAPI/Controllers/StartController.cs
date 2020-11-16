using IceShopBL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IceShopAPI.Controllers
{
    /// <summary>
    /// API controller that handles login information.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private readonly IStartService _startService;

        public StartController(IStartService startService)
        {
            _startService = startService;
        }

        /// <summary>
        /// Action that handles getting a user by email as a user, not specifically as a customer or manager.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
