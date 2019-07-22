<template>
  <div id="dashboard-overlay" v-show="getActiveStatus" >
    <strong>this.is overlay</strong>
  </div>
</template>

<script>
import store from '../../stores/dashboard.store';
export default {

  computed:{
    getActiveStatus: ()=> {return store.state.overlayVisible}
  }

}
</script>
