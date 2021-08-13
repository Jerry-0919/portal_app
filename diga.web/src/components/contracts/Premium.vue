<template>
  <div class="card p-4">
    <p class="h3">{{ $t("useful_functions") }}</p>
    <b-button
      @click="premiumModal = !premiumModal"
      v-b-tooltip.hover
      :title="$t('buy_premium_tip')"
      class="b-orange my-2"
    >
    {{ $t("buy_premium") }}
    </b-button>
    <b-modal size="lg" v-model="premiumModal" hide-footer>
      <template #modal-header>
        <div>
           <b class="mx-2">{{ $t("premium_contract") }}:</b>
        <small>{{ $t("buy_premium_tip") }}</small>
        </div>
       
      </template>
      <div id="money">
        <div class="d-flex justify-content-center my-3">
          <div>
            {{ $t("from_balance") }}:
          </div>
          <div class="mx-3">
            <span id="from_balance">0.00</span> €
          </div>
        </div>
        <b-tabs content-class="my-5" fill>
          <b-tab active>
            <template #title>
              <i
                class="bx bx-credit-card align-middle"
                style="font-size: 25px"
              ></i>
            </template>
            <div class="w-50 m-auto">
              <b-form id="creditCard">
                <p class="h5 text-center">Credit/Debit Card</p>
                <b-form-group :label="$t('name_from_card')">
                  <b-form-input
                    type="text"
                    :placeholder="$t('enter_something')"
                    v-model="card.name"
                    @input="
                      (v) => {
                        card.name = v.toUpperCase();
                      }
                    "
                  >
                  </b-form-input>
                </b-form-group>
                <b-form-group :label="$t('card_number')">
                  <b-form-input
                    type="number"
                    no-wheel
                    placeholder="****-****-****-****"
                  >
                  </b-form-input>
                </b-form-group>
                <div class="d-flex">
                  <b-form-group :label="$t('expiration')">
                    <b-input-group>
                      <b-form-input
                        aria-label="Month"
                        type="number"
                        :no-wheel="false"
                        placeholder="MM"
                      >
                      </b-form-input>
                      <b-form-input
                        type="number"
                        :no-wheel="true"
                        aria-label="Year"
                        placeholder="YY"
                      >
                      </b-form-input>
                    </b-input-group>
                  </b-form-group>
                  <b-form-group label="CVV">
                    <b-form-input type="number" placeholder="***">
                    </b-form-input>
                  </b-form-group>
                </div>
                 <b-button variant="primary" @click="buyPremium">{{
          $t("confirm_and_pay")
        }}</b-button>
              </b-form>
            </div>
          </b-tab>
          <b-tab>
            <template #title>
              <img
                src="/Assets/img/multibanco.png"
                height="25"
                alt="Multibanco"
              />
            </template>
            <div class="w-50 m-auto">
              <p class="mt-5">{{ $t("multibanco_1") }}</p>
              <b-button variant="primary my-3">
                {{ $t("get_payment_details") }}
              </b-button>
              <div class="text-left">
                <p class="my-2">{{ $t("reference") }}</p>
                <p class="my-2">{{ $t("entity") }}</p>
                <p >{{ $t("total_sum") }}:</p>
              </div>

              <p class="mt-5">
                <i class="bx bx-info-circle align-middle"></i>
                {{ $t("multibanco_2") }}
              </p>
            </div>
          </b-tab>
          <b-tab>
            <template #title>
              <img src="/Assets/img/MBWay.png" height="25" alt="MBWAY" />
            </template>
             <div class="w-50 m-auto">
            <p class="mt-5"> {{$t('mbway_1')}} </p>
              <b-form-input :placeholder="$t('telephone')" type="tel">

              </b-form-input>
              <b-button variant="primary my-3"> {{$t('confirm')}} </b-button>
                            <p class="mt-5">
                <i class="bx bx-info-circle align-middle"></i>
                {{ $t("mbway_2") }}
              </p>
             </div>
            </b-tab
          >
        </b-tabs>
      </div>
    </b-modal>
    <b-button
      @click="topModal = !topModal"
      v-b-tooltip.hover
      :title="$t('raise_top_tip')"
    class="b-blue my-2"
      >
      {{ $t("raise_top") }}</b-button
    >
    <b-modal v-model="topModal">
      <template #modal-header>
        {{ $t("select_date") }}: {{ $t("end_of_competition") }}
      </template>

      <template #modal-footer>
        <b-button class="float-right" variant="primary" @click="buyPremium">{{
          $t("extend")
        }}</b-button>
      </template>
    </b-modal>
    <b-button
      @click="bidModal = !bidModal"
      v-b-tooltip.hover
      :title="$t('hide_bids_tip')"
     class="b-grey-outline my-2"
      >{{ $t("hide_bids") }}</b-button
    >
    <b-modal v-model="bidModal">
      <template #modal-header>
        {{ $t("select_date") }}: {{ $t("end_of_competition") }}
      </template>

      <template #modal-footer>
        <b-button class="float-right" variant="primary" @click="buyPremium">{{
          $t("extend")
        }}</b-button>
      </template>
    </b-modal>
    <b-button
      @click="urgentModal = !urgentModal"
      v-b-tooltip.hover
      :title="$t('urgent_project_tip')"
      class="b-orange-outline my-2"
      >
      {{ $t("urgent_project") }}</b-button
    >

    <b-modal v-model="urgentModal">
      <template #modal-header>
        {{ $t("select_date") }}: {{ $t("end_of_competition") }}
      </template>

      <template #modal-footer>
        <b-button class="float-right" variant="primary" @click="buyPremium">{{
          $t("extend")
        }}</b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
export default {
  props: ["id"],

  data() {
    return {
      card: {
        name: "",
        number: null,
        mm: null,
        yy: null,
        cvv: null,
      },
      premiumModal: false,
      topModal: false,
      bidModal: false,
      urgentModal: false,
    };
  },
  methods: {
    buyPremium() {},
  },
};
</script>

<style scoped>
i {
  font-size: 16px;
}
</style>
