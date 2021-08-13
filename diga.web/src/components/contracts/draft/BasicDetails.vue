<template>
  <form :key="render" @submit.prevent="submit">
    <p class="h3 my-3">{{ $t("contract_data") }}</p>
    <b-row>
      <b-col>
        <b-form-group :label="$t('contract_name')">
          <b-form-input
            :placeholder="$t('contract_name')"
            @input="errors.name = false"
            :class="{ 'input-invalid': errors.name }"
            v-model="contract.name"
            type="text"
          ></b-form-input>
        </b-form-group>
        <b-form-group :label="$t('contract_type')">
          <b-form-select
            v-model="contract.contractTypeId"
            @change="errors.contractTypeId = false"
            :class="{ 'input-invalid': errors.contractTypeId }"
          >
            <option
              v-for="ctype in contractTypes"
              :value="ctype.id"
              :key="ctype.id"
              >{{ $t(ctype.name) }}</option
            >
          </b-form-select>
        </b-form-group>
        <b-form-group :label="$t('country')">
          <b-form-select
            @change="errors.countryId = false"
            :class="{ 'input-invalid': errors.countryId }"
            v-model="contract.countryId"
          >
            <option
              v-for="country in countries"
              :value="country.id"
              :key="country.id"
              >{{ country.name }}</option
            >
          </b-form-select>
        </b-form-group>
        <b-form-group :label="$t('city')">
          <b-form-select
            @change="errors.cityId = false"
            :class="{ 'input-invalid': errors.cityId }"
            v-model="contract.cityId"
          >
            <option v-for="city in cities" :value="city.id" :key="city.id">{{
              city.name
            }}</option>
          </b-form-select>
        </b-form-group>

        <b-form-group class="mb-0">
          <b-form-checkbox v-model="contract.addressHidden">
            {{ $t("hide_address") }}
          </b-form-checkbox>
        </b-form-group>
        <div class="text-right">
          <b-button @click="$emit('Cancel')" class="btn b-orange-outline">{{
            $t("cancel_post")
          }}</b-button>
        </div>
      </b-col>
      <b-col>
        <b-form-group :label="$t('priority')">
          <b-form-select
            @change="errors.contractPriorityId = false"
            :class="{ 'input-invalid': errors.contractPriorityId }"
            v-model="contract.contractPriorityId"
            name="priority"
          >
            <option
              v-for="priority in contractPriorities"
              :value="priority.id"
              :key="priority.id"
              >{{ $t(priority.value) }}</option
            >
          </b-form-select>
        </b-form-group>
        <b-form-group :label="$t('category')">
          <multiselect
            @change="errors.categoryIds = false"
            :class="{ 'input-invalid': errors.categoryIds }"
            v-model="contract.categoryIds"
            :options="categories"
            :multiple="true"
            group-values="platformCategories"
            group-label="nameId"
            :placeholder="$t('type_to_search')"
            track-by="id"
            label="nameId"
            ><span slot="noResult">{{ $t("no_elements_found") }}</span>
          </multiselect>
        </b-form-group>
        <b-form-group :label="$t('tags')">
          <multiselect
            @change="errors.tags = false"
            :class="{ 'input-invalid': errors.tags }"
            v-model="contract.tags"
            :tag-placeholder="$t('add_as_new_tag')"
            :placeholder="$t('search_or_add_tag')"
            :options="tags"
            :multiple="true"
            :taggable="true"
            @tag="addTag"
          ></multiselect>
        </b-form-group>

        <b-form-group :label="$t('contract_address')">
          <b-form-input
            :placeholder="$t('contract_address')"
            @input="errors.address = false"
            :class="{ 'input-invalid': errors.address }"
            v-model="contract.address"
            type="text"
          >
          </b-form-input>
        </b-form-group>
        <div>
          <b-button class="btn b-blue mt-3" type="submit">{{
            $t("next")
          }}</b-button>
        </div>
      </b-col>
    </b-row>
  </form>
</template>

<script>
import { mapState } from "vuex";
import ContractService from "@/services/ContractService.js";
import Multiselect from "vue-multiselect";
export default {
  props: ["contract"],
  components: { Multiselect },
  data() {
    return {
      render: 0,
      errors: {},
    };
  },
  methods: {
    addTag(newTag) {
      if (!this.contract.tags) {
        this.contract.tags = [];
      }
      this.contract.tags.push(newTag);
      this.render += 1;
      // this.value.push(tag);
    },
    nameWithLang({ name, language }) {
      return `${name} — [${language}]`;
    },
    validateForm() {
      if (
        this.contract.name &&
        this.contract.contractTypeId &&
        this.contract.countryId &&
        this.contract.cityId &&
        this.contract.contractPriorityId &&
        this.contract.categoryIds &&
        this.contract.tags &&
        this.contract.address
      ) {
        return true;
      }
      this.errors = {};
      if (!this.contract.name) {
        this.errors.name = true;
      }
      if (!this.contract.contractTypeId) {
        this.errors.contractTypeId = true;
      }
      if (!this.contract.countryId) {
        this.errors.countryId = true;
      }
      if (!this.contract.cityId) {
        this.errors.cityId = true;
      }
      if (!this.contract.contractPriorityId) {
        this.errors.contractPriorityId = true;
      }
      if (!this.contract.categoryIds) {
        this.errors.categoryIds = true;
      }
      if (!this.contract.tags) {
        this.errors.tags = true;
      }
      if (!this.contract.address) {
        this.errors.address = true;
      }
      return false;
    },

    submit() {
      if (this.validateForm() === false) {
        this.$toasted.error(this.$t("fill_in_the_fields"));
      } else {
        if (this.contract.id > 0) {
          this.sendBasicDetails();
        } else {
          this.$store.commit("IS_LOADING_TRUE");
          ContractService.createContract()
            .then((response) => {
              this.contract.id = response.data.contractId;
              this.sendBasicDetails();
            })
            .catch((error) => {
              console.log(error);
            })
            .finally(() => this.$store.commit("IS_LOADING_FALSE"));
        }
      }
    },
    sendBasicDetails() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.saveBasicDetails(this.contract.id, {
        contractId: this.contract.id,
        name: this.contract.name,
        priorityId: this.contract.contractPriorityId,
        categoryIds: this.contract.categoryIds.map((c) => {
          return c.id;
        }),
        tags: this.contract.tags,
        contractTypeId: this.contract.contractTypeId,
        cityId: this.contract.cityId,
        address: this.contract.address,
        addressHidden: this.contract.addressHidden,
      })
        .then(() => {
          this.$toasted.success(this.$t("your_request_was_successfully_sent"));
          this.$emit("TabNext");
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },
  },
  computed: {
    ...mapState([
      "countries",
      "cities",
      "contractPriorities",
      "contractTypes",
      "categories",
      "tags",
    ]),
  },
  watch: {},
};
</script>
<style></style>
