import Axios from "axios";
import Vue from "vue";
import Vuex from "vuex";
// store.js
import i18n from "@/main.js";

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        isAdmin: false,
        currentRole: "Customer",
        isDictionariesLoaded: false,
        loading: false,
        user: null,
        countries: [],
        contractPriorities: [],
        contractTypes: [],
        cities: [],
        budgets: [],
        categories: [],
        languages: [],
        tags: [],
        vats: [],
        estimates: [],
        measurementReports: [],
    },
    mutations: {
        CLEAN_MR(state, data) {
            let report = state.measurementReports.find((e) => e.id === data.id);
            if (report !== null) {
                let index = state.measurementReports.indexOf(report);
                state.measurementReports.splice(index, 1);
            }
        },
        PUSH_MR_POSITION_TO_UPDATE(state, data) {
            let report = state.measurementReports.find((e) => e.id === data.id);
            if (report == null) {
                report = {
                    id: data.id,
                    positionsToUpdate: []
                };
                state.measurementReports.push(report);
            }
            let position = report.positionsToUpdate.find((e) => e.id === data.position.id);
            if (position == null) {
                report.positionsToUpdate.push(data.position);
            } else {
                position.quantityCompleted = data.position.quantityCompleted;
                position.status = data.position.status;
                position.rejectionReason = data.position.rejectionReason;
            }
        },
        CLEAN_ESTIMATE(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate !== null) {
                let index = state.estimates.indexOf(estimate);
                state.estimates.splice(index, 1);
            }
        },
        PUSH_POSITION_TO_ADD(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate == null) {
                estimate = {
                    id: data.estimateId,
                    positionsToAdd: [],
                    positionsToUpdate: [],
                    positionIdsToRemove: [],
                    sectionsToAdd: [],
                    sectionsToUpdate: [],
                    sectionIdsToRemove: [],
                };
                state.estimates.push(estimate);
            }
            estimate.positionsToAdd.push(data.position);
        },
        PUSH_POSITION_TO_UPDATE(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate == null) {
                estimate = {
                    id: data.estimateId,
                    positionsToAdd: [],
                    positionsToUpdate: [],
                    positionIdsToRemove: [],
                    sectionsToAdd: [],
                    sectionsToUpdate: [],
                    sectionIdsToRemove: [],
                };
                state.estimates.push(estimate);
            }
            estimate.positionsToUpdate.push(data.position);
        },
        PUSH_POSITION_TO_REMOVE(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate == null) {
                estimate = {
                    id: data.estimateId,
                    positionsToAdd: [],
                    positionsToUpdate: [],
                    positionIdsToRemove: [],
                    sectionsToAdd: [],
                    sectionsToUpdate: [],
                    sectionIdsToRemove: [],
                };
                state.estimates.push(estimate);
            }
            if (data.id > 0) {
                estimate.positionIdsToRemove.push(data.id);
            } else {
                const index = estimate.positionsToAdd.indexOf(data.item);
                if (index > -1) {
                    estimate.positionsToAdd.splice(index, 1);
                }
            }
        },
        PUSH_SECTION_TO_ADD(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate == null) {
                estimate = {
                    id: data.estimateId,
                    positionsToAdd: [],
                    positionsToUpdate: [],
                    positionIdsToRemove: [],
                    sectionsToAdd: [],
                    sectionsToUpdate: [],
                    sectionIdsToRemove: [],
                };
                state.estimates.push(estimate);
            }
            estimate.sectionsToAdd.push(data.section);
        },
        PUSH_SECTION_TO_UPDATE(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate == null) {
                estimate = {
                    id: data.estimateId,
                    positionsToAdd: [],
                    positionsToUpdate: [],
                    positionIdsToRemove: [],
                    sectionsToAdd: [],
                    sectionsToUpdate: [],
                    sectionIdsToRemove: [],
                };
                state.estimates.push(estimate);
            }
            estimate.sectionsToUpdate.push(data.section);
        },
        PUSH_SECTION_TO_REMOVE(state, data) {
            let estimate = state.estimates.find((e) => e.id === data.estimateId);
            if (estimate == null) {
                estimate = {
                    id: data.estimateId,
                    positionsToAdd: [],
                    positionsToUpdate: [],
                    positionIdsToRemove: [],
                    sectionsToAdd: [],
                    sectionsToUpdate: [],
                    sectionIdsToRemove: [],
                };
                state.estimates.push(estimate);
            }
            if (data.id > 0) {
                estimate.sectionIdsToRemove.push(data.id);
            } else {
                const index = estimate.sectionsToAdd.indexOf(data.item);
                if (index > -1) {
                    estimate.sectionsToAdd.splice(index, 1);
                }
            }
        },
        PUSH_ADMIN(state) {
            state.isAdmin = true;
        },
        PUSH_ROLE(state, role) {
            state.currentRole = role;
        },
        IS_LOADING_TRUE(state) {
            state.loading = true;
        },
        IS_LOADING_FALSE(state) {
            state.loading = false;
        },
        PUSH_USER(state, new_user) {
            state.user = new_user;
        },
        LOGOUT(state) {
            state.user = null;
        },
        PUSH_COUNTRIES(state, countries) {
            state.countries = countries;
        },
        PUSH_CITIES(state, cities) {
            state.cities = cities;
        },
        PUSH_PRIORITIES(state, priorities) {
            state.contractPriorities = priorities;
        },
        PUSH_TYPES(state, types) {
            state.contractTypes = types;
        },
        PUSH_BUDGETS(state, budgets) {
            state.budgets = budgets;
            state.isDictionariesLoaded = true;
        },
        PUSH_CATEGORIES(state, categories) {
            state.categories = categories;
        },
        PUSH_TAGS(state, tags) {
            state.tags = tags;
        },
        PUSH_LANGUAGES(state, languages) {
            state.languages = languages;
            state.isDictionariesLoaded = true;
        },
        PUSH_VATS(state, vats) {
            state.vats = vats;
        },
    },
    actions: {
        getVats({ commit }) {
            Axios.get("/api/platform/vats")
                .then((response) => {
                    commit("PUSH_VATS", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getLanguages({ commit }) {
            Axios.get("/api/platform/languages")
                .then((response) => {
                    commit("PUSH_LANGUAGES", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getCountries({ commit }) {
            Axios.get("/api/platform/countries")
                .then((response) => {
                    commit("PUSH_COUNTRIES", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getCategories({ commit }) {
            Axios.get("/api/platform/categories")
                .then((response) => {
                    let rawCategories = response.data;
                    rawCategories.forEach((c) => {
                        c.nameId = i18n.t(c.nameId);
                        c.platformCategories.forEach((pc) => {
                            pc.nameId = i18n.t(pc.nameId);
                        });
                    });
                    commit("PUSH_CATEGORIES", rawCategories);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getTags({ commit }) {
            Axios.get("/api/platform/tags")
                .then((response) => {
                    let rawTags = response.data;
                    let tags = rawTags.map((t) => {
                        return t.name;
                    });
                    commit("PUSH_TAGS", tags);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getCities({ commit }) {
            Axios.get("/api/platform/cities")
                .then((response) => {
                    commit("PUSH_CITIES", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getContractPriorities({ commit }) {
            Axios.get("/api/platform/contracts/priorities")
                .then((response) => {
                    commit("PUSH_PRIORITIES", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getContractTypes({ commit }) {
            Axios.get("/api/platform/contracts/types")
                .then((response) => {
                    commit("PUSH_TYPES", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
        getContractBudgets({ commit }) {
            Axios.get("/api/platform/balances")
                .then((response) => {
                    commit("PUSH_BUDGETS", response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
    },
    getters: {
        getVatById: (state) => (id) => {
            return state.vats.find((vat) => vat.id === id);
        },
        getCountryById: (state) => (id) => {
            return state.countries.find((country) => country.id === id);
        },
        getCityById: (state) => (id) => {
            return state.cities.find((city) => city.id === id);
        },
        getContractPriorityById: (state) => (id) => {
            return state.contractPriorities.find((cp) => cp.id === id);
        },
        getContractTypeById: (state) => (id) => {
            return state.contractTypes.find((ctype) => ctype.id === id);
        },
        getBudgetById: (state) => (id) => {
            return state.budgets.find((ctype) => ctype.id === id);
        },
        getLanguageById: (state) => (id) => {
            return state.languages.find((language) => language.id === id);
        },
    },
});