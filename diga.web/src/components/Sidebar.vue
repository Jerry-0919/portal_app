<template>
  <nav class="mt-3" id="sidebar" :class="{ active: $root.showSideBar }">
    <ul class="list-unstyled components text-white bg-grey-dark my-0">
      <li class="d-flex justify-content-between align-items-center my-0">
        <div class="flex-fill">
          <router-link :to="{ name: 'home_index' }"
            ><i class="bx bx-home"></i> {{ $t("homepage") }}
          </router-link>
        </div>
        <div class="flex-shrink">
          <a
            v-if="$root.showSideBar === false"
            @click="$root.showSideBar = !$root.showSideBar"
          >
            <i
              class="nav-i toggle-icon collapse-toggle-icon bx bxs-chevrons-left"
              style="cursor:pointer"
              data-ticon="bxs-chevrons-left"
            ></i
          ></a>
        </div>
      </li>
    </ul>

    <ul class="list-unstyled components bg-blue text-white">
      <li>
        <a
          href="#profileDropdown"
          data-toggle="collapse"
          class="dropdown-toggle"
          ><i class="bx bx-user-circle align-middle"></i>
          {{ $t("profile") }}
        </a>
        <ul id="profileDropdown" class="list-unstyled">
          <li class="dropdown-item b-blue">
            <router-link :to="{ name: 'profile' }"
              ><i class="bx bx-cog align-middle"></i>
              {{ $t("settings") }}</router-link
            >
          </li>
          <li class="dropdown-item b-blue">
            <router-link :to="{ name: 'chat' }">
              <i class="bx bx-question-mark align-middle"></i>
              {{ $t("support") }}
            </router-link>
          </li>
          <li class="dropdown-item b-blue">
            <a @click="logout">
              <i class="bx bx-log-out  align-middle"></i>
              {{ $t("logout") }}</a
            >
          </li>
        </ul>
      </li>
      <hr />
      <li>
        <a
          href="#contractDropdown"
          data-toggle="collapse"
          class="dropdown-toggle"
        >
          <i class="bx bx-detail align-middle"></i> {{ $t("contracts") }}
        </a>
        <ul id="contractDropdown" class="list-unstyled">
          <li
            class="dropdown-item b-blue"
            v-if="$store.state.currentRole === 'Customer'"
          >
            <router-link :to="{ name: 'contracts' }">
              <i class="bx bx-list-check align-middle"></i>
              {{ $t("all_my_contracts") }}</router-link
            >
          </li>
          <li class="dropdown-item b-blue">
            <router-link :to="{ name: 'allContracts' }"
              ><i class="bx bx-file"></i> {{ $t("all_contracts") }}</router-link
            >
          </li>
          <li class="dropdown-item b-blue">
            <router-link
              v-if="this.$store.state.currentRole === 'Customer'"
              :to="{ name: 'contracts_create' }"
            >
              <i class="bx bx-list-plus align-middle"></i>
              {{ $t("add_contract") }}
            </router-link>
          </li>
        </ul>
      </li>
      <hr />
      <li>
        <a
          href="#activityDropdown"
          data-toggle="collapse"
          class="dropdown-toggle"
        >
          <i class="bx bx-message-rounded-edit align-middle"></i>
          {{ $t("activity") }}
        </a>
        <ul id="activityDropdown" class="list-unstyled">
          <li class="dropdown-item b-blue">
            <router-link :to="{ name: 'chat' }">
              <i class="bx bx-envelope align-middle"></i>
              {{ $t("messages") }}</router-link
            >
          </li>
          <li class="dropdown-item b-blue">
            <router-link :to="{ name: 'favouriteContracts' }"
              ><i class="bx bx-star align-middle"></i>
              {{ $t("bookmarks") }}</router-link
            >
          </li>
          <template v-if="$store.state.user && $store.state.user.verificationStatus">
            <li
              v-if="$store.state.user.verificationStatus === null"
              class="dropdown-item b-blue"
            >
              <a v-b-modal.verificationModal>
                <i class="bx bx-check-shield align-middle"></i>
                {{ $t("verification") }}
              </a>
              <verificationModal />
            </li>
            <p
              v-if="
                $store.state.user.verificationStatus === 'UnderConsideration'
              "
              class="my-3"
            >
              <i class="bx bx-check-shield align-middle"></i>
              {{ $t("verification_progress") }}
            </p>
          </template>
          <li class="dropdown-item b-blue">
            <router-link :to="{ name: 'profile' }">
              <i class="bx bx-user-plus align-middle"></i>
              {{ $t("profile_plus") }}</router-link
            >
          </li>
        </ul>
      </li>
      <hr />
      <li
        v-if="
          $store.state.user &&
            $store.state.user.domain &&
            $store.state.user.domain !== null
        "
        class=""
      >
        <a :href="$store.state.user.domain" target="_blank"
          ><i class="bx bx-file"></i> CRM/ERP</a
        >
      </li>
      <li v-if="$store.state.isAdmin === true" class="">
        <router-link :to="{ name: 'adminCategories' }"
          ><i class="bx bx-file"></i> {{ $t("categories") }}</router-link
        >
      </li>
      <li v-if="$store.state.isAdmin === true" class="">
        <router-link :to="{ name: 'adminUsers' }"
          ><i class="bx bx-file"></i> {{ $t("users") }}</router-link
        >
      </li>
    </ul>
  </nav>
</template>

<script>
import verificationModal from "@/components/profile/Verification.vue";
import cookies from "@/cookies.js";

export default {
  components: { verificationModal },
  data() {
    return {};
  },
  methods: {
    logout() {
      this.$store.commit("LOGOUT");
      cookies.deleteCookie("bearer");
      window.localStorage.removeItem("role");

      this.$router.push({ name: "login" });
    },
  },
};
</script>
<style scoped>
.router-link-exact-active {
  background: #ffffff47;
}
</style>
