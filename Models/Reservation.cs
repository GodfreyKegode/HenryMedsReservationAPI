namespace HenryMedsReservationAPI.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int TimeSlotId { get; set; }
        public int ClientId { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
