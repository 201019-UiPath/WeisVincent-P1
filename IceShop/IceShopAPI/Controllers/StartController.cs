using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceShopBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpGet("get")]
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


        // TODO: Does this even need to exist? maybe this can handle logins and sessions?
        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                var user = _startService.GetUserByEmail(email);

                return Ok(user);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



    }
}
