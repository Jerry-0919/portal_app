<template>
  <div>
    <chat-window
      v-if="$store.state.user"
      :current-user-id="$store.state.user.id"
      :room-id="selectedRoomId"
      :rooms="rooms"
      :messages="messages"
      :loading-rooms="loadingRooms"
      :rooms-loaded="roomsLoaded"
      :messages-loaded="messagesLoaded"
      :show-add-room="false"
      :show-audio="false"
      :disableReactions="true"
      :room-actions="[
        {
          name: 'addToFavorites',
          title: $t('add_to_bookmarks'),
        },
        {
          name: 'removeFromFavorites',
          title: $t('remove_from_bookmarks'),
        },
      ]"
      :menu-actions="[
        {
          name: 'viewUserList',
          title: $t('users'),
        },
        {
          name: 'addSupportAgent',
          title: $t('add_support_agent'),
        },
      ]"
      @fetch-messages="fetchMessages"
      @send-message="sendMessage"
      @room-action-handler="handleRoomClick"
      @menu-action-handler="handleMenuClick"
    />

    <b-modal v-model="modalUsersShow" hide-footer>
      <template #modal-header>
        {{ $t("users") }}
      </template>
      <p v-for="user in users" :key="user._id">
        <img
          class="vac-room-avatar"
          v-if="user.avatar"
          :style="{ 'background-image': 'url(' + user.avatar + ')' }"
        />
        <i
          v-else
          style="font-size: 25px"
          class=" bx bx-user-circle nav-i align-middle"
        ></i>
        <span>{{ user.username }}</span>
        <span class="mx-4" v-if="$store.state.user.id !== user._id">     <b-button
          v-b-tooltip.hover
          :title="$t('violation_popup_tooltip')"
          variant="primary my-2"
          @click="complainProfileModal = !complainProfileModal"
          >{{ $t("make_complaint") }}</b-button
        > </span>
                      <b-modal v-model="complainProfileModal">
          <template #modal-header>
            {{ $t("make_complaint") }}: {{user.username}}
          </template>
          <b-form-textarea
            v-model="complaintText"
            id="complaintProfile"
            name="complaintProfile"
            rows="8"
            :placeholder="$t('no_more_than_100_characters')"
          ></b-form-textarea>
          <template #modal-footer>
            <b-button @click="makeComplaint(user)" variant="primary">{{
              $t("make_complaint")
            }}</b-button>
          </template>
        </b-modal>
      </p>

    </b-modal>
  </div>
</template>

<script>
import ChatWindow from "vue-advanced-chat";
import ChatService from "@/services/ChatService.js";
import FileService from "@/services/FileService.js";
import UserService from "@/services/UserService.js";

