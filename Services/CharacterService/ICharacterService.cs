using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using first_api.Dtos.Character;

namespace first_api.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceReponse<List<GetCharacterDto>>> GetAllCharacters();
        Task<ServiceReponse<GetCharacterDto>> GetCharacterById(int id);
        Task<ServiceReponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceReponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
        Task<ServiceReponse<List<GetCharacterDto>>> DeleteCharacter(int id);
        Task<ServiceReponse<GetCharacterDto>> AddCharacterSkill(
            AddCharacterSkillDto newCharacterSkill
        );
    }
}
