"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ui_jquery_1 = require("@easyquery/ui-jquery");
document.addEventListener('DOMContentLoaded', function () {
    var options = {
        syncReportOnChange: true,
        showChart: false,
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
                eqResultGrid: {
                    tableClass: "table table-sm"
                }
            }
        },
    };
    var reportView = new ui_jquery_1.ReportViewJQuery();
    reportView.init(options);
    document['ReportViewJQuery'] = reportView;
});
