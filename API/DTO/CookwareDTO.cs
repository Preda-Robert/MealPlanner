using System;

namespace API.DTO;

public class CookwareDTO
{

        public required string Name { get; set; }
        public required string Description { get; set; }
        public PhotoDTO? Photo { get; set; } = null!;
}
