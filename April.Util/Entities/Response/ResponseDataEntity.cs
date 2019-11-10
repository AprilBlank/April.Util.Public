namespace April.Util.Entities.Response
{
    public class ResponseDataEntity
    {
        private int _Code = 0;
        private string _Msg = string.Empty;
        private int? _Count = null;
        private object _Data = default;
        /// <summary>
        /// 回调码
        /// </summary>
        public int Code { get => _Code; set => _Code = value; }
        /// <summary>
        /// 回调消息
        /// </summary>
        public string Msg { get => _Msg; set => _Msg = value; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int? Count { get => _Count; set => _Count = value; }
        /// <summary>
        /// 回调数据
        /// </summary>
        public object Data { get => _Data; set => _Data = value; }
    }
}
