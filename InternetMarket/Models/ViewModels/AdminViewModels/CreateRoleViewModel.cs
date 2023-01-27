using System.ComponentModel.DataAnnotations;

namespace InternetMarket.Models.ViewModels.AdminViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleManager { get; set; }
    }
}
