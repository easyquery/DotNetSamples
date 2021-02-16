import { ReportViewOptions, ReportView } from "@easyquery/ui";
import '@easyquery/enterprise';


window.addEventListener('load', () => {

    //Options for ReportViewJQuery
    const viewOptions: ReportViewOptions = {

        calcTotals: true,

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
        defaultModelId: 'adhoc-reporting',

        enableExport: true,

        //Different widgets options
        widgets: {
            //ColumnBar options
            columnsBar: {
                accentActiveColumn: false,
                allowAggrColumns: true,
                attrElementFormat: "{attr}",
                showColumnCaptions: true,
                adjustEntitiesMenuHeight: false,
                menuOptions: {
                    showSearchBoxAfter: 30,
                    activateOnMouseOver: true
                }
            
            },

            //QueryPanel options
            queryPanel: {
                alwaysShowButtonsInGroups: false,
                adjustEntitiesMenuHeight: false,
                menuOptions: {
                    showSearchBoxAfter: 20,
                    activateOnMouseOver: true
                }
            }
        }
    };

    const reportView = new ReportView();
    const context = reportView.getContext();

    context
        .useEndpoint('/api/adhoc-reporting')
        .useEnterprise(() => {
            reportView.init(viewOptions);
        });
   
    document['ReportView'] = reportView;
});
