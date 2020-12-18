"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ui_1 = require("@easyquery/ui");
require("@easyquery/enterprise");
window.addEventListener('load', function () {
    //Options for ReportViewJQuery
    var viewOptions = {
        //Saves report on each change
        syncReportOnChange: true,
        handlers: {
            onError: function (error) {
                console;
            }
        },
        result: {
            //Show EasyChart
            showChart: true,
            chartWidgetResolver: function (slot) { return new ui_1.GoogleChartWidget(slot); },
            //Paging options
            paging: {
                pageSize: 50
            }
        },
        //Load model on start
        loadModelOnStart: true,
        //Default model's ID (we use it here just for a nice folder name in App_Data folder)
        defaultModelId: 'adhoc-reporting',
        //Middleware endpoint 
        endpoint: window["__appPathBase"] + '/api/adhoc-reporting',
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
    var reportView = new ui_1.ReportView();
    var context = reportView.getContext();
    context.setLicenseKeyEndpoint(window["__appPathBase"] + window['__eqLckEndpoint']);
    context.useEnterprise(function () {
        reportView.init(viewOptions);
    });
    document['ReportView'] = reportView;
});
