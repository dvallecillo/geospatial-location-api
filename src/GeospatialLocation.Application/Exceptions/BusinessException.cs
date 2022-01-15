using System;
using System.Collections.Generic;
using GeospatialLocation.Application.Exceptions.Errors;

namespace GeospatialLocation.Application.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(
            string code, string messageTemplate, params object[] propertyValues)
            : base(messageTemplate)
        {
            Code = code;
            MessageTemplate = messageTemplate;
            PropertyValues = propertyValues;
        }

        public BusinessException(ErrorCode error) : this(error.Code, error.Message)
        {
        }

        public string MessageTemplate { get; }

        public IReadOnlyCollection<object> PropertyValues { get; }

        public string Code { get; }
    }
}