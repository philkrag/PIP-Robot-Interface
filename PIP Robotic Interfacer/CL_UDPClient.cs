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
    class CL_UDPClient
    {

        public static void Connect(object Function_Data)
        {
            string[] Function_Data_Array = ((IEnumerable)Function_Data).Cast<object>().Select(x => x.ToString()).ToArray();

            int Device_ID = Convert.ToInt32(Function_Data_Array[0]);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //string Message = TXB_Message.Text;
            string Message_Out = Function_Data_Array[1];

            Int32 Port_Address = Convert.ToInt32(CL_Global_Variables.Port_Address[Device_ID]);
            string IP_Address = CL_Global_Variables.IP_Address[Device_ID];

            IPAddress broadcast = IPAddress.Parse(IP_Address);

            byte[] data = Encoding.ASCII.GetBytes(Message_Out + "\r\n");
            IPEndPoint ep = new IPEndPoint(broadcast, Port_Address);

            s.SendTo(data, ep);

            //Console.WriteLine("Message sent to the broadcast address");

        }

    }
}
