namespace TravelSite.Models.Roles
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
    }
}
