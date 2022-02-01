function convertColumnsToAggr() {
    var view = document['AdvancedSearchView'];

    var model = view.getContext().getModel();
    var query = view.getContext().getQuery();
    
    query.getColumns().forEach(col => {
        if (col.expr.tag === easyquery.core.ExprTag.EntityAttribute) {
            var attr = model.getAttributeById(col.expr.value);
            if (attr.dataType === easydata.core.DataType.Currency) {
                /**
                 * Avaliable functions by default:
                 * SUM, COUNT, CNTDST, AVG, MIN, MAX
                 * */
                query.changeColumnType(col,
                    easyquery.core.ExprTag.AggregateFunction, { funcId: "SUM" });
            }
        }
    });

    query.fireColumnsChangedEvent();
}