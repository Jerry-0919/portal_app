<template>
  <div v-if="contractorInfo" :key="componentKey">
    <b-modal style="height: 100%" size="xl" hide-footer v-model="modal">
      <template #modal-header>
        <p class="h5 font-weight-bold">
          {{ $t("make_offer") }}: {{ contract.name }}
        </p>
      </template>

      <b-form style="height: 100%" @submit.prevent="">
        <div style="height: 30%" class="form-group mb-5">
          <b-table-simple
            borderless
            no-border-collapse
            hover
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
              </b-tr>
            </b-thead>

            <b-tbody>
              <estimateLine
                v-for="section in offerFull.sections"
                :key="section.id"
                :contract="contract"
                :section="section"
                :isParent="true"
                :editPriceAndNotes="true"
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
          <hr class="mt-0" />
        </div>
        <div class="form-group">
          <label for="priceOffer">{{ $t("price") }}:</label>
          <span class="font-weight-bold mx-3" id="priceOffer"
            >{{ parseFloat(offerFull.price).toFixed(2) }} €</span
          >
        </div>
        <div class="form-group">
          <label for="descriptionOfferFull"
            >{{ $t("description_offer") }}:</label
          >
          <div v-if="$v.offerFull.text.$error">
            <p
              v-if="!$v.offerFull.text.maxLength"
              class="text-center m-0 lh-1-5 error fs-13 text-red"
            >
              {{ $t("maxlength") }} 1000
            </p>
          </div>
          <b-form-textarea
            :class="{ 'input-invalid': $v.offerFull.text.$error }"
            @blur="$v.offerFull.text.$touch()"
            v-model="offerFull.text"
            id="descriptionOfferFull"
            name="descriptionOfferFull"
            rows="3"
            :placeholder="$t('no_more_than_100_characters')"
          ></b-form-textarea>
        </div>

        <div class="form-group">
          <label for="daysOffer">{{ $t("days_number") }}:</label>
          <b-form-input
            min="0"
            type="number"
            id="daysOffer"
            v-model="offerFull.daysNumber"
            placeholder="0"
          ></b-form-input>
        </div>
        <div
          v-if="
            !myBidPublished && contractorInfo.userId !== $store.state.user.id
          "
        >
          <b-button class="mr-2" @click="saveDraftOffer" type="submit">{{
            $t("save_draft")
          }}</b-button>
          <b-button
            :disabled="!countTotal()"
            variant="primary"
            type="submit"
            @click="submitOfferWithEstimate"
            >{{ $t("make_offer") }}</b-button
          >
        </div>
        <div
          v-if="
            myBidPublished && contractorInfo.userId !== $store.state.user.id
          "
        >
          <b-button
            :disabled="!countTotal()"
            variant="primary"
            type="submit"
            @click="submitOfferWithEstimate"
            >{{ $t("update_offer") }}</b-button
          >
        </div>
      </b-form>
    </b-modal>
  </div>
</template>

<script>
import BidService from "@/services/BidService.js";
import estimateLine from "../EstimateLine.vue";
import { required, maxLength } from "vuelidate/lib/validators";
import EstimateService from "@/services/EstimateService.js";

export default {
  components: {
    estimateLine,
  },
  props: [
    "offerFull",
    "contractorInfo",
    "contract",
    "modalEditOfferFull",
    "myBidAlreadyExists",
    "myBidPublished",
  ],
  data() {
    return {
      modal: false,
      componentKey: 0,
    };
  },
  validations: {
    offerFull: {
      text: {
        required,
        maxLength: maxLength(1000),
      },
    },
  },
  methods: {
    submitOfferWithEstimate() {
      if (this.myBidAlreadyExists === true) {
        this.editMyEstimate();
        this.editOffer("Published");
      } else {
        this.createOfferWithEstimate("Published");
      }
    },
    editMyEstimate() {
      this.$store.commit("IS_LOADING_TRUE");
      let allPositions = [];
      this.offerFull.sections.forEach((s) => {
        allPositions.push(...this.ejectAllPositions(s));
      });
      EstimateService.editMyEstimate(this.contract.id, {
        sections: [],
        positions: allPositions,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    editOffer(status) {
      this.$store.commit("IS_LOADING_TRUE");
      BidService.EditOffer(this.contract.id, {
        text: this.offerFull.text,
        price: this.offerFull.price,
        daysNumber: this.offerFull.daysNumber,
        status: status,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("getBids");
          this.modal = false;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    createOfferWithEstimate(status) {
      this.$store.commit("IS_LOADING_TRUE");
      let payload = JSON.parse(JSON.stringify(this.offerFull));

      payload.sections = [];
      payload.positions = [];

      this.offerFull.sections.forEach((s) => {
        payload.positions.push(...this.ejectAllPositions(s));
      });

      payload.status = status;
      BidService.MakeOffer(this.contract.id, payload)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("getBids");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    ejectAllPositions(section) {
      let result = [];
      if (section.positions && section.positions.length > 0) {
        result.push(...section.positions);
      }
      if (section.sections && section.sections.length > 0) {
        section.sections.forEach((s) => {
          result.push(...this.ejectAllPositions(s));
        });
      }
      return result;
    },
    getMyEstimate() {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getMyEstimate(this.contract.id)
        .then((response) => {
          this.offerFull.sections = response.data.sections;
          this.componentKey += 1;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    countTotalRecursive(section) {
      let total = 0.0;
      section.positions.forEach((p) => {
        if (p.quantity && p.price) {
          total += p.quantity * p.price;
        }
      });

      section.sections.forEach((s) => {
        total += this.countTotalRecursive(s);
      });

      return total;
    },
    countTotal() {
      let total = 0.0;

      if (
        this.offerFull &&
        this.offerFull.sections &&
        this.offerFull.sections.length
      ) {
        this.offerFull.sections.forEach((s) => {
          total += this.countTotalRecursive(s);
        });
      }
      this.offerFull.price = total;
      return total;
    },
    saveDraftOffer() {
      if (this.myBidAlreadyExists === true) {
        this.editMyEstimate();
        this.editOffer("Draft");
      } else {
        this.createOfferWithEstimate("Draft");
      }
    },
    getEstimate() {
      if (this.offerFull.sections && this.offerFull.sections.length > 0) {
        return;
      }
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getEstimate(this.contract.estimateId)
        .then((response) => {
          this.offerFull.sections = response.data.sections.sort(
            (a, b) => a.number - b.number
          );
          this.componentKey += 1;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    if (this.myBidAlreadyExists === true) {
      this.getMyEstimate();
    } else {
      this.getEstimate();
    }
  },
  watch: {
    offerFull: {
      immediate: true,
      handler() {
        if (this.myBidAlreadyExists === true) {
          this.getMyEstimate();
        } else {
          this.getEstimate();
        }
      },
    },
    modalEditOfferFull: {
      immediate: true,
      handler() {
        this.modal = this.modalEditOfferFull;
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
