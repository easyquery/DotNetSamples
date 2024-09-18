import '@easyquery/ui/dist/assets/css/easyquery.ui.css';

import './assets/main.less'

import { createApp } from 'vue'

import App from './App.vue'
import vuetify from "@/plugins/vuetify";

const app = createApp(App)

app.use(vuetify)

app.mount('#app')
