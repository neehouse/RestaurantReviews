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
            return Ok(id.Value); // returns long value
        }

        [Route("long/{id}")]
        public IHttpActionResult GetString(long id)
        {
            return Ok((new ApiId(id)));  // returns hash string
        }

        [Route("object")]
        public IHttpActionResult GetObject()
        {
            var data = new Loan
            {
                LoanId = 123332,
                LoanNumber = 1234567890123
            };
            return Ok(data);
        }

        [Route("loan")]
        public IHttpActionResult PostObject(Loan loan)
        {
            var result = DoSomething(loan.LoanId);

            return Ok(result);
        }

        private long DoSomething(long loanId)
        {
            return default(long);
        }
    }

    [DataContract]
    public class Loan
    {
        [DataMember]
        public ApiId LoanId { get; set; }
        [DataMember]
        public long LoanNumber { get; set; }
    }
}