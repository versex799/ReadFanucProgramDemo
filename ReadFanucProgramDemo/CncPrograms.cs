using System;
using System.Collections.Generic;
using System.Threading;

namespace ReadFanucProgramDemo
{
    public class CncPrograms
    {
        private short _ret = 0;
        private bool _hasDataServer = false;

        public string GetProgramName(ushort handle)
        {
            if (handle == 0)
                return "UNAVAILABLE";

            Focas1.ODBEXEPRG rdProg = new Focas1.ODBEXEPRG();

            _ret = Focas1.cnc_exeprgname(handle, rdProg);

            if (_ret != Focas1.EW_OK)
                return _ret.ToString();

            return new string(rdProg.name).Trim('\0');
        }


        public string GetSubProgramName(ushort handle)
        {
            if (handle == 0)
                return "UNAVAILABLE";

            Focas1.ODBPRO subProg = new Focas1.ODBPRO();

            _ret = Focas1.cnc_rdprgnum(handle, subProg);

            if (_ret != Focas1.EW_OK)
                return _ret.ToString();

            if (subProg.data != subProg.mdata)
                return subProg.data.ToString();
            return "No Sub Program";

        }

        public Dictionary<string, string> GetProgramListing(ushort handle)
        {
            Dictionary<string, string> progList = new Dictionary<string, string>();
            object dirToRead = "//CNC_MEM/USER/PATH1/";
            short file_num = 1;

            Focas1.ODBPDFNFIL pdf_nfil = new Focas1.ODBPDFNFIL();
            _ret = Focas1.cnc_rdpdf_subdirn(handle, dirToRead, pdf_nfil);

            if (_ret != Focas1.EW_OK)
            {
                Console.WriteLine($"Focas call 'cnc_rdpdf_subdirn()' failed with return code: {_ret}");
                return null;
            }

            Focas1.ODBPDFADIR pdfadirout = new Focas1.ODBPDFADIR();



            Focas1.IDBPDFADIR pdfadirin = new Focas1.IDBPDFADIR();
            pdfadirin.path = (string)dirToRead;     // Path we want to read
            pdfadirin.req_num = 0;                  // File index number
            pdfadirin.size_kind = 2;                // KByte
            pdfadirin.type = 1;                     // Size, Comment, Process Time Stamp Acquired

            for (int i = 0; i < pdf_nfil.file_num; i++)
            {
                _ret = Focas1.cnc_rdpdf_alldir(handle, ref file_num, pdfadirin, pdfadirout);

                if (_ret != Focas1.EW_OK)
                {
                    Console.WriteLine($"Focas call 'cnc_rdpdf_alldir()' failed with return code: {_ret}");
                    return null;
                }

                if (pdfadirout.data_kind == 1)
                    progList.Add(pdfadirout.d_f, pdfadirout.comment);

                pdfadirin.req_num++;
            }

            return progList;
        }

        public bool ActiveMainProgram(ushort handle, string filePath)
        {
            if (handle == 0)
                return false;

            var dirPath = "//CNC_MEM/USER/PATH1/";

            _ret = Focas1.cnc_pdf_slctmain(handle, dirPath + filePath);

            if (_ret != Focas1.EW_OK)
                return false;
            return true;
        }

        public bool StartProgram(ushort handle)
        {
            if (handle == 0)
                return false;

            _ret = Focas1.cnc_start(handle);

            if (_ret != Focas1.EW_OK)
                return false;
            return true;
        }

    }
}