namespace Application.Responses
{
    public class AuthenticationToken
    {
        public string Token { get; set; }
        public AuthenticationToken(string token)
        {
            Token = token;
        }
    }
}
