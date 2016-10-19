using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Climb.DbProvider;

namespace Climb.SqlDbHelper
{

    /// <summary>
    /// sql server DbHelper访问类
    /// </summary>
    public class SqlDbHelper : DbHelper, IBatcherProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public SqlDbHelper(string connection)
            : base(DbProviderFactories.GetFactory("System.Data.SqlClient"), connection)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sourceTableName"></param>
        /// <param name="batchSize"></param>
        public void InsertTable(DataTable dataTable, string sourceTableName, int batchSize = 10000)
        {
            if (dataTable == null || dataTable.Rows.Count <= 0) return;
            try
            {
                using (SqlConnection sqlConnection = (SqlConnection)DbConnection)
                {
                    sqlConnection.Open();
                    using (var bulk = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity, null)
                    {
                        DestinationTableName = sourceTableName,
                        BatchSize = batchSize
                    })
                    {
                        //循环所有列，为bulk添加映射
                        bulk.WriteToServer(dataTable);

                    }
                }
            }
            catch (SqlException sqlException)
            {
                //DbLog.Error(sqlException.Message);
            }
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public class SqlPageEntity : DbPageEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectFiled"></param>
        /// <param name="orderbyFiled"></param>
        /// <param name="tableNameWhere"></param>
        /// <param name="pageEnum"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="dataParameters"></param>
        public SqlPageEntity(string selectFiled, string orderbyFiled, string tableNameWhere, DbPageEnum pageEnum, int limit, int offset, params IDataParameter[] dataParameters)
            : base(selectFiled, orderbyFiled, tableNameWhere, pageEnum, limit, offset, dataParameters)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDataParameter[] GetPageParams()
        {
            int pageParamsLenth = (this.PageEnum == DbPageEnum.PkId) ? 1 : 2;
            IDataParameter[] dbpParameters = new IDataParameter[DbParameters.Length + pageParamsLenth];
            DbParameters.CopyTo(dbpParameters, 0);
            dbpParameters[dbpParameters.Length - 1] = new SqlParameter("@limit", Limit);
            if (this.PageEnum != DbPageEnum.PkId)
            {
                dbpParameters[dbpParameters.Length - 2] = new SqlParameter("@offset", OffSet);
            }
            return dbpParameters;
        }
        private static string SqlPage2012 = "SELECT {0} FROM {1} ORDER BY {2} offset @offset ROW FETCH NEXT @limit ROWS only";
        // private static string sqlPage2008 = ";WITH cte AS (SELECT TOP (@pageCount*@pageSize){0},ROW_NUMBER() OVER(ORDER BY {2}) AS f_num  FROM {1})SELECT * FROM  cte  WHERE f_num BETWEEN (@pageCount - 1 ) * @pageSize + 1 AND @pageCount * @pageSize ORDER BY f_num ";

        private static string sqlPageById = "SELECT TOP (@pageSize){0} FROM {1} ORDER BY {2};";


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetComandSqlStr()
        {
            string fromtSql = (this.PageEnum == DbPageEnum.PkId ? sqlPageById : SqlPage2012);
            return string.Format(fromtSql, SelectFiled, TableNameWhere, OrderByFiled);

        }
    }

}
