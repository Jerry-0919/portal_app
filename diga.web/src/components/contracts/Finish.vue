<template>
  <div>
    <div v-if="contract" class="d-inline-block my-4">
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
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("Finished")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item
          class=" primary"
          :to="{ name: 'ContractHistory', params: { id: this.id } }"
          >{{ $t("contract_history") }}</b-breadcrumb-item
        >
      </b-breadcrumb>
    </template>
    <div class="container-fluid bg-grey">
      <div class="card p-4">
        <p class="h5 mb-3 text-center font-weight-bold">
          {{ $t("contract_finish") }}!
        </p>
        <div class="text-center my-4">
          <i
            class="bx bx-badge-check bx-flip-horizontal bx-burst success"
            style="font-size: 200px"
          ></i>
          <p class="mt-4">
            {{ $t("positive_review_is_public_viewing") }}, {{ $t("contract") }}
            <b>{{ contract.name }} </b>
            {{ $t("completed_as_successful") }}
          </p>
          <div
            v-if="this.$store.state.currentRole === 'Customer'"
            class="d-flex justify-content-around"
          >
            <router-link
              class="btn btn-primary"
              :to="{ name: 'contracts_create' }"
              ><i class="bx bx-plus align-middle"></i>
              {{ $t("publish_new_contract") }}
            </router-link>

            <b-button variant="success">
              <i class="bx bxs-dollar-circle align-middle"></i>
              {{ $t("offer_another_contract_to") }} {{ executorCard.name }}
            </b-button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import ContractService from "@/services/ContractService.js";
import UserService from "@/services/UserService.js";

export default {
  props: ["id"],
  data() {
    return {
      progress: {},
      executorCard: {},
      contract: {},
    };
  },
  methods: {
    getContract() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.id)
        .then((response) => {
          this.contract = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error ") + "contract");
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
          // this.$toasted.error(this.$t("oops_error") + "excard");
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
          this.$toasted.error(this.$t("oops_error") + "progress");
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },

  computed: {
    ...mapGetters(["getContractTypeById"]),
  },
  mounted() {
    this.getContract();
    this.getProgress();
  },
};
</script>

<style scoped></style>
