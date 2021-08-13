<template>
  <div>
    <p class="h3 my-3">{{ $t("terms_and_conditions") }}</p>
    <p>
      Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ipsum culpa
      repellendus laborum maxime dignissimos dolor excepturi iusto nemo
      aspernatur? Qui modi inventore reprehenderit, nostrum quaerat libero
      maiores consequuntur illo veritatis. Lorem ipsum dolor sit amet,
      consectetur adipisicing elit. Saepe, culpa obcaecati. Qui accusantium sit
      error, dolorem alias incidunt est. Laborum, atque ipsum debitis obcaecati
      dolor illo magni provident harum vitae? Lorem ipsum dolor, sit amet
      consectetur adipisicing elit. Reprehenderit omnis, doloribus autem, non
      quam quibusdam harum accusamus voluptatem in perspiciatis consequuntur
      sint nam debitis sapiente ex, optio totam delectus quis. Lorem ipsum,
      dolor sit amet consectetur adipisicing elit. Facilis placeat in quisquam
      dolorum numquam, rerum expedita corporis eveniet quas nostrum, quia
      veritatis neque quos sint sit exercitationem obcaecati perferendis magnam!
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Quisquam
      consequatur laudantium voluptatibus impedit unde. Error eius consequuntur
      tenetur unde molestias esse doloribus animi, temporibus placeat eaque
      laborum, maiores, ex quo! Lorem ipsum dolor sit amet consectetur
      adipisicing elit. Recusandae, suscipit placeat accusamus voluptas odio est
      ea accusantium dignissimos et officia, cupiditate aperiam atque facilis?
      Adipisci earum fuga illo odit reiciendis. Lorem ipsum dolor sit amet
      consectetur adipisicing elit. Asperiores nihil necessitatibus sequi
      placeat tenetur, perspiciatis, excepturi dolor, consectetur assumenda amet
      accusantium velit fuga numquam tempore repellendus voluptatem vitae.
      Voluptatem, hic. Lorem ipsum dolor sit amet consectetur adipisicing elit.
      Unde perspiciatis vero, reprehenderit beatae necessitatibus dignissimos
      animi distinctio illo porro fuga maxime nemo voluptate aspernatur tempore?
      Incidunt consectetur dignissimos blanditiis. Corporis. Lorem ipsum dolor
      sit amet consectetur, adipisicing elit. At, dolore omnis! Architecto
      dolorem non, earum pariatur, molestias voluptatem saepe voluptatibus
      praesentium expedita cum quae et assumenda. Voluptas debitis praesentium
      quis. Lorem ipsum dolor sit amet consectetur adipisicing elit. Cumque
      veniam ipsa saepe sint necessitatibus incidunt nihil totam delectus, dolor
      omnis itaque libero sed molestiae assumenda non repellat, rerum, eius
      quia. lorem
    </p>
    <b-form-group :label="$t('read_terms')">
      <b-form-radio
        v-model="termsAndConditions"
        class="font-weight-bold blue"
        name="termsradio"
        :value="true"
        >{{ $t("terms_yes") }}</b-form-radio
      >
      <b-form-radio
        v-model="termsAndConditions"
        class="font-weight-bold orange"
        name="termsradio"
        :value="false"
        >{{ $t("terms_no") }}</b-form-radio
      >
    </b-form-group>

    <div class="d-flex justify-content-between my-3">
      <b-button class="btn b-grey-outline" @click="$emit('TabPrev')">{{
        $t("previous")
      }}</b-button>
      <b-button @click="$emit('Cancel')" class="btn b-orange-outline">{{
        $t("cancel_post")
      }}</b-button>
      <b-button
        @click="submit"
        :disabled="termsAndConditions === false || termsAndConditions === null"
        v-if="$moment(contract.publishDate).isSame($moment(), 'day')"
        class="btn b-blue"
        >{{ $t("post") }}</b-button
      >
      <b-button
        @click="submit"
        v-else
        :disabled="termsAndConditions === false || termsAndConditions === null"
        variant="primary mx-2"
        >{{ $t("schedule_post") }}</b-button
      >
    </div>
  </div>
</template>

<script>
import ContractService from "@/services/ContractService.js";
export default {
  props: ["contract"],
  components: {},
  data() {
    return {
      termsAndConditions: false,
    };
  },
  methods: {
    submit() {
      this.$store.commit("IS_LOADING_TRUE");
      ContractService.Publicate(this.contract.id)
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
};
</script>
<style></style>
