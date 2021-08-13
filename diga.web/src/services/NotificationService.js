import apiClient from "./MainService.js";

export default {
    getNotifications(skip, take) {
        return apiClient.get("platform/notifications/" + skip + "/" + take);
    },
    readNotification(notificationId) {
        return apiClient.post("platform/notifications/read/" + notificationId)
    }
};