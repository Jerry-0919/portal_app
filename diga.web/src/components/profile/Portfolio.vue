<template>
  <div class="card mb-0 p-4">
    <p class="h4">{{ $t("add_portfolio") }}</p>
    <b-form @submit.prevent="onSubmit">
      <div class="row">
        <div class="col-6">
          <b-form-group :label="$t('album_title')">
            <b-form-input
              v-model="newAlbum.name"
              type="text"
              :placeholder="$t('album_title')"
            ></b-form-input>
          </b-form-group>
          <div class="form-group">
            <label>{{ $t("category") }} </label>
            <multiselect
              v-model="newAlbum.categoryIds"
              :options="filteredCategories"
              :multiple="true"
              group-values="platformCategories"
              group-label="nameId"
              :placeholder="$t('type_to_search')"
              track-by="id"
              label="nameId"
              ><span slot="noResult">{{ $t("no_elements_found") }}</span>
              >
            </multiselect>
          </div>

          <b-form-group :label="$t('description_album')">
            <b-form-textarea
              id="portfolio-description"
              v-model="newAlbum.description"
              :placeholder="$t('enter_something') + '...'"
              rows="3"
              :state="
                newAlbum.description.length < 400 &&
                  newAlbum.description.length != 0
              "
            ></b-form-textarea>
            <p class="fs-13" v-if="newAlbum.description">
              {{ newAlbum.description.length }} / {{ 400 }} ({{
                $t("maxlength")
              }}400)
            </p>
          </b-form-group>

          <label for="photo-upload">{{ $t("photo_max_16") }}</label>
          <b-form-file
            id="album-photo-upload"
            multiple
            v-model="files"
            :placeholder="$t('choose_file')"
            :browse-text="$t('browse')"
            :drop-placeholder="$t('drop_file_here') + '...'"
            accept=".jpg, .jpeg, .png"
          ></b-form-file>
          <div class="mt-3">
            <!-- if not uploaded -->
            <p class="m-0" v-for="preview in filePreviews" :key="preview.url">
              <img :src="preview.url" alt="" style="max-width: 100px;" />
              <span class="btn text-red" @click="removeOneImage(preview.file)"
                >X</span
              >
            </p>
            <!-- if already uploaded but album not saved -->
            <p
              class="m-0"
              v-for="(temp, index) in newAlbum.photos"
              :key="index"
            >
              <img
                :src="'/img/temp/' + temp"
                alt=""
                style="max-width: 100px;"
              />
              <span class="btn text-red" @click="removeOneUploadedImage(temp)"
                >x</span
              >
            </p>
          </div>
        </div>
      </div>
      <a
        v-if="files && files.length > 0"
        @click="uploadPhotos"
        class="btn success"
      >
        {{ $t("upload_photo") }}
      </a>
      <b-button class="my-3" variant="primary" v-else type="submit">
        {{ $t("create_album") }}
      </b-button>
    </b-form>
    <hr />
    <p class="h4">{{ $t("add_video_portfolio") }}</p>

    <div
      class="col-6 input-group mt-1 mb-2 pl-0"
      v-for="videoUrl in videoUrls"
      :key="videoUrl.id"
    >
      <input
        :class="{ 'input-invalid': $v.videoUrls.$error }"
        @blur="$v.videoUrls.$touch()"
        type="url"
        v-model="videoUrl.value"
        class="form-control"
        style="width: 400px"
      />
      <div class="input-group-append">
        <input
          type="button"
          value="✓"
          class="form-control"
          @click="saveUrl(videoUrl.id)"
        />
        <input
          type="button"
          value="x"
          class="form-control"
          @click="removeUrl(videoUrl.id)"
        />
      </div>
    </div>

    <div class="col-2 input-group mt-1 mb-2 pl-0">
      <input
        type="button"
        class="btn btn-success btn-circle"
        @click="addUrl"
        value="+"
      />
    </div>

    <hr />
    <div>
      <p class="h4 mb-5">
        {{ $t("already_in_portfolio") }}
        <span class="counter counter-red fs-13 mx-2">{{ albumsCount }}</span>
      </p>
    </div>
    <div v-for="album in albums" :key="album.id">
      <portfolioAlbum
        :userCategories="userCategories"
        :album="album"
        @getAlbums="getAlbums"
      />
      <hr />
    </div>

    <b-pagination
      v-if="albumsCount > 2"
      class="justify-content-center"
      v-model="currentPage"
      :total-rows="albumsCount"
      :per-page="albumTake"
      aria-controls="my-table"
    ></b-pagination>
  </div>
</template>

