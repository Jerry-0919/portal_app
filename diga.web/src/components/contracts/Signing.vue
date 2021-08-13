<template>
  <div class="m-3" v-if="contract && contractorInfo && $store.state.user">
    <div class="orange-header px-4 py-3 d-flex">
      <span class="h1"> {{ $t("contract") }}: {{ contract.name }}</span>
      <div v-if="contract.contractTypeId" class="ml-auto d-flex contract-type">
        <span class="ctype fs-14">
          {{ $t(getContractTypeById(contract.contractTypeId).name) }}
        </span>
        <div class="trapezoid-left"></div>
      </div>
    </div>
    <template>
      <b-breadcrumb class="my-0 t3">
        <b-breadcrumb-item class=" blue" :to="{ name: 'home_index' }">
          <i class="bx bx-home"></i>
        </b-breadcrumb-item>

        <b-breadcrumb-item class="blue">{{ $t("Draft") }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue" disabled>{{
          $t("publication")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("choosing_contractor")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("estimate_approval")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("approval_of_conditions")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>

    <div>
      <div class="bg-white py-3 px-4 my-1">
        <p class="h2 m-0">{{ $t("Signing") }}</p>
      </div>
      <b-row>
        <b-col cols="9">
          <div class="card p-4">
            <b-form-group>
              <div
                v-if="isEdit === false"
                class="my-2"
                v-html="signing.text"
              ></div>
              <vue-editor
                v-else
                @input="
                  errors.text = false;
                  errors.minlength = false;
                "
                :class="{
                  'input-invalid': errors.text || errors.minlength,
                }"
                v-model="signing.text"
              >
              </vue-editor>
              <p class="fs-13" v-if="signing.text">
                {{ signing.text.length }} / {{ 400 }} ({{ $t("minlength") }}400)
              </p>
              <p class="error text-red" v-if="errors.minlength">
                {{ $t("minlength") }}400
              </p>
            </b-form-group>

            <b-form-group v-if="isEdit === false" :label="$t('read_terms')">
              <b-form-radio
                v-model="signed"
                class="blue"
                name="termsradio"
                :value="true"
                >{{ $t("terms_yes") }}</b-form-radio
              >
              <b-form-radio
                v-model="signed"
                class="orange"
                name="termsradio"
                :value="false"
                >{{ $t("terms_no") }}</b-form-radio
              >
            </b-form-group>
          </div>

          <div v-if="contract.isPrepayment === true" class="card p-4">
            <template
              v-if="
                $store.state.user.id === progress.executorId &&
                  $store.state.currentRole === 'Executor' &&
                  signing.prepaymentInvoiceFile === null
              "
            >
              <b class="h5 mb-3">
                {{ $t("to_sign_upload_invoice") }}
                <span class="orange">{{ contract.prepaymentValue }} EUR </span>
              </b>
              <b-form-group>
                <b-form-file
                  :disabled="isEdit === false"
                  v-model="file"
                  :placeholder="$t('choose_file')"
                  :browse-text="$t('browse')"
                  accept=".pdf, .jpeg"
                ></b-form-file>
                <p v-if="file">{{ file.name }}</p>
              </b-form-group>
            </template>
            <div v-if="signing.prepaymentInvoiceFile !== null">
              <p class="h5 font-weight-bold">{{ $t("prepayment_invoice") }}</p>
              <p v-if="file">{{ file.name }}</p>
              <p v-if="!signing.prepaymentInvoiceFile">
                {{ $t("file_not_uploaded_yet") }}
              </p>
              <a
                v-if="signing.prepaymentInvoiceFile"
                type="button"
                class="btn btn-outline-primary my-3"
                target="_blank"
                :href="'/docs/src/' + signing.prepaymentInvoiceFile"
                ><i class="bx bxs-file align-middle"></i
                >{{ signing.prepaymentInvoiceFileName }}
                <i class="bx bx-download align-middle"></i>
              </a>
              <span
                v-if="
                  isEdit === true &&
                    $store.state.user.id === progress.executorId &&
                    $store.state.currentRole === 'Executor'
                "
                class="remove-photo btn text-red"
                @click="removeInvoice"
                >x</span
              >
            </div>
          </div>

          <div class="card p-4">
            <div>
              <b-form-group label-class="p-4" :label="$t('contract')">
                <b-button-group
                  class="d-flex justify-content-between py-2 px-4 bg_grey"
                >
                  <b-button
                    :class="{
                      'b-blue-option-active': selected === 'chat',
                    }"
                    class="b-option"
                    @click="selected = 'chat'"
                  >
                    {{ $t("chat") }}
                  </b-button>
                  <b-button
                    :class="{
                      'b-orange-option-active': selected === 'history',
                    }"
                    class="b-option ml-1"
                    @click="selected = 'history'"
                  >
                    {{ $t("changes_history") }}
                  </b-button>
                </b-button-group>
              </b-form-group>
            </div>
            <div v-if="selected === 'history'">
              <history :id="id" />
            </div>
            <div v-else>
              <chat />
            </div>
          </div>
        </b-col>
        <b-col cols="3">
          <div class="card border-blue bg_grey">
            <div class="card-body p-0">
              <div v-if="author" class=" d-flex align-items-center m-3">
                <div>
                  <i
                    style="font-size: 60px"
                    width="60"
                    v-if="!author.avatar"
                    class=" bx bx-user-circle nav-i"
                  ></i>
                  <div
                    v-else
                    class="avatar "
                    v-bind:style="{
                      backgroundImage:
                        'url(' + '/img/src/' + author.avatar + ')',
                    }"
                  ></div>
                </div>
                <div class="mx-4">
                  <p class="h3">{{ $t("customer") }}</p>
                  <router-link class="blue" :to="{ name: 'profile' }">{{
                    author.fullName
                  }}</router-link>
                </div>
              </div>
              <div
                class="card-footer bg-white border-0 d-flex justify-content-around pb-4"
              >
                <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
                  ><i class="bx bxs-star align-middle"></i
                  >{{ author.rating }}</span
                >

                <a
                  v-b-tooltip.hover
                  :title="$t('positive_reviews')"
                  class="mx-1"
                  href="#"
                  ><span class="blue"
                    ><i class="bx bxs-like  align-middle"></i>
                    {{ author.goodReviews }}</span
                  >
                </a>
                <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
                  ><span class="grey-dark"
                    ><i class="bx bxs-dislike align-middle"></i>
                    {{ author.badReviews }}</span
                  ></a
                >
              </div>
              <div class="px-4 pt-0 pb-4 bg-white t1">
                <div
                  v-if="progress.signingCustomer === true"
                  class="confirmed p-2 text-center"
                >
                  <i class="bx bx-check-circle align-middle"></i>
                  {{ $t("conditions_confirmed") }}
                </div>
                <div v-else class="not-confirmed p-2 text-center">
                  <i class="bx bxs-minus-circle align-middle"></i>
                  {{ $t("under_consideration") }}
                </div>
              </div>
            </div>
          </div>
          <div class="card border-orange bg_grey">
            <div class="d-flex align-items-center m-3">
              <div>
                <i
                  style="font-size: 60px"
                  width="60"
                  v-if="!executorCard.avatar"
                  class=" bx bx-user-circle nav-i"
                ></i>
                <div
                  v-else
                  class="avatar"
                  v-bind:style="{
                    backgroundImage:
                      'url(' + '/img/src/' + executorCard.avatar + ')',
                  }"
                ></div>
              </div>
              <div class="mx-4">
                <p class="h3">{{ $t("executor") }}</p>
                <router-link class="orange" :to="{ name: 'profile' }">{{
                  executorCard.fullName
                }}</router-link>
              </div>
            </div>
            <div
              class="card-footer bg-white border-0 d-flex justify-content-around pb-4"
            >
              <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
                ><i class="bx bxs-star align-middle"></i
                >{{ executorCard.rating }}</span
              >

              <a
                v-b-tooltip.hover
                :title="$t('positive_reviews')"
                class="mx-1"
                href="#"
                ><span class="blue"
                  ><i class="bx bxs-like  align-middle"></i>
                  {{ executorCard.goodReviews }}</span
                >
              </a>
              <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
                ><span class="grey-dark"
                  ><i class="bx bxs-dislike align-middle"></i>
                  {{ executorCard.badReviews }}</span
                ></a
              >
            </div>
            <div class="px-4 pt-0 pb-4 bg-white t1">
              <div
                v-if="progress.signingExecutor === true"
                class="confirmed p-2 text-center"
              >
                <i class="bx bx-check-circle align-middle"></i>
                {{ $t("conditions_confirmed") }}
              </div>
              <div v-else class="not-confirmed p-2 text-center">
                <i class="bx bxs-minus-circle align-middle"></i>
                {{ $t("under_consideration") }}
              </div>
            </div>
          </div>

          <div class="card p-4">
            <p class="h3">{{ $t("contract_management") }}</p>
            <b-button
              v-if="isEdit === false"
              class="b-blue-outline my-2"
              @click="isEdit = !isEdit"
              >{{ $t("edit") }}</b-button
            >
            <b-button
              v-else
              class="b-orange-outline my-2"
              @click="editContractSigning(), (isEdit = false)"
              >{{ $t("save_draft") }}</b-button
            >

            <b-button
              :disabled="signed === false"
              class="b-blue my-2"
              @click="approveContractSigning"
              >{{ $t("confirm_conditions") }}</b-button
            >

            <b-button
              v-if="$store.state.user.id === this.contractorInfo.userId"
              @click="refuse()"
              class="b-orange my-2"
              >{{ $t("refuse_cooperate") }}</b-button
            >
            <b-button v-else @click="refuseQuick()" class="b-orange my-2">{{
              $t("refuse_cooperate")
            }}</b-button>
            <refusal :id="id" :refuseModalShow="refuseModalShow" />
          </div>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import { VueEditor } from "vue2-editor";
