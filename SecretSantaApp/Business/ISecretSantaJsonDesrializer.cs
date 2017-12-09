namespace SecretSantaApp.Business
{
    interface ISecretSantaJsonDeserializer
    {
        void GetSecretSantaAppConfiguration(string json);
    }
}