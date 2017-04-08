using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace workflowStarts
{
    public class spUpdateBatch
    {

        public string responseMsg;

        /// <summary>
        /// Query items from a related list by a lookup field and update each item by a field and value
        /// </summary>
        /// <param name="siteUrl">Sharepoint web url</param>
        /// <param name="listName">Name of the list to update</param>
        /// <param name="QuerylookUpField">Internal lookup field name of the relation</param>
        /// <param name="QuerylookupValueID">ID of the parent lookup list</param>
        /// <param name="FieldToUpdate">Field internal name to update</param>
        /// <param name="FieldValue">Value to update</param>
        /// <returns></returns>
        public int Execute(string siteUrl, string listName, string QuerylookUpField, int QuerylookupValueID, string FieldToUpdate, object FieldValue)
        {
            try
            {

                int results = 0;

                using (ClientContext clientContext = new ClientContext(siteUrl))
                {
                    string user = ConfigurationManager.AppSettings["USER"];

                    if (!string.IsNullOrEmpty(user))
                    {
                        string domain = ConfigurationManager.AppSettings["DOMAIN"];
                        string password = ConfigurationManager.AppSettings["PASSWORD"];

                        System.Net.NetworkCredential cred = new System.Net.NetworkCredential(user, password, domain);
                        clientContext.Credentials = cred;

                        Web web = clientContext.Web;
                        ListCollection lists = web.Lists;

                        List selectedList = lists.GetByTitle(listName);

                        clientContext.Load(lists); // this lists object is loaded successfully
                        clientContext.Load(selectedList);  // this list object is loaded successfully

                        clientContext.ExecuteQuery();

                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = @"<View><Query><Where><Eq><FieldRef Name='" + QuerylookUpField + "\' LookupId='TRUE' /><Value Type=\'Lookup\'>" + QuerylookupValueID + "</Value></</Eq></Where></Query><ViewFields><FieldRef Name='ID' /></ViewFields></View>";

                        ListItemCollection listItems = selectedList.GetItems(camlQuery);

                        clientContext.Load(listItems);
                        clientContext.ExecuteQuery();

                        foreach (ListItem item in listItems)
                        {
                            ListItem oListItem = selectedList.GetItemById(item.Id);
                            oListItem[FieldToUpdate] = FieldValue;
                            oListItem.Update();
                            clientContext.ExecuteQuery();
                            results++;
                        }

                    }

                    return results;
                }
            }

            catch (Exception err)
            {
                responseMsg = err.Message;
                return -1;
            }
        }


    }
}