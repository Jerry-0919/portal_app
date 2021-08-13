<template>
  <div v-if="contractorInfo && $store.state.user">
    <div class="container-fluid bg-grey">
      <div class="card">
        <div class="card border-0 mb-0 p-4">
          <p class="h4 mb-3">{{ $t("successful_completion") }}</p>
          <div class="bg-text">
            <img
              class="float-right"
              width="50"
              src="http://platform.diga.pt/contractor/app-assets/images/circle-cropped.png"
            />
            <p v-if="progress.executorId !== this.$store.state.user.id">
              {{ $t("review_text") }} {{ executorCard.name }}
            </p>
            <p v-else>{{ $t("review_text_customer") }} {{ author.name }}</p>
          </div>
        </div>
        <div class="card border-0 mb-0 p-4">
          <p class="h5 border-bottom">{{ $t("result_of_work") }}</p>
          <p class="my-2 fs-13">{{ $t("no_more_500") }}</p>

          <b-form-file
            class=" my-2"
            id="edit-photo-upload"
            multiple
            v-model="files"
            plain
            accept=".jpg, .jpeg, .png, .pdf, .mp4"
          ></b-form-file>
          <div v-for="doc in uploadedDocs" :key="doc">
            <p>
              {{ doc }}
              <span class="btn text-red" @click="removeOneTempDoc(doc)">x</span>
            </p>
          </div>
          <div v-if="uploadedFiles.length > 0" class="row dropzone">
            <div class="col-3" v-for="file in uploadedFiles" :key="file">
              <span class="d-flex">
                <img
                  class="position-relative"
                  :src="'/img/temp/' + file"
                  alt=""
                />
                <span
                  class="remove-photo btn text-red"
                  @click="removeOneTempImage(file)"
                  >x</span
                ></span
              >
            </div>
          </div>
        </div>
        <div class="card border-0 mb-0 p-4">
          <p class="h5 mb-3 border-bottom">{{ $t("marks_in_review") }}</p>
          <div v-if="progress.executorId !== $store.state.user.id">
            <div
              v-for="cmark in marksCustomer"
              :key="cmark.description"
              class="m-auto w-50 pb-2"
            >
              <div class="d-flex align-items-center justify-content-between">
                <label class="text-uppercase">
                  {{ $t(cmark.description) }}
                </label>
                <star-rating
                  class="m-auto"
                  :increment="0.5"
                  :show-rating="false"
                  :star-size="35"
                  v-model="cmark.value"
                />
                <span class="bg-soft-primary rounded px-1 text-primary">
                  {{ cmark.value }} {{ $t("out_of") }} 5</span
                >
              </div>
              <p class="text-center">{{ $t(cmark.textId) }}</p>
            </div>
          </div>
          <div v-else>
                        <div
              v-for="emark in marksExecutor"
              :key="emark.description"
              class="m-auto w-50 pb-2"
            >
              <div class="d-flex align-items-center justify-content-between">
                <label class="text-uppercase">
                  {{ $t(emark.description) }}
                </label>
                <star-rating
                  class="m-auto"
                  :increment="0.5"
                  :show-rating="false"
                  :star-size="35"
                  v-model="emark.value"
                />
                <span class="bg-soft-primary rounded px-1 text-primary">
                  {{ emark.value }} {{ $t("out_of") }} 5</span
                >
              </div>
              <p class="text-center">{{ $t(emark.textId) }}</p>
            </div>

          </div>
          <!-- <div v-else class="m-auto w-50">
              <div class="d-flex justify-content-between my-1">
                <label class=""> {{ $t("value") }} </label>
                <star-rating
                  :increment="0.5"
                  :show-rating="false"
                  class="my-0"
                  :star-size="25"
                />
                <span class="bg-soft-primary p-1 rounded text-primary"
                  >3.5 из 5</span
                >
              </div>
              <p class="text-center">{{ $t("value_text") }}</p>
              <div class="d-flex justify-content-between my-1">
                <label class=""> {{ $t("quality_of_work") }} </label>
                <star-rating
                  :increment="0.5"
                  :show-rating="false"
                  class="my-0"
                  :star-size="25"
                />
                <span class="bg-soft-primary p-1 rounded text-primary"
                  >3.5 из 5</span
                >
              </div>
              <p class="text-center">{{ $t("quality_of_work_text") }}</p>
              <div class="d-flex justify-content-between my-1">
                <label class=""> {{ $t("professionalism") }} </label>
                <star-rating
                  :increment="0.5"
                  :show-rating="false"
                  class="my-0"
                  :star-size="25"
                />
                <span class="bg-soft-primary p-1 rounded text-primary"
                  >3.5 из 5</span
                >
              </div>
              <p class="text-center">{{ $t("professionalism_text") }}</p>
              <div class="d-flex justify-content-between my-1">
                <label class=""> {{ $t("contact_rate") }} </label>
                <star-rating
                  :increment="0.5"
                  :show-rating="false"
                  class="my-0"
                  :star-size="25"
                />
                <span class="bg-soft-primary p-1 rounded text-primary"
                  >3.5 из 5</span
                >
              </div>
              <p class="text-center">{{ $t("contact_rate_text") }}</p>
              <div class="d-flex justify-content-between my-1">
                <label class=""> {{ $t("timing_project") }} </label>
                <star-rating
                  :increment="0.5"
                  :show-rating="false"
                  class="my-0"
                  :star-size="25"
                />
                <span class="bg-soft-primary p-1 rounded text-primary"
                  >3.5 из 5</span
                >
              </div>
              <p class="text-center">{{ $t("timing_project_text") }}</p>
              <div class="d-flex justify-content-between my-1">
                <label class=""> {{ $t("punctuality") }} </label>
                <star-rating
                  :increment="0.5"
                  :show-rating="false"
                  class="my-0"
                  :star-size="25"
                />
                <span class="bg-soft-primary p-1 rounded text-primary"
                  >3.5 из 5</span
                >
              </div>
              <p class="text-center">{{ $t("punctuality") }}</p>
            </div> -->
        </div>
        <div class="card border-0 mb-0 p-4">
          <div class="row">
            <p class="col-12 h5 mb-3 border-bottom">{{ $t("review") }}</p>
            <p class="col-12 fs-13">{{ $t("review_p") }}</p>
            <b-col cols="6">
              <b-form-group :label="$t('what_liked')">
                <b-form-input
                  v-model="review.likeText"
                  :placeholder="$t('feedback_on_cooperation')"
                ></b-form-input>
              </b-form-group>
            </b-col>
            <b-col cols="6">
              <b-form-group :label="$t('suggestion_for_improvement')">
                <b-form-input
                  v-model="review.suggestionText"
                  :placeholder="$t('feedback_on_cooperation')"
                ></b-form-input>
              </b-form-group>
            </b-col>
          </div>
          <div class="mt-3">
            <!-- <b-button variant="primary mx-3">
                {{ $t("return_to work_area") }}
              </b-button> -->
            <b-button @click="postReview" variant="primary">{{
              $t("confirm")
            }}</b-button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- </div> -->
