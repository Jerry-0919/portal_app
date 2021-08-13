<template>
  <form @submit.prevent="submit">
    <p class="h3 my-3">{{ $t("contract_budget") }}</p>
    <b-row>
      <b-col cols="4">
        <b-form-group :label="$t('specify_price')">
          <b-form-checkbox-group v-model="withPrice">
            <b-form-checkbox name="priceradio" :value="true">{{
              $t("yes")
            }}</b-form-checkbox>
            <b-form-checkbox name="priceradio" :value="false">{{
              $t("no")
            }}</b-form-checkbox>
          </b-form-checkbox-group>
        </b-form-group>
      </b-col>
      <b-col cols="8">
        <b-form-group v-if="withPrice" :label="$t('value')">
          <b-form-input
            min="0"
            type="number"
            @input="errors.price = false"
            :class="{ 'input-invalid': errors.price }"
            v-model="contract.price"
            :placeholder="$t('value')"
          ></b-form-input>
        </b-form-group>

        <b-form-group :label="$t('budget_untill')">
          <b-form-select
            @change="errors.balanceId = false"
            :class="{ 'input-invalid': errors.balanceId }"
            v-model="contract.balanceId"
            name="budget"
          >
            <option
              v-for="budget in budgets"
              :value="budget.id"
              :key="budget.id"
              >{{ $t(budget.value) }}</option
            >
          </b-form-select>
        </b-form-group>
      </b-col>
    </b-row>

    <div class="bg-white d-flex justify-content-between my-3">
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
import { mapState } from "vuex";

export default {
  props: ["contract"],
  components: {},
  data() {
    return {
      withPrice: true,
      errors: {},
    };
  },
  methods: {
    validateForm() {
      if (
        ((this.contract.price && this.withPrice === true) ||
          (!this.contract.price && this.withPrice !== true)) &&
        this.contract.balanceId
      ) {
        return true;
      }
      this.errors = {};
      if (!this.contract.price && this.withPrice === true) {
        this.errors.price = true;
      }
      if (!this.contract.balanceId) {
        this.errors.balanceId = true;
      }

      return false;
    },

    submit() {
      if (this.validateForm() === false) {
        this.$toasted.error(this.$t("fill_in_the_fields"));
      } else {
        this.$store.commit("IS_LOADING_TRUE");
        ContractService.savePrice(this.contract.id, {
          contractId: this.contract.id,
          price: this.contract.price,
          balanceId: this.contract.balanceId,
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
  computed: {
    ...mapState(["budgets"]),
  },
  watch: {
    contract() {
      if (this.contract) {
        if (this.contract.price === null) {
          this.withPrice = false;
        } else {
          this.withPrice = true;
        }
      }
    },
  },
};
</script>
<style></style>
