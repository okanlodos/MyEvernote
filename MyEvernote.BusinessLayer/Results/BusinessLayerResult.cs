using System.Collections.Generic;

namespace MyEvernote.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T : class
    {
        public List<string> Errors { get; set; }
        public T Result { get; set; }
        public BusinessLayerResult()
        {
            Errors = new List<string>();
        }
    }
}