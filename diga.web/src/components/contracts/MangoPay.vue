<template>
  <b-modal
    id="modal-customer-payment"
    :title="
      'Make the payment amount ' + parseFloat(sum).toFixed(0) + '€ ' + type
    "
    hide-footer
  >
    <b-form-group
      label="Select the payment method"
      v-slot="{ ariaDescribedby }"
    >
      <b-form-radio-group
        v-model="selectedPaymentMethod"
        :options="options"
        :aria-describedby="ariaDescribedby"
        name="radios-btn-default"
        buttons
      ></b-form-radio-group>
    </b-form-group>

    <div v-if="selectedPaymentMethod === 'BankWire'">
      <template v-if="payInResponse && payInResponse.bankAccount">
        BIC: {{ payInResponse.bankAccount.bic }} IBAN:
        {{ payInResponse.bankAccount.iban }} Owner:
        {{ payInResponse.bankAccount.ownerName }}
        Please make the payment within 30 days. Unless payment won't be possible
      </template>
      <b-button
        class="my-2"
        @click="postPayIn"
        :disabled="payInResponse !== null && payInResponse.bankAccount != null"
        >Get payment requisites</b-button
      >
    </div>
    <div v-if="selectedPaymentMethod === 'Card'">
      <div v-if="payInResponse && payInResponse.status === 2">
        Congratulations. Your payment were proceded successfully.
      </div>
      <template v-if="payInResponse === null">
        <div v-for="card in cards" :key="card.id">
          <span>{{ card.alias }}</span>
          <b-button
            class="my-2"
            @click="
              selectedCardId = card.id;
              postPayIn();
            "
            >Pay with this card</b-button
          >
        </div>
        <b-button class="my-2" @click="createRegistration"
          >Register new card</b-button
        >
      </template>

      <template v-if="showNewCardForm">
        <b-form-input v-model="cardNumber" />
        <b-form-input v-model="cardExpirationDate" />
        <b-form-input v-model="cardCvx" />
        <b-button class="my-2" @click="postCard">Register card</b-button>
      </template>
    </div>
  </b-modal>
</template>

<script>
import mangoPayService from "@/services/MangoPayService.js";
import Axios from "axios";

export default {
  props: ["sum", "type", "contractId", "measurementReportId", "paymentPartId"],
  data() {
    return {
      payInResponse: null,
      selectedCardId: null,
      selectedPaymentMethod: "Card",
      options: [
        { text: "With credit/debit card", value: "Card" },
        { text: "With bank transfer", value: "BankWire" },
      ],
      cards: [],
      cardsCount: 0,
      cardRegistration: null,
      showNewCardForm: false,
      cardNumber: "",
      cardExpirationDate: "",
      cardCvx: "",
      registrationData: null,
    };
  },
  methods: {
    postPayIn() {
      this.$store.commit("IS_LOADING_TRUE");
      mangoPayService
        .postPayIn({
          cardId: this.selectedCardId,
          method: this.selectedPaymentMethod,
          type: this.type,
          contractId: this.contractId,
          measurementReportId: this.measurementReportId,
          paymentPartId: this.paymentPartId,
        })
        .then((response) => {
          this.payInResponse = response.data;
          if (this.payInResponse.redirectURL) {
            window.location.href = this.payInResponse.redirectURL;
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    cleanCardForm() {
      this.showNewCardForm = false;
      this.cardNumber = "";
      this.cardExpirationDate = "";
      this.cardCvx = "";
    },
    updateCard() {
      this.$store.commit("IS_LOADING_TRUE");
      mangoPayService
        .updateRegistration(this.cardRegistration.id, {
          token: this.registrationData,
        })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getSavedCards();
          this.cleanCardForm();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    postCard() {
      this.$store.commit("IS_LOADING_TRUE");

      Axios.post(
        this.cardRegistration.cardRegistrationURL,
        new URLSearchParams({
          accessKeyRef: this.cardRegistration.accessKey,
          data: this.cardRegistration.preregistrationData,
          cardNumber: this.cardNumber,
          cardExpirationDate: this.cardExpirationDate,
          cardCvx: this.cardCvx,
        }),
        {
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
          },
        }
      )
        .then((response) => {
          this.registrationData = response.data;
          this.updateCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    createRegistration() {
      this.$store.commit("IS_LOADING_TRUE");
      mangoPayService
        .createRegistration()
        .then((response) => {
          this.cardRegistration = response.data;
          this.showNewCardForm = true;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getSavedCards() {
      this.$store.commit("IS_LOADING_TRUE");
      mangoPayService
        .getCards(1, 25)
        .then((response) => {
          this.cards = response.data.list;
          this.cardsCount = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getSavedCards();
  },
};
</script>

<style scoped>
</style>