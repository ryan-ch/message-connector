using XB.MT.Parser.Model;

namespace XB.Astrea.Client
{
    public static class MessageFactory
    {
        public static Messages.Assessment.Request GetAssessmentRequest(MT103SingleCustomerCreditTransferModel mt103)
        {
            return Messages.Assessment.Factory.GetAssessmentRequest(mt103);
        }

        public static Messages.ProcessTrail.Requested GetRequestedProcessTrail(Messages.Assessment.Request assessment)
        {
            return Messages.ProcessTrail.Factory.GetRequestedProcessTrail(assessment);
        }

        public static Messages.ProcessTrail.Offered GetOfferedProcessTrail(Messages.Assessment.Response assessment)
        {
            return Messages.ProcessTrail.Factory.GetOfferedProcessTrail(assessment);
        }
    }
}
