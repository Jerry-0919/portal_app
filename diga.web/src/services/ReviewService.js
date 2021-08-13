import apiClient from "./MainService.js";

export default {
    getExecutorReviews(skip, take, categoryId) {
        let url = "platform/contracts/reviews/executor/" + skip + "/" + take;
        if (categoryId > 0) {
            url += "?categoryId=" + categoryId;
        }
        return apiClient.get(url);
    },

    getCustomerReviews(skip, take) {
        return apiClient.get(
            "platform/contracts/reviews/customer/" + skip + "/" + take
        );
    },
    getCustomerReview(contractId) {
        return apiClient.get(
            "platform/contracts/reviews/" + contractId + "/customer"
        );
    },
    getExecutorReview(contractId) {
        return apiClient.get(
            "platform/contracts/reviews/" + contractId + "/executor"
        );
    },
    postReview(contractId, data) {
        return apiClient.post("platform/contracts/reviews/" + contractId, data);
    },
};