import Vue from "vue";
import Router from "vue-router";
import profile from "./views/Profile.vue";
import publicProfile from "./views/PublicProfile.vue";
import publicProfileInside from "./views/PublicProfileInside.vue";
import tokenAuth from "./views/TokenAuth.vue";
import layout from "./Index.vue";
import homeIndex from "./views/home/Index.vue";
import login from "./views/account/Login.vue";
import register from "./views/account/Register.vue";
import contracts_form from "./views/contracts/Form.vue";
import contractsIndex from "./views/contracts/Index.vue";
import allContracts from "./views/contracts/AllContracts.vue";
import versions from "./views/contracts/Versions.vue";
import favourites from "./views/contracts/Favourites.vue";
import adminCategories from "./views/admin/categories/Index.vue";
import editAdminCategories from "./views/admin/categories/Form.vue";
import adminUsers from "./views/admin/users/Index.vue";
import editAdminUsers from "./views/admin/users/Form.vue";

import completeRegistration from "./views/account/CompleteRegistration.vue";
import forgot from "./views/account/ForgotPassword.vue";
import newPass from "./views/account/SetNewPassword.vue";
import choosing from "./components/contracts/Choosing.vue";
import finish from "./components/contracts/Finish.vue";
import signing from "./components/contracts/Signing.vue";
import notifications from "./components/Notifications.vue";
import reviews from "./components/contracts/Reviews.vue";
import ContractHistory from "./components/contracts/ContractHistory.vue";
import execution from "./components/contracts/Execution.vue";
import confirmation from "./components/contracts/Confirmation.vue";
versions;
import estimateConstructor from "./components/contracts/EstimateConstructor.vue";
import chat from "./components/Chat.vue";
import AutoMeasurement from "./components/contracts/AutoMeasurement.vue";
import After3DS from "./views/After3DS.vue";
// import store from "./store.js";
Vue.use(Router);

// const loadDictionaries = (to, from, next) => {
//     if (store.state.isDictionariesLoaded === false) {
//         store.dispatch("getLanguages");
//     } else {
//         next("/");
//     }
// };
const routes = [
    { path: "/login", name: "login", component: login },
    { path: "/register", name: "register", component: register },
    { path: "/forgot", name: "forgot", component: forgot },
    {
        path: "/register-with-role/:role",
        name: "registerWithRole",
        component: register,
        props: (route) => ({ role: route.params.role }),
    },
    {
        path: "/set-new-password/:token",
        name: "setNewPassword",
        component: newPass,
        props: (route) => ({ token: route.params.token }),
    },
    {
        path: "/complete-registration",
        name: "completeRegistration",
        component: completeRegistration,
    },
    {
        path: "/complete-registration-with-role/:role",
        name: "completeRegistrationWithRole",
        component: completeRegistration,
        props: (route) => ({ role: route.params.role }),
    },

    {
        path: "/auth/:token",
        component: tokenAuth,
        props: (route) => ({ token: route.params.token }),
    },
    {
        path: "/public-profile/:id(\\d+)",
        name: "public_profile",
        component: publicProfile,
        props: (route) => ({ id: route.params.id }),
    },

    {
        path: "/",
        // name: "layout",
        component: layout,
        // beforeEnter: loadDictionaries,
        children: [
            { path: "", name: "home_index", component: homeIndex },
            { path: "/profile", name: "profile", component: profile },
            { path: "/profile/:id(\\d+)", name: "public_profile_inside", component: publicProfileInside, props: (route) => ({ id: route.params.id }) },
            {
                path: "/contracts/create",
                name: "contracts_create",
                component: contracts_form,
            },
            {
                path: "/contracts/:id(\\d+)/edit",
                name: "Draft",
                component: contracts_form,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "/contracts/:id(\\d+)/versions",
                name: "versions",
                component: versions,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "/contracts/:id(\\d+)/edit",
                name: "Deffered",
                component: contracts_form,
                props: (route) => ({ id: route.params.id }),
            },

            {
                path: "/contracts/index",
                name: "contracts",
                component: contractsIndex,
            },
            {
                path: "/contracts/published",
                name: "allContracts",
                component: allContracts,
            },

            {
                path: "/contracts/index/:status",
                name: "contracts_with_status",
                component: contractsIndex,
                props: (route) => ({ status: route.params.status }),
            },
            {
                path: "choosing-contractor/:id(\\d+)",
                name: "Contractor",
                component: choosing,
                props: (route) => ({ id: route.params.id }),
            },

            {
                path: "signing/:id(\\d+)",
                name: "Signing",
                component: signing,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "estimate-constructor/:id(\\d+)",
                name: "EstimateApproval",
                component: estimateConstructor,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "confirmation/:id(\\d+)",
                name: "ContractApproval",
                component: confirmation,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "execution/:id(\\d+)",
                name: "Executing",
                component: execution,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "review/:id(\\d+)",
                name: "Reviews",
                component: reviews,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "contract-history/:id(\\d+)",
                name: "ContractHistory",
                component: ContractHistory,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "finish/:id(\\d+)",
                name: "Finished",
                component: finish,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "/contracts/favourites",
                name: "favouriteContracts",
                component: favourites,
            },
            {
                path: "/notifications",
                name: "notifications",
                component: notifications,
            },
            {
                path: "/admin/categories",
                name: "adminCategories",
                component: adminCategories,
            },

            {
                path: "/admin/users",
                name: "adminUsers",
                component: adminUsers,
            },
            {
                path: "/admin/users/:id(\\d+)/edit",
                name: "editAdminUsers",
                component: editAdminUsers,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "/admin/categories/:id(\\d+)/edit",
                name: "editAdminCategories",
                component: editAdminCategories,
                props: (route) => ({ id: route.params.id }),
            },
            {
                path: "/admin/categories/create",
                name: "addAdminCategories",
                component: editAdminCategories,
            },
            {
                path: "/chat",
                name: "chat",
                component: chat,
            },
            {
                path: "/chat/:id(\\d+)",
                name: "chatroom",
                component: chat,
                props: (route) => ({ roomId: route.params.id }),
            },
            {
                path: "automedicao/:contractId(\\d+)/:id(\\d+)",
                name: "AutoMeasurement",
                component: AutoMeasurement,
                props: (route) => ({ id: route.params.id, contractId: route.params.contractId }),
            },
            {
                path: "payment",
                name: "after3ds",
                component: After3DS,
                props: (route) => ({ transactionId: route.params.transactionId, type: route.params.type, contractId: route.params.contractId, id: route.params.id })
            }
        ],
    },
];
export default new Router({
    routes,
});