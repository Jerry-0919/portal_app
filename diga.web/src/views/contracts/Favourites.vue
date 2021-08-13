<template>
  <div>
    <p class="h5 my-4">{{ $t("bookmarks") }}</p>

    <b-table
      id="favourite"
      :per-page="take"
      :current-page="currentPage"
      striped
      hover
      :items="getFavourites"
      :fields="contractsFields"
    >
      <!-- A custom formatted column -->
      <template #cell(publishDate)="data">
        {{ data.item.publishDate | moment("MMMM Do YYYY") }}
      </template>
      <template #cell(plannedBuildEnd)="data">
        {{ data.item.plannedBuildEnd | moment("MMMM Do YYYY") }}
      </template>
      <template #cell(type)="data">
        {{ $t(data.item.type) }}
      </template>
      <template #cell(status)="data">
        {{ $t(data.item.status) }}
      </template>
      <template #cell(delete)="data">
        <b-button @click="deleteFavourites(data.item.id)" variant="danger"
          >{{ $t("delete") }}
        </b-button>
      </template>
      <template #cell(name)="data">
        <router-link
          :to="{ name: data.item.status, params: { id: data.item.id } }"
        >
          {{ data.item.name }}
        </router-link>
      </template>
    </b-table>
    <b-pagination
      v-if="rows > 10"
      class="justify-content-center"
      v-model="currentPage"
      :total-rows="rows"
      :per-page="take"
      aria-controls="contracts"
    ></b-pagination>
  </div>
</template>

<script>
import FavouriteService from "@/services/FavouriteService.js";

export default {
  data() {
    return {
      currentPage: 1,
      take: 10,
      filter: "",
      rows: 0,
    };
  },
  methods: {
    getFavourites(ctx, callback) {
      this.$store.commit("IS_LOADING_TRUE");
      FavouriteService.getFavourites(
        (ctx.currentPage - 1) * this.take,
        this.take
      )
        .then((response) => {
          this.rows = response.data.countAll;
          callback(response.data.list);
        })
        .catch((error) => {
          console.log(error);
          callback([]);
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      return null;
    },
    deleteFavourites(id) {
      this.$store.commit("IS_LOADING_TRUE");
      FavouriteService.deleteFavourites(id)
        .then(() => {
          this.$root.$emit("bv::refresh::table", "favourite");
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  components: {},
  mounted() {},
  watch: {},
  computed: {
    skip() {
      return (this.currentPage - 1) * this.take;
    },
    contractsFields() {
      return [
        {
          key: "id",
          sortable: false,
          class: "table-header",
        },
        {
          key: "name",
          label: this.$t("contract_name"),
          sortable: false,
          class: "table-header",
        },
        {
          key: "type",
          label: this.$t("contract_type"),
          sortable: false,
          class: "table-header",
        },
        {
          key: "status",
          label: this.$t("status"),
          sortable: false,
          class: "table-header",
        },
        {
          key: "publishDate",
          label: this.$t("publication_of_a_contract"),
          sortable: false,
          class: "table-header",
        },
        {
          key: "plannedBuildEnd",
          label: this.$t("completion_of_construction"),
          sortable: false,
          class: "table-header",
        },

        {
          key: "delete",
          label: this.$t("delete"),
          sortable: false,
          class: "table-header",
        },
      ];
    },
  },
};
</script>

<style scoped></style>
