
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : Controller
{
    private readonly UserService userService = new UserService();
    private readonly UserDbContext _dbContext = new UserDbContext();

    [HttpPost("users")]
    public List<User> GetUsersByCriteria(
        [SwaggerRequestBody(Description = "sortBy and filterBy can be Name, Email, Age, empty string, or null. sortOrder can be asc, dsc, empty string, or null")]
        [Optional] UserCriteria criteria)
    {
        return userService.GetUsersByCriteria(criteria);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<User> GetUserById(Guid id)
    {
        User user = userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost("create")]
    public ActionResult CreateUser([FromBody] UserDto userDto)
    {
        var checkForSameEmail = _dbContext.Users.SingleOrDefault(u => u.Email == userDto.Email);

        if (checkForSameEmail != null)
        {
            return BadRequest(new ValidationResult($"Email {userDto.Email} is already in use."));
        }

        if (userDto.DateOfBirth.Equals(default(DateTime)))
        {
            return BadRequest();
        }

        userDto.Password = SecureHasher.Hash(userDto.Password);

        userService.CreateUser(userDto);
        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<int> UpdateUser(Guid id, [FromBody] UserDto userDto)
    {
        var checkForSameEmail = _dbContext.Users.SingleOrDefault(u => u.Email == userDto.Email);
        
        if (checkForSameEmail != null && checkForSameEmail.Id != id) 
        {   
            return BadRequest(new ValidationResult($"Email {userDto.Email} is already in use."));
        }

        if (userDto.DateOfBirth.Equals(default(DateTime)))
        {
            return BadRequest();
        }
        
        userDto.Password = SecureHasher.Hash(userDto.Password);
        int rowsAffected = userService.UpdateUser(id, userDto);

        if (rowsAffected == 0)
        {
            return NotFound();
        }

        return rowsAffected;
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<int> DeleteUser(Guid id)
    {
        if (userService.GetUserById(id) == null)
        {
            return NotFound();
        }
        
        return userService.DeleteUser(id);
    }
}