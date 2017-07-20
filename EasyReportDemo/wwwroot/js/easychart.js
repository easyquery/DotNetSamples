;(function ($, undefined) {

    $.widget('eqjs.EasyChart', {
        _dataTable: null,
        _dataView: null,

        _potentialLabelColumns: [],
        _potentialDataColumns: [],
        _labelColumn: null,
        _dataColumns: [],

        _chartDiv: null,
        _settingsDiv: null,

        _ccHeader: "eqjs-chart-header",
        _ccMain: "eqjs-chart-main",
        _ccSettings: "eqjs-chart-settings",
        _ccContent: "eqjs-chart-content",


        _stateChangedCallback: null,

        _defaultChartType: 10,
        
        options: {

            //3 - Column, 4 - Histogram, 5 - Bar, 6 - Combo, 7 - Area, 9 - Line, 10 - Pie, 12 - Donut, 17 - Gauge, 18 - Candlestick
            chartType: this._defaultChartType
        },

        _chartTypes : {
            "3": { name: "Column chart" },
            //"4": { name: "Histogram" },
            "5": { name: "Bar chart" },
            "6": { name: "Combo chart" },
            "7": { name: "Area chart" },
            "9": { name: "Line chart" },
            "10": { name: "Pie chart", chartAreaWidth: "100%" },
            //"17": { name: "Gauge" },
            //"18": { name: "Candlestick" }
        },

        _setOption: function (key, value) {
            if (arguments.length == 2) {
                if (key == "chartType") {
                    if (value && this._chartTypes.hasOwnProperty(value.toString())) {
                        this.options.chartType = value;
                    }
                    else {
                        this.options.chartType = this._defaultChartType;
                    }
                }
                else {
                    this.options[key] = value;
                };
                this._render();
                return this;
            }
            else {
                return this.options[key];
            }
        },

        _create: function () {
            
        },
		
        _clear: function () {
            this.element.html('')
        },


        init: function (dataTable, state, stateChangedCallback) {
            var self = this;

            state = state || { dataColumns: [] };
            this._setOption("chartType", state.chartType || this.options.chartType);


            self._dataView = null;
            self._dataTable = dataTable;

            self._stateChangedCallback = stateChangedCallback;

            if (self._dataTable) {
                self._potentialLabelColumns = [];
                self._potentialDataColumns = [];
                var colNum = self._dataTable.getNumberOfColumns();
                var colType, colLabel;

                var stateLabelIsOk = false;
                var stateDataColumns = [].concat(state.dataColumns);

                for (var i = 0; i < colNum; i++) {
                    colType = self._dataTable.getColumnType(i);
                    colLabel = self._dataTable.getColumnLabel(i);
                    if (colType == 'string') {
                        self._potentialLabelColumns.push({ idx: i, label: colLabel });
                        if (state.labelColumn == i) {
                            stateLabelIsOk = true;
                        }
                    }
                    else if (colType == 'number') {
                        self._potentialDataColumns.push({ idx: i, label: colLabel });
                        while (stateDataColumns.indexOf(i) >= 0) {
                            stateDataColumns.splice(stateDataColumns.indexOf(i), 1);
                        }
                    }
                };

                if (self._potentialLabelColumns.length > 0) {
                    if (stateLabelIsOk) {
                        self._labelColumn = state.labelColumn;
                    }
                    else {
                        self._labelColumn = self._potentialLabelColumns[0].idx;
                    }
                };

                if (self._potentialDataColumns.length > 0) {
                    if (state.dataColumns.length > 0 && stateDataColumns.length == 0) {  //all state data columns are available in dataTable
                        self._dataColumns = [].concat(state.dataColumns);
                    }
                    else {
                        self._dataColumns = [].concat(self._potentialDataColumns[0].idx);
                    }
                };

                if (self._labelColumn != null && self._dataColumns && self._dataColumns.length > 0) {
                    self._dataView = new google.visualization.DataView(this._dataTable);
                    self._updateChartColumns();
                }
            };

            this.refresh();
        },

        _updateChartColumns: function () {
            if (this._dataView) {
                this._dataView.setColumns([].concat(this._labelColumn).concat(this._dataColumns));
            }
        },

        refresh: function () {
            this._render();
        },

        resize: function () {
            this._render();
        },

        _render: function () {
            var self = this;

            this._clear();

            var headerDiv = $('<div></div>')
                    .addClass(self._ccHeader)
                ,
                mainDiv = $('<div></div>')
                    .addClass(self._ccMain);

            var chartTypeSelector = $("<select></select>");

            for (var ctkey in this._chartTypes) {
                var ctype = this._chartTypes[ctkey];
                var sopt = $('<option />', { value: ctkey, text: ctype.name });
                
                sopt.appendTo(chartTypeSelector);
            }

            chartTypeSelector.val(this.options.chartType)

            chartTypeSelector.appendTo(headerDiv);

            chartTypeSelector.change(function () {
                var ctkey = $(this).val();
                self.options.chartType = parseInt(ctkey);
                self.refresh();
                self._onStateChanged();
            });

            this._settingsDiv = $("<div></div>")
                .addClass(this._ccSettings)
                .hide()
                .appendTo(mainDiv);

            this._chartDiv = $("<div></div>")
                .addClass(this._ccContent)
                .hide()
                .appendTo(mainDiv)
                .fadeIn(1000);


            this._initSettingsDiv();


            if (this._dataView != null) {
                var chartSettingsBtn = $("<div></div>")
                    .addClass("eqjs-chart-settings-icon")
                    .attr("title", "Settings")
                    .appendTo(headerDiv);


                chartSettingsBtn.click(function () {
                    self._toggleSettings();
                });


                self._redrawTwice();
            }
            else {
                $("<div></div>")
                    .addClass("eqjs-chart-no-data")
                    .text("No Data")
                    .appendTo(mainDiv);
            }

            this.element.append(headerDiv);
            this.element.append(mainDiv);
        },

        _toggleSettings: function (callback) {
            var first, second;

            if (this._chartDiv.is(":visible")) {
                first = this._chartDiv;
                second = this._settingsDiv;
            }
            else {
                first = this._settingsDiv;
                second = this._chartDiv;
            }

            first.fadeToggle({
                duration: 200,
                complete: function () {
                    second.fadeToggle({
                        duration: 200,
                        complete: callback
                    });
                }
            });
        },

        _initSettingsDiv: function () {
            var self = this;

            $("<div></div")
                .text("SETTINGS")
                .addClass(this._ccSettings + '-header')
                .appendTo(this._settingsDiv);

// Label column
            var labelDiv = $("<div></div")
                .addClass(this._ccSettings + '-single')
                .appendTo(this._settingsDiv);

            $("<span></span>")
                .text("Label column: ")
                .css({ "display": "inline-block" })
                .appendTo(labelDiv);

            var labelColumnSelector = $("<select></select>")
                .css({ "display": "inline-block" })
                .appendTo(labelDiv);

            this._potentialLabelColumns.forEach(function(item, idx, arr) {
                var sopt = $('<option />', { value: item.idx, text: item.label });
                sopt.appendTo(labelColumnSelector);
            });

            labelColumnSelector.val(this._labelColumn)

            labelColumnSelector.change(function () {
                self._labelColumn = parseInt($(this).val());
                self._updateChartColumns()
                self._toggleSettings(function () {
                    self._redrawTwice();
                    self._onStateChanged();
                });
            });

// Data column
            var dataDiv = $("<div></div")
                .addClass(this._ccSettings + '-single')
                .appendTo(this._settingsDiv);

            $("<span></span>")
                .text("Data column: ")
                .css({ "display": "inline-block" })
                .appendTo(dataDiv);

            var dataColumnSelector = $("<select></select>")
                .css({ "display": "inline-block" })
                .appendTo(dataDiv);

            this._potentialDataColumns.forEach(function (item, idx, arr) {
                var sopt = $('<option />', { value: item.idx, text: item.label });
                sopt.appendTo(dataColumnSelector);
            });

            dataColumnSelector.val(this._dataColumns[0]);

            dataColumnSelector.change(function () {
                self._dataColumns[0] = parseInt($(this).val());
                self._updateChartColumns()
                self._toggleSettings(function () {
                    self._redrawTwice();
                    self._onStateChanged();
                });
            });

        },

        redraw: function () {
            if (this._dataView != null) {
                var chartOptions = {
                    width: "100%",
                    height: "100%",
                    chartArea: { width: this._chartTypes[this.options.chartType.toString()].chartAreaWidth || "50%" }
                };

                var chart = this._createChart(this._chartDiv.get(0));
                chart.draw(this._dataView, chartOptions);
            }
        },

        _redrawTwice: function () {
            var self = this;

            self.redraw();
            setTimeout(function () { self.redraw() }, 200);
        },

        _createChart: function (placeholder) {
            switch (this.options.chartType) {
                case 3:
                    return new google.visualization.ColumnChart(placeholder);
                case 4:
                    return new google.visualization.Histogram(placeholder);
                case 5:
                    return new google.visualization.BarChart(placeholder);
                case 6:
                    return new google.visualization.ComboChart(placeholder);
                case 7:
                    return new google.visualization.AreaChart(placeholder);
                case 9:
                    return new google.visualization.LineChart(placeholder);
                case 10:
                    return new google.visualization.PieChart(placeholder);
                case 17:
                    return new google.visualization.Gauge(placeholder);
                case 18:
                    return new google.visualization.CandlestickChart(placeholder);
                default:
                    return new google.visualization.PieChart(placeholder);
            }
        },

        _onStateChanged: function () {
            if (this._stateChangedCallback) {
                var state = {};

                state.chartType = this.options.chartType;
                state.labelColumn = this._labelColumn;
                state.dataColumns = [].concat(this._dataColumns);

                this._stateChangedCallback(state);
            }
        }

    });

})(jQuery);
