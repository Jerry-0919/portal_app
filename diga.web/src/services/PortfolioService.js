import apiClient from "./MainService.js";

export default {
    getAlbums(skip, take) {
        return apiClient.get("platform/portfolio/albums/" + skip + "/" + take);
    },
    postAlbum(album) {
        return apiClient.post("platform/portfolio/albums", album);
    },
    editAlbum(albumId, data) {
        return apiClient.put("platform/portfolio/albums/" + albumId, data);
    },
    deleteAlbum(albumId) {
        return apiClient.delete("platform/portfolio/albums/" + albumId);
    },

    getVideos() {
        return apiClient.get("platform/portfolio/videos");
    },
    addVideoUrl(value) {
        return apiClient.post("platform/portfolio/videos", value);
    },
    editVideoUrl(videoId, data) {
        return apiClient.put("platform/portfolio/videos/" + videoId, data);
    },
    removeVideoUrl(videoId) {
        return apiClient.delete("platform/portfolio/videos/" + videoId);
    },
};