using System.Collections.Generic;
using System.Configuration;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：config配置
    /// </summary>
    public static class ConfigHelper
    {
        #region 连接字符串
        /// <summary>
        /// 获取所有的连接字符串
        /// </summary>
        /// <returns>连接字符串字典集合</returns>
        public static Dictionary<string, string> GetConnStrings()
        {
            var dic = new Dictionary<string, string>();
            for (int i = 0, j = ConfigurationManager.ConnectionStrings.Count; i < j; i++)
            {
                dic.Add(
                    ConfigurationManager.ConnectionStrings[i].Name,
                    ConfigurationManager.ConnectionStrings[i].ConnectionString
                    );
            }
            return dic;
        }
        #endregion

        #region 读写配置文件
        /// <summary>
        /// 写入Appsetting节点集合
        /// </summary>
        /// <param name="dic">节点结婚</param>
        public static void WriteToAppSetting(IDictionary<string, string> dic)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (KeyValuePair<string, string> kv in dic)
            {
                if (config.AppSettings.Settings[kv.Key] == null)
                {
                    config.AppSettings.Settings.Add(kv.Key, kv.Value);
                }
                else
                {
                    config.AppSettings.Settings[kv.Key].Value = kv.Value;
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// 读取Appsetting节点集合
        /// </summary>
        /// <param name="dic">节点集合容器</param>
        /// <returns>节点结婚</returns>
        public static IDictionary<string, string> ReadAppConfig(IDictionary<string, string> dic)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (KeyValueConfigurationElement kv in config.AppSettings.Settings)
            {
                dic.Add(kv.Key, kv.Value);
            }
            return dic;
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>连接字符串值</returns>
        public static string ValueOfConnectionString(this string str)
        {
            return ConfigurationManager.ConnectionStrings[str] == null ? null : ConfigurationManager.ConnectionStrings[str].ConnectionString;
        }
        /// <summary>
        /// 获取AppSetting节点
        /// </summary>
        /// <param name="key">节点key</param>
        /// <returns>节点值</returns>
        public static string ValueOfAppSetting(this string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 写入Appsetting节点
        /// </summary>
        /// <param name="str">节点名</param>
        /// <param name="value">节点值</param>
        public static void WriteToAppSetting(this string str, string value)
        {
            Dictionary<string, string> _dic = new Dictionary<string, string>() { { str, value } };
            WriteToAppSetting(_dic);
        }
        #endregion

    }
}
