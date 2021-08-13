<template>
  <div style="display: contents">
    <b-tr>
      <b-td v-if="parentNumber">
        {{ parentNumber }}.{{ section.number }}.{{ position.number }}</b-td
      >
      <b-td v-else> {{ section.number }}.{{ position.number }}</b-td>
      <b-td>
        <template v-if="isEdit === false">{{ position.description }}</template>
        <input
          style="width: -webkit-fill-available"
          width="100%"
          type="text"
          v-if="isEdit === true"
          v-model="position.description"
        />
      </b-td>
      <b-td>
        <template v-if="isEdit === false">{{ position.measurement }}</template>
        <input
          style="width: -webkit-fill-available"
          width="100%"
          type="text"
          v-if="isEdit === true"
          v-model="position.measurement"
        />
      </b-td>
      <b-td>
        <template v-if="isEdit === false">{{
          parseFloat(position.quantity || 0).toFixed(2)
        }}</template>
        <input
          style="width: -webkit-fill-available"
          width="100%"
          step="0.1"
          type="number"
          v-if="isEdit === true"
          v-model.number="position.quantity"
        />
      </b-td>
      <b-td>
        <template v-if="editPriceAndNotes === false && isEdit === false">{{
          parseFloat(position.price || 0).toFixed(2)
        }}</template>
        <input
          v-if="editPriceAndNotes === true || isEdit === true"
          class="p-0"
          min="0"
          step="0.1"
          v-model.number="position.price"
          type="number"
      /></b-td>
      <b-td class="align-top">
        {{ parseFloat(countPrice || 0).toFixed(2) }}
      </b-td>
      <b-td v-if="isAutoMeasurement !== true">
        <template v-if="editPriceAndNotes === false  && isEdit === false">{{
          position.notes
        }}</template>
        <b-form-textarea
          v-if="editPriceAndNotes === true || isEdit === true"
          v-model="position.notes"
          rows="3"
        ></b-form-textarea>
      </b-td>
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
            editPosition();
          "
          type="button"
          class="bx bx-check align-middle edit p-1" 
        ></i
      ></b-td>
      <b-td v-if="actionsShow === true"
        ><i
          @click="deletePosition(position)"
          type="button"
          class="bx bx-x align-middle delete p-1"
        ></i
      ></b-td>
      <b-td @click="isNewLine = !isNewLine" v-if="actionsShow === true"
        ><i type="button" class="bx bx-subdirectory-left add align-middle p-1"></i
      ></b-td>
      <template v-if="isAutoMeasurement === true && currentReportPosition">
        <b-td>
          {{
            parseFloat(
              (currentReportPosition.quantityCompleted / position.quantity) *
                100 || 0
            ).toFixed(2)
          }}
          %
        </b-td>
        <b-td>
          <template v-if="isCustomer === true">
            {{ parseFloat(currentReportPosition.quantityCompleted).toFixed(2) }}
          </template>
          <input
            v-else
            class="p-0"
            min="0"
            step="1"
            v-model.number="currentReportPosition.quantityCompleted"
            type="number"
          />
        </b-td>
        <b-td>
          {{
            parseFloat(
              currentReportPosition.quantityCompleted * position.price || 0
            ).toFixed(2)
          }}
        </b-td>
        <b-td>
          {{ parseFloat(accumulatedQuantity).toFixed(2) }}
        </b-td>
        <b-td>
          {{
            parseFloat(accumulatedQuantity * (position.price || 0)).toFixed(2)
          }}
        </b-td>
        <b-td>
          {{
            parseFloat((position.quantity || 0) - accumulatedQuantity).toFixed(
              2
            )
          }}
        </b-td>
        <b-td>
          {{
            parseFloat(
              ((position.quantity || 0) - accumulatedQuantity) *
                (position.price || 0)
            ).toFixed(2)
          }}
        </b-td>
        <b-td v-if="isCustomer === false && isRejected === true">
          {{ currentReportPosition.rejectionReason }}
        </b-td>
        <template v-if="isCustomer === true">
          <b-td>
            <template v-if="currentReportPosition.quantityCompleted > 0">
              {{ currentReportPosition.status }}
            </template>
            <template v-else> Executor did not do anything </template>
          </b-td>
          <b-td>
            <button
              :disabled="!currentReportPosition.quantityCompleted > 0"
              @click="acceptPosition"
            >
              Accept
            </button>
          </b-td>
          <b-td>
            <b-button
              :disabled="!currentReportPosition.quantityCompleted > 0"
              v-b-modal="'modal-' + position.id"
              >Reject</b-button
            >

            <b-modal
              :id="'modal-' + position.id"
              title="Please write the reject reason"
              hide-footer
            >
              <textarea
                v-model="currentReportPosition.rejectionReason"
                class="form-control"
              >
              </textarea>
              <b-button
                class="my-2"
                @click="rejectPosition('modal-' + position.id)"
                :disabled="
                  currentReportPosition.rejectionReason === null ||
                  currentReportPosition.rejectionReason === ''
                "
                >Save</b-button
              >
            </b-modal>
          </b-td>
        </template>
      </template>
      <b-td v-if="isSummary === true">
        {{
          parseFloat((acceptedQuantity / position.quantity) * 100 || 0).toFixed(
            2
          )
        }}
        %
      </b-td>
    </b-tr>
    <b-tr v-if="isNewLine">
      <b-td colspan="13">
        <div class="d-flex justify-content-center">
          <b-button
            variant="outline-primary"
            class="mx-2"
            @click="addSubsection"
          >
            {{ $t("add_subsection") }}
          </b-button>
          <b-button
            variant="outline-primary"
            class="mx-2"
            @click="addPosition(position)"
          >
            {{ $t("add_position") }}
          </b-button>
        </div>
      </b-td>
    </b-tr>
  </div>
