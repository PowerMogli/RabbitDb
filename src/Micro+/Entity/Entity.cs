using MicroORM.Base;
using MicroORM.Storage;

namespace MicroORM.Entity
{
    public abstract class Entity
    {
        internal IDbSession EntitySession { get; set; }
        public bool Delete { get; set; }

        public Entity()
        {
            string connectionString = Registrar<string>.GetFor(this.GetType());
            DbEngine dbEngine = Registrar<DbEngine>.GetFor(this.GetType());
            this.EntitySession = new DbSession(connectionString, dbEngine);
        }
    }
}
