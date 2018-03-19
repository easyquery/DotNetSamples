var __antiForgeryTokenInput = document.querySelector('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]');
var __antiForgeryToken = __antiForgeryTokenInput != null ? __antiForgeryTokenInput.value : "";

window.easyQuerySettings = {
    serviceUrl: "/EasyQuery",
    loadDefaultModel: true,
    loadQueryOnStart: true,
    //locale: "fr",
    entitiesPanel: {
        showCheckboxes: true
    },
    columnsPanel: {
        allowAggrColumns: true,
        attrElementFormat: "{entity} {attr}",
        showColumnCaptions: true,
        adjustEntitiesMenuHeight: false,
        menuOptions: {
            showSearchBoxAfter: 30,
            activateOnMouseOver: true
        }

    },
    queryPanel: {
        showPoweredBy: false,
        alwaysShowButtonsInPredicates: false,
        //dateFormatValue: "dd.mm.yy",
        //dateFormatDisplay: "dd M yy",
        menuOptions: {
            showSearchBoxAfter: 20,
            activateOnMouseOver: true
        }

    },
    syncQueryOptions: {
        sqlOptions: { SelectDistinct: true }
    },

    listCache: EQ.listcache.localstorage.getCacheObject(),

    listRequesHandler: function (params, onResult) {
        var processed = true;
        if (params.listName == "RegionList") {
            var query = EQ.client.getQuery();
            var country = query.getOneValueForAttr(query, "Customer.Country");
            if (country == "Canada") {
                onResult([
                    { id: "BC", text: "British Columbia" },
                    { id: "Quebec", text: "Quebec" }
                ]);

            }
            else {
                onResult([
                    { id: "CA", text: "California" },
                    { id: "CO", text: "Colorado" },
                    { id: "OR", text: "Oregon" },
                    { id: "WA", text: "Washington" }
                ]);
            }
        }
        else
            processed = false;

        return processed;

    },

    antiForgeryToken: __antiForgeryToken

};

window.easyQueryViewSettings = {
    showChart: true,
    useEasyChart: true
};

function getPrefix() {
    var res = window.location.pathname;
    if (res.charAt(res.length - 1) !== '/')
        res = res + '/';
    return res;
}
