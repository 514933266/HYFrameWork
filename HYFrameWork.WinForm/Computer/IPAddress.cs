using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 关于IP的操作类
    /// </summary>
   public class IPAddress
    {
        public static string GetRandomIp()
        {
            int[][] range = {new int[]{607649792,608174079},//36.56.0.0-36.63.255.255
                            new int[]{1038614528,1039007743},//61.232.0.0-61.237.255.255
                            new int[]{1783627776,1784676351},//106.80.0.0-106.95.255.255
                            new int[]{2035023872,2035154943},//121.76.0.0-121.77.255.255
                            new int[]{2078801920,2079064063},//123.232.0.0-123.235.255.255
                            new int[]{-1950089216,-1948778497},//139.196.0.0-139.215.255.255
                            new int[]{-1425539072,-1425014785},//171.8.0.0-171.15.255.255
                            new int[]{-1236271104,-1235419137},//182.80.0.0-182.92.255.255
                            new int[]{-770113536,-768606209},//210.25.0.0-210.47.255.255
                            new int[]{-569376768,-564133889}, //222.16.0.0-222.95.255.255
            };

            Random rdint = new Random();
            int index = rdint.Next(10);
            string ip = Num2ip(range[index][0] + new Random().Next(range[index][1] - range[index][0]));
            return ip;
        }

        /*
         * 将十进制转换成ip地址
        */
        public static string Num2ip(int ip)
        {
            int[] b = new int[4];
            string x = "";
            //位移然后与255 做高低位转换
            b[0] = (int)((ip >> 24) & 0xff);
            b[1] = (int)((ip >> 16) & 0xff);
            b[2] = (int)((ip >> 8) & 0xff);
            b[3] = (int)(ip & 0xff);
            x = (b[0]).ToString() + "." + (b[1]).ToString() + "." + (b[2]).ToString() + "." + (b[3]).ToString();

            return x;
        }


        /// <summary>
        /// 检查端口是否已经被使用
        /// </summary>
        /// <param name="port">端口号</param>
        public static bool CheckPortIsUsing(int port)
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
    }
}
