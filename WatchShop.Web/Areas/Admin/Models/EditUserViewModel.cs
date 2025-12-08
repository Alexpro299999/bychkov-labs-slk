using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WatchShop.Web.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public IEnumerable<string> UserRoles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}