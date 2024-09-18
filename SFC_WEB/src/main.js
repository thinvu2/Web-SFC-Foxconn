import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import SmoothScrollbar from 'vue-smooth-scrollbar'
import Dropdown from 'vue-simple-search-dropdown';
// import 'bootstrap/dist/css/bootstrap.min.css'
// import 'bootstrap-vue/dist/bootstrap-vue.css'
// import '../public/template/vendors/jquery/dist/jquery.min.js'
// import '../public/template/build/js/custom.min.js'

import { library } from "@fortawesome/fontawesome-svg-core";
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import loader from "vue-ui-preloader";
import DatePick from 'vue-date-pick';
import 'vue-date-pick/dist/vueDatePick.css';
import Datepicker from 'vue3-datepicker'
import 'vue-multiselect-listbox/dist/vue-multi-select-listbox.css';
import VueDraggable from 'vue-draggable'
import DragSelect from 'vue-drag-select/src/DragSelect.vue'
//import VueExcelXlsx from "vue-excel-xlsx";
import VueApexCharts from "vue3-apexcharts";
import contextmenu from "v-contextmenu";
import "v-contextmenu/dist/themes/default.css";


import {faChevronLeft, faChevronDown, faMapMarkedAlt, faCircle, faDotCircle, faChevronRight, faCheck, faListAlt, faUserShield, faSearchPlus, faSearch, faGift, faSyringe, faSimCard, faDolly, faBacteria, faCalendarAlt, faBoxOpen, faPallet, faRoute, faChartPie, faLock, faTools, faList, faHistory, faPlus, faKey, faEdit, faTrashAlt, faExchangeAlt, faAngleLeft, faAngleRight, faAngleDoubleRight, faAngleDoubleLeft, faClipboardList, faThumbtack, faFileExcel, faDatabase, faChartLine, faUserSecret, faUpload, faUserCog, faCogs, faAdjust, faTimesCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(faChevronLeft, faChevronDown, faMapMarkedAlt, faCircle, faDotCircle, faChevronRight, faCheck, faListAlt, faSearchPlus, faUserShield, faSearch, faGift, faSyringe, faSimCard, faDolly, faBacteria, faCalendarAlt, faBoxOpen, faPallet, faRoute, faList, faChartPie, faClipboardList, faTools, faLock, faPlus, faEdit, faKey, faTrashAlt, faHistory, faExchangeAlt, faAngleLeft, faAngleRight, faAngleDoubleRight, faAngleDoubleLeft, faThumbtack, faFileExcel, faDatabase, faChartLine, faUserSecret, faUpload, faUserCog, faCogs, faAdjust, faTimesCircle);

createApp(App)
    .component("Icon", FontAwesomeIcon)
    .component('drag-select-container', DragSelect)         
    .component('datepicker', Datepicker)
    .component('date-pick', DatePick)
    .use(loader)
    .use(VueSweetalert2)
    .use(VueDraggable)    
    //.use(VueExcelXlsx)
    .use(contextmenu)
    .use(SmoothScrollbar)
    .use(VueApexCharts)
    .use(Dropdown)
    .use(store)
    .use(router)
    .mount('#app')