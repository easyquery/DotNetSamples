import { Component, Element, h } from '@stencil/core';
import { AdvacnedSearchHtml } from './easy-query-html';

import { EqContext } from '@easyquery/core';
import { AdvancedSearchView, EqViewOptions } from '@easyquery/ui';
import '@easyquery/enterprise';

@Component({
    tag: 'easy-query',
    styleUrl: './easy-query.css',
    shadow: true
})
export class EasyQuery {

    private view: AdvancedSearchView;
    private context: EqContext;

    @Element() host: HTMLDivElement;

    QUERY_KEY = 'easyquerycomponent-query';
    
    componentWillLoad() {
        const viewOptions: EqViewOptions = {
            shadowRoots: [ this.host.shadowRoot ],
            enableExport: true,
            loadModelOnStart: true,
            loadQueryOnStart: false,
            defaultQueryId: "test-query",
            defaultModelId: "NWindSQL",

            //Middlewares endpoint
            endpoint: '/api/easyquery',

            handlers: {
              onError: (error) => {
                console.error(error.action + " error:\n" + error.text);
              }
            },
            widgets: {
              entitiesPanel: {
                showCheckboxes: true
              },
              columnsPanel: {
                allowAggrColumns: true,
                allowCustomExpressions: true,
                attrElementFormat: "{entity} {attr}",
                titleElementFormat: "{attr}",
                showColumnCaptions: true,
                adjustEntitiesMenuHeight: false,
                customExpressionText: 2,
                showPoweredBy: false,
                menuOptions: {
                    showSearchBoxAfter: 30,
                    activateOnMouseOver: true
                }
              },
              queryPanel: {
                showPoweredBy: false,
                alwaysShowButtonsInGroups: false,
                allowParameterization: true,
                allowInJoinConditions: true,
                autoEditNewCondition: true,
                buttons: {
                    condition: ["menu"],
                    group: ["addCondition", "addPredicate", "enable", "delete"]
                },
                menuOptions: {
                    showSearchBoxAfter: 20,
                    activateOnMouseOver: true
                }
              },
              resultGrid: {
                  autoHeight: true,
                  paging: {
                      enabled: true,
                      pageSize: 30
                  }
              }
            },
            result: {
              showChart: true
            }
        }

        this.view = new AdvancedSearchView();
        this.context = this.view.getContext();

        this.context.useEnterprise(() => {
            this.view.init(viewOptions);
        });

        this.context.addEventListener('ready', () => {
            const query = this.context.getQuery();

            query.addChangedCallback(() => {
                const data = JSON.stringify({
                    modified: query.isModified(),
                    query: query.toJSONData()
                });
                localStorage.setItem(this.QUERY_KEY, data);
            });

            //add load query from local storage
            this.loadQueryFromLocalStorage();
        });
    }

    loadQueryFromLocalStorage() {
        const dataJson = localStorage.getItem(this.QUERY_KEY);
        if (dataJson) {
            const data = JSON.parse(dataJson);
            const query = this.context.getQuery();
            query.loadFromDataOrJson(data.query);
            if (data.modified) {
                query.fireChangedEvent();
            }
            else {
                this.view.getContext().refreshWidgets();
                this.view.syncQuery();
            }

            setTimeout(() => this.view.executeQuery(), 100);
        }
    };    


    render() {
        return (<AdvacnedSearchHtml />)
    }
}