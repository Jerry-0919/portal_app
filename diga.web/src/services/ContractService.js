import apiClient from "./MainService.js";

export default {
    getContracts(skip, take, filter, statuses, isActual) {
        let url = "platform/contracts/" + skip + "/" + take + "?filter=" + filter;
        if (statuses.length > 0) {
            url += "&statuses=" + statuses;
        }
        if (isActual) {
            url += "&isActual=" + isActual;
        }
        return apiClient.get(url);
    },
    getVersions(contractId, skip, take) {
        return apiClient.get(
            "platform/contracts/" + contractId + "/versions/" + skip + "/" + take
        );
    },
    getPublishedContracts(skip, take, filters, categories, tags) {
        var filterStr = new URLSearchParams(filters).toString();
        var url =
            "platform/contracts/published/" + skip + "/" + take + "?" + filterStr;
        if (categories && categories.length > 0) {
            url += "&categories=" + categories;
        }
        if (tags && tags.length > 0) {
            url += "&tags=" + tags;
        }
        return apiClient.get(url);
    },
    createContract() {
        return apiClient.post("platform/contracts");
    },
    getContract(contractId) {
        return apiClient.get("platform/contracts/" + contractId);
    },
    saveBasicDetails(contractId, data) {
        return apiClient.patch("platform/contracts/" + contractId + "/info", data);
    },
    saveEstimate(contractId, data) {
        return apiClient.patch(
            "platform/contracts/" + contractId + "/estimate",
            data
        );
    },
    savePrice(contractId, data) {
        return apiClient.patch("platform/contracts/" + contractId + "/price", data);
    },
    saveDates(contractId, data) {
        return apiClient.patch("platform/contracts/" + contractId + "/dates", data);
    },
    Publicate(contractId) {
        return apiClient.patch("platform/contracts/" + contractId + "/complete");
    },
    Remove(contractId) {
        return apiClient.delete("platform/contracts/" + contractId);
    },
    Archive(contractId) {
        return apiClient.patch("platform/contracts/" + contractId + "/archive");
    },
    Extend(contractId, data) {
        return apiClient.patch(
            "platform/contracts/" + contractId + "/tenderend",
            data
        );
    },
    getContractor(contractId) {
        return apiClient.get("platform/contracts/" + contractId + "/contractor");
    },
    CloneToEdit(id) {
        return apiClient.post("platform/contracts/clone", id);
    },
    views(contractId) {
        return apiClient.post("platform/contracts/views/" + contractId);
    },
    getProgress(contractId) {
        return apiClient.get("platform/contracts/" + contractId + "/progress");
    },
    postPaymentParts(contractId, data) {
        return apiClient.post(
            "platform/contract/changes/" + contractId + "/payment/parts",
            data
        );
    },

    getContractChanges(contractId) {
        return apiClient.get(
            "platform/contract/changes/" + contractId + "/approval"
        );
    },
    saveContractChanges(contractId) {
        return apiClient.patch(
            "platform/contract/changes/" + contractId + "/approve"
        );
    },
    editContractChanges(contractId, data) {
        return apiClient.put("platform/contract/changes/" + contractId, data);
    },
    refuse(contractId, refusalCase) {
        return apiClient.patch(
            "platform/contracts/" + contractId + "/refuse",
            refusalCase
        );
    },
    getSearchFilter() {
        return apiClient.get("users/settings/filter");
    },
    saveSearchFilter(data) {
        return apiClient.put("users/settings/filter", data);
    },
    finishContract(contractId) {
        return apiClient.patch("platform/contracts/" + contractId + "/finish");
    },

    getContractHistory(contractId) {
        return apiClient.get("platform/contract/changes/" + contractId);
    },
    getContractExecution(contractId){
        return apiClient.get("platform/contracts/" + contractId + "/execution")
    }
};