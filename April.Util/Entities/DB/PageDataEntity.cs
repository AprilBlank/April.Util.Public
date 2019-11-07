using System.Collections.Generic;

namespace April.Util.Entities.DB
{
    public class PageDataEntity<T>
    {
        private int _Count = 0;
        private List<T> _Data = null;
        /// <summary>
        /// 分页总数
        /// </summary>
        public int Count { get => _Count; set => _Count = value; }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        public List<T> Data { get => _Data; set => _Data = value; }
    }
}
