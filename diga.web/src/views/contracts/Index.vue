<template>
  <div>
    <p class="h5 my-4">{{ $t("all_my_contracts") }}</p>

    <b-table
      id="contracts"
      :per-page="take"
      :current-page="currentPage"
      striped
      hover
      :items="getContracts"
      :fields="contractsFields"
    >
      <!-- A custom formatted column -->
      <template #cell(publishDate)="data">
        {{ data.item.publishDate | moment("DD MMMM YYYY") }}
      </template>
      <template #cell(plannedBuildEnd)="data">
        {{ data.item.plannedBuildEnd | moment("DD MMMM YYYY") }}
      </template>
     <template #cell(message_count)="data">
        <router-link :to="{name: 'chatroom', params:{id: data.item.chatRoomId}}" 
        v-if="data.item.chatRoomId>0" class="p-grey"
          ><i class="bx bx-chat mr-50 position-relative"
            ><span v-if="data.item.unreadMessagesCount>0" class="badge badge-pill badge-danger badge-message">{{
              data.item.unreadMessagesCount
            }}</span></i
          ></router-link
        >
      </template>
      <template #cell(type)="data">
        {{ $t(data.item.type) }}
      </template>
      <template #cell(status)="data">
        {{ $t(data.item.status) }}
      </template>
      <template #cell(versionStatus)="data">
        {{ $t(data.item.versionStatus) }}
      </template>
      <template #cell(versions)="data">
        <router-link
          v-if="data.item.originalId"
          :to="{ name: 'versions', params: { id: data.item.originalId } }"
        >
          {{ $t("see_all") }}
        </router-link>
        <template v-else> </template>
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
    </b-table>
    <b-pagination
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
  props: ["status"],
  data() {
    return {
      currentPage: 1,
      take: 10,
      filter: "",
      rows: 0,
    };
  },
  methods: {
    getContracts(ctx, callback) {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContracts(
        (ctx.currentPage - 1) * this.take,
        this.take,
        this.filter,
        this.status ?? ""
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
          key: "versions",
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

        // {
        //   key: "category",
        //   label: this.$t("category"),
        //   sortable: false,
        //   class: "table-header",
        // },
        // {
        //   key: "status",
        //   label: this.$t("status"),
        //   sortable: false,
        //   class: "table-header",
        // },
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
