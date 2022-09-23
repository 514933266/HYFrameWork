using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using HYFrameWork.Core;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 程序配置文件帮助类
    /// </summary>
    public class AppConfigHelper
    {
        #region 程序配置
        string _desKey = "xhpDeKey";
        string configPath = string.Empty;
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public AppConfigHelper()
        {

        }
        public string ConfigPath
        {
            get { return configPath; }
            set
            {
                configPath = value;
                this.config = ConfigurationManager.OpenExeConfiguration(this.configPath);
            }
        }
        public string DesKey
        {
            get { return this._desKey; }
            set { this._desKey = value; }
        }

        #endregion

        /// <summary>
        /// 读取appconfig的节点内容，并根据类型进行解密
        /// 当为MD5和None时不进行解密
        /// 当为Rsa时,如果不存在公钥,不进行解密
        /// </summary>
        /// <param name="securityType">加密类型</param>
        /// <returns></returns>
        public Dictionary<string, string> Read_AppConfig(EncryptType securityType)
        {
            Dictionary<string, string> dic;
            try
            {
                dic = DecodeAppSettings(config.AppSettings.Settings, securityType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dic;
        }
        /// <summary>
        /// 写出appconfig的节点内容，并根据类型进行解密
        /// 当为MD5和None时不进行解密
        /// 当为Rsa时,如果不存在公钥,不进行解密
        /// </summary>
        /// <param name="dic">APP.config的键值对</param>
        /// <param name="securityType">加密类型</param>
        public bool Write_AppConfig(Dictionary<string, string> dic, EncryptType securityType)
        {
            var b = false;
            try
            {
                b = EncodeAppSettings(dic, securityType);
            }
            catch (Exception ex)
            {
                b = false;
                throw ex;
            }
            return b;
        }
        private Dictionary<string, string> DecodeAppSettings(KeyValueConfigurationCollection Settings, EncryptType securityType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            switch (securityType)
            {
                case EncryptType.DES:
                    foreach (KeyValueConfigurationElement kv in Settings)
                    {
                        dic.Add(kv.Key, kv.Value.EncToDES(_desKey));
                    }
                    break;
                case EncryptType.MD5:
                    foreach (KeyValueConfigurationElement kv in Settings)
                    {
                        dic.Add(kv.Key, kv.Value);
                    }
                    break;
                case EncryptType.RSA:
                    string publickey = string.Empty;
                    string privatekey = string.Empty;
                    if (config.AppSettings.Settings["rsaPublicKey"] != null && config.AppSettings.Settings["rsaPrivateKey"] != null)
                    {
                        publickey = config.AppSettings.Settings["rsaPublicKey"].Value;
                        privatekey = config.AppSettings.Settings["rsaPrivateKey"].Value;
                        foreach (KeyValueConfigurationElement kv in Settings)
                        {
                            dic.Add(kv.Key,kv.Value.EncToRSA(privatekey));
                        }
                    }
                    else
                    {
                        foreach (KeyValueConfigurationElement kv in Settings)
                        {
                            dic.Add(kv.Key, kv.Value);
                        }
                    }
                    break;
                case EncryptType.None:
                    foreach (KeyValueConfigurationElement kv in Settings)
                    {
                        dic.Add(kv.Key, kv.Value);
                    }
                    break;
            }
            return dic;
        }
        private bool EncodeAppSettings(Dictionary<string, string> dic, EncryptType securityType)
        {
            bool b = false;
            try
            {
                switch (securityType)
                {
                    case EncryptType.DES:
                        foreach (KeyValuePair<string, string> kv in dic)
                        {
                            if (config.AppSettings.Settings[kv.Key] == null)
                                config.AppSettings.Settings.Add(kv.Key, kv.Value.EncToDES(_desKey));
                            else
                                config.AppSettings.Settings[kv.Key].Value = kv.Value.EncToDES(_desKey);

                        }
                        break;
                    case EncryptType.MD5:
                        foreach (KeyValuePair<string, string> kv in dic)
                        {
                            if (config.AppSettings.Settings[kv.Key] == null)
                                config.AppSettings.Settings.Add(kv.Key, kv.Value.EncToMD5());
                            else
                                config.AppSettings.Settings[kv.Key].Value = kv.Value.EncToMD5();
                        }
                        break;
                    case EncryptType.RSA:
                        string publickey = string.Empty;
                        string privatekey = string.Empty;
                        if (config.AppSettings.Settings["rsaPublicKey"] == null && config.AppSettings.Settings["rsaPrivateKey"] == null)
                        {
                            RSACryptoServiceProvider crypt = new RSACryptoServiceProvider();
                            publickey = crypt.ToXmlString(false);//公钥
                            privatekey = crypt.ToXmlString(true);//私钥
                            crypt.Clear();
                        }
                        else
                        {
                            publickey = config.AppSettings.Settings["rsaPublicKey"].Value;
                            privatekey = config.AppSettings.Settings["rsaPrivateKey"].Value;
                        }
                        foreach (KeyValuePair<string, string> kv in dic)
                        {
                            if (config.AppSettings.Settings[kv.Key] == null)
                                config.AppSettings.Settings.Add(kv.Key,kv.Value.EncToAES(publickey));
                            else
                                config.AppSettings.Settings[kv.Key].Value = kv.Value.EncToAES(publickey);
                        }
                        if (config.AppSettings.Settings["rsaPublicKey"] == null && config.AppSettings.Settings["rsaPrivateKey"] == null)
                        {
                            config.AppSettings.Settings.Add("rsaPublicKey", publickey);
                            config.AppSettings.Settings.Add("rsaPrivateKey", privatekey);
                        }
                        else
                        {
                            config.AppSettings.Settings["rsaPublicKey"].Value = publickey;
                            config.AppSettings.Settings["rsaPrivateKey"].Value = privatekey;
                        }
                        break;
                    case EncryptType.None:
                        foreach (KeyValuePair<string, string> kv in dic)
                        {
                            if (config.AppSettings.Settings[kv.Key] == null)
                                config.AppSettings.Settings.Add(kv.Key, kv.Value);
                            else
                                config.AppSettings.Settings[kv.Key].Value = kv.Value;
                        }
                        break;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                b = true;
            }
            catch (Exception ex)
            {
                b = false;
                throw ex;
            }
            return b;
        }

    }
}
