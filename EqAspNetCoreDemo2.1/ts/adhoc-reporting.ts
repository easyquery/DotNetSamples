import { ReportViewOptions, GoogleChartWidget, ReportView } from "@easyquery/ui";
import '@easyquery/enterprise'

window.addEventListener('load', () => {

    //Options for ReportViewJQuery
    let options: ReportViewOptions = {

        //Saves report on each change
        syncReportOnChange: true,

        result: {
            //Show EasyChart
            showChart: true,

            chartWidgetResolver: (slot) => { return new GoogleChartWidget(slot) },

            //Paging options
            paging: {
            }
        },

        //Load model on start
        loadModelOnStart: true,

        //Default model's ID (we use it here just for a nice folder name in App_Data folder)
        defaultModelId: 'adhoc-reporting',

        //Middleware endpoint 
        endpoint: window["_appPathBase"] + '/api/adhoc-reporting',

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
            },

            //ResultGrid options
            resultGrid: {
                tableClass: "table table-sm",
                formatGridCell: function (dataTable, rowIndex, colIndex, value) {
                    var props = dataTable.getColumnProperties(colIndex);
                    if (props.dataType == 'Decimal') {
                        return "$" + value;
                    }
                    else {
                        return value;
                    }
                }
            }
        }
    };
    
    const reportView = new ReportView();
    reportView.getContext().setLicenseKeyEndpoint(window["_appPathBase"] + window['__eqLckEndpoint']);
    reportView.getContext().useEnterprise(() => {
        reportView.init(options);
    });
   
    document['ReportView'] = reportView;
});
