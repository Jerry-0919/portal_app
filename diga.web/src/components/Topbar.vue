<template>
  <b-navbar
    v-if="$store.state.user"
    toggleable="lg"
    type="light"
    variant="faded"
  >
    <b-navbar-brand href="../../../Home/Index">
      <img src="/Assets/platform/logo.png" alt="Diga Logo" />
    </b-navbar-brand>
    <b-navbar-nav v-if="$root.showSideBar === true">
      <b-nav-item @click="$root.showSideBar = !$root.showSideBar">
        <i
          class="nav-i toggle-icon font-medium-4 collapse-toggle-icon primary bx bxs-chevrons-right align-middle"
          data-ticon="bxs-chevrons-right"
        ></i>
      </b-nav-item>
    </b-navbar-nav>

    <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

    <b-collapse id="nav-collapse" is-nav>
      <b-navbar-nav class="m-auto">
        <b-nav-item @click="selectCustomer">
          <b-button
            :class="{
              'b-customer-selected': $store.state.currentRole === 'Customer',
            }"
            class="badge text-uppercase b-customer my-0 h4"
          >
            {{ $t("customer") }}
          </b-button>
        </b-nav-item>
        <b-nav-item @click="selectExecutor">
          <b-button
            :class="{
              'b-executor-selected': $store.state.currentRole === 'Executor',
            }"
            class="badge text-uppercase b-executor my-0 h4"
          >
            {{ $t("executor") }}
          </b-button>
        </b-nav-item>
        <b-nav-item>
          <router-link :to="{ name: 'chat' }" class="position-relative">
            <i class=" bx bx-chat nav-i"></i>
            <span
              v-if="$store.state.user.unreadMessagesCount > 0"
              class="badge badge-pill badge-danger badge-up"
            >
              {{ $store.state.user.unreadMessagesCount }}
            </span>
          </router-link>
        </b-nav-item>
        <b-nav-item href="#"
          ><i class=" bx bx-check-circle nav-i"></i
        ></b-nav-item>
        <b-nav-item href="#"
          ><i class=" bx bx-calendar-alt nav-i"></i
        ></b-nav-item>
        <b-nav-item>
          <router-link :to="{ name: 'favouriteContracts' }">
            <i class=" bx bx-star warning nav-i"></i>
          </router-link>
        </b-nav-item>
        <b-nav-item>
          <i class=" bx bx-search nav-i"></i>
        </b-nav-item>
        <b-nav-item class="position-relative">
          <router-link :to="{ name: 'notifications' }">
            <template v-if="$store.state.user.unreadNotificationsCount > 0">
              <i class=" bx bx-bell orange nav-i"></i>
              <span class="badge badge-pill badge-danger badge-up">{{
                $store.state.user.unreadNotificationsCount
              }}</span></template
            >
            <i v-else class=" bx bx-bell nav-i"></i>
          </router-link>
        </b-nav-item>
      </b-navbar-nav>
      <!-- Right aligned nav items -->
      <b-navbar-nav class="ml-auto align-items-center">
        <b-nav-item-dropdown right no-caret>
          <template #button-content>
            <img v-if="currentLang === 'ru'" src="/Assets/img/ru.png" />
            <img v-if="currentLang === 'en'" src="/Assets/img/eng.png" />
            <img v-if="currentLang === 'es'" src="/Assets/img/sp.png" />
            <img v-if="currentLang === 'pt'" src="/Assets/img/por.png" />
          </template>
          <b-dropdown-item-button @click="changeLang('ru')">
            <img src="/Assets/img/ru.png" alt="RU" />
            Русский</b-dropdown-item-button
          >
          <b-dropdown-item-button @click="changeLang('en')">
            <img src="/Assets/img/eng.png" alt="EN" />
            English</b-dropdown-item-button
          >
          <b-dropdown-item-button @click="changeLang('es')"
            ><img src="/Assets/img/sp.png" alt="ES" />
            Español</b-dropdown-item-button
          >
          <b-dropdown-item-button @click="changeLang('pt')">
            <img src="/Assets/img/por.png" alt="PT" />
            Português</b-dropdown-item-button
          >
        </b-nav-item-dropdown>

        <b-nav-item-dropdown right no-caret>
          <!-- Using 'button-content' slot -->
          <template #button-content>
            <div class="d-flex align-items-center">
              <span class="mx-2">{{ username }} {{ surname }}</span>
              <div
                v-if="$store.state.user.avatar"
                class="avatar"
                v-bind:style="{
                  backgroundImage:
                    'url(' + '/img/src/' + $store.state.user.avatar + ')',
                }"
              ></div>
              <i
                v-else
                style="font-size: 40px"
                class=" bx bx-user-circle nav-i align-middle"
              ></i>
            </div>
          </template>
          <b-dropdown-item
            ><router-link :to="{ name: 'profile' }">{{
              $t("profile")
            }}</router-link></b-dropdown-item
          >
          <b-dropdown-item @click="logout">{{ $t("logout") }}</b-dropdown-item>
        </b-nav-item-dropdown>
      </b-navbar-nav>
    </b-collapse>
  </b-navbar>
</template>

<script>
import cookies from "@/cookies.js";
// import MainService from "@/services/MainService.js";

export default {
  components: {},
  data() {
    return {
      currentLang: localStorage.getItem("language") || "en",
      skip: 0,
      take: 5,
    };
  },
  methods: {
    selectCustomer() {
      this.$store.commit("PUSH_ROLE", "Customer");
      window.localStorage.setItem("role", "Customer");
    },
    selectExecutor() {
      // this.$root.currentRole = "Executor";
      this.$store.commit("PUSH_ROLE", "Executor");
      window.localStorage.setItem("role", "Executor");

      if (
        !this.$root.roles.some((r) => r.role === "PlatformExecutorMaster") &&
        !this.$root.roles.some((r) => r.role === "PlatformExecutorTeam") &&
        !this.$root.roles.some((r) => r.role === "PlatformExecutorCompany")
      ) {
        this.$router.push({ name: "completeRegistration" });
      }
    },
    changeLang(val) {
      this.$root.changeLang(val);
      this.$moment.locale(val);
      this.currentLang = val;
      this.$store.dispatch("getCategories");
    },
    logout() {
      this.$store.commit("LOGOUT");
      cookies.deleteCookie("bearer");
      window.localStorage.removeItem("role");

      this.$router.push({ name: "login" });
    },
  },
  sockets: {
    ReceiveNotification() {
      this.$store.state.user.unreadNotificationsCount += 1;
    },
  },
  computed: {
    username() {
      return this.$store.state.user === null
        ? ""
        : this.$store.state.user.name !== null
        ? this.$store.state.user.name
        : this.$store.state.user.email;
    },
    surname() {
      return this.$store.state.user === null
        ? ""
        : this.$store.state.user.surname !== null
        ? this.$store.state.user.surname
        : this.$store.state.user.email;
    },
  },
};
</script>
<style scoped>
/* .topbar-avatar {
  height: 25px;
  width: 25px;
  margin: unset !important;
} */
i {
  color: #438d90;
}
i:active,
i:hover,
i:focus {
  color: #f16b24;
}
</style>
