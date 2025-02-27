
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserDto
{    
    [Required]
    [StringLength(100, ErrorMessage = "Max amount of characters is 100")]
    public String Name { get; set; }

    [Required]
    [EmailAddress]
    public String Email { get; set; }

    [Required]
    [MinAge(18)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public String Password { get; set; }

    public UserDto(String name, String email, DateTime dateOfBirth, String password) {
        this.Name = name;
        this.Email = email;
        this.DateOfBirth = dateOfBirth;
        this.Password = password;
    }
}