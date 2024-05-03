using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HenryMedsReservationAPI.Models;

namespace HenryMedsReservationAPI.Controllers
{
    [Route("clients/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationContext _reservationContext;

        public ReservationsController(ReservationContext reservationContext)
        {
            _reservationContext = reservationContext;
        }

        /// <summary>
        /// endpoint to get all reservations
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
        {
            return await _reservationContext.Reservation.ToListAsync();
        }
        
        /// <summary>
        /// endpoint for clients to make reservations
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation([FromBody]ReservationRequest reservationRequest)
        {
            /// check to ensure reservation must be made atleast 24 hrs in advance
            var timeSlot = GetTimeSlot(reservationRequest.TimeSlotId);

            if (timeSlot.StartTime < DateTime.Now.AddHours(24))
                return BadRequest("Reservations must be made at least 24 hrs in advance. Please choose a future reservation");

            /// set reservation requirement for 15 mins
            timeSlot.EndTime = timeSlot.StartTime.Value.AddMinutes(15);

            _reservationContext.Reservation.Add(new Reservation 
            {
                TimeSlotId = reservationRequest.TimeSlotId,
                ClientId = reservationRequest.ClientId
            });

            await _reservationContext.SaveChangesAsync();

            return Ok("Success: Reservation made");
        }

        /// <summary>
        /// endpoint for clients to confirm reservations
        /// </summary>
        [HttpPost("confirmation")]
        public async Task<ActionResult<Reservation>> PostReservationConfirmation([FromBody] ReservationConfirmation reservationConfirmation)
        {
            var retrieveReservation = GetReservation(reservationConfirmation.ReservationId);

            if (retrieveReservation == null)
                return NotFound("The reservation was not found");

            retrieveReservation.IsConfirmed = true;

            await _reservationContext.SaveChangesAsync();

            return Ok("Success: Reservation confirmed");
        } 

        /// <summary>
        /// get reservations by reservation id
        /// </summary>
        private Reservation GetReservation(int reservationId)
        {
            var reservation = _reservationContext.Reservation.Where(r => r.ReservationId == reservationId).FirstOrDefault();

            return reservation;
        }

        /// <summary>
        /// get timeslot by timeslot id
        /// </summary>
        private TimeSlot GetTimeSlot(int timeslotId)
        {
            var timeslot = _reservationContext.TimeSlots.Where(t => t.TimeSlotId == timeslotId).FirstOrDefault();

            return timeslot;
        }
    }
}
