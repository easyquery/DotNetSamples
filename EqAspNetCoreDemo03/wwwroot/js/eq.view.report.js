; (function ($, window) {

    //Ensure that global variables exist
    var EQ = window.EQ = window.EQ || {};


    var isGoogleVisualizationDefined = function() {
        return (typeof google != 'undefined') && (typeof google.visualization != 'undefined');
    };

    var isGoogleChartDefined = function() {
        return isGoogleVisualizationDefined() && (typeof google.visualization.PieChart != 'undefined');
    };


    /// <namespace name="EQ.view.report" version="1.0.0">
    /// <summary>
    /// Contains different functions for managing core EasyQuery pages (views): process user input, render result set, etc.
    /// </summary>
    /// </namespace>
    EQ.view.report = {

        _resultPanel : null,

        _sqlPanel : null,

        _clearQueryButton : null,

        _loadQueryButton : null,

        _saveQueryButton : null,

        _executeQueryButton : null,  

        _exportButtons: null,

        _gridClass: "",

        _funcs: {},

        _findControlById: function (controlId) {
            var result = $("#" + controlId);
            if (result.length == 0)
                result = null;

            return result;
        }, 

        init: function (options) {
            options = options || window.easyQueryViewSettings || {};

            EQ.view.applyCommonOptions(options);

            var resultPanelId = options.resultPanelId || "ResultPanel";
            this._resultPanel = this._findControlById(resultPanelId);

            var sqlPanelId = options.sqlPanelId || "SqlPanel";
            this._sqlPanel = this._findControlById(sqlPanelId);

            var clearReportButtonId = options.clearReportButtonId || "ClearReportButton";
            this._clearReportButton = this._findControlById(clearReportButtonId);

            var newReportButtonId = options.newReportButtonId || "NewReportButton";
            this._newReportButton = this._findControlById(newReportButtonId);

            var loadReportButtonId = options.loadReportButtonId || "LoadReportButton";
            this._loadReportButton = this._findControlById(loadReportButtonId);

            var saveReportButtonId = options.saveReportButtonId || "SaveReportButton";
            this._saveReportButton = this._findControlById(saveReportButtonId);

            var removeReportButtonId = options.removeReportButtonId || "RemoveReportButton";
            this._removeReportButton = this._findControlById(removeReportButtonId);
            

            var updateReportButtonId = options.updateReportButtonId || "UpdateReportButton";
            this._updateReportButton = this._findControlById(updateReportButtonId);

            var resultCountSpanId = options.resultCountSpanId || "ResultCount";
            this._resultCountSpan = this._findControlById(resultCountSpanId);

            var exportButtonsId = options.exportButtonsId || "ResultExportButtons";
            this._exportButtons = this._findControlById(exportButtonsId );
            
            this._gridClass = options.gridClass || "table table-striped table-condensed";

            this._reportListPlaceholder = $("#ReportList");


            if (typeof(options.rebuildOnReportChange) === "undefined")    
                options.rebuildOnReportChange =  true;

            if (typeof (options.syncReportOnChange) === "undefined")
                options.syncReportOnChange = true;

            this.showChart = typeof(options.showChart) !== "undefined" ? options.showChart : true;
            
            this.pagingOptions = options.paging;

            this.reports = [];

            var self = this;

            EQ.client.init();


            //Clear Report button
            if (!this._funcs.clearReportButtonClick) {
                this._funcs.clearReportButtonClick = function () {
                    self._clearErrors();
                    if (self._exportButtons) {
                        self._exportButtons.hide();
                    }
                    if (self._resultCountSpan) {
                        self._resultCountSpan.hide();
                    }
                    self._clearSqlPanel();
                    EQ.client.clearQuery();
                };
            }


            if (this._clearReportButton) {
                this._clearReportButton.off("click", this._funcs.clearReportButtonClick);
                this._clearReportButton.on("click", this._funcs.clearReportButtonClick);
            }


            //New Report button
            if (!this._funcs.newReportButtonClick) {
                this._funcs.newReportButtonClick = function () {
                    self.newReport();
                };
            }

            if (this._newReportButton) {
                this._newReportButton.off("click", this._funcs.newReportButtonClick);
                this._newReportButton.on("click", this._funcs.newReportButtonClick);
            }


            //Load Report button
            if (!this._funcs.loadReportButtonClick) {
                this._funcs.loadReportButtonClick = function () {
                    EQ.client.loadQuery({
                        id: "LastQuery",
                        sucess: function (data) {

                            self._clearErrors();
                            self._clearResultPanel();
                            //buildReport();
                        },
                        error: self._errorHandler
                    });
                };
            }

            if (this._loadReportButton) {
                this._loadReportButton.off("click", this._funcs.loadReportButtonClick);
                this._loadReportButton.on("click", this._funcs.loadReportButtonClick);
            }


            //Save Report button
            if (!this._funcs.saveReportButtonClick) {
                this._funcs.saveReportButtonClick = function () {
                    self.saveCurrentReportAs();
                };
            }

            if (this._saveReportButton) {
                this._saveReportButton.off("click", this._funcs.saveReportButtonClick);
                this._saveReportButton.on("click", this._funcs.saveReportButtonClick);
            }

            //Remove Report button
            if (!this._funcs.removeReportButtonClick) {
                this._funcs.removeReportButtonClick = function () {
                    self.removeCurrentReport();
                };
            }

            if (this._removeReportButton) {
                this._removeReportButton.off("click", this._funcs.removeReportButtonClick);
                this._removeReportButton.on("click", this._funcs.removeReportButtonClick);
            }


            //Update Report button             
            if (!this._funcs.updateReportButtonClick) {
                this._funcs.updateReportButtonClick = function () {
                    self.buildAndExecute();
                };
            }

            if (this._updateReportButton) {
                this._updateReportButton.off("click", this._funcs.updateReportButtonClick);
                this._updateReportButton.on("click", this._funcs.updateReportButtonClick);
            }



            //Query changed handler
            if (!this._funcs.queryChangedHandler) {
                this._funcs.queryChangedHandler = function (params) {
                    self._clearSqlPanel();
                    self._clearResultPanel();
                    if (options.syncReportOnChange) {
                        self.syncReport();
                    }
                };
            }
            
            var query = EQ.client.getQuery();
            query.addChangedCallback(this._funcs.queryChangedHandler);

            window.setTimeout(function() {
                EQ.client.loadQueryList({
                    success: function (data) {
                        if ($.isArray(data)) {
                            self.reports = data
                            self.renderReportList();
                        }

                    }
                })
            }, 500);
        },

        _insertIntoReportList: function(report) {
            this.reports.push(report);
        },

        _removeFromReportList: function(reportId) {
            var index = this._indexOfReportById(reportId);
            if (index >= 0)
                this.reports.splice(index, 1);
            return index;
        },

        _indexOfReportById: function(reportId) {
            for (var i = 0; i < this.reports.length; i++) {
                if (this.reports[i].id == reportId)
                    return i;
            }
            return -1;
        },

        renderReportList: function (options) {
            this._renderReportPanels();

            if (this._reportListPlaceholder.length > 0) {
                this._reportListPlaceholder.empty();
                options = options || { reportId: (this.reports.length > 0 ? this.reports[0].id : null) };
                var ul = $("<ul class='nav nav-pills nav-stacked'></ul>").appendTo(this._reportListPlaceholder);
                for (var i = 0; i < this.reports.length; i++) {
                    var report = this.reports[i];
                    this._renderReportItemInList(report, ul);
                }

                if (options.reportId) {
                    this.loadReport(options.reportId);
                }
                if (typeof (options.reportIndex) !== "undefined") {
                    var idx = options.reportIndex;
                    if (idx >= this.reports.length)
                        idx = this.reports.length - 1;
                    if (idx < 0 && this.reports.length > 0)
                        idx = 0;
                    if (idx >= 0)
                        this.loadReport(this.reports[idx].id);
                }
            }
        },

        _renderReportPanels: function () {
            var reportPanel = $("#ReportPanel");
            var noReportPanel = $("#NoReportPanel");
            if (this.reports.length > 0) {
                noReportPanel.hide();
                reportPanel.show();
            } 
            else {
                reportPanel.hide();
                noReportPanel.show();
            }
        },

        _renderReportItemInList: function (report, ul) {
            if (!ul) ul = this._reportListPlaceholder.find("ul");
            if (ul.length == 0) return;
            var reportName = report.name || report.text;
            var li = $("<li data-rid='" + report.id + "'><a href='javascript:void(0)'>" + reportName + "</a></li>");
            var self = this;
            li.appendTo(ul).click(function () {
                var rid = $(this).data("rid");
                self.loadReport(rid);
            });
        },

        renderCurrentReport: function() {
            var query = EQ.client.getQuery();
            $("#ReportTitle").text(query.getName());

            var columnsPanel = $(".columns-panel");
            columnsPanel.empty();
            columnsPanel.append("<strong>Columns:</strong> ");
            var columns = query.getColumns();
            for (var i = 0; i < columns.length; i++) {
                columnsPanel.append('<span class="label label-default">' + columns[i].caption + '</span> ');
            }
        },

        setActiveReport: function (reportId) {
            var rlItems = this._reportListPlaceholder.find("li");
            rlItems.removeClass("active");
            rlItems.filter("[data-rid='" + reportId + "']").addClass("active");
        },

        newReport: function () {
            var self = this;
            var reportName = prompt("Enter report name", "New report");

            if (reportName) {
                EQ.client.newQuery({
                    queryName: reportName,
                    success: function (query) {
                        var reportId = query.getId();
                        self._insertIntoReportList({ id: reportId, name: query.getName() });
                        self.renderReportList({ reportId: reportId });
                    }
                });
            }
        },

        loadReport: function (reportId) {
            if (!reportId) return;
            var self = this;
            EQ.client.loadQuery({
                id: reportId,
                silent: false,
                success: function() {
                    self.buildAndExecute();
                    self.renderCurrentReport();
                }
            });
            this.setActiveReport(reportId);
        },

        saveCurrentReportAs: function () {
            var self = this;
            var query = EQ.client.getQuery();
            var newReportName = prompt("Enter new report name", query.getName());

            if (newReportName) {
                EQ.client.saveQuery({
                    "query": query,
                    "queryName": newReportName,
                    "success": function (data) {
                        var savedQuery = EQ.client.getQuery();
                        var reportId = savedQuery.getId();
                        self.buildAndExecute();
                        self._insertIntoReportList({ id: reportId, name: query.getName() });
                        self.renderReportList({ reportId: reportId });
                        self.setActiveReport(reportId);
                        self.renderCurrentReport();
                    },
                    error: self._errorHandler
                });
            }
        },

        removeCurrentReport: function () {
            var report = EQ.client.getQuery();
            if (confirm("Remove report '" + report.getName() + "'?")) {
                var self = this;
                var query = EQ.client.getQuery();
                var reportId = query.getId();
                EQ.client.removeQuery({
                    queryId : reportId,
                    success: function () {
                        var index = self._removeFromReportList(reportId);
                        self.renderReportList({ reportIndex: index });
                        //alert("Report removed!");
                    }
                });
            }            
        },

        _clearErrorsInPanel : function(panel) {
            if (panel) {
                if (panel.hasClass('error')) {
                    panel.removeClass('error');
                }
                panel.empty();
            }        
        },

        _clearErrors: function () {
            this._clearErrorsInPanel(this._resultPanel);
            this._clearErrorsInPanel(this._sqlPanel);
        },

        _clearSqlPanel: function () {
            if (this._sqlPanel) {
                this._sqlPanel.empty();
            }

        },

        _clearResultPanel: function () {
            if (this._resultPanel) {
                this._resultPanel.empty();
            }

            if (this._exportButtons) {
                this._exportButtons.hide();
            }

        },

        syncReport: function () {
            var self = this;

            var sqlProgressIndicator = $('<div></div>', { 'class': 'result-panel loader' });
            var sqlPanel = self._sqlPanel;

            EQ.client.syncQuery({
                beforeSend: function () {
                    if (sqlPanel) {
                        sqlPanel.animate({ opacity: '0.5' }, 200);
                        sqlPanel.append(sqlProgressIndicator);
                    }
                },
                success: function (result) {
                    var sqlText = result.statement || "";
                    self.renderSqlStatement(sqlText);
                    sqlProgressIndicator.remove();
                },
                error: function (statusCode, errorMessage, operation) {
                    sqlProgressIndicator.remove();
                    alert("Error: " + statusCode + "\n" + errorMessage);
                }
            });
        },


        renderSqlStatement: function(sql) {
            var sqlPanel = this._sqlPanel;

            if (sqlPanel) {
                this._clearErrorsInPanel(sqlPanel);
                sqlPanel.animate({ 'opacity': 1 }, 200);
                if (sqlPanel.prop("tagName") !== "TEXTAREA") {
                    sqlPanel.html('<div class="sql-panel-result"></div>');
                    sqlPanel = sqlPanel.find('div');
                }

                sqlPanel.text(sql ? sql : "");
                var sqlText = sqlPanel.html().replace(/\r\n/g, "<br />").replace(/  /g, "&nbsp;&nbsp;");
                sqlPanel.html(sqlText);
            }

        },

        buildAndExecute:function (options) {
            var self = this;
			options = options || { page: 1 };
            var query = EQ.client.getQuery();

            var resultProgressIndicator = $('<div></div>', { 'class': 'result-panel loader' });
            var resultPanel = self._resultPanel;
            EQ.client.buildAndExecute({
                "query": query,
                "options": {
                    "page": options.page,                	
                    "resultFormat": 1
                },
                beforeSend: function () {
                    if (resultPanel) {
                        resultPanel.animate({ opacity: '0.5' }, 200);
                        resultPanel.html(resultProgressIndicator);
                    }
                },
                success: function (result) {
                    try {
                        if (result.statement) {
                            self.renderSqlStatement(result.statement);    
                        }
                                
                        if (resultPanel) {
                            resultPanel.empty();
                            if (result.resultSet) {
                                
                                var grid;
                                if (result.resultSet.table) {
                                    //old format
                                    grid = EQ.view.renderGridOldStyle(result.resultSet.table);                                
                                }
                                else {
                                    var DataTable = EQ.view.getDataTableClass();

                                    // Create the data table.
                                    var dataTable = new DataTable(result.resultSet);
                                    //var myDataTable = new EqDataTable(result.resultSet);
                                    if (self.showChart) {
                                        var chartPanel = $("<div />").addClass("chart-panel").css({ "float": "right" }).appendTo(resultPanel);
                                        EQ.view.drawChart(dataTable, chartPanel.get(0));
                                    }

                                    grid = EQ.view.renderGridByDataTable(dataTable);
                                    grid.addClass("table table-striped table-condensed");
                                }

                                var gridPanel = $("<div />").addClass("grid-panel").appendTo(resultPanel);
                                gridPanel.append(grid);

                                if (self._exportButtons) {
                                    self._exportButtons.show();
                                }

                                //paging
                                var paging = result.paging;
                                if (paging && paging.enabled && paging.pageCount > 1) {
                                    var pageNavigator = EQ.view.renderPageNavigator({
                                        pageIndex: paging.pageIndex,
                                        pageCount: paging.pageCount,
                                        pageSelectedCallback: function (pageNum) {
                                            self.buildAndExecute({ page: pageNum });
                                        },
                                        maxButtonCount: self.pagingOptions ? self.pagingOptions.maxButtonCount : null,
                                        cssClass: self.pagingOptions ? self.pagingOptions.cssClass : null
                                    });

                                    gridPanel.css("height", resultPanel.innerHeight()-30);
                                    pageNavigator.appendTo(resultPanel);
                                }

                                resultPanel.animate({ 'opacity': 1 }, 200);
                            }
                        }

                        //result count
                        if (result.resultCount) {
                            self._resultCountSpan.text(result.resultCount);
                            self._resultCountSpan.show();
                        }
                    }
                    finally {
                        resultProgressIndicator.remove();
                    }

                },
                error: function(statusCode, errorMessage, operation) {
                    resultProgressIndicator.remove();
                    if (resultPanel) {
                        resultPanel.empty();
                        resultPanel
                            .append("<div></div>").addClass('error')
                            .append('<div>Error during ' + operation + ' request:  <div>' + errorMessage + '</div></div>');
                    }
                }
            });
        }

    };

    $(function () {
        EQ.view.report.init();
    });

})(jQuery, window);
