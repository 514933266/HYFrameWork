using HYFrameWork.Core;
using System;

namespace HYFrameWork.Test.Core.Net
{
   public class TestHttpCookie
    {
       public static void Test_SetCookie()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory.Append("\\Core.Net\\TestCookie_1.txt");
            string cookies =System.IO.File.ReadAllText(fileName);
            System.Net.CookieContainer cc= HttpCookieHelper.ToCookieContainer(cookies,"www.baidu.com");
        }
    }
}
