<template>
  <b-modal id="verificationModal" :title="$t('verification')">
    <div>{{ $t("to_verify_profile") }}</div>
    <ul>
      <li>{{ $t("personal_docs") }}</li>
      <li v-if="roles.some((r) => r.role === 'PlatformExecutorMaster')">
        RV recibo verde
      </li>
      <li v-if="roles.some((r) => r.role === 'PlatformExecutorTeam')">
        RV recibo verde/Codigo certidao permanente
      </li>
      <li v-if="roles.some((r) => r.role === 'PlatformExecutorCompany')">
        Certidao permanente, alvara
      </li>
    </ul>
    <b-form-file
      multiple
      required
      v-model="files"
      :state="Boolean(files)"
      placeholder="Choose a file or drop it here..."
      drop-placeholder="Drop file here..."
      accept=".jpg, .png, .pdf"
    ></b-form-file>
    <template #modal-footer>
      <b-button @click="postVerificationDocs" variant="primary">{{
        $t("verify")
      }}</b-button>
    </template>
  </b-modal>
</template>

<script>
import UserService from "@/services/UserService.js";
import FileService from "@/services/FileService.js";

export default {
  data() {
    return {
      roles: [],
      files: [],
      uploadedDocs: [],
    };
  },
  methods: {
    uploadFiles() {
      let formDataDocs = new FormData();

      this.files.forEach((f) => {
        formDataDocs.append("files", f);
      });

      return FileService.uploadPhotos(formDataDocs);
    },
    postVerificationDocs() {
      this.uploadFiles()
        .then((response) => {
          this.uploadedDocs.push(
            ...response.data.map((fs) => {
              return fs.fileName;
            })
          );
          UserService.postVerificationDocs({ photos: this.uploadedDocs })
            .then(() => {
              this.$toasted.success(
                this.$t("your_request_was_successfully_sent")
              );
              this.$store.state.user.verificationStatus = 'UnderConsideration'
            })
            .catch(() => {
              this.$toasted.error(this.$t("oops_error"));
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    getRoles() {
      UserService.getRoles()
        .then((response) => {
          this.roles = response.data;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  mounted() {
    this.getRoles();
  },
};
</script>

<style scoped></style>
