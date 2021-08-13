<template>
  <div class="wrapper">
    <div class="text-center loading" v-if="$store.state.loading">
      <b-spinner variant="primary" label="Text Centered"></b-spinner>
    </div>
    <div id="content">
      <div class="container-fluid position-relative p-0">
        <topbar />
        <div class="d-flex align-items-start bg-grey">
          <sidebar />
          <!-- <div class="float-right position-absolute chat-icon">
          <b-button variant="primary"
            ><i class="bx bx-chat bx-flashing"></i
          ></b-button>
        </div> -->
          <transition name="fade" mode="out-in">
            <router-view :key="$route.fullPath" />
          </transition>
        </div>
        <footerp />
      </div>
    </div>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
import sidebar from "./components/Sidebar.vue";
import topbar from "./components/Topbar.vue";
import footerp from "./components/Footer.vue";
import cookies from "@/cookies.js";

export default {
  components: {
    sidebar,
    topbar,
    footerp,
  },
  mounted() {
    // this.$store.commit("IS_LOADING_TRUE");
    var bearer = cookies.getCookie("bearer");
    if (bearer === false) {
      var currentRoute = this.$route.name;
      if (currentRoute !== "login" && currentRoute !== "register") {
        this.$router.push({ name: "login" });
      }
    } else {
      UserService.getUser()
        .then((response) => {
          this.$store.commit("PUSH_USER", response.data);
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    }
    this.$store.dispatch("getLanguages");
    this.$store.dispatch("getCountries");
    this.$store.dispatch("getCategories");
    this.$store.dispatch("getTags");
    this.$store.dispatch("getCities");
    this.$store.dispatch("getContractPriorities");
    this.$store.dispatch("getContractTypes");
    this.$store.dispatch("getContractBudgets");
    this.$store.dispatch("getVats");

    UserService.getRoles()
      .then((response) => {
        this.$root.roles = response.data;
        var localRole = window.localStorage.getItem("role");
        if (localRole !== null) {
          this.$store.state.currentRole = localRole;
        } else {
          if (response.data.some((r) => r.role === "PlatformCustomer")) {
            this.$store.state.currentRole = "Customer";
          }
          if (response.data.some((r) => r.role.includes("PlatformExecutor"))) {
            this.$store.state.currentRole = "Executor";
          }
        }
        if (response.data.some((r) => r.role === "Admin")) {
          this.$store.commit("PUSH_ADMIN");
        }

        //signalR
        this.$socket.authenticate(cookies.getCookie("bearer"));
        this.$socket.start({
          log: true,
        });
      })
      .catch(() => {
        this.$toasted.error(this.$t("oops_error"));
      })
      .finally(() => this.$store.commit("IS_LOADING_FALSE"));
  },
  computed: {},
};
</script>
<style>
.chat-icon {
  bottom: 0;
  right: 0;
}
.loading {
  position: fixed; /* Sit on top of the page content */
  width: 100%; /* Full width (cover the whole page) */
  height: 100%; /* Full height (cover the whole page) */
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5); /* Black background with opacity */
  z-index: 999; /* Specify a stack order in case you're using a different order for other elements */
}
</style>
<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>
