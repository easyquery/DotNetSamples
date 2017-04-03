;(function ($, window) {

    //Ensure that global variables exist
    var EQ = window.EQ = window.EQ || {};

    EQ.view = EQ.view || {};

    /// <namespace  version="1.0.0">
    /// <summary>
    /// Contains different functions for managing basic EasyQuery views: process user input, render result set, etc.
    /// </summary>
    /// </namespace>
    EQ.view.basic = {

        _resultPanel: null,

        _sqlPanel: null,

        _clearQueryButton: null,

        _loadQueryButton: null,

        _saveQueryButton: null,

        _executeQueryButton: null,

        _exportButtons: null,

        _chartOptions: null,

        _funcs: {},

        _findControlById: function (controlId) {
            var result = $("#" + controlId);
            if (result.length == 0)
                result = null;

            return result;
        },

        _syncActionAvailable: true,

        init: function (options) {
            options = options || window.easyQueryViewSettings || {};

            EQ.view.applyCommonOptions(options);

            var resultPanelId = options.resultPanelId || "ResultPanel";
            this._resultPanel = this._findControlById(resultPanelId);

            var sqlPanelId = options.sqlPanelId || "SqlPanel";
            this._sqlPanel = this._findControlById(sqlPanelId);

            var clearQueryButtonId = options.clearQueryButtonId || "ClearQueryButton";
            this._clearQueryButton = this._findControlById(clearQueryButtonId);

            var loadQueryButtonId = options.loadQueryButtonId || "LoadQueryButton";
            this._loadQueryButton = this._findControlById(loadQueryButtonId);

            var saveQueryButtonId = options.saveQueryButtonId || "SaveQueryButton";
            this._saveQueryButton = this._findControlById(saveQueryButtonId);

            var executeQueryButtonId = options.executeQueryButtonId || "ExecuteQueryButton";
            this._executeQueryButton = this._findControlById(executeQueryButtonId);

            var resultCountSpanId = options.resultCountSpanId || "ResultCount";
            this._resultCountSpan = this._findControlById(resultCountSpanId);

            var exportButtonsId = options.exportButtonsId || "ResultExportButtons";
            this._exportButtons = this._findControlById(exportButtonsId);

            if (typeof (options.rebuildOnQueryChange) === "undefined")
                options.rebuildOnQueryChange = true;

            if (typeof (options.syncQueryOnChange) === "undefined")
                options.syncQueryOnChange = true;

            this.showChart = typeof (options.showChart) !== "undefined" ? options.showChart : true;

            this.pagingOptions = options.paging;

            this._chartOptions = options.defaultChartType ? { chartType: options.defaultChartType } : null;

            var self = this;

            EQ.client.init();


            //Clear Query button
            if (!this._funcs.clearButtonClick) {
                this._funcs.clearButtonClick = function () {
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


            //clear button
            if (this._clearQueryButton) {
                this._clearQueryButton.off("click", this._funcs.clearButtonClick);
                this._clearQueryButton.on("click", this._funcs.clearButtonClick);
            }


            // Load Query button
            if (!this._funcs.loadQueryButtonClick) {
                this._funcs.loadQueryButtonClick = function () {
                    EQ.client.loadQuery({
                        queryId: "LastQuery",
                        queryName: "LastQuery",
                        success: function (data) {
                            self._clearErrors();
                            self._clearResultPanel();
                            //buildQuery();
                        },
                        error: self._errorHandler
                    });
                };
            }

            // load query button
            if (this._loadQueryButton) {
                this._loadQueryButton.off("click", this._funcs.loadQueryButtonClick);
                this._loadQueryButton.on("click", this._funcs.loadQueryButtonClick);
            }


            // Save Query button
            if (!this._funcs.saveQueryButtonClick) {
                this._funcs.saveQueryButtonClick = function () {
                    var queryObj = EQ.client.getQuery();
                    queryObj.setId("LastQuery");

                    EQ.client.saveQuery({
                        "query": queryObj,
                        "success": function () {
                            window.alert("Query saved!");
                        },
                        error: self._errorHandler
                    });
                };
            }


            // save query button
            if (this._saveQueryButton) {
                this._saveQueryButton.off("click", this._funcs.saveQueryButtonClick);
                this._saveQueryButton.on("click", this._funcs.saveQueryButtonClick);
            }

            // Execute Query button             
            if (!this._funcs.executeQueryButtonClick) {
                this._funcs.executeQueryButtonClick = function () {
                    self.buildAndExecute();
                };
            }

            if (this._executeQueryButton) {
                this._executeQueryButton.off("click", this._funcs.executeQueryButtonClick);
                this._executeQueryButton.on("click", this._funcs.executeQueryButtonClick);
            }


            // Query changed handler
            if (!this._funcs.queryChangedHandler) {
                this._funcs.queryChangedHandler = function (params) {
                    self._clearSqlPanel();
                    self._clearResultPanel();
                    if (options.syncQueryOnChange && options.rebuildOnQueryChange) {
                        if (self._syncActionAvailable)
                            self.syncQuery();
                        else
                            self.buildQuery();
                    }
                };
            }

            var query = EQ.client.getQuery();

            query.addChangedCallback(this._funcs.queryChangedHandler);
        },

        _clearErrorsInPanel: function (panel) {
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

            if (this._resultCountSpan) {
                this._resultCountSpan.hide();
            }
        },

        syncQuery: function () {
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
                    if (statusCode == 404) {
                        self.buildQuery();
                        self._syncActionAvailable = false;
                    }
                }
            });
        },


        //deprecated
        buildQuery: function () {
            var self = this;
            var query = EQ.client.getQuery();

            var sqlProgressIndicator = $('<div></div>', { 'class': 'result-panel loader' });
            var sqlPanel = self._sqlPanel;

            EQ.client.buildQuery({
                "query": query,
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
                    if (sqlPanel) {
                        sqlPanel.empty();
                        sqlPanel.animate({ 'opacity': 1 }, 200);
                        sqlPanel.addClass('error').append('<div>Error during ' + operation + ' request:  <div>' + errorMessage + '</div></div>');
                    }
                }
            });
        },

        renderSqlStatement: function (sql) {
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

        buildAndExecute: function (options) {
            var self = this;
            options = options || { page: 1 };
            var query = EQ.client.getQuery();

            var resultProgressIndicator = $('<div></div>', { 'class': 'result-panel loader' });
            var resultPanel = self._resultPanel;
            EQ.client.buildAndExecute({
                "query": query,
                "options": {
                    //"formats": {bracketJoins: false},
                    //"sqlOptions": {selectTop: "100"},
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
                            if (result.resultSet && result.resultSet.cols.length > 0) {

                                var grid;
                                if (result.resultSet.table) {
                                    //old format
                                    grid = EQ.view.renderGridOldStyle(result.resultSet.table);
                                }
                                else {
                                    var DataTable = EQ.view.getDataTableClass();

                                    //var DataTable1 = google.visualization.DataTable;

                                    // Create the data table.

                                    var dataTable = new DataTable(result.resultSet); 

                                    if (self.showChart) {
                                        var chartPanel = $("<div />")
                                            .addClass("chart-panel")
                                            //.css({ "float": "right", "width": "360px" })
                                            .appendTo(resultPanel);

                                        EQ.view.drawChart(dataTable, chartPanel.get(0), self._chartOptions);
                                    }

                                    grid = EQ.view.renderGridByDataTable(dataTable);

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
                error: function (statusCode, errorMessage, operation) {
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

    EQ.view.init = EQ.view.basic.init;
    
    $(function () {
        EQ.view.basic.init();
    });

})(jQuery, window);
