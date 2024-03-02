﻿using System;

namespace Ex03.GarageLogic
{
    public class MotorizedMotorcycle : MotorizedVehicle
    {
        private const float k_MaxAirPressure = 29f;
        private const eFuelType k_FuelType = eFuelType.Octan98;
        private const float k_MaxFuelAmount = 5.8f;
        private const int k_NumOfWheels = 2;
        private const string k_LicensetypeTopicName = "License Type";
        private const string k_EngineVolumeTopicName = "Engine Volume";

        private eLicenseType m_LicenseType;
        private int m_EngineVolume;

        public enum eLicenseType
        {
            A1 = 1,
            A2,
            AB,
            B2
        }

        public eLicenseType LicenseType
        {
            get { return m_LicenseType; }

            set
            {
                if (value != eLicenseType.A1 && value != eLicenseType.A2 && value != eLicenseType.AB && value != eLicenseType.B2)
                {
                    throw new Exception("\nInvalid Input!\n\nThe license type should be one of the following license types: " +
                        $"{eLicenseType.A1},  {eLicenseType.A2}, {eLicenseType.AB} ,{eLicenseType.B2}" + k_NewLines);
                }

                m_LicenseType = value;
            }
        }

        public int EngineVolume
        {
            get { return m_EngineVolume; }

            set
            {
                if (value < 0)
                {
                    throw new ValueOutOfRangeException(int.MaxValue, 0, $"\nInvalid Input!\n\nEngine volume must be positive" + k_NewLines);
                }

                m_EngineVolume = value;
            }
        }

        public MotorizedMotorcycle()
        {
            SpecificQuestionForVehicle specificQuestionForMotorizedMotorcycle1 = new SpecificQuestionForVehicle();
            SpecificQuestionForVehicle specificQuestionForMotorizedMotorcycle2 = new SpecificQuestionForVehicle();

            specificQuestionForMotorizedMotorcycle1.Question = string.Format(@"Enter the engine volume: ");
            specificQuestionForMotorizedMotorcycle1.VehicleSpecificDataMemberName = "Engine Volume";

            SpecificQuestions.Add(specificQuestionForMotorizedMotorcycle1);

            specificQuestionForMotorizedMotorcycle2.Question = string.Format(@"Enter the license type: 
1. A1
2. A2
3. AB
4. B2");
            specificQuestionForMotorizedMotorcycle2.VehicleSpecificDataMemberName = k_LicensetypeTopicName;

            SpecificQuestions.Add(specificQuestionForMotorizedMotorcycle2);

            base.FuelType = k_FuelType;
            base.MaxAirPressure = k_MaxAirPressure;
            base.MaxFuelAmount = base.MaxEnergyAmount = k_MaxFuelAmount;
            base.NumOfWheels = k_NumOfWheels;
            IsFueled = true;
        }

        public override void CheckValidationForSpecificDetailsAndSetIfValid(SpecificAnswerForVehicle i_SpecificAnswer)
        {
            if (i_SpecificAnswer.VehicleSpecificDataMemberName == k_EngineVolumeTopicName)
            {
                CheckValidationForEngineVolumeAndSetIfValid(i_SpecificAnswer.Answer);
            }
            else if (i_SpecificAnswer.VehicleSpecificDataMemberName == k_LicensetypeTopicName)
            {
                CheckValidationForLicenseTypeAndSetIfValid(i_SpecificAnswer.Answer);
            }
        }

        public void CheckValidationForEngineVolumeAndSetIfValid(string i_Answer)
        {
            int answer;

            if (!(int.TryParse(i_Answer, out answer)))
            {
                throw new FormatException("\n" + k_InvalidInputMessage + k_NewLines);
            }
            else
            {
                EngineVolume = answer;
            }
        }

        public void CheckValidationForLicenseTypeAndSetIfValid(string i_Answer)
        {
            int answer;

            if (!(int.TryParse(i_Answer, out answer)))
            {
                Console.WriteLine();
                throw new FormatException("\n" + k_InvalidInputMessage + k_NewLines);
            }
            else
            {
                LicenseType = (eLicenseType)answer;
            }
        }

        public override string ToString()
        {
            string message = string.Format("--------Motorized Motorcycle--------\n\n" + base.ToString() + 
$@"License Type: {m_LicenseType}
Engine Volume: {m_EngineVolume}

");

            return message;
        }

        public override string GetVehicleType()
        {
            return "Motorizid Motorcycle";
        }
    }
}