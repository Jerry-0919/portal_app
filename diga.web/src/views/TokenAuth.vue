<template>
  <div>

  </div>
</template>

<script>
import axios from 'axios';

export default {
    props: ['token'],
    mounted(){
      this.$store.commit('IS_LOADING_TRUE')
      window.$cookies.set("AuthToken", this.token, "27d")
      axios
        .post('/authToken', {
          authToken: this.token
        })
        .then(response => {
          this.$store.commit('PUSH_USER', response.data)
          window.$cookies.set("bearer", response.data.access_token, "27d")
          axios.interceptors.request.use(
            (config) => {
              let token = response.data.access_token;

              if (token) {
                config.headers['Authorization'] = `Bearer ${ token }`;
              }

              return config;
            }, 
            (error) => {
              return Promise.reject(error);
            }
          );

          this.$router.push('/');
        })
        .catch(error => {
          console.log(error);
          alert('Error occured');
        })
        .finally(() => this.$store.commit('IS_LOADING_FALSE'));
    }
}
</script>

<style>

</style>