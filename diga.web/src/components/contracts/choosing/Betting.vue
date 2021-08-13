<template>
  <div>
    <div v-if="myBid">
      <hr />
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
              >EUR {{ parseFloat(myBid.price).toFixed(0) }}
            </span>
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
        <b-button
          @click="updateOffer"
        
          class="btn b-blue"
          >
          {{ $t("update_offer") }}</b-button
        >
        <b-button
          @click="WithdrawMyOffer"
          class="b-orange-outline mx-3"
          >
          {{ $t("withdraw_bid") }}</b-button
        >
      </div>
    </div>
    <div v-for="otherBid in otherBids" :key="otherBid.userId">
      <hr class="grey-divider" />
      <div class="d-flex">
        <div class="mr-3">
          <i
            style="font-size: 60px"
            v-if="!otherBid.userAvatar"
            class="bx bx-user-circle nav-i"
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
              >EUR {{ parseFloat(otherBid.price).toFixed(0) }}
            </span>
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
        <b-button
          @click="selectExecutorModal = !selectExecutorModal"
          class="b-blue"
        >
          {{ $t("select_executor") }}</b-button
        >
        <b-button class="b-blue-outline"> {{ $t("message") }}</b-button>
        <b-button
          @click="addToBookmarks(otherBid.userId)"
          class="b-orange-outline"
        >
          {{ $t("add_to_bookmarks") }}</b-button
        >
        <b-button @click="reject(otherBid.userId)" class="b-orange">
          {{ $t("reject") }}</b-button
        >
        <b-modal hide-footer v-model="selectExecutorModal">
          <template #modal-header>
            <p class="h5 font-weight-bold">
              {{ $t("select_executor") }}
            </p>
          </template>
          <div class="text-center my-3">
            {{ $t("are_you_sure") }}
          </div>
          <div class="d-flex justify-content-between">
            <b-button
              @click="selectExecutor(otherBid.userId)"
              class="mr-auto b-blue"
              type="submit"
              >{{ $t("confirm") }}</b-button
            >

            <b-button class="ml-auto b-orange-outline" type="submit">{{
              $t("see_more_offers")
            }}</b-button>
          </div>
        </b-modal>
      </div>
    </div>
    <bidWithEstimate
      v-if="myBid && contract && contract.estimateName !== null"
      :offerFull="myBid"
      :contract="contract"
      :contractorInfo="contractorInfo"
      :modalEditOfferFull="modalEditOfferFull"
      :myBidAlreadyExists="myBidAlreadyExists"
      :myBidPublished="myBidPublished"
      @getBids="getBids"
    />
    <bid
      v-if="myBid && contract && contract.estimateName === null"
      :offer="myBid"
      :contract="contract"
      :contractorInfo="contractorInfo"
      :modalEditOffer="modalEditOffer"
      :myBidAlreadyExists="myBidAlreadyExists"
      :myBidPublished="myBidPublished"
      @getBids="getBids"
    />
  </div>
</template>

<script>
import BidService from "@/services/BidService.js";
import { required, maxLength } from "vuelidate/lib/validators";
import bidWithEstimate from "./MyBidWithEstimate.vue";
import bid from "./MyBid.vue";
import UserService from "@/services/UserService.js";

export default {
  props: [
    "bids",
    "contract",
    "myBidAlreadyExists",
    "myBidPublished",
    "contractorInfo",
  ],
  components: {
    bidWithEstimate,
    bid,
  },
  data() {
    return {
      modalEditOfferFull: false,
      selectExecutorModal: false,
      modalEditOffer: false,
      complainModal: false,
      complaintText: "",
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
  computed: {
    otherBids() {
      if (this.bids && this.bids.length > 0 && this.$store.state.user) {
        return this.bids.filter((b) => b.userId !== this.$store.state.user.id);
      } else {
        return [];
      }
    },
    myBid() {
      if (this.bids && this.bids.length > 0 && this.$store.state.user) {
        let bid = this.bids.find((b) => b.userId === this.$store.state.user.id);
        return bid;
      } else {
        return null;
      }
    },
  },
  watch: {},
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
    getBids() {
      this.$emit("getBids");
    },
    updateOffer() {
      if (this.contract.estimateName !== null) {
        this.modalEditOfferFull = !this.modalEditOfferFull;
      } else {
        this.modalEditOffer = !this.modalEditOffer;
      }
    },

    selectExecutor(userId) {
      this.$store.commit("IS_LOADING_TRUE");
      BidService.SelectExecutor(this.contract.id, userId)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          if (this.contract.estimateName !== null) {
            this.$router.push({
              name: "EstimateApproval",
              params: { id: this.contract.id },
            });
          } else {
            this.$router.push({
              name: "ContractApproval",
              params: { id: this.contract.id },
            });
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    addToBookmarks(userId) {
      this.$store.commit("IS_LOADING_TRUE");
      BidService.AddtoBookmarks(this.contract.id, userId)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("getBids");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    WithdrawMyOffer() {
      if (confirm(this.$t("are_you_sure"))) {
        this.$store.commit("IS_LOADING_TRUE");
        BidService.WithdrawMyOffer(this.contract.id)
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
    reject(userId) {
      if (confirm(this.$t("are_you_sure"))) {
        this.$store.commit("IS_LOADING_TRUE");
        BidService.Reject(this.contract.id, userId)
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
  },
};
</script>

<style>
.light-primary {
  border: transparent !important;
  background-color: #e2ecff !important;
  color: #5a8dee !important;
}
.light-primary:hover {
  background-color: #719df0 !important;
  color: #fff !important;
}
.light-danger {
  border: transparent !important;
  background-color: #ffdede !important;
  color: #ff5b5c !important;
}
.light-danger:hover {
  background-color: #ff7575 !important;
  color: #fff !important;
}

.chip {
  background-color: #fdac41;
  font-size: 0.8rem;
  border-radius: 1.428rem;
  display: inline-flex;
  padding: 0 10px;
  margin-bottom: 5px;
  vertical-align: middle;
  justify-content: center;
}
.chip .chip-body {
  color: #fff;
  display: flex;
  justify-content: space-between;
  min-height: 1.857rem;
  min-width: 1.857rem;
}
.text-success {
  color: #39da8a !important;
}
.text-success i,
.text-light-danger i {
  font-size: 1.2rem !important;
}
.text-light-danger {
  color: rgba(255, 91, 92, 0.2);
}
</style>
