<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 text-center m-auto">
        <form @submit.prevent="authorize">
          <div class="p-3 p-md-3 p-xl-5 p-lg-5">
            <p class="p-purple-dark fs-24 font-weight-bolder text-center">
              {{ $t("login") }}
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

              <div v-if="$v.password.$error">
                <p
                  v-if="!$v.password.required"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("required") }}
                </p>
                <p
                  v-if="!$v.password.minLength"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("min_number_of_symbols") }} 4
                </p>
              </div>

              <div
                :class="{ 'input-invalid': $v.password.$error }"
                class="form-group input-group diga-input-hover"
              >
                <span class="login input-group-text form-icon"
                  ><i class="fas fa-lock"></i
                ></span>
                <input
                  @blur="$v.password.$touch()"
                  v-model="password"
                  type="password"
                  name="password"
                  id="inputPasswordLogin"
                  class="form-control diga-input border-0"
                  :placeholder="$t('password')"
                />
              </div>
            </div>
            <div class="text-center">
              <button
                type="submit"
                :disabled="$v.$invalid"
                class="btn btn_azul fs-18"
              >
                {{ $t("login") }}
              </button>
            </div>
            <div class="my-5">
              {{ $t("forgot_password") }}
              <router-link
                class="mt-2 font-weight-bold"
                :to="{ name: 'forgot' }"
              >
                {{ $t("restore_access") }}
              </router-link>
            </div>
            <div class="my-5">
              {{ $t("dont_have_account") }}
              <router-link
                class="mt-2 font-weight-bold"
                :to="{ name: 'register' }"
              >
                {{ $t("register") }}
              </router-link>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
import MainService from "@/services/MainService.js";
import cookies from "@/cookies.js";
import { required, email, minLength } from "vuelidate/lib/validators";
export default {
  components: {},
  data() {
    return {
      email: "",
      password: "",
    };
  },
  validations: {
    email: {
      required,
      email,
    },
    password: {
      required,
      minLength: minLength(4),
    },
  },
  methods: {
    authorize() {
      this.$store.commit("IS_LOADING_TRUE");
      MainService.post("Account/token", {
        email: this.email,
        password: this.password,
      })
        .then((response) => {
          this.$toasted.success(this.$t("welcome_back"));
          this.$store.commit("PUSH_USER", response.data);
          cookies.setCookie("bearer", response.data.access_token, "27d");

          MainService.defaults.headers.common["Authorization"] =
            "Bearer " + response.data.access_token;
          UserService.getUser()
            .then((response) => {
              this.$store.commit("PUSH_USER", response.data);
            })
            .catch((error) => {
              console.log(error);
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));
          UserService.getRoles()
            .then((response) => {
              var roles = response.data;
              if (
                roles.some(
                  (e) =>
                    e.role === "PlatformCustomer" ||
                    e.role.includes("PlatformExecutor")
                )
              ) {
                this.$router.push({ name: "home_index" });
              } else {
                this.$router.push({ name: "completeRegistration" });
              }
            })
            .catch(() => {
              this.$toasted.error(this.$t("oops_error"));
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));
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
