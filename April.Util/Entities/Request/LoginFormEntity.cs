namespace April.Util.Entities.Request
{
    public class LoginFormEntity
    {
        private string _LoginName = string.Empty;
        private string _Password = string.Empty;
        private string _Code = string.Empty;
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get => _LoginName; set => _LoginName = value; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get => _Password; set => _Password = value; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get => _Code; set => _Code = value; }
    }
}
