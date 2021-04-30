function manageSubtotals() {

    var view = document['AdvancedSearchView'];
    var context = view.getContext();

    // loads predefined query
    // for example
    context.loadQuery({
        queryId: 'totals-example',
        success: function () {

            var query = context.getQuery();
            var settings = context.getTotalsSettings();

            var cols = query.getUsedInTotalsColumns(); // get columns that can be used in grouping

            var colsWithSubTotals = ["Customer.Country", "Customer.ContactName"];
            cols.forEach(col => {
                settings.cols[col.id] = {
                    calcSubTotals: colsWithSubTotals.indexOf(col.expr.value) >= 0 // turn on sub totals
                };
            });

            settings.calcGrandTotals = true; // turn on grand totals
            context.setTotalsSettings(settings, true); // update settings silents
            
            view.fetchData(); // fetch data
        }
    });
}