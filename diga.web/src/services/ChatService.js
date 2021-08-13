import apiClient from "./MainService.js";

export default {
    getRooms(skip, take) {
        return apiClient.get("platform/chat/rooms" + "/" + skip + "/" + take);
    },
    getMessages(roomId, skip, take) {
        return apiClient.get(
            "platform/chat/messages/" + roomId + "/" + skip + "/" + take
        );
    },
    sendMessage(message) {
        return apiClient.post('platform/chat/messages/send', message);
    },
    readMessage(messageId) {
        return apiClient.post('platform/chat/messages/read/' + messageId);
    },
    addRoomToFavourites(roomId) {
        return apiClient.post('platform/chat/rooms/favourites/' + roomId);
    },
    removeRoomToFavourites(roomId) {
        return apiClient.delete('platform/chat/rooms/favourites/' + roomId);
    },
    addSupportToRoom(roomId) {
        return apiClient.post('platform/chat/rooms/support/add/' + roomId);
    }
};