namespace ChemRegulator
{
    using System;
    using System.Collections.Generic;
    using DHI.Services;

    public class FakeMyEntityRepository : FakeRepository<MyEntity, Guid>, IMyEntityRepository
    {
        public IEnumerable<MyEntity> Get(Query<MyEntity> query)
        {
            return Get(query.ToExpression());
        }
    }
}