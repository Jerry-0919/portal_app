<template>
  <div v-if="contract && contractorInfo && $store.state.user">
    <div class="d-inline-block my-4">
      <span class="h4 mr-3"> {{ $t("contract") }}: {{ contract.name }} </span>
      <span class="ctype fs-14"
        >{{ $t("contract_type") }}:
        <template v-if="getContractTypeById(contract.contractTypeId)">
          {{ $t(getContractTypeById(contract.contractTypeId).name) }}
        </template>
      </span>
      <span class=" font-weight-normal mx-3 success">
        {{ $t("status_completed") }}
      </span>
    </div>
    <template>
      <b-breadcrumb class="my-0">
        <b-breadcrumb-item
          class=""
          :to="{ name: 'home_index' }"
        >
          <b-icon
            icon="house-fill"
            scale="1.25"
            shift-v="1.25"
            aria-hidden="true"
          ></b-icon>
        </b-breadcrumb-item>
        <b-breadcrumb-item
          class=""
          :to="{ name: 'contracts' }"
          >{{ $t("contracts") }}</b-breadcrumb-item
        >
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("feedback_exchange") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("completion") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("Finish") }}</b-breadcrumb-item>
        <b-breadcrumb-item
          class=" blue"
          :to="{ name: 'ContractHistory', params: { id: this.id } }"
          >{{ $t("contract_history") }}</b-breadcrumb-item
        >
      </b-breadcrumb>
    </template>
    <div class="container-fluid bg-grey">
      <div class="row">
        <div class="col-9">
          <div class="card p-4">
            <p class="h4 mb-3">{{ $t("contract_history") }}</p>

                   <div>
              <b-form-group label-class="p-4" :label="$t('contract')">
                <b-button-group
                  class="d-flex justify-content-between py-2 px-4 bg_grey"
                >
                  <b-button
                    :class="{
                      'b-blue-option-active': selected === 'chat',
                    }"
                    class="b-option"
                    @click="selected = 'chat'"
                  >
                    {{ $t("chat") }}
                  </b-button>
                  <b-button
                    :class="{
                      'b-orange-option-active':
                        selected === 'history',
                    }"
                    class="b-option ml-1"
                    @click="selected = 'history'"
                  >
                    {{ $t("changes_history") }}
                  </b-button>
                </b-button-group>
              </b-form-group>
            </div>
            <div v-if="selected === 'history'">
              <history :id="id" />
            </div>
            <div v-else>
              <chat />
            </div>
          </div>
        </div>
        <div class="col-3">
          <div class="card p-4">
            <div class="card-body p-0">
              <p class="h4 mb-3">{{ $t("customer") }}</p>
              <div
                v-if="author"
                class=" d-flex align-items-center"
              >
                <div class="mr-3 ml-0">
                  <i
                    style="font-size: 60px"
                    width="60"
                    v-if="!author.avatar"
                    class=" bx bx-user-circle nav-i"
                  ></i>
                  <div
                    v-else
                    class="avatar "
                    v-bind:style="{
                      backgroundImage:
                        'url(' + '/img/src/' + author.avatar + ')',
                    }"
                  ></div>
                  <!-- <img
                    v-else
                    class="rounded-circle"
                    :src="'/img/src/' + author.avatar"
                    width="60"
                    alt=""
                  /> -->
                </div>
                <div>
                  <a href="#"
                    ><p class="h4 blue">{{ author.name }}</p></a
                  >
                  <div>
  <span
                     v-b-tooltip.hover
                      :title="$t('Rating')"
                     class="orange"
                      ><i class="bx bxs-star align-middle"></i
                      >{{ author.rating }}</span
                    >
                    <a
                      v-b-tooltip.hover
                      :title="$t('positive_reviews')"
                      class="mx-1"
                      href="#"
                      ><span class="blue"
                        ><i class="bx bxs-like align-middle"></i>
                        {{ author.goodReviews }}</span
                      >
                    </a>
                    <a
                      v-b-tooltip.hover
                      :title="$t('negative_reviews')"
                      href="#"
                      ><span class="grey-dark"
                        ><i class="bx bxs-dislike align-middle"></i>
                        {{ author.badReviews }}</span
                      ></a
                    >
                  </div>

                  <div
                    v-if="progress.finishCustomer === true"
                    class="users-view-id text-success"
                  >
                    <div>
                      <i class="bx bxs-check-square align-middle"></i>
                      {{ $t("project_accepted") }}
                    </div>
                    <div>
                      <i class="bx bx-calendar align-middle"></i>
                      {{ contract.dateTime | moment("dd/mm/yyyy hh:mm") }}
                    </div>
                  </div>
                  <div v-else class="text-danger">
                    {{ $t("under_consideration") }}
                    <i class="bx bxs-minus-circle align-middle"></i>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="card p-4">
            <div class="card-body p-0">
              <p class="h4 mb-3">{{ $t("executor") }}</p>
              <div class=" d-flex align-items-center">
                <div class="mr-3 ml-0">
                  <i
                    style="font-size: 60px"
                    width="60"
                    v-if="!executorCard.avatar"
                    class=" bx bx-user-circle nav-i"
                  ></i>
                  <div
                    v-else
                    class="avatar "
                    v-bind:style="{
                      backgroundImage:
                        'url(' + '/img/src/' + executorCard.avatar + ')',
                    }"
                  ></div>
                  <!-- <img
                    v-else
                    class="rounded-circle"
                    :src="'/img/src/' + executorCard.avatar"
                    width="60"
                    alt=""
                  /> -->
                </div>
                <div>
                  <a href="#"
                    ><p class="h5">{{ executorCard.name }}</p></a
                  >
                  <div>
  <span
                     v-b-tooltip.hover
                      :title="$t('Rating')"
                     class="orange"
                      ><i class="bx bxs-star align-middle"></i
                      >{{ executorCard.rating }}</span
                    >
                    <a
                      v-b-tooltip.hover
                      :title="$t('positive_reviews')"
                      class="mx-1"
                      href="#"
                      ><span class="blue"
                        ><i class="bx bxs-like align-middle"></i>
                        {{ executorCard.goodReviews }}</span
                      >
                    </a>
                    <a
                      v-b-tooltip.hover
                      :title="$t('negative_reviews')"
                      href="#"
                      ><span class="grey-dark"
                        ><i class="bx bxs-dislike align-middle"></i>
                        {{ executorCard.badReviews }}</span
                      ></a
                    >
                    <div
                      v-if="progress.finishExecutor === true"
                      class="users-view-id text-success"
                    >
                      <div>
                        <i class="bx bxs-check-square align-middle"></i>
                        {{ $t("payment_received") }}
                      </div>
                      <div>
                        <i class="bx bx-calendar align-middle"></i>
                        {{ contract.dateTime | moment("dd/mm/yyyy hh:mm") }}
                      </div>
                    </div>

                    <!-- <div v-else class="text-danger">
                      {{ $t("under_consideration") }}
                      <i class="bx bxs-minus-circle align-middle"></i>
                    </div> -->
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="card p-4">
            <p class="h4 mb-3">{{ $t("contract_management") }}</p>

            <b-button
              v-if="$store.state.user.id === this.contractorInfo.userId"
              variant="primary my-2"
              @click="clone()"
              >{{ $t("open_similar_contract") }}</b-button
            >
            <a
              href="mailto:geral@diga.pt?subject=Suporte%20de%20contrato:Executing%20"
              target="_blank"
              class="btn btn-primary my-2"
              >{{ $t("contact_support") }}</a
            >
          </div>
        </div>
        <div class="col-12">
          <div class="card p-4">
            <div class="row">
              <b-col>
                <p class="h5 mb-3">{{ $t("customer_review") }}</p>
                <div
                  v-for="mark in customerReview.marks"
                  :key="mark.description"
                  class="row"
                >
                  <div class="col-4 my-0">
                    <label class="my-0">{{ $t(mark.description) }}</label>
                  </div>
                  <div class="col-3 my-0">
                    <star-rating
                      :increment="0.5"
                      :show-rating="false"
                      class="my-0"
                      :star-size="20"
                      read-only
                      :rating="mark.value"
                    />
                  </div>
                </div>
                <p class="my-3">{{ customerReview.likeText }}</p>
                <p class="fs-13 p-grey">
                  {{ $t("published_on") }}:
                  {{ customerReview.publishDate | moment("MMMM Do YYYY") }}
                </p>
              </b-col>
            </div>
            <div class="row my-3">
              <div class="col-6"></div>
              <div class="col-6">
                <p class="h5 mb-3">{{ $t("executor_review") }}</p>
                <div
                  v-for="mark in executorrReview.marks"
                  :key="mark.description"
                  class="row"
                >
                  <div class="col-4 my-0">
                    <label class="my-0">{{ $t(mark.description) }}</label>
                  </div>
                  <div class="col-3 my-0">
                    <star-rating
                      :increment="0.5"
                      :show-rating="false"
                      class="my-0"
                      :star-size="20"
                      read-only
                      :rating="mark.value"
                    />
                  </div>
                </div>
                <p class="my-3">{{ executorrReview.likeText }}</p>
                <p class="fs-13 p-grey">
                  {{ $t("published_on") }}:
                  {{ executorrReview.publishDate | moment("MMMM Do YYYY") }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import ContractService from "@/services/ContractService.js";
import UserService from "@/services/UserService.js";
import ReviewService from "@/services/ReviewService.js";
import chat from "../Chat.vue";
import history from "./History.vue";
import StarRating from "vue-star-rating";

export default {
  props: ["id"],
  components: { chat, history, StarRating },
  data() {
    return {
      errors: {},
      selected: "history",
      options: [
        { text: this.$t("chat"), value: "chat" },
        { text: this.$t("changes_history"), value: "history" },
      ],
      contract: {},
      progress: {},
      executorCard: {},
      author: {},
      contractorInfo: null,
      customerReview: {},
      executorrReview: {},
    };
  },
  methods: {
    getCustomerReview() {
      ReviewService.getCustomerReview(this.id)
        .then((response) => {
          this.customerReview = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getExecutorReview() {
      ReviewService.getExecutorReview(this.id)
        .then((response) => {
          this.executorrReview = response.data;
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

    finishContract() {
      ContractService.finishContract(this.id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getProgress();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getContractor() {
      ContractService.getContractor(this.id)
        .then((response) => {
          this.contractorInfo = response.data;
          this.getCard();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
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
    getExecutorCard() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCard(this.progress.executorId)
        .then((response) => {
          this.executorCard = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getProgress() {
      ContractService.getProgress(this.id)
        .then((response) => {
          this.progress = response.data;

          this.getExecutorCard();
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
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getContract();
    this.getContractor();
    this.getProgress();
    this.getExecutorReview();
    this.getCustomerReview();
  },
  computed: {
    ...mapGetters(["getContractTypeById"]),
  },
};
</script>
<style>
.widget-timeline li {
  padding: 1.1rem 0;
  list-style: none;
  position: relative;
}
.widget-timeline li.timeline-items.success:before {
  background: #39da8a !important;
}
.widget-timeline li.timeline-items:before {
  position: absolute;
  content: "";
  left: -37px;
  top: 17px;
  border: 3px solid #ffffff;
  box-shadow: 1px 2px 6px 0 rgb(25 42 70 / 30%);
  border-radius: 50%;
  background: #5a8dee;
  height: 13px;
  width: 13px;
  z-index: 2;
}
.widget-timeline li .timeline-time {
  float: right;
  color: #828d99;
  font-size: 0.9rem;
}
.timeline-title {
  font-family: "Rubik", Helvetica, Arial, serif;
  font-weight: 400;
  line-height: 1.2;
  color: #475f7b;
  font-size: 1rem;
}
.widget-timeline li.timeline-items .timeline-content {
  padding: 0.5rem 1rem;

  border-radius: 0.267rem;
  display: flex;
  align-items: center;
  color: #727e8c;
  font-size: 0.9rem;
}
.widget-timeline li.timeline-items:not(:last-child):after {
  position: absolute;
  content: "";
  width: 1px;
  background: #dfe3e7;
  left: -31px;
  top: 22px;
  height: 100%;
  z-index: 1;
}
</style>
