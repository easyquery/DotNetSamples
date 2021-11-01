easyquery = easyquery || {};
easyquery.blazor = {

    initEasyQueryView: function (view, viewOptions) {
        var context = view.getContext();
        if (viewOptions.endpoint) {
            context.useEndpoint(viewOptions.endpoint);
        }
        context.useEnterprise(function () {
            view.init(viewOptions);
        });
    },

    startAdvancedSearch: function () {
        //Options for AdvancedSearchViewJQuery
        var viewOptions = {

            //Load model on start
            loadModelOnStart: true,

            defaultModelId: 'nwind',

            defaultQueryId: 'fee014cd-ad47-4a33-9c16-4cdf903bfbd2',

            //Load query on start
            loadQueryOnStart: true,

            enableExport: true,
            serverExporters: ['pdf', 'excel', 'csv'],

            //locale: 'en-US',
            localeSettings: {
                shortDateFormat: 'yyyy-MM-dd',
            },

            //Handlers
            handlers: {
                //Error handler
                onError: function (_, error) {
                    console.error(error.sourceError);
                }
            },
            result: {
                //Show EasyChart
                showChart: true,
                paging: {
                    pageSize: 30
                }
            }
        };

        var view = new easyquery.ui.AdvancedSearchView();
        this.initEasyQueryView(view, viewOptions);
        document.AdvancedSearchView = view;
    },

    stopAdvancedSearch: function () {
        if (document.AdvancedSearchView) {
            document.AdvancedSearchView.detach();
            delete document.AdvancedSearchView;
        }
    }
}