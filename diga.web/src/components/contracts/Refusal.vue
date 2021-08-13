<template>
  <div>
    <b-modal id="refuseModalShow" hide-footer >
      <template #modal-header>
        {{ $t("refuse_case_text") }}
      </template>
      <b-form-group>
        <b-form-radio-group
          stacked
          v-model="refusalCase"
          :label="$t('select_from_list') + ':'"
        >
          <b-form-radio name="refusalCase" value="PlansChanged">
            {{ $t("PlansChanged") }}
          </b-form-radio>
          <b-form-radio name="refusalCase" value="ExecutorFailed">
            {{ $t("ExecutorFailed") }}</b-form-radio
          >
          <b-form-radio name="refusalCase" value="ExecutorIncorrectData">
            {{ $t("ExecutorIncorrectData") }}</b-form-radio
          >
          <b-form-radio name="refusalCase" value="FoundAnotherOnPlatform">
            {{ $t("FoundAnotherOnPlatform") }}</b-form-radio
          >
          <b-form-radio name="refusalCase" value="FoundAnother">
            {{ $t("FoundAnother") }}</b-form-radio
          >
          <b-form-radio name="refusalCase" value="ExecutorRefused">
            {{ $t("ExecutorRefused") }}</b-form-radio
          ></b-form-radio-group
        >
      </b-form-group>
      <b-button
        @click="confirmRefuse"
        class="float-right mt-4"
        variant="primary"
        :disabled="!refusalCase"
        type="submit"
        >{{ $t("confirm") }}</b-button
      >
    </b-modal>
  </div>
</template>

<script>
import ContractService from "@/services/ContractService.js";

export default {
  props: ["id"],
  data() {
    return {
      refusalCase: "",
    };
  },
  methods: {
    confirmRefuse() {
      ContractService.refuse(this.id, { refusalCase: this.refusalCase })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({ name: "allContracts" });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
};
</script>

<style scoped></style>
