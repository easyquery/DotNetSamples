import { EqHtmlGrid, EqViewOptions } from "@easyquery/ui"
import { Widget } from "@easyquery/core"
import { DataFilterViewJQuery } from '@easyquery/ui-jquery'

// import '../../../packs/ui-jquery/src/jqui/jqueryui-widgets'
// import {EqBasicView} from '../../../packs/ui-jquery/src/views/eq_basic_view'

declare var $: any

class PartialViewDataFilter extends DataFilterViewJQuery {

    constructor() {
        super();
    }

    protected createResultGridWidget(element: HTMLElement): Widget {
        return new EqHtmlGrid(element);
    }
}

document.addEventListener('DOMContentLoaded', () => {
    let options: EqViewOptions = {
        paging: {
            useBootstrap: true
        },
        context: {
            loadModelOnStart: true,
            clearResultOnQueryChange: false,
            broker: {
                serviceUrl: "/Order"
            },
            widgets: {
                filterBar: {
                    queryPanel: {
                        attrElementFormat: "{entity} {attr}"
                    }
                }
            }
        }
    }
    let basicView = new PartialViewDataFilter();
    basicView.init(options);
    document['easyQueryView'] = basicView;
});


