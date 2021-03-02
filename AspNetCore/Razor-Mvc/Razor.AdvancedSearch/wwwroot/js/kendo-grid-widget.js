class KendoGridWidget extends easyquery.ui.Grid {

    constructor(slot) {
        super(slot);
    }

    init(options) {
        super.init(options);

        $(this.slot).kendoGrid({
            scrollable: false,
            pageable: true
        });

        this.grid = $(this.slot).data("kendoGrid");
    }

    render() {
        if (!this.context.resultTable.getCachedCount())
            return;

        this.grid.setOptions({
            columns: this.getColumnsDefs(),
        });

        const dataSource = this.createDataSource();
        this.grid.setDataSource(dataSource);
    }

    getAggregate() {
        const aggrCols = this.context.getQuery().getAggregatedColumns();
        const aggregate = [];
        for (const col of aggrCols) {
            aggregate.push({
                field: this.idToField(col.id),
                aggregate: col.expr.func
            });
        }

        return aggregate;
    }

    // specify the columns
    getColumnsDefs() {
        const columns = [];

        const resultTable = this.context.resultTable;
        const query = this.context.getQuery();
        for (const col of resultTable.columns.getItems()) {
            const column = {
                title: col.label,
                field: this.idToField(col.id)
            };
            columns.push(column);
        };

        if (this.context.calcTotals) {
            const settings = this.context.getTotalsSettings();
            if (settings.calcGrandTotals) {
                const aggrCols = query.getAggregatedColumns();
                for (const aggrCol of aggrCols) {
                    for (const col of columns) {
                        if (col.field === this.idToField(aggrCol.id)) {
                            col.footerTemplate = `${aggrCol.expr.func}: #=data.${col.field} ? data.${col.field}.${aggrCol.expr.func}: 0#`
                            break;
                        }
                    }
                }
            }

            const cols = query.getUsedInTotalsColumns();
            for (const col of cols) {
                for (const kendoCol of columns) {
                    if (kendoCol.field === this.idToField(col.id)) {
                        kendoCol.groupHeaderTemplate = this.buildGroupHeaderTemplate(col, settings);
                        kendoCol.hidden = true;
                        break;
                    }
                }
            }
        }

        return columns;
    }

    buildGroupHeaderTemplate(col, settings) {
        let result = `${col.caption}: #=value#`;
        if (!settings.cols || settings.cols[col.id].calcSubTotals) {
            const aggrCols = this.context.getQuery().getAggregatedColumns();
            for (const col of aggrCols) {
                result += `   ${col.caption}: #=aggregates.${this.idToField(col.id)}.${col.expr.func} #`;
            }
        }
        return result;
    }

    idToField(id) {
        return id.replace('-', '_');
    }

    fieldToId(field) {
        return field.replace('_', '-');
    }

    // specify the data
    convertToGridData(rows) {
        const result = [];

        const resultTable = this.context.resultTable;
        for (const row of rows) {
            const dataRow = {};
            for (let i = 0; i < resultTable.columns.count; i++) {
                const col = resultTable.columns.get(i);
                dataRow[this.idToField(col.id)] = row.getValue(i);
            }

            result.push(dataRow);
        }

        return result;
    }

    createDataSource() {

        const options = {
            transport: {
                read: async (options) => {
                    console.log("Data source options", options);
                    const rows = await this.context.resultTable.getRows({
                        offset: options.data.skip,
                        limit: options.data.take
                    });

                    const data = this.convertToGridData(rows);
                    let groups = [], aggregates = {};
                    if (options.data.group && options.data.group.length) {
                        groups = await this.buildGroups(0, data);
                    }
                    if (options.data.aggregate && options.data.aggregate.length) {
                        aggregates = await this.buildAggregates();
                    }
                    const result = { data, groups, aggregates };
                    console.log("Result", result);
                    options.success(result);
                }
            },
            serverGrouping: true,
            serverAggregates: true,
            serverPaging: true,
            schema: {
                data: (r) => r.data,
                groups: (r) => r.groups,
                aggregates: (r) => r.aggregates,
                total: () => this.context.resultTable.getTotal()
            },
            pageSize: 10,
        };

        if (this.context.calcTotals) {
            const query = this.context.getQuery();
            if (query.hasEnabledAggrColumns()) {
                const settings = this.context.getTotalsSettings();
                options.aggregate = this.getAggregate();
                if (settings.cols) {
                    options.group = [];
                    for (const colId in settings.cols) {
                        options.group.push({
                            field: this.idToField(colId),
                            aggregates: this.getAggregate()
                        });
                    }
                }
            }
        }

        return new kendo.data.DataSource(options);
    }

    async buildAggregates() {
        const cols = this.context.resultTable.columns;
        const row = new easydata.core.DataRow(cols, new Array(cols.count));

        // fill aggr cols in data row
        const totals = this.context.getServices()
            .getTotalCalculator()
            .getTotals();
        await totals.fillTotals(0, row);

        const aggregates = {};
        const aggrCols = this.context.getQuery().getAggregatedColumns();
        for (const col of aggrCols) {
            aggregates[this.idToField(col.id)] = {};
            aggregates[this.idToField(col.id)][col.expr.func] = row.getValue(col.id); // get total value from DataRow
        }

        return aggregates;
    }

    async buildGroups(colIndex, data) {
        const groups = [];
        const query = this.context.getQuery();
        const cols = query.getUsedInTotalsColumns();
        const col = cols[colIndex];
        const field = this.idToField(col.id);
        let prevValue;
        for (let i = 0; i < data.length; i++) {
            const value = data[i][field];
            if (value !== prevValue) {
                groups.push(await this.buildGroup({
                    field: field,
                    value: value,
                    colIndex: colIndex,
                    lastIndex: cols.length - 1,
                    data: data.filter(rec => rec[field] === value)
                }));
                prevValue = value;
            }
        }
        return groups;
    }

    async buildGroup(options) {
        const group = {
            field: options.field,
            value: options.value,
            hasSubgroups: options.colIndex !== options.lastIndex
        };
        if (!group.hasSubgroups) {
            group.items = options.data;
        }
        else {
            group.items = await this.buildGroups(options.colIndex + 1, options.data);
        }

        group.aggregates = {};
        const settings = this.context.getTotalsSettings();
        const colId = this.fieldToId(group.field);
        if (!settings.cols || settings.cols[colId].calcSubTotals) {
            const totals = this.context.getServices()
                .getTotalCalculator()
                .getTotals();

            // fill data row with key columns 
            const cols = this.context.resultTable.columns;
            const totalCols = this.context.getQuery().getUsedInTotalsColumns();
            const values = options.data[0];
            const row = new easydata.core.DataRow(cols, new Array(cols.count));
            for (const col of totalCols) {
                const value = values[this.idToField(col.id)];
                row.setValue(col.id, value);
            }

            // fill aggr cols in data row
            await totals.fillTotals(options.colIndex + 1, row);

            const aggrCols = this.context.getQuery().getAggregatedColumns();
            for (const col of aggrCols) {
                group.aggregates[this.idToField(col.id)] = {};
                group.aggregates[this.idToField(col.id)][col.expr.func] = row.getValue(col.id); // get total value from DataRow
            }
        }

        return group;
    }

    clear() {
        this.grid.setDataSource(new kendo.data.DataSource({
            data: []
        }));
        this.grid.setOptions({
            columns: []
        });
    }
}