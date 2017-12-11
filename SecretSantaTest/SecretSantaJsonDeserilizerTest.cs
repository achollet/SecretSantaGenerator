using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSantaApp.Business;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace SecretSantaTest
{
    [TestClass]
    public class SecretSantaJsonDeserilizerTest
    {
        [TestMethod]
        public void GetConfigurationFromJsonString()
        {
            var path = @"./SecretSanta.json"; 
            
            var json = new JsonFileLoader().GetDataFromFile(path);

            //var configuration = new Configuration
            //{
            //    MaxAmount = 10,
            //    SenderEmailAddress = "SecretSantaCheckout@gmail.com"
            //};

            //var result = GetSecretSantaAppConfiguration(json);

            //Assert.IsNotNull(result);
            //Assert.AreSame(configuration, result);
        }
    }
}
