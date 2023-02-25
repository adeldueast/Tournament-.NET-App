using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services
{
    public class SeatService : ISeatService
    {
        private readonly ApplicationDbContext _dbContext;

        public SeatService(ApplicationDbContext dbContext) => _dbContext = dbContext;
		
		/// <exception cref="EventNotFoundException"></exception>
        public async Task<List<SeatResponseDTO>> GetAllSeats(int eventId)
        {
            var e = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Seats)
                .ThenInclude(u =>u.User)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (e == null)
            {
				throw new EventNotFoundException();
            }

            List<SeatResponseDTO> seatsDtos = e.Seats.Select(e => new SeatResponseDTO
            {
                Id = e.Id,
                Prefix = e.Prefix,
                Position = e.Position,
                EventId = e.EventId,
                IsFree =  e.User == null

            }).ToList();

            return seatsDtos;
        }

		/// <exception cref="SeatDuplicateIdException"></exception>
		/// <exception cref="SeatDuplicatePositionException"></exception>
		/// <exception cref="SeatAlreadyExistException"></exception>
		/// <exception cref="SeatNotFoundException"></exception>
        public void Upsert(Event e, List<SeatUpsertDTO> seatsToUpsert)
        {

            var duplicateIds = seatsToUpsert.Where(sU => sU.Id != 0).Select(sU => sU.Id).Distinct().ToList();
            if (duplicateIds.Count != seatsToUpsert.Where(sU => sU.Id != 0).Count())
            {
                throw new SeatDuplicateIdException();
            }

            var seatsUpsertGroupedBy = seatsToUpsert.Where(s => s.Id == 0).GroupBy(s => s.Prefix);
            foreach (var group in seatsUpsertGroupedBy)
            {
                var groupPrefix = group.Key;

                //Check for duplicates, if yes, throw exception
                var anyDuplicate = group.GroupBy(seat => seat.Position).Any(position => position.Count() > 1);
                if (anyDuplicate)
                {
                    throw new SeatDuplicatePositionException();
                }
            }

            var toKeep = seatsToUpsert.Where(sU => sU.Id != 0).Select(sU => sU.Id).ToList();

            var query = e.Seats.Where(s => !toKeep.Contains(s.Id)).AsQueryable();

            var toDelete = query.ToList();
            //var toDeleteIndexes = query.Select((item, index) => index).ToArray();

            //_dbContext.Seats.RemoveRange(toDelete);

            foreach (var seatMemory in toDelete)
            {
                e.Seats.Remove(seatMemory);
            }
            //foreach (var index in toDeleteIndexes.OrderByDescending(i => i))
            //{
            //	eventInMemory.Seats.RemoveAt(index);
            //}

            foreach (var seat in seatsToUpsert)
            {
                var seatAlreadyExist = seat.Id == 0
                    ? e.Seats.Any(s => s.Prefix == seat.Prefix && s.Position == seat.Position)
                    : e.Seats.Any(s => s.Prefix == seat.Prefix && s.Position == seat.Position && s.Id != seat.Id);

                if (seatAlreadyExist)
                {
                    throw new SeatAlreadyExistException();
                }

                Seat upsertSeatDomain;

                //Create
                if (seat.Id == 0)
                {
                    upsertSeatDomain = new Seat()
                    {
                        Prefix = seat.Prefix,
                        Position = (int)seat.Position,
                        EventId = e.Id,
                    };
                    _dbContext.Seats.Add(upsertSeatDomain);

                }
                else
                {
                    //Update, so fecthing seat to change and checking if exists
                    upsertSeatDomain = e.Seats.FirstOrDefault(s => s.Id == seat.Id);

                    if (upsertSeatDomain == null)
                    {
						throw new SeatNotFoundException();
					}

                    upsertSeatDomain.Prefix = seat.Prefix;
                    upsertSeatDomain.Position = (int)seat.Position;

                    _dbContext.Seats.Update(upsertSeatDomain);
                }

            }

        }

		/// <exception cref="SeatNotFoundException"></exception>
		/// <exception cref="UserNotFoundException"></exception>
		/// <exception cref="UserNotOwnerOfSeatException"></exception>
		/// <exception cref="UserNotInEventException"></exception>
		/// <exception cref="SeatAlreadyReservedException"></exception>
        public async Task ReserveSeat(int seatId, string userId)
        {
            var seat = await _dbContext.Seats.Include(s => s.Event).ThenInclude(e => e.Players).FirstOrDefaultAsync(s => s.Id == seatId);
            if (seat == null)
            {
				throw new SeatNotFoundException();

            }

            var user = await _dbContext.Users.Include(u => u.Seats).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }


            if (seat.User != null && seat.User != user)
            {
				throw new UserNotOwnerOfSeatException();
            }

            //list object 1
            //list object 2


            if (!(seat.Event.Players.Any(u => u.Id == user.Id)))
            {
                throw new UserNotInEventException();
            }

            //check if user already has a seat in existing event

            foreach (Seat s in user.Seats.ToList())
            {
                if (s.EventId == seat.EventId)
                {
                    if (s.Position == seat.Position && s.Prefix == seat.Prefix)
                    {
                        throw new SeatAlreadyReservedException();
                    }
                    user.Seats.Remove(s);

                }
            }
            user.Seats.Add(seat);

            await _dbContext.SaveChangesAsync();
        }

		/// <exception cref="UserNotFoundException"></exception>
		/// <exception cref="SeatNotFoundException"></exception>
        public async Task RemoveSeat(int seatId, string userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Seats)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
				throw new UserNotFoundException();
            }

            //Check if provided seat is assigned to user
            var seatToRemove = user.Seats.FirstOrDefault(s => s.Id == seatId);
            if (seatToRemove == null)
            {
                throw new SeatNotFoundException();
            }
            //Unlink the relation
            user.Seats.Remove(seatToRemove);

            await _dbContext.SaveChangesAsync();

            //Todo: tester la method, jai pas testé 😈🤣🤣🤣🤣🤣
        }

       
    }

    public interface ISeatService
    {
        public  Task<List<SeatResponseDTO>> GetAllSeats(int eventId);
        public void  Upsert(Event e, List<SeatUpsertDTO> seatsToUpsert);
        public  Task ReserveSeat(int seatId, string userId);
        public  Task RemoveSeat(int seatId, string userId);

    }
}
