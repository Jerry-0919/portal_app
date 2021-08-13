<template>
  <div class="card p-4">
    <!-- <router-link class="btn btn-success" :to="{ name: 'addAdminCategories' }">
      {{ $t("add_category") }}
    </router-link> -->
    <p class="h4 my-4">{{ $t("number_of_users") }}: {{ countAll }}</p>
    <b-table-simple no-border-collapse responsive sticky-header>
      <b-thead head-variant="light">
        <b-tr>
          <b-th> ID </b-th>
          <b-th>{{ $t("full_name") }}</b-th>
          <b-th>Email</b-th>
          <b-th>{{ $t("telephone") }}</b-th>
          <b-th>{{ $t("domain") }}</b-th>
          <b-th>{{ $t("verification") }}</b-th>
          <b-th>{{ $t("interface_language") }}</b-th>
          <b-th>{{ $t("your_balance") }}</b-th>
        </b-tr>
      </b-thead>
      <b-tbody>
        <b-tr v-for="u in users" :key="u.id">
          <b-td>
            {{ u.id }}
          </b-td>
          <b-td>
            <router-link
              target="_blank"
              :to="{
                name: 'editAdminUsers',
                params: { id: u.id },
              }"
            >
              {{ u.name }}
            </router-link>
          </b-td>
          <b-td>
            {{ u.email }}
          </b-td>
          <b-td>
            {{ u.phoneNumber }}
          </b-td>
          <b-td>
            {{ u.domain }}
          </b-td>
          <b-td v-if="u.verificationRequested === true">
            <router-link
              target="_blank"
              :to="{
                name: 'editAdminUsers',
                params: { id: u.id },
              }"
            >
              {{ $t("verify") }}
            </router-link>
      
          </b-td>
          <b-td v-else>  </b-td>
          <b-td>
            {{ u.language }}
          </b-td>
          <b-td>
            {{ u.balance }}
          </b-td>
        </b-tr>
      </b-tbody>
    </b-table-simple>

    <hr class="my-5 bg-grey" />
    <div class="h4 my-4">{{ $t("Rating") }}</div>

    <b-table-simple no-border-collapse responsive sticky-header>
      <b-thead head-variant="light">
        <b-tr>
          <b-th> ID </b-th>
          <b-th>{{ $t("full_name") }}</b-th>
          <b-th>{{ $t("Points") }}</b-th>
          <b-th>{{ $t("description") }}</b-th>
          <b-th>{{ $t("date") }}</b-th>
        </b-tr>
      </b-thead>
      <b-tbody>
        <b-tr v-for="ur in usersRatings" :key="ur.id">
          <b-td>
            {{ ur.userId }}
          </b-td>
          <b-td>
            {{ ur.userName }}
          </b-td>

          <b-td>
            {{ ur.points }}
          </b-td>
          <b-td>
            {{ $t(ur.description) }}
          </b-td>
          <b-td>
            {{ ur.dateTime | moment("DD/MM/YYYY") }}
          </b-td>
        </b-tr>
      </b-tbody>
    </b-table-simple>
  </div>
</template>

<script>
import AdminUsersService from "@/services/AdminUsersService.js";

export default {
  data() {
    return {
      users: [],
      user: {},
      usersRatings: [],
      skip: 0,
      take: 10,
      countAll: 0,
    };
  },
  methods: {
    getUsers() {
      this.$store.commit("IS_LOADING_TRUE");
      AdminUsersService.getUsers(this.skip, this.take)
        .then((response) => {
          this.users = response.data.list;
          this.countAll = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getUser() {
      this.$store.commit("IS_LOADING_TRUE");
      AdminUsersService.getUser(this.user.id)
        .then((response) => {
          this.user = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    putUser(id) {
      this.$store.commit("IS_LOADING_TRUE");
      AdminUsersService.putUser(id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getUsers();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getUsersRatings() {
      this.$store.commit("IS_LOADING_TRUE");
      AdminUsersService.getUsersRatings(this.skip, this.take)
        .then((response) => {
          this.usersRatings = response.data.list;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getUsers();
    this.getUsersRatings();
  },
};
</script>

<style scoped>
.b-table-sticky-header {
  max-height: 500px !important;
  margin-bottom: 0 !important;
}
</style>
