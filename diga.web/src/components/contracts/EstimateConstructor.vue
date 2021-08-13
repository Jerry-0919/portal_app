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
        <b-breadcrumb-item>{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>
    <div>
      <div class="bg-white py-3 px-4 my-1">
        <p class="h2 m-0">{{ $t("general_info") }}</p>
      </div>
      <b-row>
        <b-col cols="9">
          <div class="card border-orange p-4">
            <b-row>
              <b-col>
                <b-form-group :label="$t('selected_estimate')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    :value="contract.estimateName"
                  ></b-form-input>
                </b-form-group>
                <b-form-group :label="$t('version')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    :value="'v.' + selectVersionId"
                  ></b-form-input>
                </b-form-group>
              </b-col>
              <b-col>
                <b-form-group :label="$t('editions_of_estimates')">
                  <b-form-radio-group v-model="selectVersionId" stacked>
                    <b-form-radio name="versions" :value="contract.estimateId">
                      {{ contract.estimateName }}
                    </b-form-radio>
                    <b-form-radio
                      v-for="version in versions"
                      :key="version.estimateId"
                      name="versions"
                      :value="version.estimateId"
                    >
                      {{ version.estimateName }}
                    </b-form-radio>
                  </b-form-radio-group>
                </b-form-group>
              </b-col>
            </b-row>
          </div>

          <div class="card p-4">
            <b-table-simple
              bordered
              no-border-collapse
              responsive
              sticky-header
            >
              <b-thead head-variant="light">
                <b-tr>
                  <b-th>№</b-th>
                  <b-th>{{ $t("description") }}</b-th>
                  <b-th>{{ $t("units") }}</b-th>
                  <b-th>{{ $t("quantity") }}</b-th>
                  <b-th>{{ $t("unit_price") }}</b-th>
                  <b-th>{{ $t("value") }}</b-th>
                  <b-th>{{ $t("notes") }}</b-th>
                  <b-th colspan="3" v-if="actionsShow === true">{{
                    $t("actions")
                  }}</b-th>
                </b-tr>
              </b-thead>

              <b-tbody>
                <estimateLine
                  v-for="section in estimate.sections"
                  :key="section.id"
                  :contract="contract"
                  :section="section"
                  :isParent="true"
                  :actionsShow="actionsShow"
                  :estimateId="selectVersionId"
                  :editPriceAndNotes="false"
                  @deleteChildSection="deleteChildSection"
                  @saveDraft="saveDraft"
                />
              </b-tbody>
              <b-tfoot>
                <b-tr>
                  <b-th colspan="2" class="text-right">TOTAL </b-th>
                  <b-td colspan="3"></b-td>
                  <b-td colspan="2"
                    >{{ parseFloat(countTotal()).toFixed(2) }} €</b-td
                  >
                </b-tr>
              </b-tfoot>
            </b-table-simple>
          </div>
          <div class="card p-4">
            <p class="h3 mb-3">{{ $t("other_information") }}</p>
            <div class="d-flex justify-content-around">
              <b-form-group :label="$t('select_vat_mode')">
                <b-form-select
                  v-model="estimate.vatType"
                  :disabled="actionsShow === false"
                >
                  <option value="Private">
                    {{ $t("private") }}
                  </option>
                  <option value="NotCharged">
                    {{ $t("not_charged") }}
                  </option>
                  <option value="Simple">
                    {{ $t("business") }}
                  </option>
                </b-form-select>
              </b-form-group>

              <b-form-group
                v-if="estimate.vatType === 'Simple'"
                :label="$t('vat')"
              >
                <b-form-select
                  v-model="platformVatId"
                  :disabled="actionsShow === false"
                >
                  <option value="null">{{ $t("browse") }}...</option>
                  <option
                    v-for="vat in vats.filter(
                      (v) => v.countryId === this.contract.countryId
                    )"
                    :key="vat.id"
                    :value="vat.id"
                  >
                    {{ vat.percent }}% - {{ vat.name }}
                  </option>
                </b-form-select>
              </b-form-group>
              <div v-if="estimate.vatType === 'NotCharged'">
                {{ $t("not_charged") }} (%)
                <i
                  class="blue bx bxs-help-circle align-middle"
                  v-b-tooltip.hover
                  :title="$t('vat_by_contractor')"
                ></i>
              </div>
              <template v-if="estimate.vatType === 'Private'">
                <b-form-group :label="$t('labor') + ' (6%)'">
                  <b-form-input
                    :disabled="actionsShow === false"
                    v-model="laborPercent"
                    type="number"
                    min="0"
                    max="100"
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('Material') + ' (23%)'">
                  <b-form-input
                    :disabled="actionsShow === false"
                    v-model="materialPercent"
                    type="number"
                    min="0"
                    max="100"
                  >
                  </b-form-input>
                </b-form-group>
              </template>
            </div>
          </div>
          <div class="card p-4">
            <p class="h3 mb-3">{{ $t("summary_data") }}</p>
            <div class="d-flex justify-content-around">
              <template v-if="estimate.vatType === 'Private'">
                <b-form-group :label="$t('total_no_vat')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="estimate.price.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('vat_on_labor')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="vatOnLabor.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('vat_on_material')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="vatOnMaterial.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('total_com_vat')">
                  <b-form-input
                    class="orange-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="totalWithVat.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
              </template>
              <template v-if="estimate.vatType === 'NotCharged'">
                <b-form-group :label="$t('total_no_vat')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="estimate.price.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('total_com_vat')">
                  <b-form-input
                    class="orange-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="estimate.price.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
              </template>
              <template v-if="estimate.vatType === 'Simple'">
                <b-form-group :label="$t('total_no_vat')">
                  <b-form-input
                    class="blue-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="estimate.price.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('total_com_vat')">
                  <b-form-input
                    class="orange-disabled"
                    disabled
                    type="number"
                    min="0"
                    :value="empresa.toFixed(2)"
                  >
                  </b-form-input>
                </b-form-group>
              </template>
            </div>
          </div>
          <div class="card p-4">
            <p class="h3 mb-3">{{ $t("contract_management") }}</p>
            <div class="d-flex justify-content-around">
              <b-button
                v-if="actionsShow === false"
                class="btn b-blue-outline my-2"
                @click="editEstimate"
                >{{ $t("edit_estimate") }}</b-button
              >
              <b-button
                v-else
                class="btn b-blue-outline my-2"
                @click="
                  actionsShow = false;
                  saveDraft();
                "
                >{{ $t("save_draft") }}</b-button
              >
              <b-button class="b-orange-outline my-2" @click="saveAsNewDraft">{{
                $t("save_as_new_version")
              }}</b-button>
              <b-button class="b-blue my-2" @click="postEstimateApproved">{{
                $t("confirm_estimate")
              }}</b-button>

              <b-button
                v-if="$store.state.user.id === contractorInfo.userId"
                @click="refuse()"
                class="btn b-orange my-2"
                >{{ $t("refuse_cooperate") }}</b-button
              >
              <b-button v-else @click="refuseQuick()" class="b-orange my-2">{{
                $t("refuse_cooperate")
              }}</b-button>
              <refusal :id="id" />
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
                      'b-orange-option-active':
                        selected === 'history',
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
                  v-if="progress.estimateCustomer === true"
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
                v-if="progress.estimateExecutor === true"
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
              v-if="actionsShow === false"
              class="b-blue-outline my-2"
              @click="editEstimate"
              >{{ $t("edit_estimate") }}</b-button
            >
            <b-button
              v-else
              class="b-blue-outline my-2"
              @click="
                saveDraft();
                actionsShow = false;
              "
              >{{ $t("save_draft") }}</b-button
            >
            <b-button class="b-orange-outline my-2" @click="saveAsNewDraft">{{
              $t("save_as_new_version")
            }}</b-button>

            <b-button
              :disabled="
                ($store.state.user.id === contractorInfo.userId &&
                  progress.estimateCustomer === true) ||
                  (progress.estimateExecutor === true &&
                    $store.state.user.id === progress.executorId)
              "
              class="b-blue my-2"
              @click="postEstimateApproved"
              >{{ $t("confirm_estimate") }}</b-button
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
          </div>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import ContractService from "@/services/ContractService.js";