export default {
  props: ["roomId"],
  components: {
    ChatWindow,
  },
  data() {
    return {
      messagesSkip: 0,
      messagesTake: 10,
      complainProfileModal: false,
      complaintText: "",

      skip: 0,
      take: 100,
      rooms: [],
      messages: [],
      loadingRooms: false,
      roomsLoaded: false,
      messagesLoaded: false,
      currentRoom: null,
      modalUsersShow: false,
        users: [],
        selectedRoomId: null,
    };
  },
  methods: {
        makeComplaint(user) {
      UserService.makeComplaint({
        userId: user._id,
        text: this.complaintText,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.complainProfileModal = false
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    handleMenuClick(model) {
      let room = this.rooms.find((r) => r.roomId === model.roomId);
      if (model.action.name === "viewUserList") {
        this.users = room.users;
        this.modalUsersShow = true;
      }
      if (model.action.name === "addSupportAgent") {
        this.$store.commit("IS_LOADING_TRUE");
        ChatService.addSupportToRoom(model.roomId)
          .then((response) => {
            if (response._id > 0) {
              room.users.push(response.data);
              this.messages.push({
                _id: 0,
                content:
                  this.$t("technical_support_agent_was_added") +
                  " " +
                  response.data.username,
                senderId: "System",
                system: true,
              });
            } else {
              this.messages.push({
                _id: 0,
                content: this.$t("thanks_for_support_request"),
                senderId: "System",
                system: true,
              });
            }
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    handleRoomClick(model) {
      let room = this.rooms.find((r) => r.roomId === model.roomId);
      if (model.action.name === "addToFavorites") {
        this.$store.commit("IS_LOADING_TRUE");
        ChatService.addRoomToFavourites(model.roomId)
          .then(() => {
            room.isFavourite = true;
            this.rooms.sort((a, b) => {
              return a.isFavourite - b.isFavourite;
            });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
      if (model.action.name === "removeFromFavorites") {
        ChatService.removeRoomToFavourites(model.roomId)
          .then(() => {
            room.isFavourite = null;
            this.rooms.sort((a, b) => {
              return a.isFavourite - b.isFavourite;
            });
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    sendMessage(model) {
      if (model.file != null) {
        let formData = new FormData();
        formData.append("file", model.file.blob);
        this.$store.commit("IS_LOADING_TRUE");
        FileService.uploadFile(formData)
          .then((response) => {
            model.file.fileName = response.data.fileName;
            this.pushMessage(model);
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      } else {
        this.pushMessage(model);
      }
    },
    pushMessage(model) {
      let room = this.rooms.find((r) => r.roomId === model.roomId);
      ChatService.sendMessage(model)
        .then((response) => {
          this.messages.push(response.data);
          if (
            room.isSystem === true &&
            room.users.length == 1 &&
            !this.messages.some((m) => m._id === 0)
          ) {
            this.messages.push({
              _id: 0,
              content: this.$t("thanks_for_support_request"),
              senderId: "System",
              date: response.data.date,
              timestamp: response.data.Timestamp,
              system: true,
            });
          }
          room.lastMessage = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        });
    },
    getRooms() {
      this.loadingRooms = true;
      ChatService.getRooms(this.skip, this.take)
        .then((response) => {
          this.rooms.push(...response.data);
          if (response.data.length === 0) {
            this.roomsLoaded = true;
            }
            if (this.roomId > 0) {
                this.selectedRoomId = this.roomId;
            }
            
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => {
          this.loadingRooms = false;
          this.roomsLoaded = true;
        });
    },
    fetchMessages(model) {
      if (this.currentRoom !== model.room.roomId) {
        this.messagesSkip = 0;
      }
      this.messagesLoaded = false;
      ChatService.getMessages(
        model.room.roomId,
        this.messagesSkip,
        this.messagesTake
      )
        .then((response) => {
          response.data = response.data.reverse();
          if (this.currentRoom === model.room.roomId) {
            this.messages.unshift(...response.data);
          } else {
            this.messages = response.data;
          }
          this.messagesSkip += this.messagesTake;

          if (response.data.length === 0) {
            this.messagesLoaded = true;
          }

          this.messages.forEach((m) => {
            if (m.seen === false && m.senderId !== this.$store.state.user.id) {
              ChatService.readMessage(m._id)
                .then(() => {
                  m.seen = true;
                  if (this.$store.state.user.unreadMessagesCount > 0) {
                    this.$store.state.user.unreadMessagesCount -= 1;
                  }
                })
                .catch(() => {
                  this.$toasted.error(this.$t("oops_error"));
                });
            }
          });
          this.rooms.forEach((r) => {
            if (r.roomId === model.room.roomId) {
              r.unreadCount = 0;
            }
          });
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => (this.currentRoom = model.room.roomId));
    },
  },
  sockets: {
    ReceiveMessage(response) {
      this.$store.state.user.unreadMessagesCount += 1;
      if (response.roomId === this.currentRoom) {
        if (!this.messages.some((m) => m._id === response._id)) {
          this.messages.push(response);
        }
      }

      this.rooms.forEach((r) => {
        if (r.roomId === response.roomId) {
          r.unreadCount = (r.unreadCount == null ? 0 : r.unreadCount) + 1;
          r.lastMessage = response;
        }
      });
    },
    ReceiveUserIsOnline(response) {
      if (response.userId !== this.$store.state.user.id) {
        this.rooms.forEach((r) => {
          if (r.users.some((u) => u._id === response.userId)) {
            r.isOnline = true;
            r.users.forEach((u) => {
              if (u._id === response.userId) {
                u.status.state = "online";
                u.state.lastChanged = response.lastChanged;
              }
            });
          }
        });
      }
    },
    ReceiveUserIsOffline(response) {
      if (response.userId !== this.$store.state.user.id) {
        this.rooms.forEach((r) => {
          if (r.users.some((u) => u._id === response.userId)) {
            r.isOnline = false;
            r.users.forEach((u) => {
              if (u._id === response.userId) {
                u.status.state = "offline";
                u.state.lastChanged = response.lastChanged;
              }
            });
          }
        });
      }
    },
    ReceiveRoom(response) {
      if (!this.rooms.some((r) => r.roomId === response.roomId)) {
        this.rooms.unshift(response);
      }
    },
  },
  mounted() {
    this.getRooms();
  },
  created() {},
};
</script>

<style scoped></style>
