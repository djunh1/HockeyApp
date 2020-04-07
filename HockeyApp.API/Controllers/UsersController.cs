using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HockeyApp.API.Data;
using HockeyApp.API.dtos;
using HockeyApp.API.helpers;
using HockeyApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // Can use this action filter anytime this controller is accessed
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
        public async Task<IActionResult> getUsers([FromQuery]UserParams userParams)
        {
            
           // For Filtering
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.PlayerPosition)){
                userParams.PlayerPosition = userFromRepo.PlayerPosition == "Forward" ? "Defence" : "Forward";
            }
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            // Passing this information in the response header
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
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

        // FOLLOW 6/9
        [HttpPost("{id}/follow/{recipientId}")]
        public async Task<IActionResult> FollowUser(int id, int recipientId){

            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var follow = await _repo.GetFollow(id, recipientId);

            if ( follow != null){
                return BadRequest("You are already following this user.");
            }

            if (await _repo.GetUser(recipientId) == null) {
                return NotFound();
            }

            follow = new Follow {
                FollowerId = id,
                FollowedId = recipientId
            };

            _repo.Add<Follow>(follow);

            if ( await _repo.SaveAll()){
                return Ok();
            }

            return BadRequest("Failed to follow user.");

        }

    }
}