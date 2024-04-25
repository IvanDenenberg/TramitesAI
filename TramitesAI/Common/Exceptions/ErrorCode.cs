namespace TramitesAI.Common.Exceptions
{
    using System;

    // Defining attributes
    public class ErrorDetailsAttribute : Attribute
    {
        public string Message { get; }
        public int StatusCode { get; }

        public ErrorDetailsAttribute(string message, int statusCode = 500)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }

    // Defining common errors
    public enum ErrorCode
    {
        [ErrorDetails("unknown Error")]
        UNKNOWN_ERROR = 1,

        [ErrorDetails("internal Server Error")]
        INTERNAL_SERVER_ERROR = 2,

        [ErrorDetails("invalid json", 400)]
        INVALID_JSON = 3,

        [ErrorDetails("invalid params", 400)]
        INVALID_PARAMS = 4,

        [ErrorDetails("fail generating ID")]
        FAIL_GENERATING_ID = 5,

        [ErrorDetails("fail parsing transfer")]
        FAIL_PARSING_TRANSFER = 6,

        [ErrorDetails("config not found", 404)]
        CONFIG_NOT_FOUND = 7,

        [ErrorDetails("error post")]
        ERROR_POST_REFUND = 8,

        [ErrorDetails("fail executing query")]
        FAIL_EXECUTING_QUERY = 9,

        [ErrorDetails("invalid query", 400)]
        INVALID_QUERY = 10,

        [ErrorDetails("invalid range", 400)]
        INVALID_RANGE = 11,

        [ErrorDetails("Authorization Required", 401)]
        UNAUTHORIZED = 12,

        [ErrorDetails("Method not implemented", 400)]
        NOT_IMPLEMENTED = 13,

        [ErrorDetails("invalid institution", 400)]
        INVALID_INSTITUTION = 14,

        [ErrorDetails("fail inserting in DB", 500)]
        FAIL_INSERT_DB = 15,

        [ErrorDetails("fail deleting in DB", 500)]
        FAIL_DELETE_DB = 16,

        [ErrorDetails("delete key not found", 400)]
        DELETE_KEY_NOT_FOUND = 17,

        [ErrorDetails("missing fields in body", 400)]
        MISSING_FIELDS = 18,

        [ErrorDetails("fail authenticate", 500)]
        FAIL_AUTHENTICATE = 19,

        [ErrorDetails("invalid type", 400)]
        INVALID_TYPE = 20,

        [ErrorDetails("not found", 404)]
        NOT_FOUND = 21,

        [ErrorDetails("file not found", 404)]
        FILE_NOT_FOUND = 22,

        [ErrorDetails("user not allowed", 403)]
        FORBIDDEN = 23,

        [ErrorDetails("error in response", 400)]
        ERROR_IN_RESPONSE = 24,

        [ErrorDetails("fail parsing map to json", 400)]
        FAIL_PARSING_MAP_TO_JSON = 25,

        [ErrorDetails("conflict", 409)]
        CONFLICT = 26,

        [ErrorDetails("bad request", 400)]
        BAD_REQUEST = 27,

        [ErrorDetails("fail parse to json", 400)]
        FAIL_PARSE_TO_JSON = 28,

        [ErrorDetails("service unavailable", 503)]
        SERVICE_UNAVAILABLE = 29,

        [ErrorDetails("Error download zip file from Url", 500)]
        ERROR_DOWNLOAD_FILE = 30,

        [ErrorDetails("Invalid file extension", 500)]
        INVALID_FILE_EXTENSION = 31,

        [ErrorDetails("Error waiting async process", 500)]
        ERROR_WAITING_ASYNC_PROCESS = 32,

        [ErrorDetails("No such property found on config service", 500)]
        MISSING_CONFIG_PROPERTY = 33
    }
}

