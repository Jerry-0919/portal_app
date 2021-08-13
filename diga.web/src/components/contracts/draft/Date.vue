<template>
  <form @submit.prevent="submit">
    <p class="h3 my-3">{{ $t("date") }}</p>
    <b-row>
      <b-col>
        <b-form-group :label="$t('publication_of_a_contract')">
          <b-form-datepicker
            v-model="contract.publishDate"
            :min="beforeToday"
            :locale="$root.locale"
            :placeholder="$t('select_date')"
            id="publication-datepicker"
            class="mb-2"
            @change="errors.publishDate = false"
            :class="{ 'input-invalid': errors.publishDate }"
          ></b-form-datepicker>
        </b-form-group>
        <b-form-group :label="$t('start_of_construction')">
          <b-form-datepicker
            :min="beforeToday"
            :locale="$root.locale"
            :placeholder="$t('select_date')"
            v-model="contract.buildStart"
            id="start-datepicker"
            class="mb-2"
          ></b-form-datepicker>
        </b-form-group>
      </b-col>
      <b-col>
        <b-form-group :label="$t('end_of_competition')">
          <b-form-datepicker
            @change="errors.tenderEndDate = false"
            :class="{ 'input-invalid': errors.tenderEndDate }"
            v-model="contract.tenderEndDate"
            :min="competition"
            :locale="$root.locale"
            :placeholder="$t('select_date')"
            id="end-datepicker"
            class="mb-2"
          ></b-form-datepicker>
        </b-form-group>
        <b-form-group :label="$t('completion_of_construction')">
          <b-form-datepicker
            :min="beforeToday"
            :locale="$root.locale"
            :placeholder="$t('select_date')"
            v-model="contract.plannedBuildEnd"
            id="completion-datepicker"
            class="mb-2"
          ></b-form-datepicker>
        </b-form-group>
      </b-col>
    </b-row>

    <div class="d-flex justify-content-between my-3">
      <b-button class="btn b-grey-outline" @click="$emit('TabPrev')">{{
        $t("previous")
      }}</b-button>
      <b-button @click="$emit('Cancel')" class="btn b-orange-outline">{{
        $t("cancel_post")
      }}</b-button>
      <b-button class="btn b-blue" type="submit">{{ $t("next") }}</b-button>
    </div>
  </form>
</template>

<script>
import ContractService from "@/services/ContractService.js";

export default {
  props: ["contract"],
  components: {},
  data() {
    const beforePost = new Date();
    beforePost.setDate(beforePost.getDay() + 7);

    return {
      publication: new Date().toISOString().slice(0, 10),
      beforeToday: new Date().toISOString().substr(0, 10),
      competition: beforePost,
      errors: {},
    };
  },
  methods: {
    validateForm() {
      if (this.contract.publishDate && this.contract.tenderEndDate) {
        return true;
      }
      this.errors = {};
      if (!this.contract.publishDate) {
        this.errors.publishDate = true;
      }
      if (!this.contract.tenderEndDate) {
        this.errors.tenderEndDate = true;
      }

      return false;
    },

    submit() {
      if (this.validateForm() === false) {
        this.$toasted.error(this.$t("fill_in_the_fields"));
      } else {
        this.$store.commit("IS_LOADING_TRUE");
        ContractService.saveDates(this.contract.id, {
          contractId: this.contract.id,
          publishDate: this.contract.publishDate,
          tenderEndDate: this.contract.tenderEndDate,
          buildStart: this.contract.buildStart,
          plannedBuildEnd: this.contract.plannedBuildEnd,
        })
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$emit("TabNext");
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
  },
};
</script>
<style></style>
