using System;
using Ex03.GarageLogic;
using static Ex03.ConsoleUI.Validations;

namespace Ex03.ConsoleUI
{
    public class GarageManagement
    {
        private const string k_NewLine = "\n";
        private const string k_NewLines = "\n\n";
        private const string k_InvalidInputMessage = "Invalid Input!";

        private bool   m_ContinueProgram;
        private Garage m_Garage;

        public GarageManagement()
        {
            m_Garage = new Garage();
            m_ContinueProgram = true;
        }

        public static void Run()
        {
            GarageManagement program = new GarageManagement();

            while (program.m_ContinueProgram)
            {
                try
                {
                    program.Menu();
                }
                catch (FormatException i_Fex)
                {
                    Console.WriteLine(i_Fex.Message);
                }
                catch (ArgumentException i_Aex)
                {
                    Console.WriteLine(i_Aex.Message);
                }
                catch (ValueOutOfRangeException i_Vex)
                {
                    Console.WriteLine(i_Vex.Message);
                }
                catch (Exception i_Ex)
                {
                    Console.WriteLine(i_Ex.Message);
                }
            }
        }

        public void MenuMessage()
        {
            Console.WriteLine(@"------******** WELCOME TO THE GARAGE ********------

Please choose from the following options: 
1. Add new car to garage
2. Display Vehicles license numbers by their state
3. Change the status of a car in the garage
4. Inflate the wheels of a car to the maximum
5. Refuel a motorized vehicle 
6. Charge an electric vehicle
7. Display vehicle data
8. Exit");
        }

        public void Menu()
        {
            MenuMessage();
            int choice;

            if (int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine();
                switch (choice)
                {
                    case 1:
                        {
                            AddNewVehicleToTheGarage();
                            break;
                        }
                    case 2:
                        {
                            if (AskIfDisplayByState())
                            {
                                VehicleInGarage.eVehicleState eChoice = (VehicleInGarage.eVehicleState)GetVehicleState();
                                DisplayVehiclesLicenseNumbersByState(eChoice);
                            }
                            else
                            {
                                DisplayVehiclesLicenseNumbers();
                            }

                            break;
                        }
                    case 3:
                        {
                            UpdateVehicleState();
                            break;
                        }
                    case 4:
                        {
                            InflateWheels();
                            break;
                        }
                    case 5:
                        {
                            Refuel();
                            break;
                        }
                    case 6:
                        {
                            Charge();
                            break;
                        }
                    case 7:
                        {
                            DisplayVehicleDetails();
                            break;
                        }
                    case 8:
                        {
                            m_ContinueProgram = false;
                            Console.WriteLine("Thank you, we hope you enjoyed our services");
                            break;
                        }
                    default:
                        {
                            throw new Exception(k_InvalidInputMessage + k_NewLines);
                        }
                }
            }
            else
            {
                Console.WriteLine();
                throw new FormatException(k_InvalidInputMessage + k_NewLines);
            }
        }

        public void AddNewVehicleToTheGarage()
        {
            string ownerName = null, ownerPhone = null, licenseNumber = null, modelName = null, wheelsManuFacturerName = null;
            float currentWheelsAirPressure = 0;
            VehicleInGarage vehicleOfClient = GetLicenseNumberAndCheckIsExists(ref licenseNumber);

            if (vehicleOfClient == null)
            {
                int vehicleType = GetVehicleType(m_Garage.VehiclesTypeNames);
                Vehicle vehicle = m_Garage.CreateNewVehicle(vehicleType);

                Console.WriteLine("Enter the model name: ");
                modelName = Console.ReadLine();
                Console.WriteLine();

                float currentEnergyAmount = GetCurrentEnergyAmount(vehicle.IsFueled, vehicle.MaxEnergyAmount);

                vehicle.EnergyPercentage = (currentEnergyAmount / vehicle.MaxEnergyAmount) * 100;
                GetWheelsDetails(ref wheelsManuFacturerName, ref currentWheelsAirPressure, vehicle.MaxAirPressure);
                vehicle.SetVehicleData(modelName, licenseNumber, wheelsManuFacturerName, currentWheelsAirPressure);
                GetAnswersAboutSpecificDetailsOfTheVehicle(vehicle);
                GetOwnerDetails(ref ownerName, ref ownerPhone);
                m_Garage.AddNewVehicle(vehicle, ownerName, ownerPhone);
                Console.WriteLine("Your vehicle is being repaired" + k_NewLines);
            }
            else
            {
                vehicleOfClient.VehicleState = VehicleInGarage.eVehicleState.UnderRepair;
                Console.WriteLine("The vehicle return to be UnderRepair\n");
            }
        }

        public void GetAnswersAboutSpecificDetailsOfTheVehicle(Vehicle i_Vehicle)
        {
            foreach (SpecificQuestionForVehicle specificQuestionForVehicle in i_Vehicle.SpecificQuestions)
            {
                Console.WriteLine(specificQuestionForVehicle.Question);

                i_Vehicle.CheckValidationForSpecificDetailsAndSetIfValid
                            (new SpecificAnswerForVehicle(Console.ReadLine(), specificQuestionForVehicle.VehicleSpecificDataMemberName));
                Console.WriteLine();
            }
        }

        public VehicleInGarage GetLicenseNumberAndCheckIsExists(ref string io_LicenseNumber)
        {
            Console.WriteLine("Please enter car's license number:");
            io_LicenseNumber = Console.ReadLine();
            Console.WriteLine();
            CheckLicenseNumberValidation(io_LicenseNumber);
            VehicleInGarage vehicleOfClient = m_Garage.GetVehicle(io_LicenseNumber);

            if (vehicleOfClient != null)
            {
                Vehicle kindOfVehicle = vehicleOfClient.Vehicle;
                string type1 = kindOfVehicle.GetVehicleType();
                Console.WriteLine($"This vehicle {type1} is exist in the system!\n");
                /*if (vehicle.Vehicle is MotorizedCar)
                {
                    Console.WriteLine($"This Motorized Car is exist in the system!\n");
                }*/
            }

            return vehicleOfClient;
        }

        public void DisplayVehiclesLicenseNumbersByState(VehicleInGarage.eVehicleState i_State)
        {
            bool isVehicleStateExist = false;
            int count = 1;

            foreach (VehicleInGarage vehicle in m_Garage.VehiclesOfClients.Values)
            {
                if (vehicle.VehicleState == i_State)
                {
                    if (count == 1)
                    {
                        Console.WriteLine($"------Vehicle's License Numbers In State {i_State}------\n");
                    }
                    Console.WriteLine(count++ + ". " + vehicle.Vehicle.LicenseNumber);
                    isVehicleStateExist = true;
                }
            }

            if (!isVehicleStateExist)
            {
                Console.WriteLine($"There are no vehicles in the garage in the state: {i_State}");
            }

            Console.WriteLine(k_NewLine);
        }

        public void UpdateVehicleState()
        {
            int newState;

            Console.WriteLine("Please enter the license number: ");
            string licenseNumber = Console.ReadLine();
            Console.WriteLine();
            CheckLicenseNumberValidation(licenseNumber);

            Console.WriteLine(@"Please enter the new state of the vehicle:
1. Under Repair
2. Repaired
3. Paid");

            if (!(int.TryParse(Console.ReadLine(), out newState)))
            {
                Console.WriteLine();
                throw new FormatException(k_InvalidInputMessage + k_NewLines);
            }
            else if (newState < 1 || newState > 3)
            {
                Console.WriteLine();
                throw new Exception(k_InvalidInputMessage + k_NewLines);
            }
            else
            {
                Console.WriteLine();
                VehicleInGarage.eVehicleState eNewState = (VehicleInGarage.eVehicleState)newState;
                m_Garage.UpdateVehicleState(licenseNumber, eNewState);
                Console.WriteLine($"The status of {licenseNumber} has been changed to {eNewState}\n\n");
            }
        }

        public void InflateWheels()
        {
            Console.WriteLine("Please enter the license number: ");
            string licenseNumber = Console.ReadLine();
            Console.WriteLine();
            CheckLicenseNumberValidation(licenseNumber);
            m_Garage.InflateWheels(licenseNumber);
            Console.WriteLine("The wheels were inflated to the maximum position\n\n");
        }

        public void Refuel()
        {
            float fuelAmountToFill;
            int fuelType;

            Console.WriteLine("Please enter the license number: ");
            string licenseNumber = Console.ReadLine();
            Console.WriteLine();
            CheckLicenseNumberValidation(licenseNumber);

            Console.WriteLine(@"Please enter the fuel type: 
1. Soler
2. Octan 95
3. Octan 96
4. Octan 98");

            if (!(int.TryParse(Console.ReadLine(), out fuelType)))
            {
                Console.WriteLine();
                throw new FormatException(k_InvalidInputMessage + k_NewLines);
            }
            else if (fuelType < 1 || fuelType > 4)
            {
                Console.WriteLine();
                throw new Exception(k_InvalidInputMessage + k_NewLines);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Please enter the amount of fuel to fill: ");
                if (!(float.TryParse(Console.ReadLine(), out fuelAmountToFill)))
                {
                    Console.WriteLine();
                    throw new FormatException(k_InvalidInputMessage + k_NewLines);
                }
                else
                {
                    Console.WriteLine();
                    m_Garage.Refuel(licenseNumber, (MotorizedVehicle.eFuelType)fuelType, fuelAmountToFill);
                    Console.WriteLine("Refueling completed successfully\n\n");
                }
            }
        }

        public void Charge()
        {
            float chargingTime;

            Console.WriteLine("Please enter the license number: ");
            string licenseNumber = Console.ReadLine();
            Console.WriteLine();
            CheckLicenseNumberValidation(licenseNumber);

            Console.WriteLine("Enter the charging time: ");

            if (float.TryParse(Console.ReadLine(), out chargingTime))
            {
                Console.WriteLine();
                m_Garage.Charge(licenseNumber, chargingTime);
            }
            else
            {
                Console.WriteLine();
                throw new FormatException(k_InvalidInputMessage + k_NewLines);
            }

            Console.WriteLine("Charging completed successfully\n\n");
        }

        public void DisplayVehiclesLicenseNumbers()
        {
            int counter = 1;
            Console.WriteLine("------Vehicle's License Numbers------\n\n");

            foreach (VehicleInGarage vehicle in m_Garage.VehiclesOfClients.Values)
            {
                Console.WriteLine(counter++ + ". " + vehicle.Vehicle.LicenseNumber + $" - {vehicle.VehicleState}");
            }

            Console.WriteLine(k_NewLine);
        }

        public void DisplayVehicleDetails()
        {
            Console.WriteLine("Please enter the license number: ");
            string licenseNumber = Console.ReadLine();
            Console.WriteLine();
            CheckLicenseNumberValidation(licenseNumber);

            VehicleInGarage vehicleInGarage = m_Garage.GetVehicle(licenseNumber);

            if (vehicleInGarage != null)
            {
                Console.WriteLine(vehicleInGarage.ToString());

                Vehicle vehicle = vehicleInGarage.Vehicle;

                if (vehicle is MotorizedCar)
                {
                    Console.WriteLine(((MotorizedCar)vehicle).ToString());
                }
                else if (vehicle is MotorizedMotorcycle)
                {
                    Console.WriteLine(((MotorizedMotorcycle)vehicle).ToString());
                }
                else if (vehicle is ElectricCar)
                {
                    Console.WriteLine(((ElectricCar)vehicle).ToString());
                }
                else if (vehicle is ElectricMotorcycle)
                {
                    Console.WriteLine(((ElectricMotorcycle)vehicle).ToString());
                }
                else if (vehicle is Truck)
                {
                    Console.WriteLine(((Truck)vehicle).ToString());
                }
                /*else if (vehicle is Tractor)
                {
                    Console.WriteLine(((Tractor)vehicle).ToString());
                }*/
            }
            else
            {
                throw new Exception("The vehicle does not exist in the garage" + k_NewLines);
            }
        }
    }
}