namespace HenryMedsReservationAPI.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public int ProviderId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
