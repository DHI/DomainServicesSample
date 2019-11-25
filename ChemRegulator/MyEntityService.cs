namespace ChemRegulator
{
    using System;
    using System.Collections.Generic;
    using DHI.Services;

    public class MyEntityService : BaseUpdatableDiscreteService<MyEntity, Guid>
    {
        private readonly IMyEntityRepository _repository;

        public MyEntityService(IMyEntityRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<MyEntity> Get(Query<MyEntity> query)
        {
            return _repository.Get(query);
        }
    }
}