window.addEventListener('load', function () {
    // Grid selector renderer
    function renderGridSelector() {
        var gridSelector = document.createElement('select');
        gridSelector.classList.add('eqv-select');

        var menuContent = document.getElementsByClassName('eqv-menu-content')[0];
        menuContent.appendChild(gridSelector);

        var defOption = document.createElement('option');
        defOption.setAttribute('value', 'default');
        defOption.text = "EasyGrid";
        gridSelector.appendChild(defOption);

        var agOption = document.createElement('option');
        agOption.setAttribute('value', 'ag-grid');
        agOption.text = "AgGrid";
        gridSelector.appendChild(agOption);


        var kendoOption = document.createElement('option');
        kendoOption.setAttribute('value', 'kendo-grid');
        kendoOption.text = "Kendo";
        gridSelector.appendChild(kendoOption);

        var gridType = this.localStorage.getItem('grid-type') || 'default';
        gridSelector.value = gridType;

        gridSelector.addEventListener('change', function () {
            window.localStorage.setItem('grid-type', gridSelector.value);
            window.location.reload();
        });
    }

    // Code samples renderer
    function renderSamplesSelector() {
        var samples = {
            showAggregationDialog: {
                name: 'Show aggregation/grouping dialog',
                handler: showAggregationDialog
            },

            simpleColToAggr: {
                name: 'Currency columns to aggregates',
                handler: convertColumnsToAggr
            },

            manageTotals: {
                name: 'Manage sub and grand totals',
                handler: manageSubtotals
            }

        }

        var container = document.createElement('div');
        container.className = "eqv-dropdown-container";

        var a = document.createElement('a');
        a.classList = 'eqv-button eqv-drop-button'
        a.innerHTML = 'Code samples <span style="float: right">▼</span>';

        a.addEventListener('click', function () {
            content.classList.add('eqv-dropdown-show');
        });

        a.addEventListener('blur', function () {
            content.classList.remove('eqv-dropdown-show');
        });

        container.appendChild(a);

        var content = document.createElement('div');
        content.className = 'eqv-dropdown-content';

        Object.keys(samples).forEach(key => {
            var sample = samples[key];
            var link = document.createElement('a');
            link.innerHTML = sample.name;
            link.addEventListener('click', sample.handler);
            content.append(link);
        });

        container.appendChild(content);

        var menuContent = document.getElementsByClassName('eqv-menu-content')[0];
        menuContent.appendChild(container);
    }

    renderSamplesSelector();
    renderGridSelector();
});

function configureOptionsForSelectedGrid(viewOptions) {
    var gridType = window.localStorage.getItem('grid-type');
    if (gridType == 'ag-grid') {
        viewOptions.calcTotals = false;
        viewOptions.result.resultGridResolver = function (slot) {
            return new EqAgGrid(slot);
        }
    }
    else if (gridType == 'kendo-grid') {
        viewOptions.result.resultGridResolver = function (slot) {
            return new KendoGridWidget(slot);
        }
    }
}

function showAggregationDialog() {
    var view = document['AdvancedSearchView'];
    if (!view) {
        throw "Can't find view object";
    }
    var dialog = new easyquery.ui.AggrSettingsDialog(view.getContext());
    dialog.show();
}
