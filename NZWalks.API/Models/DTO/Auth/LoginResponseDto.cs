using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Models.DTO {
    public class LoginResponseDto {
        // public IdentityUser? User { get; set; }

        public string Id { get; set; } = "";
        public string UserName { get; set; } = "";

        public string Token { get; set; } = "";
    }
}