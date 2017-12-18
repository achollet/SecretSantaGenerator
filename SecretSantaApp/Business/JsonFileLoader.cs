using System.IO;

namespace SecretSantaApp.Business
{
    public class JsonFileLoader
    {
        public string GetDataFromFile(string path)
        {
            var errorMessage = string.Format("\t /!\\ Sorry the {0} file does not exist !", path);
            
            var fileExists = File.Exists(path);
            
            var fileContains = fileExists ? File.ReadAllText(path) : errorMessage;

            return fileContains;
        }
    }
}