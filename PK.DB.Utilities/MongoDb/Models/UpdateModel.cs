using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PK.DB.Utilities.MongoDb
{
    /// <summary>
    /// 更新模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UpdateModel<T>
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
