import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import {ReportView, ReportViewOptions} from "@easyquery/ui";
import '@easyquery/enterprise';
import '@easyquery/ui/dist/assets/css/easyquery.css'

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));

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
        .useEndpoint('http://localhost:5285/api/adhoc-reporting')
        .useEnterprise(() => {
            reportView.init(viewOptions);
        });

    // @ts-ignore
    document['ReportView'] = reportView;
});
