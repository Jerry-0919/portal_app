<template>
  <div class="container">
    <p class="h5" v-if="isCreating === false">
      {{ $t("edit") }}: {{ $t("category") }} {{ $t(category.nameId) }}
    </p>
    <p class="h5" v-else>
      {{ $t("add_category") }}
    </p>
    <b-form @submit.prevent="editAdminCategory">
      <b-form-group :label="$t('name')">
        <b-form-input v-model="category.nameId">
          {{ category.nameId }}
        </b-form-input>
      </b-form-group>
      <b-form-group :label="$t('parent_category')">
        <b-form-select v-model="category.parentCategoryId">
          <option value="null">{{ $t("parent_category") }}</option>
          <option
            v-for="parentCategory in parentCategories"
            :key="parentCategory.id"
            :value="parentCategory.id"
          >
            {{ $t(parentCategory.nameId) }}
          </option>
        </b-form-select>
      </b-form-group>
            <b-form-group :label="$t('reserve_percentage')">
        <b-form-input v-model="category.reservationPercent">
          {{ category.reservationPercent }}
        </b-form-input>
      </b-form-group>
      <b-button type="submit">{{ $t("save") }}</b-button>
    </b-form>
  </div>
</template>

<script>
import AdminCategoriesService from "@/services/AdminCategoriesService.js";
import DictionaryService from "@/services/DictionaryService.js";

export default {
  props: ["id"],
  data() {
    return {
      isCreating: false,
      category: { nameId: "", parentCategoryId: null, reservationPercent: 0 },
      parentCategories: [],
    };
  },

  methods: {
    postAdminCategories() {
      AdminCategoriesService.postAdminCategories(this.category)
        .then(() => {
          this.$router.push({ name: "adminCategories" });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getCategories() {
      DictionaryService.getCategories()
        .then((response) => {
          this.parentCategories = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getAdminCategory() {
      AdminCategoriesService.getAdminCategory(this.id)
        .then((response) => {
          this.category = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    editAdminCategory() {
      if (this.isCreating === false) {
        AdminCategoriesService.editAdminCategory(this.id, this.category)
          .then(() => {
            this.$router.push({ name: "adminCategories" });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      } else {
        this.postAdminCategories();
      }
    },
  },
  mounted() {
    if (this.id > 0) {
      this.isCreating = false;
    } else {
      this.isCreating = true;
    }

    if (this.isCreating === false) {
      this.getAdminCategory();
    }

    this.getCategories();
  },
};
</script>

<style scoped></style>
