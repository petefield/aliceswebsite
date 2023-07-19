public class LoginResponse
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}