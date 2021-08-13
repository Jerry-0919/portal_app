<template>
  <div class="card mb-0 p-4">
    <b-form @submit="onSubmit" v-if="info">
      <div class="row">
        <div class="col-6">
                    <b-row>
            <b-col>          <b-form-group :label="$t('name')">
            <b-form-input
              v-model="info.name"
              type="text"
              :placeholder="$t('name')"
            ></b-form-input>
          </b-form-group></b-col>
            <b-col>                    <b-form-group :label="$t('surname')">
            <b-form-input
              v-model="info.surname"
              type="text"
              :placeholder="$t('surname')"
            ></b-form-input>
          </b-form-group></b-col>
          </b-row>


          <b-form-group :label="$t('mobile_phone')">
            <b-form-input
              v-model="info.mainPhoneNumber"
              type="tel"
              :placeholder="$t('mobile_phone')"
            ></b-form-input>
          </b-form-group>

          <b-form-group :label="$t('contract_address')">
            <b-form-input
              type="text"
              v-model="info.address"
              :placeholder="$t('contract_address')"
            ></b-form-input>
          </b-form-group>
                        <b-form-group :label="$t('country_of_residence')">
                <select
                  class="form-control"
                  name="residense"
                  v-model="info.residenceCountryId"
                >
                  <option
                    v-for="country in countries"
                    :value="country.id"
                    :key="country.id"
                    >{{ country.name }}</option
                  >
                </select>
              </b-form-group>

          <div class="row">
            <div class="col">
              <b-form-group :label="$t('education')">
                <b-form-input
                  type="text"
                  v-model="info.education"
                  :placeholder="$t('education')"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col">
              <b-form-group :label="$t('nationality')">
                <select
                  class="form-control"
                  name="nationality"
                  v-model="info.nationalityId"
                >
                  <option
                    v-for="country in countries"
                    :value="country.id"
                    :key="country.id"
                    >{{ country.name }}</option
                  >
                </select>
              </b-form-group>
            </div>
          </div>
        </div>

        <div class="col-6">
          <div class="row mb-3">
            <div class="col">
              <b-form-group :label="$t('dob')">
                <b-form-datepicker
                  v-model="info.dateOfBirth"
                  :max="beforeTwenty"
                  :locale="$root.locale"
                  :placeholder="$t('select_date')"
                  id="dob-datepicker"
                ></b-form-datepicker>
              </b-form-group>
            </div>
            <div class="col">
              <b-form-group :label="$t('custom_email')">
                <b-form-input
                  disabled
                  v-model="info.email"
                  type="email"
                ></b-form-input>
              </b-form-group>
            </div>
            <!-- CUSTOM EMAIL INPUT
  
              <b-form-group :label="$t('custom_email')">
              <b-form-input
                type="email"
                v-model="info.email"
                :placeholder="Email"
              ></b-form-input>
            </b-form-group> -->

            <div class="col-12">
              <p class="h6">{{ $t("phones") }}</p>
            </div>
            <div
              class="col-10 input-group mt-1 mb-2"
              v-for="phone in info.phoneNumbers"
              :key="phone.id"
            >
              <div class="input-group-prepend">
                <select class="custom-select" v-model="phone.type">
                  <option value="mobile">mobile</option>
                  <option value="home">home</option>
                  <option value="work">work</option>
                </select>
              </div>
              <input type="tel" v-model="phone.value" class="form-control" />
              <div class="input-group-append">
                <input
                  type="button"
                  value="x"
                  class="form-control"
                  @click="removePhone(phone)"
                />
              </div>
            </div>

            <div class="col-2 input-group mt-1 mb-2">
              <input
                type="button"
                class="btn btn-success btn-circle"
                @click="addPhone"
                value="+"
              />
            </div>
          </div>
          <b-form-group :label="$t('zipcode')">
            <b-form-input
              type="text"
              v-model="info.postalCode"
              :placeholder="$t('zipcode')"
            ></b-form-input>
          </b-form-group>
          <div class="row">
            <div class="col">
              <b-form-group :label="$t('languages')">
                <multiselect
                  v-model="info.languages"
                  :placeholder="$t('type_to_search')"
                  :options="languages"
                  :multiple="true"
                  track-by="id"
                  label="name"
                ></multiselect>
              </b-form-group>
            </div>
            <div class="col">
              <b-form-group :label="$t('status')">
                <select
                  @change="changeRole"
                  class="form-control"
                  v-model="role"
                >
                  <option value="PlatformExecutorMaster">{{
                    $t("specialist")
                  }}</option>
                  <option value="PlatformExecutorTeam">{{
                    $t("brigade")
                  }}</option>
                  <option value="PlatformExecutorCompany">{{
                    $t("company")
                  }}</option
                  >Î
                </select>
              </b-form-group>
            </div>
          </div>
        </div>
        <div class="col-12">
          <b-form-group :label="$t('cv')">
            <b-form-textarea
              :class="{ 'input-invalid': $v.info.resume.$error }"
              @blur="$v.info.resume.$touch()"
              id="cv-person"
              v-model="info.resume"
              name="resume"
              :placeholder="$t('cv_placeholder')"
              :state="info.resume.length < 400 && info.resume.length != 0"
              rows="3"
            ></b-form-textarea>
            <p
              class="m-0 lh-1-5 error fs-13 text-red"
              v-if="$v.info.resume.$error"
            >
              {{ $t("text_no_html") }}
            </p>
          </b-form-group>
        </div>

        <b-form-group>
          <b-button type="submit" variant="primary">{{ $t("save") }}</b-button>
        </b-form-group>
      </div>
    </b-form>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
