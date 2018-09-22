using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIP_Robotic_Interfacer
{
    class CL_Drive_Commands
    {

        public static string Basic_Transmission(int Unit_ID, int X_Axis, int Y_Axis, int Z_Axis, int Push_Button)
        {
            // 001 => Command
            // 002 => Unit ID
            // 003 => X-Axis Command
            // 004 => Y-Axis Command
            // 005 => Z-Axis Command

            string Return_String = "010"+ ":" + Unit_ID.ToString().PadLeft(3, '0') + ":" + X_Axis.ToString().PadLeft(3, '0') + ":" + Y_Axis.ToString().PadLeft(3, '0') +":"+ Z_Axis.ToString().PadLeft(3, '0') + ":" + Push_Button.ToString().PadLeft(3, '0');
            
            return Return_String;
        }




    }
}
