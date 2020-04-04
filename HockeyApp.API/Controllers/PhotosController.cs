using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HockeyApp.API.Data;
using HockeyApp.API.dtos;
using HockeyApp.API.helpers;
using HockeyApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HockeyApp.API.Controllers
{
    // Step 4 - CLOUD STORAGE - Want to authorize, create the rote, and make it an API
    // (next create PhotoForCreationDto  )
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        // need a repository, the mapper, and Cloudinary settings (From the helper)
        private readonly IRinkRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IRinkRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            // Create the cloudinary account per the docs.
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        // Step 6 CLOUD STORAGE, named get task to get the photo (next update the interface IRink)
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id){
            var photoFromRepo = await _repo.GetPhoto(id);
            // We do not want to get all the params from the photo, so map to a new dto
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo)
;        }

        // Step 5 CLOUD STORAGE create the method to add the photo (next add another http request)
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto){

            // Check token ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            // Get the user from the Repo
            var userFromRepo = await _repo.GetUser(userId);

            // Need a var for the File
            var file = photoForCreationDto.File;

            // Need to store 
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0) {
                // Filestream so we must dispose file in memory once we finish calling this method.
                using(var stream = file.OpenReadStream()){
                    var uploadParams = new ImageUploadParams() {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            // Mapping from the Dto
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            // For the Main photo
            if(!userFromRepo.Photos.Any(u => u.IsMain)){
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if( await _repo.SaveAll()){
                // Return from HTTP Post
                // Created a get request and modify the IRinkRepository
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id}, photoToReturn);
            } 

            return BadRequest("We could not add the photo.");
            //next create the component for SPA

        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id){
            // Check token ID
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            // User updating own photo?
            var user = await _repo.GetUser(userId);
            // Does the ID match any id in the photos collection
            if (!user.Photos.Any(p => p.Id == id)){
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain){
                return BadRequest();
            }

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);

            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll()){
                return NoContent();
            }

            return BadRequest("Could not set the main photo.  Please try again. ");


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var user = await _repo.GetUser(userId);
    
            if (!user.Photos.Any(p => p.Id == id)){
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain){
                return BadRequest("Can not delete main photo.");
            }

            if (photoFromRepo.PublicID != null) {
                var deleteParams = new DeletionParams(photoFromRepo.PublicID);
                var result = _cloudinary.Destroy(deleteParams);

                 if (result.Result == "ok") {
                     _repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicID == null) {
                _repo.Delete(photoFromRepo);
            }
            
            if (await _repo.SaveAll()){
                return Ok();
            } 

            return BadRequest("Failed to delete the photo");
        }
    }
}