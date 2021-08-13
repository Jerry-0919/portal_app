<template>
  <div>
    <div v-if="myBid">
      <hr class="grey-divider" />
      <div class="d-flex">
        <div class="mr-3">
          <i
            style="font-size: 60px"
            v-if="!myBid.userAvatar"
            class=" bx bx-user-circle nav-i"
          ></i>
          <div
            v-else
            class="avatar"
            v-bind:style="{
              backgroundImage: 'url(' + '/img/src/' + myBid.userAvatar + ')',
            }"
          ></div>
        </div>
        <div>
          <a class="h4 blue py-1" href="#">{{ myBid.fullName }}</a>
          <div>
            <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
              ><i class="bx bxs-star align-middle"></i
              >{{ myBid.userRating }}</span
            >
            <a
              v-b-tooltip.hover
              :title="$t('positive_reviews')"
              class="mx-1"
              href="#"
              ><span class="blue"
                ><i class="bx bxs-like align-middle"></i>
                {{ myBid.userGoodReviews }}</span
              >
            </a>
            <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
              ><span class="grey-dark"
                ><i class="bx bxs-dislike align-middle"></i>
                {{ myBid.userBadReviews }}</span
              ></a
            >
          </div>
        </div>
        <div class="ml-auto">
          <div class="d-flex align-items-end">
            <span
              v-b-tooltip.hover
              :title="$t('total_work_time')"
              class="badge counter-blue p-2 mr-2"
              >{{ myBid.daysNumber }} {{ $t("days") }}</span
            >
            <span
              v-b-tooltip.hover
              :title="$t('total_cost_of_work')"
              class="badge counter-orange p-2"
            >
              EUR {{ parseFloat(myBid.price).toFixed(0) }}</span
            >
          </div>
        </div>
      </div>

      <div class="my-3">
        <div class="my-3">{{ myBid.text }}</div>
        <a
          v-if="contract.estimateFileName"
          :href="
            '/api/pdf/estimate/' +
              contract.id +
              '/' +
              myBid.userId +
              '/' +
              $root.locale
          "
          target="_blank"
          class="blue"
        >
          {{ $t("view_estimate_pdf") }}
        </a>
      </div>
      <div>
        <b-button @click="ReturnMyOffer" class="b-orange-outline">
          {{ $t("return") }}</b-button
        >
      </div>
    </div>
    <div v-for="otherBid in otherBids" :key="otherBid.userId">
      <hr />
      <div class="d-flex">
        <div class="mr-3">
          <i
            style="font-size: 60px"
            v-if="!otherBid.userAvatar"
            class=" bx bx-user-circle nav-i"
          ></i>
          <div
            v-else
            class="avatar"
            v-bind:style="{
              backgroundImage: 'url(' + '/img/src/' + otherBid.userAvatar + ')',
            }"
          ></div>
        </div>
        <div>
          <a class="h4 blue py-1" href="#">{{ otherBid.fullName }}</a>
          <div>
            <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
              ><i class="bx bxs-star align-middle"></i
              >{{ otherBid.userRating }}</span
            >
            <a
              v-b-tooltip.hover
              :title="$t('positive_reviews')"
              class="mx-1"
              href="#"
              ><span class="blue"
                ><i class="bx bxs-like align-middle"></i>
                {{ otherBid.userGoodReviews }}</span
              >
            </a>
            <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
              ><span class="grey-dark"
                ><i class="bx bxs-dislike align-middle"></i>
                {{ otherBid.userBadReviews }}</span
              ></a
            >
          </div>
        </div>
        <div class="ml-auto">
          <div class="d-flex align-items-end">
            <span
              v-b-tooltip.hover
              :title="$t('total_work_time')"
              class="badge counter-blue p-2 mr-2"
              >{{ otherBid.daysNumber }} {{ $t("days") }}</span
            >
            <span
              v-b-tooltip.hover
              :title="$t('total_cost_of_work')"
              class="badge counter-orange p-2"
              >EUR {{ parseFloat(otherBid.price).toFixed(0) }}</span
            >
          </div>
        </div>
      </div>

      <div class="my-3">
          <div class="my-3"> {{ otherBid.text }}</div> 

        <a
          v-if="contract.estimateFileName"
          :href="
            '/api/pdf/estimate/' +
              contract.id +
              '/' +
              otherBid.userId +
              '/' +
              $root.locale
          "
          target="_blank"
          class="blue"
        >
          {{ $t("view_estimate_pdf") }}
        </a>
      </div>
      <div v-if="$store.state.currentRole === 'Executor'">
               <b-button class="b-blue-outline"
          >
          {{ $t("dumping_prices") }}</b-button
        >
        <b-button
          v-b-tooltip.hover
          :title="$t('violation_popup_tooltip')"
          @click="complainModal = !complainModal"
         class="b-orange-outline mx-3"
          >
          {{ $t("service_rules_violation") }}</b-button
        >
        <b-modal v-model="complainModal">
          <template #modal-header>
            {{ $t("report_violation") }}: {{ otherBid.userName }}
            {{ $t("in") }} {{ contract.name }}
          </template>
          <b-form-textarea
            v-model="complaintText"
            id="report"
            name="report"
            rows="8"
            :placeholder="$t('no_more_than_100_characters')"
          ></b-form-textarea>
          <template #modal-footer>
            <b-button
              @click="makeComplaint(otherBid.userId)"
              variant="primary"
              >{{ $t("report_violation") }}</b-button
            >
          </template>
        </b-modal>
      </div>
      <div class="d-flex justify-content-between" v-else>
        <b-button class="b-blue-outline"> {{ $t("message") }}</b-button>
        <b-button @click="unReject(otherBid.userId)" class="b-orange-outline">
          {{ $t("return") }}</b-button
        >
      </div>
    </div>
  </div>
</template>

<script>
import BidService from "@/services/BidService.js";
import { required, maxLength } from "vuelidate/lib/validators";
import UserService from "@/services/UserService.js";

export default {
  props: ["bids", "contract"],
  data() {
    return {
      myBid: null,
      complainModal: false,
      complaintText: ""
    };
  },
    validations: {
    myBid: {
      text: {
        required,
        maxLength: maxLength(1000),
      },
    },
  },
  methods: {
        makeComplaint(id) {
      UserService.makeComplaint({
        userId: id,
        text: this.complaintText,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.complainModal = false;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    ReturnMyOffer() {
      if (confirm(this.$t("are_you_sure"))) {
        this.$store.commit("IS_LOADING_TRUE");
        BidService.ReturnMyOffer(this.contract.id)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$emit("getBids");
            this.myBid = null;
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    unReject(userId) {
      console.log("ttt");
      this.$store.commit("IS_LOADING_TRUE");
      BidService.UnReject(this.contract.id, userId)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("getBids");
          this.myBid = null;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  computed: {
    otherBids() {
      if (this.bids && this.bids.length > 0 && this.$store.state.user) {
        return this.bids.filter((b) => b.userId !== this.$store.state.user.id);
      } else {
        return [];
      }
    },
  },
  watch: {
    bids: {
      immediate: true,
      handler() {
        if (this.bids && this.$store.state.user && this.bids.length > 0) {
          this.myBid = this.bids.find(
            (b) => b.userId === this.$store.state.user.id
          );
        }
      },
    },
  },
};
</script>

<style scoped></style>
