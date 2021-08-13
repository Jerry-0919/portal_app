<template>
  <div style="display: contents">
    <b-tr>
      <b-td v-if="isParent">{{ mySection.number }}.</b-td>
      <b-td v-else> {{ parentNumber }}.{{ mySection.number }}</b-td>
      <b-th
        v-if="isParent"
        :colspan="
          isAutoMeasurement === true
            ? isCustomer === true
              ? 15
              : isRejected === true
              ? 13
              : 12
            : isSummary === true
            ? 7
            : 6
        "
      >
        <template v-if="isEdit === false">{{ mySection.name }}</template>
        <input
          style="width: -webkit-fill-available"
          width="100%"
          type="text"
          v-if="isEdit === true"
          v-model="mySection.name"
        />
      </b-th>

      <template v-else>
        <b-td
          :colspan="
            isAutoMeasurement === true
              ? isCustomer === true
                ? 15
                : isRejected === true
                ? 13
                : 12
              : isSummary === true
            ? 7
            : 6
          "
        >
          <template v-if="isEdit === false">{{ mySection.name }}</template>
          <input
            style="width: -webkit-fill-available"
            width="100%"
            type="text"
            v-if="isEdit === true"
            v-model="mySection.name"
          />
        </b-td>
      </template>
      <b-td v-if="actionsShow === true"
        ><i
          v-if="isEdit === false"
          @click="isEdit = true"
          type="button"
          class="bx bxs-pencil align-middle edit p-1"
        ></i>
        <i
          v-else
          @click="
            isEdit = false;
            editSection();
          "
          type="button"
          class="bx bx-check align-middle edit p-1"
        ></i>
      </b-td>
      <b-td v-if="actionsShow === true"
        ><i
          @click="deleteSection(mySection)"
          type="button"
          class="bx bx-x align-middle delete p-1"
        ></i
      ></b-td>
      <b-td @click="isNewLine = !isNewLine" v-if="actionsShow === true"
        ><i type="button" class="bx bx-subdirectory-left add p-1 align-middle"></i
      ></b-td>
    </b-tr>
    <b-tr v-if="isNewLine">
      <b-td colspan="13">
        <div class="d-flex justify-content-center">
          <!-- <b-button variant="outline-primary" class="mx-2" @click="addSection">
            {{ $t("add_section") }}
          </b-button> -->
          <b-button
            variant="outline-primary"
            class="mx-2"
            @click="addSubsection"
          >
            {{ $t("add_subsection") }}
          </b-button>
          <b-button variant="outline-primary" class="mx-2" @click="addPosition">
            {{ $t("add_position") }}
          </b-button>
        </div>
      </b-td>
    </b-tr>

    <template v-if="mySection.positions && mySection.positions.length > 0">
      <estimateLinePosition
        v-for="position in mySection.positions"
        :key="position.id"
        :section="mySection"
        :pos="position"
        :parentNumber="parentNumber"
        :contract="contract"
        :actionsShow="actionsShow"
        :estimateId="estimateId"
        :editPriceAndNotes="editPriceAndNotes"
        :isAutoMeasurement="isAutoMeasurement"
        :measurementReportId="measurementReportId"
        :isCustomer="isCustomer"
        :isRejected="isRejected"
        :isSummary="isSummary"
        @addSubsection="addSubsection"
        @updatePositions="updatePositions"
        @deletePosition="deletePosition"
      />
      <b-tr>
        <b-td></b-td>
        <b-th colspan="4" class="text-right">TOTAL DO CAPITULO</b-th>
        <b-td
          v-if="isAutoMeasurement === true"
          :colspan="isCustomer === true ? 11 : isRejected === true ? 9 : 8"
          >{{
            parseFloat(countMeasurementTotal(mySection.positions)).toFixed(2)
          }}
          €</b-td
        >
        <b-td v-else :colspan="isSummary === true ? 3 : 2"
          >{{ parseFloat(countTotal(mySection.positions)).toFixed(2) }} €</b-td
        >
      </b-tr>
    </template>

    <template v-if="mySection.sections && mySection.sections.length > 0">
      <node
        v-for="(child, index) in mySection.sections"
        :key="child.id + index"
        :isParent="false"
        :section="child"
        :parentNumber="mySection.number"
        :contract="contract"
        :actionsShow="actionsShow"
        :estimateId="estimateId"
        :editPriceAndNotes="editPriceAndNotes"
        :isAutoMeasurement="isAutoMeasurement"
        :measurementReportId="measurementReportId"
        :isCustomer="isCustomer"
        :isRejected="isRejected"
        :isSummary="isSummary"
        @deleteChildSection="deleteChildSection"
        @saveDraft="saveDraft"
      ></node>
    </template>
  </div>
