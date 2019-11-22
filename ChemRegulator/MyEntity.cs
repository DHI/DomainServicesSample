namespace ChemRegulator
{
    using System;
    using DHI.Services;

    [Serializable]
    public class MyEntity : BaseNamedEntity<Guid>
    {
        public MyEntity(Guid id, string name) : base(id, name)
        {
        }

        public string Foo { get; set; }

        public string Bar { get; set; }
    }
}
