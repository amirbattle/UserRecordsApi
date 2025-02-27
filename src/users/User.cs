
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    public Guid Id { get; set;}
    
    [Required]
    [StringLength(100, ErrorMessage = "Max amount of characters is 100")]
    public String Name { get; set; }

    [Required]
    [EmailAddress]
    public String Email { get; set; }

    [Required]
    [MinAge(18)]
    public DateTime DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    [Required]
    public String Password { get; set; }

    public User(Guid id, String name, String email, DateTime dateOfBirth, DateTime createdAt, DateTime updatedAt, String password) {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.DateOfBirth = dateOfBirth;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
        this.Password = password;
    }

    public User()
    {
    }
}