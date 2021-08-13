<template>
  <div class="m-3">
    <div class="orange-header h1 px-4 py-3">
      {{ $t("my_contracts") }}
    </div>
    <template>
      <b-breadcrumb class="my-0 t3">
        <b-breadcrumb-item
          class=" blue"
          :to="{ name: 'home_index' }"
        >
          <i class="bx bx-home"></i>
        </b-breadcrumb-item>
        <b-breadcrumb-item
          class=""
          :to="{ name: 'contracts' }"
          >{{ $t("contracts") }}</b-breadcrumb-item
        >
      </b-breadcrumb>
    </template>

    <div class="my-4">
      <div class="active-header d-flex align-items-center my-2">
        <img class="m-2" src="/Assets/platform/icons/active.svg" alt="active" />
        <p class="h2">{{ $t("active_contracts") }}</p>
      </div>

      <div class="p-table bg-white p-4">
        <b-table
          hover
          fixed
          id="activecontracts"
          head-variant="light"
          :items="published"
          :fields="contractsFields"
        >
          <!-- A custom formatted column -->
          <template #cell(publishDate)="data">
            {{ data.item.publishDate | moment("DD MMMM YYYY") }}
          </template>
          <template #cell(plannedBuildEnd)="data">
            <div v-if="data.item.plannedBuildEnd" class="date-tb p-2">
              {{ data.item.plannedBuildEnd | moment("DD MMMM YYYY") }}
            </div>
          </template>
          <template #cell(message_count)="data">
            <div class="text-center">
              <router-link
                :to="{ name: 'chatroom', params: { id: data.item.chatRoomId } }"
                v-if="data.item.chatRoomId > 0"
                class="p-grey"
                ><i class="bx bx-chat position-relative"
                  ><span
                    v-if="data.item.unreadMessagesCount > 0"
                    class="badge badge-pill badge-danger badge-message"
                    >{{ data.item.unreadMessagesCount }}</span
                  ></i
                ></router-link
              >
            </div>
          </template>
          <template #cell(type)="data">
            {{ $t(data.item.type) }}
          </template>
          <template #cell(status)="data">
            <div class="status-tb p-2">{{ $t(data.item.status) }}</div>
          </template>
          <template #cell(name)="data">
            <router-link
              :to="{ name: data.item.status, params: { id: data.item.id } }"
            >
              <template v-if="data.item.name !== null">{{
                data.item.name
              }}</template>
              <template v-else>{{ $t("untitled") }}</template>
            </router-link>
          </template>
          <template #cell(versions)="data">
            <router-link
              v-if="data.item.originalId"
              :to="{ name: 'versions', params: { id: data.item.originalId } }"
            >
              {{ $t("see_all") }}
            </router-link>
          </template>
        </b-table>
        <div class="d-flex justify-content-end">
          <router-link
            v-if="this.$store.state.currentRole === 'Customer'"
            class="btn b-orange h4 mx-2"
            :to="{ name: 'contracts_create' }"
            >{{ $t("add_contract") }}
          </router-link>
          <router-link
            class="btn b-blue h4 mx-2"
            :to="{
              name: 'contracts_with_status',
              params: {
                status:
                  'Contractor&statuses=EstimateApproval&statuses=ContractApproval&statuses=Signing&statuses=Executing&statuses=Deffered',
              },
            }"
            >{{ $t("see_all") }}</router-link
          >
        </div>
      </div>
    </div>

    <div class="my-4">
      <div class="draft-header d-flex align-items-center my-2">
        <img class="m-2" src="/Assets/platform/icons/draft.svg" alt="draft" />
        <p class="h2">{{ $t("Draft") }}</p>
      </div>
      <div class="p-table bg-white p-4">
        <b-table fixed hover :items="drafts" :fields="draftFields">
          <!-- A custom formatted column -->
          <template #cell(versions)="data">
            <router-link
              v-if="data.item.originalId"
              :to="{ name: 'versions', params: { id: data.item.originalId } }"
            >
              {{ $t("see_all") }}
            </router-link>
            <template v-else> </template>
          </template>
          <template #cell(publishDate)="data">
            {{ data.item.publishDate | moment("DD MMMM YYYY") }}
          </template>
          <template #cell(plannedBuildEnd)="data">
            <div v-if="data.item.plannedBuildEnd" class="date-tb p-2">
              {{ data.item.plannedBuildEnd | moment("DD MMMM YYYY") }}
            </div>
          </template>
          <template #cell(type)="data">
            {{ $t(data.item.type) }}
          </template>
          <template #cell(status)="data">
            <div class="status-tb p-2">
              {{ $t(data.item.status) }}
            </div>
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

        <div class="d-flex justify-content-end">
          <router-link
            class="btn b-blue h4 mx-2"
            :to="{ name: 'contracts_with_status', params: { status: 'Draft' } }"
            >{{ $t("see_all") }}</router-link
          >
        </div>
      </div>
    </div>
    <div class="my-4">
      <div class="archive-header d-flex align-items-center my-2">
        <img
          class="m-2"
          src="/Assets/platform/icons/archive.svg"
          alt="archive"
        />
        <p class="h2">{{ $t("archive") }}</p>
      </div>
      <div class="p-table bg-white p-4">
        <b-table fixed hover :items="archive" :fields="contractsFields">
          <template #cell(versions)="data">
            <router-link
              v-if="data.item.originalId"
              :to="{ name: 'versions', params: { id: data.item.originalId } }"
            >
              {{ $t("see_all") }}
            </router-link>
            <template v-else> </template>
          </template>
          <template #cell(publishDate)="data">
            {{ data.item.publishDate | moment("DD MMMM YYYY") }}
          </template>
          <template #cell(plannedBuildEnd)="data">
            <div v-if="data.item.plannedBuildEnd" class="date-tb p-2">
              {{ data.item.plannedBuildEnd | moment("DD MMMM YYYY") }}
            </div>
          </template>
          <!-- A custom formatted column -->
          <template #cell(message_count)="data">
            <router-link
              :to="{ name: 'chatroom', params: { id: data.item.chatRoomId } }"
              v-if="data.item.chatRoomId > 0"
              class="p-grey"
              ><i class="bx bx-chat mr-50 position-relative"
                ><span
                  v-if="data.item.unreadMessagesCount > 0"
                  class="badge badge-pill badge-danger badge-message"
                  >{{ data.item.unreadMessagesCount }}</span
                ></i
              ></router-link
            >
          </template>
          <template #cell(type)="data">
            {{ $t(data.item.type) }}
          </template>
          <template #cell(status)="data">
            <div class="status-tb p-2">
              {{ $t(data.item.status) }}
            </div>
          </template>
          <template #cell(name)="data">
            <router-link :to="{ name: 'Draft', params: { id: data.item.id } }">
              <template v-if="data.item.name !== null">{{
                data.item.name
              }}</template>
              <template v-else>{{ $t("untitled") }}</template></router-link
            >
          </template>
        </b-table>

        <div class="d-flex justify-content-end">
          <router-link
            :to="{
              name: 'contracts_with_status',
              params: {
                status:
                  'Archived&statuses=Finished&statuses=Refusal&statuses=Closed',
              },
            }"
            class="btn b-blue h4 mx-2"
            >{{ $t("see_all") }}</router-link
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ContractService from "@/services/ContractService.js";

