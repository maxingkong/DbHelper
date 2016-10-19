/**/
/**************************************************
* 文 件 名：Recoder.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/26 11:16:47
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Data;

namespace Climb.DbProvider
{
    /// <summary>
    /// 获取数据的的方法
    /// </summary>
    public static class DataRecord
    {
        // Disallow Construction 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static string GetString(this  IDataRecord rec, int fldnum)
        {
            return rec.IsDBNull(fldnum) ? "" : rec.GetString(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static decimal GetDecimal(IDataRecord rec, int fldnum)
        {
            return rec.IsDBNull(fldnum) ? 0 : rec.GetDecimal(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static int GetInt(IDataRecord rec, int fldnum)
        {
            return rec.IsDBNull(fldnum) ? 0 : rec.GetInt32(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static bool GetBoolean(IDataRecord rec, int fldnum)
        {
            return !rec.IsDBNull(fldnum) && rec.GetBoolean(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static byte GetByte(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return 0;
            return rec.GetByte(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return new DateTime(0);
            return rec.GetDateTime(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static double GetDouble(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return 0;
            return rec.GetDouble(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static float GetFloat(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return 0;
            return rec.GetFloat(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static Guid GetGuid(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return Guid.Empty;
            return rec.GetGuid(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static Int32 GetInt32(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return 0;
            return rec.GetInt32(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static Int16 GetInt16(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return 0;
            return rec.GetInt16(fldnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldnum"></param>
        /// <returns></returns>
        public static Int64 GetInt64(IDataRecord rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum)) return 0;
            return rec.GetInt64(fldnum);
        }

        // By Name 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static string GetString(IDataRecord rec, string fldname)
        {
            return GetString(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static decimal GetDecimal(IDataRecord rec, string fldname)
        {
            return GetDecimal(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static int GetInt(IDataRecord rec, string fldname)
        {
            return GetInt(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static bool GetBoolean(IDataRecord rec, string fldname)
        {
            return GetBoolean(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static byte GetByte(IDataRecord rec, string fldname)
        {
            return GetByte(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(IDataRecord rec,
                                           string fldname)
        {
            return GetDateTime(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static double GetDouble(IDataRecord rec, string fldname)
        {
            return GetDouble(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static float GetFloat(IDataRecord rec, string fldname)
        {
            return GetFloat(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static Guid GetGuid(IDataRecord rec, string fldname)
        {
            return GetGuid(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static Int32 GetInt32(IDataRecord rec, string fldname)
        {
            return GetInt32(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static Int16 GetInt16(IDataRecord rec, string fldname)
        {
            return GetInt16(rec, rec.GetOrdinal(fldname));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="fldname"></param>
        /// <returns></returns>
        public static Int64 GetInt64(IDataRecord rec, string fldname)
        {
            return GetInt64(rec, rec.GetOrdinal(fldname));
        }
    }
}
