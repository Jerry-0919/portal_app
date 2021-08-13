<template>
  <div class="m-3">
    <div class="orange-header h1 px-4 py-3">
      {{ $t("all_contracts") }}
    </div>
    <template>
      <b-breadcrumb class="my-0 t3">
        <b-breadcrumb-item class=" blue" :to="{ name: 'home_index' }">
          <i class="bx bx-home"></i>
        </b-breadcrumb-item>
        <b-breadcrumb-item class="" :to="{ name: 'allContracts' }">{{
          $t("all_contracts")
        }}</b-breadcrumb-item>
      </b-breadcrumb>
    </template>
    <b-row>
      <b-col cols="3">
        <div class="card p-4">
          <b-input-group>
            <b-form-input
              :placeholder="$t('search')"
              v-model="filter.filter"
              type="text"
              class="form-control"
            />
            <b-input-group-append>
              <i
                class="font-weight-bold btn b-blue-outline bx bx-search"
                type="button"
              ></i>
            </b-input-group-append>
          </b-input-group>

          <b-form-group class="mt-3">
            <label class="h4 font-weight-bold" for="searchCategories">{{
              $t("categories")
            }}</label>
            <multiselect
              v-model="filter.categories"
              :options="categories"
              :multiple="true"
              group-values="platformCategories"
              group-label="nameId"
              :placeholder="$t('type_to_search')"
              track-by="id"
              label="nameId"
              id="searchCategories"
              ><span slot="noResult">{{ $t("no_elements_found") }}</span>
            </multiselect>
          </b-form-group>
          <b-form-group>
            <label class="h4 font-weight-bold" for="searchTags">{{
              $t("tags")
            }}</label>
            <multiselect
              v-model="filter.tags"
              id="searchTags"
              :placeholder="$t('type_to_search')"
              :options="tags"
              :multiple="true"
              ><span slot="noResult">{{
                $t("no_elements_found")
              }}</span></multiselect
            >
          </b-form-group>

          <b-form-group stacked class="t3" size="lg">
            <b-form-checkbox class="my-2" v-model="filter.yourSpecialization"
              >{{ $t("your_specialization") }}
              <!-- <i class="bx bxs-id-card align-middle mx-1"></i> -->
            </b-form-checkbox>
            <b-form-checkbox class="my-2" v-model="filter.hideMyBids"
              >{{ $t("hide_my_bids") }}
              <!-- <i class="bx bx-hide align-middle"></i > -->
            </b-form-checkbox>
            <b-form-checkbox class="my-2" v-model="filter.safeDeal"
              >{{ $t("safe_deal") }}
              <!-- <i class="bx bxs-shield-alt-2 align-middle mx-1"></i> -->
            </b-form-checkbox>
            <b-form-checkbox class="my-2" v-model="filter.onlyUrgent"
              >{{ $t("urgent_only") }}
              <!-- <i class="bx bxs-bell-ring align-middle mx-1"></i> -->
            </b-form-checkbox>
            <b-form-group class="mt-3">
              <label class="h4 font-weight-bold" for="budgetSearch"
                >{{ $t("budget_untill") }}:</label
              >
              <b-form-select
                class="form-control"
                id="budgetSearch"
                v-model="filter.balanceId"
                name="budget"
              >
                <option value="0">{{ $t("all") }}</option>
                <option
                  v-for="budget in budgets"
                  :value="budget.id"
                  :key="budget.id"
                  >{{ $t(budget.value) }}</option
                >
              </b-form-select>
            </b-form-group>
            <b-form-checkbox class="t3" @change="saveFilter"
              >{{ $t("save_filter") }}
              <!-- <i class="bx bxs-hdd align-middle mx-1"></i> -->
            </b-form-checkbox>
          </b-form-group>

          <b-button @click="clearFilter" class="btn b-blue" type="button">
            {{ $t("reset") }}
          </b-button>
        </div>
      </b-col>
      <b-col id="contractSearch" col="9">
        <div v-for="contract in contracts" :key="contract.id" class="card p-3">
          <div class="card-header border-0">
            <router-link
              :to="{ name: 'Contractor', params: { id: contract.id } }"
              class="h2 pb-3"
              >{{ contract.name }}</router-link
            >
            <div class=" d-flex align-items-center">
              <div class="mr-3">
                <i
                  v-if="!contract.userAvatar"
                  style="font-size: 60px"
                  class=" bx bx-user-circle nav-i"
                ></i>
                <div
                  v-else
                  class="avatar "
                  v-bind:style="{
                    backgroundImage:
                      'url(' + '/img/src/' + contract.userAvatar + ')',
                  }"
                ></div>
              </div>
              <div>
                <div>
                  <router-link :to="{ name: 'home_index' }" class="blue h4 ml-1"
                    >{{ contract.fullName }}
                  </router-link>
                  <div>
                    <span
                     v-b-tooltip.hover
                      :title="$t('Rating')"
                     class="orange"
                      ><i class="bx bxs-star align-middle"></i
                      >{{ contract.rating }}</span
                    >
                    <a
                      v-b-tooltip.hover
                      :title="$t('positive_reviews')"
                      class="mx-1"
                      href="#"
                      ><span class="blue"
                        ><i class="bx bxs-like align-middle"></i>
                        {{ contract.goodReviews }}</span
                      >
                    </a>
                    <a
                      v-b-tooltip.hover
                      :title="$t('negative_reviews')"
                      href="#"
                      ><span class="grey-dark"
                        ><i class="bx bxs-dislike align-middle"></i>
                        {{ contract.badReviews }}</span
                      ></a
                    >
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="card-body">
            <div
              class="mb-3 d-flex justify-content-between align-items-center font-weight-bold"
            >
              <span>
                {{ $t("tender_start") }}
                <span class="tenderstart p-2 mx-2">
                  <i class="bx bx-calendar"></i>
                  {{ contract.publishDate | moment("DD.MM.YYYY") }}
                </span>
              </span>
              <span>
                {{ $t("end_of_competition") }}
                <span class="tenderend p-2 mx-2">
                  <i class="bx bx-calendar"></i>
                  {{ contract.tenderEndDate | moment("DD.MM.YYYY") }}
                </span>
              </span>
              <span>
                {{ $t("contract_budget") }}
                <span v-if="contract.price" class="budget p-2 mx-2">
                  <i class="bx bx-euro"></i>
                  {{ contract.price }}
                </span>
                <span v-else class="budget p-2 mx-2">
                  <i class="bx bx-euro"></i>
                  {{ $t("before") }}
                  {{ getBudgetById(contract.balanceId).value }}
                </span>
              </span>
            </div>
            <div class="h4 m-0" v-html="contract.description"></div>
          </div>
          <div class="card-footer d-flex justify-content-between border-0 py-0">
            <div class="d-inline-flex align-items-center mr-2 t4">
              <span>
                <i class="bx bx-check-circle align-middle orange mr-2"></i>
                <span class="blue h4">{{ contract.countBids }}</span>
                {{ $t("bids") }}
              </span>
              <span class="mx-3">
                <i class="bx bx-comment-detail align-middle orange mr-2"></i>
                <span class="blue h4"> {{ contract.countDiscussions }} </span>
                {{ $t("comments") }}
              </span>
            </div>

            <div>
              <a
                v-if="contract.estimateFileName"
                type="button"
                class="btn b-blue-outline mx-2"
                target="_blank"
                :href="'/docs/src/' + contract.estimateFileName"
                >{{ $t("estimate") }}
                <i class="bx bx-download align-middle"></i>
              </a>
              <router-link
                class="btn b-blue"
                :to="{ name: 'Contractor', params: { id: contract.id } }"
                target="_blank"
                >{{ $t("details") }}</router-link
              >
            </div>
          </div>
        </div>

        <div v-if="contractsCount === 0" class="text-center">
          {{ $t("no_elements_found") }}
        </div>
      </b-col>
    </b-row>

    <b-pagination
      align="right"
      v-if="contractsCount > 4"
      v-model="currentPage"
      :total-rows="contractsCount"
      :per-page="contractTake"
      aria-controls="contracts"
    ></b-pagination>
  </div>
