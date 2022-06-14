namespace Entities.Enums
{
    public class ConfigurationEnum
    {
        public static string BaseConnAudit = "";
        public static string ApiConverterDicom = "https://prod-21.eastus.logic.azure.com:443/workflows/22ddbf55a102418dbe4b1b8c9d5cd451/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=OZUY1XbFQ6oh5gSeiDMXJkC6NG9Ggus1NZmdvo8pbNU";
        public static string BaseModelAIUrl = "http://ccovidia.eastus.azurecontainer.io";
        public static string Prediction = "/predecir";
        public static string Folder = "/carpeta";
        public static string ResultPrediction = "/prediccion";
        public static string ModelAIToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE1OTA3ODM2NzAsImp0aSI6ImM4NDQ3ODY1LWE2ZmItNDM5Ni04Y2M3LTUyNDVjYTIwZDk0MiIsImlkZW50aXR5IjoidXJnZW5jaWFzY2xpbmljYWxhc2FtZXJpY2FzIiwidHlwZSI6ImFjY2VzcyIsImZyZXNoIjpmYWxzZSwibmJmIjoxNTkwNzgzNjcwfQ.GUvFcrBtxMlTI3byp0FDjI2M1YVXIqFGBFQ-uz_k1Dg";
        public static string ApiUrl = "https://web-site-covidia.azurewebsites.net";
        #region Database
        public static string Server = "ConnectionStrings:Chatbot-AzureDB-Server";
        public static string Bd = "ConnectionStrings:Chatbot-AzureDB-Bd";
        public static string User = "ConnectionStrings:Chatbot-AzureDB-User";
        public static string Password = "ConnectionStrings:Chatbot-AzureDB-Password";
        #endregion
    }
}