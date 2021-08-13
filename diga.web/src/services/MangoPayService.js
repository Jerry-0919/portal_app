import apiClient from "./MainService.js";

export default {
    postFile(file) {
        return apiClient.post("platform/user/kyc/individual", file)
    },
    postCompanyFiles(files) {
        return apiClient.post("platform/user/kyc/company", files)
    },
    getCards(page, perPage) {
        return apiClient.get('platform/user/cards/' + page + '/' + perPage);
    },
    createRegistration() {
        return apiClient.post('platform/user/cards/register');
    },
    updateRegistration(registrationId, data) {
        return apiClient.put('platform/user/cards/registrations/update/' + registrationId, data);
    },
    createWallet() {
        return apiClient.post("platform/user/wallets")
    },
    addBankAccount(data) {
        return apiClient.post("platform/user/bankAccounts", data)
    },
    makeComplaint(data) {
        return apiClient.post("users/complaint", data)
    },
    postVerificationDocs(data) {
        return apiClient.post("platform/verifications/apply", data)
    },
    postPayIn(data) {
        return apiClient.post("platform/payIns", data)
    }
}