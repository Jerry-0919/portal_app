import apiClient from "./MainService.js";

export default {
    getEstimate(estimateId) {
        return apiClient.get("platform/estimates/" + estimateId);
    },
    getCompare(contractId, data) {
        return apiClient.get(
            "platform/estimates/" + contractId + "/executors",
            data
        );
    },
    editEstimate(estimateId, data) {
        return apiClient.put("platform/estimates/" + estimateId, data);
    },
    getMyEstimate(contractId) {
        return apiClient.get("platform/estimates/" + contractId + "/my");
    },
    editMyEstimate(contractId, data) {
        return apiClient.put("platform/estimates/" + contractId + "/my", data);
    },
    getEstimateApproved(contractId) {
        return apiClient.get("platform/estimates/" + contractId + "/approval");
    },
    postEstimateApproved(contractId, estimateId) {
        return apiClient.post("platform/estimates/" + contractId + '/' + estimateId + "/approve");
    },
    editEstimateApproved(estimateId, data) {
        return apiClient.put(
            "platform/estimates/" + estimateId + "/approval",
            data
        );
    },
    cloneEstimateApproved(estimateId, data) {
        return apiClient.put(
            "platform/estimates/" + estimateId + "/approval/clone",
            data
        );
    },
    getEstimateVersions(contractId) {
        return apiClient.get(
            "platform/estimates/" + contractId + "/approval/versions"
        );
    },
};