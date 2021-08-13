<template>
  <div class="container-fluid my-3" v-if="contract">
    <div class="orange-header px-4 py-3 d-flex">
      <span class="h1"> {{ $t("add_contract") }}: {{ contract.name }}</span>
      <div v-if="contract.contractTypeId" class="ml-auto d-flex contract-type">
        <span class="ctype fs-14">
          {{ $t(getContractTypeById(contract.contractTypeId).name) }}
        </span>
        <div class="trapezoid-left"></div>
      </div>
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
          class=" blue"
          :to="{ name: 'contracts' }"
          >{{ $t("contracts") }}</b-breadcrumb-item
        >
        <b-breadcrumb-item
          :class="{ 'blue': tabIndex === 0 }"
          v-if="is_creating"
          >{{ $t("add_contract") }}</b-breadcrumb-item
        >
        <b-breadcrumb-item
          :class="{ 'blue': tabIndex === 0 }"
          v-if="!is_creating"
          >{{ $t("edit_contract") }}</b-breadcrumb-item
        >
        <b-breadcrumb-item disabled>{{ $t("publication") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("choosing_contractor") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("estimate_approval") }}</b-breadcrumb-item>

        <b-breadcrumb-item>{{
          $t("approval_of_conditions")
        }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("Signing") }}</b-breadcrumb-item>
        <b-breadcrumb-item>{{ $t("execution") }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>

    <draft
      v-if="tabIndex === 0"
      :contract="contract"
      @switchParentTab="switchTab"
    />
  </div>
</template>

<script>
import draft from "@/components/contracts/Draft.vue";
import ContractService from "@/services/ContractService.js";
import { mapState, mapGetters } from "vuex";

export default {
  props: ["id"],
  data() {
    return {
      is_creating: true,
      tabIndex: 0,
      contract: {
        id: 0,
      },
    };
  },
  components: {
    draft,
  },
  mounted() {
    if (this.id > 0) {
      this.is_creating = false;
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.getContract(this.id)
        .then((response) => {
          this.contract = response.data;
          if (!this.contract.publishDate) {
            this.contract.publishDate = this.$moment().format("YYYY-MM-DD"); //new Date().toISOString().slice(0, 10)
          }
          if (!this.contract.tenderEndDate) {
            this.contract.tenderEndDate = this.$moment()
              .add(7, "d")
              .format("YYYY-MM-DD");
          }
          if (this.contract.categories.length > 0) {
            this.contract.categoryIds = [];
            this.contract.categories.forEach((c) => {
              this.categories.forEach((ca) => {
                ca.platformCategories.forEach((ch) => {
                  if (c === ch.id) {
                    this.contract.categoryIds.push({
                      nameId: ch.nameId,
                      id: ch.id,
                    });
                  }
                });
              });
            });
          }
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    } else {
      this.contract.status = "Draft";
      this.contract.contractPriorityId = 1;
    }
  },
  methods: {
    switchTab() {
      if (
        this.$moment(this.contract.publishDate).isSame(this.$moment(), "day")
      ) {
        this.$router.push({
          name: "Contractor",
          params: { id: this.contract.id },
        });
      } else {
        this.$toasted.success(this.$t("schedule_post_done"));

        this.$router.push({ name: "home_index" });
      }
    },
  },
  computed: {
    ...mapState(["categories"]),
    ...mapGetters([
      "getCityById",
      "getCountryById",
      "getContractPriorityById",
      "getContractTypeById",
      "getBudgetById",
    ]),
  },
  watch: {
    contract() {
      if (this.tabIndex === 0) {
        if (this.contract.status === "Contractor") {
          // this.tabIndex = 1;
          this.$router.push({
            name: "Contractor",
            params: { id: this.contract.id },
          });
        }
      }
    },
  },
};
</script>
<style>
.contract-type {
  filter: drop-shadow(0px 4px 4px rgba(0, 0, 0, 0.25));
}
.breadcrumb {
  background-color: transparent !important;
}
/* .breadcrumb .breadcrumb-item::before {
  content: ">" !important;
  font-size: 10px;
  line-height: 2;
} */
.breadcrumb .breadcrumb-item:nth-child(1):before {
  content: "" !important;
}

.ctype {
  color: #f16b24;
  background-color: white;
  padding: 0.34rem 1.11rem;
}
.trapezoid-left {
  border: 17px solid white; /* All borders set */
  border-left: 0; /* Remove left border */
  border-right: 17px solid transparent; /* Right transparent */
  /*   width:        100px;                Increase element Width */
}
</style>
