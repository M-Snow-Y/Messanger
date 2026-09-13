using System.ComponentModel.DataAnnotations;

namespace Messanger.Shared.Module
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string UserName {  get; set; } = string.Empty;
    }
}
