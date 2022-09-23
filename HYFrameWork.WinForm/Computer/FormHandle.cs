using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 窗体句柄
    /// </summary>
   public class FormHandle
    {
        /// <summary>
        /// 用来根据类名和窗口名来得到窗口句柄的。但是这个函数不能查找子窗口，也不区分大小写。
        /// </summary>
        /// <param name="lpClassName">指向一个指定了类名的空结束字符串，或一个标识类名字符串的成员的指针。如果该参数为一个成员，则它必须为前次调用theGlobaIAddAtom函数产生的全局成员。该成员为16位，必须位于lpClassName的低16位，高位必须为0。</param>
        /// <param name="lpWindowName">指向一个指定了窗口名（窗口标题）的空结束字符串。如果该参数为 NULL，则为所有窗口全匹配。返回值：如果函数成功，返回值为具有指定类名和窗口名的窗口句柄。如果函数失败，返回值为NULL。</param>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 查找子窗体句柄
        /// </summary>
        /// <param name="hwndParent">父窗体句柄</param>
        /// <param name="hwndChildAfter">子窗体句柄</param>
        /// <param name="lpClassName">子窗体类名</param>
        /// <param name="lpWindowName">子窗体类名</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [StructLayout(LayoutKind.Sequential)]//定义与API相兼容结构体，实际上是一种内存转换  
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]//获取鼠标坐标  
        public static extern int GetCursorPos(
            ref POINTAPI lpPoint
        );

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄  
        public static extern int WindowFromPoint(
            int xPoint,
            int yPoint
        );
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄  
        public static extern IntPtr WindowFromPoint(
            Point p
        );
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            int hWnd,
            StringBuilder lpString,
            int nMaxCount
        );
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCount
        );
        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            int hWnd,
            StringBuilder lpString,
            int nMaxCont
        );
        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCont
        );
        /// <summary>
        /// 该函数将指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。该函数是应用程序和应用程序之间进行消息传递的主要手段之一。
        /// </summary>
        /// <param name="hWnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口。</param>
        /// <param name="Msg">指定被发送的消息</param>
        /// <param name="wParam">指定附加的消息指定信息。</param>
        /// <param name="lParam">指定附加的消息指定信息。</param>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
    }
}
