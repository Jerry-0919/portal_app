<template>
  <div>
    <div v-if="info.isWalletCreated === false" class="card mb-0 p-4">
      <p>
        {{ $t("name") }}: <b>{{ info.name }}</b>
      </p>

      <p v-if="info.surname">
        {{ $t("surname") }}: <b>{{ info.surname }}</b>
      </p>
      <p v-else class="danger">
        {{ $t("fill_in_the_fields") }}: <b>{{ $t("surname") }}</b>
        {{ $t("in_tab") }}
        <b>{{ $t("personal_data") }}</b>
      </p>
      <p>
        {{ $t("nationality") }}:
        <b>{{ countries.find((c) => c.id === info.nationalityId).name }}</b>
      </p>

      <p>
        Email: <b>{{ info.email }}</b>
      </p>

      <p v-if="info.dateOfBirth">
        {{ $t("dob") }}: <b>{{ info.dateOfBirth | moment("MMMM Do YYYY") }}</b>
      </p>
      <p v-else class="danger">
        {{ $t("fill_in_the_fields") }}: <b>{{ $t("dob") }}</b>
        {{ $t("in_tab") }}
        <b>{{ $t("personal_data") }}</b>
      </p>

      <p v-if="info.residenceCountryId">
        {{ $t("country_of_residence") }}:
        <b>{{
          countries.find((c) => c.id === info.residenceCountryId).name
        }}</b>
      </p>
      <p v-else class="danger">
        {{ $t("fill_in_the_fields") }}: <b>{{ $t("country_of_residence") }}</b>
        {{ $t("in_tab") }}
        <b>{{ $t("personal_data") }}</b>
      </p>
      <b-form-group>
        <b-form-checkbox v-model="isLegal">
          {{ $t("create_wallet_as_legal") }}
        </b-form-checkbox>
      </b-form-group>
      <template v-if="isLegal">
        <p v-if="companyInfo.name">
          {{ $t("company_name") }}: <b>{{ companyInfo.name }}</b>
        </p>
        <p v-else class="danger">
          {{ $t("fill_in_the_fields") }}: <b>{{ $t("company_name") }}</b>
          {{ $t("in_tab") }}
          <b>{{ $t("company_data") }}</b>
        </p>
        <p v-if="companyInfo.organizationType">
          {{ $t("organization_type") }}:
          <b>{{ companyInfo.organizationType }}</b>
        </p>
        <p v-else class="danger">
          {{ $t("fill_in_the_fields") }}: <b>{{ $t("organization_type") }}</b>
          {{ $t("in_tab") }}
          <b>{{ $t("company_data") }}</b>
        </p>
      </template>

      <b-button @click="createWallet" variant="primary">
        {{ $t("create_wallet") }}
      </b-button>
    </div>
    <div
      v-if="
        info.isWalletCreated === true && info.isBankAccountConnected === false
      "
      class="card mb-0 p-4"
    >
      <b-form>
        <p class="h4">{{ $t("attachment_bank_account") }}</p>
        <b-form-group :label="$t('name') + ' ' + $t('and') + $t('surname')">
          <b-form-input
            type="text"
            v-model="bankAccount.ownerName"
            :value="info.name + ' ' + info.username"
          >
          </b-form-input>
        </b-form-group>
        <b-form-group :label="$t('contract_address')">
          <b-form-input
            placeholder="Address Line 1"
            type="text"
            v-model="bankAccount.ownerAddress.addressLine1"
          >
          </b-form-input>
          <b-form-input
            placeholder="Address Line 2"
            type="text"
            v-model="bankAccount.ownerAddress.addressLine2"
          >
          </b-form-input>
          <b-row>
            <b-col>
              <b-form-input
                :placeholder="$t('city')"
                type="text"
                v-model="bankAccount.ownerAddress.city"
              >
              </b-form-input
            ></b-col>
            <b-col>
              <b-form-input
                placeholder="Region"
                type="text"
                v-model="bankAccount.ownerAddress.region"
              >
              </b-form-input
            ></b-col>
            <b-col>
              <b-form-input
                :placeholder="$t('zipcode')"
                type="text"
                v-model="bankAccount.ownerAddress.postalCode"
              >
              </b-form-input
            ></b-col>
          </b-row>

          <select
            class="form-control"
            v-model="bankAccount.ownerAddress.country"
          >
            <option
              v-for="country in countries"
              :value="country.code"
              :key="country.id"
              >{{ country.name }}</option
            >
          </select>
        </b-form-group>
        <b-row>
          <b-col>
            <b-form-group :label="$t('bic_text')">
              <b-form-input type="text" v-model="bankAccount.bic">
              </b-form-input> </b-form-group
          ></b-col>
          <b-col>
            <b-form-group label="IBAN">
              <b-form-input type="text" v-model="bankAccount.iban">
              </b-form-input> </b-form-group
          ></b-col>
        </b-row>
        <b-button @click="addBankAccount" variant="primary">
          {{ $t("send_request") }}
        </b-button>
      </b-form>
    </div>
    <div
      v-if="
        info.isWalletCreated === true &&
          info.isBankAccountConnected === true &&
          !companyInfo.name &&
          info.bankAccountValidationStatus !== 'VALIDATION_ASKED'
      "
      class="card mb-0 p-4"
    >
      <b-form-group :label="$t('the_portfolio_of_documents')">
        <b-form-file
          v-model="file"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          accept=".pdf, .jpeg"
        ></b-form-file>
        <p v-if="file">{{ file.name }}</p>
      </b-form-group>
      <b-button @click="sendIndividual" variant="primary">
        {{ $t("send") }}
      </b-button>
    </div>
    <div
      v-if="
        info.isWalletCreated === true &&
          info.isBankAccountConnected === true &&
          companyInfo.name &&
          info.bankAccountValidationStatus !== 'VALIDATION_ASKED'
      "
      class="card mb-0 p-4"
    >
      <p class="h4 my-4">{{ $t("the_portfolio_of_documents") }}</p>
      <b-form-group :label="$t('general_manager_id_card')">
        <b-form-file
          v-model="idCard"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          :drop-placeholder="$t('drop_file_here') + '...'"
        ></b-form-file>
      </b-form-group>
      <b-form-group :label="$t('articles_association')">
        <b-form-file
          v-model="articleAssociation"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          :drop-placeholder="$t('drop_file_here') + '...'"
        ></b-form-file>
      </b-form-group>
      <b-form-group :label="$t('shareholder_declaration')">
        <b-form-file
          v-model="shareholderDeclaration"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          :drop-placeholder="$t('drop_file_here') + '...'"
        ></b-form-file>
      </b-form-group>
      <b-form-group :label="$t('registration_proof')">
        <b-form-file
          v-model="registrationProof"
          :placeholder="$t('choose_file')"
          :browse-text="$t('browse')"
          :drop-placeholder="$t('drop_file_here') + '...'"
        ></b-form-file>
      </b-form-group>
      <b-button @click="sendCompany" variant="primary">
        {{ $t("send") }}
      </b-button>
    </div>
    <div
      v-if="info.bankAccountValidationStatus === 'VALIDATION_ASKED'"
      class="card mb-0 p-4 text-center"
    >
      <p class="my-4 h3 success">
        {{ $t("your_request_was_successfully_sent") }}
      </p>
      <p class="h6">{{ $t("validation_asked") }}</p>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
