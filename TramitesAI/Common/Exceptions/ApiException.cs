using System.Numerics;

namespace TramitesAI.Common.Exceptions
{
    public class ApiException : Exception
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int StatusCode { get; set; }

        public ApiException(ErrorCode errorCode)
        {
            // Obtener los detalles del error del atributo ErrorDetails
            var errorDetails = GetErrorDetails(errorCode);

            // Establecer las propiedades de la excepción
            Code = errorCode.ToString();
            Description = errorDetails.Message;
            StatusCode = errorDetails.StatusCode;
        }

        private ErrorDetailsAttribute GetErrorDetails(ErrorCode errorCode)
        {
            var fieldInfo = errorCode.GetType().GetField(errorCode.ToString());
            var attributes = (ErrorDetailsAttribute[])fieldInfo.GetCustomAttributes(typeof(ErrorDetailsAttribute), false);
            return attributes.Length > 0 ? attributes[0] : null;
        }
    }
}
