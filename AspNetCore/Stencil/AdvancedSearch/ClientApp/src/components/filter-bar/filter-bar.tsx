import { Component, Element, h } from '@stencil/core';

import { DataFilterView, EqViewOptions } from '@easyquery/ui';
import '@easyquery/enterprise';

@Component({
    tag: 'filter-bar',
    styleUrl: './filter-bar.css',
    shadow: true
})
export class FilterBar {

    private options: EqViewOptions;
    private view: DataFilterView;

    @Element() host: HTMLDivElement;
    
    componentWillLoad() {
        this.options = {
            shadowRoots: [ this.host.shadowRoot ],
            loadModelOnStart: true,
            result: {
                paging: {
                    pageSize: 15
                },
                showChart: false
            }
        };

        this.view = new DataFilterView();

        const context = this.view.getContext();
        context.addEventListener('ready', () => {
            const model = context.getModel();
            const query = context.getQuery();
         
            // here you can specify wich entities 
            // and their attributes
            // will be used for select
            model.rootEntity.scan(null,
                (entity) => {
                    if (entity.name == "Order") {
                        for(const attr of entity.attributes) {
                            query.addColumn({
                                attribute: attr
                            });
                        }
                    }
                }
            );
        });

        context
            .useEndpoint('api/data-filtering')
            .useEnterprise(() => {
                this.view.init(this.options);
            });

    }

    render() {
        return (
            <div>
                <div id="FilterBar"></div>
                <div id="ResultPanel"></div>
            </div>
        )
    }
}