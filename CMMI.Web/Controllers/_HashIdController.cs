using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Xml;
using CMMI.Business.Types;
using System.Xml.Serialization;

namespace CMMI.Web.Controllers
{
    [RoutePrefix("hashid")]
    public class _HashIdController : ApiController
    {


        [Route("hash/{id}")]
        public IHttpActionResult Get(ApiId id)
        {
            return Ok((long)id);
        }

        [Route("long/{id}")]
        public IHttpActionResult GetString(long id)
        {
            return Ok((string)(new ApiId(id)));
        }

        [Route("object")]
        public IHttpActionResult GetObject()
        {
            var data = new TestObject
            {
                Id = 123332,
                Text = "text"
            };
            return Ok(data);
        }

        [Route("object")]
        public IHttpActionResult PostObject(TestObject data)
        {
            return Ok((long)data.Id);
        }
    }

    [DataContract]
    public class TestObject
    {
        [DataMember]
        public ApiId Id { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}