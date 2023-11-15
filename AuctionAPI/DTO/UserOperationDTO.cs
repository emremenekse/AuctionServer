namespace AuctionAPI.DTO
{
    public class UserOperationDTO
    {

        public string Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsNewUser{ get; set; }
    }
}
