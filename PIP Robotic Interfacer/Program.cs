using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIP_Robotic_Interfacer
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WindowWidth = 100;
            Console.ResetColor();

            
            

            System_Setup();



            Thread Robot_Listener = new Thread(CL_TCPListener.Run_Listener);
            Robot_Listener.Start(0);

            Thread Control_Listener = new Thread(CL_TCPListener.Run_Listener);
            Control_Listener.Start(100);


            ConsoleKey key;
            do
            {
                while (!Console.KeyAvailable)
                {
                    Monitor_Incoming_Data();
                }

                key = Console.ReadKey(true).Key;

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[20]\t'" + Console.ReadKey(true).Key + "' Key Pressed.");

                if (key == ConsoleKey.S)
                {
                    string Command_String = CL_Drive_Commands.Basic_Transmission(001, 000, 256, 256, 256);
                    string[] Function_Data = new string[] { "1", Command_String };
                    Thread Robot_Send = new Thread(CL_TCPClient.Connect);
                    Robot_Send.Start(Function_Data);
                }

                else if (key == ConsoleKey.Escape)
                {
                    Console.ResetColor();
                    Console.WriteLine("[00]\tExiting Program.");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                }

            } while (key != ConsoleKey.Escape);

        }




        // //////////////////////////////////////// FUNCTIONS

        private static void System_Setup()
        {
            if (CL_Global_Variables.System_Startup_Required)
            {

            // ROBOT => PC
            CL_Global_Variables.IP_Address[0] = "127.0.0.1";
            CL_Global_Variables.Port_Address[0] = "60000";
            
            // PC => ROBOT
            CL_Global_Variables.IP_Address[1] = "192.168.0.183";
            CL_Global_Variables.Port_Address[1] = "60001";

            // CONTROLLER RECEIVE
            CL_Global_Variables.IP_Address[100] = "127.0.0.1";
            CL_Global_Variables.Port_Address[100] = "60100";

            // CONTROLLER SEND
            CL_Global_Variables.IP_Address[101] = "";
            CL_Global_Variables.Port_Address[101] = "";


            Console.ResetColor();
            Console.WriteLine("Welcome to the PIP Robotic Interfacer");
            Console.WriteLine("Created:\t2018-08-19");
            Console.WriteLine("Developer:\tPhillip Kraguljac");
            Console.WriteLine("Licence:\tGPLv3 ");
            Console.WriteLine("");

            Console.WriteLine("This program is free software: you can redistribute it and / or modify");
            Console.WriteLine("it under the terms of the GNU General Public License as published by");
            Console.WriteLine("the Free Software Foundation, either version 3 of the License, or");
            Console.WriteLine("(at your option) any later version.");
            Console.WriteLine("");
            Console.WriteLine("This program is distributed in the hope that it will be useful, ");
            Console.WriteLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
            Console.WriteLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the");
            Console.WriteLine("GNU General Public License for more details.");
            Console.WriteLine("");
            Console.WriteLine("You should have received a copy of the GNU General Public License");
            Console.WriteLine("along with this program.If not, see < https://www.gnu.org/licenses/>.");
                Console.WriteLine("");

                //Console.WriteLine("");

                CL_Global_Variables.System_Startup_Required = false;
        }
    }


        private static void Monitor_Incoming_Data()
        {
            if (CL_Global_Variables.Received_Data != CL_Global_Variables.Memory_Received_Data)
            {
                Console.ResetColor();
                Console.WriteLine("[00]\t[127.0.0.1][System]\tAnalysing New Data Block.");
                CL_Global_Variables.Memory_Received_Data = CL_Global_Variables.Received_Data;
                               
                if (CL_Global_Variables.Received_Data.Length >= 3)
                {
                    string Category_Substring = CL_Global_Variables.Received_Data.Substring(0, 3);

                    switch (Category_Substring)
                    {
                        case "001": // <<< MODE
                            // MODE:UNIT-ID:???:???:???
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[00]\t[127.0.0.1][System]\tSensor Response.");                            
                            break;

                        case "002": // <<< MODE
                            // MODE:UNIT-ID:X-AXIS:Y-AXIS:Z-AXIS
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[00]\t[127.0.0.1][System]\tController Response.");

                            int[] Translation = CL_Sensor_Feedback.Transate_Controller_Commands(CL_Global_Variables.Received_Data);
                            string Command_String = CL_Drive_Commands.Basic_Transmission(001, Translation[0], Translation[1], Translation[2], Translation[3]);
                            string[] Function_Data = new string[] { "1", Command_String };
                            Thread Robot_Send = new Thread(CL_TCPClient.Connect);

                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[00]\t[127.0.0.1][System]\tSending Drive Command.");
                            Robot_Send.Start(Function_Data);

                            break;

                        default:
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[00]\t[127.0.0.1][System]\tCould Not Determine.");
                            break;
                    }
                }
            }
            else
            {
                
            }
        }
    }    
}
