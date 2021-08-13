import apiClient from "./MainService.js";
export default {
  getAdminCategories(skip, take) {
    return apiClient.get("admin/platform/categories/" + skip + "/" + take);
  },
  getAdminCategory(id) {
    return apiClient.get("admin/platform/categories/" + id);
  },
  postAdminCategories(data) {
    return apiClient.post("admin/platform/categories", data);
  },
  editAdminCategory(id, data) {
    return apiClient.put("admin/platform/categories/" + id, data);
  },
  deleteAdminCategory(id) {
    return apiClient.delete("admin/platform/categories/" + id);
  },
};
