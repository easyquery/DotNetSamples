import '@babel/polyfill';

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
import '@easydata/ui/dist/assets/css/easy-forms.css';

import Vue from 'vue';
import './plugins/axios';
import './plugins/vuetify';
import App from './App.vue';
import router from './router';
import store from '@/store/index';
import './registerServiceWorker';

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