import EstimateService from "@/services/EstimateService.js";
import UserService from "@/services/UserService.js";
import { mapGetters, mapState } from "vuex";
import estimateLine from "./EstimateLine.vue";
import history from "./History.vue";
import refusal from "./Refusal.vue";
import chat from "../Chat.vue";

export default {
  props: ["id"],
  components: {
    estimateLine,
    refusal,
    history,
    chat,
  },
  data() {
    return {
      selected: "history",
      chatAndHistory: [
        { text: this.$t("chat"), value: "chat" },
        { text: this.$t("changes_history"), value: "history" },
      ],
      platformVatId: 0,
      laborPercent: null,
      materialPercent: null,
      progress: {},
      executorCard: {},
      contract: {},
      versions: [],
      selectVersionId: 0,
      estimate: {
        contractId: this.id,
        daysNumber: null,
        price: null,
        text: "",
        sections: [],
        userName: "",
        userAvatar: "",
      },
      author: {},
      contractorInfo: null,
      actionsShow: false,
    };
  },
  methods: {
    editEstimate() {
      if (
        this.progress.estimateCustomer === true ||
        this.progress.estimateExecutor === true
      ) {
        if (confirm(this.$t("are_you_sure_estimate_edit"))) {
          this.actionsShow = true;
          this.progress.estimateCustomer = false;
          this.progress.estimateExecutor = false;
        }
      } else {
        this.actionsShow = true;
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
    refuse() {
      if (confirm(this.$t("are_you_sure_refuse"))) {
        this.$bvModal.show("refuseModalShow");
      }
    },

    getEstimateApproved() {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getEstimateApproved(this.id)
        .then((response) => {
          this.estimate = response.data;
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    postEstimateApproved() {
      if (this.actionsShow === true) {
        this.$store.commit("IS_LOADING_TRUE");
        this.saveDraftPromise()
          .then(() => {
            this.$toasted.success(this.$t("Saved"));
            this.getEstimate();
            this.$store.commit("CLEAN_ESTIMATE", {
              estimateId: this.selectVersionId,
            });
            EstimateService.postEstimateApproved(this.id, this.selectVersionId)
              .then(() => {
                this.$toasted.success(
                  this.$t("your_request_was_successfully_sent")
                );
                this.getProgress();
                this.getContract();
              })
              .catch(() => {
                this.$toasted.error(this.$t("oops_error"));
              });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      } else {
        this.$store.commit("IS_LOADING_TRUE");
        EstimateService.postEstimateApproved(this.id, this.selectVersionId)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.getProgress();
            this.getContract();
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    saveAsNewDraft() {
      let estimateToSave = this.$store.state.estimates.find(
        (e) => e.id === this.selectVersionId
      );
      // if (estimateToSave) {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.cloneEstimateApproved(this.selectVersionId, {
        platformEstimateVats: this.getPlatformEstimateVats(),
        vatType: this.estimate.vatType,
        sectionsToAdd:
          estimateToSave == null ? [] : estimateToSave.sectionsToAdd,
        sectionsToUpdate:
          estimateToSave == null ? [] : estimateToSave.sectionsToUpdate,
        sectionIdsToRemove:
          estimateToSave == null ? [] : estimateToSave.sectionIdsToRemove,
        positionsToAdd:
          estimateToSave == null ? [] : estimateToSave.positionsToAdd,
        positionsToUpdate:
          estimateToSave == null ? [] : estimateToSave.positionsToUpdate,
        positionIdsToRemove:
          estimateToSave == null ? [] : estimateToSave.positionIdsToRemove,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getEstimateVersions();
          this.$store.commit("CLEAN_ESTIMATE", {
            estimateId: this.selectVersionId,
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      // }
    },
    getPlatformEstimateVats() {
      let platformEstimateVats = [];
      if (this.estimate.vatType === "Simple") {
        platformEstimateVats.push({
          platformEstimateId: this.selectVersionId,
          platformVATId: this.platformVatId,
          percent: 100,
        });
      } else if (this.estimate.vatType === "Private") {
        platformEstimateVats.push({
          platformEstimateId: this.selectVersionId,
          platformVATId: this.vats.find((v) => v.percent === 6).id,
          percent: this.laborPercent || 0,
        });
        platformEstimateVats.push({
          platformEstimateId: this.selectVersionId,
          platformVATId: this.vats.find((v) => v.percent === 23).id,
          percent: this.materialPercent || 0,
        });
      }
      return platformEstimateVats;
    },
    saveDraft() {
      this.saveDraftPromise()
        .then(() => {
          this.$toasted.success(this.$t("Saved"));
          this.getEstimate();
          this.$store.commit("CLEAN_ESTIMATE", {
            estimateId: this.selectVersionId,
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    saveDraftPromise() {
      let estimateToSave = this.$store.state.estimates.find(
        (e) => e.id === this.selectVersionId
      );
      this.$store.commit("IS_LOADING_TRUE");

      return EstimateService.editEstimateApproved(this.selectVersionId, {
        platformEstimateVats: this.getPlatformEstimateVats(),
        vatType: this.estimate.vatType,
        sectionsToAdd:
          estimateToSave == null ? [] : estimateToSave.sectionsToAdd,
        sectionsToUpdate:
          estimateToSave == null ? [] : estimateToSave.sectionsToUpdate,
        sectionIdsToRemove:
          estimateToSave == null ? [] : estimateToSave.sectionIdsToRemove,
        positionsToAdd:
          estimateToSave == null ? [] : estimateToSave.positionsToAdd,
        positionsToUpdate:
          estimateToSave == null ? [] : estimateToSave.positionsToUpdate,
        positionIdsToRemove:
          estimateToSave == null ? [] : estimateToSave.positionIdsToRemove,
      });
    },
    getEstimateVersions() {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getEstimateVersions(this.id)
        .then((response) => {
          this.versions = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getEstimate() {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getEstimate(this.selectVersionId)
        .then((response) => {
          this.estimate = response.data;
          if (this.estimate.vatType === null) {
            this.estimate.vatType = "Private";
          }
          if (this.estimate.platformEstimateVats.length > 0) {
            if (this.estimate.vatType === "Private") {
              this.laborPercent = this.estimate.platformEstimateVats[0].percent;
              this.materialPercent = this.estimate.platformEstimateVats[1].percent;
            }
            if (this.estimate.vatType === "Simple") {
              this.platformVatId = this.estimate.platformEstimateVats[0].platformVATId;
            }
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => {
          this.$store.commit("IS_LOADING_FALSE");
        });
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
    deleteChildSection(section) {
      const index = this.estimate.sections.indexOf(section);
      if (index > -1) {
        this.estimate.sections.splice(index, 1);
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
          this.getProgress();
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
        // case "EstimateApproval":
        //   this.$router.push({
        //     name: "EstimateApproval",
        //     params: { id: this.id },
        //   });
        //   break;
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
          this.selectVersionId = this.contract.mainEstimateId;
          this.getEstimateVersions();
          this.checkContractStatus();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    countTotalRecursive(section) {
      let total = 0.0;
      if (section && section.positions) {
        section.positions.forEach((p) => {
          if (p.quantity && p.price) {
            total += p.quantity * p.price;
          }
        });
      }

      if (section && section.sections) {
        section.sections.forEach((s) => {
          total += this.countTotalRecursive(s);
        });
      }

      return total;
    },
    countTotal() {
      let total = 0.0;

      if (
        this.estimate &&
        this.estimate.sections &&
        this.estimate.sections.length
      ) {
        this.estimate.sections.forEach((s) => {
          total += this.countTotalRecursive(s);
        });
      }
      this.estimate.price = total;
      return total;
    },
  },
  mounted() {
    this.getContract();
    this.getContractor();
  },
  computed: {
    empresa() {
      if (
        this.estimate.price &&
        this.estimate.vatType === "Simple" &&
        this.estimate.platformVATId
      ) {
        let vat = this.vats.find((v) => v.id === this.estimate.platformVATId);
        if (vat !== null) {
          return (
            this.estimate.price + this.estimate.price * (vat.percent / 100)
          );
        }
      }
      return 0;
    },
    vatOnLabor() {
      if (this.estimate.price && this.estimate.vatType === "Private") {
        return this.estimate.price * (this.laborPercent / 100) * 0.06;
      }
      return 0;
    },
    vatOnMaterial() {
      if (this.estimate.price && this.estimate.vatType === "Private") {
        return this.estimate.price * (this.materialPercent / 100) * 0.23;
      }
      return 0;
    },
    totalWithVat() {
      if (
        this.estimate.price &&
        this.estimate.vatType === "Private" &&
        this.vatOnLabor &&
        this.vatOnMaterial
      ) {
        return this.estimate.price + this.vatOnLabor + this.vatOnMaterial;
      }
      return 0;
    },
    ...mapGetters([
      "getCityById",
      "getCountryById",
      "getContractPriorityById",
      "getContractTypeById",
      "getBudgetById",
      "getVatById",
    ]),
    ...mapState(["vats"]),
  },
  watch: {
    selectVersionId() {
      this.getEstimate();
    },
  },
};
</script>

<style scoped>
.b-table-sticky-header {
  max-height: 900px !important;
}
div::-webkit-scrollbar {
  height: 7px;
}

div::-webkit-scrollbar-thumb {
  background-color: #438d90;

  webkit-box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.5);
}
</style>
