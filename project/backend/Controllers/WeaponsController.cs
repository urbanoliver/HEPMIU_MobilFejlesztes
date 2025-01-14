using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeaponMobileAPI.Data;
using WeaponMobileAPI.Models;


namespace MobileWeaponsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WeaponsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Weapon>>> GetWeapons()
        {
            var weapons = await _context.Weapons.ToListAsync();

            return weapons;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Weapon>> GetWeapon(int Id)
        {
            var weapon = await _context.Weapons.FindAsync(Id);
            if (weapon == null) return NotFound();
            return weapon;
        }

        [HttpPost]
        public async Task<ActionResult<Weapon>> CreateWeapon([FromBody] Weapon weapon)
        {
            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWeapon), new { Id = weapon.Id }, weapon);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteWeapon(int Id)
        {
            var weapon = await _context.Weapons.FindAsync(Id);
            if (weapon == null) return NotFound();

            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateWeapon(int Id, UpdateWeaponDTO updateWeaponDto)
        {

            var weapon = await _context.Weapons.FindAsync(Id);
            if (weapon == null)
            {
                return NotFound();
            }


            if (updateWeaponDto.Name != null)
            {
                weapon.Name = updateWeaponDto.Name;
            }

            if (updateWeaponDto.Manufacturer != null)
            {
                weapon.Manufacturer = updateWeaponDto.Manufacturer;
            }

            if (updateWeaponDto.Price != null)
            {
                weapon.Price = updateWeaponDto.Price;
            }

            if (updateWeaponDto.Quantity != null)
            {
                weapon.Quantity = updateWeaponDto.Quantity;
            }


            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
