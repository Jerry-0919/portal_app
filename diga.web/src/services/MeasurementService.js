import apiClient from "./MainService.js";

export default {
    getMeasurments(contractId) {
        return apiClient.get("platform/measurementReports/list/" + contractId)
    },
    getMeasurment(id) {
        return apiClient.get("platform/measurementReports/" + id)
    },
    editMeasurment(id, data) {
        return apiClient.put("platform/measurementReports/" + id, data)
    },
    createMeasurment(contractId) {
        return apiClient.post("platform/measurementReports/" + contractId)
    },
    deleteMeasurment(id) {
        return apiClient.delete("platform/measurementReports/" + id)
    },
    publishMeasurment(id) {
        return apiClient.post("platform/measurementReports/" + id + "/publish")
    },
    cancelPublication(id) {
        return apiClient.post("platform/measurementReports/" + id + "/cancelPublication")
    }
}