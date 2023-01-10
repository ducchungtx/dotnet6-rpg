using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using first_api.Dtos.Fight;
using Microsoft.AspNetCore.Mvc;

namespace first_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceReponse<AttackResultDto>>> WeaponAttack(
            WeaponAttackDto request
        )
        {
            return Ok(await _fightService.WeaponAttack(request));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceReponse<AttackResultDto>>> SkillAttack(
            SkillAttackDto request
        )
        {
            return Ok(await _fightService.SkillAttack(request));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceReponse<FightResultDto>>> Fight(FightRequestDto request) {
          return Ok(await _fightService.Fight(request));
        }

        [HttpGet]
        public async Task<ActionResult<ServiceReponse<List<HighScoreDto>>>> GetHighScore() {
          return Ok(await _fightService.GetHighScore());
        }
    }
}
