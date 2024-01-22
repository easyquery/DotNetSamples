import '@easyquery/ui/dist/assets/css/easyquery.css';
import '@easyquery/ui/dist/assets/css/eq-icons-default.css';
import '@easyquery/ui/dist/assets/css/easyquery-mobile.css';
import '@easyquery/ui/dist/assets/css/eqview.css';
import '@easyquery/ui/dist/assets/css/easychart.css';
import '@easyquery/ui/dist/assets/css/easyfacets.css';
import '@easydata/ui/dist/assets/css/easy-grid.css';
import '@easydata/ui/dist/assets/css/easy-dialog.css';
import '@easydata/ui/dist/assets/css/easy-forms.css';

import './assets/main.less'

import { createApp } from 'vue'

import App from './App.vue'
import vuetify from "@/plugins/vuetify";

const app = createApp(App)

app.use(vuetify)

app.mount('#app')
