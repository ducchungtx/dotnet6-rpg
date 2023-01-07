using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using first_api.Dtos.Character;
using first_api.Dtos.Weapon;
using first_api.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace first_api.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class WeaponController : ControllerBase
  {
    private readonly IWeaponService _weaponService;

    public WeaponController(IWeaponService weaponService)
    {
      _weaponService = weaponService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceReponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon) {
      return Ok(await _weaponService.AddWeapon(newWeapon));
    }
  }
}