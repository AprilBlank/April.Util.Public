using System;
using System.Collections.Generic;
using System.Text;

namespace April.Util.Entities.DB
{
    public class PageParamEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="limit">分页大小</param>
        public PageParamEntity(int index, int limit)
        {
            Index = index;
            Limit = limit;
        }

        private int _Index = 0;
        private int _Limit = 0;
        /// <summary>
        /// 页码
        /// </summary>
        public int Index { get => _Index; set => _Index = value; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int Limit { get => _Limit; set => _Limit = value; }
    }
}
