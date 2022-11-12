using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using first_api.Data;
using first_api.Dtos.Character;
using Microsoft.EntityFrameworkCore;

namespace first_api.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceReponse<List<GetCharacterDto>>> AddCharacter(
            AddCharacterDto newCharacter
        )
        {
            var serviceResponse = new ServiceReponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Characters
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToList();
            return serviceResponse;
        }

        public async Task<ServiceReponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceReponse<List<GetCharacterDto>> reponse =
                new ServiceReponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstAsync(c => c.Id == id);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                reponse.Data = _context.Characters
                    .Select(c => _mapper.Map<GetCharacterDto>(c))
                    .ToList();
            }
            catch (Exception ex)
            {
                reponse.Success = false;
                reponse.Message = ex.Message;
            }

            return reponse;
        }

        public async Task<ServiceReponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var reponse = new ServiceReponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            reponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return reponse;
        }

        public async Task<ServiceReponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceReponse<GetCharacterDto>();
            var dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacters);
            return serviceResponse;
        }

        public async Task<ServiceReponse<GetCharacterDto>> UpdateCharacter(
            UpdateCharacterDto updatedCharacter
        )
        {
            ServiceReponse<GetCharacterDto> reponse = new ServiceReponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == updatedCharacter.Id
                );

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();

                reponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                reponse.Success = false;
                reponse.Message = ex.Message;
            }

            return reponse;
        }
    }
}
