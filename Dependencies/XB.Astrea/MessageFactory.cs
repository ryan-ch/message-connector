using XB.MT.Parser.Model;

namespace XB.Astrea.Client
{
    public static class MessageFactory
    {
        public static Messages.Assessment.AssessmentRequest GetAssessmentRequest(MT103SingleCustomerCreditTransferModel mt103)
        {
            return Messages.Assessment.AssessmentGenerator.GetAssessmentRequest(mt103);
        }

        public static Messages.ProcessTrail.RequestedProcessTrail GetRequestedProcessTrail(Messages.Assessment.AssessmentRequest assessment)
        {
            return Messages.ProcessTrail.ProcessTrailGenerator.GetRequestedProcessTrail(assessment);
        }

        public static Messages.ProcessTrail.OfferedProcessTrail GetOfferedProcessTrail(Messages.Assessment.AssessmentResponse assessment)
        {
            return Messages.ProcessTrail.ProcessTrailGenerator.GetOfferedProcessTrail(assessment);
        }
    }
}
