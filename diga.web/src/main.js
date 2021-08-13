import Vue from "vue";
import router from "./router";
import store from "./store";

import VueI18n from "vue-i18n";
Vue.use(VueI18n);

import SignalR from "@latelier/vue-signalr";
Vue.use(SignalR, window.location.origin + "/hub");

import axios from "axios";

import Vuelidate from "vuelidate";
Vue.use(Vuelidate);

import Clipboard from 'v-clipboard' 
Vue.use(Clipboard);

import Toasted from "vue-toasted";
Vue.use(Toasted, {
    position: "top-right",
    duration: 3000,
    keepOnHover: true,
    closeOnSwipe: true,
});

import { BootstrapVue, IconsPlugin } from "bootstrap-vue";
Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
// multiselect
import Multiselect from "vue-multiselect";
Vue.component("multiselect", Multiselect);

import "@/assets/platform.css";

Vue.config.productionTip = false;

export const i18n = new VueI18n({
    locale: "en",
    fallbackLocale: "en",
});

const loadedLanguages = [];

function setI18nLanguage(lang) {
    i18n.locale = lang;
    axios.defaults.headers.common["Accept-Language"] = lang;
    document.querySelector("html").setAttribute("lang", lang);
    return lang;
}

const moment = require("moment");
require("moment/locale/es");
require("moment/locale/ru");
require("moment/locale/uk");
require("moment/locale/pt");

Vue.use(require("vue-moment"), {
    moment,
});

import $ from "jquery";
export function loadLanguage(lang) {
    if (loadedLanguages.includes(lang)) {
        if (i18n.locale !== lang) {
            setI18nLanguage(lang);
        }
    }
    $.ajax({
        async: false,
        url: `/api/translation/lang/${lang}`,
        type: "GET",
        success: function(response) {
            let msgs = response;
            loadedLanguages.push(lang);
            i18n.setLocaleMessage(lang, msgs);
            setI18nLanguage(lang);
            moment.locale(lang);
        }.bind(this),
    });
}

var locale = window.localStorage.getItem("language") || "en";
loadLanguage(locale);

// Vue.component("personal_area_index", require("./Index.vue").default);

window.Vue = Vue;
new Vue({
    i18n,
    router,
    store,
    data() {
        return {
            roles: [],
            showSideBar: false,
            locale: window.localStorage.getItem("language") || "en",
        };
    },
    methods: {
        changeLang(lang) {
            this.locale = lang;
            loadLanguage(lang);
            window.localStorage.setItem("language", lang);
        },
    },
    computed: {},
}).$mount("#app");

import upperFirst from "lodash/upperFirst";
import camelCase from "lodash/camelCase";
const requireComponent = require.context(
    "./components",
    false,
    /Base[A-Z]\w+\.(vue|js)$/
);

requireComponent.keys().forEach((fileName) => {
    const componentConfig = requireComponent(fileName);

    const componentName = upperFirst(
        camelCase(fileName.replace(/^\.\/(.*)\.\w+$/, "$1"))
    );

    Vue.component(componentName, componentConfig.default || componentConfig);
});

export default i18n;