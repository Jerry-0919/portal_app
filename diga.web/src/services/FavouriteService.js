import apiClient from "./MainService.js";

export default {
    getFavourites(skip, take) {
        return apiClient.get("platform/favorites/" + skip + "/" + take);
    },
    postFavourites(contractId) {
        return apiClient.post("platform/favorites/" + contractId);
    },

    deleteFavourites(contractId) {
        return apiClient.delete("platform/favorites/" + contractId);
    },
};