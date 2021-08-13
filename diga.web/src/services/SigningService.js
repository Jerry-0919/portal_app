import apiClient from "./MainService.js";

export default {
    getContractSigning(contractId) {
        return apiClient.get("platform/contracts/signing/" + contractId);
    },

    editContractSigning(contractId, data) {
        return apiClient.patch(
            "platform/contracts/signing/edit/" + contractId,
        data
        );
    },
    approveContractSigning(contractId) {
        return apiClient.patch("platform/contracts/signing/approve/" + contractId);
    },
};