using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GreetingController : ControllerBase
    {

        private static long _counter = 0;
        private static readonly string _template = "Hello, {0}!";
       

        [HttpGet]
        public Greeting Get([FromQuery] string name = "World")
        {
            var id = Interlocked.Increment(ref _counter);
            var content = string.Format(_template, name);
            return new Greeting(1, content);
        }
    }
}