import Multiselect from "vue-multiselect";
import { mapState } from "vuex";

const textValidator = (value) =>
  value == null || (value.indexOf("http") === -1 && value.indexOf("@") === -1);
export default {
  props: ["info", "roles"],
  validations: {
    info: {
      resume: {
        textValidator,
      },
    },
  },
  components: { Multiselect },
  data() {
    return {
      beforeTwenty: new Date().toISOString().substr(0, 10),
      role: "",
    };
  },
  mounted() {
    this.beforeTwenty = this.$moment()
      .add(-18, "y")
      .format("YYYY-MM-DD");
  },
  methods: {
    changeRole() {
      if (!this.roles.some((r) => r.role === this.role)) {
        UserService.postRole(this.role)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.removeRole("PlatformExecutorMaster");
            this.removeRole("PlatformExecutorTeam");
            this.removeRole("PlatformExecutorCompany");
            // this.roles = this.roles.filter((r) => {
            //   r.role !== "PlatformExecutorMaster" &&
            //     r.role !== "PlatformExecutorTeam" &&
            //     r.role !== "PlatformExecutorCompany";
            // });
            this.roles.push({ role: this.role });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    removeRole(role) {
      const index = this.roles.findIndex((r) => r.role === role);
      if (index > -1) {
        this.roles.splice(index, 1);
      }
    },
    removePhone(phone) {
      const index = this.info.phoneNumbers.indexOf(phone);
      if (index > -1) {
        this.info.phoneNumbers.splice(index, 1);
      }
    },
    addPhone() {
      this.info.phoneNumbers.push({
        type: "mobile",
        value: "",
      });
    },
    onSubmit(evt) {
      evt.preventDefault();

      this.$v.$touch();

      if (!this.$v.$invalid) {
        this.$store.commit("IS_LOADING_TRUE");
        UserService.editUser({
          avatar: this.info.avatar,
          name: this.info.name,
          surname: this.info.surname,
          email: this.info.email,
          dateOfBirth: this.info.dateOfBirth,
          address: this.info.address,
          residenceCountryId: this.info.residenceCountryId,
          education: this.info.education,
          postalCode: this.info.postalCode,
          nationalityId: this.info.nationalityId,
          language: this.info.language,
          resume: this.info.resume,
          mainPhoneNumber: this.info.mainPhoneNumber,
          phoneNumbers: this.info.phoneNumbers,
          languageIds: this.info.languages.map((l) => {
            return l.id;
          }),
        })
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
  },
  computed: {
    ...mapState(["countries", "cities", "categories", "tags", "languages"]),
    user() {
      return this.$store.state.user;
    },
  },
  watch: {
    roles() {
      if (this.roles && this.roles.length > 0) {
        this.roles.forEach((role) => {
          switch (role.role) {
            case "PlatformExecutorMaster":
              this.role = "PlatformExecutorMaster";
              break;
            case "PlatformExecutorTeam":
              this.role = "PlatformExecutorTeam";
              break;
            case "PlatformExecutorCompany":
              this.role = "PlatformExecutorCompany";
              break;
          }
        });
      }
    },
  },
};
</script>

<style scoped>

.btn-circle {
  /* width: 30px;
  height: 30px; */
  border-radius: 25px;
}
</style>
