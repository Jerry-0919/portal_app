<template>
  <div class="card mb-0 p-4">
    <div>
      <p class="h4 mb-5">
        {{ $t("reviews_projects") }}
        <span class="counter counter-red fs-13 mx-2">{{ reviewsCount }}</span>
      </p>
      <template v-if="$store.state.currentRole === 'Executor'">
        <b-button @click="selectCategory(0)" class="m-1" variant="primary">{{
          $t("all_reviews")
        }}</b-button>
        <b-button
          @click="selectCategory(category.id)"
          class="m-1"
          variant="primary"
          v-for="category in filteredCategories"
          :key="category.id"
          >{{ category.nameId }}</b-button
        >
      </template>
    </div>
    <div v-for="review in reviews" :key="review.id" class="mt-4">
      <div class="border-left-primary">
        <div class="px-4">
          <div class="d-flex align-items-start">
            <a href="#" class="h5 media-heading mr-auto">{{
              $t(review.contractName)
            }}</a>
            <span
              v-b-tooltip.hover
              :title="$t('total_work_time')"
              class="badge badge-primary mr-1"
              >{{ workPeriod(review.publishDate) }} {{ $t("days") }}</span
            >
            <span
              v-b-tooltip.hover
              :title="$t('total_cost_of_work')"
              class="badge badge-success"
              >{{ review.price }} EUR</span
            >
          </div>

          <p class="h6 mt-4">
            {{ $t("what_liked") }}
            <i class="bx bxs-plus-circle text-success align-middle"></i>
          </p>
          <p>{{ review.likeText }}</p>
          <p class="h6 mt-4 text-right">
            {{ $t("suggestion_for_improvement") }}
            <i class="bx bxs-error-alt text-warning align-middle"></i>
          </p>
          <p class="text-right">{{ review.suggestionText }}</p>
          <div class="d-flex justify-content-between">
            <div>
              <div
                v-for="mark in review.marks"
                :key="mark.description"
                class="row"
              >
                <div class="col-6 my-0">
                  <label class="my-0">{{ $t(mark.description) }}</label>
                </div>
                <div class="col-6 my-0">
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
            </div>
            <div class="ml-auto align-self-center">
              <div
                class="ravatar review-avatar ml-auto"
                v-bind:style="{
                  backgroundImage:
                    'url(' + '/img/src/' + review.userAvatar + ')',
                }"
              ></div>
              <p>— {{ review.userName }}</p>
            </div>
          </div>

          <p class="fs-13 p-grey text-right mt-5">
            {{ $t("published_on") }}:
            {{ review.publishDate | moment("MMMM Do YYYY") }}
          </p>
        </div>
      </div>
      <hr />
    </div>

    <b-pagination
      v-if="reviewsCount > 6"
      class="justify-content-center"
      v-model="currentPage"
      :total-rows="reviewsCount"
      :per-page="take"
      aria-controls="my-table"
    ></b-pagination>
  </div>
</template>

<script>
import ReviewService from "@/services/ReviewService.js";
import StarRating from "vue-star-rating";
import { mapState } from "vuex";

export default {
  props: ["userCategories"],

  components: {
    StarRating,
  },
  data() {
    return {
      selectedCategoryId: 0,
      currentPage: 1,
      take: 6,
      reviewsCount: 0,
      reviews: [],
    };
  },
  methods: {
    selectCategory(categoryId) {
      this.selectedCategoryId = categoryId;
      this.getExecutorReviews();
    },
    workPeriod(review) {
      var a = this.$moment(review.buildStart);
      var b = this.$moment(review.plannedBuildEnd);

      return b.diff(a, "days");
    },
    getExecutorReviews() {
      this.$store.commit("IS_LOADING_TRUE");
      ReviewService.getExecutorReviews(
        this.skip,
        this.take,
        this.selectedCategoryId
      )
        .then((response) => {
          this.reviews = response.data.list;
          this.reviewsCount = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getCustomerReviews() {
      this.$store.commit("IS_LOADING_TRUE");
      ReviewService.getCustomerReviews(this.skip, this.take)
        .then((response) => {
          this.reviews = response.data.list;
          this.reviewsCount = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  computed: {
    filteredCategories() {
      if (this.categories.length > 0) {
        if (this.userCategories && this.userCategories.length > 0) {
          var result = [];
          this.categories.forEach((category) => {
            category.platformCategories.forEach((pc) => {
              if (this.userCategories.includes(pc.id)) {
                result.push(pc);
              }
            });
          });
          return result;
        } else {
          return this.categories;
        }
      } else {
        return [];
      }
    },
    ...mapState(["categories"]),

    skip() {
      return this.take * (this.currentPage - 1);
    },
  },
  mounted() {
    if (this.$store.state.currentRole === "Executor") {
      this.getExecutorReviews();
    } else {
      this.getCustomerReviews();
    }
  },
  watch: {
    "$store.state.currentRole": function() {
      this.reviews = [];
      this.reviewsCount = 0;
      if (this.$store.state.currentRole === "Executor") {
        this.getExecutorReviews();
      } else {
        this.getCustomerReviews();
      }
    },
  },
};
</script>

<style scoped>
.ravatar {
  background-size: cover;
  background-position: 50%;
  background-repeat: no-repeat;
  background-color: #ddd;
  border-radius: 50%;
}
.review-avatar {
  height: 60px;
  width: 60px;
}
img {
  width: 100%;
}
.border-left-primary {
  border-left: 3px solid #5a8dee;
}
</style>
