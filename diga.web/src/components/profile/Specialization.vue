<template>
  <div class="card mb-0 p-4">
    <b-form>
      <b-form-group :label="$t('user_skills')">
        <div class="row">
          <div class="col-4">
            <select
              v-model="userParentCategoryId"
              name="parentCategory"
              class="form-control"
            >
              <option
                v-for="parentCategory in categories"
                :value="parentCategory.id"
                :key="parentCategory.id"
                >{{ $t(parentCategory.nameId) }}</option
              >
            </select>
          </div>
          <div class="col-8">
            <!-- <p>{{ userParentCategoryId}}</p> -->
            <b-form-checkbox-group
              @input="updateCategories"
              v-model="localeCategories"
              :options="childCategories"
              class="categories-list"
              value-field="id"
              text-field="nameId"
              disabled-field="notEnabled"
            ></b-form-checkbox-group>
          </div>
        </div>
      </b-form-group>
      <div
        v-if="userCategories && userCategories.length > 0"
        class="col-12 my-4"
      >
        <p class="h5">{{ $t("you_choosed") }}:</p>
        <div
          class="d-inline-block border rounded p-1 m-2"
          v-for="uc in userCategories"
          :key="uc"
        >
          <span class="fs-14 text-nowrap">{{ categoryNameById(uc) }}</span>
        </div>
      </div>
      <b-form-group :label="$t('tags')">
        <multiselect
          :max="20"
          v-model="userTags"
          :tag-placeholder="$t('add_as_new_tag')"
          :placeholder="$t('search_or_add_tag')"
          @select="selectTag"
          @remove="removeTag"
          :options="tags"
          :multiple="true"
          :taggable="true"
          @tag="addTag"
        ></multiselect>
      </b-form-group>
    </b-form>
  </div>
</template>

<script>
import { mapState } from "vuex";
import Multiselect from "vue-multiselect";
import UserService from "@/services/UserService.js";

export default {
  props: ["userTags", "userCategories"],
  data() {
    return {
      userParentCategoryId: null,
      localeCategories: [],
    };
  },

  components: { Multiselect },

  methods: {
    updateCategories(newCategories) {
      let difference = newCategories.filter(
        (n) => !this.userCategories.includes(n)
      );

      difference.forEach((d) => {
        this.selectCategory(d);
      });

      let toDelete = this.userCategories.filter(
        (n) => !newCategories.includes(n)
      );
      toDelete.forEach((d) => {
        this.removeCategory(d);
      });
    },
    categoryNameById(id) {
      var val = "";
      this.categories.forEach((c) => {
        var category = c.platformCategories.find((pc) => pc.id === id);
        if (category) {
          val = category.nameId;
        }
      });
      return val;
    },
    selectCategory(category) {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.postCategory({ categoryId: category })
        .then((response) => {
          if (response.data.valid !== true) {
            this.$toasted.error(this.$t("oops_error"));
          }
          this.$emit("fetchCategories");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    removeCategory(category) {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.removeCategory({ categoryId: category })
        .then(() => {
          this.$emit("fetchCategories");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    selectTag(tag) {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.postTag({ tag: tag })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    removeTag(tag) {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.removeTag({ tag: tag })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          //this.userTags.push(tag);
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    addTag(newTag) {
      this.$store.commit("IS_LOADING_TRUE");
      UserService.postTag({ tag: newTag })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.userTags.push(newTag);
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },

  computed: {
    ...mapState(["categories", "tags"]),
    childCategories() {
      if (this.userParentCategoryId > 0) {
        return this.categories.find((c) => c.id === this.userParentCategoryId)
          .platformCategories;
      } else {
        return [];
      }
    },
  },
  watch: {
    categories() {
      if (this.categories && this.categories.length > 0) {
        this.userParentCategoryId = this.categories[0].id;
      }
    },
    userCategories: {
      immediate: true,
      handler() {
        this.localeCategories = this.userCategories;
      },
      //   let toAdd = newCategories.filter((n) => !oldCategories.includes(n));
      //   toAdd.forEach((d) => {
      //     this.selectCategory(d);
      //   });

      //   let toRemove = oldCategories.filter((n) => !newCategories.includes(n));
      //   toRemove.forEach((d) => {
      //     this.removeCategory(d);
      //   });
    },
  },
};
</script>

<style>
.categories-list .custom-control-inline {
  display: block !important;
  margin-right: 0 !important;
}
</style>
