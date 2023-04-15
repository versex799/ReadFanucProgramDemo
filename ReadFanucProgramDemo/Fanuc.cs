using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReadFanucProgramDemo
{
    public class Fanuc
    {
        public ushort Handle = 0;
        public CncPrograms Programs;

        private short _ret = 0;

        public Fanuc()
        {
            Programs = new CncPrograms();
        }

        public ushort Connect(string ipAddress)
        {
            _ret = Focas1.cnc_allclibhndl3(ipAddress, 8193, 6, out Handle);

            if (_ret != Focas1.EW_OK)
            {
                Console.WriteLine($"Unable to connect to 192.168.2.123 on port 8193\n\nReturn Code: {_ret}\n\nExiting....");
                Console.Read();
            }
            else
            {
                Console.WriteLine($"Our Focas handle is {Handle}\n\n");
            }

            return Handle;
        }

    }
}
