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
        <b-breadcrumb-item>{{
          $t("approval_of_conditions")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("execution")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("feedback_exchange") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("completion") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>

    <div>
      <div class="bg-white py-3 px-4 my-1">
        <p class="h2 m-0">{{ $t("project_progress_info") }}</p>
      </div>
      <b-row>
        <b-col cols="9">
          <div
            v-if="
              $store.state.user.id === progress.executorId &&
                contract.isPrepaymentMade !== true
            "
            class="card p-4"
            style="border: 2px solid #fdac41"
          >
            <div class="d-flex justify-content-between my-3">
              <div class="flex-shrink mr-4">
                {{ $t("accepted_by_both") }}
                <b> {{ $t("please_dont_work") }}</b> {{ $t("only_in_case") }}
              </div>
              <b-alert show dismissible variant="warning w-100 ">
                <i class="bx bx-error-circle align-middle"></i>
                {{ $t("absence_of_payment") }}
              </b-alert>
            </div>
          </div>
          <template v-if="$store.state.user.id === contractorInfo.userId">
            <div
              v-if="
                contract.isPrepayment === true &&
                  contract.isPrepaymentMade !== true
              "
              class="card border-orange p-4"
            >
              <div class="d-flex my-3">
                <img src="/Assets/platform/icons/message-alert.svg" alt="" />
                <div class="mx-5">
                  {{ $t("for_executor_to_start") }}
                  <span class="orange font-weight-bold">
                    {{ contract.prepaymentValue }} EUR
                  </span>
                  <div>{{ $t("after_payment_we_notify_contractor") }}</div>
                </div>
              </div>
              <mango-pay
                :sum="contract.prepaymentValue"
                :type="'Prepayment'"
                :contractId="id"
              />
              <div class="card-footer bg-white border-0 text-right p-0">
                <b-button v-b-modal="'modal-customer-payment'" class="b-blue">
                  {{ $t("pay_now") }}
                </b-button>
              </div>
            </div>
            <div
              v-if="
                contract.paymentType === 'Safe' &&
                  contract.isReservationMade !== true
              "
              class="card p-4"
              style="border: 2px solid red"
            >
              <div class="d-flex justify-content-between my-3">
                <div>
                  Please pay the reservation
                  {{ parseFloat(contract.sumOfReservation).toFixed(0) }}€
                </div>

                <mango-pay
                  :sum="contract.sumOfReservation"
                  :type="'Reserve'"
                  :contractId="id"
                />
                <b-button
                  v-b-modal="'modal-customer-payment'"
                  variant="success text-nowrap"
                >
                  {{ $t("pay_now") }}
                </b-button>
              </div>
            </div>
          </template>

          <div class="card p-4">
            <p class="h3 mb-3 text-uppercase">{{ $t("object_data") }}</p>

            <div>
              <p>
                <img
                  class="mr-4"
                  src="/Assets/platform/icons/location.svg"
                  alt=""
                />
                <b class="blue"> {{ $t("contract_address") }}: </b>
                {{ contract.address }}
              </p>
              <p>
                <img
                  class="mr-4"
                  src="/Assets/platform/icons/customer.svg"
                  alt=""
                />
                <b class="blue"> {{ $t("customer_status") }}: </b>
                <span v-if="profile.companyName === null">
                  {{ $t("individual") }}</span
                >
                <span v-else> {{ $t("legal_entity") }}</span>
              </p>
              <p>
                <img
                  class="mr-4"
                  src="/Assets/platform/icons/categories.svg"
                  alt=""
                />
                <b class="blue"> {{ $t("category") }}: </b>
                {{ contract.categories }}
              </p>
              <p>
                <img
                  class="mr-4"
                  src="/Assets/platform/icons/budget-calc.svg"
                  alt=""
                />
                <b class="blue"> {{ $t("current_safe_balance") }}: </b>
                <span v-if="contract.isPrepaymentMade !== true" class="orange">
                  {{ $t("awaiting_payment") }}
                </span>
              </p>
              <b-row class="card-footer bg-white border-0">
                <b-col v-if="contract.buildStart">
                  <p class="h6 mb-3">{{ $t("start_of_construction") }}</p>
                  <span class="tenderstart p-2 mx-2">
                    <i class="bx bx-calendar"></i>
                    {{ contract.buildStart | moment("DD.MM.YYYY") }}
                  </span>
                </b-col>
                <b-col v-if="contract.plannedBuildEnd">
                  <p class="h6 mb-3">{{ $t("completion_of_construction") }}</p>
                  <span class="tenderend p-2 mx-2">
                    <i class="bx bx-calendar"></i>
                    {{ contract.plannedBuildEnd | moment("DD.MM.YYYY") }}
                  </span>
                </b-col>
                <b-col>
                  <p class="h6 mb-3">{{ $t("contract_budget") }}</p>
                  <span v-if="contract.price" class="budget p-2 mx-2">
                    <i class="bx bx-euro"></i>
                    {{ parseFloat(contract.price).toFixed(2) }}
                  </span>
                  <span v-else class="budget p-2 mx-2">
                    <i class="bx bx-euro"></i>
                    {{ $t("before") }}
                    {{ getBudgetById(contract.balanceId).value }}
                  </span>
                </b-col>
                <b-col>
                  <p class="h6 mb-3">{{ $t(contract.paymentType) }}, EUR</p>
                  <span class="budget p-2 mx-2">
                    <img src="/Assets/platform/icons/safe.svg" alt="" />
                    {{ contract.prepaymentValue }}
                  </span>
                </b-col>
              </b-row>
              <div
                class="mt-4 d-flex align-items-center justify-content-between"
              >
                <div class="blue ml-2">
                  <a
                    v-if="contractorInfo && contractorInfo.estimateFileName"
                    type="button"
                    class="blue"
                    target="_blank"
                    :href="'/docs/src/' + contractorInfo.estimateFileName"
                  >
                    <span> {{ $t("project_files") }} </span>
                    <img src="/Assets/platform/icons/xls.svg" alt="" />
                    <img src="/Assets/platform/icons/download.svg" alt="" />
                  </a>
                </div>

                <div
                  class="d-flex align-items-center flex-wrap"
                  v-if="contractorInfo && contractorInfo.files"
                >
                  <a
                    v-for="file in contractorInfo.files"
                    :key="file.name"
                    type="button"
                    class="btn btn-blue-outline m-1"
                    target="_blank"
                    :href="'/docs/src/' + file"
                  >
                    <span>{{ file }} </span>
                    <img src="/Assets/platform/icons/xls.svg" alt="" />
                    <img src="/Assets/platform/icons/download.svg" alt="" />
                  </a>
                </div>
              </div>
            </div>
          </div>
          <div
            v-if="contract.cooperationType === 'IndependentMeasurements'"
            class="card p-4">
            <p class="h3 mb-3 text-uppercase">{{ $t("results_for_today") }}</p>
            <b-row>
              <b-col>
                <b-row class="align-items-center">
                  <b-col cols="2">
                    <img src="/Assets/platform/icons/check.svg" alt="" />
                  </b-col>
                  <b-col class="px-1" cols="10">
                    <p class="mb-1">
                      <b class="blue"> {{ $t("done") }}:</b>
                      {{ objectData.works }}%
                    </p>
                    <p class="mb-1">
                      <b class="orange"> {{ $t("remains_to_execute") }}:</b>
                      {{ objectData.worksLeft }}%
                    </p></b-col
                  >
                </b-row>
              </b-col>
              <b-col>
                <b-row class="align-items-center">
                  <b-col cols="2">
                    <img src="/Assets/platform/icons/wallet.svg" alt="" />
                  </b-col>
                  <b-col class="px-1" cols="10">
                    <p class="mb-1">
                      <b class="blue"> {{ $t("paid_up") }}:</b>
                      {{ objectData.paidPercent }}%/{{ objectData.paidMoney }}€
                    </p>
                    <p class="mb-1">
                      <b class="orange"> {{ $t("to_pay") }}:</b>
                      {{ objectData.paidPercentLeft }}%/{{
                        objectData.paidMoneyLeft
                      }}€
                    </p></b-col
                  >
                </b-row>
              </b-col>
              <b-col>
                <b-row class="align-items-center">
                  <b-col cols="2">
                    <img src="/Assets/platform/icons/invoice.svg" alt="" />
                  </b-col>
                  <b-col class="px-1" cols="10">
                    <p class="mb-1">
                      <b class="blue"> {{ $t("invoiced") }}:</b>
                      {{ objectData.invoiced }}%
                    </p>
                    <p class="mb-1">
                      <b class="orange"> {{ $t("not_invoiced") }}:</b>
                      {{ objectData.invoicedLeft }}%
                    </p>
                  </b-col>
                </b-row>
              </b-col>
              <b-col>
                <b-row class="align-items-center">
                  <b-col cols="2">
                    <img src="/Assets/platform/icons/deadline.svg" alt="" />
                  </b-col>
                  <b-col class="px-1" cols="10">
                    <p class="mb-1">
                      <b class="blue"> {{ $t("deadline_passed") }}:</b>
                      {{ objectData.term }}%
                    </p>
                    <p class="mb-1">
                      <b class="orange"> {{ $t("time_left") }}:</b>
                      {{ objectData.termLeft }}%
                    </p>
                  </b-col>
                </b-row>
              </b-col>
            </b-row>
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
          <div class="card p-4">
            <p class="h4 mb-3">{{ $t("general_info") }}</p>
            <div class="row">
              <div class="col-4">
                <label class="h5 mt-4" for="selectedEstimateName">{{
                  $t("selected_estimate")
                }}</label>
                <p class="h6" id="selectedEstimateName">
                  {{ contract.estimateName }}
                </p>
                <label class="h5 mt-4" for="selectedEstimateVersion">{{
                  $t("version")
                }}</label>
                <p class="h6" id="selectedEstimateVersion">
                  v. {{ selectVersionId }}
                </p>
              </div>
              <div class="col-4">
                <label class="h5 mt-4">
                  {{ $t("editions_of_estimates") }}
                </label>
                <b-form-radio-group v-model="selectVersionId" stacked>
                  <a
                    :href="'/docs/src/' + contract.estimateId"
                    type="button"
                    class="btn btn-outline-info m-1"
                    target="_blank"
                    name="versions"
                  >
                    <i class="bx bxs-file-pdf align-middle"></i>
                    {{ contract.estimateName }}
                    <i class="bx bx-download align-middle"></i>
                  </a>
                  <a
                    :href="'/docs/src/' + version"
                    type="button"
                    class="btn btn-outline-info m-1"
                    target="_blank"
                    v-for="version in versions"
                    :key="version.fileName"
                    name="versions"
                  >
                    <i class="bx bxs-file-pdf align-middle"></i>
                    {{ contract.estimateName }} v. {{ version }}
                    <i class="bx bx-download align-middle"></i>
                  </a>
                </b-form-radio-group>
              </div>
              <div class="col-4">
                <label class="h5 mt-4">
                  {{ $t("versions_of_ac") }}
                </label>
                <div v-for="am in amlist" :key="am.id">
                  version {{ am.id }} {{ am.status }}
                  <router-link
                    v-if="
                      $store.state.user.id === contractorInfo.userId &&
                        am.status === 'Published'
                    "
                    :to="{
                      name: 'AutoMeasurement',
                      params: { id: am.id, contractId: id },
                    }"
                  >
                    View and evaluate
                  </router-link>
                  <router-link
                    v-if="
                      $store.state.user.id === progress.executorId &&
                        (am.status === 'Rejected' || am.status === 'Created')
                    "
                    :to="{
                      name: 'AutoMeasurement',
                      params: { id: am.id, contractId: id },
                    }"
                  >
                    View and edit
                  </router-link>
                </div>
                <b-button
                  v-if="$store.state.user.id === progress.executorId"
                  variant="warning"
                  @click="createMeasurment"
                >
                  {{ $t("create_autocalculation") }}
                </b-button>
              </div>
            </div>
          </div>
          <div class="card p-4" v-if="estimate">
            <b-table-simple bordered no-border-collapse hover responsive>
              <b-thead head-variant="light">
                <b-tr>
                  <b-th>№</b-th>
                  <b-th>{{ $t("description") }}</b-th>
                  <b-th>{{ $t("units") }}</b-th>
                  <b-th>{{ $t("quantity") }}</b-th>
                  <b-th>{{ $t("unit_price") }}</b-th>
                  <b-th>{{ $t("value") }}</b-th>
                  <b-th>{{ $t("notes") }}</b-th>
                  <b-th>{{ $t("progress") }}</b-th>
                </b-tr>
              </b-thead>

              <b-tbody>
                <estimateLine
                  v-for="section in estimate.sections"
                  :key="section.id"
                  :contract="contract"
                  :section="section"
                  :isParent="true"
                  :actionsShow="false"
                  :isSummary="true"
                  :estimateId="contract.masterEstimateId"
                />
              </b-tbody>
              <!-- <b-tfoot>
                <b-tr>
                  <b-th colspan="2" class="text-right">TOTAL </b-th>
                  <b-td colspan="3"></b-td>
                  <b-td colspan="2"
                    >{{ parseFloat(countTotal()).toFixed(2) }} €</b-td
                  >
                </b-tr>
              </b-tfoot> -->
            </b-table-simple>
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
                  v-if="progress.finishCustomer === true"
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
                v-if="progress.finishExecutor === true"
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

            <b-button class="b-blue my-2" @click="finishContract">{{
              $t("сontract_completed")
            }}</b-button>
            <a
              href="mailto:geral@diga.pt?subject=Suporte%20de%20contrato:Executing%20"
              target="_blank"
              class="btn b-orange-outline my-2"
              >{{ $t("contact_support") }}</a
            >
            <b-button
              v-if="$store.state.user.id === contractorInfo.userId"
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
import { mapGetters, mapState } from "vuex";
import ContractService from "@/services/ContractService.js";
import MeasurementService from "@/services/MeasurementService.js";
import UserService from "@/services/UserService.js";
import chat from "../Chat.vue";
import history from "./History.vue";
import refusal from "./Refusal.vue";
import estimateLine from "./EstimateLine.vue";
import EstimateService from "@/services/EstimateService.js";
import MangoPay from "./MangoPay.vue";

