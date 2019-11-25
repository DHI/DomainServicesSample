namespace ChemRegulator
{
    using System;
    using System.Collections.Generic;
    using DHI.Services;

    public interface IMyEntityRepository : IRepository<MyEntity, Guid>,
        IDiscreteRepository<MyEntity, Guid>,
        IUpdatableRepository<MyEntity, Guid>
    {
        IEnumerable<MyEntity> Get(Query<MyEntity> query);
    }
}