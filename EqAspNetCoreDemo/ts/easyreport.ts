import { ReportViewJQuery } from "@easyquery/ui-jquery"
import { EqReportViewOptions } from "@easyquery/ui";


document.addEventListener('DOMContentLoaded', () => {
    let options: EqReportViewOptions = {
        syncReportOnChange: true,
        showChart: true,
        paging: {
            useBootstrap: true,
            maxButtonCount: 10,
            cssClass: 'pagination-sm'
        },
        context: {
            loadModelOnStart: true,
            broker: {
                serviceUrl: "/api-easyreport"
            },
            widgets: {
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
                queryPanel: {
                    alwaysShowButtonsInPredicates: false,
                    adjustEntitiesMenuHeight: false,
                    menuOptions: {
                        showSearchBoxAfter: 20,
                        activateOnMouseOver: true
                    }
                },
            }
        },
    };
    
    let reportView = new ReportViewJQuery();
    reportView.init(options);
    document['ReportViewJQuery'] = reportView;
});
