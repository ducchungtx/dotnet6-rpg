using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using first_api.Dtos.Fight;

namespace first_api.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceReponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceReponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
    }
}
