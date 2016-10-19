using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Climb.Dapper
{
    public interface IDapper<T>
    {
        int Insert(dynamic data);

        int Update(dynamic data, dynamic condition);

        int Delete(dynamic condition);

        int QueryCount(dynamic condition);

        int QueryMaxId(dynamic condition);

        T QueryFirst(dynamic condition, string columns = "*");

        List<T> Query(dynamic condition, string columns = "*");

        List<T> Query(dynamic condition, int limit, int offset, string columns = "*");

        List<T> Query(dynamic condition, int limit, int offset, out int allct, string columns = "*");
    }
}