using System;

namespace HYFrameWork.Test.SQLite.Model
{
    /// <summary>
    /// 数据对象模型层类
    /// </summary>
    public class tb_user
    {
        public tb_user() { }
        protected int _rownumber;
        protected int _id;
        protected int _groupid;
        protected string _username;
        protected string _password;
        protected string _name;
        protected string _mobile;
        protected string _channel;
        protected int _smsremaincount;
        protected int _smssentcount;
        protected int _type;
        protected bool _defaultuser;
        private string _mobile2;
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Mobile2
        {
            get { return _mobile2; }
            set { _mobile2 = value; }
        }
        /// <summary>
        /// 排序序号
        /// </summary>
        public int Rownumber { get { return _rownumber; } set { _rownumber = value; } }
        /// <summary>
        /// 列名id
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// 列名groupID
        /// </summary>
        public int GroupID { get { return _groupid; } set { _groupid = value; } }
        /// <summary>
        /// 列名userName
        /// </summary>
        public string UserName { get { return _username; } set { _username = value; } }
        /// <summary>
        /// 列名password
        /// </summary>
        public string Password { get { return _password; } set { _password = value; } }
        /// <summary>
        /// 列名name
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        /// <summary>
        /// 列名mobile
        /// </summary>
        public string Mobile { get { return _mobile; } set { _mobile = value; } }
        /// <summary>
        /// 列名channel
        /// </summary>
        public string Channel { get { return _channel; } set { _channel = value; } }
        /// <summary>
        /// 列名smsRemainCount
        /// </summary>
        public int SmsRemainCount { get { return _smsremaincount; } set { _smsremaincount = value; } }
        /// <summary>
        /// 列名smsSentCount
        /// </summary>
        public int SmsSentCount { get { return _smssentcount; } set { _smssentcount = value; } }
        /// <summary>
        /// 列名type
        /// </summary>
        public int Type { get { return _type; } set { _type = value; } }
        /// <summary>
        /// 列名defaultUser
        /// </summary>
        public bool DefaultUser { get { return _defaultuser; } set { _defaultuser = value; } }
    }
}
