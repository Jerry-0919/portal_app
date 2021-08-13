<template>
  <div>
    <div class="card p-4">
      <p class="h5">{{ $t("edit") }}: {{ $t(user.name) }}</p>

      <b-form @submit.prevent="editUser">
        <b-row>
          <b-col>
            <b-form-group :label="$t('name')">
              <b-form-input v-model="user.name">
                {{ user.name }}
              </b-form-input>
            </b-form-group>
            <b-form-group :label="$t('surname')">
              <b-form-input v-model="user.surname">
                {{ user.surname }}
              </b-form-input>
            </b-form-group>
            <b-form-group :label="$t('domain')">
              <b-form-input v-model="user.domain">
                {{ user.domain }}
              </b-form-input>
            </b-form-group>
            <b-form-group :label="$t('balance')">
              <b-form-input v-model="user.balance">
                {{ user.balance }}
              </b-form-input>
            </b-form-group>
          </b-col>
          <b-col>
            <b-form-group :label="$t('telephone')">
              <b-form-input v-model="user.phoneNumber">
                {{ user.phoneNumber }}
              </b-form-input>
            </b-form-group>
            <b-form-group :label="$t('language')">
              <b-form-input v-model="user.language">
                {{ user.language }}
              </b-form-input>
            </b-form-group>
          </b-col>
        </b-row>

        <b-button type="submit" variant="primary">
          {{ $t("save") }}
        </b-button>
      </b-form>
      <div>
        <b-row v-for="v in verification" :key="v.id">
          <b-col>
            <p class="t4">
              {{ $t("verification") }} {{ v.id }} :
              {{ v.status }}
            </p>
            <div v-for="p in v.photos" :key="p">
              {{ p }}
            </div>
          </b-col>
        </b-row>
        <b-button @click="verify" variant="primary">
          {{ $t("verify") }}
        </b-button>
      </div>
    </div>
  </div>
</template>

<script>
import AdminUsersService from "@/services/AdminUsersService.js";

export default {
  props: ["id"],
  data() {
    return {
      user: {},
      verification: [],
    };
  },
  methods: {
    getUser() {
      this.$store.commit("IS_LOADING_TRUE");
      AdminUsersService.getUser(this.id)
        .then((response) => {
          this.user = response.data;
          this.verification = response.data.verifications;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    editUser() {
      this.$store.commit("IS_LOADING_TRUE");
      AdminUsersService.putUser(this.id, this.user)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({ name: "adminUsers" });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getUser();
  },
};
</script>
