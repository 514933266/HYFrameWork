using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Net;
using System.Net.Sockets;  // 添加System.Management引用

namespace HYFrameWork.WinForm
{
    public  class ComputerInfo
    {
        private string _cupID;
        private string _macAddress;
        private string _ipAddress;
        private System.Net.IPAddress _iPAddress;
        private string _diskID;
        private string _userName;
        private string _systemType;
        private string _totalPhysicalMemory;
        private string _computerName;


        #region 属性
        /// <summary>
        /// cpu序列号
        /// </summary>
        public string CupID
        {
            get {
                this._cupID = GetCpuID();
                return this._cupID;
            }
        }
        /// <summary>
        /// 网卡硬件地址 
        /// </summary>
        public string MacAddress
        {
            get
            {
                this._macAddress = GetMacAddress();
                return this._macAddress;
            }
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public string IpAddress
        {
            get
            {
                this._ipAddress =GetIPAddress();
                return this._ipAddress;
            }
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public System.Net.IPAddress IPAddress
        {
            get
            {
                this._iPAddress = GetIPv4();
                return this._iPAddress;
            }
        }
        /// <summary>
        /// 获取硬盘ID 
        /// </summary>
        public string DiskID
        {
            get
            {
                this._diskID = GetDiskID();
                return this._diskID;
            }
        }
        /// <summary>   
        /// 获取操作系统的登录用户名   
        /// </summary>   
        public string UserName
        {
            get
            {
                this._userName =GetUserName();
                return this._userName;
            }
        } 
        /// <summary>   
        /// 获取PC类型   
        /// </summary>   
        public string SystemType
        {
            get
            {
                this._systemType = GetSystemType();
                return this._systemType;
            }
        } 
        /// <summary>   
        /// 获取物理内存   
        /// </summary>   
        public string TotalPhysicalMemory
        {
            get
            {
                this._totalPhysicalMemory = GetTotalPhysicalMemory();
                return this._totalPhysicalMemory;
            }
        }
        /// <summary>   
        /// 获取电脑名称
        /// </summary>  
        public string ComputerName
        {
            get
            {
                this._computerName = GetComputerName();
                return this._computerName;
            }
        }
        #endregion
        public ComputerInfo()
        {

        }
        /// <summary>
        /// 获取cpu序列号
        /// </summary>
        public static string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码   
                string cpuInfo = "";//cpu序列号   
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>
        /// 获取网卡硬件地址  
        /// </summary>
        public static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址   
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public static string GetIPAddress()
        {
            try
            {
                //获取IP地址   
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString();   
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>
        /// 遍历IP数组，得到IPv4的地址，否则返回一个IPv6地址
        /// </summary>
       public static System.Net.IPAddress GetIPv4()
        {
            System.Net.IPAddress[] ipe = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in ipe)
            {
                //如果数组里面的IP是Ipv4的地址则返回
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return ipe[0];
        }
        /// <summary>
        /// 获取硬盘ID 
        /// </summary>
       public static string GetDiskID()
        {
            try
            {
                //获取硬盘ID   
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>   
        /// 获取操作系统的登录用户名   
        /// </summary>   
       public static string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["UserName"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>   
        /// 获取PC类型   
        /// </summary>   
       public static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>   
        /// 获取物理内存   
        /// </summary>   
       public static string GetTotalPhysicalMemory()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
        /// <summary>   
        /// 获取电脑名称
        /// </summary>   
       public static string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
        }

    }
}