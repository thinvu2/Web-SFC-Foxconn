<template>
  <div class="app">
    <ModalLoad v-if="$store.state.isLoadingAxios" />
    <ModalShow
      :isShowModal="$store.state.isShowModal"
      @hideModal="hideModal()"
    />
    <ModalDualSelectModel
      :isShowModal="$store.state.isShowQMModel"
      @hideModal="hideModal()"
    />
    <ModalDualSelectMO
      :isShowModal="$store.state.isShowQMMO"
      @hideModal="hideModal()"
    />
    <ModalDualSelectLine
      :isShowModal="$store.state.isShowQMLine"
      @hideModal="hideModal()"
    />
    <ModalDualSelectGroup
      :isShowModal="$store.state.isShowQMGroup"
      @hideModal="hideModal()"
    />
    <ModelAddEditLock
      :isShowModelLock="$store.state.isShowModelLock"
      @hideModal="hideModal()"
    />
    <ModelEmpPassword @hideModal="hideModal()" />
    <ModeLockUnlock @hideModal="hideModal()" />

    <ModalRouteQ6 @hideModal="hideModal()" />
    <ModalQueryListQuery6 @hideModal="hideModal()" />
    <ModalMixChart @hideModal="hideModal()" />
    <ModalPieChart @hideModal="hideModal()" />
    <ModalSystemLog @hideModal="hideModal()" />
    <router-view />
  </div>
</template>

<script>
import axios from "axios";
import ModalShow from "../src/components/Modal/ModalShow.vue";
import ModalDualSelectModel from "./components/Modal/ModalDualSelectModel.vue";
import ModalDualSelectMO from "./components/Modal/ModalDualSelectMO.vue";
import ModalDualSelectLine from "./components/Modal/ModalDualSelectLine.vue";
import ModalDualSelectGroup from "./components/Modal/ModalDualSelectGroup.vue";
import ModelAddEditLock from "./components/Modal/ModelAddEditLock.vue";
import ModelEmpPassword from "./components/Modal/ModelEmpPassword.vue";
import ModeLockUnlock from "./components/Modal/ModeLockUnlock.vue";
import ModalRouteQ6 from "./components/Modal/ModalRouteQ6.vue";
import ModalQueryListQuery6 from "./components/Modal/ModalQueryListQuery6.vue";
import ModalMixChart from "./components/Modal/ModalMixChart.vue";
import ModalPieChart from "./components/Modal/ModalPieChart.vue";
import ModalSystemLog from "./components/Modal/ModalSystemLog.vue";
import ModalLoad from "./components/Modal/ModalLoad.vue";

import $ from "jquery";
export default {
  data() {
    return {};
  },
  components: {
    ModalShow,
    ModalDualSelectModel,
    ModalDualSelectMO,
    ModalDualSelectLine,
    ModalDualSelectGroup,
    ModelAddEditLock,
    ModelEmpPassword,
    ModeLockUnlock,
    ModalRouteQ6,
    ModalQueryListQuery6,
    ModalMixChart,
    ModalPieChart,
    ModalSystemLog,
    ModalLoad,
  },
  methods: {
    hideModal() {
      this.$store.commit("closeModal");
      $("body").removeClass("modal-open");
    },
    showLoad() {
      this.$store.commit("loadAxios");
      $(window).scrollTop(0);
      $("body").addClass("modal-open");
      $("body").addClass("position-fixed");
    },
    hideLoad() {
      this.$store.commit("unloadAxios");
      $("body").removeClass("modal-open");
    },
  },
  created() {
    const vm = this;
    function showLoad() {
      vm.$store.commit("loadAxios");
    }
    function hideLoad() {
      vm.$store.commit("unloadAxios");
    }
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
    axios.interceptors.request.use(
      function (config) {
        showLoad();
        return config;
      },
      function (error) {
        hideLoad();
        return Promise.reject(error);
      }
    );

    axios.interceptors.response.use(
      function (response) {
        hideLoad();
        return response;
      },
      function (error) {
        hideLoad();
        return Promise.reject(error);
      }
    );
  },
};
</script>

<style>
.fixed-button {
  display: none;
}
</style>
