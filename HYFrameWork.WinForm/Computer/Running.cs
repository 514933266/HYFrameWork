using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HYFrameWork.WinForm
{
   public class Running
    {
        #region 功能描述：静态成员 C#里运行命令行里的命令
        /// <summary>
        /// 功能描述：静态成员 C#里运行命令(默认为cmd.exe)
        /// </summary>
        /// <param name="startFileName">要启动的应用程序</param>
        /// <param name="commands">命令行数组</param>
        /// <returns></returns>
        public static string StartProcess(string startFileName, string[] commands)
        {
            string msg = string.Empty;
            using (Process p = new Process())
            {

                try
                {
                    //实例一个Process类，启动一个独立进程

                    //Process类有一个StartInfo属性，这个是ProcessStartInfo类，包括了一些属性和方法，
                    //下面我们用到了他的几个属性：

                    p.StartInfo.FileName = startFileName ?? "cmd.exe";//设定程序名
                    p.StartInfo.Arguments = "/c C:\\Windows\\System32\\cmd.exe";
                    p.StartInfo.UseShellExecute = false;//关闭Shell的使用
                    p.StartInfo.RedirectStandardInput = true;//重定向标准输入
                    p.StartInfo.RedirectStandardOutput = true;//重定向标准输出
                    p.StartInfo.RedirectStandardError = true;//重定向错误输出
                    p.StartInfo.CreateNoWindow = true; //设置不显示窗口
                    p.StartInfo.Verb = "RunAs";
                    //上面几个属性的设置是比较关键的一步。
                    //既然都设置好了那就启动进程吧
                    p.Start();
                    //输入要执行的命令
                    foreach (string command in commands)
                    {
                        p.StandardInput.WriteLine(command);
                    }
                    p.StandardInput.WriteLine("exit");
                    msg = p.StandardOutput.ReadToEnd();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

                //从输出流获取命令执行结果
                return msg;
            }
        }

        public static void KillProcess(string pName)
        {
            // 获取所有进程
            Process[] ps = Process.GetProcesses();
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].ProcessName.ToLower().StartsWith(pName))
                {
                    ps[i].Kill();
                }
            }
        }
        #endregion
    }
}