</template>

<script>
import estimateLinePosition from "./EstimateLinePosition.vue";

export default {
  name: "node",
  props: [
    "contract",
    "estimateId",
    "section",
    "isParent",
    "actionsShow",
    "parentNumber",
    "editPriceAndNotes",
    "isAutoMeasurement",
    "measurementReportId",
    "isCustomer",
    "isRejected",
    "isSummary",
  ],
  components: {
    estimateLinePosition,
  },
  data() {
    return {
      isEdit: false,
      isNewLine: false,
      mySection: null,
    };
  },
  mounted() {
    if (this.section.isEdit === true) {
      this.isEdit = true;
    }
    if (this.mySection.positions) {
      this.mySection.positions = this.mySection.positions.sort(
        (a, b) => a.number - b.number
      );
    }
    if (this.mySection.sections) {
      this.mySection.sections = this.mySection.sections.sort(
        (a, b) => a.number - b.number
      );
    }
  },
  methods: {
    saveDraft() {
      this.$emit("saveDraft");
    },
    updatePositions(positions) {
      this.mySection.positions = positions;
    },
    addPosition() {
      let positions = JSON.parse(JSON.stringify(this.mySection.positions));
      this.mySection.positions = [];

      let newPosition = {
        description: "",
        measurement: "",
        quantity: null,
        number: 1,
        notes: "",
        price: null,
        isEdit: true,
        parentSectionId: this.mySection.id,
        sectionId: this.mySection.id,
      };

      positions.forEach((p) => {
        p.number = p.number + 1;
      });

      positions.push(newPosition);

      positions = positions.sort((a, b) => a.number - b.number);

      this.mySection.positions = positions;
      this.isNewLine = false;
    },
    addSubsection() {
      let sections = JSON.parse(JSON.stringify(this.mySection.sections));
      this.mySection.sections = [];

      let newSection = {
        name: "",
        number: 1,
        isEdit: true,
        sections: [],
        positions: [],
        parentSectionId: this.mySection.id,
      };

      sections.forEach((s) => {
        s.number = s.number + 1;
      });

      sections.push(newSection);

      sections = sections.sort((a, b) => a.number - b.number);

      this.mySection.sections = sections;
      this.isNewLine = false;
    },
    editSection() {
      if (this.mySection.id > 0) {
        this.$store.commit("PUSH_SECTION_TO_UPDATE", {
          estimateId: this.estimateId,
          section: this.mySection,
        });
      } else {
        this.$store.commit("PUSH_SECTION_TO_ADD", {
          estimateId: this.estimateId,
          section: this.mySection,
        });
        this.$emit("saveDraft");
      }
    },
    deleteChildSection(section) {
      const index = this.mySection.sections.indexOf(section);
      if (index > -1) {
        this.mySection.sections.splice(index, 1);
      }
    },
    deleteSection(section) {
      if (confirm(this.$t("are_you_sure_section"))) {
        this.$store.commit("PUSH_SECTION_TO_REMOVE", {
          estimateId: this.estimateId,
          id: section.id,
          item: section,
        });
        this.$emit("deleteChildSection", section);
      }
    },
    deletePosition(index) {
      this.mySection.positions.splice(index, 1);
    },
    countPrice(position) {
      if (position.quantity && position.price) {
        return position.quantity * position.price;
      } else {
        return 0;
      }
    },
    countTotal(positions) {
      let total = 0.0;
      positions.forEach((p) => {
        total += this.countPrice(p);
      });
      return total;
    },
    countMeasurementTotal(positions) {
      let total = 0.0;
      positions.forEach((p) => {
        if (this.measurementReportId > 0 && p.measurementReportPositions) {
          let mposition = p.measurementReportPositions.find(
            (mp) => parseInt(mp.reportId) === parseInt(this.measurementReportId)
          );
          if (mposition != null) {
            total += mposition.quantityCompleted * p.price;
          }
        }
      });
      return total;
    },
  },
  watch: {
    section: {
      immediate: true,
      handler() {
        this.mySection = this.section;
      },
    },
  },
};
</script>

<style scoped></style>
