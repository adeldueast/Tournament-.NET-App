using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services
{
    public class EventService :IEventService
    {
        private readonly ApplicationDbContext _dbContext;

        public EventService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<EventResponseDTO>> GetAllEvents(string userId)
        {
            var events = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Players)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            List<EventResponseDTO> eventsDtos = events.Select(e => new EventResponseDTO
            {
                Id = e.Id,
                Name = e.Name,
                MaxPlayerNumber = e.MaxPlayerNumber,
                EndDate = e.EndDate,
                StartDate = e.StartDate,
                Description = e.Description,
                Joined = string.IsNullOrEmpty(userId) ? false : e.Players.Any(p => p.Id == userId)

            }).ToList();

            return eventsDtos;
        }

		/// <exception cref="EventNotFoundException"></exception>
        public async Task<EventResponseDTO> GetEvent(int id, string userId)
        {
            var existingEvent = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Players)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingEvent == null)
            {
                throw new EventNotFoundException();
            }

            EventResponseDTO existingEventDto = new EventResponseDTO
            {
                Id = existingEvent.Id,
                Name = existingEvent.Name,
                MaxPlayerNumber = existingEvent.MaxPlayerNumber,
                EndDate = existingEvent.EndDate,
                StartDate = existingEvent.StartDate,
                Description = existingEvent.Description,
                Joined = string.IsNullOrEmpty(userId) ? false : existingEvent.Players.Any(p => p.Id == userId)
            };

            return existingEventDto;
        }

        public async Task<EventResponseDTO> Upsert(EventUpsertDTO eventToUpsert)
        {

            //Check if NAME is already taken..
            var eventAlreadyExist = eventToUpsert.Id == 0
                ? await _dbContext.Events.AnyAsync(e => e.Name == eventToUpsert.Name)
                : await _dbContext.Events.AnyAsync(e => e.Name == eventToUpsert.Name && e.Id != eventToUpsert.Id);

            if (eventAlreadyExist)
            {
                throw new EventNameTakenException();
            }


            //Validate logic before creating the ressource.
            if (DateTime.Compare(eventToUpsert.StartDate.Value, eventToUpsert.EndDate.Value) > 0)
            {
                throw new EventInvalidDatesException();
            }

            Event upsertEventDomain;

            //Create
            if (eventToUpsert.Id == 0)
            {
                //If not, create object
                upsertEventDomain = new Event()
                {
                    Name = eventToUpsert.Name,
                    MaxPlayerNumber = eventToUpsert.MaxPlayerNumber,
                    Description = eventToUpsert.Description,
                    StartDate = (DateTime)eventToUpsert.StartDate,
                    EndDate = (DateTime)eventToUpsert.EndDate,
                };
                _dbContext.Events.Add(upsertEventDomain);
            }
            else
            {
                //Update

                //Check if Event exist (by Id)..
                upsertEventDomain = await _dbContext.Events.FindAsync(eventToUpsert.Id);
                if (upsertEventDomain == null)
                {
					throw new EventNotFoundException();

				}

                //Update the tracked entity.
                upsertEventDomain.Name = eventToUpsert.Name;
                upsertEventDomain.MaxPlayerNumber = eventToUpsert.MaxPlayerNumber;
                upsertEventDomain.Description = eventToUpsert.Description;
                upsertEventDomain.StartDate = (DateTime)eventToUpsert.StartDate;
                upsertEventDomain.EndDate = (DateTime)eventToUpsert.EndDate;

            }


            await _dbContext.SaveChangesAsync();

            EventResponseDTO newEventDto = new EventResponseDTO
            {
                Id = upsertEventDomain.Id,
                Name = upsertEventDomain.Name,
                MaxPlayerNumber = upsertEventDomain.MaxPlayerNumber,
                EndDate = upsertEventDomain.EndDate,
                StartDate = upsertEventDomain.StartDate,
                Description = upsertEventDomain.Description,
            };

            return newEventDto;

        }

        public async Task DeleteEvent(int eventId)
        {
            var eventToDelete = await _dbContext.Events.FindAsync(eventId);

            if (eventToDelete == null)
            {
				throw new EventNotFoundException();
            }

            _dbContext.Events.Remove(eventToDelete);
            await _dbContext.SaveChangesAsync();

        }

       

        public async Task JoinEvent(string userId, int eventId)
        {
            var eventToJoin = await _dbContext.Events
                .Include(eJ => eJ.Players)
                .FirstOrDefaultAsync(eJ => eJ.Id == eventId);

            if (eventToJoin == null)
            {
				throw new EventNotFoundException();
            }

            var joiningPlayer = await _dbContext.Users.FindAsync(userId);
            if (joiningPlayer == null)
            {
                throw new UserNotFoundException();
            }

            var userAlreadyJoined = eventToJoin.Players.Any(p => p.Id == userId);
            if (userAlreadyJoined)
            {
				throw new UserAlreadyJoinedEventException();
            }

            if (eventToJoin.Players.Count == eventToJoin.MaxPlayerNumber)
            {
                throw new EventMaxPlayersNumberReachedException();
            }

            eventToJoin.Players.Add(joiningPlayer);
            _dbContext.SaveChanges();

        }
    }

    public interface IEventService
    {
        public  Task<List<EventResponseDTO>> GetAllEvents(string userId);
        public  Task<EventResponseDTO> GetEvent(int id, string userId);
        public  Task<EventResponseDTO> Upsert(EventUpsertDTO eventToUpsert);
        public  Task DeleteEvent(int eventId);
        public  Task JoinEvent(string userId, int eventId);
    }
}
