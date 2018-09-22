using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PIP_Robotic_Interfacer
{
    class CL_TCPClient
    {

        public static void Connect(object Function_Data)
        {
            //int Device_ID = Convert.ToInt32(Function_Data[0]);
            string[] Function_Data_Array = ((IEnumerable)Function_Data).Cast<object>().Select(x => x.ToString()).ToArray();

            int Device_ID = Convert.ToInt32(Function_Data_Array[0]);
            string Message_Out = Function_Data_Array[1];

            if (CL_Global_Variables.IP_Address[Device_ID] != null && CL_Global_Variables.Port_Address[Device_ID] != null)
            {

                //TcpListener server = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    //Int32 port = 13000;
                    //IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                    
                    Int32 Port_Address = Convert.ToInt32(CL_Global_Variables.Port_Address[Device_ID]);
                    IPAddress IP_Address = IPAddress.Parse(CL_Global_Variables.IP_Address[Device_ID]);
                    //string message = "Test";


                    // Create a TcpClient.
                    // Note, for this client to work you need to have a TcpServer 
                    // connected to the same address as specified by the server, port
                    // combination.
                    //Int32 port = 13000;
                    TcpClient client = new TcpClient(IP_Address.ToString(), Port_Address);

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(Message_Out);

                    // Get a client stream for reading and writing.
                    //  Stream stream = client.GetStream();

                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);

                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[30]\t[" + IP_Address+"]["+Port_Address+ "]\tSent: " + Message_Out);

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String Message_In = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    Message_In = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[30]\t[" + IP_Address + "][" + Port_Address + "]\tReceived: " + Message_In);
                    //Console.WriteLine("Received: {0}", responseData);

                    // Close everything.
                    stream.Close();
                    client.Close();

                }
                catch
                {

                }

            }
        }
    }
}
