using System.ComponentModel.DataAnnotations.Schema;

namespace WeaponMobileAPI.Models
{
    [Table("Weapons", Schema = "dwh")]
    public class Weapon
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