</template>

<script>
import ContractService from "@/services/ContractService.js";
import { mapState, mapGetters } from "vuex";
import Multiselect from "vue-multiselect";
import UserService from "@/services/UserService.js";

export default {
  components: { Multiselect },
  data() {
    return {
      contractsCount: null,
      contractTake: 4,
      currentPage: 1,
      contracts: [],
      userCategories: [],
      filter: {
        yourSpecialization: false,
        hideMyBids: false,
        safeDeal: false,
        onlyUrgent: false,
        saveFilter: false,
        filter: "",
        categories: [],
        tags: [],
        balanceId: 0,
        sort: 1,
      },
    };
  },
  methods: {
    getFilter() {
      ContractService.getSearchFilter()
        .then((response) => {
          this.filter.hideMyBids = response.data.hideMyBids;
          //  this.filter.categories = response.data.categories
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getCategories() {
      UserService.getCategories()
        .then((response) => {
          this.userCategories = response.data;
          this.userCategories.forEach((u) => {
            u.nameId = this.$t(u.name);
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    saveFilter(value) {
      if (value === true) {
        var obj = { ...this.filter };
        let categories = obj.categories.map((c) => {
          return c.id;
        });
        delete obj.categories;

        ContractService.saveSearchFilter({
          hideMyBids: this.filter.hideMyBids,
          categories: categories,
          tags: this.filter.tags,
          safeDeal: this.safeDeal,
          onlyUrgent: this.onlyUrgent,
          yourSpecialization: this.filter.yourSpecialization,
          balanceId: this.balanceId,
        })
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    clearFilter() {
      this.filter = {
        yourSpecialization: false,
        hideMyBids: false,
        safeDeal: false,
        onlyUrgent: false,

        filter: "",
        categories: [],
        tags: [],
        balanceId: 0,
        sort: 0,
      };
    },
    getPublishedContracts() {
      this.$store.commit("IS_LOADING_TRUE");
      var obj = { ...this.filter };
      let categories = obj.categories
        .map((c) => {
          return c.id;
        })
        .join("&categories=");

      let tags = obj.tags.join("&tags=");
      delete obj.categories;
      delete obj.tags;
      if (this.filter.balanceId === 0) {
        delete obj.balanceId;
      }
      ContractService.getPublishedContracts(
        this.contractSkip,
        this.contractTake,
        obj,
        categories,
        tags
      )
        .then((response) => {
          this.contracts = response.data.list;
          this.contractsCount = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },

  mounted() {
    this.getPublishedContracts();
    this.getFilter();
    this.getCategories();
  },
  watch: {
    "filter.yourSpecialization"() {
      if (this.filter.yourSpecialization === true) {
        this.filter.categories = this.userCategories;
        this.getPublishedContracts();
      }
    },
    currentPage() {
      this.getPublishedContracts();
    },
    filter: {
      handler() {
        this.getPublishedContracts();
      },
      deep: true,
    },
  },
  computed: {
    ...mapState(["budgets", "categories", "tags"]),
    ...mapGetters(["getBudgetById"]),

    contractSkip() {
      return (this.currentPage - 1) * this.contractTake;
    },
  },
};
</script>

<style scoped>
i .align-middle {
  font-size: 16px;
}
.card-header,
.card-footer {
  background-color: #fff !important;
}
.col-form-label {
  font-weight: bold;
}
#contractSearch .card {
  border-top: 3px solid #f16b24;
}
.card-footer i {
  font-size: 24px;
}
</style>
