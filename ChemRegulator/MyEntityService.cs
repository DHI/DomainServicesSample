namespace ChemRegulator
{
    using System;
    using DHI.Services;

    public class MyEntityService : BaseUpdatableDiscreteService<MyEntity, Guid>
    {
        public MyEntityService(IMyEntityRepository repository)
            : base(repository)
        {
        }
    }
}