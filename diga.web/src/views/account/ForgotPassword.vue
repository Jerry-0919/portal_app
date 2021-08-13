<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 text-center m-auto">
        <form @submit.prevent="onSubmit">
          <div class="p-3 p-md-3 p-xl-5 p-lg-5">
            <p class="p-purple-dark fs-24 font-weight-bolder text-center">
              {{ $t("restore_access") }}
            </p>
            <div class="m-auto cart-forms">
              <div v-if="$v.email.$error">
                <p
                  v-if="!$v.email.required"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("required") }}
                </p>
                <p
                  v-if="!$v.email.email"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("valid_email") }}
                </p>
              </div>

              <div
                :class="{ 'input-invalid': $v.email.$error }"
                class="form-group input-group diga-input-hover"
              >
                <span class="login input-group-text"
                  ><img src="../../../Assets/img/mail1.png"
                /></span>
                <input
                  @blur="$v.email.$touch()"
                  v-model="email"
                  type="email"
                  name="email"
                  class="form-control diga-input border-0"
                  id="inputEmailLogin"
                  placeholder="Email"
                />
              </div>
            </div>
            <div class="text-center">
              <button
                type="submit"
                :disabled="$v.$invalid"
                class="btn btn_azul fs-18"
              >
                {{ $t("restore_access") }}
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { required, email } from "vuelidate/lib/validators";
import UserService from "@/services/UserService.js";

export default {
  data() {
    return {
      email: "",
    };
  },
  validations: {
    email: {
      required,
      email,
    },
  },
  methods: {
    onSubmit(evt) {
      evt.preventDefault();
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      }

      this.$store.commit("IS_LOADING_TRUE");
      UserService.forgotPasswordEmail({
        email: this.email,
      })
        .then(() => {
          this.$toasted.success(this.$t("forgot_password_success"));
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
