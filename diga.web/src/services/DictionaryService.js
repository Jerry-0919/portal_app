import apiClient from "./MainService.js";

export default {
    getCountries() {
        return apiClient.get("platform/countries");
    },
    getCities() {
        return apiClient.get("platform/cities");
    },
    getContractPriorities() {
        return apiClient.get("platform/contracts/priorities");
    },
    getContractTypes() {
        return apiClient.get("platform/contracts/types");
    },
    getContractBudgets() {
        return apiClient.get("platform/balances");
    },
    getCategories() {
        return apiClient.get("platform/categories");
    },
    getPublishedCategories() {
        return apiClient.get("platform/categories/published");
    },
    getTags() {
        return apiClient.get("platform/tags");
    },
    getLanguages() {
        return apiClient.get("platform/languages");
    },
    getVats() {
        return apiClient.get("platform/vats");
    },
};