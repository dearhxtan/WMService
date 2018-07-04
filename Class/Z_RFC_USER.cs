using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.WMService.Class
{
    public class Z_RFC_USER
    {
        public string ZTYPE { get; set; }
        public ZRFC_TEST GS_ZRFC_TEST2 { get; set; }
        public List<ZRFC_TEST> GT_ZRFC_TEST { get; set; }
        public string ERROR_MESSAGE { get; set; }
    }

    public class ZRFC_TEST
    {
        public string ZNO1 { get; set; }
        public string ZNAME1 { get; set; }
        public string ZREMARK { get; set; }
        public string ZDATE1 { get; set; }
        public string ZTIME1 { get; set; }
        public string ZQTY { get; set; }
    }
}