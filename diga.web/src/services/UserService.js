import apiClient from "./MainService.js";

export default {
    getUser() {
        return apiClient.get("users/info");
    },
    postUser(user) {
        return apiClient.post("profile/me", user);
    },
    postRole(role) {
        return apiClient.post("roles/users/" + role);
    },
    // checkIsAll() {
    //     return apiClient.get("Account/fields");
    // },
    getRoles() {
        return apiClient.get("roles/users");
    },
    deleteRole(role) {
        return apiClient.delete("roles/users/" + role);
    },
    // getInfo() {
    //     return apiClient.get("users/info");
    // },
    editUser(user) {
        return apiClient.put("users/edit", user);
    },
    getCompanyInfo() {
        return apiClient.get("platform/companies/get");
    },
    postCompanyInfo(info) {
        return apiClient.put("platform/companies", info);
    },
    changePassword(model) {
        return apiClient.post("users/password/change", model);
    },
    forgotPasswordEmail(email) {
        return apiClient.post("account/password/forgot", email);
    },
    setNewPassword(model) {
        return apiClient.post("account/password/forgot/check", model);
    },
    getTags() {
        return apiClient.get("users/tags/list");
    },
    postTag(tag) {
        return apiClient.post("users/tags/add", tag);
    },
    removeTag(tag) {
        return apiClient.delete("users/tags/remove/" + tag);
    },
    getCategories() {
        return apiClient.get("users/categories/list");
    },
    postCategory(categoryId) {
        return apiClient.post("users/categories/add", categoryId);
    },
    removeCategory(categoryId) {
        return apiClient.delete("users/categories/remove/" + categoryId);
    },
    getUserCard() {
        return apiClient.get("users/card");
    },
    getCard(userId) {
        return apiClient.get("users/card/" + userId);
    },
    getSettings() {
        return apiClient.get("users/settings")
    },
    editSettings(data) {
        return apiClient.put("users/settings", data)
    },
    getPublicProfile(userId) {
        return apiClient.get("platform/publicProfile/" + userId)
    },
    getPublicProfileReviews(userId, skip, take) {
        return apiClient.get("platform/publicProfile/reviews/" + userId + "/" + skip + "/" + take)
    }
};