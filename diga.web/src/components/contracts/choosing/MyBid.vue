<template>
  <div v-if="contractorInfo">
    <b-modal v-if="offer" hide-footer v-model="modal">
      <template #modal-header>
        <p class="h5 font-weight-bold">
          {{ $t("update_offer") }}: {{ contract.name }}
        </p>
      </template>
      <b-form @submit.prevent="saveOffer">
        <div class="form-group">
          <label for="descriptionOffer">{{ $t("description_offer") }}:</label>
          <div v-if="$v.offer.text.$error">
            <p
              v-if="!$v.offer.text.required"
              class="text-center m-0 lh-1-5 error fs-13 text-red"
            >
              {{ $t("required") }}
            </p>
            <p
              v-if="!$v.offer.text.maxLength"
              class="text-center m-0 lh-1-5 error fs-13 text-red"
            >
              {{ $t("maxlength") }} 1000
            </p>
          </div>
          <b-form-textarea
            :class="{ 'input-invalid': $v.offer.text.$error }"
            @blur="$v.offer.text.$touch()"
            v-model="offer.text"
            id="descriptionOffer"
            name="descriptionOffer"
            rows="8"
            :placeholder="$t('no_more_than_100_characters')"
          ></b-form-textarea>
        </div>
        <div class="form-group">
          <label for="priceOffer">{{ $t("price") }}:</label>
          <b-form-input
            step=".01"
            min="0"
            type="number"
            id="priceOffer"
            v-model="offer.price"
            :placeholder="$t('price')"
          ></b-form-input>
        </div>
        <div class="form-group">
          <label for="daysOffer">{{ $t("days_number") }}:</label>
          <b-form-input
            min="0"
            type="number"
            id="daysOffer"
            v-model="offer.daysNumber"
            placeholder="0"
          ></b-form-input>
        </div>
        <div>
          <b-button
            v-if="contractorInfo.userId !== $store.state.user.id"
            :disabled="$v.$invalid"
            class="float-right"
            variant="primary"
            type="submit"
            >{{ $t("make_offer") }}</b-button
          >
        </div>
      </b-form>
    </b-modal>
  </div>
</template>

<script>
import { required, maxLength } from "vuelidate/lib/validators";
import BidService from "@/services/BidService.js";

export default {
  props: [
    "offer",
    "contractorInfo",
    "contract",
    "modalEditOffer",
    "myBidAlreadyExists",
    "myBidPublished",
  ],
  data() {
    return {
      modal: false,
    };
  },
  validations: {
    offer: {
      text: {
        required,
        maxLength: maxLength(1000),
      },
    },
  },
  methods: {
    saveOffer() {
      if (this.myBidAlreadyExists === true) {
        this.editOffer("Published");
      } else {
        this.createOffer("Published");
      }
    },
    editOffer(status) {
      this.$store.commit("IS_LOADING_TRUE");
      BidService.EditOffer(this.contract.id, {
        text: this.offer.text,
        price: this.offer.price,
        daysNumber: this.offer.daysNumber,
        status: status,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.modal = false;
          this.$emit("getBids");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    createOffer(status) {
      this.$store.commit("IS_LOADING_TRUE");
      BidService.MakeOffer(this.contract.id, {
        text: this.offer.text,
        daysNumber: this.offer.daysNumber,
        price: this.offer.price,
        status: status,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.modal = false;
          this.$emit("getBids");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  watch: {
    modalEditOffer: {
      immediate: true,
      handler() {
        this.modal = this.modalEditOffer;
      },
    },
  },
};
</script>

<style scoped>
.b-table-sticky-header {
  max-height: 500px !important;
  margin-bottom: 0 !important;
}
.modal-xl {
  width: auto !important;
}
</style>