export default {
  props: ["id"],

  components: { chat, history, refusal, estimateLine, MangoPay },

  data() {
    return {
      errors: {},
      selected: "chat",
      options: [
        { text: this.$t("chat"), value: "chat" },
        { text: this.$t("changes_history"), value: "history" },
      ],
      contract: {},
      progress: {},
      executorCard: {},
      author: {},
      contractorInfo: null,
      refuseModalShow: false,
      objectData: {},
      selectVersionId: 0,
      versions: [],
      amlist: [],
      profile: null,
      estimate: null,
    };
  },
  methods: {
    getEstimate(estimateId) {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getEstimate(estimateId)
        .then((response) => {
          this.estimate = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    CompanyInfo() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCompanyInfo()
        .then((response) => {
          this.profile = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getMeasurments() {
      this.$store.commit("IS_LOADING_TRUE");
      MeasurementService.getMeasurments(this.id)
        .then((response) => {
          this.amlist = response.data;
          // if (this.versions.length > 0) {
          //   let latestVersion = Math.max(...this.versions);
          //   this.selectVersionId = latestVersion;
          // }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    createMeasurment() {
      this.$store.commit("IS_LOADING_TRUE");
      MeasurementService.createMeasurment(this.id)
        .then((response) => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({
            name: "AutoMeasurement",
            params: { id: response.data.id, contractId: this.id },
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getEstimateVersions() {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getEstimateVersions(this.id)
        .then((response) => {
          this.versions = response.data.versionIds;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContractExecution() {
      ContractService.getContractExecution(this.id)
        .then((response) => {
          this.objectData = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    finishContract() {
      ContractService.finishContract(this.id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getProgress();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    completeContract() {
      if (
        this.progress.finishCustomer === true &&
        this.progress.finishExecutor === true
      ) {
        this.$router.push({ name: "Reviews" });
      }
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
          this.completeContract();
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
        case "Signing":
          this.$router.push({
            name: "Signing",
            params: { id: this.id },
          });
          break;
        case "Refusal":
          this.$router.push({
            name: "ContractHistory",
            params: { id: this.id },
          });
          break;

        case "Executing":
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
          if (this.contract.mainEstimateId > 0) {
            this.getEstimate(this.contract.mainEstimateId);
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getContract();
    this.getContractExecution();
    this.getContractor();
    this.getProgress();
    this.getEstimateVersions();
    this.getMeasurments();
    this.CompanyInfo();
  },
  computed: {
    ...mapGetters(["getContractTypeById", "getBudgetById"]),
    ...mapState(["categories"]),
  },
};
</script>
