using System.ComponentModel.DataAnnotations;

namespace GLMS.Models
{
    public class ManageUserRolesViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }
    public class UserRolesViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
