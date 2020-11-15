using IceShopBL;
using IceShopDB.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using System.Collections.Generic;
using IceShopAPI.DTO;

namespace IceShopAPI.Controllers
{
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

        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetAllManagers()
        {
            try
            {
                var managers = _managerService.GetAllManagers();

                var mappedManagers = new List<ManagerDTO>();

                foreach(Manager manager in managers)
                {
                    var managerDTO = _mapper.Map<ManagerDTO>(manager);
                    mappedManagers.Add(managerDTO);
                }

                return Ok(mappedManagers);
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
                var foundManager = _managerService.GetManagerByEmail(email);

                var mappedManager = _mapper.Map<ManagerDTO>(foundManager);

                return Ok(mappedManager);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("add/{manager}")]
        [Consumes("application/json")]
        public IActionResult AddManager(ManagerDTO newManager)
        {
            try
            {
                var receivedManager = _mapper.Map<Manager>(newManager);
                _managerService.AddManager(receivedManager);

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
