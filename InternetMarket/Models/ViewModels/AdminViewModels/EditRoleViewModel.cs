using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetMarket.Models.ViewModels.AdminViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "Error in role name")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
