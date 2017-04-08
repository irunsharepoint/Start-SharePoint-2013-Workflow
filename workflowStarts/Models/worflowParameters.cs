using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workflowStart.Models
{
    /// <summary>
    /// Workflow call parameters
    /// </summary>
    public class worflowParameters
    {
        /// <summary>
        ///URL of the SharePoitn Site
        /// </summary>
        public string siteUrl;
        /// <summary>
        /// Name of the SharePoint2013 List Workflow
        /// </summary>
        public string workflowName;
        /// <summary>
        /// ID of the Item on which to start the workflow
        /// </summary>
        public int itemID;
        /// <summary>
        /// Any custom parameters you want to send to the workflow
        /// </summary>
        public Dictionary<string, object> initiationData;
    }
}