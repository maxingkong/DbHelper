/**/
/**************************************************
* 文 件 名：Class2.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/26 16:23:15
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/

using System.Data;

namespace Climb.DbProvider
{
    /// <summary>
    /// 向数据库中批量插入数据
    /// </summary>
    public interface IBatcherProvider
    {
        /// <summary>
        /// 将的数据批量插入到数据库中。
        /// </summary>
        /// <param name="dataTable">要批量插入的 Datable</param>
        /// <param name="sourceTableName">数据源表的名称</param>
        /// <param name="batchSize">每批次写入的数据量。默认是1w条数据</param>
        void InsertTable(DataTable dataTable,string sourceTableName, int batchSize = 10000);
    }
}
