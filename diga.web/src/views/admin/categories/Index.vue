<template>
  <div>
    <router-link class="btn btn-success" :to="{ name: 'addAdminCategories' }">
      {{ $t("add_category") }}
    </router-link>
    <b-table-simple no-border-collapse responsive sticky-header>
      <b-thead head-variant="light">
        <b-tr>
          <b-th>{{ $t("parent_category") }}</b-th>
          <b-th>{{ $t("category") }}</b-th>
          <b-th>{{ $t("actions") }}</b-th>
        </b-tr>
      </b-thead>
      <b-tbody>
        <b-tr v-for="pcategory in pcategories" :key="pcategory.id">
          <b-th>
            {{ $t(pcategory.nameId) }}
          </b-th>
          <b-td>
            <ul>
              <li
                class="d-flex align-items-center justify-content-between my-1 border-bottom"
                v-for="category in pcategory.children"
                :key="category.id"
              >
                <template class="mr-auto"> {{ $t(category.nameId) }} </template>

                <router-link
                  class="btn btn-primary ml-auto"
                  :to="{
                    name: 'editAdminCategories',
                    params: { id: category.id },
                  }"
                >
                  {{ $t("edit") }}
                </router-link>
                <b-button
                  @click="deleteAdminCategory(category.id)"
                  variant="danger"
                >
                  {{ $t("delete") }}
                </b-button>
              </li>
            </ul>
          </b-td>
          <b-td>
            <router-link
              target="_blank"
              class="btn btn-primary"
              :to="{
                name: 'editAdminCategories',
                params: { id: pcategory.id },
              }"
            >
              {{ $t("edit") }}
            </router-link>
            <b-button
              @click="deleteAdminCategory(pcategory.id)"
              variant="danger"
            >
              {{ $t("delete") }}
            </b-button>
          </b-td>
        </b-tr>
      </b-tbody>
    </b-table-simple>
  </div>
</template>

<script>
import AdminCategoriesService from "@/services/AdminCategoriesService.js";

export default {
  data() {
    return {
      pcategories: [],
      skip: 0,
      take: 10,
    };
  },
  methods: {
    getAdminCategories() {
      this.$store.commit("IS_LOADING_TRUE");
      AdminCategoriesService.getAdminCategories(this.skip, this.take)
        .then((response) => {
          this.pcategories = response.data.list;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    deleteAdminCategory(id) {
      this.$store.commit("IS_LOADING_TRUE");
      AdminCategoriesService.deleteAdminCategory(id)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.getAdminCategories();
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getAdminCategories();
  },
};
</script>

<style scoped>
.b-table-sticky-header {
  max-height: 500px !important;
  margin-bottom: 0 !important;
}
</style>
