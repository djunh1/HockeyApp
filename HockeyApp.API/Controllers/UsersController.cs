using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HockeyApp.API.Data;
using HockeyApp.API.dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRinkRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IRinkRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> getUsers()
        {
            var users = await _repo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")] 
        public async Task<IActionResult> getUser(int id)
        {
            var user = await _repo.GetUser(id);
            // The mapper does not know how to make between the user and the DTOs, so we create the
            // mapping in a new class.
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        //STEP 3- Updating on API (Next SPA, UserService)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto){
            // Check token ID
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            // Get the user from the Repo
            var userFromRepo = await _repo.GetUser(id);

            // Update values from DTO and upate them on the repo user
            _mapper.Map(userForUpdateDto, userFromRepo);

            //Save changes

            if (await _repo.SaveAll()){
                return NoContent();
            } 

            throw new Exception($"Updatating user with {id} failed while saving.");

        }

    }
}