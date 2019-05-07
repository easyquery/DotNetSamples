import React, { Component } from 'react';

import { AdvancedSearchViewJQuery } from '@easyquery/ui-jquery';
import  AdvancedSearchHtml  from './EasyQueryHtml';

export class EasyQuery extends Component {
    static displayName = EasyQuery.name;

    view = new AdvancedSearchViewJQuery();

    componentDidMount() {
        const options = {
            enableExport: true,
            loadModelOnStart: true,
            loadQueryOnStart: true,
            defaultQueryId: "test-query",
            defaultModelId: "NWindSQL",
            handlers: {
              onError: (error) => {
                console.error(error.type + " error:\n" + error.text);
              },
              listRequestHandler: (params, onResult) => {
                let processed = true;
                if (params.listName == "RegionList") {
                    let query = this.context.getQuery();
                    let country = query.getOneValueForAttr("Customer.Country");
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

        options.handlers.onInit = () => {
            //here we need to add query autosave
            let query = this.view.getContext().getQuery();
  
            query.addChangedCallback(() => {
                let queryJson = query.toJSON();
                localStorage.setItem(this.QUERY_KEY, queryJson);
                console.log("Query saved", query);
            });
  
            //add load query from local storage
            this.loadQueryFromLocalStorage();
        }

        this.view.init(options);

    }

    loadQueryFromLocalStorage() {
        let queryJson = localStorage.getItem(this.QUERY_KEY);
        if (queryJson) {
            let query = this.view.getContext().getQuery();
            query.setData(queryJson);
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