<script>
import { mapState } from "vuex";
import FileService from "@/services/FileService.js";
import PortfolioService from "@/services/PortfolioService.js";
import Multiselect from "vue-multiselect";
import { url } from "vuelidate/lib/validators";
import portfolioAlbum from "./PortfolioAlbum.vue";
export default {
  props: ["userCategories"],
  components: { Multiselect, portfolioAlbum },
  validations: {
    videoUrls: {
      $each: {
        value: {
          url,
        },
      },
    },
  },
  data() {
    return {
      currentPage: 1,
      albums: [],
      albumsCount: 0,

      albumTake: 2,
      newAlbum: { description: "" },
      files: [],
      videoUrls: [],
    };
  },
  methods: {
    getAlbums() {
      this.$store.commit("IS_LOADING_TRUE");
      PortfolioService.getAlbums(this.albumSkip, this.albumTake)
        .then((response) => {
          this.albums = response.data.list;
          this.albumsCount = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    removeUrl(videoId) {
      if (videoId > 0) {
        PortfolioService.removeVideoUrl(videoId)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.getUrls();
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      } else {
        const index = this.videoUrls.indexOf(videoId.value);
        if (index > -1) {
          this.videoUrls.splice(index, 1);
        }
      }
    },
    getUrls() {
      PortfolioService.getVideos()
        .then((response) => {
          this.videoUrls = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    addUrl() {
      this.videoUrls.push({
        id: 0,
        value: "",
      });
    },
    saveUrl(videoUrl) {
      this.$v.$touch();
      if (!this.$v.$invalid) {
        if (videoUrl.id > 0) {
          videoUrl.videoId = videoUrl.id;
          PortfolioService.editVideoUrl(videoUrl.id, {
            value: this.videoUrl.value,
          })
            .then(() => {
              this.$toasted.success(
                this.$t("your_request_was_successfully_sent")
              );
              this.getUrls();
            })
            .catch(() => {
              this.$toasted.error(this.$t("oops_error"));
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));
        } else {
          PortfolioService.addVideoUrl({
            value: this.videoUrl.value,
          })
            .then(() => {
              this.$toasted.success(
                this.$t("your_request_was_successfully_sent")
              );
              this.getUrls();
            })
            .catch(() => {
              this.$toasted.error(this.$t("oops_error"));
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));
        }
      }
    },
    onSubmit(evt) {
      evt.preventDefault();
      this.$store.commit("IS_LOADING_TRUE");
      PortfolioService.postAlbum({
        name: this.newAlbum.name,
        description: this.newAlbum.description,
        categoryIds: this.newAlbum.categoryIds.map((l) => {
          return l.id;
        }),
        photos: this.newAlbum.photos,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.newAlbum = { description: "" };
          this.getAlbums();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    uploadPhotos() {
      if (this.files.length > 0) {
        let formData = new FormData();

        this.files.forEach((f) => {
          formData.append("files", f);
        });

        this.$store.commit("IS_LOADING_TRUE");
        FileService.uploadPhotos(formData)
          .then((response) => {
            this.newAlbum.photos = [];
            this.newAlbum.photos.push(
              ...response.data.data.map((fs) => {
                return fs.fileName;
              })
            );
            this.files = [];
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    removeOneImage(file) {
      const index = this.files.indexOf(file);
      if (index > -1) {
        this.files.splice(index, 1);
      }
    },
    removeOneUploadedImage(temp) {
      const index = this.newAlbum.photos.indexOf(temp);
      if (index > -1) {
        this.newAlbum.photos.splice(index, 1);
      }
    },
  },
  computed: {
    albumSkip() {
      return this.albumTake * (this.currentPage - 1);
    },
    ...mapState(["categories"]),
    filePreviews() {
      if (this.files && this.files.length > 0) {
        let result = [];
        this.files.forEach((file) => {
          result.push({ file: file, url: URL.createObjectURL(file) });
        });
        return result;
      } else {
        return [];
      }
    },
    filteredCategories() {
      if (this.categories.length > 0) {
        if (this.userCategories && this.userCategories.length > 0) {
          var result = [];
          this.categories.forEach((category) => {
            var contains = false;
            category.platformCategories.forEach((pc) => {
              if (this.userCategories.includes(pc.id)) {
                contains = true;
              }
            });
            if (contains === true) {
              var newCategory = { ...category };
              newCategory.platformCategories = newCategory.platformCategories.filter(
                (pc) => this.userCategories.includes(pc.id)
              );
              result.push(newCategory);
            }
          });
          return result;
        } else {
          return this.categories;
        }
      } else {
        return [];
      }
    },
  },
  watch: {
    currentPage() {
      this.getAlbums();
    },
  },
  mounted() {
    this.getUrls();
    this.getAlbums();
  },
};
</script>

<style scoped>

.btn-circle {
  /* width: 30px;
  height: 30px; */
  border-radius: 25px;
}
</style>
