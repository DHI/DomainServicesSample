namespace ChemRegulator.Test
{
    public class MyEntityServiceFixture
    {
        public MyEntityServiceFixture()
        {
            Service = new MyEntityService(new FakeMyEntityRepository());
        }

        public MyEntityService Service { get; }
    }
}