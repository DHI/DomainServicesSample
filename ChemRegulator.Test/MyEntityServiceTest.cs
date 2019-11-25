namespace ChemRegulator.Test
{
    using System;
    using System.Linq;
    using DHI.Services;
    using Xunit;

    public class MyEntityServiceTest : IClassFixture<MyEntityServiceFixture>
    {
        private readonly MyEntityService _service;

        public MyEntityServiceTest(MyEntityServiceFixture fixture)
        {
            _service = fixture.Service;
        }

        [Fact]
        public void CreateWithNullRepositoryThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new MyEntityService(null));
        }

        [Fact]
        public void GetByQueryIsOk()
        {
            var myFooEntity = new MyEntity(Guid.NewGuid(), "Entity1") { Foo = "foo" };
            var myBarEntity = new MyEntity(Guid.NewGuid(), "Entity2") { Foo = "foo" };

            _service.Add(myFooEntity);
            _service.Add(myBarEntity);

            var query = new Query<MyEntity> {new QueryCondition("Name", QueryOperator.Equal, "Entity1")};
            Assert.Single(_service.Get(query));

            query = new Query<MyEntity> { new QueryCondition("Foo", QueryOperator.Equal, "foo") };
            Assert.Equal(2, _service.Get(query).Count());
        }
    }
}
