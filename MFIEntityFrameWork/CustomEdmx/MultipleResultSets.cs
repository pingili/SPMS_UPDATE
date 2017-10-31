using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFIEntityFrameWork
{
    /// <summary>
    /// This class is usefull when your sp is returning multiple result sets.
    /// </summary>
    public static class MultipleResultSets
    {
        public static MultipleResultSetWrapper MultipleResults(this DbContext db, string storedProcedure)
        {
            return new MultipleResultSetWrapper(db, storedProcedure);
        }

        public static MultipleResultSetWrapper MultipleResults(this DbContext db, string storedProcedure, ObjectParameter[] arrParams)
        {
            return new MultipleResultSetWrapper(db, storedProcedure, arrParams);
        }

        public static MultipleResultSetWrapper MultipleResults(this DbContext db, string storedProcedure, ObjectParameter param)
        {
            return new MultipleResultSetWrapper(db, storedProcedure, param);
        }

        public class MultipleResultSetWrapper
        {
            private readonly DbContext _db;
            private readonly string _storedProcedure;
            private readonly ObjectParameter[] _arrParams;

            public List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>> _resultSets;

            public MultipleResultSetWrapper(DbContext db, string storedProcedure)
            {
                _db = db;
                _storedProcedure = storedProcedure;
                _resultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
            }

            public MultipleResultSetWrapper(DbContext db, string storedProcedure, ObjectParameter[] arrParams)
            {
                _db = db;
                _storedProcedure = storedProcedure;
                _arrParams = arrParams;
                _resultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
            }

            public MultipleResultSetWrapper(DbContext db, string storedProcedure, ObjectParameter param)
            {
                _db = db;
                _storedProcedure = storedProcedure;
                _arrParams = new ObjectParameter[1];
                _arrParams[0] = param;

                _resultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
            }

            public MultipleResultSetWrapper With<TResult>()
            {
                _resultSets.Add((adapter, reader) => adapter
                    .ObjectContext
                    .Translate<TResult>(reader)
                    .ToList());

                return this;
            }

            public List<IEnumerable> Execute()
            {
                var results = new List<IEnumerable>();

                using (var connection = _db.Database.Connection)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = _storedProcedure;
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (ObjectParameter op in _arrParams)
                    {
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = op.Name,
                            Value = op.Value,
                            SqlValue = op.Value
                        });
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        var adapter = ((IObjectContextAdapter)_db);
                        foreach (var resultSet in _resultSets)
                        {
                            results.Add(resultSet(adapter, reader));
                            reader.NextResult();
                        }
                    }

                    return results;
                }
            }
        }
    }
}
