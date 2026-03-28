namespace Persistence;

public class EncryptionOptions
{
    public static readonly string SectionName = "Encryption";
    
    public required string Key { get; set; }
    
    public required string InitializationVector { get; set; }
    
    public byte[] KeyBytes => Convert.FromBase64String(Key);
    public byte[] InitializationVectorBytes => Convert.FromBase64String(InitializationVector);
}