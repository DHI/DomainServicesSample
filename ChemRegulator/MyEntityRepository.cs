namespace ChemRegulator
{
    using System;
    using DHI.Services;

    public class MyEntityRepository : FakeRepository<MyEntity, Guid>, IMyEntityRepository
    {
    }
}