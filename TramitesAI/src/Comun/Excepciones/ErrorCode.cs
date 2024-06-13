namespace TramitesAI.src.Common.Exceptions
{
    using System;

    public class DetallesErrorAttribute : Attribute
    {
        public string Mensaje { get; }
        public int CodigoHTTP { get; }

        public DetallesErrorAttribute(string message, int statusCode = 500)
        {
            Mensaje = message;
            CodigoHTTP = statusCode;
        }
    }

    // Definiendo errores
    public enum ErrorCode
    {
        [DetallesError("Error desconocido")]
        ERROR_DESCONOCIDO = 1,

        [DetallesError("Error interno del servidor")]
        ERROR_INTERNO_SERVIDOR = 2,

        [DetallesError("JSON Invalido", 400)]
        JSON_INVALIDO = 3,

        [DetallesError("Parametros invalidos", 400)]
        PARAMETROS_INVALIDOS = 4,

        [DetallesError("Error extrayendo los textos de los archivos", 500)]
        ERROR_EXTRAYENDO_TEXTOS = 5,

        [DetallesError("Asunto de mail invalido", 400)]
        ASUNTO_INVALIDO = 6,

        [DetallesError("Non encontrado", 404)]
        NO_ENCONTRADO = 7,

        [DetallesError("Archivo no encontrado", 404)]
        ARCHIVO_NO_ENCONTRADO = 8,

        [DetallesError("Modelo no implementado", 501)]
        MODELO_NO_IMPLEMENTADO = 9,

        [DetallesError("Error al descargar el archivo", 500)]
        ERROR_DESCARGANDO_ARCHIVO = 10,

        [DetallesError("Propiedad de condifuracion no encontrada", 500)]
        PROPIEDAD_DE_CONFIGURACION_FALTANTE = 11
    }
}

