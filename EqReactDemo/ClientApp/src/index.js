import 'bootstrap/dist/css/bootstrap.css';

import '@easyquery/ui/dist/assets/css/easyquery.css';
import '@easyquery/ui/dist/assets/css/eq-icons-default.css';
import '@easyquery/ui/dist/assets/css/easyquery-mobile.css';
import '@easyquery/ui/dist/assets/css/eqview.css';
import '@easyquery/ui/dist/assets/css/easychart.css';
import '@easyquery/ui/dist/assets/css/eq-grid.css';
import '@easyquery/ui/dist/assets/css/eq-grid-mobile.css';
import '@easyquery/ui/dist/assets/css/easyfacets.css';
import '@easydata/ui/dist/assets/css/easy-grid.css';
import '@easydata/ui/dist/assets/css/easy-dialog.css';
import '@easydata/ui/dist/assets/css/forms.css';
import '@easydata/ui/dist/assets/css/controls.css';
import '@easydata/ui/dist/assets/css/columns.css';
import '@easydata/ui/dist/assets/css/buttons.css';

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();
