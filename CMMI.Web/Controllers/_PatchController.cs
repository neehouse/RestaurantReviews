using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using CMMI.Business.Generics;
using CMMI.Business.Types;

namespace CMMI.Web.Controllers
{
    public class _PatchController : ApiController
    {
        [AllowAnonymous]
        [HttpPatch]
        [Route("patch")]
        public IHttpActionResult Patch(Delta<TestObject1> delta)
        {
            var x = GetData();

            delta.Patch(x);

            Validate(x);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(x);
        }

        private TestObject1 GetData()
        {
            return new TestObject1
            {
                Id = 12345,
                String = "String",
                Int = 12345,
                Long = 12345,
                DateTime = DateTime.Now,
                Bool = false,
                Decimal = 12345.12345m,
                Float = 12345.12345f,
                Char = 'A',
                Double = 12345.12345,
                Enum = TestEnum.A,
                Test = new TestObject2
                {
                    Int = 12345,
                    String = "String"
                }
            };
        }

    }

    public class TestObject1
    {
        public TestObject1()
        {
            TimeStamp = DateTime.Now;
        }

        public ApiId Id { get; set; }
        public string String { get; set; }
        [Range(12300,12400)]
        public int Int { get; set; }
        public DateTime DateTime { get; set; }
        public long Long { get; set; }
        public bool? Bool { get; set; }
        public decimal Decimal { get; set; }
        public float Float { get; set; }
        public char Char { get; set; }
        public double Double { get; set; }
        public TestEnum Enum { get; set; }
        public TestObject2 Test { get; set; }
        public DateTime TimeStamp { get; private set; }
    }

    public enum TestEnum
    {
        A = 1,
        B = 2,
        C = 3
    }

    public class TestObject2
    {
        public string String { get; set; }
        public int Int { get; set; }
    }
}
