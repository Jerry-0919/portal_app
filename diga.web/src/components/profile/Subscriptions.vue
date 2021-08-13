<template>
    <div>
      <h2>
        {{$t('your_tariff')}}
      </h2>
      <div v-if="subs">
        <div class="row" v-for="ut in subs.userTariffs" :key="ut.tariffId">
          <div class="col-md-1">
            {{$t(ut.tariff.name)}}
          </div>
          <div class="col-md-2">
            {{$t('price')}}: {{ut.currentPrice}} €
          </div>
          <div class="col-md-2">
            {{$t('date_start')}}: {{ ut.start | moment("MMMM Do YYYY")}} 
          </div>
          <div class="col-md-2">
            {{$t('date_finish')}}: {{ut.end | moment("MMMM Do YYYY")}} 
          </div>
          <div class="col-md-1">
            <template>
              {{$t('trial')}}: 
            </template>
            <template v-if="ut.isTrial">
              {{$t('yes')}}
            </template>
            <template v-if="!ut.isTrial">
              {{$t('no')}}
            </template>
          </div>
          <div class="col-md-2">
            {{$t('number_of_users')}}: {{ut.numberOfUsers}} 
          </div>
          <div class="col-md-2">
            {{$t('price_for_next_user')}}: {{ut.priceForNextUser}} €
          </div>
        </div>
      </div>

      <h2>
        {{$t('your_modules')}}
      </h2>
      <div v-if="subs">
        <div class="row" v-for="um in subs.userModules" :key="um.moduleId">
          <div class="col-md-2">
            {{$t(um.module.name)}}
          </div>
          <div class="col-md-2">
            {{$t('price')}}: {{um.currentPrice}} €
          </div>
          <div class="col-md-3">
            {{$t('date_start')}}: {{ um.start | moment("MMMM Do YYYY")}} 
          </div>
          <div class="col-md-3">
            {{$t('date_finish')}}: {{um.end | moment("MMMM Do YYYY")}} 
          </div>
          <div class="col-md-2">
            <template>
              {{$t('trial')}}: 
            </template>
            <template v-if="um.isTrial">
              {{$t('yes')}}
            </template>
            <template v-if="!um.isTrial">
              {{$t('no')}}
            </template>
          </div>
        </div>

        <div v-for="(ut, i) in subs.userTariffs" :key="i">
          <div class="row" v-for="(tm, j) in ut.tariff.tariffModules" :key="j">
            <div class="col-md-2">
              {{$t(tm.module.bigTitleTextId)}}
            </div>
            <div class="col-md-2">
              {{$t('basic_module')}}
            </div>
            <div class="col-md-3">
              {{$t('date_start')}}: {{ ut.start | moment("MMMM Do YYYY")}} 
            </div>
            <div class="col-md-3">
              {{$t('date_finish')}}: {{ut.end | moment("MMMM Do YYYY")}} 
            </div>
            <div class="col-md-2">
              <template>
                {{$t('trial')}}: 
              </template>
              <template v-if="ut.isTrial">
                {{$t('yes')}}
              </template>
              <template v-if="!ut.isTrial">
                {{$t('no')}}
              </template>
            </div>
          </div>
        </div>
      </div>

      <div class="row">
        <div class="col-md-12 text-center">
          <b-button @click="changeSubs" variant="primary">{{$t('change_subscription')}}</b-button>
        </div>        
      </div>

    </div>
</template>

<script>
import SubscriptionService from '@/services/SubscriptionService.js';

export default {
    data() {
      return {
        subs: null,
      }
    },
    mounted(){
      this.$store.commit('IS_LOADING_TRUE')
      SubscriptionService.getSubscriptions()
        .then(response => {
          this.subs = response.data;
        })
        .catch(error => {
          console.log(error);
          alert('Error occured');
        })
        .finally(() => this.$store.commit('IS_LOADING_FALSE'));      
    },
    methods: {
      changeSubs(){
        window.location = '/price/index';
      }
    }
}
</script>

<style>

</style>