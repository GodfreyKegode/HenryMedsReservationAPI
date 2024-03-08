using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HenryMedsReservationAPI.Models;
using System.Net;
using System.Text;

namespace HenryMedsReservationAPI.Controllers
{
    [Route("providers/[controller]")]
    [ApiController]
    public class TimeSlotsController : ControllerBase
    {
        private readonly ReservationContext _reservationContext;
        private static List<TimeSlot> timeslots = new List<TimeSlot>();

        public TimeSlotsController(ReservationContext reservationContext)
        {
            _reservationContext = reservationContext;
        }

        /// <summary>
        /// get all available provider timeslots
        /// </summary>
        [HttpGet("availabilities")]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> GetAllTimeSlots()
        {
            return await _reservationContext.TimeSlots.ToListAsync();
        }        
        
        /// <summary>
        /// endpoint to get available provider timeslots by providerid
        /// </summary>
        [HttpGet("{providerId}/availabilities")]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> GetTimeSlots(int providerId)
        {
            return await _reservationContext.TimeSlots.Where(id => id.ProviderId == providerId).ToListAsync();
        }

        /// <summary>
        /// endpoint for providers to add new provider availablity timeslots
        /// </summary>
        [HttpPost("{providerId}/availabilities")]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> AddProviderAvailabilies(int providerId, [FromBody] IEnumerable<AvailabilityRequest> availabilityRequest)
        {
            // Check if user exists
            //var provider = GetUser(providerId);
            //if (provider == null)
            //    return NotFound("The provider was not found");

            // I would move all of this business logic to the Service/ Manager layer
            foreach (var availability in availabilityRequest)
            {
                timeslots.Add(new TimeSlot 
                { 
                    ProviderId = providerId, 
                    StartTime = availability.StartTime,
                    EndTime = availability.EndTime 
                });
            }

            _reservationContext.TimeSlots.AddRange(timeslots);
            await _reservationContext.SaveChangesAsync();

            //I would implement a HttpResponseMessage middleware to wrap my outgoing API messages
            // with a HTTP status success, status code and user friendly message as part of the json response body
            return Ok(timeslots);            
        }        

        // This method to check if the user exists would also live in the Service/ Manager layer
        //private Users GetUser(int userId)
        //{
        //    return _reservationContext.User.Where(u => u.UserId == userId).FirstOrDefault();
        //}
    }
}
