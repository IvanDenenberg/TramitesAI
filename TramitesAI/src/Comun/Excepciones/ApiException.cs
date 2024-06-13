using System.Numerics;

namespace TramitesAI.src.Common.Exceptions
{
    public class ApiException : Exception
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int StatusCode { get; set; }

        public ApiException(ErrorCode errorCode)
        {
            // Obtener los detalles del error del atributo ErrorDetails
            var errorDetails = ObtenerDetallesError(errorCode);

            // Establecer las propiedades de la excepción
            Codigo = errorCode.ToString();
            Descripcion = errorDetails.Mensaje;
            StatusCode = errorDetails.CodigoHTTP;
        }

        private DetallesErrorAttribute ObtenerDetallesError(ErrorCode errorCode)
        {
            var fieldInfo = errorCode.GetType().GetField(errorCode.ToString());
            var attributes = (DetallesErrorAttribute[])fieldInfo.GetCustomAttributes(typeof(DetallesErrorAttribute), false);
            return attributes.Length > 0 ? attributes[0] : null;
        }
    }
}
