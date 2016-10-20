using System;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Climb.DbProvider
{
    /// <summary>
    /// 实现ADO.NET 数据作接口类
    /// </summary>
    public interface IDbProvider
    {
        #region 添加参数
        /// <summary>
        /// 获得dbParams对象参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns>返回dbparams对象</returns>
        DbParameter AddInParameter(string parameterName, DbType dbType, object value);

        /// <summary>
        /// 获得带有输出参数的dbParams对象参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">长度</param>
        /// <returns>返回dbparams对象</returns>
        DbParameter AddOutParameter(string parameterName, DbType dbType, int size);

        /// <summary>
        /// 获取返回参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">返回类型</param>
        /// <returns>返回dbparams对象</returns>
        DbParameter AddReturnParameter(string parameterName, DbType dbType);

        /// <summary>
        /// 获取返回参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="parameterDirection">参数传入方向</param>
        /// <returns>返回dbparams对象</returns>
        DbParameter AddParameterWithValue(string parameterName, object value, ParameterDirection parameterDirection = ParameterDirection.Input);
        #endregion

        #region   执行sql
        /// <summary>
        /// 执行一个sql 语句 update save delete
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="dbParameters">参数列表集合</param>
        /// <returns></returns>
        int ExecSql(string sqlStr, params IDataParameter[] dbParameters);

        /// <summary>
        /// 异步执行一个sql 语句 update save delete
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="dbParameters">参数列表集合</param>
        /// <returns></returns>
        int ExecSqlAsync(string sqlStr, params IDataParameter[] dbParameters);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="func">赋值方法</param>
        /// <param name="dbParameters">参数集合</param>
        /// <typeparam name="T">返回一个类的对象</typeparam>
        /// <returns>返回一个类的对象</returns>
        T GetDataInfo<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters);


        /// <summary>
        /// 从数据库获取对象集合
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="func">赋值方法</param>
        /// <param name="dbParameters">参数集合</param>
        /// <typeparam name="T">T对象</typeparam>
        /// <returns>返回一个类对象的集合</returns>
        List<T> GetDataInfolList<T>(string sqlStr, Func<IDataReader, T> func, params IDataParameter[] dbParameters);


        /// <summary>
        /// 从数据库获取数据集DataSet
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="dbParameters">参数集合</param>
        /// <returns>返回一个数据集合dataset</returns>
        DataSet GetDataSet(string sqlStr, params IDataParameter[] dbParameters);

        /// <summary>
        /// 获取单条记录 单个字段值
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="dbParameters">参数集合</param>
        /// <returns>返回object对象</returns>
        object GetScalarFiled(string sqlStr, params IDataParameter[] dbParameters);

        #endregion

        #region 执行存储过程
        /// <summary>
        /// 返回一个数据集合
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sortedList">返回参数集合</param>
        /// <param name="dbParameters">参数集合</param>
        /// <returns>存储过程的集合</returns>
        DataSet GetSetSqlProc(string procName, SortedList<string, object> sortedList = null, params IDataParameter[] dbParameters);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sortedList">返回参数集合</param>
        /// <param name="dbParameters">参数集合</param>
        void ExecSqlProc(string procName, SortedList<string, object> sortedList = null, params IDataParameter[] dbParameters);
        #endregion

        #region 执行分页
        /// <summary>
        /// 获取分页数据对象
        /// </summary>
        /// <typeparam name="T">T类型对象</typeparam>
        /// <param name="dbPage">分页对象</param>
        /// <param name="tFunc">赋值方法</param>
        /// <returns>返回分页的数据集</returns>
        List<T> GetPageList<T>(DbPageEntity dbPage, Func<IDataReader, T> tFunc);

        /// <summary>
        /// 分页获取数据集
        /// </summary>
        /// <param name="dbPage">分页对象</param>
        /// <returns>返回分页的数据集</returns>
        DataSet GetPageSet(DbPageEntity dbPage);
        #endregion

        #region 释放资源
        /// <summary>
        /// 关闭方法
        /// </summary>
        void Close();

        /// <summary>
        /// 释放对象资源
        /// </summary>
        void Dispose();
        #endregion

        #region 处理事物
        /// <summary>
        /// 开始一个事物
        /// </summary>
        void BeginTrancation();

        /// <summary>
        /// 提交一个事物
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚一个事物
        /// </summary>
        void Rollback();
        #endregion
    }
}