import FileService from "@/services/FileService.js";
import MangoPayService from "@/services/MangoPayService.js";

export default {
  props: ["info", "companyInfo"],
  computed: {
    ...mapState(["countries", "cities", "categories", "tags", "languages"]),
  },
  data() {
    return {
      isLegal: false,
      file: null,
      proofOfIdentity: {},
      documents: [],
      shareholderDeclaration: null,
      registrationProof: null,
      idCard: null,
      articleAssociation: null,
      bankAccount: {
        ownerAddress: {},
      },
    };
  },
  methods: {
    sendCompany() {
      this.uploadFiles()
        .then((response) => {
          let fileNames = response.data;

          this.documents.push({
            file: fileNames[0].fileName,
            fileName: this.idCard.name,
          });
          this.documents.push({
            file: fileNames[1].fileName,
            fileName: this.articleAssociation.name,
          });
          this.documents.push({
            file: fileNames[2].fileName,
            fileName: this.shareholderDeclaration.name,
          });
          this.documents.push({
            file: fileNames[3].fileName,
            fileName: this.registrationProof.name,
          });
          this.postFiles();
          this.$emit("getUser");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    sendIndividual() {
      this.uploadFile()
        .then((response) => {
          this.proofOfIdentity.file = response.data.fileName;
          this.proofOfIdentity.fileName = this.file.name;
          this.postFile();
          this.$emit("getUser");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    uploadFiles() {

        let formData = new FormData();

        formData.append("files", this.idCard);
        formData.append("files", this.articleAssociation);
        formData.append("files", this.shareholderDeclaration);
        formData.append("files", this.registrationProof);

        return FileService.uploadFiles(formData);

    },
    uploadFile() {
      let formData = new FormData();
      formData.append("file", this.file);
      return FileService.uploadFile(formData);
    },
    postFile() {
      this.$store.commit("IS_LOADING_TRUE");
      MangoPayService.postFile({
        file: this.proofOfIdentity.file,
        fileName: this.proofOfIdentity.fileName,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    postFiles() {
      this.$store.commit("IS_LOADING_TRUE");
      MangoPayService.postCompanyFiles({
        documents: this.documents,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    addBankAccount() {
      MangoPayService.addBankAccount(this.bankAccount)
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("getUser");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
    createWallet() {
      this.$store.commit("IS_LOADING_TRUE");
      MangoPayService.createWallet()
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("getUser");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
};
</script>

<style scoped></style>
