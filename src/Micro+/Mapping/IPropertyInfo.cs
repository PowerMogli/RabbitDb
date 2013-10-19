﻿using System;
using System.Data;
using MicroORM.Attributes;

namespace MicroORM.Mapping
{
    internal interface IPropertyInfo
    {
        NamedAttribute ColumnAttribute { get; }

        DbType? DbType { get; set; }

        object GetValue(object obj);

        void SetValue(object obj, object value);

        bool IsNullable { get; set; }

        bool CanWrite { get; }

        Type PropertyType { get; }

        string Name { get; }
    }
}
