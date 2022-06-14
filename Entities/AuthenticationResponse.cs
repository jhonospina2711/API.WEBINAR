namespace Entities
{
    public class AuthenticationResponse
    {
        public bool Authenticated { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public bool IsFirstLogin { get; set; }
        public decimal InitialCoins { get; set; }
    }
}