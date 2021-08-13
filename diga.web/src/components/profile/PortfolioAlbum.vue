<template>
  <div>
    <p v-if="isEdit === false" class="h5 font-weight-bold">{{ album.name }}</p>
    <div class="float-right align-top">
      <i
        v-if="isEdit === false"
        @click="isEdit = true"
        class="btn cursor-pointer bx bx-edit-alt px-1"
      ></i>
      <i
        v-if="isEdit === false"
        @click="deleteAlbum"
        class="btn cursor-pointer bx bxs-trash px-1"
      ></i>
    </div>

    <input
      @click="saveChanges()"
      class="btn cursor-pointer bx bx-edit-alt align-top float-right"
      v-if="isEdit === true"
      type="button"
      value="✓"
    />
    <b-form-group
      v-if="isEdit === true"
      class="col-6"
      :label="$t('album_title')"
    >
      <b-form-input
        v-model="album.name"
        type="text"
        :placeholder="$t('album_title')"
      ></b-form-input>
    </b-form-group>
    <div v-if="isEdit === false">
      <div
        class="d-inline-block border rounded p-1 m-2"
        v-for="category in album.categories"
        :key="category.id"
      >
        <span class="fs-14 text-nowrap"> {{ $t(category.nameId) }}</span>
      </div>
    </div>
    <div v-if="isEdit === true" class="form-group">
      <label>{{ $t("category") }} </label>
      <multiselect
        v-model="album.categories"
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
    <p v-if="isEdit === false" class="w-75 fs-14">{{ album.description }}</p>
    <b-form-group v-if="isEdit === true" :label="$t('description_album')">
      <b-form-textarea
        id="portfolio-description"
        v-model="album.description"
        :placeholder="$t('enter_something') + '...'"
        rows="3"
        :state="album.description.length < 400 && album.description.length != 0"
      ></b-form-textarea>
      <p class="fs-13" v-if="album.description">
        {{ album.description.length }} / {{ 400 }} ({{ $t("maxlength") }}400)
      </p>
    </b-form-group>

    <p class="fs-13 p-grey">
      {{ $t("published_on") }}: {{ album.publishDate | moment("MMMM Do YYYY") }}
    </p>
    <label v-if="isEdit === true" for="photo-upload">{{
      $t("photo_max_16")
    }}</label>
    <b-form-file
      class="my-2"
      v-if="isEdit === true"
      id="edit-photo-upload"
      multiple
      v-model="files"
      :placeholder="$t('choose_file')"
      :browse-text="$t('browse')"
      :drop-placeholder="$t('drop_file_here') + '...'"
      accept=".jpg, .jpeg, .png"
    ></b-form-file>
    <div class="row">
      <div class="col-3" v-for="photo in album.photoes" :key="photo">
        <span class="d-flex">
          <img class="position-relative" :src="'/img/src/' + photo" alt="" />
          <span
            v-if="isEdit === true"
            class="remove-photo btn text-red"
            @click="removeOneUploadedImage(photo)"
            >x</span
          ></span
        >
      </div>
      <template v-if="uploadedFiles.length > 0">
        <div class="col-3" v-for="photo in uploadedFiles" :key="photo">
          <span class="d-flex">
            <img class="position-relative" :src="'/img/temp/' + photo" alt="" />
            <span
              v-if="isEdit === true"
              class="remove-photo btn text-red"
              @click="removeOneTempImage(photo)"
              >x</span
            ></span
          >
        </div></template
      >
    </div>
  </div>
</template>

<script>
import PortfolioService from "@/services/PortfolioService.js";
import FileService from "@/services/FileService.js";

import { mapState } from "vuex";

export default {
  data() {
    return {
      uploadedFiles: [],
      files: [],
      isEdit: false,
    };
  },
  props: ["album", "userCategories"],
  methods: {
    uploadPhotos() {
      if (this.files.length > 0) {
        let formData = new FormData();

        this.files.forEach((f) => {
          formData.append("files", f);
        });

        this.$store.commit("IS_LOADING_TRUE");
        FileService.uploadPhotos(formData)
          .then((response) => {
            this.uploadedFiles = [];
            this.uploadedFiles.push(
              ...response.data.map((fs) => {
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
    removeOneTempImage(photo) {
      const index = this.uploadedFiles.indexOf(photo);
      if (index > -1) {
        this.uploadedFiles.splice(index, 1);
      }
    },
    removeOneUploadedImage(photo) {
      const index = this.album.photoes.indexOf(photo);
      if (index > -1) {
        this.album.photoes.splice(index, 1);
      }
    },
    saveChanges() {
      this.$store.commit("IS_LOADING_TRUE");
      let photos = this.album.photoes;
      if (this.uploadedFiles.length > 0) {
        photos.push(...this.uploadedFiles);
      }
      PortfolioService.editAlbum(this.album.id, {
        albumId: this.album.id,
        name: this.album.name,
        description: this.album.description,
        categoryIds: this.album.categories.map((l) => {
          return l.id;
        }),
        photos: photos,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.uploadedFiles = [];
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => {
          this.$store.commit("IS_LOADING_FALSE");
          this.isEdit = false;
          this.$emit("getAlbums");
        });
    },
    deleteAlbum() {
      if (confirm(this.$t("delete_album_confirm"))) {
        this.$store.commit("IS_LOADING_TRUE");
        PortfolioService.deleteAlbum(this.album.id, {
          albumId: this.album.id,
        })
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => {
            this.$store.commit("IS_LOADING_FALSE");

            this.$emit("getAlbums");
          });
      }
    },
  },
  computed: {
    ...mapState(["categories"]),
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
    files() {
      if (this.files.length > 0) {
        this.uploadPhotos();
      }
    },
    album: {
      immediate: true,
      handler() {
        if (this.album) {
          this.album.categories.forEach((c) => {
            c.nameId = this.$t(c.nameId);
          });
        }
      },
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
img {
  width: 100%;
}
</style>
