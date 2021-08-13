<template>
  <div v-if="profile" class="row">
    <div class="col-9">
      <div class="card p-4">
        <p v-if="profile.company && profile.company.name" class="h3 my-3">
          {{ profile.company.name }}
        </p>
        <p v-else class="h3 my-3">{{ profile.name }}</p>

        <hr />
        <div class="row">
          <div class="col-3 text-center">
            <div class="">
              <i
                width="300"
                v-if="!profile.avatar"
                class=" bx bx-user-circle nav-i"
              ></i>
              <div
                v-else
                class="avatar profile-avatar"
                v-bind:style="{
                  backgroundImage: 'url(' + '/img/src/' + profile.avatar + ')',
                }"
              ></div>
              <div class="my-3">
                <p v-if="profile.company && profile.company.name" class="h4">
                  {{ profile.name }}
                </p>
                <span class="chip mr-3">
                  <span class="chip-body">
                    <span
                      ><i
                        class="bx bx-star"
                        style="padding: 5px 0px; font-size: 1rem"
                      ></i
                    ></span>
                    <span v-if="profile.rating" style="padding: 5px 0px;">{{
                      profile.rating
                    }}</span>
                  </span>
                </span>
                <a
                  class="mx-1"
                  href="#"
                  
                  ><span v-if="profile.goodReviews >= 0" class="text-success"
                    ><i class="bx bxs-like"></i
                  ></span>
                  {{ profile.goodReviews }}</a
                >
                <a
                  href="#"

                  ><span
                    v-if="profile.badReviews >= 0"
                    class="text-light-danger"
                    ><i class="bx bxs-dislike "></i>
                    {{ profile.badReviews }}</span
                  ></a
                >
              </div>
              <p>
                {{ $t("position") }}: <b> {{ $t(profile.position) }} </b>
              </p>
            </div>
          </div>
          <div class="col-4">
            <div>
              <p class="h5">{{ $t("field_activity") }}</p>
              <ul>
                <li v-for="pc in profile.categories" :key="pc">{{ $t(pc) }}</li>
              </ul>
            </div>
            <div class="mt-2">
              <p class="h5">{{ $t("additionally") }}</p>
              <ul>
                <li v-for="cc in profile.subCategories" :key="cc">
                  {{ $t(cc) }}
                </li>
              </ul>
            </div>
          </div>
          <div class="col-4">
            <p class="h5">
              {{ $t("average_rating") }}
              <i
                v-b-tooltip.hover
                :title="$t('based_on_reviews')"
                class="fs-14 bx bxs-info-circle align-middle"
              ></i>
            </p>
            <ul class="marks">
              <li
                class="d-flex justify-content-between"
                v-for="(mark, index) in profile.reviews"
                :key="index"
              >
                <div>{{ $t(mark.textId) }}</div>

                <div>
                  <star-rating
                    :increment="0.5"
                    :show-rating="false"
                    class="my-0"
                    :star-size="20"
                    read-only
                    :rating="mark.value"
                  />
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div v-if="profile.company && profile.company.about" class="card p-4">
        <p class="h5 text-center">{{ $t("about_us") }}</p>
        <p>{{ profile.company.about }}</p>
      </div>
      <div v-if="profile.portfolioVideos.length !== 0" class="card p-4">
        <p class="h5 text-center">{{ $t("portfolio_video") }}</p>
        <div class="row">
          <div
            class="col-4 m-3"
            v-for="v in profile.portfolioVideos"
            :key="v.id"
          >
            <p class="h6 my-2 text-center">
              {{ $t("date_of_publication") }} :
              {{ v.publishDate | moment("DD MMMM YYYY") }}
            </p>
            <iframe
              :src="'https://www.youtube.com/embed/' + getId(v.value)"
              title="YouTube video player"
              frameborder="0"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowfullscreen
            ></iframe>
          </div>
        </div>
      </div>
      <div v-if="profile.portfolioAlbums.length !== 0" class="card p-4">
        <p class="h5 text-center">{{ $t("portfolio_photo") }}</p>
        <div
          class="my-3"
          v-for="album in profile.portfolioAlbums"
          :key="album.id"
        >
          <p class="h6 font-weight-bold">
            {{ album.name }}
          </p>
          <p class="w-75 fs-14">
            {{ album.description }}
          </p>
          <div class="row">
            <div class="col-3" v-for="photo in album.photos" :key="photo.id">
              <span class="d-flex">
                <img :src="photo" alt="" />
              </span>
            </div>
          </div>
          <p class="fs-13 p-grey">
            {{ $t("published_on") }}:
            {{ album.publishDate | moment("MMMM Do YYYY") }}
          </p>
          <hr />
        </div>
      </div>
      <div v-if="reviewsCount > 0" class="card p-4">
        <p class="h4 mb-5">
          {{ $t("reviews_projects") }}
          <span class="counter counter-red fs-13 mx-2">{{ reviewsCount }}</span>
        </p>
        <div
          v-for="review in reviews"
          :key="review.id"
          class="mt-4 position-relative"
        >
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
                <i
                  style="vertical-align: middle"
                  class="bx bxs-plus-circle text-success"
                ></i>
              </p>
              <p>{{ review.likeText }}</p>
              <p class="h6 mt-4 text-right">
                {{ $t("suggestion_for_improvement") }}
                <i
                  style="vertical-align: middle"
                  class="bx bxs-error-alt text-warning"
                ></i>
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
                  <p class="my-3 ml-auto">— {{ review.userName }}</p>
                </div>
              </div>
              <div class="mt-5 text-right">
                <p class="fs-13 p-grey text-right">
                  {{ $t("published_on") }}:
                  {{ review.publishDate | moment("MMMM Do YYYY") }}
                </p>
              </div>
            </div>
          </div>
          <hr />
        </div>
      </div>
    </div>
    <div class="col-3">
      <div class="card p-4">
        <p class="h5">{{ $t("general_info") }}:</p>
        <p class="success">
          <i class="bx bxs-check-square align-middle"></i>
          {{ $t(profile.loadStatus) }}
        </p>
        <p>
          <i class="bx bxs-flag-alt align-middle"></i> {{ profile.nationality }}
        </p>
        <p>
          <i class="bx bxs-building-house text-warning align-middle"></i>
          {{ $t("in_work") }}: {{ profile.contractsInWork }}
        </p>
        <p>
          <i class="bx bx-task text-primary align-middle"></i>
          {{ $t("completed_projects") }}: {{ profile.contractsCompleted }}
        </p>
        <p>
          <i class="bx bxs-star-half text-primary align-middle"></i>
          {{ $t("average_rating") }}: {{ profile.averageReview }}
        </p>
      </div>
      <div class="card p-4">
        <p class="h4 mb-3">{{ $t("communication") }}</p>

        <router-link
          :to="{ name: 'chatroom', params: { roomId: this.profile.id } }"
          class="btn btn-primary my-2"
          type="button"
          >{{ $t("write") }}
          <span class="text-lowercase">{{ $t("message") }} </span></router-link
        >
        <b-button variant="primary my-2"
          >{{ $t("invite_to_project") }}
        </b-button>
        <b-button variant="primary my-2">{{ $t("offer_a_job") }} </b-button>
        <b-button variant="primary my-2">{{ $t("add_to_team") }} </b-button>
        <b-button
          v-b-tooltip.hover
          :title="$t('violation_popup_tooltip')"
          variant="primary my-2"
          @click="complainProfileModal = !complainProfileModal"
          >{{ $t("make_complaint") }}</b-button
        >
        <b-modal v-model="complainProfileModal">
          <template #modal-header>
            {{ $t("make_complaint") }}: {{ profile.name }}
          </template>
          <b-form-textarea
            v-model="complaintText"
            id="complaintProfile"
            name="complaintProfile"
            rows="8"
            :placeholder="$t('no_more_than_100_characters')"
          ></b-form-textarea>
          <template #modal-footer>
            <b-button @click="makeComplaint" variant="primary">{{
              $t("make_complaint")
            }}</b-button>
          </template>
        </b-modal>
      </div>
    </div>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
