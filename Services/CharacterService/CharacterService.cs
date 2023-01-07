using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(
            IMapper mapper,
            DataContext context,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() =>
            int.Parse(
                _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

        public async Task<ServiceReponse<List<GetCharacterDto>>> AddCharacter(
            AddCharacterDto newCharacter
        )
        {
            var serviceResponse = new ServiceReponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToList();
            return serviceResponse;
        }

        public async Task<ServiceReponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceReponse<List<GetCharacterDto>> response =
                new ServiceReponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == id && c.User!.Id == GetUserId()
                );
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Characters
                        .Where(c => c.User!.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceReponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceReponse = new ServiceReponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .ToListAsync();
            serviceReponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceReponse;
        }

        public async Task<ServiceReponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceReponse<GetCharacterDto>();
            var dbCharacters = await _context.Characters.FirstOrDefaultAsync(
                c => c.Id == id && c.User!.Id == GetUserId()
            );
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
                Character character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(
                    c => c.Id == updatedCharacter.Id
                );

                if(character is null || character.User!.Id != GetUserId()) {
                  throw new Exception($"Character with Id '{updatedCharacter.Id}'");
                }

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
