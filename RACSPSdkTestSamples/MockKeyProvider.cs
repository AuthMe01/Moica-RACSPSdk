using System;

namespace RACSPSdkSample.samples
{
    public class MockKeyProvider
    {
        

        // The service id and api key is just for testing, wont working in any enviroment.
        // you should replace the key with the one you get from MOICA service provider. 
        
        public static String TestAPIHost { get; set; } = "https://fido-test.moi.gov.tw/moise/";
        public static String ServiceId { get; set; } = "20000301144513184060";
        public static String ApiKey { get; set; } = "uGaNBzALOQGKJ-KZ3E1WwkddAUbulI2qp6oEsoLayVk";
    }
}