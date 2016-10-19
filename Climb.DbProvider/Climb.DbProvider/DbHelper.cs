/**/
/**************************************************
* 文 件 名：DataRowExt.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/22 13:36:26
* 文件说明：ado.net 数据访问的方式 如果有问题请即使提交 到邮箱 或者qq 

* 修 改 人：mxk
* 修改日期：2015年4月30日 15:28:03
* 备注描述：添加sql 执行日志
*           
*************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

// ReSharper disable All

namespace Climb.DbProvider
{
    /// <summary>
    /// 数据访问基类 描述了 所有的数据访问
    /// </summary>
    public class DbHelper : IDisposable, IDbProvider
    {
        #region 全局声明
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected DbConnection DbConnection;
        /// <summary>
        /// 数据库执行对象
        /// </summary>
        protected DbCommand DbCommand;
        /// <summary>
        /// 数据库事物对象
        /// </summary>
        protected DbTransaction DbTransaction;
        /// <summary>
        /// 数据库工厂
        /// </summary>
        protected DbProviderFactory DbProviderFactory;
        /// <summary>
        /// 数据库链接 字符串
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected string dbConnectionString;//数据库链接 字符串
        /// <summary>
        /// 释放对象disposed
        /// </summary>
        protected bool Disposed;
        /// <summary>
        /// 数据适配器
        /// </summary>
        protected DbDataAdapter DbaAdapter;

        /// <summary>
        ///数据库链接字符串
        /// </summary>
        protected string DbConnectionString
        {
            get { return dbConnectionString; }
        }


        //protected static readonly ILog DbLog = Mfg.Comm.Log.LogHelper.LogHelper.getLogByConfigLogName("DbLog");

        //static readonly ILog DbQueryLog =    Mfg.Comm.Log.LogHelper.LogHelper.getLogByConfigLogName("DbQueryLog");
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbfFactory"></param>
        /// <param name="connectionString"></param>
        protected DbHelper(DbProviderFactory dbfFactory, string connectionString)
        {
            DbProviderFactory = dbfFactory;//创造对象
            dbConnectionString = connectionString;
            DbConnection = DbProviderFactory.CreateConnection();
            if (DbConnection != null) DbConnection.ConnectionString = connectionString;
        }
        #endregion

        #region debug 模式下Dbhelperlog
#if DEBUG
        /// <summary>
        /// 日志
        /// </summary>
        private  string _queryDetaiLog ="";
        /// <summary>
        /// 
        /// </summary>
        public string QueryDetaiLog
        {
            get { return _queryDetaiLog; }
            set { _queryDetaiLog = value; }
        }
        /// <summary>
        /// 获取查询日志 用于日志分析
        /// </summary>
        ///
        /// <param name="dtStart">执行的开始时间</param>
        /// <param name="dtEnd">执行结束的时间</param>
        /// <returns></returns>
        private  string GetQueryDetail(DateTime dtStart, DateTime dtEnd)
        {
            string tr = "<tr style=\"background: rgb(255, 255, 255) none repeat scroll 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">";
            string colums = "";
            string dbtypes = "";
            string values = "";
            string paramdetails = "";
            if (DbCommand.Parameters.Count > 0)
            {
                foreach (DbParameter param in DbCommand.Parameters)
                {
                    if (param == null)
                    {
                        continue;
                    }
                    colums += "<td>" + param.ParameterName + "</td>";
                    dbtypes +="<td>" + param.DbType.ToString() + "</td>";
                    values += "<td>" + param.Value + "</td>";
                }
                paramdetails = string.Format("<table width=\"100%\" cellspacing=\"1\" cellpadding=\"0\" style=\"background: rgb(255, 255, 255) none repeat scroll 0%; margin-top: 5px; font-size: 12px; display: block; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">{0}{1}</tr>{0}{2}</tr>{0}{3}</tr></table>", tr, colums, dbtypes, values);
            }

            string reult= string.Format("<center><div style=\"border: 1px solid black; background:#FFF; margin: 2px; padding: 1em; text-align: left; width: 96%; clear: both;\"><div style=\"font-size: 12px; float: right; width: 100px; margin-bottom: 5px;\"><b>TIME:</b> {0}</div><span style=\"font-size: 12px;\">{1}{2}</span></div><br /></center>", dtEnd.Subtract(dtStart).TotalMilliseconds / 1000, DbCommand.CommandText, paramdetails);
            //DbQueryLog.Warn(reult);
            return reult;
        }

#endif
        #endregion

        #region 事物的方法--------------------------------------------------------------------测试完成
        /// <summary>
        /// 开始一个ado.net 的事务
        /// </summary>
        public void BeginTrancation()
        {
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
            DbTransaction = DbConnection.BeginTransaction();

        }
        /// <summary>
        /// 提交一个事务
        /// </summary>
        public void Commit()
        {
            DbTransaction.Commit();
        }
        /// <summary>
        /// 回滚一个事务
        /// </summary>
        public void Rollback()
        {
            DbTransaction.Rollback();
        }
        #endregion

        #region 执行SQL语句-------------------------------------------------------------------测试完成

        /// <summary>
        /// 执行单条的sql 语句
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecSql(string sqlStr, params IDataParameter[] dbParameters)
        {
            int execSqlRes = -1;
            SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
            try
            {
#if DEBUG
                DateTime btime = DateTime.Now;
#endif
                execSqlRes = DbCommand.ExecuteNonQuery();
#if DEBUG
                DateTime etime = DateTime.Now;
                _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
            }
            catch (DbException dbException)
            {
                ShowException(dbException);
            }
            return execSqlRes;
        }
        #endregion

        #region 获取单条记录------------------------------------------------------------------测试完成

        /// <summary>
        /// 获取单条记录Scalar
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public object GetScalarFiled(string sqlStr, params IDataParameter[] dbParameters)
        {
            object queryObject = null;
            SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
            try
            {
#if DEBUG
                DateTime btime = DateTime.Now;
#endif
                queryObject = DbCommand.ExecuteScalar();
#if DEBUG
                DateTime etime = DateTime.Now;
                _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
            }
            catch (DbException dbException)
            {
                string msg = dbException.Message;
                ShowException(dbException);
            }
            return queryObject;
        }

        #endregion

        #region 执行存储过程
        /// <summary>
        /// 执行存储过程存储过程返回数据集
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sortedList">返回值参数</param>
        /// <param name="dbParameters">存储过程参数</param>
        /// <returns>返回数据集</returns>
        public DataSet GetSetSqlProc(string procName, SortedList<string, object> sortedList = null, params IDataParameter[] dbParameters)
        {
            DataSet dataSet = new DataSet();

            SetDbCommandOpen(procName, CommandType.StoredProcedure, dbParameters);
            SetDbAdapter(procName, CommandType.Text, dbParameters);
            if (DbaAdapter != null)
            {
                DbaAdapter.SelectCommand = DbCommand;
                try
                {
#if DEBUG
                    DateTime btime = DateTime.Now;
#endif
                    if (DbaAdapter != null) DbaAdapter.Fill(dataSet);
#if DEBUG
                    DateTime etime = DateTime.Now;
                    _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
                }
                catch (DbException dbException)
                {
                    ShowException(dbException);
                }
            }
            return dataSet;

        }
        /// <summary>
        /// 执行存储过程 
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="paramsList">返回值参数</param>
        /// <param name="dbParameters">存储过程参数</param>
        public void ExecSqlProc(string procName, SortedList<string, object> paramsList = null, params IDataParameter[] dbParameters)
        {
            List<IDbDataParameter> papersnList = new List<IDbDataParameter>();
            if (paramsList != null&& paramsList.Count>0)
            {
                papersnList.AddRange(paramsList.Select(paramsValue => AddParameterWithValue(paramsValue.Key, null, ParameterDirection.Output)));
            }
            SetDbCommandOpen(procName, CommandType.StoredProcedure, dbParameters);
            try
            {
#if DEBUG
                DateTime btime = DateTime.Now;
#endif
                DbCommand.ExecuteNonQuery();//执行sql 语句
#if DEBUG
                DateTime etime = DateTime.Now;
                _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
            }
            catch (DbException dbException)
            {
                ShowException(dbException);
            }
        }
        #endregion

        #region 获取数据集--------------------------------------------------------------------测试完成
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sqlStr, params IDataParameter[] dbParameters)
        {
            DataSet dataSet = new DataSet();
            SetDbAdapter(sqlStr, CommandType.Text, dbParameters);
            try
            {
#if DEBUG
                DateTime btime = DateTime.Now;
#endif
                if (DbaAdapter != null) DbaAdapter.Fill(dataSet);
#if DEBUG
                DateTime etime = DateTime.Now;
                _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
            }
            catch (DbException dbException)
            {
                ShowException(dbException);
            }
            return dataSet;
        }

        #endregion

        #region 获取实体类--------------------------------------------------------------------测试完成

        private DbDataReader GetDataReader(string sqlStr, params IDataParameter[] dbParameters)
        {
            DbDataReader reader = null;
            SetDbCommandOpen(sqlStr, CommandType.Text, dbParameters);
            try
            {
#if DEBUG
                DateTime btime = DateTime.Now;
#endif
                reader = DbCommand.ExecuteReader(CommandBehavior.CloseConnection);
#if DEBUG
                DateTime etime = DateTime.Now;
                _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
            }
            catch (DbException dbException)
            {
                ShowException(dbException);
            }
            return reader;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="func"></param>
        /// <param name="dbParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataInfo<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters)
        {
            var list = GetDataInfolList(sqlStr, func, dbParameters);
            var t = default(T);
            if (list != null && list.Any())
            {
                t = list.First();
            }
            return t;
        }
        /// <summary>
        /// 获取实体list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStr"></param>
        /// <param name="func"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public List<T> GetDataInfolList<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters)
        {

            DbDataReader reader = GetDataReader(sqlStr, dbParameters);
            var list = new List<T>();
            //if (reader == null || !reader.HasRows) return list;
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(func(reader));
                }
            }
            return list;
        }

        #endregion

        #region 执行存储过程------------------------------------------------------------------测试完成
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="dbParameters"></param>
        public void ExecSqlProc(string procName, params IDataParameter[] dbParameters)
        {
            SetDbCommandOpen(procName, CommandType.StoredProcedure, dbParameters);
            try
            {
#if DEBUG
                DateTime btime = DateTime.Now;
#endif
                DbCommand.ExecuteNonQuery();//执行sql 语句
#if DEBUG
                DateTime etime = DateTime.Now;
                _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
            }
            catch (DbException dbException)
            {
                ShowException(dbException);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataSet GetSetSqlProc(string procName, params IDataParameter[] dbParameters)
        {
            DataSet dataSet = new DataSet();

            SetDbCommandOpen(procName, CommandType.StoredProcedure, dbParameters);
            DbaAdapter = DbProviderFactory.CreateDataAdapter();
            if (DbaAdapter != null)
            {
                DbaAdapter.SelectCommand = DbCommand;
                try
                {
#if DEBUG
                    DateTime btime = DateTime.Now;
#endif
                    if (DbaAdapter != null) DbaAdapter.Fill(dataSet);
#if DEBUG
                    DateTime etime = DateTime.Now;
                    _queryDetaiLog += GetQueryDetail(btime, etime);
#endif
                }
                catch (DbException dbException)
                {
                    ShowException(dbException);
                }
            }
            return dataSet;
        }

        #endregion

        #region 分页
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbPage"></param>
        /// <returns></returns>
        public DataSet GetPageSet(DbPageEntity dbPage)
        {
            string sqlstr = dbPage.GetComandSqlStr();
            IDataParameter[] parameters = dbPage.GetPageParams();
            DataSet dbSet = GetDataSet(sqlstr, parameters);
            return dbSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbPage"></param>
        /// <param name="tFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetPageList<T>( DbPageEntity dbPage, Func<IDataReader, T> tFunc)
        {

            string sqlstr = dbPage.GetComandSqlStr();
            IDataParameter[] parameters = dbPage.GetPageParams();
            var list = GetDataInfolList(sqlstr, tFunc, parameters);
            return list;
        }
        #endregion

        #region 公共方法

        #region 显示错误消息
        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="dbException">异常信息</param>
        private void ShowException(DbException dbException)
        {
            StringBuilder errorBuilder = new StringBuilder();
            errorBuilder.Append(DateTime.Now);
            errorBuilder.Append("错误信息：");
            errorBuilder.Append(dbException.Message);
            errorBuilder.Append("\t");
            errorBuilder.AppendLine("Sql语句：");
            errorBuilder.Append(DbCommand.CommandText);
            errorBuilder.AppendLine("参数信息：");
            errorBuilder.AppendLine("参数名字\t参数类型\t参数值");
            foreach (DbParameter param in DbCommand.Parameters)
            {
                if (param == null)
                {
                    continue;
                }
                errorBuilder.AppendLine(param.ParameterName);
                errorBuilder.Append("\t");
                errorBuilder.Append(param.DbType);
                errorBuilder.Append("\t");
                errorBuilder.Append(param.Value);
            }

           // DbLog.Error(errorBuilder);
        }
        #endregion

        #region  实现 IDisposable

        ///析构函数
        ~DbHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// 公开dispose方法
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// dispose方法
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                if (DbConnection != null)
                {
                    DbConnection.Dispose();
                }
                if (DbCommand != null)
                {
                    //_dbCommand.Parameters.Clear();
                    DbCommand.Dispose();
                }
                if (DbTransaction != null)
                {
                    DbTransaction.Dispose();
                }
                if (DbaAdapter != null)
                {
                    DbaAdapter.Dispose();
                }
            }
            DbaAdapter = null;
            DbConnection = null;
            DbCommand = null;
            DbTransaction = null;
            Disposed = true;

        }

        /// <summary>
        /// 虚函数 获取访问类的params参数
        /// </summary>
        /// <returns></returns>
        protected virtual DbParameter GetDbParameter()
        {
            return null;
        }

        /// <summary>
        /// 实现close方法
        /// </summary>
        public void Close()
        {
            ((IDisposable)this).Dispose();
        }
        #endregion

        #region  返回params函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public  DbParameter AddOutParameter(string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = GetDbParameter();
            if (dbParameter == null) return null;
            dbParameter.DbType = dbType;

            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;

            return dbParameter;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public  DbParameter AddInParameter(string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = GetDbParameter();
            if (dbParameter == null) return null;
            dbParameter.DbType = dbType;

            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;

            return dbParameter;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public  DbParameter AddReturnParameter(string parameterName, DbType dbType)
        {
            DbParameter dbParameter = GetDbParameter();
            if (dbParameter == null) return null;
            dbParameter.DbType = dbType;

            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            return dbParameter;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="parameterDirection"></param>
        /// <returns></returns>
        public  DbParameter AddParameterWithValue(string parameterName, object value, ParameterDirection parameterDirection= ParameterDirection.Input)
        {
            DbParameter dbParameter = GetDbParameter();
            if (dbParameter == null) return null;

            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;

            return dbParameter;

        }

        #endregion

        #region SetCommand and  Adapter

        /// <summary>
        /// 设置dbcommand
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        private void SetDbCommandOpen(string sqlStr, CommandType commandType, params IDataParameter[] dbParameters)
        {
            DbCommand = DbProviderFactory.CreateCommand();
            if (DbCommand != null)
            {
                DbCommand.Connection = DbConnection;
                DbCommand.CommandText = sqlStr;
                DbCommand.CommandType = commandType;
                if (dbParameters.Length > 0)
                {
                    DbCommand.Parameters.AddRange(dbParameters);

                    //foreach (var item in dbParameters)
                    //{
                    //    DbCommand.Parameters.Add(item);
                    //}
                }
            }
            if (DbConnection.State == ConnectionState.Open) return;
            try
            {
                DbConnection.Open();
            }
            catch (DbException dbException)
            {
                ShowException(dbException);
            }
        }
        /// <summary>
        /// 初始化dataadapter yxk
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        private void SetDbAdapter(string sqlStr, CommandType commandType, IDataParameter[] dbParameters)
        {

            SetDbCommandOpen(sqlStr, commandType, dbParameters);
            DbaAdapter = DbProviderFactory.CreateDataAdapter();
            if (DbaAdapter != null) DbaAdapter.SelectCommand = DbCommand;
        }


        #endregion

        #endregion
    }
}