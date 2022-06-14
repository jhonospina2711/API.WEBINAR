namespace Entities.Enums
{
    public class Enums
    {
        public enum States
        {
            DicomConversion = 1,
            Prediction = 2,
            WaitingForThePredictionResults = 3,
            Finalized = 4,
            Error = 5
        }
        public enum Patterns
        {
            NoCovid = 1,
            Atípico = 2,
            Indeterminado = 3,
            Típico = 4,
            NoEvaluado = 5,
            SinResultados = 6
        }
    }
}