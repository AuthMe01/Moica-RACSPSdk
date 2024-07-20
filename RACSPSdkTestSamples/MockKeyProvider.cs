using System;

namespace RACSPSdkSample.samples
{
    public class MockKeyProvider
    {
        // The service id and api key is just for testing, wont working in any enviroment.
        // you should replace the key with the one you get from MOICA service provider. 

        // public static string TestAPIHost { get; set; } = "https://fido-test.moi.gov.tw/moise/";
        
        public static string TestAPIHost { get; set; } = "http://localhost:8080/moise/";
        public static string ServiceId { get; set; } = "20000301144513184060";
        public static string ApiKey { get; set; } = "";

        public static string IdNum { get; set; } = "";

        public static String EncCert { get; set; } =
            "MIIF3TCCA8WgAwIBAgIRAIjQWhyLzSgKxzh+yPQCYNQwDQYJKoZIhvcNAQELBQAwUzELMAkGA1UEBhMCVFcxEjAQBgNVBAoMCeihjOaUv+mZojEwMC4GA1UECwwnKOa4rOippueUqCkg5YWn5pS/6YOo5oaR6K2J566h55CG5Lit5b+DMB4XDTI0MDcwODAzMjE0M1oXDTI0MDkwODE1NTk1OVowPDELMAkGA1UEBhMCVFcxEjAQBgNVBAMMCea4rOippuS6ujEZMBcGA1UEBRMQMTA2MTcyMDQ3MjAwNDgwOTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAIR5pX6LW4JkkOLJUrXyo1drQlSMrcilrczuNWW0NPdo6l5VXfz+3Q71dhBWrvViJ6j7F3mSvsGqk2UVpAM6mHaAxyxDqQxlkd6pyfv0jOYDi6LP1FVn9l9BZH5HkUBcvJXD693lgQfJbAaBClSkX0A9LnZvatNmqEWzt090zMxbHl157d/Guvz62LZOsRXLa/gLI6PcMXRcHH9p14N56vo8AryiBI+9dBOa7/l2Vo2iw2vJn2y7NBiGrsIQ+6BJryL8IuzGpfkQ9/uuYfavgId0cQC1GnzkoEDcfCDY0AcnhLPq7TyZTX4Y5hGGjC7HcVSvVfwAD1Czpcw0GSTB8+kCAwEAAaOCAcEwggG9MB8GA1UdIwQYMBaAFOp6Sqt4wtqSKOBRGuPbcE+MAbVYMB0GA1UdDgQWBBROJuhfyNRqNr6J9PXv4dps9IFVrzAOBgNVHQ8BAf8EBAMCBDAwFAYDVR0gBA0wCzAJBgdghnZlAAMDMEgGA1UdCQRBMD8wFQYHYIZ2AWQCATEKBghghnYBZAMBATATBgdghnYBZAICMQgTBm1vYmlsZTARBgdghnYBZAIzMQYMBDIxMTYwgYEGA1UdHwR6MHgwOKA2oDSGMmh0dHA6Ly9vY3NwLW1vaWNhLm1vaS5nb3YudHcvY3JsL01PSUNBLUczLTEwLTguY3JsMDygOqA4hjZodHRwOi8vb2NzcC1tb2ljYS5tb2kuZ292LnR3L2NybC9NT0lDQS1HMy1jb21wbGV0ZS5jcmwwgYYGCCsGAQUFBwEBBHoweDBHBggrBgEFBQcwAoY7aHR0cDovL21vaWNhLm5hdC5nb3YudHcvcmVwb3NpdG9yeS9DZXJ0cy9Jc3N1ZWRUb1RoaXNDQS5wN2IwLQYIKwYBBQUHMAGGIWh0dHA6Ly9tb2ljYS5uYXQuZ292LnR3L09DU1Avb2NzcDANBgkqhkiG9w0BAQsFAAOCAgEAqVfwTs+DCFVGp3xYRdyWU+mfcKGwBvoBd60tlxo/tscnHnpXc2Nz0p4Hn+6c8mOCQ9y8vUGLJrXPo3b1pc/1gMCA+cQElzivmIL/CqALRLsp69JnCNisjsOqJZwGXqFdp+u8sE3X9gpZdGdh9vqjC4rspeOzz083gCOviublLxYOA5MbM+TK8UVw1bKckx+kKQ3IgoApWfSusvMboH62TzKaTKaG3Le+ISMVvNUv/hMrfmLnfoXVd+WpQYGCKRMBUczORelI9oTjzKNvyRYtT1XIsh3X0IDbx12UUVIDX3uT7vUCJsZyyxDjiPuv/+kCCRgqJPo3cvJlB3e6Haj1U/bsahTdqsMIHY7pprsZ3iCnr8rtcLI+G29oCZxMogFf/4e8tCPhiM7Cv9OOuPtfqMs5I5B/WTw2fKzac0Z3stooe/wBn20cFIRGHQQq6CiLUftuLydNivjbl7M6kXQ07q8U05Ta5ZBtEfbS3/+JEFLqalSegZWtSo5UWO7JxCkwlWSmh8wQdT3uqOSMeeLu+mfWuLmEbAkUpl7RGCkL4v6nAvnV3KBj3DtAUqrnWvSqrTnZanFHgfbGVEYkQ2e8bCtXSKXhmUW/vt5RS7eAUrNzlGjk6Ebjq8QCSbJrVLgOCGGeVJUYx0Mva/9GeNgWe7+I7cquRd1WpggaEnMviFs=";
        public static String EncCertSN { get; set; } = "88D05A1C8BCD280AC7387EC8F40260D4";
        
    }
}