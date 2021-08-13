<template>
    <div>
<publicProfile :id="id" />
    </div>
</template>

<script>
import publicProfile from "../views/PublicProfile.vue";
    export default {
        components:{publicProfile},
        props: ["id"],
        
    }
</script>

<style scoped>

</style>