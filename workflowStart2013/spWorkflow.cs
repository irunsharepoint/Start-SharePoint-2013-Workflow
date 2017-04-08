using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using workflowStart.Models;

namespace workflowStart
{
    /// <summary>
    /// SharePoint Workflow Wrapper
    /// </summary>
    public class spWorkflow
    {

        public string responseMsg;


        /// <summary>
        /// Start a SharePoint List Workflow
        /// </summary>
        /// <param name="siteUrl">URL of the SharePoitn Site</param>
        /// <param name="workflowName">Name of the SharePoint2013 Workflow</param>
        /// <param name="itemID">ID of the Item on which to start the workflow List, if item ID is equal to 0 the worlflow to start is considered as site workflow</param>
        /// <param name="initiationData">Any custom parameters you want to send to the workflow</param>
        public bool Execute(worflowParameters param)
        {
            try
            {
                using (ClientContext clientContext = new ClientContext(param.siteUrl))
                {
                    string user = ConfigurationManager.AppSettings["USER"];
                    string domain = ConfigurationManager.AppSettings["DOMAIN"];
                    string password = ConfigurationManager.AppSettings["PASSWORD"];

                    System.Net.NetworkCredential cred = new System.Net.NetworkCredential(user, password, domain);
                    clientContext.Credentials = cred;

                    Web web = clientContext.Web;

                    //Workflow Services Manager which will handle all the workflow interaction.
                    WorkflowServicesManager wfServicesManager = new WorkflowServicesManager(clientContext, web);

                    //The Subscription service is used to get all the Associations currently on the SPSite
                    WorkflowSubscriptionService wfSubscriptionService = wfServicesManager.GetWorkflowSubscriptionService();

                    //All the subscriptions (associations)
                    WorkflowSubscriptionCollection wfSubscriptions = wfSubscriptionService.EnumerateSubscriptions();

                    //Load only the subscription (association) which we want. You can also get a subscription by definition id.
                    clientContext.Load(wfSubscriptions, wfSubs => wfSubs.Where(wfSub => wfSub.Name == param.workflowName));

                    clientContext.ExecuteQuery();

                    //Get the subscription.
                    WorkflowSubscription wfSubscription = wfSubscriptions.First();

                    //The Instance Service is used to start workflows and create instances.
                    WorkflowInstanceService wfInstanceService = wfServicesManager.GetWorkflowInstanceService();

                    if (param.initiationData == null)
                        param.initiationData = new Dictionary<string, object>();

                    //validate item id
                    if (param.itemID > 0)
                    {
                        wfInstanceService.StartWorkflowOnListItem(wfSubscription, param.itemID, param.initiationData);
                    }
                    else
                    {
                        wfInstanceService.StartWorkflow(wfSubscription, param.initiationData);
                    }

                    clientContext.ExecuteQuery();

                    return true;
                }
            }



            catch(Exception err)
            {
                responseMsg = err.Message;
                return false;
            }

        }



    }
}