namespace WeaponMobileAPI.Models
{
    public class UpdateWeaponDTO
    {
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
