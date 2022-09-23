using HYFrameWork.Core;
using HYFrameWork.Demo.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HYFrameWork.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            new ExcelTest().HttpImportTest();
            t.Stop();
            Console.WriteLine("总共花费时间{0}".Fmt(t.Elapsed));
            Console.ReadKey();
        }
    }
}
