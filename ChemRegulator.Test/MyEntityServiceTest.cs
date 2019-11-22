namespace ChemRegulator.Test
{
    using System;
    using Xunit;

    public class MyEntityServiceTest
    {
        [Fact]
        public void CreateWithNullRepositoryThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new MyEntityService(null));
        }
    }
}
