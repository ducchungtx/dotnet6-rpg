using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using first_api.Dtos.Character;
using first_api.Dtos.Weapon;

namespace first_api.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceReponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}