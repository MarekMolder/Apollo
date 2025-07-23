import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from '@/router';
import App from './App.vue';

import { i18n } from './i18n';
import { switchLanguage } from './Helpers/i18nHelper';

const app = createApp(App);
app.use(createPinia());
app.use(router);
app.use(i18n);

switchLanguage('en').then(() => {
  app.mount('#app');
});
