<template>
  <div class="m-3">
    <div class="orange-header h1 px-4 py-3">
      {{ $t("profile") }}
    </div>
    <div class="my-3 d-flex justify-content-start">
      <p>{{ $t("permanent_link_to_your_profile") }}</p>
      <div class="mx-4">
        <b-input
          @focus="$event.target.select()"
          id="publicLink"
          class="fs-13"
          :value="formatLink()"
        >
        </b-input>
      </div>
      <i
        type="button"
        v-clipboard="formatLink()"
        class="bx bxs-copy align-middle"
        v-b-tooltip.hover
        :title="$t('copy_link')"
        data-toggle="tooltip"
      ></i>
    </div>
    <div class="row" v-if="info">
      <div v-if="info" class="col-3">
        <div class="card p-4">
          <div class="card-body p-0 text-center">
            <div class="mb-1">
              <i
                width="300"
                v-if="!info.avatar"
                class=" bx bx-user-circle nav-i"
              ></i>
              <div
                v-else
                class="avatar profile-avatar"
                v-bind:style="{
                  backgroundImage:
                    'url(' +
                    (info.isTemp === true ? '/img/temp/' : '/img/src/') +
                    info.avatar +
                    ')',
                }"
              ></div>
            </div>
            <label for="photo-upload">
              {{ $t("upload_photo") }}
            </label>
            <b-form-file
              id="photo-upload"
              accept="image/*"
              v-model="avatar"
              class="a"
              plain
            ></b-form-file>
            <a href="#"> </a>
            <div class="mt-4">
              <p v-if="roles.some((r) => r.role === 'PlatformExecutorMaster')">
                {{ $t("specialist") }}
              </p>
              <p v-if="roles.some((r) => r.role === 'PlatformExecutorTeam')">
                {{ $t("brigade") }}
              </p>
              <p v-if="roles.some((r) => r.role === 'PlatformExecutorCompany')">
                {{ $t("company") }}
              </p>
              <a href="#"
                ><p class="h4">{{ info.name }} {{ info.surname }}</p></a
              >

              <div>
                <span class="chip mr-3">
                  <span class="chip-body">
                    <span
                      ><i
                        class="bx bx-star"
                        style="padding: 5px 0px; font-size: 1rem"
                      ></i
                    ></span>
                    <span v-if="rating" style="padding: 5px 0px;">{{
                      rating
                    }}</span>
                  </span>
                </span>
                <a class="mx-1" href="#"
                  ><span v-if="goodReviews >= 0" class="text-success"
                    ><i class="bx bxs-like"></i
                  ></span>
                  {{ goodReviews }}</a
                >
                <a href="#"
                  ><span v-if="badReviews >= 0" class="text-light-danger"
                    ><i class="bx bxs-dislike "></i> {{ badReviews }}</span
                  ></a
                >
              </div>
              <p class="h5 mt-5 mb-3">{{ $t("contact_info") }}</p>
              <div class="d-flex justify justify-content-between">
                <p class="font-weight-bold">Email</p>
                <p>{{ info.email }}</p>
              </div>

              <div class="d-flex justify justify-content-between mb-5">
                <p class="font-weight-bold">{{ $t("telephone") }}</p>
                <p>{{ info.mainPhoneNumber }}</p>
              </div>
              <router-link
                :to="{ name: 'public_profile_inside', params: { id: info.id } }"
                class="btn btn-primary"
                type="button"
                target="_blank"
                >{{ $t("public_profile") }}</router-link
              >
              <b-button
                v-if="
                  $store.state.user.verificationStatus &&
                    $store.state.user.verificationStatus === null
                "
                v-b-modal.verificationModal
                variant="warning"
              >
                {{ $t("verify") }}
              </b-button>
              <p
                v-if="
                  $store.state.user.verificationStatus &&
                    $store.state.user.verificationStatus ===
                      'UnderConsideration'
                "
                class="my-3"
              >
                {{ $t("verification_progress") }}
              </p>
            </div>
          </div>
        </div>
      </div>
      <div class="col-9">
        <b-tabs v-model="tabIndex" content-class="mt-3">
          <b-tab :title="$t('personal_data')" active
            ><personalData :info="info" :roles="roles"
          /></b-tab>
          <b-tab :title="$t('company_data')"
            ><companyData :companyInfo="companyInfo"
          /></b-tab>
          <b-tab :title="$t('Safe')"
            ><safe :companyInfo="companyInfo" :info="info" @getUser="Info"
          /></b-tab>
          <b-tab :title="$t('settings')"
            ><profileSettings :settings="settings"
          /></b-tab>
          <b-tab
            v-if="$store.state.currentRole === 'Executor'"
            :title="$t('specialization')"
            ><specialization
              @fetchCategories="getCategories"
              :userTags="userTags"
              :userCategories="userCategories"
          /></b-tab>
          <b-tab
            v-if="$store.state.currentRole === 'Executor'"
            :title="$t('portfolio')"
            ><portfolio :userCategories="userCategories"
          /></b-tab>
          <b-tab :title="$t('reviews')"
            ><reviews :userCategories="userCategories"
          /></b-tab>
          <!-- <b-tab :title="$t('subscriptions')"><subscriptions /></b-tab> -->
        </b-tabs>
      </div>
    </div>
  </div>
