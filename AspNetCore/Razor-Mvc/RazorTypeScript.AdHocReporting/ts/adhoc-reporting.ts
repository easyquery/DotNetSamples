import { ReportViewOptions, ReportView } from "@easyquery/ui";
import '@easyquery/enterprise';


window.addEventListener('load', () => {

    //Options for ReportViewJQuery
    const viewOptions: ReportViewOptions = {
        calcTotals: true,
        enableExport: true,
     
        serverExporters: ['pdf', 'excel', 'excel-html', 'csv'],

        //Saves report on each change
        syncReportOnChange: true,

        handlers: {
            onError: function (context, error) {
                console.error(error.sourceError);
            }
        },

        result: {
            //Show EasyChart
            showChart: true,

            //Paging options
            paging: {
                pageSize: 30				
            }
        },

        //Load model on start
        loadModelOnStart: true,

        //Default model's ID (we use it here just for a nice folder name in App_Data folder)
        defaultModelId: 'adhoc-reporting'
    };

    const reportView = new ReportView();
    reportView.getContext()
        .useEndpoint('/api/adhoc-reporting')
        .useEnterprise(() => {
            reportView.init(viewOptions);
        });
   
    document['ReportView'] = reportView;
});
