using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using System.Collections.Generic;
using IceShopAPI.DTO;
using System.Threading.Tasks;

namespace IceShopAPI.Controllers
{
    /// <summary>
    /// API controller that handles manager information, which includes getting a list of all managers, getting a manager by email, and adding a new manager.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService, IMapper mapper)
        {
            _managerService = managerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Action that handles getting a list of all managers.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllManagers()
        {
            try
            {
                var getManagers = Task.Factory.StartNew(() => { return _managerService.GetAllManagers(); });
                var managers = await getManagers;

                var mapAllManagers = new List<Task<ManagerDTO>>();

                foreach(Manager manager in managers)
                {
                    var mapManager = Task.Factory.StartNew(() => { return _mapper.Map<ManagerDTO>(manager); });
                    mapAllManagers.Add(mapManager);
                }

                var mappedManagers = await Task.WhenAll(mapAllManagers);

                return Ok(mappedManagers);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Action that handles getting a manager by their associated email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("get/{email}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetManagerByEmail(string email)
        {
            try
            {
                var findManager = Task.Factory.StartNew(() => { return _managerService.GetManagerByEmail(email); });

                var foundManager = await findManager;

                var mappedManager = _mapper.Map<ManagerDTO>(foundManager);

                return Ok(mappedManager);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Action that handles adding a manager.
        /// </summary>
        /// <param name="newManager"></param>
        /// <returns></returns>
        [HttpPost("add/{manager}")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddManager(ManagerDTO newManager)
        {
            try
            {
                var receivedManager = _mapper.Map<Manager>(newManager);

                var addManager = Task.Factory.StartNew( () => _managerService.AddManager(receivedManager));

                await addManager;

                return CreatedAtAction("AddManager", receivedManager);
            }
            catch (Exception)
            {
                // TODO: Check if this is the right error code for this.
                return StatusCode(500);
            }
        }


    }
}
