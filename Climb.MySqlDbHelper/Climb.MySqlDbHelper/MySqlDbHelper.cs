using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Climb.DbProvider;
using MySql.Data.MySqlClient;

namespace Climb.MySqlDbHelper
{
    /// <summary>
    /// MysqlDbhelper 访问类
    /// </summary>
    public sealed class MySqlDbHelper : DbHelper, IBatcherProvider
    {
        /// <summary>
        /// 构造函数 创建mysqlDbherper访问对象
        /// </summary>
        /// <param name="connectionstring">数据库连接字符串</param>
        public MySqlDbHelper(string connectionstring)
            : base(DbProviderFactories.GetFactory(new MySqlConnection()), connectionstring)
        {
         
        }


        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符,列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义,并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    DataColumn colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sourceTableName"></param>
        /// <param name="batchSize"></param>
        public void InsertTable(DataTable dataTable, string sourceTableName, int batchSize = 10000)
        {
            if (dataTable.Rows.Count == 0) return;

            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(dataTable);
            File.WriteAllText(tmpPath, csv);
            using (MySqlConnection conn = new MySqlConnection(DbConnectionString))
            {
                MySqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\n",//\n 兼容 linux 操作系统
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = dataTable.TableName,
                    };
                    bulk.Columns.AddRange(dataTable.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToArray());
                    bulk.Load();
                    tran.Commit();
                }
                catch (MySqlException mySqlException)
                {
                    if (tran != null) tran.Rollback();

                    //DbLog.Error(mySqlException.Message);
                    throw;
                }
            }
            File.Delete(tmpPath);
        }

    }


    /// <summary>
    /// mysql分页对象
    /// </summary>
    public class MySqlPageEntity : DbPageEntity
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
        public MySqlPageEntity(string selectFiled, string orderbyFiled, string tableNameWhere, DbPageEnum pageEnum, int limit, int offset, params IDataParameter[] dataParameters)
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
            dbpParameters[dbpParameters.Length - 1] = new MySqlParameter("?limit", Limit);
            if (this.PageEnum != DbPageEnum.PkId)
            {
                dbpParameters[dbpParameters.Length - 2] = new MySqlParameter("?offset", OffSet);
            }
            return dbpParameters;
        }
        //private static string SqlPage2012 = "SELECT {0} FROM {1} ORDER BY {2} offset (@pageCount-1)*@pageSize ROW FETCH NEXT @pageSize ROWS only";
        private const string SqlPage = "SELECT  {0} FROM {1} {2} limit ?offset,?limit;";
        private const string SqlPageById = "SELECT {0} FROM {1}  {2} limit ?limit;";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetComandSqlStr()
        {
            string fromtSql = (this.PageEnum == DbPageEnum.PkId ? SqlPageById : SqlPage);
            string orderbysql = "";
            if (!string.IsNullOrEmpty(OrderByFiled))
            {
                orderbysql = "  ORDER BY " + OrderByFiled;
            }
            string sqlStr = string.Format(fromtSql, SelectFiled, TableNameWhere, orderbysql);
            return sqlStr;
        }




    }

}
