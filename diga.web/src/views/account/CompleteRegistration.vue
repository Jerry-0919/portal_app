<template>
  <div class="mx-3">
    <div class="orange-header h1 px-4 py-3">
      {{ $t("select_profile") }}
    </div>
    <template v-if="isExecutor === false">
      <div class="d-flex justify-content-center align-items-stretch">
        <div class="card m-2" v-if="role !== 'PlatformExecutor'">
          <img src="/Assets/platform/img/customer-role.png" alt="" />
          <p class="shadow-title text-center py-2 blue h2">
            {{ $t("customer") }}
          </p>
          <b-row class="m-4 t1">
            <div class="col-1 px-0">
              <i class="bx bx-check-circle blue align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("looking_for_builders") }}
            </div>

            <div class="col-1 px-0">
              <i class="bx bx-check-circle blue align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("publishes_orders") }}
            </div>
            <div class="col-1 px-0">
              <i class="bx bx-check-circle blue align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("conducts_correspondence") }}
            </div>
            <div class="col-1 px-0">
              <i class="bx bx-check-circle blue align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("compares_prices") }}
            </div>
            <div class="col-1 px-0">
              <i class="bx bx-check-circle blue align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("write_reviews_about_builders") }}
            </div>
          </b-row>

          <b-button
            @click="postRole(['PlatformCustomer'])"
            class="b-blue mx-3 my-auto"
            >{{ $t("confirm") }}</b-button
          >
        </div>
        <div class="card m-2" v-if="role !== 'PlatformCustomer'">
          <img src="/Assets/platform/img/executor-role.png" alt="" />
          <p class="shadow-title text-center py-2 orange h2">
            {{ $t("executor") }}
          </p>
          <b-row class="m-4 t1">
            <div class="col-1 px-0">
              <i class="bx bx-check-circle orange align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("publishes_info_about_himself") }}
            </div>

            <div class="col-1 px-0">
              <i class="bx bx-check-circle orange align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("gets_access_objects_profile") }}
            </div>
            <div class="col-1 px-0">
              <i class="bx bx-check-circle orange align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("participating_contest") }}
            </div>
            <div class="col-1 px-0">
              <i class="bx bx-check-circle orange align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("responds_to_customer_requests") }}
            </div>
            <div class="col-1 px-0">
              <i class="bx bx-check-circle orange align-middle "></i>
            </div>
            <div class="col-11">
              {{ $t("creates_portfolio_works") }}
            </div>
          </b-row>

          <b-button @click="isExecutor = true" class="b-orange mx-3 mb-4">{{
            $t("confirm")
          }}</b-button>
        </div>
      </div>
      <div
        class="container d-flex align-items-center justify-content-center mt-4"
      >
        <img
          class="m-4"
          src="/Assets/platform/img/warning.png"
          alt="exclamation"
        />

        <div class="t1 w-75">
          <p>{{ $t("role_text") }}</p>
          <p>{{ $t("role_wallet") }}</p>
        </div>
      </div>
    </template>
    <template v-else>
      <b-container>
        <div class="card my-4 mx-auto">
          <b-row>
            <b-col cols="6">
              <img src="/Assets/platform/img/master.png" alt="" />
            </b-col>
            <b-col cols="6">
              <p class="bg-greylight text-center py-2 m-3 blue h2">
                {{ $t("specialist") }}
              </p>
              <b-row class="m-4 t1">
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("works_individually") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_info_about_himself") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("responds_to_customer_requests") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_price_lists") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("conducts_correspondence") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_orders_for_contractors") }}
                </div>
              </b-row>

              <div class="text-center">
                <b-button
                  @click="postRole(['PlatformExecutorMaster'])"
                  class="b-orange w-75"
                  >{{ $t("confirm") }}</b-button
                >
              </div>
            </b-col>
          </b-row>
        </div>
        <div class="card my-4 mx-auto">
          <b-row>
            <b-col cols="6">
              <img src="/Assets/platform/img/brigade.png" alt="" />
            </b-col>
            <b-col cols="6">
              <p class="bg-greylight text-center py-2 m-3 blue h2">
                {{ $t("brigade") }}
              </p>
              <b-row class="m-4 t1">
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("team_work") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_info_about_himself") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("responds_to_customer_requests") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_price_lists") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_orders_for_contractors") }}
                </div>
              </b-row>

               <div class="text-center">
                <b-button
                  @click="postRole(['PlatformExecutorTeam'])"
                  class="b-orange w-75"
                  >{{ $t("confirm") }}</b-button
                >
              </div>
            </b-col>
          </b-row>
        </div>
        <div class="card my-4 mx-auto">
          <b-row>
            <b-col cols="6">
              <img src="/Assets/platform/img/company.png" alt="" />
            </b-col>
            <b-col cols="6">
              <p class="bg-greylight text-center py-2 m-3 blue h2">
                {{ $t("company") }}
              </p>
              <b-row class="m-4 t1">
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("legal_entity") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("conducts_correspondence") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("responds_to_customer_requests") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_price_lists") }}
                </div>
                <div class="col-1 px-0">
                  <i class="bx bx-check-circle orange align-middle"></i>
                </div>
                <div class="col-11">
                  {{ $t("publishes_orders_for_contractors") }}
                </div>
              </b-row>

               <div class="text-center">
                <b-button
                  @click="postRole(['PlatformExecutorCompany'])"
                  class="b-orange w-75"
                  >{{ $t("confirm") }}</b-button
                >
              </div>
            </b-col>
          </b-row>
        </div>
      </b-container>
    </template>
  </div>
</template>

<script>
import UserService from "@/services/UserService.js";
export default {
  props: ["role"],
  data() {
    return {
      isExecutor: false,
    };
  },
  mounted() {
    if (this.role === "PlatformCustomer") {
      this.postRole(["PlatformCustomer"]);
    }
  },
  methods: {
    postRole(roles) {
      roles.forEach((role) => {
        this.$store.commit("IS_LOADING_TRUE");
        UserService.postRole(role)
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$root.roles.push({ role: role });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      });

      if (roles.includes("PlatformCustomer")) {
        this.$router.push({ name: "contracts_create" });
      } else {
        this.$router.push({ name: "allContracts" });
      }
    },
  },
};
</script>

<style scoped>
.card {
  max-width: 825px;
}
.d-flex .card {
  max-width: 400px;
}

.shadow-title {
  box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25);
}
</style>
