namespace ChemRegulator.WebApi.Test
{
    using System.Net.Http;
    using System.Text;
    using Newtonsoft.Json;

    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}