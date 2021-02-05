namespace XB.IBM.MQ.Interfaces
{
    public interface IMqConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waitTimeMs">The time, in milliseconds, that the call waits for a message before return null. default=0: the call waits indefinitely for a message.
        /// </param>
        /// <returns>If message in the queue exists it will return that message Text, otherwise it will return null after the waiting time.</returns>
        string ReceiveMessage(long waitTimeMs = 0);
        void Commit();
        void Rollback();
    }
}
