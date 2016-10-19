/**/
/**************************************************
* 文 件 名：DbPageEntity.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/26 15:37:00
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
    /// 分页枚举
    /// </summary>
    public enum DbPageEnum : byte
    {
        /// <summary>
        /// 普通分页
        /// </summary>
        Nomal,
        /// <summary>
        /// 按照id检索分页
        /// </summary>
        PkId
    }
    /// <summary>
    /// 分页对象
    /// </summary>
    public abstract class DbPageEntity
    {
        #region $sql分页语句

        //private static string _SqlPage2012 = "SELECT {0} FROM {1} ORDER BY {2} offset (@pageCount-1)*@pageSize ROW FETCH NEXT @pageSize ROWS only";
        //private static string _sqlPage2008 = ";WITH cte AS (SELECT TOP (@pageCount*@pageSize){0},ROW_NUMBER() OVER(ORDER BY {2}) AS f_num  FROM {1})SELECT * FROM  cte  WHERE f_num BETWEEN (@pageCount - 1 ) * @pageSize + 1 AND @pageCount * @pageSize ORDER BY f_num ";

        //private static string _sqlPageById = "SELECT TOP (@pageSize){0} FROM {1} ORDER BY {2};";

        #endregion

        #region 属性
        /// <summary>
        /// 起始的偏移量
        /// </summary>
        public int OffSet { get; set; }
        /// <summary>
        /// 从offset开始获取控制的数量
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 查询字段
        /// </summary>
        public string SelectFiled { get; private set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderByFiled { get; private set; }
        /// <summary>
        /// 表名称以及条件
        /// </summary>
        public string TableNameWhere { get; private set; }

        /// <summary>
        /// 数据访问的参数
        /// </summary>
        public IDataParameter[] DbParameters { get;  set; }

        /// <summary>
        /// 分页枚举
        /// </summary>
        public DbPageEnum PageEnum { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="selectFiled"></param>
        /// <param name="orderbyFiled"></param>
        /// <param name="tableNameWhere"></param>
        /// <param name="pageEnum"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="dataParameters"></param>
        protected DbPageEntity(string selectFiled, string orderbyFiled, string tableNameWhere,DbPageEnum pageEnum=DbPageEnum.Nomal, int limit = 10,
            int offset = 0, params IDataParameter[] dataParameters)
        {
            Limit = limit;
            OffSet = offset;
            SelectFiled = selectFiled;
            TableNameWhere = tableNameWhere;
            OrderByFiled = orderbyFiled;
            DbParameters = dataParameters;
        }

        #endregion

        #region 保护虚函数
        /// <summary>
        /// 虚函数获取参数
        /// </summary>
        /// <returns>返回参数</returns>
        public abstract IDataParameter[] GetPageParams();

        /// <summary>
        /// 获取分页执行的sql 语句
        /// </summary>
        /// <returns>返回sql语句</returns>
        public abstract string GetComandSqlStr();

        #endregion

    }
}