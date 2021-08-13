<template>
  <div class="card p-4">
    <p class="h5 my-4">{{ $t("notifications") }}</p>
    <div
      v-for="n in notifications"
      :key="n.id"
      class="d-flex border-bottom my-2"
    >
      <div class="mr-2">
        <i
          v-b-tooltip.hover
          :title="$t('mark_as_read')"
          v-if="n.isRead === false"
          @click="readNotification(n)"
          class="bx bxs-circle fs-13 primary"
        ></i>
        <i v-else class="bx bxs-check-circle fs-13 success"></i>
      </div>
      <div class="mx-3">{{ $t(n.type) }}</div>
      <div v-for="(data, index) in n.datas" :key="index">
        <router-link
          class="primary mx-2"
          v-if="data.key === 'ContractId'"
          :to="{ name: 'EstimateApproval', params: { id: data.value } }"
        >
          {{ $t(data.key) }}
        </router-link>
        <span v-else> {{ $t(data.key) }}: {{ data.value }} </span>
      </div>
      <div class="ml-auto">
        {{ n.dateTime | moment("from", "now") }}
        <!-- {{ n.dateTime | moment("MMMM Do YYYY") }} -->
      </div>
    </div>
    <b-pagination
      v-if="countAll > 10"
      class="justify-content-center"
      v-model="currentPage"
      :total-rows="countAll"
      :per-page="take"
      aria-controls="my-table"
    ></b-pagination>
  </div>
</template>

<script>
import NotificationService from "@/services/NotificationService.js";

export default {
  data() {
    return {
      notifications: [],
      take: 10,
      countAll: 0,
      currentPage: 1,
    };
  },
  methods: {
    getNotifications() {
      NotificationService.getNotifications(this.skip, this.take)
        .then((response) => {
          this.notifications = response.data.list;
          this.countAll = response.data.countAll;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    readNotification(notification) {
      NotificationService.readNotification(notification.id)
        .then(() => {
          notification.isRead = true;
          this.$store.state.user.unreadNotificationsCount -= 1;
        })
        .catch((err) => console.error(err));
      notification.isRead = true;
    },
  },
  mounted() {
    this.getNotifications();
  },
  computed: {
    PublishedAgo(dateTime) {
      if (dateTime) {
        var a = this.$moment(dateTime);
        return a.fromNow();
      } else {
        return 0;
      }
    },
    skip() {
      return this.take * (this.currentPage - 1);
    },
  },
};
</script>

<style scoped></style>
