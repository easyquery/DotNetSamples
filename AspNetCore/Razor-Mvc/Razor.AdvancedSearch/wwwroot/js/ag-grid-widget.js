
/*
 * ES6 implementation
 * 
class EqAgGrid extends easyquery.ui.Grid {

    pageSize = 30;

    constructor(slot) {
        super(slot);

        slot.classList.add('ag-theme-alpine');

    }

    render() {

        this.agGridOptions = {};

        if (this.context.resultTable && this.context.resultTable.columns.count > 0) {
            this.applyDisplayFormats();

            this.agGridOptions.columnDefs = this.getColumnsDefs();
            this.agGridOptions.pagination = true;
            this.agGridOptions.rowModelType = 'infinite';
            this.agGridOptions.cacheBlockSize = this.pageSize;
            this.agGridOptions.paginationPageSize = this.pageSize;

            this.agGridOptions.datasource = {
                rowCount: null,
                getRows: (params) => {
                    this.context.resultTable.getRows({
                        offset: params.startRow,
                        limit: this.pageSize
                    })
                        .then(rows => {
                            this.applyDisplayFormats();
                            params.successCallback(this.convertToRowData(rows), this.context.resultTable.getTotal());
                        });
                }
            };
        }

        this.agGrid = new agGrid.Grid(this.slot, this.agGridOptions);
    }

    clear() {
        if (this.agGrid) {
            this.agGrid.destroy();
            this.agGrid = null;
        }
    }

    // specify the columns
    getColumnsDefs() {
        const columns = [];

        const resultTable = this.context.resultTable;
        for (const col of resultTable.columns.getItems()) {
            columns.push({ headerName: col.label, field: col.id });
        }

        return columns;
    }

    // specify the data
    convertToRowData(rows) {
        const result = [];

        const resultTable = this.context.resultTable;
        for (const row of rows) {
            const dataRow = {};
            for (let i = 0; i < resultTable.columns.count; i++) {
                const col = resultTable.columns.get(i);
                dataRow[col.id] = row.getValue(i);
            }

            result.push(dataRow);
        }

        return result;
    }
}
*/

var EqAgGrid = function (slot) {
    Object.getPrototypeOf(EqAgGrid.prototype).constructor.call(this, slot);
    slot.classList.add('ag-theme-alpine');

    this.pageSize = 30;
};

EqAgGrid.prototype = Object.create(easyquery.ui.Grid.prototype);


EqAgGrid.prototype.render = function () {

    this.agGridOptions = {};

    if (this.context.resultTable && this.context.resultTable.columns.count > 0) {
        this.applyDisplayFormats();

        this.agGridOptions.columnDefs = this.getColumnsDefs();
        this.agGridOptions.pagination = true;
        this.agGridOptions.rowModelType = 'infinite';
        this.agGridOptions.cacheBlockSize = this.pageSize;
        this.agGridOptions.paginationPageSize = this.pageSize;

        function getRowsHandler(params) {
            this.context.resultTable.getRows({
                offset: params.startRow,
                limit: this.pageSize
            })
            .then(function (rows) {
                this.applyDisplayFormats();
                params.successCallback(this.convertToRowData(rows), this.context.resultTable.getTotal());
            }.bind(this));
        }

        this.agGridOptions.datasource = {
            rowCount: null,
            getRows: getRowsHandler.bind(this)
        };
    }

    this.agGrid = new agGrid.Grid(this.slot, this.agGridOptions);
}

EqAgGrid.prototype.clear = function () {
    if (this.agGrid) {
        this.agGrid.destroy();
        this.agGrid = null;
    }
}

EqAgGrid.prototype.getColumnsDefs = function () {
    var columns = [];

    var resultTable = this.context.resultTable;
    for (var i = 0; i < resultTable.columns.count; i++) {
        var col = resultTable.columns.get(i);
        columns.push({ headerName: col.label, field: col.id });
    }

    return columns;
}

EqAgGrid.prototype.convertToRowData = function (rows) {
    var result = [];

    var resultTable = this.context.resultTable;

    for (var rowIdx = 0; rowIdx < rows.length; rowIdx++) {
        var row = rows[rowIdx];
        var dataRow = {};
        for (var i = 0; i < resultTable.columns.count; i++) {
            var col = resultTable.columns.get(i);
            dataRow[col.id] = row.getValue(i);
        }

        result.push(dataRow);
    }

    return result;
}