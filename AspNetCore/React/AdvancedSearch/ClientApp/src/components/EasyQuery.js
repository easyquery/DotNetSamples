import React, { Component } from 'react';

import { AdvancedSearchView } from '@easyquery/ui';
import '@easyquery/enterprise';

import  AdvancedSearchHtml  from './EasyQueryHtml';

export class EasyQuery extends Component {
    static displayName = EasyQuery.name;

    QUERY_KEY = 'easyquerycomponent-query';

    view = new AdvancedSearchView();

    componentDidMount() {
        const viewOptions = {
            enableExport: true,
            serverExporters: ['pdf', 'excel', 'csv'],
            loadModelOnStart: true,
            loadQueryOnStart: false,

            handlers: {
              onError: (_, error) => {
                console.error(error.sourceError);
              }
            },
            widgets: {
              easyGrid: {
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

        this.context
            .useEndpoint('/api/easyquery')
            .useEnterprise(() => {
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

            setTimeout(() => this.view.fetchData(), 100);
        }
    };    

    render() {
        return (
            <AdvancedSearchHtml />
        );
    }

    shouldComponentUpdate() {
        return false;
    }
}
