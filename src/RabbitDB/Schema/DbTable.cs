﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbTable.cs" company="">
//   
// </copyright>
// <summary>
//   The db table.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using RabbitDB.Contracts.Schema;

namespace RabbitDB.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The db table.
    /// </summary>
    internal class DbTable : IDbTable
    {
        #region Fields

        /// <summary>
        /// The _primary keys.
        /// </summary>
        private List<IDbColumn> _primaryKeys;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable"/> class.
        /// </summary>
        internal DbTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable"/> class.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        internal DbTable(string tableName)
        {
            Name = tableName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the clean name.
        /// </summary>
        public string CleanName { get; set; }

        /// <summary>
        /// Gets or sets the db columns.
        /// </summary>
        public List<IDbColumn> DbColumns { get; set; }

        // public List<DbTableIndex> Indices { get; set; }
        /// <summary>
        /// Gets or sets the foreign keys.
        /// </summary>
        public List<DbForeignKey> ForeignKeys { get; set; }

        /// <summary>
        /// Gets a value indicating whether has primary keys.
        /// </summary>
        public bool HasPrimaryKeys => PrimaryKeys != null && PrimaryKeys.Count > 0;

        /// <summary>
        /// Gets or sets a value indicating whether ignore.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is view.
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the primary keys.
        /// </summary>
        public List<IDbColumn> PrimaryKeys
        {
            get
            {
                return _primaryKeys ?? (_primaryKeys = DbColumns.Where(column => column.IsPrimaryKey).ToList());
            }
        }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the sequence name.
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// Gets or sets the sql.
        /// </summary>
        public string Sql { get; set; }

        #endregion

        #region Public Indexers

        /// <summary>
        /// The 
        /// </summary>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DbColumn"/>.
        /// </returns>
        public IDbColumn this[string columnName] => GetColumn(columnName);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get column.
        /// </summary>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DbColumn"/>.
        /// </returns>
        public IDbColumn GetColumn(string columnName)
        {
            return
                DbColumns.Single(column => string.Compare(column.Name, columnName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        #endregion

        // public DbTableIndex GetIndex(string indexName)
        // {
        // return Indices.Single(tableIndex => string.Compare(tableIndex.Name, indexName, true) == 0);
        // }
        #region Methods

        /// <summary>
        /// The skip while.
        /// </summary>
        /// <param name="resolvedColumnName">
        /// The resolved column name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        internal bool SkipWhile(string resolvedColumnName)
        {
            return DbColumns.Find(dbColumn => dbColumn.IsToSkip(resolvedColumnName)) != null;
        }

        #endregion
    }
}