</template>

<script>
import personalData from "@/components/profile/PersonalData.vue";
import companyData from "@/components/profile/CompanyData.vue";
import safe from "@/components/profile/Safe.vue";
import portfolio from "@/components/profile/Portfolio.vue";
import profileSettings from "@/components/profile/ProfileSettings.vue";
import reviews from "@/components/profile/Reviews.vue";
import specialization from "@/components/profile/Specialization.vue";
// import subscriptions from "@/components/profile/Subscriptions.vue";
import UserService from "@/services/UserService.js";
import FileService from "@/services/FileService.js";
export default {
  components: {
    personalData,
    companyData,
    profileSettings,
    specialization,
    portfolio,
    reviews,
    safe,
    // subscriptions,
  },
  data() {
    return {
      avatar: null,
      info: { resume: "" },
      companyInfo: { about: "" },
      settings: {},
      tabIndex: 0,
      roles: [],
      userTags: [],
      userCategories: [],
      userCategoriesLoaded: false,
      rating: 0,
      goodReviews: 0,
      badReviews: 0,
    };
  },
  methods: {
    formatLink() {
      return (
        window.location.host +
        "/platform/index#/public-profile/" +
        this.$store.state.user.id
      );
    },
    getCategories() {
      UserService.getCategories()
        .then((response) => {
          this.userCategories = response.data.map((t) => {
            return t.id;
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getTags() {
      UserService.getTags()
        .then((response) => {
          this.userTags = response.data.map((t) => {
            return t.name;
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    uploadPhoto() {
      if (this.avatar) {
        let formData = new FormData();
        formData.append("file", this.avatar);
        this.$store.commit("IS_LOADING_TRUE");
        FileService.uploadPhoto(formData)
          .then((response) => {
            this.info.avatar = response.data.fileName;
            this.avatar = null;
            this.info.isTemp = true;
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    getRoles() {
      UserService.getRoles()
        .then((response) => {
          this.roles = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    Info() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getUser()
        .then((response) => {
          this.info = response.data;
          this.info.isTemp = false;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    CompanyInfo() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getCompanyInfo()
        .then((response) => {
          this.companyInfo = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getUserSettings() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getSettings()
        .then((response) => {
          this.settings = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getRating() {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.getUserCard()
        .then((response) => {
          this.rating = response.data.rating;
          this.goodReviews = response.data.goodReviews;
          this.badReviews = response.data.badReviews;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.Info();
    this.CompanyInfo();
    this.getRoles();
    this.getTags();
    this.getCategories();
    this.getRating();
    this.getUserSettings();
  },
  watch: {
    avatar() {
      this.uploadPhoto();
    },
  },
};
</script>
<style scoped>
.profile-avatar {
  height: 200px;
  width: 200px;
}
input[type="file"] {
  display: none;
}
.photo-upload {
  cursor: pointer;
}
.photo-upload:hover {
  color: blue;
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
