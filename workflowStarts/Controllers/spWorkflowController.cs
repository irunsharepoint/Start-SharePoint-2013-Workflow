using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using workflowStart;

namespace workflowStarts.Controllers
{
    public class spWorkflowController : ApiController
    {
        public class workflow
        {
            /// <summary>
            ///URL of the SharePoitn Site
            /// </summary>
            public string siteurl { get; set; }
            /// <summary>
            /// Name of the SharePoint2013 List Workflow
            /// </summary>
            public string workflowname { get; set; }
            /// <summary>
            /// ID of the Item on which to start the workflow
            /// </summary>
            public int itemid { get; set; }
            /// <summary>
            /// Any custom parameters you want to send to the workflow
            /// </summary>
            public string initiationdata { get; set; }
        }


        public HttpResponseMessage Get([FromUri] workflow parameters)
        {
            var result = new HttpResponseMessage();

            workflowStart.Models.worflowParameters param = new workflowStart.Models.worflowParameters();

            param.itemID = parameters.itemid;
            param.siteUrl = parameters.siteurl;
            param.workflowName = parameters.workflowname;

            if (parameters.initiationdata != null)
            {
                param.initiationData = JsonConvert.DeserializeObject<Dictionary<string, object>>(parameters.initiationdata);

                Dictionary<string, object> normalizeParam = new Dictionary<string, object>();

                foreach (var k in param.initiationData)
                {
                    normalizeParam.Add(k.Key.Replace(" ", ""), k.Value);
                }

                param.initiationData = normalizeParam;
            }

            spWorkflow wf = new spWorkflow();

            if (wf.Execute(param))
            {
                result.StatusCode = HttpStatusCode.OK;
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
