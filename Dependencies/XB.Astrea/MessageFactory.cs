namespace XB.Astrea.Client
{
    public static class MessageFactory
    {
        public static Messages.Assessment.Request GetAssessmentRequest(string mt)
        {
            return Messages.Assessment.AssessmentFactory.GetAssessmentRequest(mt);
        }

        public static Messages.ProcessTrail.Requested GetRequestedProcessTrail(Messages.Assessment.Request assessment)
        {
            return Messages.ProcessTrail.ProcessTrailFactory.GetRequestedProcessTrail(assessment);
        }

        public static Messages.ProcessTrail.Offered GetOfferedProcessTrail(Messages.Assessment.Response assessment)
        {
            return Messages.ProcessTrail.ProcessTrailFactory.GetOfferedProcessTrail(assessment);
        }
    }
}
