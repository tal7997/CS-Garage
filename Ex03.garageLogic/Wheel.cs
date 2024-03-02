﻿using System;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private string m_ManufacturerName;
        private float  m_CurrentAirPressure;
        private float  m_MaxAirPressure;

        public string ManufacturerName 
        {
            get { return m_ManufacturerName; }
            set 
            {
                if ((value.ToString())[0] == ' ')
                {
                    throw new Exception("Invalid Name!\n\n");
                }

                m_ManufacturerName = value;
            }
        }

        public float CurrentAirPressure
        {
            get { return m_CurrentAirPressure; }

            set
            {
                if (value <= 0)
                {
                    throw new ValueOutOfRangeException(m_MaxAirPressure, 0, "The value is out of range, the amount of air pressure must be positive.\n\n");
                }

                if (value > m_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(m_MaxAirPressure, 0, $"The amount of the air pressure can not exceed {m_MaxAirPressure}\n\n");
                }

                m_CurrentAirPressure = value;
            }
        }

        public float MaxAirPressure
        {
            get { return m_MaxAirPressure; }
        }

        public Wheel(string i_ManufacturerName, float i_CurrentAirPressure, float i_MaxAirPressure)
        {
            m_MaxAirPressure = i_MaxAirPressure;
            ManufacturerName = i_ManufacturerName;
            CurrentAirPressure = i_CurrentAirPressure;
        }

        public void Inflate(float airPressure)
        {
            //add check range
            if(CurrentAirPressure + airPressure <= m_MaxAirPressure)
            {
                CurrentAirPressure += airPressure;
            }
        }

        public override string ToString()
        {
            string message = string.Format(
$@"Manufacturer Name: {m_ManufacturerName}
Current Air Pressure: {m_CurrentAirPressure}
Max Air Pressure: {m_MaxAirPressure}

");

            return message;
        }
    }
}