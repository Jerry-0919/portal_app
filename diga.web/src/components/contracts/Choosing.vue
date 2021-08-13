<template>
  <div class="m-3" v-if="contract && contractorInfo && $store.state.user">
    <div class="orange-header px-4 py-3 d-flex">
      <span class="h1"> {{ $t("contract") }}: {{ contract.name }}</span>
      <div v-if="contract.contractTypeId" class="ml-auto d-flex contract-type">
        <span class="ctype fs-14">
          {{ $t(getContractTypeById(contract.contractTypeId).name) }}
        </span>
        <div class="trapezoid-left"></div>
      </div>
    </div>

    <template>
      <b-breadcrumb class="my-0 t3">
        <b-breadcrumb-item class=" blue" :to="{ name: 'home_index' }">
          <i class="bx bx-home"></i>
        </b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{ $t("Draft") }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue" disabled>{{
          $t("publication")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item class="blue">{{
          $t("choosing_contractor")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("estimate_approval") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{
          $t("approval_of_conditions")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>

    <div>
      <div class="bg-white py-3 px-4 my-1">
        <p class="h2 m-0">{{ $t("about_contract") }}</p>
      </div>
      <b-row v-if="contractorInfo && contract">
        <b-col cols="9">
          <div>
            <div class="card border-orange p-4">
              <div class="m-2" v-html="contract.description"></div>

              <b-row class="card-footer bg-white">
                <b-col v-if="contract.buildStart">
                  <p class="h6 mb-3">{{ $t("start_of_construction") }}</p>
                  <span class="tenderstart p-2 mx-2">
                    <i class="bx bx-calendar"></i>
                    {{ contract.buildStart | moment("DD.MM.YYYY") }}
                  </span>
                </b-col>
                <b-col v-if="contract.plannedBuildEnd">
                  <p class="h6 mb-3">{{ $t("completion_of_construction") }}</p>
                  <span class="tenderend p-2 mx-2">
                    <i class="bx bx-calendar"></i>
                    {{ contract.plannedBuildEnd | moment("DD.MM.YYYY") }}
                  </span>
                </b-col>
                <b-col>
                  <p class="h6 mb-3">{{ $t("contract_budget") }}</p>
                  <span v-if="contract.price" class="budget p-2 mx-2">
                    <i class="bx bx-euro"></i>
                    {{ parseFloat(contract.price).toFixed(2) }}
                  </span>
                  <span v-else class="budget p-2 mx-2">
                    <i class="bx bx-euro"></i>
                    {{ $t("before") }}
                    {{ getBudgetById(contract.balanceId).value }}
                  </span>
                </b-col>
              </b-row>
              <div
                class="mt-4 d-flex align-items-center justify-content-between"
              >
                <div class="blue ml-2">
                  <a
                    v-if="contractorInfo && contractorInfo.estimateFileName"
                    type="button"
                    class="blue"
                    target="_blank"
                    :href="'/docs/src/' + contractorInfo.estimateFileName"
                  >
                    <span> {{ $t("project_files") }} </span>
                    <img src="/Assets/platform/icons/xls.svg" alt="" />
                    <img src="/Assets/platform/icons/download.svg" alt="" />
                  </a>
                </div>

                <div
                  class="d-flex align-items-center flex-wrap"
                  v-if="contractorInfo && contractorInfo.files"
                >
                  <a
                    v-for="file in contractorInfo.files"
                    :key="file.name"
                    type="button"
                    class="btn btn-blue-outline m-1"
                    target="_blank"
                    :href="'/docs/src/' + file"
                  >
                    <span>{{ file }} </span>
                    <img src="/Assets/platform/icons/xls.svg" alt="" />
                    <img src="/Assets/platform/icons/download.svg" alt="" />
                  </a>
                </div>
              </div>
              <div v-if="$store.state.currentRole === 'Executor'">
                <hr />
                <div class="d-flex justify-content-around">
                  <b-button
                    @click="modalWithEstimate = !modalWithEstimate"
                    v-if="contractorInfo.estimateFileName"
                    class="b-orange"
                    :disabled="myBidPublished === true"
                  >
                    {{ $t("make_offer") }}
                  </b-button>

                  <b-button
                    v-else
                    @click="modalOffer = !modalOffer"
                    :disabled="myBidPublished === true"
                    class="b-orange"
                  >
                    <!-- <template v-if="offered">
                      {{ $t("offer_submitted") }}
                    </template> -->
                    {{ $t("make_offer") }}
                  </b-button>
                  <b-button class="b-blue-outline">
                    {{ $t("message") }}</b-button
                  >

                  <b-button
                    v-if="contractorInfo.isFavorite"
                    @click="deleteFavourites"
                    class="b-orange-outline"
                  >
                    {{ $t("remove_from_bookmarks") }}</b-button
                  >
                  <b-button
                    v-else
                    @click="postFavourites"
                    class="b-orange-outline"
                  >
                    {{ $t("add_to_bookmarks") }}</b-button
                  >

                  <bidWithEstimate
                    v-if="
                      offerFull &&
                        contract &&
                        contract.estimateName !== null &&
                        myBidPublished === false
                    "
                    :offer-full="offerFull"
                    :contractor-info="contractorInfo"
                    :contract="contract"
                    :modal-edit-offer-full="modalWithEstimate"
                    :my-bid-already-exists="myBidAlreadyExists"
                    :my-bid-published="myBidPublished"
                    @getBids="getBids"
                  />

                  <bid
                    v-if="
                      offer &&
                        contract &&
                        contract.estimateName === null &&
                        myBidPublished === false
                    "
                    :offer="offer"
                    :contractor-info="contractorInfo"
                    :contract="contract"
                    :modal-edit-offer="modalOffer"
                    :my-bid-already-exists="myBidAlreadyExists"
                    :my-bid-published="myBidPublished"
                    @getBids="getBids"
                  />
                </div>
              </div>
            </div>
          </div>

          <div class="card p-4">
            <div v-if="$store.state.currentRole === 'Customer'">
              <div
                v-if="bids.length === 0"
                class="alert bg-grey alert-dismissible mb-2"
                role="alert"
              >
                <button
                  @click="waitingBet = !waitingBet"
                  type="button"
                  class="close"
                  data-dismiss="alert"
                  aria-label="Close"
                >
                  <span aria-hidden="true">×</span>
                </button>
                <div class="d-flex align-items-center">
                  <i class="bx bxs-hourglass-top mx-2"></i>
                  <span class="blue">
                    {{ $t("wait_a_bit") }}
                  </span>
                </div>
              </div>
            </div>
            <!-- Tabs with card integration -->
            <div>
              <b-tabs v-model="tabIndex" fill pills card>
                <b-tab
                  :title-link-class="tabClass(0)"
                  v-if="$store.state.currentRole === 'Customer'"
                  active
                >
                  <template #title>
                    <p class="my-0 fw-600">
                      {{ $t("betting") }}
                      <span class="counter counter-blue fs-13 mx-2">{{
                        bids.filter(
                          (b) => b.status === "Published" && b.favorite !== true
                        ).length
                      }}</span>
                    </p>
                  </template>
                  <betting
                    v-if="contract && contractorInfo"
                    :bids="
                      bids.filter(
                        (b) => b.status === 'Published' && b.favorite !== true
                      )
                    "
                    :contract="contract"
                    :contractor-info="contractorInfo"
                    :my-bid-already-exists="myBidAlreadyExists"
                    :my-bid-published="myBidPublished"
                    @getBids="getBids"
                  />
                </b-tab>
                <b-tab :title-link-class="tabClass(0)" v-else active>
                  <template #title>
                    <p class="my-0 fw-600">
                      {{ $t("betting") }}
                      <span class="counter counter-blue fs-13 mx-2">{{
                        bids.filter((b) => b.status === "Published").length
                      }}</span>
                    </p>
                  </template>
                  <betting
                    :bids="bids.filter((b) => b.status === 'Published')"
                    :contract="contract"
                    :contractor-info="contractorInfo"
                    :myBidAlreadyExists="myBidAlreadyExists"
                    :myBidPublished="myBidPublished"
                    @getBids="getBids"
                  />
                </b-tab>
                <b-tab :title-link-class="tabClass(1)">
                  <template #title>
                    <p class="my-0 fw-600">
                      {{ $t("rejected") }}
                      <span class="counter counter-orange fs-13 mx-1">
                        {{
                          bids.filter(
                            (b) =>
                              b.status === "Withdrawn" ||
                              b.status === "Rejected"
                          ).length
                        }}</span
                      >
                    </p>
                  </template>
                  <rejected
                    :bids="
                      bids.filter(
                        (b) =>
                          b.status === 'Withdrawn' || b.status === 'Rejected'
                      )
                    "
                    :contract="contract"
                    @getBids="getBids"
                  />
                </b-tab>
                <b-tab
                  :title-link-class="tabClass(2)"
                  v-if="
                    $store.state.currentRole === 'Customer' &&
                      $store.state.user &&
                      $store.state.user.id === contractorInfo.userId
                  "
                >
                  <template #title>
                    <p class="my-0 fw-600">
                      {{ $t("compare_estimates") }}
                      <span class="counter counter-grey fs-13 mx-2">
                        {{
                          bids.filter((b) => b.status === "Published").length
                        }}
                      </span>
                    </p>
                  </template>
                  <compareEstimates :id="id" />
                </b-tab>
                <b-tab
                  :title-link-class="tabClass(3)"
                  v-if="
                    $store.state.currentRole === 'Customer' &&
                      $store.state.user &&
                      $store.state.user.id === contractorInfo.userId
                  "
                >
                  <template #title>
                    <p class="my-0 fw-600">
                      {{ $t("bookmarks") }}
                      <span
                        class="fs-13 counter counter-orange-outline align-middle mx-1"
                      >
                        {{
                          bids.filter(
                            (b) =>
                              b.status === "Published" && b.favorite === true
                          ).length
                        }}
                      </span>
                    </p>
                  </template>
                  <bookmarks
                    :bids="
                      bids.filter(
                        (b) => b.status === 'Published' && b.favorite === true
                      )
                    "
                    :contract="contract"
                    @getBids="getBids"
                  />
                </b-tab>
                <b-tab :title-link-class="tabClass(4)">
                  <template #title>
                    <p class="my-0 fw-600">
                      {{ $t("discussion") }}
                      <span class="counter counter-blue-outline fs-13 mx-1">{{
                        discussionCount
                      }}</span>
                    </p>
                  </template>
                  <discussion />
                </b-tab>
              </b-tabs>
            </div>
          </div>
        </b-col>

        <b-col cols="3">
     
            <div class="card border-blue bg_grey">
              <div v-if="author" class="d-flex align-items-center m-3">
                <div>
                  <i
                    style="font-size: 60px"
                    width="60"
                    v-if="!author.avatar"
                    class=" bx bx-user-circle nav-i"
                  ></i>
                  <div
                    v-else
                    class="avatar"
                    v-bind:style="{
                      backgroundImage:
                        'url(' + '/img/src/' + author.avatar + ')',
                    }"
                  ></div>
                </div>
                <div class="mx-4">
                  <p class="h3">{{ $t("customer") }}</p>
                  <router-link class="blue" :to="{ name: 'profile' }">{{
                    author.fullName
                  }}</router-link>
                </div>
              </div>
              <div
                class="card-footer bg-white border-0 d-flex justify-content-around pb-4"
              >
                <span v-b-tooltip.hover :title="$t('Rating')" class="orange"
                  ><i class="bx bxs-star align-middle"></i
                  >{{ author.rating }}</span
                >

                <a
                  v-b-tooltip.hover
                  :title="$t('positive_reviews')"
                  class="mx-1"
                  href="#"
                  ><span class="blue"
                    ><i class="bx bxs-like  align-middle"></i>
                    {{ author.goodReviews }}</span
                  >
                </a>
                <a v-b-tooltip.hover :title="$t('negative_reviews')" href="#"
                  ><span class="grey-dark"
                    ><i class="bx bxs-dislike align-middle"></i>
                    {{ author.badReviews }}</span
                  ></a
                >
              </div>
              <div
                class=" bg-white"
                v-if="
                  $store.state.user &&
                    $store.state.user.id !== contractorInfo.userId
                "
              >
                <b-button class="b-blue mx-5 mt-3 mb-4 w-75">
                  {{ $t("message") }}
                </b-button>
              </div>
            </div>
       
          <premium
            v-if="
              $store.state.currentRole === 'Customer' &&
                contractorInfo &&
                $store.state.user &&
                contractorInfo.userId === $store.state.user.id
            "
            :id="id"
          />

          <div class="card border-blue">
            <table v-if="contractorInfo">
              <tbody>
                <tr>
                  <td class="bg_grey fs-20 px-2 text-center">
                    <i class="bx bx-time-five"></i>
                  </td>
                  <td class="px-2">
                    <p class="font-weight-bold mt-3 mb-0">
                      {{ $t("contract_published") }}
                    </p>
                    <p class="blue mt-0">{{ PublishedAgo }}</p>
                  </td>
                </tr>
                <tr>
                  <td class="bg_grey fs-20 px-2 text-center">
                    <i class="bx bx-show"></i>
                  </td>
                  <td class="font-weight-bold px-2">
                    {{ $t("views") }}: {{ contractorInfo.viewsCount }}
                  </td>
                </tr>
                <tr>
                  <td class="bg_grey fs-20 px-2 py-3 text-center">
                    <i class="bx bx-calendar"></i>
                  </td>
                  <td class="px-2 py-3">
                    {{ $t("before_closing") + ":" + " " }}
                    <span class="orange"> {{ BeforeClosing }} </span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>

          <div
            v-if="
              $store.state.currentRole === 'Customer' &&
                contractorInfo &&
                $store.state.user &&
                contractorInfo.userId === $store.state.user.id
            "
            class="card p-4"
          >
            <p class="h3">{{ $t("contract_management") }}</p>
            <b-button class="b-blue my-2" @click="modalShow = !modalShow">{{
              $t("extend")
            }}</b-button>
            <b-button class="b-blue-outline my-2" @click="clone()">{{
              $t("edit")
            }}</b-button>
            <a
              href="mailto:geral@diga.pt?subject=Suporte%20de%20contrato:%20"
              target="_blank"
              class="btn b-grey-outline my-2"
              >{{ $t("contact_support") }}</a
            >
            <b-button @click="Archive(id)" class="b-orange-outline my-2">{{
              $t("delete")
            }}</b-button>

            <b-modal size="sm" v-model="modalShow" hide-header hide-footer>
              <b-form-group :label="$t('extend_to')">
                <b-form-datepicker
                  v-model="contract.tenderEnd"
                  :min="contract.tenderEndDate"
                  :locale="$root.locale"
                  :placeholder="$t('select_date')"
                  id="tenderend-datepicker"
                  class="mb-2"
                ></b-form-datepicker>
              </b-form-group>

              <b-button class="btn b-blue" @click="submitExtend">{{
                $t("extend")
              }}</b-button>
            </b-modal>
          </div>
          <div v-else class="card p-4">
            <b-button
              v-b-tooltip.hover
              :title="$t('violation_popup_tooltip')"
              class="b-blue"
              @click="complainModal = !complainModal"
              >{{ $t("report_violation") }}</b-button
            >

            <b-modal v-model="complainModal" hide-footer>
              <template #modal-header>
                {{ $t("report_violation") }}: {{ contract.name }}
              </template>
              <b-form-textarea
                id="report"
                name="report"
                rows="8"
                :placeholder="$t('no_more_than_100_characters')"
              ></b-form-textarea>
              <b-button class="b-blue">{{
                $t("report_violation")
              }}</b-button>
            </b-modal>
          </div>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import ContractService from "@/services/ContractService.js";
import BidService from "@/services/BidService.js";
import betting from "./choosing/Betting.vue";
import rejected from "./choosing/Rejected";
import compareEstimates from "./choosing/CompareEstimates";
import bookmarks from "./choosing/Bookmarks";
import discussion from "./choosing/Discussion.vue";
import { required, maxLength } from "vuelidate/lib/validators";
import UserService from "@/services/UserService.js";
import FavouriteService from "@/services/FavouriteService.js";
import bidWithEstimate from "./choosing/MyBidWithEstimate.vue";
import bid from "./choosing/MyBid.vue";
import premium from "./Premium.vue";

export default {
  props: ["id"],
  components: {
    betting,
    rejected,
    compareEstimates,
    bookmarks,
    discussion,
    bidWithEstimate,
    bid,
    premium,
  },
  data() {
    return {
      tabIndex: 0,
      author: {},
      contract: {},
      complainModal: false,
      discussionCount: 5,
      counter: 4,
      waitingBet: true,
      contractorInfo: null,
      modalShow: false,
      modalOffer: false,
      modalWithEstimate: false,
      bids: [],
      offer: { contractId: this.id, daysNumber: null, price: null, text: "" },
      offerFull: {
        contractId: this.id,
        daysNumber: null,
        price: null,
        text: "",
        sections: [],
      },
      myBidAlreadyExists: false,
      myBidPublished: false,
    };
  },
  validations: {
    offer: {
      text: {
        required,
        maxLength: maxLength(1000),
      },
    },
    offerFull: {
      text: {
        required,
        maxLength: maxLength(1000),
      },
    },
  },
  methods: {
    tabClass(idx) {
      if (this.tabIndex === idx) {
        if (idx === 0 || idx === 4) {
          return ["tab-blue"];
        }
        if (idx === 1 || idx === 3) {
          return ["tab-orange"];
        }
        if (idx === 2) {
          return ["tab-grey"];
        }
      } else {
        return ["tab-contract"];
      }
    },
    getCard() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCard(this.contractorInfo.userId)
        .then((response) => {
          this.author = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    postFavourites() {
      this.$store.commit("IS_LOADING_TRUE");
      FavouriteService.postFavourites(this.id)
        .then(() => {
          this.contractorInfo.isFavorite = true;
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    deleteFavourites() {
      this.$store.commit("IS_LOADING_TRUE");
      FavouriteService.deleteFavourites(this.id)
        .then(() => {
          this.contractorInfo.isFavorite = false;
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    clone() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.CloneToEdit({
        id: this.contract.id,
      })
        .then((response) => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));

          this.$router.push({
            name: "Draft",
            params: { id: response.data.id },
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContract() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.id)
        .then((response) => {
          this.contract = response.data;
          this.checkContractStatus();
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContractor() {
      ContractService.getContractor(this.id)
        .then((response) => {
          this.contractorInfo = response.data;
          this.getCard();
          this.views();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getBids() {
      BidService.getBids(this.id)
        .then((response) => {
          this.bids = response.data.bids;
          if (response.data.userBid) {
            this.myBidAlreadyExists = true;
            this.myBidPublished = response.data.userBid.status === "Published";
            this.offer.daysNumber = response.data.userBid.daysNumber;
            this.offer.text = response.data.userBid.text;
            this.offer.price = response.data.userBid.price;

            this.offerFull.daysNumber = response.data.userBid.daysNumber;
            this.offerFull.text = response.data.userBid.text;
            this.offerFull.price = response.data.userBid.price;
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    submitExtend() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.Extend(this.contract.id, {
        contractId: this.contract.id,
        tenderEnd: this.contract.tenderEnd,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          location.reload();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    Archive(id) {
      ContractService.Archive(id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$router.push({ name: "home_index" });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    views() {
      if (
        this.$store.state.currentRole === "Executor" &&
        this.contractorInfo.userId !== this.$store.state.user.id
      ) {
        ContractService.views(this.id)
          .then(() => {})
          .catch(() => {});
      }
    },
    checkContractStatus() {
      switch (this.contract.status) {
        case "Draft":
          this.$router.push({
            name: "Draft",
            params: { id: this.id },
          });
          break;
        case "Deferred":
        case "Archived":
          this.$router.push({
            name: "allContracts",
          });
          break;
        // case "Contractor":
        //   this.$router.push({
        //     name: "Contractor",
        //     params: { id: this.id },
        //   });
        //   break;
        case "EstimateApproval":
          this.$router.push({
            name: "EstimateApproval",
            params: { id: this.id },
          });
          break;
        case "ContractApproval":
          this.$router.push({
            name: "ContractApproval",
            params: { id: this.id },
          });
          break;
        case "Signing":
          this.$router.push({
            name: "Signing",
            params: { id: this.id },
          });
          break;
        case "Refusal":
          this.$router.push({
            name: "ContractHistory",
            params: { id: this.id },
          });
          break;

        case "Executing":
          this.$router.push({
            name: "Executing",
            params: { id: this.id },
          });
          break;
        case "Closed":
          this.$router.push({
            name: "allContracts",
          });
          break;
        case "Finished":
          this.$router.push({
            name: "Finish",
            params: { id: this.id },
          });
          break;
      }
    },
  },
  computed: {
    // offered() {
    //   if (this.bids.length > 0) {
    //     if (this.bids.some((b) => b.userId === this.$store.state.user.id)) {
    //       return true;
    //     }
    //   }
    //   return false;
    // },
    PublishedAgo() {
      if (this.contract.publishDate) {
        var a = this.$moment(this.contract.publishDate);
        return a.fromNow();
      } else {
        return 0;
      }
    },
    BeforeClosing() {
      if (this.contract.tenderEndDate) {
        var a = this.$moment(this.contract.tenderEndDate);
        return a.fromNow();
      } else {
        return 0;
      }
    },
    ...mapGetters([
      "getCityById",
      "getCountryById",
      "getContractPriorityById",
      "getContractTypeById",
      "getBudgetById",
    ]),
  },
  watch: {},
  mounted() {
    this.getContract();
    this.getContractor();
    this.getBids();
  },
};
</script>
<style>
.fw-600 {
  font-weight: 500;
}
.tab-blue {
  border: 1px solid #438d90;
  color: #2f2c33 !important;
  background-color: #ccebec !important;
  border-radius: 0 !important;
}
.tab-orange {
  border: 1px solid #f16b24;
  color: #2f2c33 !important;
  background-color: #fdd6c1 !important;
  border-radius: 0 !important;
}
.tab-grey {
  border: 1px solid #606161;
  color: #2f2c33 !important;
  background-color: #e5e5e5 !important;
  border-radius: 0 !important;
}
.tab-contract {
  border: 1px solid #b4b4b4;
  border-radius: 0 !important;
}

.border-orange {
  border-top: 3px solid #f16b24 !important;
}
.border-blue {
  border-top: 3px solid #438d90 !important;
}
.fs-20 .bx {
  font-size: 20px;
}
.tabs .card-header {
  background-color: #e5e5e5;
}
</style>
