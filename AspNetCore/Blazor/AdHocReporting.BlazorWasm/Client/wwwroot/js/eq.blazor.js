﻿easyquery = easyquery || {};

easyquery.blazor = {
    initEasyQueryView: function (view, viewOptions) {
        var context = view.getContext();
        if (viewOptions.token) {
            var http = context.getServices().getHttpClient();
            http.defaultHeaders['Authorization'] = 'Bearer ' + viewOptions.token;
        }
        context.useEndpoint('/api/adhoc-reporting');
        context.useEnterprise(function () {
            view.init(viewOptions);
        });
    },

    startAdhocReporting: function (token) {
        //Options for ReportView
        var viewOptions = {
            //Saves report on each change
            syncReportOnChange: true,

            enableExport: true,
            serverExporters: ['pdf', 'excel', 'excel-html', 'csv'],

            handlers: {
                onError: function (_, error) {
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
            defaultModelId: 'adhoc-reporting',

            token: token

        }

        var view = new easyquery.ui.ReportView();
        this.initEasyQueryView(view, viewOptions);
        document.AdhocReportingView = view;
    },

    stopAdhocReporting: function () {
        if (document && document.AdhocReportingView) {
            document.AdhocReportingView.detach();
            delete document.AdhocReportingView;
        }
    }
}