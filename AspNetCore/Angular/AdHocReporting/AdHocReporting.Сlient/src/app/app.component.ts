import { HttpClient } from '@angular/common/http';
import { Component, Inject, AfterViewInit } from '@angular/core';

import { EqContext } from '@easyquery/core';
import { EqViewOptions, AdvancedSearchView } from '@easyquery/ui';

import '@easyquery/enterprise';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent implements AfterViewInit {
  private QUERY_KEY = 'easyquerycomponent-query';

  private context: EqContext;

  private view: AdvancedSearchView;

  constructor(@Inject('BASE_URL') baseUrl: string) {
    console.log(baseUrl);
    this.view = new AdvancedSearchView();
    this.context = this.view.getContext();
  }

  ngAfterViewInit() {
    //here we define some options for AdvancedSearchView
    const viewOptions: EqViewOptions = {
      enableExport: true,
      serverExporters: ['pdf', 'excel', 'csv'],
      loadModelOnStart: true,
      loadQueryOnStart: false,
      handlers: {
        onError: (_, error) => {
          console.error(error.sourceError);
        }
      },
      result: {
        showChart: true
      }
    }

    this.context
      .useEndpoint('/api/adhoc-reporting')
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

      setTimeout(() => this.view.fetchData(), 100);
    }
  };    

  title = 'AdHocReporting Demo';
}
