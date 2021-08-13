<template>
  <div v-if="contract && $store.state.user">
    <div class="d-inline-block my-4">
      <span class="h4 mr-3"> {{ $t("contract") }}: {{ contract.name }} </span>
      <span class="ctype fs-14"
        >{{ $t("contract_type") }}:
        <template v-if="getContractTypeById(contract.contractTypeId)">
          {{ $t(getContractTypeById(contract.contractTypeId).name) }}
        </template>
      </span>
    </div>
    <template>
      <b-breadcrumb class="my-0">
        <b-breadcrumb-item
          class=""
          :to="{ name: 'home_index' }"
        >
          <b-icon
            icon="house-fill"
            scale="1.25"
            shift-v="1.25"
            aria-hidden="true"
          ></b-icon>
        </b-breadcrumb-item>
        <b-breadcrumb-item
          class=""
          :to="{ name: 'contracts' }"
          >{{ $t("contracts") }}</b-breadcrumb-item
        >

        <b-breadcrumb-item>{{ $t("choosing_contractor") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("estimate_approval") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{
          $t("approval_of_conditions")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("execution")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("feedback_exchange") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("completion") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>
    <div class="container-fluid bg-grey">
      <b-row>
        <b-col cols="9">
          <div class="card p-4" style="border: 2px solid #fdac41">
            <div class="d-flex justify-content-between my-3">
              <div class="mr-4">
                {{ $t("to_set_auto") }}
              </div>
              <b-alert show dismissible variant="warning">
                <i class="bx bx-error-circle align-middle"></i>
                {{ $t("deadlines_for_autocalculation") }}
              </b-alert>
            </div>
          </div>
          <div class="card p-4">
            <p class="h4 mb-3">{{ $t("autocalculation_data") }}</p>
          </div>
          <div class="card p-4" v-if="contract && measurement.estimate">
            <b-table-simple bordered no-border-collapse hover responsive>
              <b-thead head-variant="light">
                <b-tr>
                  <b-th colspan="3"> ARTICULADO </b-th>
                  <b-th colspan="3"> Contrato </b-th>
                  <b-th colspan="3"> AUTO Nº {{ measurement.version }} </b-th>
                  <b-th colspan="2"> ACUMULADO ATUAL </b-th>
                  <b-th colspan="2"> SALDO ATUAL </b-th>
                  <b-th rowspan="2" v-if="measurement.status === 'Rejected'"
                    >Rejection reason</b-th
                  >
                  <b-th
                    colspan="3"
                    v-if="
                      $store.state.user &&
                      contractorInfo &&
                      $store.state.user.id === contractorInfo.userId
                    "
                  >
                    {{ $t("actions") }}
                  </b-th>
                </b-tr>
                <b-tr>
                  <b-th>№</b-th>
                  <b-th>{{ $t("description") }}</b-th>
                  <b-th>{{ $t("units") }}</b-th>
                  <b-th>{{ $t("quantity") }}</b-th>
                  <b-th>{{ $t("unit_price") }}</b-th>
                  <b-th>{{ $t("value") }}</b-th>
                  <b-th>%</b-th>
                  <b-th>QUANT. TOTAIS</b-th>
                  <b-th>{{ $t("value") }}</b-th>
                  <b-th>QUANT. TOTAIS</b-th>
                  <b-th>{{ $t("value") }}</b-th>
                  <b-th>QUANT. TOTAIS</b-th>
                  <b-th>{{ $t("value") }}</b-th>
                  <template
                    v-if="
                      $store.state.user &&
                      contractorInfo &&
                      $store.state.user.id === contractorInfo.userId
                    "
                  >
                    <b-th>STATUS</b-th>
                    <b-th>CONCLUÍDO</b-th>
                    <b-th>CANCELADO</b-th>
                  </template>
                </b-tr>
              </b-thead>

              <b-tbody>
                <estimateLine
                  v-for="section in measurement.estimate.sections"
                  :key="section.id"
                  :contract="contract"
                  :section="section"
                  :isParent="true"
                  :actionsShow="false"
                  :estimateId="contract.mainEstimateId"
                  :editPriceAndNotes="false"
                  :isAutoMeasurement="true"
                  :measurementReportId="id"
                  :isCustomer="$store.state.user.id === contractorInfo.userId"
                  :isRejected="measurement.status === 'Rejected'"
                />
              </b-tbody>
              <b-tfoot>
                <b-tr>
                  <b-th colspan="6" class="text-right">TOTAL </b-th>
                  <b-td
                    :colspan="
                      $store.state.user.id === contractorInfo.userId
                        ? 10
                        : measurement.status === 'Rejected'
                        ? 8
                        : 7
                    "
                    >{{ parseFloat(countTotal).toFixed(2) }} €</b-td
                  >
                </b-tr>
              </b-tfoot>
            </b-table-simple>
            <router-link
              :to="{
                name: 'Executing',
                params: { id: contractId },
              }"
            >
              Return
            </router-link>
            <template v-if="$store.state.user.id === progress.executorId">
              <template
                v-if="
                  measurement.status === 'Created' ||
                  measurement.status === 'Rejected'
                "
              >
                <button
                  v-if="measurement.status === 'Created'"
                  @click="deleteMeasurment"
                >
                  Cancel creation
                </button>
                <button v-if="!isSaved" @click="saveMeasurement('Created')">
                  Save draft
                </button>
                <button v-else @click="publishMeasurement">Publish</button>
              </template>
              <template v-if="measurement.status === 'Published'">
                <button @click="cancelPublication">Cancel publication</button>
              </template>
            </template>
            <template v-if="$store.state.user.id === contractorInfo.userId">
              <template v-if="measurement.status === 'Published'">
                <b-button
                  :disabled="isMeasurementAcceptedOrRejected !== true"
                  @click="
                    saveMeasurement(
                      isMeasurementAccepted === true ? 'Accepted' : 'Rejected'
                    )
                  "
                >
                  {{
                    isMeasurementAccepted === true ? "Accept" : "Reject"
                  }}</b-button
                >
              </template>
            </template>
          </div>
        </b-col>
        <b-col cols="3">
          <div class="card p-4">
            <div class="card-body p-0">
              <p class="h4 mb-3">{{ $t("customer") }}</p>
              <div
                v-if="author"
                class=" d-flex align-items-center"
              >
                <div class="mr-3 ml-0">
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
                <div>
                  <a href="#"
                    ><p class="h4 blue">{{ author.name }}</p></a
                  >
                  <div>
                   <span   v-b-tooltip.hover
                      :title="$t('Rating')" class="orange"
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
                <a
                v-b-tooltip.hover
          :title="$t('negative_reviews')"
                  href="#"

                  ><span class="grey-dark"
                    ><i class="bx bxs-dislike align-middle"></i>
                    {{ author.badReviews }}</span
                  ></a
                >
                  </div>

                  <div
                    v-if="measurement.status === 'Accepted'"
                    class="users-view-id text-success"
                  >
                    Accepted
                    <i class="bx bxs-check-square align-middle"></i>
                  </div>
                  <div
                    v-if="measurement.status === 'Published'"
                    class="users-view-id text-success"
                  >
                    Under consideration
                    <i class="bx bxs-check-square align-middle"></i>
                  </div>
                  <div
                    v-if="measurement.status === 'Rejected'"
                    class="text-danger"
                  >
                    Rejected
                    <i class="bx bxs-minus-circle align-middle"></i>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="card p-4">
            <div class="card-body p-0">
              <p class="h4 mb-3">{{ $t("executor") }}</p>
              <div class=" d-flex align-items-center">
                <div class="mr-3 ml-0">
                  <i
                    style="font-size: 60px"
                    width="60"
                    v-if="!executorCard.avatar"
                    class=" bx bx-user-circle nav-i"
                  ></i>
                  <div
                    v-else
                    class="avatar "
                    v-bind:style="{
                      backgroundImage:
                        'url(' + '/img/src/' + executorCard.avatar + ')',
                    }"
                  ></div>
                </div>
                <div>
                  <a href="#"
                    ><p class="h4 orange">{{ executorCard.name }}</p></a
                  >
                  <div>
                    <span   v-b-tooltip.hover
                      :title="$t('Rating')" class="orange"
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
                <a
                v-b-tooltip.hover
          :title="$t('negative_reviews')"
                  href="#"

                  ><span class="grey-dark"
                    ><i class="bx bxs-dislike align-middle"></i>
                    {{ executorCard.badReviews }}</span
                  ></a
                >

                
                    <div
                      v-if="measurement.status === 'Created'"
                      class="users-view-id text-success"
                    >
                      Created
                      <i class="bx bxs-check-square align-middle"></i>
                    </div>
                    <div
                      v-if="measurement.status === 'Published'"
                      class="users-view-id text-success"
                    >
                      Published
                      <i class="bx bxs-check-square align-middle"></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import ContractService from "@/services/ContractService.js";
import MeasurementService from "@/services/MeasurementService.js";
import UserService from "@/services/UserService.js";
import estimateLine from "./EstimateLine.vue";

export default {
  props: ["id", "contractId"],
  components: { estimateLine },
  data() {
    return {
      measurement: {},
      contract: {},
      progress: {},
      executorCard: {},
      author: {},
      contractorInfo: {},
      isSaved: false,
    };
  },

  methods: {
    cancelPublication() {
      this.$store.commit("IS_LOADING_TRUE");
      MeasurementService.cancelPublication(this.id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getMeasurment();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    publishMeasurement() {
      this.$store.commit("IS_LOADING_TRUE");
      MeasurementService.publishMeasurment(this.id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({
            name: "Executing",
            params: { id: this.contractId },
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    saveMeasurement(status) {
      this.$store.commit("IS_LOADING_TRUE");
      let mReport = this.$store.state.measurementReports.find(
        (e) => e.id === this.id
      );
      MeasurementService.editMeasurment(this.id, {
        contractId: this.contractId,
        status: status,
        positions:
          mReport != null
            ? mReport.positionsToUpdate.map((p) => {
                return {
                  id: p.id,
                  quantityCompleted: p.quantityCompleted,
                  status: p.status,
                  rejectionReason: p.rejectionReason,
                };
              })
            : [],
      })
        .then(() => {
          this.isSaved = true;
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$store.commit("CLEAN_MR", {
            id: this.id,
          });
          if (status === "Accepted" || status === "Rejected") {
            this.$router.push({
              name: "Executing",
              params: { id: this.contractId },
            });
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    deleteMeasurment() {
      this.$store.commit("IS_LOADING_TRUE");
      MeasurementService.deleteMeasurment(this.id)
        .then(() => {
          this.$router.push({
            name: "Executing",
            params: { id: this.contractId },
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getMeasurment() {
      this.$store.commit("IS_LOADING_TRUE");
      MeasurementService.getMeasurment(this.id)
        .then((response) => {
          this.measurement = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContractor() {
      ContractService.getContractor(this.contractId)
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
      ContractService.getProgress(this.contractId)
        .then((response) => {
          this.progress = response.data;
          this.getExecutorCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContract() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.contractId)
        .then((response) => {
          this.contract = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    positionMeasurementSum(position) {
      let sum = 0.0;
      if (position.measurementReportPositions.length) {
        let currentReportPosition = position.measurementReportPositions.find(
          (mp) => parseInt(mp.reportId) === parseInt(this.id)
        );
        if (currentReportPosition != null) {
          sum += currentReportPosition.quantityCompleted * position.price;
        }
      }
      return sum;
    },
    sectionMeasurementSumRecursive(section) {
      let sum = 0.0;
      if (section.positions && section.positions.length) {
        section.positions.forEach((position) => {
          sum += this.positionMeasurementSum(position);
        });
      }

      if (section.sections && section.sections.length) {
        section.sections.forEach((s) => {
          sum += this.sectionMeasurementSumRecursive(s);
        });
      }

      return sum;
    },
    isSectionAcceptedOrRejectedRecursive(section) {
      let result = true;

      if (section.positions && section.positions.length) {
        section.positions.forEach((position) => {
          if (position.measurementReportPositions.length) {
            let currentReportPosition = position.measurementReportPositions.find(
              (mp) => parseInt(mp.reportId) === parseInt(this.id)
            );
            if (
              currentReportPosition != null &&
              currentReportPosition.status === "Created" &&
              currentReportPosition.quantityCompleted > 0
            ) {
              result = false;
            }
          }
        });
      }

      if (section.sections && section.sections.length) {
        section.sections.forEach((s) => {
          if (this.isSectionAcceptedOrRejectedRecursive(s) === false) {
            result = false;
          }
        });
      }

      return result;
    },
    isSectionAcceptedRecursive(section) {
      let result = true;

      if (section.positions && section.positions.length) {
        section.positions.forEach((position) => {
          if (position.measurementReportPositions.length) {
            let currentReportPosition = position.measurementReportPositions.find(
              (mp) => parseInt(mp.reportId) === parseInt(this.id)
            );
            if (
              currentReportPosition != null &&
              currentReportPosition.status === "Rejected" &&
              currentReportPosition.quantityCompleted > 0
            ) {
              result = false;
            }
          }
        });
      }

      if (section.sections && section.sections.length) {
        section.sections.forEach((s) => {
          if (this.isSectionAcceptedRecursive(s) === false) {
            result = false;
          }
        });
      }

      return result;
    },
  },
  mounted() {
    this.getContract();
    this.getMeasurment();
    this.getProgress();
    this.getContractor();
  },
  computed: {
    ...mapGetters(["getContractTypeById", "getBudgetById"]),
    countTotal() {
      let sum = 0.0;
      if (
        this.measurement &&
        this.measurement.estimate &&
        this.measurement.estimate.sections.length
      ) {
        this.measurement.estimate.sections.forEach((s) => {
          sum += this.sectionMeasurementSumRecursive(s);
        });
      }

      return sum;
    },
    isMeasurementAcceptedOrRejected() {
      let result = true;
      if (
        this.measurement &&
        this.measurement.estimate &&
        this.measurement.estimate.sections.length
      ) {
        this.measurement.estimate.sections.forEach((s) => {
          result = this.isSectionAcceptedOrRejectedRecursive(s);
        });
      }
      return result;
    },
    isMeasurementAccepted() {
      let result = true;
      if (
        this.measurement &&
        this.measurement.estimate &&
        this.measurement.estimate.sections.length
      ) {
        this.measurement.estimate.sections.forEach((s) => {
          result = this.isSectionAcceptedRecursive(s);
        });
      }
      return result;
    },
  },
};
</script>

<style scoped></style>
