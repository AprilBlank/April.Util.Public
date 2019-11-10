using System;
using System.Collections.Generic;

namespace April.Util.Entities.Admin
{
    /// <summary>
    /// 管理员实体
    /// </summary>
    public class AdminEntity
    {
        private int _ID = -1;
        private string _UserName = string.Empty;
        private string _Avator = string.Empty;
        private List<string> _Controllers = new List<string>();
        private List<string> _Permissions = new List<string>();
        private int _TokenType = 0;
        private bool _IsSuperManager = false;
        private List<int> _Depts = new List<int>();
        private int _CurrentDept = -1;
        private DateTime _ExpireTime = DateTime.Now;

        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get => _ID; set => _ID = value; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get => _UserName; set => _UserName = value; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avator { get => _Avator; set => _Avator = value; }
        /// <summary>
        /// 控制器集合
        /// </summary>
        public List<string> Controllers { get => _Controllers; set => _Controllers = value; }
        /// <summary>
        /// 权限集合
        /// </summary>
        public List<string> Permissions { get => _Permissions; set => _Permissions = value; }
        /// <summary>
        /// 访问方式
        /// </summary>
        public int TokenType { get => _TokenType; set => _TokenType = value; }
        /// <summary>
        /// 是否为超管
        /// </summary>
        public bool IsSuperManager { get => _IsSuperManager; set => _IsSuperManager = value; }
        /// <summary>
        /// 企业集合
        /// </summary>
        public List<int> Depts { get => _Depts; set => _Depts = value; }
        /// <summary>
        /// 当前企业
        /// </summary>
        public int CurrentDept { get => _CurrentDept; set => _CurrentDept = value; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get => _ExpireTime; set => _ExpireTime = value; }
    }
}
