using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPI.Controllers.Helper
{
    //[Serializable]    // 1. Möglichkeit    
    //public class RepoException : Exception
    //{
    //    public UpdateResultType Type { get; set; }
    //    public RepoException(UpdateResultType type) { Type = type; }
    //    public RepoException(string message, UpdateResultType type) : base(message) { Type = type; }
    //    public RepoException(string message, Exception inner) : base(message, inner) { } protected RepoException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    //}    // 2. Möglichkeit    
    public class RepoException : Exception
    {
        public ExceptionType RepoExceptionType { get; set; }

        public RepoException(ExceptionType type){
            RepoExceptionType = type;
        }
        public RepoException(string message, ExceptionType type) : base(message){
            RepoExceptionType = type;
        }
        public RepoException(string message, Exception inner) : base(message, inner) { }
        protected RepoException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public enum ExceptionType
        {
            OK,
            SQLERROR,
            NOTFOUND,
            INVALIDARGUMENT,
            ERROR,
            NOCONTENT
        }
    }    
}

