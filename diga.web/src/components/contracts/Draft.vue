<template>
  <div>
    <div class="bg-white py-3 px-4 my-1">
      <p class="h2 m-0">{{ $t("fill_in_the_fields") }}</p>
    </div>
    <b-tabs class="draft" v-model="tabIndex" pills fill>
      <b-tab title-link-class="timeline">
        <template #title>
          <span
            class=""
            :class="{
              'text-uppercase': true,
              blue: 0 < tabIndex,
              orange: 0 === tabIndex,
            }"
          >
            {{ $t("basic_details") }}</span
          >
          <div :class="getTabClass(0)">
            <span :class="getTabIcon(0)"></span>
          </div>
        </template>

        <basicdetails
          class="mt-1 card p-4"
          :contract="contract"
          @TabNext="TabNext"
          @Cancel="Cancel"
        />
      </b-tab>

      <b-tab title-link-class="timeline">
        <template #title>
          <span
            class=""
            :class="{
              'text-uppercase': true,
              blue: 1 < tabIndex,
              orange: 1 === tabIndex,
              'grey-dark': 1 > tabIndex,
            }"
          >
            {{ $t("estimate") }}</span
          >
          <div :class="getTabClass(1)">
            <span :class="getTabIcon(1)"></span>
          </div>
        </template>

        <estimate
          class="mt-1 card p-4"
          :contract="contract"
          @TabNext="TabNext"
          @TabPrev="TabPrev"
          @Cancel="Cancel"
        />
      </b-tab>

      <b-tab title-link-class="timeline">
        <template #title>
          <span
            class=""
            :class="{
              'text-uppercase': true,
              blue: 2 < tabIndex,
              orange: 2 === tabIndex,
              'grey-dark': 2 > tabIndex,
            }"
          >
            {{ $t("price") }}</span
          >
          <div :class="getTabClass(2)">
            <span :class="getTabIcon(2)"></span>
          </div>
        </template>
        <price
          class="mt-1 card p-4"
          :contract="contract"
          @TabNext="TabNext"
          @TabPrev="TabPrev"
          @Cancel="Cancel"
        />
      </b-tab>

      <b-tab title-link-class="timeline">
        <template #title>
          <span
            class=""
            :class="{
              'text-uppercase': true,
              blue: 3 < tabIndex,
              orange: 3 === tabIndex,
              'grey-dark': 3 > tabIndex,
            }"
          >
            {{ $t("date") }}</span
          >
          <div :class="getTabClass(3)">
            <span :class="getTabIcon(3)"></span>
          </div>
        </template>
        <date
          class="mt-1 card p-4"
          :contract="contract"
          @TabNext="TabNext"
          @TabPrev="TabPrev"
          @Cancel="Cancel"
        />
      </b-tab>

      <b-tab title-link-class="timeline">
        <template #title>
          <span
            class=""
            :class="{
              'text-uppercase': true,
              blue: 4 < tabIndex,
              orange: 4 === tabIndex,
              'grey-dark': 4 > tabIndex,
            }"
          >
            {{ $t("terms_and_conditions") }}</span
          >
          <div :class="getTabClass(4)">
            <span :class="getTabIcon(4)"></span>
          </div>
        </template>
        <terms
          class="mt-1 card p-4"
          :contract="contract"
          @TabPrev="TabPrev"
          @Cancel="Cancel"
          @TabNext="TabNext"
        />
      </b-tab>
    </b-tabs>
  </div>
</template>

<script>
import basicdetails from "./draft/BasicDetails.vue";
import estimate from "./draft/Estimate.vue";
import price from "./draft/Price.vue";
import date from "./draft/Date.vue";
import terms from "./draft/TermsAndConditions.vue";
import ContractService from "@/services/ContractService.js";

export default {
  props: ["contract"],

  components: {
    basicdetails,
    estimate,
    price,
    date,
    terms,
  },
  data() {
    return {
      tabIndex: 0,
    };
  },
  methods: {
    TabNext() {
      console.log(this.tabIndex);
      if (this.tabIndex === 4) {
        this.$emit("switchParentTab");
      } else {
        this.tabIndex++;
      }
    },
    TabPrev() {
      this.tabIndex--;
    },
    Cancel() {
      if (confirm(this.$t("are_you_sure_remove_contract"))) {
        ContractService.Remove(this.contract.id)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$router.push({ name: "home_index" });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    getTabClass(index) {
      if (index < this.tabIndex) {
        return "draft-finished";
      }
      if (index === this.tabIndex) {
        return "draft-current";
      }
      if (index > this.tabIndex) {
        return "draft-future";
      }
    },
    getTabIcon(index) {
      if (index < this.tabIndex) {
        return "i bx bxs-check-square blue";
      }
      if (index === this.tabIndex) {
        return "i bx bxs-square orange";
      }
      if (index > this.tabIndex) {
        return "nothing";
      }
    },
    switchParentTab() {
      this.$emit("switchParentTab");
    },
  },
  watch: {
    contract() {
      if (this.tabIndex === 0) {
        if (this.contract.editStatus === "Info") {
          this.tabIndex = 1;
        }
        if (this.contract.editStatus === "Estimate") {
          this.tabIndex = 2;
        }
        if (this.contract.editStatus === "Price") {
          this.tabIndex = 3;
        }
        if (this.contract.editStatus === "Dates") {
          this.tabIndex = 4;
        }
      }
    },
  },
};
</script>
<style>
.nothing {
  padding-bottom: 2rem !important;
}
.nav,
.draft .nav-pills .nav-link.active {
  background-color: white !important;
}
.timeline {
  background: white;
  pointer-events: none;
  padding-left: 0px !important;
  padding-right: 0px !important;
  font-weight: 700;
}
.timeline .bx {
  font-size: xx-large;
}
.draft-finished,
.draft-current,
.draft-future {
  margin: 20px 0;
  display: flex;
  align-items: center;
  text-align: center;
}
.draft-finished::before,
.draft-finished::after,
.draft-current::before,
.draft-current::after,
.draft-future::before,
.draft-future::after {
  content: "";
  flex: 1;
}

.draft-current::before {
  border-bottom: 3px solid #f16b24;
}

.draft-current::after {
  border-bottom: 3px solid #f16b24;
}

.draft-finished::before,
.draft-finished:after {
  border-bottom: 3px solid #438d90;
}

.draft-future:before,
.draft-future::after {
  border-bottom: 1px solid #b4b4b4;
}
</style>
