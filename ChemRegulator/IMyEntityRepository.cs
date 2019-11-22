namespace ChemRegulator
{
    using System;
    using DHI.Services;

    public interface IMyEntityRepository : IRepository<MyEntity, Guid>,
        IDiscreteRepository<MyEntity, Guid>,
        IUpdatableRepository<MyEntity, Guid>
    {
    }
}