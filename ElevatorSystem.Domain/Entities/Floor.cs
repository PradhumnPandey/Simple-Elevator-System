namespace ElevatorSystem.Domain.Entities
{
    public class Floor
    {
        public int FloorNumber { get; private set; }

        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
        }
    }
}