using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Structs
{
    public struct TranscatTypes
    {
        public static int Rental = 2;
        public static int Sale = 1;
        public static string RentName = "ايجار";
        public static string SaleName = "بيع";
    }

    public struct DeptCodes
    {
        public static int TeleSales = 1;
        public static int Sales = 2;
        public static int Manager = 3;
        public static string SalesName = "Sales";
        public static string TeleSalesName = "TeleSales";
        public static string BranchManagerName = "Branch Manager";
    }
}
