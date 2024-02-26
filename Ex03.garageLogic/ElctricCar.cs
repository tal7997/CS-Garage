﻿using Ex03.garageLogic;
using System;

namespace Ex03.GarageLogic
{
    public class ElectricCar : ElectricVehicle
    {

        private const string k_ColorTopicName = "Color";
        private const string k_NumberOfDoorsTopicName = "Number Of Doors";
        private const float k_MaxBatteryTime = 4.8f;
        private const float k_MaxAirPressure = 30f;
        private const int k_NumOfWheels = 5;

        private eColor m_Color;
        private eNumberOfDoors m_NumberOfDoors;

        public enum eColor
        {
            Blue = 1,
            White,
            Red,
            Yellow
        }

        public enum eNumberOfDoors
        {
            Two = 1,
            Three,
            Four,
            Five
        }

        public eNumberOfDoors NumberOfDoors
        {
            get { return m_NumberOfDoors; }

            set
            {
                if (value != eNumberOfDoors.Two && value != eNumberOfDoors.Three &&
                    value != eNumberOfDoors.Four && value != eNumberOfDoors.Five)
                {
                    throw new ValueOutOfRangeException((int)eNumberOfDoors.Five, (int)eNumberOfDoors.Two,
                        $"\nInvalid Input!\n\nThe amount of doors should be between {eNumberOfDoors.Two} to {eNumberOfDoors.Five}" + k_NewLines);
                }

                m_NumberOfDoors = value;
            }
        }

        public eColor Color
        {
            get { return m_Color; }

            set
            {
                if (value != eColor.Red && value != eColor.Blue &&
                   value != eColor.White && value != eColor.Yellow)
                {
                    throw new Exception($"\nInvalid Input!\n\nThe color of the car should be one of the following colors: " +
                        $" {eColor.Blue} , {eColor.White} , {eColor.Red} , {eColor.Yellow}" + k_NewLines);
                }

                m_Color = value;
            }
        }


        public ElectricCar()
        {
            SpecificQuestionForVehicle specificQuestionForElectricCar1 = new SpecificQuestionForVehicle();
            SpecificQuestionForVehicle specificQuestionForElectricCar2 = new SpecificQuestionForVehicle();

            specificQuestionForElectricCar1.Question = string.Format(@"Enter the one of the following colors: 
1. Blue
2. White
3. Red
4. Yellow");
            specificQuestionForElectricCar1.VehicleSpecificDataMemberName = k_ColorTopicName;

            SpecificQuestions.Add(specificQuestionForElectricCar1);

            specificQuestionForElectricCar2.Question = string.Format(@"Enter the number of doors: 
1. Two
2. Three
3. Four
4. Five");
            specificQuestionForElectricCar2.VehicleSpecificDataMemberName = k_NumberOfDoorsTopicName;

            SpecificQuestions.Add(specificQuestionForElectricCar2);

            base.MaxAirPressure = k_MaxAirPressure;
            base.MaxBatteryTime = base.MaxEnergyAmount = k_MaxBatteryTime;
            base.NumOfWheels = k_NumOfWheels;
        }

        public override void CheckValidationForSpecificDetailsAndSetIfValid(SpecificAnswerForVehicle i_SpecificAnswer)
        {
            if (i_SpecificAnswer.VehicleSpecificDataMemberName == k_ColorTopicName)
            {
                CheckValidationForColorAndSetIfValid(i_SpecificAnswer.Answer);
            }
            else if (i_SpecificAnswer.VehicleSpecificDataMemberName == k_NumberOfDoorsTopicName)
            {
                CheckValidationForNumberOfDoorsAndSetIfValid(i_SpecificAnswer.Answer);
            }
        }

        public void CheckValidationForColorAndSetIfValid(string i_Answer)
        {
            int answer;

            if (!(int.TryParse(i_Answer, out answer)))
            {
                throw new FormatException(k_InvalidInputMessage + k_NewLines);
            }
            else
            {
                Color = (eColor)answer;
            }
        }

        public void CheckValidationForNumberOfDoorsAndSetIfValid(string i_Answer)
        {
            int answer = 0;

            if (!(int.TryParse(i_Answer, out answer)))
            {
                throw new FormatException(k_InvalidInputMessage + k_NewLines);
            }

            else
            {
                NumberOfDoors = (eNumberOfDoors)answer;
            }
        }

        public override string ToString()
        {
            string message = string.Format("--------Electric Car--------\n\n" + base.ToString() + 
$@"Color: {m_Color}
Number Of Doors: {m_NumberOfDoors}
");
            return message;
        }
    }
}