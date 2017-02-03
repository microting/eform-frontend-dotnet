using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eFormFrontendDotNet.Models
{
    public class DataResponse
    {
        public string model_id;
        public Data data;
        public class Data
        {
            public string message;
            public string status;

            public Data(string message, string status)
            {
                this.message = message;
                this.status = status;
            }
        }
    }
}