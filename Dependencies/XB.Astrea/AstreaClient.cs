using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XB.Astrea.Client.Exceptions;
using XB.Astrea.Client.Messages.Assessment;
using XB.Astrea.Client.Messages.ProcessTrail;
using XB.Kafka;
using XB.MT.Parser.Parsers;

namespace XB.Astrea.Client
{
    public class AstreaClient : IAstreaClient
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private IKafkaProducer KafkaProducer { get; }

        public AstreaClient(IHttpClientFactory httpClientFactory, IKafkaProducer kafkaProducer)
        {
            HttpClientFactory = httpClientFactory;
            KafkaProducer = kafkaProducer;
        }

        public async Task<AssessmentResponse> AssessAsync(string mt)
        {
            try
            {
                var mt103 = MT103SingleCustomerCreditTransferParser.ParseMessage(mt);

                var request = new AssessmentRequest(mt103);

                var data = new StringContent(request.Mt, Encoding.UTF8, "text/plain");

                //var result = await HttpClientFactory.CreateClient("astrea").PostAsync("/swift", data);
                var result = await HttpClientFactory.CreateClient("astrea")
                    .PostAsync("/sas/v3/assessOrders/paymentInstruction", data);

                if (!result.IsSuccessStatusCode)
                    throw new AssessmentErrorException("Request to Astrea API could not be completed");

                await SendRequestedProcessTrail(request);

                var apiResponse = await result.Content.ReadAsStringAsync();
                var assessmentResponse = JsonConvert.DeserializeObject<AssessmentResponse>(apiResponse);

                //TODO: Here we need to make a decision if this is a offered or rejected assessment
                SendDecisionProcessTrail(assessmentResponse);

                return assessmentResponse;
            }
            catch (Exception e)
            {
                throw new AssessmentErrorException("Something went wrong in Astrea Client", e);
            }
        }

        private async Task SendRequestedProcessTrail(AssessmentRequest request)
        {
            try
            {
                //Todo: is this ProcessTrailRequest or RequestedProcessTrail ?
                var requestedProcessTrail = new RequestedProcessTrail(request);
                await KafkaProducer.Execute(JsonConvert.SerializeObject(requestedProcessTrail));
            }
            catch (Exception e)
            {
                throw new ProcessTrailErrorException("Could not send requested process trail", e);
            }
        }

        private void SendDecisionProcessTrail(AssessmentResponse assessmentResponse)
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
