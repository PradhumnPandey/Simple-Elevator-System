namespace ElevatorSystem.Domain.Configuration
{
    public class ElevatorSystemConfig
    {
        public int NumberOfElevators { get; set; }
        public int NumberOfFloors { get; set; }

        public ElevatorSystemConfig() { }

        public ElevatorSystemConfig(int numberOfElevators, int numberOfFloors)
        {
            NumberOfElevators = numberOfElevators;
            NumberOfFloors = numberOfFloors;
        }
    }
}