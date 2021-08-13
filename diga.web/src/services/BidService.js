import apiClient from "./MainService.js";
export default {
    MakeOffer(contractId, data) {
        return apiClient.post("platform/contracts/bids/" + contractId, data);
    },
    getBids(contractId) {
        return apiClient.get("platform/contracts/bids/" + contractId + "/list");
    },
    EditOffer(contractId, data) {
        return apiClient.put("platform/contracts/bids/" + contractId, data);
    },
    WithdrawMyOffer(contractId) {
        return apiClient.patch(
            "platform/contracts/bids/" + contractId + "/withdrawn"
        );
    },
    ReturnMyOffer(contractId) {
        return apiClient.patch(
            "platform/contracts/bids/" + contractId + "/withdrawn/return"
        );
    },
    AddtoBookmarks(contractId, bidUserId) {
        return apiClient.post(
            "platform/contracts/bids/" +
            contractId +
            "/favorite/" +
            bidUserId +
            "/add"
        );
    },
    RemoveFromBookmarks(contractId, bidUserId) {
        return apiClient.delete(
            "platform/contracts/bids/" +
            contractId +
            "/favorite/" +
            bidUserId +
            "/remove"
        );
    },
    SelectExecutor(contractId, bidUserId) {
        return apiClient.patch(
            "platform/contracts/bids/" + contractId + "/select/" + bidUserId
        );
    },
    Reject(contractId, bidUserId) {
        return apiClient.patch(
            "platform/contracts/bids/" + contractId + "/remove/" + bidUserId
        );
    },
    UnReject(contractId, bidUserId) {
        return apiClient.patch(
            "platform/contracts/bids/" +
            contractId +
            "/remove/" +
            bidUserId +
            "/return"
        );
    },
};