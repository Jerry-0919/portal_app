<template>
    <form  @submit.prevent="submit">
      <div class="w-75 m-auto">
        <p class="h3 my-3">{{ $t("add_description") }}</p>
        <vue-editor
          @input="
            errors.description = false;
            errors.minlength = false;
          "
          :class="{ 'input-invalid': errors.description || errors.minlength }"
          v-model="contract.description"
          id="placeholder"
        >
          <template #placeholder>
            <p>{{ $t("add_description") }}</p>
          </template>
        </vue-editor>
        <p class="fs-13" v-if="contract.description">
          {{ contract.description.length }} / {{ 400 }} ({{
            $t("minlength")
          }}400)
        </p>
        <p class="error text-red" v-if="errors.minlength">
          {{ $t("minlength") }}400
        </p>

        <p class="h5 mt-5 mb-3">{{ $t("upload_estimate") }}</p>
        <p class="my-3 font-weight-normal">
          {{ $t("file_excel_preference") }}
        </p>
        <b-form-file
          v-if="!contract.estimateFileName"
          v-model="file"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          accept=".xlsx, .xls, .pdf"
        ></b-form-file>
        <div class="mt-3" v-else>
          <p>
            {{ contract.estimateName
            }}<span class="btn text-red" @click="clearFile">X</span>
          </p>
        </div>
        <div class="mt-3">
          <p v-if="file">
            {{ file.name
            }}<span class="btn text-red" @click="clearFile">X</span>
          </p>
        </div>
        <div class="my-5" v-if="!file">
          <p class="font-weight-normal">{{ $t("dont_have_estimate") }}</p>
          <b-button
            @click="
              contract.isEstimateOrdered = true;
              $toasted.success(
                $t('your_request_was_successfully_sent') +
                  ' ' +
                  $t('we_wiil_contact_you_shortly')
              );
            "
           class="btn b-orange"
            >{{ $t("order_estimate") }}</b-button
          >
        </div>
        <div v-if="showWarning">
          <div class="text-danger">{{ $t("pdf_warning") }}</div>
          <div class="font-weight-bold">{{ $t("order_pdf") }}</div>
          <b-button
            @click="
              contract.isEstimateOrdered = true;
              showWarning = !showWarning;
              $toasted.success($t('your_request_was_successfully_sent'));
            "
            variant="outline-success mx-2"
            >{{ $t("yes") }}</b-button
          >
          <b-button
            variant="outline-danger mx-2"
            @click="
              showWarning = !showWarning;
              contract.isEstimateOrdered = false;
            "
            >{{ $t("no") }}</b-button
          >
        </div>
        <p class="my-3 font-weight-normal">{{ $t("other_files") }}</p>
        <b-form-file
          multiple
          v-model="files"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          :drop-placeholder="$t('drop_file_here') + '...'"
        ></b-form-file>
        <div class="mt-3">
          <p class="m-0" v-for="mfile in contract.files" :key="mfile">
            {{ mfile }}
            <span class="btn text-red" @click="removeOneFile(mfile)">X</span>
          </p>
        </div>
     </div>
      <div class="bg-white d-flex justify-content-between my-3">
        <b-button class="btn b-grey-outline" @click="$emit('TabPrev')">{{
          $t("previous")
        }}</b-button>
        <b-button @click="$emit('Cancel')" class="btn b-orange-outline">{{
          $t("cancel_post")
        }}</b-button>

        <b-button
          v-if="file === null && files.length === 0"
         class="btn b-blue"
          type="submit"
          >{{ $t("next") }}</b-button
        >

        <b-button v-else variant="primary mx-2" @click="uploadFiles">{{
          $t("upload")
        }}</b-button>
      </div>
    </form>
</template>

<script>
import { VueEditor } from "vue2-editor";
import FileService from "@/services/FileService.js";
import ContractService from "@/services/ContractService.js";

export default {
  props: ["contract"],
  components: {
    VueEditor,
  },
  data() {
    return {
      content: "",
      file: null,
      files: [],
      showWarning: false,
      errors: {},
    };
  },
  methods: {
    clearFile() {
      this.file = null;
      this.contract.estimateFileName = null;
    },
    removeOneFile(mfile) {
      const index = this.contract.files.indexOf(mfile);
      if (index > -1) {
        this.contract.files.splice(index, 1);
      }
    },
    validateForm() {
      if (this.contract.description) {
        if (this.contract.description.length >= 400) {
          return true;
        }
      }
      this.errors = {};
      if (!this.contract.description) {
        this.errors.description = true;
      } else if (this.contract.description.length < 400) {
        this.errors.minlength = true;
      }
      return false;
    },
    uploadEstimate() {
      let formData = new FormData();
      formData.append("file", this.file);
      this.$store.commit("IS_LOADING_TRUE");
      FileService.uploadFile(formData)
        .then((response) => {
          this.contract.estimateFileName = response.data.fileName;
          this.contract.estimateName = this.file.name;
          this.file = null;
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    uploadFiles() {
      if (this.file) {
        this.uploadEstimate();
      }
      if (this.files.length > 0) {
        let formData = new FormData();

        this.files.forEach((f) => {
          formData.append("files", f);
        });

        this.$store.commit("IS_LOADING_TRUE");
        FileService.uploadFiles(formData)
          .then((response) => {
            let fileNames = response.data;
            if (!this.contract.files) {
              this.contract.files = [];
            }
            this.contract.files.push(
              ...fileNames.map((fs) => {
                return fs.fileName;
              })
            );
            this.files = [];
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
    submit() {
      if (this.validateForm() === false) {
        this.$toasted.error(this.$t("fill_in_the_fields"));
      } else {
        this.$store.commit("IS_LOADING_TRUE");

        ContractService.saveEstimate(this.contract.id, {
          contractId: this.contract.id,
          description: this.contract.description,
          fileName: this.contract.estimateFileName,
          name: this.contract.estimateName,
          files: this.contract.files ?? [],
          isOrdered: this.contract.isEstimateOrdered ?? false,
        })
          .then(() => {
            this.$toasted.success(
              this.$t("your_request_was_successfully_sent")
            );
            this.$emit("TabNext");
          })
          .catch(() => {
            this.$toasted.error(this.$t("oops_error"));
          })
          .finally(() => this.$store.commit("IS_LOADING_FALSE"));
      }
    },
  },
  watch: {
    file() {
      if (this.file && this.file.name.includes(".pdf")) {
        this.showWarning = true;
      }
    },
  },
};
</script>
<style></style>
