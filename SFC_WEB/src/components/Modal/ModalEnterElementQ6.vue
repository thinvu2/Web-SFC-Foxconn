<template>
  <div class="content">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowQ6Element"
        @click="hideModal()"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowQ6Element">
        <div class="div-content" style="text-align: center">
          <span style="color: #000; font-size: 16px; font-weight: 555">{{
            $store.state.language == "En"
              ? $store.state.Q6ElementNameEn
              : $store.state.Q6ElementNameVi
          }}</span>
        </div>
        <div class="div-content">
          <input
            v-on:keyup.enter="SearchElement()"
            autofocus
            autocomplete="off"
            v-model="$store.state.lock_emp_pass"    
            type="text"
            style="border: 1px solid #4e7da6"
            class="form-control form-control-sm"
            ref="inputText"
            id="textSearchInput"
          />
        </div>
        <div class="div-content" style="text-align: center">
          <button @click="SearchElement()" class="btnSave btn">
            <Icon icon="search" class="sidenav-icon" />
          </button>
        </div>

        <div class="div-content" style="text-align: center">
          <span style="color: red">{{ text_alert }}</span>
        </div>
      </div>
    </transition>
  </div>
</template>
<script>
import Repository from "../../services/Repository";

import $ from "jquery";
export default {
  props: ["receiveState"],
  watch: {
    receiveState: function () {
      $(document).ready(function () {
        $("#textSearchInput").focus();
      });      
    },
  },
  components: {},
  data() {
    return {
      showModal: false,
      isDisableSection: false,
      modelSearch: "",
      modelList: [],
      modelShow: false,
      emp_pass: "",
      model_name: "",
      group_name: "",
      type: "",
      value: "",
      text_alert: "",
    };
  },
  created() {
    this.type = "COUNT";
  },
  activated() {
    console.log("updated");
  },
  mounted() {},
  methods: {
    async SearchElement() {
      if (this.$store.state.lock_emp_pass == 0) {
        if (this.$store.state.language == "En") {
          this.text_alert = "Have field empty";
        } else {
          this.text_alert = "Không được bỏ trống";
        }
      } else {
        
        this.$store.state.lock_emp_pass=this.$store.state.lock_emp_pass.replace(/[\u200B-\u200D\uFEFF]/g,'');
        var payload = {
          database: localStorage.databaseName,
          option: this.$store.state.elementQ6,
          value_input: this.$store.state.lock_emp_pass,
        };
        var { data } = await Repository.getRepo("OptionQuery", payload);
        this.hideModal();
        if (data.result == "ok") {
          this.$store.state.listDetailClickAll = data.data;
          this.$store.state.listDetailClick =
            this.$store.state.listDetailClickAll.slice(0, 500);
          if (data.data.length > 0) {
            this.$store.commit("showModal");
            var arr = [];
            Object.keys(this.$store.state.listDetailClick[0]).forEach(
              (element) => {
                arr.push({
                  label: element,
                  field: element,
                });
              }
            );
            this.$store.state.listDetailClickHeader = arr;
          }
        } else {
          console.log("fail");
        }
        this.text_alert = "";
      }
    },
    changeEmpPass() {
      console.log(this.$store.state.lock_emp_pass);
    },
    hideModal() {
      // var add = this;
      // add.$refs.searchbar.$el.focus();
      this.$store.state.isShowQ6Element = false;
      this.$store.state.lock_emp_pass = "";
    },
    modelClickInput() {
      console.log("modelclick");
      if (!this.modelShow) {
        this.modelShow = true;
        this.clickSearch();
      }
    },
    async searchChangeModel(value) {
      this.$store.commit("changeLockModelName", value);
      var payload = {
        database_name: localStorage.databaseName,
        value: this.$store.state.lock_model_name,
      };
      var { data } = await Repository.getRepo("GetLockModel", payload);
      if (data.result == "ok") {
        this.modelList = data.data;
        if (!this.modelShow) {
          this.modelShow = true;
        }
      } else {
        console.log(`no data`);
        this.modelList = [];
        this.moShow = false;
      }
    },
    selectKeyModel(type) {
      this.$store.commit("changeLockModelName", type);
      this.modelShow = false;
    },
    async clickSearch() {
      var payload = {
        database_name: localStorage.databaseName,
        value: this.modelSearch,
      };
      var { data } = await Repository.getRepo("GetLockModel", payload);
      if (data.result == "ok") {
        this.modelList = data.data;
        this.modelShow = true;
      } else {
        console.log(`no data`);
        this.modelList = [];
        this.moShow = false;
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.btnExport {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;
}
.div-content {
  margin-top: 10px;
  span {
    font-size: 14px;
    font-weight: 55 0;
  }
}
.btnSave {
  border-radius: 10%;
  padding: 5px 20px;
  color: #fff;
  background-color: #f88838;
  text-align: center;
  align-self: center;
  justify-self: center;
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
.div-table {
  width: 92vw;
  height: 80vh;
}
thead {
  background: #418bca;
  tr {
    th {
      color: #fff;
    }
  }
}
.table-condensed {
  font-size: 12px;
  margin-top: 10px;
}
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
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

  width: 100%;
  max-width: 20vw;
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
