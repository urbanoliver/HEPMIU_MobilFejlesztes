using System.ComponentModel.DataAnnotations;

namespace WeaponMobileApp.Services
{
    public class UpdateWeaponDTO
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A gyártó megadása kötelező.")]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "Az ár megadása kötelező.")]
        [Range(0, double.MaxValue, ErrorMessage = "Az árnak pozitívnak kell lennie.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "A mennyiség megadása kötelező.")]
        [Range(0, int.MaxValue, ErrorMessage = "A mennyiségnek nem negatív egész számnak kell lennie.")]
        public int Quantity { get; set; }
    }
}