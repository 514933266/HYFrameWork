using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace HYFrameWork.WinForm
{
   public class SocketHelper
    {
        /// <summary>
        /// 检查端口是否已经被使用
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
       public static bool CheckPortIsInUsing(int port)
       {
           bool inUsing = false;
           IPEndPoint _remoteEndPoint = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().FirstOrDefault(c => c.Port == port) as IPEndPoint;
           if (_remoteEndPoint != null) inUsing = true;
           return inUsing;
       }
    }
}
