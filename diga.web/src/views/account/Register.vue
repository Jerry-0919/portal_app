<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 text-center m-auto">
        <form @submit.prevent="register">
          <div class="p-3 p-md-3 p-xl-5 p-lg-5">
            <p class="p-purple-dark fs-24 font-weight-bolder text-center">
              {{ $t("register") }}
            </p>
            <div class="m-auto cart-forms">
              <div v-if="$v.name.$error">
                <p
                  v-if="!$v.name.required"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("required") }}
                </p>
                <p
                  v-if="!$v.name.minLength"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("min_number_of_symbols") }} 5
                </p>
              </div>

              <div
                :class="{ 'input-invalid': $v.name.$error }"
                class="form-group input-group diga-input-hover"
              >
                <span class="login input-group-text form-name"
                  ><img src="../../../Assets/img/name1.png"
                /></span>
                <input
                  @blur="$v.name.$touch()"
                  v-model="name"
                  type="text"
                  name="name"
                  class="form-control diga-input
                border-0"
                  id="inputNameRegister"
                  :placeholder="$t('name')"
                  minlength="5"
                />
              </div>

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
                  id="inputEmailRegister"
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
                  id="inputPasswordRegister"
                  class="form-control diga-input border-0"
                  :placeholder="$t('password')"
                />
              </div>

              <div v-if="$v.phoneNumber.$error">
                <p
                  v-if="!$v.phoneNumber.required"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("required") }}
                </p>
                <p
                  v-if="!$v.phoneNumber.minLength"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("min_number_of_symbols") }} 9
                </p>
                <p
                  v-if="!$v.phoneNumber.integer"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("valid_phone") }}
                </p>
              </div>
              <div
                :class="{ 'input-invalid': $v.phoneNumber.$error }"
                class="form-group input-group diga-input-hover"
              >
                <span class="login input-group-text"
                  ><img src="../../../Assets/img/call1.png"
                /></span>
                <input
                  @blur="$v.phoneNumber.$touch()"
                  v-model="phoneNumber"
                  type="tel"
                  name="telephone"
                  id="inputTelephoneRegister"
                  class="form-control diga-input border-0"
                  :placeholder="$t('telephone')"
                />
              </div>

              <b-form-checkbox
                class="mb-3"
                id="with-erp"
                v-model="withErp"
                name="with-erp"
              >
                {{ $t("with_erp") }}
              </b-form-checkbox>
              <div v-if="$v.domain.$error">
                <p
                  v-if="!$v.domain.required"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("required") }}
                </p>
                <p
                  v-if="!$v.domain.alpha"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("domain_alpha") }}
                </p>
                <p
                  v-if="!$v.domain.minLength"
                  class="text-center m-0 lh-1-5 error fs-13 text-red"
                >
                  {{ $t("min_number_of_symbols") }} 3
                </p>
              </div>
              <div v-if="withErp">
                <div
                  :class="{ 'input-invalid': $v.domain.$error }"
                  id="with-erp"
                  class="input-group form-group diga-input-hover ml-0"
                >
                  <span class="login input-group-text border-0"
                    ><img src="../../../Assets/img/domen1.png"
                  /></span>
                  <input
                    @blur="$v.domain.$touch()"
                    v-model="domain"
                    id="domainvalRegister"
                    type="text"
                    name="domain"
                    class="form-control diga-input border-0"
                    :placeholder="$t('domain')"
                    maxlength="25"
                  />
                  <input
                    type="text"
                    class="col-sm-4 form-control diga-input diga-disabled border-0 pl-0 bg-white font-weight-bold p-valid"
                    value="| .diga.pt"
                    disabled="disabled"
                  />
                </div>
                <p
                  id="domainIsBusyRegister"
                  class="text-center text-red"
                  style="display: none;"
                >
                  {{ $t("domain_is_busy") }}
                </p>
                <div class="fs-14 mb-3 ml-2 text-left lh-1-5">
                  * {{ $t("modal_domain_text") }}
                </div>

                <div class="form-group my-4">
                  <div class="m-auto">
                    {{ $t("interface_language") }}:
                    <b-dropdown
                      v-model="lang"
                      variant="outline-primary"
                      no-caret
                    >
                      <template #button-content>
                        <div v-if="lang === 'ru'">
                          <img src="/Assets/img/ru.png" />Русский
                        </div>
                        <div v-if="lang === 'en'">
                          <img src="/Assets/img/eng.png" /> English
                        </div>
                        <div v-if="lang === 'es'">
                          <img src="/Assets/img/sp.png" /> Español
                        </div>
                        <div v-if="lang === 'pt'">
                          <img src="/Assets/img/por.png" /> Português
                        </div>
                      </template>
                      <b-dropdown-item @click="lang = 'ru'" value="ru">
                        <img src="/Assets/img/ru.png" alt="RU" />
                        Русский</b-dropdown-item
                      >
                      <b-dropdown-item @click="lang = 'en'" value="en">
                        <img src="/Assets/img/eng.png" alt="EN" />
                        English</b-dropdown-item
                      >
                      <b-dropdown-item @click="lang = 'es'" value="es"
                        ><img src="/Assets/img/sp.png" alt="ES" />
                        Español</b-dropdown-item
                      >
                      <b-dropdown-item @click="lang = 'pt'" value="pt">
                        <img src="/Assets/img/por.png" alt="PT" />
                        Português</b-dropdown-item
                      >
                    </b-dropdown>
                  </div>
                </div>
              </div>
            </div>
            <div class="text-center">
              <button
                :disabled="$v.$invalid"
                type="submit"
                class="btn btn_azul fs-18"
              >
                {{ $t("register") }}
              </button>
            </div>
            <div class="my-5">
              {{ $t("have_account") }}
              <router-link
                class="mt-2 font-weight-bold"
                :to="{ name: 'login' }"
              >
                {{ $t("login") }}
              </router-link>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import MainService from "@/services/MainService.js";
