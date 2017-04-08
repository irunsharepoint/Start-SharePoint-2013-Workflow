# Start-SharePoint-2013-Workflow
A SharePoint 2013/2016 workflow application that allow start a SharePoint 2013 workflows from another SharePoint 2013 Workflow.

The application have a two components:
1) A SharePoint Designer 2013 Activity to call the another workflow (SharePoint Sandbox Feature)
2) A REST Service to start the instance remotely from CSOM API (IIS Web Server)

For control purposes I added another SharePoint Designer Activity named updateByLookup to handle hierarchy changes to multiple items associated with a parent - child list (primary/secondary workflows)
 * you can start site workflows or list/library workflow !
 
For detailed implementation of functionality you can visit my blog iRunSharePoint or view the MS approach in the Microsoft related page
 
Published by: Ernesto Garcia
