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
        private readonly ISeatService _seatService;
        private readonly IPictureService _pictureService;

        public EventService(ApplicationDbContext dbContext, ISeatService seatService, IPictureService pictureService)
        {
            _dbContext = dbContext;
            _seatService = seatService;
            _pictureService = pictureService;
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

		/// <exception cref="EventNameTakenException"></exception>
		/// <exception cref="EventInvalidDatesException"></exception>
		/// <exception cref="EventNotFoundException"></exception>
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

		/// <exception cref="EventNotFoundException"></exception>
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

		/// <exception cref="EventNotFoundException"></exception>
		/// <exception cref="NoSeatsGivenException"></exception>
        public virtual async Task UpsertPlanLayout(SeatsUpsertDTO seatsUpsertDTO)
        {

            var e = await _dbContext.Events
                .Include(e => e.Seats)
                .Include(e => e.PlanPicture)
                .FirstOrDefaultAsync(e => e.Id == seatsUpsertDTO.EventId);

            if (e == null)
            {
                throw new EventNotFoundException(); 
            }

            //Must provide seats
            if (!seatsUpsertDTO.Seats.Any())
            {
                throw new NoSeatsGivenException();
            }

			bool isUpdate = e.Seats.Any() && e.PlanPicture != null;
			bool updateOnlySeat = seatsUpsertDTO.File == null;

			//Update: because seats AND picture already exist for Event
			if (isUpdate && updateOnlySeat)
            {
				_seatService.Upsert(e, seatsUpsertDTO.Seats);
				////Only update seats
				//if ()
    //            {
                    
    //            }
    //            //Only update photo (but seats are always included so 
    //            else
    //            {
    //                await _pictureService.PlanPictureUpsert(seatsUpsertDTO.File, e);
    //                await _seatService.Upsert(e, seatsUpsertDTO.Seats);
    //            }

            }
            //Create: because seats AND picture both dont exist, so we create 
            else
            {
                await _pictureService.PlanPictureUpsert(seatsUpsertDTO.File, e);
                _seatService.Upsert(e, seatsUpsertDTO.Seats);
            }

            await _dbContext.SaveChangesAsync();

        }

		/// <exception cref="EventNotFoundException"></exception>
		/// <exception cref="UserNotFoundException"></exception>
		/// <exception cref="UserAlreadyJoinedEventException"></exception>
		/// <exception cref="EventMaxPlayersNumberReachedException"></exception>
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
        public  Task UpsertPlanLayout(SeatsUpsertDTO seatsUpsertDTO);
        public  Task JoinEvent(string userId, int eventId);
    }
}
