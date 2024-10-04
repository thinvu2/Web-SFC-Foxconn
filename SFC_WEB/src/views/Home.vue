<template>
  
  
  <div class="container body">
    <div class="main_container">
      <LeftNav />
      <!-- top navigation -->
      <TopNav />
      <!-- /top navigation -->
      <!-- page content -->
      <div class="right_col" role="main">

        <router-view />
      </div>
      <!-- /page content -->
      <!-- footer content -->
      <footer>
        <div class="pull-right">
          @Copyright 2025 IT Shopfloor Vietnam. All Rights Reserved. (535) 31529
        </div>
        <div class="clearfix"></div>
      </footer>
      <!-- /footer content -->
    </div>
  </div>
</template>

<script>
import TopNav from "../components/Home/TopNav.vue";
import LeftNav from "../components/Home/LeftNav.vue";
// import * as mykey  from '../../public/template/build/js/custom.min.js'
export default {
  name: "Home",
  data() {
    return {
      copyright: "",
      language: "",
    };
  },
  components: {
    TopNav,
    LeftNav,
  },
  methods: {
    changeLanguage(lang) {
      this.$store.commit("changeLanguage", lang);
      localStorage.language = lang;
    },
  },
  mounted() {
    // let externalScript = document.createElement('script')
    // externalScript.setAttribute('src', '/../../public/template/build/js/custom.min.js')
    // document.body.appendChild(externalScript)
  },
  created() {
    if (
      typeof localStorage.username == "undefined" ||
      typeof localStorage.password == "undefined"
    ) {
      this.$router.push({ name: "Login" });
    } else {
      if (localStorage.username.length == 0) {
        this.$router.push({ name: "Login" });
      }
    }
  },
};
</script>
<style scoped lang="scss">
#pleaserotate-graphic-login {
  width: 40%;
  top: 40%;
  left: 30%;
  transform: scale(50%, 50%);
  position: absolute;
  transform-origin: 50% 50% 0px;
  animation-name: rotate;
  animation-duration: 2s;
  animation-iteration-count: infinite;
  transition-timing-function: linear;
}
@keyframes rotate {
  0% {
    -webkit-transform: rotate(0deg);
  }
  40% {
    -webkit-transform: rotate(90deg);
  }
  100% {
    -webkit-transform: rotate(0deg);
  }
}
.div-portrait {
  position: fixed;
  height: 100vh;
  width: 100vw;
  z-index: 1000 !important;
  background: #000;
  display: none;
  //display: flex;
  align-content: center;
  justify-content: center;
  align-items: center;
}

@media only screen and (hover: none) and (pointer: coarse) and (orientation: portrait) {
  .div-portrait {
    display: block;
  }
}
.main_container {
  background: #024873 !important;
}
footer div {
  color: #000 !important;
}
</style>
<style src="../../public/template/vendors/bootstrap/dist/css/bootstrap.min.css"></style>
<style src="../../public/template/vendors/font-awesome/css/font-awesome.min.css"></style>
<style src="../../public/template/vendors/nprogress/nprogress.css"></style>
<style src="../../public/template/vendors/iCheck/skins/flat/green.css"></style>
<style src="../../public/template/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css"></style>
<style src="../../public/template/vendors/jqvmap/dist/jqvmap.min.css"></style>
<style src="../../public/template/vendors/bootstrap-daterangepicker/daterangepicker.css"></style>
<style src="../../public/template/build/css/custom.min.css"></style>