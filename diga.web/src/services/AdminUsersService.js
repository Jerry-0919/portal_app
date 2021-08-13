import apiClient from "./MainService.js";
export default {
    getUsersRatings(skip, take) {
        return apiClient.get("admin/users/ratings/" + skip + "/" + take);
    },
    getUsers(skip, take) {
        return apiClient.get("admin/users/" + skip + "/" + take);
    },
    getUser(id) {
        return apiClient.get("admin/users/" + id);
    },
    putUser(id, data) {
        return apiClient.put("admin/users/" + id, data);
    },
};