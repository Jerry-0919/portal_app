<template>
  <div class="card mb-0 p-4">
    <div>
      <p class="h4 mb-3">{{ $t("email_notifications") }}</p>
      <b-form-checkbox-group>
        <b-form-checkbox v-model="settings.newMessagesEmailNotification">
          {{ $t("get_notifications_msg") }}
        </b-form-checkbox>
      </b-form-checkbox-group>
      <b-button class="my-3" @click="editSettings" variant="primary">{{
        $t("save")
      }}</b-button>
    </div>
    <div>
      <p class="h4 my-4">{{ $t("change_password") }}</p>
      <b-form @submit="onSubmit" class="row">
        <b-col>
          <b-form-group :label="$t('old_password')">
            <b-form-input
              :class="{ 'input-invalid': $v.oldPassword.$error }"
              type="password"
              v-model="oldPassword"
              :placeholder="$t('old_password')"
            ></b-form-input>
          </b-form-group>
        </b-col>
        <b-col>
          <b-form-group :label="$t('new_password')">
            <b-form-input
              :class="{ 'input-invalid': $v.newPassword.$error }"
              type="password"
              v-model="newPassword"
              :placeholder="$t('new_password')"
            ></b-form-input>
          </b-form-group>
        </b-col>
        <b-col>
          <b-form-group :label="$t('confirm_password')">
            <b-form-input
              :class="{ 'input-invalid': $v.confirmNewPassword.$error }"
              type="password"
              v-model="confirmNewPassword"
              :placeholder="$t('confirm_password')"
            ></b-form-input>
          </b-form-group>
        </b-col>
        <b-form-group class="col-12 text-center">
          <b-button type="submit" variant="primary">{{ $t("save") }}</b-button>
        </b-form-group>
      </b-form>
    </div>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
import {
  required,
  minLength,
  maxLength,
  sameAs,
} from "vuelidate/lib/validators";

export default {
  props: ["settings"],
  data() {
    return {
      oldPassword: "",
      newPassword: "",
      confirmNewPassword: "",
    };
  },
  validations: {
    oldPassword: { required },
    newPassword: {
      required,
      minLength: minLength(6),
      maxLength: maxLength(12),
    },
    confirmNewPassword: { required, sameAsPassword: sameAs("newPassword") },
  },
  methods: {
    editSettings() {
      UserService.editSettings({
        newMessagesEmailNotification: this.settings
          .newMessagesEmailNotification,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    onSubmit(evt) {
      evt.preventDefault();
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      }
      this.$store.commit("IS_LOADING_TRUE");
      UserService.changePassword({
        oldPassword: this.oldPassword,
        newPassword: this.newPassword,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
};
</script>

<style scoped></style>
