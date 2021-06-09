using System;

namespace PR.Entity
{
    public class GenericResult<T> where T : class
    {
        public T GenericObject { get; set; }
        public bool IsValid { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
