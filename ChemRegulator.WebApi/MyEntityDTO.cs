namespace ChemRegulator.WebApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MyEntityDTO
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Foo { get; set; }

        public string Bar { get; set; }

        public MyEntity ToMyEntity()
        {
            var myEntity = Id is null ? new MyEntity(Guid.NewGuid(), Name) : new MyEntity((Guid)Id, Name);
            myEntity.Foo = Foo;
            myEntity.Bar = Bar;
            return myEntity;
        }
    }
}
