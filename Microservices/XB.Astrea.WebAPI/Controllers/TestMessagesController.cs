using Microsoft.AspNetCore.Mvc;
using XB.IBM.MQ.Interfaces;

namespace XB.Astrea.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMessagesController : ControllerBase
    {
        private readonly IMqProducer _mqProducer;

        public TestMessagesController(IMqProducer mqProducer)
        {
            _mqProducer = mqProducer;
        }

        [HttpPost]
        public ActionResult Add([FromBody] string message = null, [FromQuery] int numberOfMessages = 1000)
        {
            message = !string.IsNullOrWhiteSpace(message)
                ? message
                : @"{1:F01ESSESES0AXXX8000427729}{2:O1030955100518ANZBAU3MAXXX76763960792009301009N}{3:{108:78}{111:001}{121:b5c8ae42-5e1b-44da-909d-c145baab4394}}{4:
:20:MG-AUD
:23B:CRED
:32A:200930AUD71,01
:33B:AUD71,01
:50K:/9991
ONE OF OUR CUSTOMERS
NAME AND ADDRESS
:52A:IRVTUS3N
:53A:ANZBAU30
:59:/54401010043
BENEFICIARY NAME
BENEFICIARY ADRESS
:70:BETORSAK RAD 1
BETORSAK RAD 2
:71A:SHA
-}{S:{MAN:UAKOUAK4600}}";
            for (int i = 0; i < numberOfMessages; i++)
            {
                _mqProducer.WriteMessage(message);

                _mqProducer.Commit();
            }

            return Ok(numberOfMessages);
        }
    }
}
