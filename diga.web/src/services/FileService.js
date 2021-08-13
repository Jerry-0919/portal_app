import apiClient from "./MainService.js";

export default {
    uploadFile(formdata) {
        return apiClient.post("documents/temp", formdata, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
    },
    uploadFiles(formdata) {
        return apiClient.post("documents/temp/list", formdata, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
    },
    uploadPhoto(formdata) {
        return apiClient.post("images/temp", formdata, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
    },
    uploadPhotos(formdata) {
        return apiClient.post("images/temp/list", formdata, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
    },
};