using System.ComponentModel.DataAnnotations;

namespace InternetMarket.Models.ViewModels.AccountViewModels
{
    public class LogginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
