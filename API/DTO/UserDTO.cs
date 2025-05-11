using System;

namespace API.DTO;

public class UserDTO
{
        public required string UserName { get; set; }
        public required string DisplayName { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
        public bool EmailConfirmed { get; set; }
}
