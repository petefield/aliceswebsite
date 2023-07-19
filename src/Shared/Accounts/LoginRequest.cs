public class LoginRequest
{
    public LoginRequest() : this(string.Empty, string.Empty)
    { 
    }

    public LoginRequest(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
}