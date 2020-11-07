namespace XB.Astrea.Client
{
    public static class MessagesFactory
    {
        public static Messages.Assessment.Request GetAssessmentRequest(string mt)
        {
            return Messages.Assessment.AssessmentFactory.GetAssessmentRequest(mt);
        }
    }
}
