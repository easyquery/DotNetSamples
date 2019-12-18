import React, { Component } from 'react';

import { AdvancedSearchView } from '@easyquery/ui';
import '@easyquery/enterprise'

import  AdvancedSearchHtml  from './EasyQueryHtml';

export class EasyQuery extends Component {
    static displayName = EasyQuery.name;

    QUERY_KEY = 'easyquerycomponent-query';

    view = new AdvancedSearchView();

    componentDidMount() {
        const options = {
            enableExport: true,
            loadModelOnStart: true,
            loadQueryOnStart: false,
            defaultQueryId: "test-query",
            defaultModelId: "NWindSQL",

            //Middlewares endpoint
            endpoint: '/api/easyquery',

            handlers: {
              onError: (error) => {
                console.error(error.type + " error:\n" + error.text);
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
                alwaysShowButtonsInPredicates: false,
                allowParameterization: true,
                allowInJoinConditions: true,
                autoEditNewCondition: true,
                buttons: {
                    condition: ["menu"],
                    predicate: ["addCondition", "addPredicate", "enable", "delete"]
                },
                menuOptions: {
                    showSearchBoxAfter: 20,
                    activateOnMouseOver: true
                }
              }
            },
            result: {
              showChart: true
            }
        }

        this.view.getContext().addEventListener('ready', () => {
            //here we need to add query autosave
            let query = this.view.getContext().getQuery();

            query.addChangedCallback(() => {
                let queryJson = query.toJSON();
                localStorage.setItem(this.QUERY_KEY, queryJson);
                //console.log("Query saved", query);
            });

            //add load query from local storage
            this.loadQueryFromLocalStorage();
        });

        this.view.getContext().useEnterprise("AlzWbvUgrkISH9AEAEoV7wBKJXGX14");


        this.view.init(options);

    }

    loadQueryFromLocalStorage() {
        const queryJson = localStorage.getItem(this.QUERY_KEY);
        if (queryJson) {
            const query = this.view.getContext().getQuery();
            query.loadFromDataOrJson(queryJson);
            query.fireChangedEvent();
            setTimeout(() => this.view.executeQuery(), 100);
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
