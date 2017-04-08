using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace workflowStarts.Controllers
{
    public class updateBatchController : ApiController
    {
        public class updateParameters
        {
            /// <summary>
            ///URL of the SharePoitn Site
            /// </summary>
            public string siteurl { get; set; }
            /// <summary>
            /// List Name
            /// </summary>
            public string listName { get; set; }

            /// <summary>
            /// Internal field name of the lookup field
            /// </summary>
            public string QuerylookUpField { get; set; }
            
            /// <summary>
            /// ID of the Item of the parent list
            /// </summary>
            public int QuerylookupValueID { get; set; }

            /// <summary>      
            /// Internal field name to update
            /// </summary>
            public string FieldToUpdate { get; set; }

            /// <summary>      
            /// Value to update
            /// </summary>
            public object FieldValue { get; set; }

        }


        public HttpResponseMessage Get([FromUri] updateParameters parameters)
        {
            var result = new HttpResponseMessage();
            
            var wf = new spUpdateBatch();

            int count = wf.Execute(parameters.siteurl, parameters.listName, parameters.QuerylookUpField, parameters.QuerylookupValueID, parameters.FieldToUpdate, parameters.FieldValue);

            if (count>-1)
            {
                result.StatusCode = HttpStatusCode.OK;
                result.Content = Request.CreateResponse(HttpStatusCode.OK, count.ToString()).Content;
            }
            else
            {
                result.Content = Request.CreateResponse(HttpStatusCode.InternalServerError, wf.responseMsg).Content;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }

            return result;
        }


    }
}
