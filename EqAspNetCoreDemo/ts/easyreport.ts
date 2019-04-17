import { ReportViewJQuery } from "@easyquery/ui-jquery"
import { ReportViewOptions } from "@easyquery/ui";


document.addEventListener('DOMContentLoaded', () => {

    //Options for ReportViewJQuery
    let options: ReportViewOptions = {

        //Saves report on each change
        syncReportOnChange: true,

        //Show EasyChart
        showChart: true,

        //Paging options
        paging: {
            //Use bootstrap v4 styles
            useBootstrap: true,

            //max count of displayed buttons
            maxButtonCount: 10,

            //paging css class
            cssClass: 'pagination-sm'
        },

        //Context options
        context: {

            //Load model on start
            loadModelOnStart: true,

            //Broker options
            broker: {
                //Middleware endpoint 
                serviceUrl: "/api-easyreport"
            },

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
                    alwaysShowButtonsInPredicates: false,
                    adjustEntitiesMenuHeight: false,
                    menuOptions: {
                        showSearchBoxAfter: 20,
                        activateOnMouseOver: true
                    }
                },

                //EqResultGrid options
                eqResultGrid: {
                    tableClass: "table table-sm"
                }
            }
        },
    };
    
    let reportView = new ReportViewJQuery();
    reportView.init(options);
    document['ReportViewJQuery'] = reportView;
});
