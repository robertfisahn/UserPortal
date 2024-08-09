using System.ComponentModel.DataAnnotations;

namespace UserPortal.Models.Dtos
{
    public class UpdateUserDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
