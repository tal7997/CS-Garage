using Ex03.garageLogic;

namespace Ex03.GarageLogic
{
    public abstract class ElectricVehicle : Vehicle
    {
        private float m_CurrentBatteryTime;
        private float m_MaxBatteryTime;

        public float MaxBatteryTime
        {
            get { return m_MaxBatteryTime; }
            set { m_MaxBatteryTime = value; }
        }

        public float CurrentBatteryTime
        {
            get { return m_CurrentBatteryTime; }
            set { m_CurrentBatteryTime = value; }
        }

        public override void SetVehicleData(string i_ModelName, string i_LicenseNumber, string i_WheelsManuFacturerName, float i_CurrentWheelsAirPressure)
        {
            base.SetVehicleData(i_ModelName, i_LicenseNumber, i_WheelsManuFacturerName, i_CurrentWheelsAirPressure);
            CurrentBatteryTime = (base.EnergyPercentage / 100) * MaxBatteryTime;
        }

        public void Charge(float i_HoursToAdd)
        {
            if (i_HoursToAdd <= 0)
            {
                throw new ValueOutOfRangeException(MaxBatteryTime, 0, "Invalid charging time. The amount to charging must be positive." + k_NewLines);
            }

            if (CurrentBatteryTime + i_HoursToAdd > MaxBatteryTime)
            {
                throw new ValueOutOfRangeException(MaxBatteryTime, 0, $"Cannot charge more than {MaxBatteryTime - CurrentBatteryTime} hours." + k_NewLines);
            }

            CurrentBatteryTime += i_HoursToAdd;
        }

        public override string ToString()
        {
            string message = string.Format(base.ToString() + 
$@"General Details:

Max Battery Time: {m_MaxBatteryTime}
Current Battery Time: {m_CurrentBatteryTime}
");
            return message;
        }
    }
}