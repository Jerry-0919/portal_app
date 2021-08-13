<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 text-center m-auto">
        <b-form @submit="onSubmit">
          <div class="p-3 p-md-3 p-xl-5 p-lg-5">
            <p class="p-purple-dark fs-24 font-weight-bolder text-center">
              {{ $t("enter_new_password") }}
            </p>
            <div v-if="$v.newPassword.$error">
              <p
                v-if="!$v.newPassword.required"
                class="text-center m-0 lh-1-5 error fs-13 text-red"
              >
                {{ $t("required") }}
              </p>
              <p
                v-if="!$v.newPassword.minLength"
                class="text-center m-0 lh-1-5 error fs-13 text-red"
              >
                {{ $t("min_number_of_symbols") }} 6
              </p>

              <p
                v-if="!$v.newPassword.maxLength"
                class="text-center m-0 lh-1-5 error fs-13 text-red"
              >
                {{ $t("maxlength") }} 12
              </p>
            </div>
            <b-form-group>
              <b-form-input
                @blur="$v.newPassword.$touch()"
                :class="{ 'input-invalid': $v.newPassword.$error }"
                type="password"
                v-model="newPassword"
                :placeholder="$t('new_password')"
              ></b-form-input>
            </b-form-group>
            <div v-if="$v.confirmNewPassword.$error">
              <p
                v-if="!$v.confirmNewPassword.required"
                class="text-center m-0 lh-1-5 error fs-13 text-red"
              >
                {{ $t("required") }}
              </p>

              <p
                v-if="!$v.confirmNewPassword.sameAsPassword"
                class="text-center m-0 lh-1-5 error fs-13 text-red"
              >
                {{ $t("passwords_dont_match") }}
              </p>
            </div>
            <b-form-group>
              <b-form-input
                @blur="$v.confirmNewPassword.$touch()"
                :class="{ 'input-invalid': $v.confirmNewPassword.$error }"
                type="password"
                v-model="confirmNewPassword"
                :placeholder="$t('confirm_password')"
              ></b-form-input>
            </b-form-group>
            <b-form-group class="text-center">
              <b-button
                :disabled="$v.$invalid"
                type="submit"
                variant="primary"
                >{{ $t("save") }}</b-button
              >
            </b-form-group>
          </div>
        </b-form>
      </div>
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
  props: ["token"],
  data() {
    return {
      newPassword: "",
      confirmNewPassword: "",
    };
  },
  validations: {
    newPassword: {
      required,
      minLength: minLength(6),
      maxLength: maxLength(12),
    },
    confirmNewPassword: { required, sameAsPassword: sameAs("newPassword") },
  },
  methods: {
    onSubmit(evt) {
      evt.preventDefault();
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      }

      this.$store.commit("IS_LOADING_TRUE");
      UserService.setNewPassword({
        code: this.token,
        newPassword: this.newPassword,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({ name: "login" });
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
