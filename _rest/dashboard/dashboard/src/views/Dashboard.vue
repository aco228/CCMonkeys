<template>
  <div @keydown.tab.native="onTabPress">
    <DashboardOverlay/>
    <DashboardTableView/>
  </div>
</template>

<script>
import store from '../stores/dashboard.store.js';
import DashboardTableView from '../components/dashboard/table.dashboard.view';
import DashboardOverlay from '../components/dashboard/overlay.dashboard.view';

console.log(store);

export default {
  name: 'dashboard',
  components:{
    DashboardTableView, DashboardOverlay
  },
  created: ()=> {
    clickEventHandling();
  },
  methods: {

    clickEventHandling: ()=>{
      document.onkeydown = function (e) {
        var keyCode = e ? (e.which ? e.which : e.keyCode) : event.keyCode;
        if(keyCode == 9){ // this is tab
          e.preventDefault();
          if(store.state.overlayVisible) store.state.overlayVisible = false;
          else store.state.overlayVisible = true;
        }
        console.log('aco');
      };
    }

  }

}
</script>