import StarRating from "vue-star-rating";

export default {
  props: ["id"],
  components: { StarRating },
  data() {
    return {
      profile: {},
      complainProfileModal: false,
      skip: 0,
      take: 4,
      reviews: [],
      reviewsCount: 0,
      complaintText: "",
    };
  },
  methods: {
    makeComplaint() {
      UserService.makeComplaint({
        userId: this.id,
        text: this.complaintText,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.complainProfileModal = false
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    workPeriod(review) {
      var a = this.$moment(review.buildStart);
      var b = this.$moment(review.plannedBuildEnd);

      return b.diff(a, "days");
    },
    getProfileReviews() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getPublicProfileReviews(this.id, this.skip, this.take)
        .then((response) => {
          this.reviews = response.data.list;
          this.reviewsCount = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getPublicProfile() {
      UserService.getPublicProfile(this.id)
        .then((response) => {
          this.profile = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getId(url) {
      const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/;
      const match = url.match(regExp);

      return match && match[2].length === 11 ? match[2] : null;
    },
  },
  mounted() {
    this.getPublicProfile();
    this.getProfileReviews();
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
.profile-avatar {
  height: 200px;
  width: 200px;
}
.marks {
  list-style-type: none;
  padding: 0;
  margin: 0;
}
img {
  width: 100%;
}
.review-avatar {
  height: 60px;
  width: 60px;
}

.border-left-primary {
  border-left: 3px solid #5a8dee;
}
</style>
