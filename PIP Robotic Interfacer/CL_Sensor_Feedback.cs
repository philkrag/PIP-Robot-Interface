using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIP_Robotic_Interfacer
{
    class CL_Sensor_Feedback
    {



        public static int[] Transate_Controller_Commands(string Sensor_Data)
        {
            string[] Data = new string[4];
            int[] Return_Data = new int[4];

            
            Return_Data[0] = 256;
            Return_Data[1] = 256;
            Return_Data[2] = 256;
            Return_Data[3] = 000;

            bool Success_Flag = false;

            // If incorrect data is sent - all of the outputs will be set to 256.
            try
            {
                Data = Sensor_Data.Split(':');
                Convert.ToInt32(Data[2]);
                Convert.ToInt32(Data[3]);
                Convert.ToInt32(Data[4]);
                Convert.ToInt32(Data[5]);
                Success_Flag = true;
            }
            catch
            {

            }

            if (Success_Flag) {
                Return_Data[0] = Convert.ToInt32(Data[2]);
                Return_Data[1] = Convert.ToInt32(Data[3]);
                Return_Data[2] = Convert.ToInt32(Data[4]);
                Return_Data[3] = Convert.ToInt32(Data[5]);
            }

            return Return_Data;
        }
    }
}
