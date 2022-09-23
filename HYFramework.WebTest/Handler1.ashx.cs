using HYFramework.WebTest.Models;
using HYFrameWork.File;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HYFramework.WebTest
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //var student = new Student()
            //{
            //    Name = "xhp",
            //    Native = "汉",
            //    Sex = false,
            //    Birthday = DateTime.Now
            //};
            //IWordHandler word = new AsposeWord();
            //word.HttpExport("通用版新生报名系统功能说明", @"C:\Users\xuhaopeng\Desktop\通用版新生报名系统功能说明.docx", student);
            var str = new string[] { "只有一行一列" };
            var stream = FileHelper.ReadStream(@"C:\Users\xuhaopeng\Desktop\学生报表.xlsx");
            //var dts = NPOIExcel.Import(stream, FileType.xlsx, true);
            var table = NPOIExcel.Import(stream, FileType.xlsx);
            NPOIExcel.HttpExport(table, "学生报表2.xlsx", FileType.xlsx);
            //NPOIExcel.HttpExport(dts, "职工表格", FileType.xlsx, null, true);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}