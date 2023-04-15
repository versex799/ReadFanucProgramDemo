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
        private static short _ret = 0;
        private static Fanuc _fanuc;


        static void Main(string[] args)
        {

            _fanuc = new Fanuc();
            var handle = _fanuc.Connect("192.168.2.123");

            var progList = _fanuc.Programs.GetProgramListing(handle);

            int index = 0;

            foreach (var prg in progList)
            {
                Console.WriteLine($"{index}) Name: {prg.Key}\tComment: {prg.Value}");
                index++;
            }

            Console.WriteLine("\nPlease Select a program number to activate");
            var num = Console.ReadLine();

            var activateSuccess = _fanuc.Programs.ActiveMainProgram(handle, progList.ElementAt(int.Parse(num)).Key);

            if (activateSuccess)
            {
                Console.WriteLine($"{progList.ElementAt(int.Parse(num)).Key} successfully activated. Do you wish to start the program (Y/N)? ");
                var startPrg = Console.ReadLine();

                if (startPrg == "Y")
                {
                    var startSuccess = _fanuc.Programs.StartProgram(handle);
                    if (startSuccess)
                        Console.WriteLine($"{progList.ElementAt(int.Parse(num)).Key} successfully started!");
                    else
                        Console.WriteLine($"Unable to start {progList.ElementAt(int.Parse(num)).Key}");
                }
                else
                {
                    Console.WriteLine("You can start the program by pressing the 'Cycle Start' button on the operator panel");
                }
            }
            else
            {
                Console.WriteLine($"Unable to activate '{progList.ElementAt(int.Parse(num)).Key}'");
            }

            Console.ReadLine();
        }
    }
}
