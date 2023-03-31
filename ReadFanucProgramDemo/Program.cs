using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ReadFanucProgramDemo
{
    internal class Program
    {
        static ushort _handle = 0;
        static short _ret = 0;

        static void Main(string[] args)
        {

            _ret = Focas1.cnc_allclibhndl3("192.168.2.123", 8193, 6, out _handle);

            if (_ret != Focas1.EW_OK)
            {
                Console.WriteLine($"Unable to connect to 192.168.2.123 on port 8193\n\nReturn Code: {_ret}\n\nExiting....");
                Console.Read();
            }
            else
            {
                Console.WriteLine($"Our Focas handle is {_handle}\n\n");


                while (true)
                {

                    Console.Write($"Program Name: {GetProgramName()}\tSub-Program Name: {GetSubProgramName()}                             \r");

                    Thread.Sleep(500);
                }

            }

        }

        public static string GetProgramName()
        {
            if (_handle == 0)
                return "UNAVAILABLE";

            Focas1.ODBEXEPRG rdProg = new Focas1.ODBEXEPRG();

            _ret = Focas1.cnc_exeprgname(_handle, rdProg);

            if (_ret != Focas1.EW_OK)
                return _ret.ToString();

            return new string(rdProg.name).Trim('\0');
        }


        public static string GetSubProgramName()
        {
            if (_handle == 0)
                return "UNAVAILABLE";

            Focas1.ODBPRO subProg = new Focas1.ODBPRO();

            _ret = Focas1.cnc_rdprgnum(_handle, subProg);

            if (_ret != Focas1.EW_OK)
                return _ret.ToString();

            if (subProg.data != subProg.mdata)
                return subProg.data.ToString();
            return "No Sub Program";

        }

    }
}
