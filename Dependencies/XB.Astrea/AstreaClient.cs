using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XB.Astrea.Client.Exceptions;
using XB.Astrea.Client.Messages.Assessment;
using XB.Kafka;
using XB.MT.Parser.Parsers;

namespace XB.Astrea.Client
{
    public class AstreaClient : IAstreaClient
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private IProducer KafkaProducer { get; }

        public AstreaClient(IHttpClientFactory httpClientFactory, IProducer kafkaProducer)
        {
            HttpClientFactory = httpClientFactory;
            KafkaProducer = kafkaProducer;
        }

        public async Task<Response> AssessAsync(string mt)
        {
            try
            {
                var mt103 = MT103SingleCustomerCreditTransferParser.ParseMessage(mt);   

                var request = MessageFactory.GetAssessmentRequest(mt103);

                var data = new StringContent(request.Mt, Encoding.UTF8, "text/plain");

                //var result = await HttpClientFactory.CreateClient("astrea").PostAsync("/swift", data);
                var result = await HttpClientFactory.CreateClient("astrea")
                    .PostAsync("/sas/v3/assessOrders/paymentInstruction", data);

                if (!result.IsSuccessStatusCode)
                    throw new AssessmentErrorException("Request to Astrea API could not be completed");

                await SendRequestedProcessTrail(request);

                var apiResponse = await result.Content.ReadAsStringAsync();
                var assessmentResponse = JsonConvert.DeserializeObject<Response>(apiResponse);

                //TODO: Here we need to make a decision if this is a offered or rejected assessment
                SendDecisionProcessTrail(assessmentResponse);

                return assessmentResponse;
            }
            catch (Exception e)
            {
                throw new AssessmentErrorException("Something went wrong in Astrea Client", e);
            }
        }

        private async Task SendRequestedProcessTrail(Request request)
        {
            try
            {
                var requestedProcessTrail = MessageFactory.GetRequestedProcessTrail(request);
                await KafkaProducer.Execute(JsonConvert.SerializeObject(requestedProcessTrail));
            }
            catch (Exception e)
            {
                throw new ProcessTrailErrorException("Could not send requested process trail", e);
            }
        }

        private void SendDecisionProcessTrail(Response assessmentResponse)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw new ProcessTrailErrorException("Could not send decision process trail", e);
            }
        }
    }
}
