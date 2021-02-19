namespace XB.MtParser.Enums
{
    public enum OperationTypes
    {
        // Normal credit transfer
        CRED = 1,
        // Test message
        CRTS,
        // SWIFTPay
        SPAY,
        // Priority
        SPRI,
        // Standard
        SSTD,

        Unknown = -1
    }
}
