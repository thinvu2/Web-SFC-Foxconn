<template>
  <div class="div_route">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowPieChart"
        @click="$emit('hideModal')"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowPieChart">
        <div class="content-top">
          <b> </b>
          <span class="closeButton" @click="$emit('hideModal')"
            ><Icon style="font-size: 23px; color: red" icon="times-circle"
          /></span>
        </div>
        <div id="chart">
          <apexchart
            type="pie"
            height="500"
            :options="chartOptions"
            :series="series"
          ></apexchart>
        </div>
      </div>
    </transition>
  </div>
</template>
<script>
// import axios from "axios";
export default {
  props: {
    isShowModal: Boolean,
    listDetail: Array,
  },
  components: {},
  data() {
    return {
      series: this.$store.state.mixPieSeries,
      chartOptions: this.$store.state.mixPieOption,
    };
  },
  methods: {},
};
</script>
<style lang="scss" >
.contentRoute {
  border: 1px solid #0959a2;
  border-radius: 10%;
  font-size: 13px;
  padding: 0 10px;
  font-weight: 555;
  background-color: #d0e9fd;
  margin: auto;
  text-align: center;
  padding-top: 5px;
}
#line_relation {
  // height: 500px !important;
  min-height: 100px !important;
  td {
    height: 250px !important;
    // min-height: 500px !important;
  }
}
#tb_data tr {
  text-align: left;
  /* line-height: 4em; */
}

#tb_data tr td {
  /* padding: 0 10px; */
  text-align: center;
  /* padding-right:1em;; 
	border:1px solid #ccc; */
  white-space: nowrap;
}

#tb_data tr .space {
  min-width: 1em;
}

#tb_data tr .have_data {
  /* background-color:#ccc;	 */
  /* border:1px dotter #8a8a8a; */
  padding: 5px 0;
}

#tb_data tr .have_data span {
  cursor: pointer;
}

#tb_data tr .relation,
#tb_data tr .relation_return {
  background-repeat: no-repeat;
  // background-repeat-y: initial;
  background-position: center;
  height: 100%;
  width: 100%;
  line-height: 100%;
}
#tb_data {
  //height: 400px;
  /* max-height:100px; */
  width: 1%;
  z-index: 100;
}
#line_main {
  position: relative;
  /* background-co
    lor:#ccc; */
  /* background-image: url(bg_line_main.png); */
  background-repeat: no-repeat;
  // background-repeat-x: initial;
  background-position: center;
}
#line_over {
  height: 80px !important;
}
#tb_data {
  z-index: 1000 !important;
  height: 350px !important;
}
#content {
  height: 70vh !important;
  width: 90vw;
  overflow-y: hidden;
}
.space {
  width: 60px !important;
}
</style>
<style lang="scss" scoped>
.text-search {
  text-decoration: underline;
  font-size: 14px;
  color: #fa255d;
  font-weight: 555;
  cursor: pointer;
  user-select: none;
  &:hover {
    color: #ba375a;
  }
}
.text-route {
  color: #418bca;
}
.img-excel {
  height: 45px;
  width: 70px;
  &:hover {
    cursor: pointer;
  }
}
.closeButton {
  margin-top: -15px;
  margin-right: -15px;
  &:hover {
    cursor: pointer;
  }
}
.count-number {
  color: red;
}
.content-top {
  display: flex;
  justify-content: space-between;
}

body {
  font-family: "montserrat", sans-serif;
}

#app {
  position: absolute;

  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
  min-height: 100vh;
  overflow-x: hidden;
}

.button {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;

  display: inline-block;
  padding: 15px 25px;
  background-image: linear-gradient(to right, #cc2e5d, #ff5858);
  border-radius: 8px;

  color: #fff;
  font-size: 18px;
  font-weight: 700;

  box-shadow: 3px 3px rgba(0, 0, 0, 0.4);
  transition: 0.4s ease-out;

  &:hover {
    box-shadow: 6px 6px rgba(0, 0, 0, 0.6);
  }
}

.modal-overlay {
  position: fixed;
  z-index: 99990 !important;
  height: 100vh;
  width: 100vw;
 // position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 99;
  background-color: rgba(0, 0, 0, 0.3);
}

.my_modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 99991 !important; 
  height: 85vh;
  width: 100%;
  max-width: 95vw;
  background-color: #fff;
  border-radius: 16px;

  padding: 25px;

  h1 {
    color: #222;
    font-size: 32px;
    font-weight: 900;
    margin-bottom: 15px;
  }

  p {
    color: #666;
    font-size: 18px;
    font-weight: 400;
    margin-bottom: 15px;
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.5s;
}

.slide-enter,
.slide-leave-to {
  transform: translateY(-50%) translateX(100vw);
}
</style>
