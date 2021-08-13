<template>
  <div class="card mb-0 p-4">
    <b-form @submit="onSubmit" v-if="companyInfo">
      <div class="row">
        <div class="col-6">
          <b-form-group :label="$t('company_name')">
            <b-form-input
              v-model="companyInfo.name"
              type="text"
              :placeholder="$t('company_name')"
            ></b-form-input>
          </b-form-group>
          <b-form-group :label="$t('legal_address')">
            <b-form-input
              v-model="companyInfo.legalAddress"
              type="text"
              :placeholder="$t('legal_address')"
            ></b-form-input>
          </b-form-group>
          <b-form-group :label="$t('mailing_address')">
            <b-form-input
              type="text"
              v-model="companyInfo.mailingAddress"
              :placeholder="$t('mailing_address')"
            ></b-form-input>
          </b-form-group>
          <div class="row">
            <div class="col-6">
              <b-form-group :label="$t('itn')">
                <b-form-input
                  type="text"
                  v-model="companyInfo.taxpayerNumber"
                  :placeholder="$t('itn')"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-6">
              <b-form-group :label="$t('bin')">
                <b-form-input
                  v-model="companyInfo.bankIdentificationCode"
                  type="text"
                  :placeholder="$t('bic_text')"
                ></b-form-input>
              </b-form-group>
            </div>
          </div>
        </div>

        <div class="col-6">
          <b-form-group :label="$t('company_group')">
            <b-form-input
              v-model="companyInfo.companyGroup"
              type="text"
              :placeholder="$t('company_group')"
            ></b-form-input>
          </b-form-group>

          <div class="row">
            <div class="col-6">
              <b-form-group :label="$t('telephone')">
                <b-form-input
                  v-model="companyInfo.phoneNumber"
                  type="tel"
                  :placeholder="$t('telephone')"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-6">
              <b-form-group :label="$t('website')">
                <b-form-input
                  type="text"
                  v-model="companyInfo.site"
                  :placeholder="$t('website')"
                ></b-form-input>
              </b-form-group>
            </div>
          </div>
  <b-row>
    <b-col>
                <b-form-group label="Email">
            <b-form-input
              v-model="companyInfo.email"
              type="email"
            ></b-form-input>
          </b-form-group>
    </b-col>
    <b-col>              <b-form-group :label="$t('organization_type')">
                        <b-form-select v-model="companyInfo.organizationType" :options="options">
                  </b-form-select>
              </b-form-group></b-col>
  </b-row>

          <div class="row">
            <div class="col-6">
              <b-form-group :label="$t('checking_account')">
                <b-form-input
                  type="text"
                  v-model="companyInfo.checkingAccount"
                  :placeholder="$t('checking_account')"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-6">
              <b-form-group :label="$t('correspondent_account')">
                <b-form-input
                  v-model="companyInfo.correspondentAccount"
                  type="text"
                  :placeholder="$t('correspondent_account')"
                ></b-form-input>
              </b-form-group>
            </div>
          </div>
        </div>

        <div class="col-12">
          <b-form-group :label="$t('about_company')">
            <b-form-textarea
              :class="{ 'input-invalid': $v.companyInfo.about.$error }"
              @blur="$v.companyInfo.about.$touch()"
              id="about-company"
              name="about"
              v-model="companyInfo.about"
              :placeholder="$t('cv_placeholder')"
              rows="3"
              :state="
                companyInfo.about &&
                  companyInfo.about.length < 400 &&
                    companyInfo.about.length != 0
              "
            ></b-form-textarea>
            <p
              class="m-0 lh-1-5 error fs-13 text-red"
              v-if="$v.companyInfo.about.$error"
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
import { mapState } from "vuex";

const textValidator = (value) =>
  value == null || (value.indexOf("http") === -1 && value.indexOf("@") === -1);
export default {
  props: ["companyInfo"],
  validations: {
    companyInfo: {
      about: {
        textValidator,
      },
    },
  },
  components: {},
  data() {
    return {
      options:[
          { value: 'BUSINESS', text: this.$t("business") },
          { value: 'ORGANIZATION', text: this.$t("organization") },
          { value: 'SOLETRADER', text: this.$t("soletrader") },
      ]
    };
  },
  mounted() {},
  methods: {
    onSubmit(evt) {
      evt.preventDefault();
      this.$v.$touch();

      if (!this.$v.$invalid) {
        this.$store.commit("IS_LOADING_TRUE");
        UserService.postCompanyInfo({
          name: this.companyInfo.name,
          legalAddress: this.companyInfo.legalAddress,
          mailingAddress: this.companyInfo.mailingAddress,
          taxpayerNumber: this.companyInfo.taxpayerNumber,
          checkingAccount: this.companyInfo.checkingAccount,
          correspondentAccount: this.companyInfo.correspondentAccount,
          bankIdentificationCode: this.companyInfo.bankIdentificationCode,
          phoneNumber: this.companyInfo.phoneNumber,
          site: this.companyInfo.site,
          email: this.companyInfo.email,
          organizationType: this.companyInfo.organizationType,
          about: this.companyInfo.about,
          companyGroup: this.companyInfo.companyGroup,
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
  },
  watch: {},
};
</script>

<style scoped>

</style>
