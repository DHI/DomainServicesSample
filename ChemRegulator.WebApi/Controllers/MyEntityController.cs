namespace ChemRegulator.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/myentities")]
    [Authorize]
    [ApiController]
    public class MyEntitiesController : ControllerBase
    {
        private readonly MyEntityService _service;

        public MyEntitiesController(IMyEntityRepository repository)
        {
            _service = new MyEntityService(repository);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Consumes("application/json")]
        public ActionResult<MyEntity> Add([FromBody] MyEntityDTO entityDto)
        {
            var myEntity = entityDto.ToMyEntity();
            _service.Add(myEntity);
            return CreatedAtAction(nameof(Get), new { id = myEntity.Id }, myEntity);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public ActionResult<MyEntity> Update([FromBody] MyEntityDTO entityDto)
        {
            var myEntity = entityDto.ToMyEntity();
            _service.Update(myEntity);
            return Ok(_service.Get(myEntity.Id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            _service.Remove(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<MyEntity> Get(Guid id)
        {
            return Ok(_service.Get(id));
        }

        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            return Ok(_service.Count());
        }

        [HttpGet("ids")]
        public ActionResult<IEnumerable<MyEntity>> GetIds()
        {
            return Ok(_service.GetIds());
        }

        [HttpGet]
        public ActionResult<IEnumerable<MyEntity>> GetAll()
        {
            return Ok(_service.GetAll());
        }
    }
}
