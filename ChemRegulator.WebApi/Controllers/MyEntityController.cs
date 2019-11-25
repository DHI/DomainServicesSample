namespace ChemRegulator.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DHI.Services;
    using DHI.Services.WebApiCore;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public ActionResult<MyEntity> Update([FromBody] MyEntityDTO entityDto)
        {
            var myEntity = entityDto.ToMyEntity();
            _service.Update(myEntity);
            return Ok(_service.Get(myEntity.Id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(Guid id)
        {
            _service.Remove(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<MyEntity> Get(Guid id)
        {
            return Ok(_service.Get(id));
        }

        [HttpPost("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public ActionResult<IEnumerable<MyEntity>> GetByQuery([FromBody] QueryDTO<MyEntity> queryDto)
        {
            return Ok(_service.Get(queryDto.ToQuery()));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MyEntity>> GetByQueryString()
        {
            var query = new Query<MyEntity>();
            foreach (var condition in Request.Query)
            {
                var queryCondition = new QueryCondition(condition.Key, condition.Value.ToString().ToObject());
                query.Add(queryCondition);
            }

            return Ok(query.Any() ? _service.Get(query) : _service.GetAll());
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetCount()
        {
            return Ok(_service.Count());
        }

        [HttpGet("ids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MyEntity>> GetIds()
        {
            return Ok(_service.GetIds());
        }
    }
}
