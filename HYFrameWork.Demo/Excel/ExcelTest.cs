using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using HYFrameWork.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HYFrameWork.Demo.Excel
{

    [TestClass]
    public class ExcelTest
    {
        /// <summary>
        /// 下载Excel模版
        /// </summary>
        [TestMethod]   
        public  void TestDownTeamplate()
        {
            var list = new List<ExcelImportModel>();
        }

        public void HttpImportTest()
        {
            var str = new string[] { "只有一行一列" };
            var stream = FileHelper.ReadStream(@"C:\Users\xuhaopeng\Desktop\学生报表.xlsx");
            //var dts = NPOIExcel.Import(stream, FileType.xlsx, true);
            var table = NPOIExcel.Import(stream, FileType.xlsx);
            NPOIExcel.Export(table, FileType.xlsx, @"C:\Users\xuhaopeng\Desktop\学生报表2.xlsx");
            //NPOIExcel.HttpExport(dts, "职工表格", FileType.xlsx, null, true);
        }


    }
}
