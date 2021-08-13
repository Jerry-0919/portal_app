<template>
  <div>
    <b-table-simple
      hover
      no-border-collapse
      stripped
      sticky-header
      responsive
    >
      <b-thead >
        <b-tr id="first" class="bg-white">
          <b-th colspan="4" class="text-uppercase text-center align-middle">
            <p>{{$t('comparison_card')}}</p>
          </b-th>
          <b-th colspan="2" v-for="(bid, idx) in list" :key="bid.id">
            <p :class="compareExecutor(idx)">
              {{ $t("executor") }} {{ idx + 1 }}</p
            >
           <p> {{ bid.fullName }}</p> 
            <span :class="compareDays(idx)">
              {{ bid.daysNumber }} {{ $t("days") }}</span
            >
          </b-th>
        </b-tr>
        <b-tr id="second">
          <b-th style="background: #e5e5e5;">№</b-th>
          <b-th style="background: #e5e5e5;">{{ $t("description") }}</b-th>
          <b-th style="background: #e5e5e5;">{{ $t("quantity") }}</b-th>
          <b-th style="background: #e5e5e5;">{{ $t("units") }}</b-th>

          <div 
            style="display: contents;"
            v-for="(bid, idx) in list"
            :key="bid.id"
          >
            <b-th style="position: sticky" :class="compareClass(idx)">{{ $t("unit_price") }}</b-th>
            <b-th style="position: sticky" :class="compareClass(idx)">{{ $t("value") }}</b-th>
          </div>
        </b-tr>
      </b-thead>

      <b-tbody>
        <b-tr v-for="position in mainEstimatePositions" :key="position.id">
          <b-td> {{ position.fullNumber }}. </b-td>
          <b-td>
            {{ position.description }}
          </b-td>
          <b-td>
            {{ parseFloat(position.quantity).toFixed(2) }}
          </b-td>
          <b-td>
            {{ position.measurement }}
          </b-td>

          <div
            style="display: contents"
            v-for="(bid, idx) in list"
            :key="bid.id"
          >
            <b-td :class="compareClass(idx)">
              {{ getPositionByNumber(position.fullNumber, bid).price }}
            </b-td>
            <b-td :class="compareClass(idx)">
              {{
                parseFloat(
                  getPositionByNumber(position.fullNumber, bid).price *
                    getPositionByNumber(position.fullNumber, bid).quantity
                ).toFixed(2)
              }}
            </b-td>
            <!-- <b-td class="col-2"
              >{{ getPositionByNumber(position.fullNumber, bid).notes }}
            </b-td> -->
            <!-- <b-td class="col-2"> {{ bid.userName }} </b-td> -->
            <!-- <b-td class="col-1"> </b-td> -->
          </div>
        </b-tr>
        <!-- <estimateLine
          v-for="section in estimate.sections"
          :key="section.id"
          :contract="contract"
          :estimate="estimate"
          :section="section"
          :isParent="true"
        /> -->
      </b-tbody>
      <b-tfoot>
        <b-tr>
          <b-th colspan="4"> </b-th>
          <div  v-for="(bid, idx) in list" :key="bid.id" style="display: contents">
        
          <b-td colspan=2 :class="compareTotal(idx)">
            <div class="d-flex justify-content-between">
            <span> TOTAL</span>
            <span> {{ parseFloat(bid.price).toFixed(2) }} €</span>
            </div>
            
           </b-td>
          </div>
        </b-tr>
      </b-tfoot>
    </b-table-simple>
  </div>
</template>

<script>
import EstimateService from "@/services/EstimateService.js";

export default {
  data() {
    return {
      list: [],
      mainEstimatePositions: [],
    };
  },
  props: ["id"],
  methods: {
    compareTotal(idx){
        if (idx % 2 == 0) {
        return "bg-orangelight totalOrange";
      } else {
        return "bg-bluelight totalBlue";
      }
    },
    compareExecutor(idx){
        if (idx % 2 == 0) {
        return "orange";
      } else {
        return "blue";
      }
    },
        compareDays(idx){
        if (idx % 2 == 0) {
        return "badge counter-orange p-2";
      } else {
        return "badge counter-blue p-2";
      }
    },
    compareClass(idx) {
      if (idx % 2 == 0) {
        return "bg-orangelight";
      } else {
        return "bg-bluelight";
      }
    },
    customSort(d, order) {
      var sort = {
          asc: function(a, b) {
            var l = 0,
              m = Math.min(a.value.length, b.value.length);
            while (l < m && a.value[l] === b.value[l]) {
              l++;
            }
            return l === m
              ? a.value.length - b.value.length
              : a.value[l] - b.value[l];
          },
          desc: function(a, b) {
            return sort.asc(b, a);
          },
        },
        // temporary array holds objects with position and sort-value
        mapped = d.map(function(el, i) {
          return { index: i, value: el.fullNumber.split(".").map(Number) };
        });

      // sorting the mapped array containing the reduced values
      mapped.sort(sort[order] || sort.asc);

      // container for the resulting order
      return mapped.map(function(el) {
        return d[el.index];
      });
    },
    getCompare() {
      this.$store.commit("IS_LOADING_TRUE");
      EstimateService.getCompare(this.id)
        .then((response) => {
          this.list = response.data;
          if (response.data.length > 0) {
            this.mainEstimatePositions = this.customSort(
              this.list[0].positions
            );
          }
        })
        .catch(() => {
          this.$toasted.error(this.$t("oops_error"));
        })
        .finally(() => this.$store.commit("IS_LOADING_FALSE"));
    },

    getPositionByNumber(fullNumber, bid) {
      let position = { price: 0, notes: "", quantity: 0 };
      if (bid.positions && bid.positions.length > 0) {
        position = bid.positions.find((b) => b.fullNumber === fullNumber);
      }

      return position;
    },
  },
  mounted() {
    this.getCompare();
  },
  computed: {},
};
</script>

<style scoped>
table {
  text-align: left;
  position: relative;
}
#first th {
  position: sticky !important;
  top: 0; /* Don't forget this, required for the stickiness */
}
#second th {
  position: sticky !important;
  top: 13.2%; /* Don't forget this, required for the stickiness */
}
.b-table-sticky-header {
  max-height: 900px !important;
  margin-bottom: 0 !important;
}

</style>
