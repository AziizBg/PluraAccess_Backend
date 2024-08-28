using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OddoBhf.Dto.User
{
    public class LoginDto
    {

        [DefaultValue("example@oddo-bhf.com")]
        [Required]
        public string? Email { get; set; }
        [DefaultValue("12345678")]
        [Required]
        public string? Password { get; set; }

    }
}
