using System.ComponentModel.DataAnnotations;

public class RegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

}