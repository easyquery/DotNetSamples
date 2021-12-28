"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ui_1 = require("@easyquery/ui");
require("@easyquery/enterprise");
window.addEventListener('load', function () {
    //Options for ReportViewJQuery
    var viewOptions = {
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
    var reportView = new ui_1.ReportView();
    reportView.getContext()
        .useEndpoint('/api/adhoc-reporting')
        .useEnterprise(function () {
        reportView.init(viewOptions);
    });
    document['ReportView'] = reportView;
});
