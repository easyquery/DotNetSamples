import { Component, AfterViewInit, OnInit, ViewChild} from '@angular/core';

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

  public pageSize: number = 0;
  public columns: any[] = [];
  public gridData: any;
  public skip: number = 0;

  constructor() {
    super(document.createElement('div'));
  }

    protected render() {
      this.context.resultSet.setDisplayFormats();
      this.pageSize = this.context.paging.pageSize,
      this.skip =  this.pageSize * (this.context.paging.pageIndex - 1);
      this.columns = this.createColumnDefs();
      this.gridData = this.createRowData();
  }

  // specify the columns
  private createColumnDefs() {
      const columns = [];

      const resultSet = this.context.resultSet;
      for(let i = 0; i < resultSet.getNumberOfColumns(); i++) {
          columns.push({title: resultSet.getColumnLabel(i), field: resultSet.getColumnProperties(i).field});
      }

      return columns;
  }

  // specify the data
  private createRowData() {
      const rows = [];

      const resultSet = this.context.resultSet;
      for(let i = 0; i < resultSet.getNumberOfRows(); i++) {

          const row: any = {};

          for(let j = 0; j < resultSet.getNumberOfColumns(); j++) {
             row[resultSet.getColumnProperties(j).field] = resultSet.getFormattedValue(i, j);
          }
         
          rows.push(row);
      }
      
      return {
          data: rows,
          total: this.context.paging.totalRecords
      };
  }

  protected clear() {
      this.pageSize = 0;
      this.columns = [];
      this.gridData = [];
  }

  public pageChange(event: PageChangeEvent) {
    const pageNum = event.skip / this.pageSize + 1;
    if (pageNum !== this.context.paging.pageIndex) {
        this.context.paging.pageSelectedCallback(pageNum, () => {
            this.context.resultSet.setDisplayFormats();
            this.gridData = this.createRowData();
            this.skip =  this.pageSize * (this.context.paging.pageIndex - 1);
        }, false);
    }
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
          enableExport: true,
          loadModelOnStart: true,
          loadQueryOnStart: false,

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
            showChart: true,
            resultGridResolver: (slot) => {
              return this.grid;
            }
          }
        }
  
        this.view = new AdvancedSearchView();
        this.context = this.view.getContext();

        this.context.useEnterprise("AlzWbvUgrkISH9AEAEoV7wBKJXGX14");
  
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
        
        this.view.init(options);
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
