namespace ElevatorSystem.Domain.Entities
{
    public class ElevatorRequest
    {
        public int RequestedFloor { get; private set; }
        public DateTime RequestTime { get; private set; }

        public ElevatorRequest(int requestedFloor)
        {
            RequestedFloor = requestedFloor;
            RequestTime = DateTime.Now;
        }
    }
}