using System;
using System.Windows.Forms;
using System.Configuration;
using HYFrameWork.Core;
namespace HYFrameWork.WinForm.Active
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Agreement agreement = new Agreement();
            if (agreement.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bool b = CheckSoftKey();
                    if (b)
                        Application.Run();//已经激活过
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            agreement.Close();
            agreement.Dispose();
        }
        private static bool CheckSoftKey()
        {
            string DESkey = "xhpDeKey";
            //获取Configuration对象
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string isActivate =config.AppSettings.Settings["isActivate"].Value.EncFromDES(DESkey);//是否激活
            string serialNumber =config.AppSettings.Settings["serialNumber"].Value.EncFromDES(DESkey);//用户计算机唯一标示，用于指示激活码是否用于新机器上，是则激活码使用次数+1
            string buildTime = config.AppSettings.Settings["buildTime"].Value.EncFromDES( DESkey);//产品第一次使用时间
            string isActivateTrial = config.AppSettings.Settings["isActivateTrial"].Value.EncFromDES( DESkey);//是否试用
            
            
            if (isActivate == "0" && isActivateTrial == "0")
                return false;
            else if (isActivate == "1")
            {
                string computerName = new ComputerInfo().ComputerName;
                if (computerName != serialNumber)
                    return false;
            }
            else if (isActivateTrial != "0")
            {
                DateTime time1 = Convert.ToDateTime(buildTime);
                DateTime time2 = DateTime.Now;
                double days = (time2 - time1).TotalDays;
                if (days > 5)
                {
                    MessageBox.Show("试用期已经结束，请对产品进行激活");
                    return false;
                }
                else
                    return true;
            }
            return true;
            ////写入<add>元素的Value
            //config.AppSettings.Settings["name"].Value = "xieyc";
            ////增加<add>元素
            //config.AppSettings.Settings.Add("url", "");
            ////删除<add>元素
            //config.AppSettings.Settings.Remove("name");
            ////一定要记得保存，写不带参数的config.Save()也可以
            //config.Save(ConfigurationSaveMode.Modified);
            ////刷新，否则程序读取的还是之前的值（可能已装入内存）
            //System.Configuration.ConfigurationManager.RefreshSection("appSettings");

        }
    }
}
