;(function ($, window) {

    //Ensure that global variables exist
    var EQ = window.EQ = window.EQ || {};
    EQ.view = EQ.view || {};


    /// <namespace name="EQ.view.grid" version="1.0.0">
    /// <summary>
    /// Contains different functions for managing core EasyQuery pages (views): process user input, render result set, etc.
    /// </summary>
    /// </namespace>

    EQ.view.grid = {

        _resultPanel: null,

        _clearQueryButton: null,

        _loadQueryButton: null,

        _saveQueryButton: null,

        _applyFilterButton: null,

        _exportButtons: null,

        _funcs: {},

        _findControlById: function (controlId) {
            var result = $("#" + controlId);
            if (result.length === 0)
                result = null;

            return result;
        },

        _syncActionAvailable: true,

        init: function (options) {
            options = options || window.easyQueryViewSettings || {};

            EQ.view.applyCommonOptions(options);

            var resultPanelId = options.resultPanelId || "ResultPanel";
            this._resultPanel = this._findControlById(resultPanelId);

            var clearQueryButtonId = options.clearQueryButtonId || "ClearQueryButton";
            this._clearQueryButton = this._findControlById(clearQueryButtonId);

            var loadQueryButtonId = options.loadQueryButtonId || "LoadQueryButton";
            this._loadQueryButton = this._findControlById(loadQueryButtonId);

            var saveQueryButtonId = options.saveQueryButtonId || "SaveQueryButton";
            this._saveQueryButton = this._findControlById(saveQueryButtonId);

            var resultCountSpanId = options.resultCountSpanId || "ResultCount";
            this._resultCountSpan = this._findControlById(resultCountSpanId);


            var applyFilterButtonId = options.applyFilterButtonId || "ApplyFilterButton";
            this._applyFilterButton = this._findControlById(applyFilterButtonId);

            var exportButtonsId = options.exportButtonsId || "ResultExportButtons";
            this._exportButtons = this._findControlById(exportButtonsId);

            if (typeof (options.syncQueryOnChange) === "undefined")
                options.syncQueryOnChange = false;

            if (typeof (options.applyFilterOnStart) === "undefined")
                options.applyFilterOnStart = true;

            this.showChart = typeof (options.showChart) !== "undefined" ? options.showChart : true;

            this.pagingOptions = options.paging;


            var self = this;


            EQ.client.onInit = function () {
                if (options.applyFilterOnStart) {
                    self.applyFilter();
                }
            };

            EQ.client.init();

            this.applyFilterUrl = options.applyFilterUrl || EQ.core.combinePath(EQ.client.serviceUrl, "ApplyFilter");

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
                    var queryName = queryObj.id;
                    if (!queryName)
                        queryName = "LastQuery";

                    EQ.client.saveQuery({
                        "query": queryObj,
                        "queryName": queryName,
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

            if (!this._funcs.applyFilterButtonClick) {
                this._funcs.applyFilterButtonClick = function () {
                    self.applyFilter({ page: 1 });
                };
            }

            if (EQ.client.controls.FBWidget) {
                EQ.client.controls.FBWidget.FilterBar("option", "applyFilterCallback", this._funcs.applyFilterButtonClick);
            }

            // apply filter button             
            //if (this._applyFilterButton) {
            //    this._applyFilterButton.off("click", this._funcs.applyFilterButtonClick);
            //    this._applyFilterButton.on("click", this._funcs.applyFilterButtonClick);
            //}


            if (!this._funcs.queryChangedHandler) {
                this._funcs.queryChangedHandler = function (params) {
                    //self._clearResultPanel();
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

            EQ.client.syncQuery({
                beforeSend: function () {
                },
                success: function (result) {
                },
                error: function (statusCode, errorMessage, operation) {
                }
            });
        },


        applyFilter: function (options) {
            var self = this;
            var paging, pageNavigator;
            var query = EQ.client.getQuery();
            var resultProgressIndicator = $('<div></div>', { 'class': 'result-panel loader' });

            options = options || {};
            if (!options.page) {
                var pg = self.getCurrentPaging();
                options.page = pg.page;
            }

            var requestData = { "query": query.getQueryObject(), "options": options };
            var resultPanel = self._resultPanel;

            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: self.applyFilterUrl,
                data: JSON.stringify(requestData),

                beforeSend: function () {
                    if (resultPanel) {
                        resultPanel.animate({ opacity: '0.5' }, 200);
                        resultPanel.html(resultProgressIndicator);
                    }
                },
                success: function (data) {
                    try {
                        if (resultPanel) {
                            if (data.resultSet) {
                                resultPanel.empty();

                                var grid;
                                if (data.resultSet.table) {
                                    //old format
                                    grid = EQ.view.renderGridOldStyle(data.resultSet.table);
                                }
                                else {
                                    var DataTable = EQ.view.getDataTableClass();

                                    // Create the data table.
                                    var dataTable = new DataTable(data.resultSet);

                                    if (self.showChart) {
                                        var chartPanel = $("<div />").addClass("chart-panel").css({ "float": "right" }).appendTo(resultPanel);
                                        EQ.view.drawChart(dataTable, chartPanel.get(0));
                                    }

                                    grid = EQ.view.renderGridByDataTable(dataTable);
                                }

                                var gridPanel = $("<div />").addClass("grid-panel").appendTo(resultPanel);
                                gridPanel.append(grid);

                                if (self._exportButtons) {
                                    self._exportButtons.show();
                                }

                                //paging
                                paging = result.paging;
                                if (paging && paging.enabled && paging.pageCount > 1) {
                                    pageNavigator = EQ.view.renderPageNavigator({
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
                            else {
                                resultPanel.html(data);
                                paging = self.getCurrentPaging();
                                if (paging.pageCount > 1) {
                                    var pageNavigatorPlaceholder = $("#PageNavigator");
                                    if (pageNavigatorPlaceholder.length > 0) {
                                        pageNavigator = EQ.view.renderPageNavigator({
                                            pageIndex: paging.pageIndex,
                                            pageCount: paging.pageCount,
                                            pageSelectedCallback: function (pageNum) {
                                                //alert("Go to page " + $(this).data("page"));
                                                self.applyFilter({ page: pageNum });
                                            }
                                        });

                                        pageNavigatorPlaceholder.append(pageNavigator);
                                    }
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

        },

        getCurrentPaging: function () {
            result = { pageIndex: 1, pageCount: 1 };
            var pageNavigator = $("#PageNavigator");
            if (pageNavigator.length > 0) {
                var pageIndex = pageNavigator.data("pageindex");
                var pageCount = pageNavigator.data("pagecount");
                if (pageIndex)
                    result.pageIndex = pageIndex;
                if (pageCount)
                    result.pageCount = pageCount;
            }
            return result;
        }

    }

    $(function () {
        EQ.view.grid.init();
    });

})(jQuery, window);
