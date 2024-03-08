using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using HenryMedsReservationAPI.Models;

namespace HenryMedsReservationAPI.Models
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) 
        {
            
        }

        public DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        // public DbSet<Users> User { get; set; } = null!;
        public DbSet<HenryMedsReservationAPI.Models.Reservation> Reservation { get; set; } = default!;
    }
}
