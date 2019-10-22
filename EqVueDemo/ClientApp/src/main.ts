import '@babel/polyfill';

import '@easyquery/ui/dist/assets/css/easyquery.css';
import '@easyquery/ui/dist/assets/css/eq-icons-default.css';
import '@easyquery/ui/dist/assets/css/eqview.css';

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
