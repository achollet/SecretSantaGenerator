namespace SecretSanta.App.Business
{
    public interface ISecretSantaDeserializer
    {
        string LoadingExternalJsonFile();
        void ConfigurationDeserializer(string json);
    }
}