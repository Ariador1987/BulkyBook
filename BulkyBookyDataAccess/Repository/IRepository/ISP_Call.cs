using Dapper;
using System;
using System.Collections.Generic;

namespace BulkyBooky.DataAccess.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        // single koristi execute scalar, koji vraća INT ili BOOL vrijednost
        T Single<T>(string procedureName, DynamicParameters param = null);

        void Execute(string procedureName, DynamicParameters param = null);
        // OneRecord vraća cijeli jedan red.
        T OneRecord<T>(string procedureName, DynamicParameters param = null);

        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);
        // Za vratit 2 stola koriste Tuple
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
    }
}