import { mapGetters } from "vuex";
import ContractService from "@/services/ContractService.js";
import UserService from "@/services/UserService.js";
import refusal from "./Refusal.vue";
import chat from "../Chat.vue";
import history from "./History.vue";
import SigningService from "@/services/SigningService.js";
import FileService from "@/services/FileService.js";

export default {
  props: ["id"],
  components: {
    VueEditor,
    chat,
    history,
    refusal,
  },
  data() {
    return {
      file: null,
      errors: {},
      selected: "history",
      options: [
        { text: this.$t("chat"), value: "chat" },
        { text: this.$t("changes_history"), value: "history" },
      ],
      signed: false,
      isEdit: false,
      signing: {
        text: "",
        prepaymentInvoiceFile: null,
        prepaymentInvoiceFileName: null,
      },
      contract: {},
      progress: {},
      executorCard: {},
      author: {},
      contractorInfo: null,
      refuseModalShow: false,
    };
  },
  methods: {
    removeInvoice() {
      this.$store.commit("IS_LOADING_TRUE");
      this.signing.prepaymentInvoiceFile = null;
      this.signing.prepaymentInvoiceFileName = null;
      this.$store.commit("IS_LOADING_FALSE");
    },
    uploadFile() {
      this.$store.commit("IS_LOADING_TRUE");
      let formData = new FormData();
      formData.append("file", this.file);
      this.$store.commit("IS_LOADING_FALSE");
      return FileService.uploadFile(formData);
    },
    refuse() {
      if (confirm(this.$t("are_you_sure_refuse"))) {
        this.refuseModalShow = true;
      }
    },
    refuseQuick() {
      if (confirm(this.$t("are_you_sure_refuse"))) {
        ContractService.refuse(this.id)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$router.push({ name: "allContracts" });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },

    getContractSigning() {
      this.$store.commit("IS_LOADING_TRUE");
      SigningService.getContractSigning(this.id)
        .then((response) => {
          this.signing = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    editContractSigning() {
      this.$store.commit("IS_LOADING_TRUE");
      if (
        this.contract.isPrepayment === true &&
        this.$store.state.user.id === this.progress.executorId
      ) {
        this.uploadFile()
          .then((response) => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.signing.prepaymentInvoiceFile = response.data.fileName;
            this.signing.prepaymentInvoiceFileName = this.file.name;
            SigningService.editContractSigning(this.id, {
              text: this.signing.text,
              prepaymentInvoiceFile: this.signing.prepaymentInvoiceFile,
              prepaymentInvoiceFileName: this.signing.prepaymentInvoiceFileName,
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
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      } else {
        SigningService.editContractSigning(this.id, {
          text: this.signing.text,
        })
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.getProgress();
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    approveContractSigning() {
      SigningService.approveContractSigning(this.id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getProgress();
          this.getContract();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    getContractor() {
      ContractService.getContractor(this.id)
        .then((response) => {
          this.contractorInfo = response.data;
          this.getCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getCard() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCard(this.contractorInfo.userId)
        .then((response) => {
          this.author = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getExecutorCard() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCard(this.progress.executorId)
        .then((response) => {
          this.executorCard = response.data;
          if (
            this.contractorInfo.userId !== this.$store.state.user.id &&
            this.progress.executorId !== this.$store.state.user.id
          ) {
            this.$toasted.error(this.$t("access_denied"));
            this.$router.push({ name: "allContracts" });
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getProgress() {
      ContractService.getProgress(this.id)
        .then((response) => {
          this.progress = response.data;
          this.getExecutorCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    checkContractStatus() {
      switch (this.contract.status) {
        case "Draft":
          this.$router.push({
            name: "Draft",
            params: { id: this.id },
          });
          break;
        case "Deferred":
        case "Archived":
          this.$router.push({
            name: "allContracts",
          });
          break;
        case "Contractor":
          this.$router.push({
            name: "Contractor",
            params: { id: this.id },
          });
          break;
        case "EstimateApproval":
          this.$router.push({
            name: "EstimateApproval",
            params: { id: this.id },
          });
          break;
        case "ContractApproval":
          this.$router.push({
            name: "ContractApproval",
            params: { id: this.id },
          });
          break;
        // case "Signing":
        //   this.$router.push({
        //     name: "Signing",
        //     params: { id: this.id },
        //   });
        //   break;
        case "Refusal":
          this.$router.push({
            name: "ContractHistory",
            params: { id: this.id },
          });
          break;

        case "Executing":
          this.$router.push({
            name: "Executing",
            params: { id: this.id },
          });
          break;
        case "Closed":
          this.$router.push({
            name: "allContracts",
          });
          break;
        case "Finished":
          this.$router.push({
            name: "Finish",
            params: { id: this.id },
          });
          break;
      }
    },
    getContract() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.id)
        .then((response) => {
          this.contract = response.data;
          this.checkContractStatus();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getContract();
    this.getContractor();
    this.getProgress();
    this.getContractSigning();
  },
  computed: {
    ...mapGetters(["getContractTypeById"]),
  },
};
</script>
<style></style>