import UserService from "@/services/UserService.js";

import cookies from "@/cookies.js";
import {
  required,
  requiredIf,
  email,
  minLength,
  integer,
  alpha,
} from "vuelidate/lib/validators";
export default {
  components: {},
  props: ["role"],

  data() {
    return {
      name: "",
      email: "",
      password: "",
      phoneNumber: "",
      domain: "",
      lang: "pt",
      withErp: false,
    };
  },
  validations: {
    name: {
      required,
      minLength: minLength(5),
    },
    email: {
      required,
      email,
    },
    password: {
      required,
      minLength: minLength(4),
    },
    phoneNumber: {
      required,
      minLength: minLength(9),
      integer,
    },
    domain: {
      required: requiredIf(function() {
        if (this.withErp === true) return true;
      }),
      alpha,
      minLength: minLength(3),
    },
  },
  methods: {
    register() {
      this.$store.commit("IS_LOADING_TRUE");
      MainService.post("Account/signup", {
        name: this.name,
        email: this.email,
        password: this.password,
        phoneNumber: this.phoneNumber,
        domain: this.domain,
        language: this.lang,
      })
        .then((response) => {
          this.$toasted.success(this.$t("welcome"));

          this.$store.commit("PUSH_USER", response.data);
          cookies.setCookie("bearer", response.data.token, "27d");

          MainService.defaults.headers.common["Authorization"] =
            "Bearer " + response.data.token;
          UserService.getUser()
            .then((response) => {
              this.$store.commit("PUSH_USER", response.data);
            })
            .catch((error) => {
              console.log(error);
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));

          if (this.role) {
            this.$router.push({
              name: "completeRegistrationWithRole",
              params: { role: this.role },
            });
          } else {
            this.$router.push({ name: "completeRegistration" });
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  computed: {},
};
</script>

<style scoped></style>