export default {
  data() {
    return {
      take: 4,
      drafts: [],
      published: [],
      archive: [],
    };
  },
  methods: {
    getContractDraft() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContracts(0, this.take, "", "Draft", true)
        .then((response) => {
          this.drafts = response.data.list;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      return null;
    },
    getContractPublished() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContracts(
        0,
        this.take,
        "",
        "Contractor&statuses=EstimateApproval&statuses=ContractApproval&statuses=Signing&statuses=Executing&statuses=Deffered",
        true
      )

        .then((response) => {
          this.published = response.data.list;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      return null;
    },
    getContractArchive() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContracts(
        0,
        this.take,
        "",
        "Archived&statuses=Finished&statuses=Refusal&statuses=Closed",
        true
      )
        .then((response) => {
          this.archive = response.data.list;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      return null;
    },
  },
  mounted() {
    this.getContractPublished();
    this.getContractArchive();
    this.getContractDraft();
  },
  computed: {
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
          key: "message_count",
          label: this.$t("messages"),
          sortable: false,
          class: "table-header",
        },
      ];
    },
    draftFields() {
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
      ];
    },
  },
};
</script>
<style>
.orange-header {
  background-color: #f16b24;
  color: #ffffff;
}
.active-header {
  background: #ffffff;
  box-shadow: inset 3px 0px 0px #f16b24;
  color: #f16b24;
}
.draft-header {
  background: #ffffff;
  box-shadow: inset 3px 0px 0px #438d90;
  color: #438d90;
}
.archive-header {
  background: #ffffff;
  box-shadow: inset 3px 0px 0px #606161;
  color: #606161;
}
.p-table {
  border: 1px solid #e5e5e5;
  box-sizing: border-box;
  box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25);
}
#activecontracts tbody tr:hover {
  font-weight: 600;
  box-shadow: inset 3px 0px 0px #f16b24;
  border-left: 3px solid #f16b24;
  color: #2f2c33 !important;
}
#activecontracts tbody tr:hover i {
  color: #f16b24;
}
.status-tb {
  background-color: #fdd6c1;
  border-radius: 2px;
}
.date-tb {
  background-color: #ccebec;
  border-radius: 2px;
}
#activecontracts tbody tr:hover .status-tb,
#activecontracts tbody tr:hover .date-tb {
  background-color: #ffdf8b;
}
</style>
