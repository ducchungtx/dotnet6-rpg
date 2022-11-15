using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using first_api.Dtos.Character;
using first_api.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace first_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceReponse<List<GetCharacterDto>>>> Get()
        {
            int userId = int.Parse(
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value
            );
            return Ok(await _characterService.GetAllCharacters(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceReponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceReponse<List<GetCharacterDto>>>> Delete(int id)
        {
            var reponse = await _characterService.DeleteCharacter(id);
            if (reponse.Data == null)
            {
                return NotFound(reponse);
            }
            return Ok(reponse);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceReponse<List<GetCharacterDto>>>> AddCharacter(
            AddCharacterDto newCharacter
        )
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceReponse<GetCharacterDto>>> UpdateCharacter(
            UpdateCharacterDto updatedCharacter
        )
        {
            var reponse = await _characterService.UpdateCharacter(updatedCharacter);
            if (reponse.Data == null)
            {
                return NotFound(reponse);
            }
            return Ok(reponse);
        }
    }
}
