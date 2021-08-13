<template>
  <div v-if="contract">
    <p class="h5 my-4">{{ $t("contract") }}: {{ contract.name }}</p>

    <b-table
      id="contracts"
      :per-page="take"
      :current-page="currentPage"
      striped
      hover
      :items="getVersions"
      :fields="contractsFields"
    >
      <!-- A custom formatted column -->
      <template #cell(type)="data">
        {{ $t(data.item.type) }}
      </template>
      <template #cell(status)="data">
        {{ $t(data.item.status) }}
      </template>
      <template #cell(publishDate)="data">
        {{ data.item.publishDate | moment("MMMM Do YYYY") }}
      </template>
      <template #cell(plannedBuildEnd)="data">
        {{ data.item.plannedBuildEnd | moment("MMMM Do YYYY") }}
      </template>
      <template #cell(message_count)="data">
        <a class="p-grey" href="#"
          ><i class="bx bx-chat mr-50 position-relative"
            ><span class="badge badge-pill badge-danger badge-message">{{
              data.item.message_count
            }}</span></i
          ></a
        >
      </template>
            <template #cell(category)="data">
        {{ $t(data.item.type) }}
      </template>
      <template #cell(name)="data">
        <router-link
          :to="{ name: data.item.status, params: { id: data.item.id } }"
        >
          <template v-if="data.item.name !== null">{{
            data.item.name
          }}</template>
          <template v-else>{{ $t("untitled") }}</template></router-link
        >
      </template>
      <template #cell(versionStatus)="data">
        {{ $t(data.item.versionStatus) }}
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
import ContractService from "@/services/ContractService.js";

export default {
  props: ["id"],
  data() {
    return {
      currentPage: 1,
      take: 10,
      contract: {},
      rows: 0,
    };
  },
  methods: {
    getVersions(ctx, callback) {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getVersions(
        this.id,
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
    },
    getContract() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.id)
        .then((response) => {
          this.contract = response.data;
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      return null;
    },
  },
  components: {},
  mounted() {
    this.getContract();
    this.getVersions();
  },
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
          key: "version",
          label: this.$t("versions"),
          sortable: false,
          class: "table-header",
        },
        {
          key: "versionStatus",
          label: this.$t("versions_status"),
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
          key: "category",
          label: this.$t("category"),
          sortable: false,
          class: "table-header",
        },
        {
          key: "status",
          label: this.$t("status"),
          sortable: false,
          class: "table-header",
        },
        // {
        //   key: "project_start",
        //   label: this.$t("project_start"),
        //   sortable: false,
        //   class: "table-header",
        // },
        // {
        //   key: "finish_project",
        //   label: this.$t("finish_project"),
        //   sortable: false,
        //   class: "table-header",
        // },
        {
          key: "message_count",
          label: this.$t("messages"),
          sortable: false,
          class: "table-header",
        },
      ];
    },
  },
};
</script>

<style></style>
