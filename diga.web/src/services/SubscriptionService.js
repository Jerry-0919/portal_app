import apiClient from "./MainService.js";

export default {
  getSubscriptions() {
    return apiClient.get("profile/subscriptions");
  },
};
