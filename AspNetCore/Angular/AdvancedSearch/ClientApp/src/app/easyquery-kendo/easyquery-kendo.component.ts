import { Component, AfterViewInit, OnInit, ViewChild} from '@angular/core';

import { DataRow } from '@easydata/core';
import { EqContext } from '@easyquery/core';
import { EqViewOptions, AdvancedSearchView, Grid } from '@easyquery/ui';

import '@easyquery/enterprise';

import { PageChangeEvent } from '@progress/kendo-angular-grid';
@Component({
  selector: 'eq-kendo-grid',
  template: `<kendo-grid
      [data]="gridData"
      [pageSize]="pageSize"
      [pageable]="true"
      [style.height.%]="100"
      [skip] = "skip"
      (pageChange)="pageChange($event)"
      >
        <kendo-grid-column *ngFor="let col of columns" [field]="col.field" [title]="col.field"></kendo-grid-column>
    </kendo-grid>`
})
export class EqKendoGridComponent extends Grid  {

  public pageSize = 15;
  public columns: any[] = [];
  public gridData: any  = { data: [], total: 0 };
  public skip = 0;

  constructor() {
    super(document.createElement('div'));
  }

  protected render() {
    this.applyDisplayFormats();
    this.columns = this.getColumnsDefs();

    this.pageChange({
      skip: this.skip,
      take: this.pageSize
    });
  }

  // specify the columns
  private getColumnsDefs() {
    const columns = [];

    const resultTable = this.context.resultTable;
    for (const col of resultTable.columns.getItems()) {
      columns.push({ headerName: col.label, field: this.toValidFieldName(col.id) });
    }

    return columns;
  }

  // specify the data
  private convertToGridData(rows: DataRow[]) {
    const result = [];

    const resultTable = this.context.resultTable;
    for (const row of rows) {
      const dataRow: any = {};
      for (let i = 0; i < resultTable.columns.count; i++) {
        const col = resultTable.columns.get(i);
        dataRow[this.toValidFieldName(col.id)] = row.getValue(i);
      }

      result.push(dataRow);
    }

    return {
      data: result,
      total: this.context.resultTable.getTotal()
    };
  }

  protected clear() {
      this.columns = [];
      this.gridData = [];
  }

  private toValidFieldName(field: string) {
    return field.replace(' ', '_');
  }

  public pageChange(event: PageChangeEvent) {
    this.context.resultTable.getRows({
      offset: event.skip,
      limit: event.take
    })
    .then(rows => {
      this.applyDisplayFormats();
      this.gridData = this.convertToGridData(rows);
      this.skip = event.skip;
    });
  }

}


@Component({
    selector: 'easyquery-kendo',
    templateUrl: './easyquery-kendo.component.html'
})
export class EasyQueryKendoComponent implements AfterViewInit {

    private QUERY_KEY = 'easyquerycomponent-query';

    protected context: EqContext;

    private view: AdvancedSearchView;

    @ViewChild(EqKendoGridComponent) 
    grid: EqKendoGridComponent;

    constructor() {
      
    }
   

    ngAfterViewInit() {

        const options: EqViewOptions = {

          loadModelOnStart: true,
          loadQueryOnStart: false,

          //Middlewares endpoint
          endpoint: '/api/easyquery',
  
          handlers: {
            onError: (context, error) => {
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
                  group: ["addCondition", "addGroup", "enable", "delete"]
              },
              menuOptions: {
                  showSearchBoxAfter: 20,
                  activateOnMouseOver: true
              }
            }
          },
          result: {
            showChart: true,
            resultGridResolver: (slot) => {
              return this.grid;
            }
          }
        }
  
        this.view = new AdvancedSearchView();
        this.context = this.view.getContext();

        this.context.addEventListener('ready', () => {
          const query = this.context.getQuery();

          query.addChangedCallback(() => {
            const queryJson = query.toJSON();
            localStorage.setItem(this.QUERY_KEY, queryJson);
            console.log("Query saved", query);
          });

          //add load query from local storage
          this.loadQueryFromLocalStorage();
        });

        this.context.useEnterprise(() => {
          this.view.init(options);
        });
  
      
        
       }  
  
      private loadQueryFromLocalStorage() {
          const queryJson = localStorage.getItem(this.QUERY_KEY);
          if (queryJson) {
            const query = this.context.getQuery();
            query.loadFromDataOrJson(queryJson);
            query.fireChangedEvent();
            setTimeout(() => this.view.executeQuery(), 100);
          }
      };

    
}
