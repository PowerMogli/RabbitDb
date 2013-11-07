﻿using System;
using System.Collections.Generic;
using System.Data;
using RabbitDB.Expressions;
using RabbitDB.Mapping;
using RabbitDB.Storage;

namespace RabbitDB.Query.Generic
{
    internal class UpdateQuery<TEntity> : IQuery
    {
        private TEntity _entity;

        internal UpdateQuery(TEntity entity)
        {
            _entity = entity;
        }

        public IDbCommand Compile(IDbProvider dbProvider)
        {
            var valuesToUpdate = Utils.Utils.GetEntityArguments(_entity, TableInfo<TEntity>.GetTableInfo);
            if (valuesToUpdate == null || valuesToUpdate.Length <= 0)
                throw new InvalidOperationException("Entity had no properties provided!");

            Tuple<string, QueryParameterCollection> result = UpdateTableBuilder<TEntity>.PrepareForUpdate(_entity, dbProvider, valuesToUpdate[0] as KeyValuePair<string, object>[]);

            SqlQuery sqlQuery = new SqlQuery(result.Item1, result.Item2);
            return sqlQuery.Compile(dbProvider);
        }
    }
}