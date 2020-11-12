using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IceShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetAllManagers()
        {
            try
            {
                return Ok(_managerService.GetAllManagers());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpGet("get/{email}")]
        [Produces("application/json")]
        public IActionResult GetManagerByEmail(string email)
        {
            try
            {
                return Ok(_managerService.GetManagerByEmail(email));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("add/{manager}")]
        [Consumes("application/json")]
        public IActionResult AddManager(Manager newManager)
        {
            try
            {
                _managerService.AddManager(newManager);
                return CreatedAtAction("AddManager", newManager);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }


    }
}
