import { Component, AfterViewInit} from '@angular/core';

import { EqContext, Condition } from '@easyquery/core';
import { EqViewOptions, AdvancedSearchView } from '@easyquery/ui';

import '@easyquery/enterprise'

import { CustomExpressionRenderer } from './custom_expression_renderer'

@Component({
    selector: 'easyquery',
    templateUrl: './easyquery.component.html'
})

export class EasyQueryComponent implements AfterViewInit {

    private QUERY_KEY = 'easyquerycomponent-query';

    private context: EqContext;

    private view: AdvancedSearchView;

    constructor() {
      
    }
   

    ngAfterViewInit() {

      //here we define some options for AdvancedSearchView
      const viewOptions: EqViewOptions = {
        enableExport: true,
        loadModelOnStart: true,
        loadQueryOnStart: false,

        //EasyQuery endpoint
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
                showSearchBoxAfter: 2,
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
                group: ["addCondition", "addGroup", "enable", "delete"]
            },
            menuOptions: {
                showSearchBoxAfter: 20,
                activateOnMouseOver: true
            },
            //the following piece demonstrates how to set a custom expression renderer
            //for some value editors (for ContextNameEditor in this particular example)
            onGetExpressionRenderer: (queryPanel, expression, valueEditor, slot) => {
              const condition = expression.getParent() as Condition;
              const model = condition.getQuery().getModel();
              const attrId = condition.expressions[0].value;
              const attr = model.getAttributeById(attrId);

              if (attr.defaultEditor.id === "ContactNameEditor") {
                return new CustomExpressionRenderer(queryPanel, expression, attr.defaultEditor, slot);
              }

              return null;
            }
          },
          resultGrid: {
            autoHeight: true
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

    private loadQueryFromLocalStorage() {
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
}
