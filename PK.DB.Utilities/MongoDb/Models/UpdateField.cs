using System;
using System.Linq.Expressions;

namespace PK.DB.Utilities.MongoDb
{
    /// <summary>
    /// 更新字段类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UpdateField<T>
    {
        /// <summary>
        /// 字段
        /// </summary>
        public Expression<Func<T, object>> Field { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