</template>

<script>
export default {
  props: [
    "section",
    "contract",
    "pos",
    "actionsShow",
    "estimateId",
    "parentNumber",
    "editPriceAndNotes",
    "isAutoMeasurement",
    "measurementReportId",
    "isCustomer",
    "isRejected",
    "isSummary",
  ],
  data() {
    return {
      position: {},
      isEdit: false,
      isNewLine: false,
    };
  },
  mounted() {
    if (this.pos.isEdit === true) {
      this.isEdit = true;
    }
  },
  methods: {
    acceptPosition() {
      this.currentReportPosition.status = "Accepted";
      this.$store.commit("PUSH_MR_POSITION_TO_UPDATE", {
        id: this.measurementReportId,
        position: this.currentReportPosition,
      });
    },
    rejectPosition(modalId) {
      this.currentReportPosition.status = "Rejected";
      this.$store.commit("PUSH_MR_POSITION_TO_UPDATE", {
        id: this.measurementReportId,
        position: this.currentReportPosition,
      });
      this.$bvModal.hide(modalId);
    },
    addSubsection() {
      this.$emit("addSubsection");
      this.isNewLine = false;
    },

    addPosition(position) {
      let positions = JSON.parse(JSON.stringify(this.section.positions));

      let newPosition = {
        description: "",
        measurement: "m2",
        quantity: null,
        number: position.number + 1,
        notes: "",
        price: null,
        isEdit: true,
        parentSectionId: this.section.id,
        sectionId: this.section.id,
      };

      var biggerPositions = positions.filter((p) => {
        return p.number > position.number;
      });
      biggerPositions.forEach((p) => {
        p.number = p.number + 1;
      });
      positions.push(newPosition);
      positions = positions.sort((a, b) => a.number - b.number);
      this.isNewLine = false;
      this.$emit("updatePositions", positions);
    },
    editPosition() {
      if (this.position.id > 0) {
        this.$store.commit("PUSH_POSITION_TO_UPDATE", {
          estimateId: this.estimateId,
          position: this.position,
        });
      } else {
        this.$store.commit("PUSH_POSITION_TO_ADD", {
          estimateId: this.estimateId,
          position: this.position,
        });
      }
    },
    deletePosition(position) {
      if (confirm(this.$t("are_you_sure_section"))) {
        this.$store.commit("PUSH_POSITION_TO_REMOVE", {
          estimateId: this.estimateId,
          id: position.id,
          item: position,
        });
        const index = this.section.positions.indexOf(position);
        if (index > -1) {
          this.$emit("deletePosition", index);
        }
      }
    },
  },
  computed: {
    countPrice() {
      if (this.position.quantity > 0 && this.position.price > 0) {
        return (
          parseFloat(this.position.quantity) * parseFloat(this.position.price)
        );
      } else {
        return 0;
      }
    },
    currentReportPosition() {
      if (
        this.measurementReportId > 0 &&
        this.position &&
        this.position.measurementReportPositions
      ) {
        return this.position.measurementReportPositions.find(
          (mp) => parseInt(mp.reportId) === parseInt(this.measurementReportId)
        );
      } else {
        return null;
      }
    },
    accumulatedQuantity() {
      if (
        this.position &&
        this.position.measurementReportPositions && 
        this.position.measurementReportPositions.length 
      ) {
        return this.position.measurementReportPositions.reduce(
          (a, b) => a.quantityCompleted || 0 + b.quantityCompleted || 0
        );
      } else {
        return 0;
      }
    },
    acceptedQuantity() {
      if (
        this.position &&
        this.position.measurementReportPositions &&
        this.position.measurementReportPositions.length 
      ) {
        return this.position.measurementReportPositions
          .filter((p) => p.status === "Accepted")
          .reduce(
            (a, b) => a.quantityCompleted || 0 + b.quantityCompleted || 0, 0
          );
      } else {
        return 0;
      }
    },
  },
  watch: {
    pos: {
      immediate: true,
      handler() {
        this.position = this.pos;
      },
    },
    "currentReportPosition.quantityCompleted": {
      deep: true,
      handler() {
        this.$store.commit("PUSH_MR_POSITION_TO_UPDATE", {
          id: this.measurementReportId,
          position: this.currentReportPosition,
        });
      },
    },
  },
};
</script>

<style scoped></style>
