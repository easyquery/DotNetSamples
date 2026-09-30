import { Component, State, h, Build } from '@stencil/core';

interface Forecast {
    dateFormatted?: string; 
    temperatureC?: number;
    temperatureF?: number;
    summary?: string;
}

@Component({
    tag: 'fetch-data'
})
export class FetchData {
    static displayName = FetchData.name;

    @State() loading: boolean;

    @State() forecasts: Forecast[];

    constructor () {

      this.loading = true;
      this.forecasts = [];

    }

    componentWillLoad() {
        // prerender runs in Node, where the EasyQuery client cannot reach the API
        if (!Build.isBrowser) return;
        // fetch('api/SampleData/WeatherForecasts')
        // .then(response => response.json())
        // .then(data => {
        //     this.loading = false;
        //     this.forecasts = data;
        // });
    }
  
    static renderForecastsTable (forecasts: Forecast[]) {
      return (
        <table class='table table-striped'>
          <thead>
            <tr>
              <th>Date</th>
              <th>Temp. (C)</th>
              <th>Temp. (F)</th>
              <th>Summary</th>
            </tr>
          </thead>
          <tbody>
            {forecasts.map(forecast =>
              <tr key={forecast.dateFormatted}>
                <td>{forecast.dateFormatted}</td>
                <td>{forecast.temperatureC}</td>
                <td>{forecast.temperatureF}</td>
                <td>{forecast.summary}</td>
              </tr>
            )}
          </tbody>
        </table>
      );
    }
  
    render () {
      let contents = this.loading
        ? <p><em>Loading...</em></p>
        : FetchData.renderForecastsTable(this.forecasts);
  
      return (
        <div>
          <h1>Weather forecast</h1>
          <p>This component demonstrates fetching data from the server.</p>
          {contents}
        </div>
      );
    }
}