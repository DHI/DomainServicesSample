namespace ChemRegulator.WebApi.Test
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Xunit;

    [Collection("Controllers collection")]
    public class MyEntityControllerTest 
    {
        private readonly ControllersFixture _fixture;
        private readonly HttpClient _client;

        public MyEntityControllerTest(ControllersFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.Client;
        }

        [Fact]
        public async Task GetWithoutBearerTokenReturns401Unauthorized()
        {
            var client = _fixture.CreateClient();
            var response = await client.GetAsync($"api/myentities/{Guid.NewGuid()}");
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetNonExistingReturns404NotFound()
        {
            var response = await _client.GetAsync($"api/myentities/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllIsOk()
        {
            var response = await _client.GetAsync("api/myentities");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<IEnumerable<MyEntity>>(json);
            Assert.Empty(entities);
        }

        [Fact]
        public async Task GetByQueryStringIsOk()
        {
            // Add
            var request = new
            {
                Url = "/api/myentities",
                Body = new MyEntityDTO
                {
                    Name = "Entity1",
                    Foo = "Foo1"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var myEntity = JsonConvert.DeserializeObject<MyEntity>(json);

            // Query
            response = await _client.GetAsync("api/myentities?Name=Entity1");
            json = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<IEnumerable<MyEntity>>(json);
            Assert.Single(entities);

            // Delete
            response = await _client.DeleteAsync($"{request.Url}/{myEntity.Id}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task PostQueryIsOk()
        {
            // Add
            var request = new
            {
                Url = "/api/myentities",
                Body = new MyEntityDTO
                {
                    Name = "Entity1",
                    Foo = "Foo1"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var myEntity = JsonConvert.DeserializeObject<MyEntity>(json);

            // Query
            var queryRequest = new
            {
                Url = "api/myentities/query",
                Body = new List<object>
                {
                    new {Item = "Name", QueryOperator = "Equal", Value = "Entity1"}
                }
            };

            response = await _client.PostAsync(queryRequest.Url, ContentHelper.GetStringContent(queryRequest.Body));
            json = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<IEnumerable<MyEntity>>(json);
            Assert.Single(entities);

            // Delete
            response = await _client.DeleteAsync($"{request.Url}/{myEntity.Id}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact]
        public async Task AddUpdateAndDeleteIsOk()
        {
            var request = new
            {
                Url = "/api/myentities",
                Body = new MyEntityDTO
                {
                    Name = "Entity1",
                    Foo = "Foo1"
                }
            };

            // Add
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var json = await response.Content.ReadAsStringAsync();
            var myEntity = JsonConvert.DeserializeObject<MyEntity>(json);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal($"http://localhost/api/myentities/{myEntity.Id}", response.Headers.Location.ToString());
            Assert.Equal(request.Body.Foo, myEntity.Foo);

            // Update
            request.Body.Id = myEntity.Id;
            request.Body.Bar = "Bar1";
            response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _client.GetAsync($"api/myentities/{request.Body.Id}");
            json = await response.Content.ReadAsStringAsync();
            myEntity = JsonConvert.DeserializeObject<MyEntity>(json);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Bar1", myEntity.Bar);

            // Delete
            response = await _client.DeleteAsync($"{request.Url}/{myEntity.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            response = await _client.GetAsync($"{request.Url}/{myEntity.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}