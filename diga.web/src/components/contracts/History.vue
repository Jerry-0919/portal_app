<template>
  <div>
    <p class="h5 my-4 font-weight-bold">{{ $t("timeline") }}</p>
    <ul class="widget-timeline">
      <li class="timeline-items success" v-for="h in history" :key="h[0]">
        <p class="timeline-time">
          {{ h[0] | moment("Do MMMM YYYY  h:mm:ss a") }}
        </p>
        <div v-for="(el, index) in h[1]" :key="el.case + index">
          <p class="h6 timeline-title">{{ $t(el.case) }}</p>
          <p
            v-if="el.case === 'ApproveEstimate' && el.to === 'True'"
            class="timeline-content"
          >
            {{ el.userName }} {{ $t("approved_history") }}
            {{ $t("estimate") }}
          </p>
          <p
            v-if="el.case === 'Refuse' && el.to === ''"
            class="timeline-content"
          >
            {{ el.userName }} {{ $t("refused_cooperate") }}
          </p>
          <p
            v-if="el.case === 'Refuse' && el.to !== ''"
            class="timeline-content"
          >
            {{ el.userName }} {{ $t("refused_cooperate") }}
            {{ $t("reason_refusal") }}: {{ $t(el.to) }}.
          </p>
          <p
            v-if="el.case === 'ApproveContract' && el.to === 'True'"
            class="timeline-content"
          >
            {{ el.userName }} {{ $t("approved_history") }}
            {{ $t("contract") }}
          </p>
          <p
            v-if="el.case === 'ApproveSigning' && el.to === 'True'"
            class="timeline-content"
          >
            {{ el.userName }} {{ $t("approved_history") }}
            {{ $t("contract") }}
          </p>
          <p
            v-if="el.case === 'Finish' && el.to === 'True'"
            class="timeline-content"
          >
            {{ el.userName }}
            {{ $t("successfully_finished_contract") }}
          </p>
          <p
            v-if="
              el.case === 'EstimateName' ||
                el.case === 'EditSigning' ||
                el.case === 'Description' ||
                el.case === 'Price' ||
                el.case === 'PaymentType' ||
                el.case === 'GeneralTerm' ||
                el.case === 'ComissionType' ||
                el.case === 'PaymentFrequency' ||
                el.case === 'CooperationType'
            "
            class="timeline-content"
          >
            {{ $t("сhange_from") }}{{ el.from }} {{ $t("сhange_to") }}
            {{ el.to }} {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>
          <p v-if="el.case === 'PlatformVATId'" class="timeline-content">
            {{ $t("choice") }} {{ $t("vat") }} {{ el.to }}
            {{ $t("by_the_user") }} {{ el.userName }}
          </p>

          <p v-if="el.case === 'AddPosition'" class="timeline-content">
            {{ $t("the_position") }} {{ el.from }} {{ $t("has_been_added") }}
            {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>
          <p v-if="el.case === 'AddSection'" class="timeline-content">
            {{ $t("section") }} {{ el.from }} {{ $t("has_been_added") }}
            {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>
          <p v-if="el.case === 'AddStep'" class="timeline-content">
            {{ $t("payment_stage") }} {{ el.from }} {{ $t("has_been_added") }}
            {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>

          <p v-if="el.case === 'EditPosition'" class="timeline-content">
            {{ $t("the_position") }} {{ el.from }} {{ $t("has_been_added") }}
            {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>
          <p v-if="el.case === 'EditSection'" class="timeline-content">
            {{ $t("section") }} {{ el.from }} {{ $t("has_been_added") }}
            {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>
          <p
            v-if="
              el.case === 'StepName' ||
                el.case === 'StepValue' ||
                el.case === 'StepDate'
            "
            class="timeline-content"
          >
            {{ $t("payment_stage") }} {{ $t("сhange_from") }} {{ el.from }}
            {{ $t("сhange_to") }} {{ el.to }} {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>

          <p v-if="el.case === 'RemovePosition'" class="timeline-content">
            {{ $t("the_position") }} {{ el.from }} {{ $t("deleted") }}
            {{ $t("by_the_user") }} {{ el.userName }}
          </p>
          <p v-if="el.case === 'RemoveSection'" class="timeline-content">
            {{ $t("section") }} {{ el.from }} {{ $t("deleted") }}
            {{ $t("by_the_user") }} {{ el.userName }}
          </p>
          <p v-if="el.case === 'RemoveStep'" class="timeline-content">
            {{ $t("payment_stage") }} {{ el.from }} {{ $t("deleted") }}
            {{ $t("by_the_user") }}
            {{ el.userName }}
          </p>
          <p
            v-if="el.case === 'BuildStart' || el.case === 'PlannedBuildEnd'"
            class="timeline-content"
          >
            <template v-if="el.from !== '01.01.0001 00:00:00'">
              {{ $t("сhange_from") }}
              {{ el.from }}
            </template>
            {{ $t("сhange_to") }}
            {{ el.to }}
            {{ $t("by_the_user") }} {{ el.userName }}
          </p>
        </div>
      </li>
    </ul>
  </div>
</template>

<script>
import ContractService from "@/services/ContractService.js";
export default {
  props: ["id"],

  data() {
    return {
      history: {},
    };
  },
  methods: {
    getContractHistory() {
      ContractService.getContractHistory(this.id)
        .then((response) => {
          this.history = Object.entries(response.data);
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getContractHistory();
  },
};
</script>

<style scoped>
.widget-timeline li {
  padding: 1.1rem 0;
  list-style: none;
  position: relative;
}
.widget-timeline li.timeline-items.success:before {
  background: #39da8a !important;
}
.widget-timeline li.timeline-items:before {
  position: absolute;
  content: "";
  left: -37px;
  top: 17px;
  border: 3px solid #ffffff;
  box-shadow: 1px 2px 6px 0 rgb(25 42 70 / 30%);
  border-radius: 50%;
  background: #5a8dee;
  height: 13px;
  width: 13px;
  z-index: 2;
}
.widget-timeline li .timeline-time {
  float: right;
  color: #828d99;
  font-size: 0.9rem;
}
.timeline-title {
  font-family: "Rubik", Helvetica, Arial, serif;
  font-weight: 400;
  line-height: 1.2;
  color: #475f7b;
  font-size: 1rem;
}
.widget-timeline li.timeline-items .timeline-content {
  padding: 0.5rem 1rem;

  border-radius: 0.267rem;
  display: flex;
  align-items: center;
  color: #727e8c;
  font-size: 0.9rem;
}
.widget-timeline li.timeline-items:not(:last-child):after {
  position: absolute;
  content: "";
  width: 1px;
  background: #dfe3e7;
  left: -31px;
  top: 22px;
  height: 100%;
  z-index: 1;
}
</style>