</template>

<script>
import ReviewService from "@/services/ReviewService.js";
import UserService from "@/services/UserService.js";
import ContractService from "@/services/ContractService.js";
import StarRating from "vue-star-rating";
import FileService from "@/services/FileService.js";

export default {
  props: ["id"],
  components: {
    StarRating,
  },
  data() {
    return {
      marksExecutor: [
        { description: "payment", textId: "payment_text", value: 0 },
        {
          description: "formulation_of_problem",
          textId: "clarity_of_requirements_text",
          value: 0,
        },
        {
          description: "contact_rate",
          textId: "contact_rate_text_executor",
          value: 0,
        },
        { description: "politeness", textId: "politeness_text", value: 0 },
        { description: "punctuality", textId: "punctuality_text", value: 0 },
      ],
      marksCustomer: [
        {
          description: "professionalism",
          textId: "professionalism_text",
          value: 0,
        },
        {
          description: "preservation_property",
          textId: "preservation_property_text",
          value: 0,
        },
        { description: "contact_rate", textId: "contact_rate_text", value: 0 },

        {
          description: "politeness",
          textId: "politeness_text_executor",
          value: 0,
        },
        {
          description: "punctuality",
          textId: "punctuality_text_executor",
          value: 0,
        },
      ],
      review: {
        images: {},
      },
      executorCard: {},
      progress: {},
      author: {},
      uploadedFiles: [],
      uploadedDocs: [],
      files: [],
      contractorInfo: null,
    };
  },
  methods: {
    uploadPhotos() {
      if (this.files.length > 0) {
        let formDataDocs = new FormData();
        let formDataImgs = new FormData();

        this.files.forEach((f) => {
          if (
            f.name.includes(".jpg") ||
            f.name.includes(".jpeg") ||
            f.name.includes(".png") ||
            f.name.includes(".JPG") ||
            f.name.includes(".PNG")
          ) {
            formDataImgs.append("files", f);
          } else {
            formDataDocs.append("files", f);
          }
        });

        this.$store.commit("IS_LOADING_TRUE");
        FileService.uploadPhotos(formDataImgs)
          .then((response) => {
            this.uploadedFiles.push(
              ...response.data.map((fs) => {
                return fs.fileName;
              })
            );
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));

        FileService.uploadFiles(formDataDocs)
          .then((response) => {
            this.uploadedDocs.push(
              ...response.data.map((fs) => {
                return fs.fileName;
              })
            );
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    removeOneTempImage(image) {
      const index = this.uploadedFiles.indexOf(image);
      if (index > -1) {
        this.uploadedFiles.splice(index, 1);
      }
    },
    removeOneTempDoc(doc) {
      const index = this.uploadedDocs.indexOf(doc);
      if (index > -1) {
        this.uploadedDocs.splice(index, 1);
      }
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
          if (
            this.contractorInfo.userId !== this.$store.state.user.id &&
            this.progress.executorId !== this.$store.state.user.id
          ) {
            this.$toasted.error(this.$t("access_denied"));
            this.$router.push({ name: "allContracts" });
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    postReview() {
      this.$store.commit("IS_LOADING_TRUE");

      let payload = {
        likeText: this.review.likeText,
        suggestionText: this.review.suggestionText,
        documents: this.uploadedDocs.map((d) => {
          return { fileName: d };
        }),
        images: this.uploadedFiles.map((d) => {
          return { fileName: d };
        }),
      };
      if (this.progress.executorId !== this.$store.state.user.id) {
        payload.marks = this.marksCustomer;
      } else {
        payload.marks = this.marksExecutor;
      }
      ReviewService.postReview(this.id, payload)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    getCustomerReview() {
      ReviewService.getCustomerReview(this.id)
        .then((response) => {
          this.review = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    getExecutorReview() {
      ReviewService.getExecutorReview(this.id)
        .then((response) => {
          this.review = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
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
  mounted() {
    this.getProgress();
    this.getContractor();
    // if (this.$store.state.currentRole === "Executor") {
    //   this.getExecutorReview();
    // } else {
    //   this.getCustomerReview();
    // }
  },
  watch: {
    files() {
      if (this.files.length > 0) {
        this.uploadPhotos();
      }
    },
  },
};
</script>

<style scoped>
.remove-photo {
  position: absolute;
  top: -3%;
  right: 3%;
  font-size: 1rem;
}
.dropzone img {
  width: 100%;
}
.bg-text {
  background: #f5f5f5;
  border: 0.1px solid rgba(128, 128, 128, 0.5);
  padding: 20px;
}
.bg-soft-primary {
  background-color: rgba(83, 105, 248, 0.25);
}
.dropzone {
  padding: 15px;
  min-height: 350px;
  border: 2px dashed #5a8dee;
  background: #f2f4f4;
}
</style>
