<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="isShowModelLock"
        @click="hideModal()"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="isShowModelLock">
        <div class="div-content">
          <span>{{
            $store.state.language == "En" ? "Employee password" : "Mật khẩu"
          }}</span>
          <input
            v-model="$store.state.lock_emp_pass"
            autocomplete="false"
            type="password"
            class="form-control form-control-sm"
          />
        </div>
        <div class="div-content">
          <span>{{
            $store.state.language == "En" ? "Model name" : "Tên hàng"
          }}</span>
          <!-- :keySearch="modelSearch" -->
          <SelectionSearch
            :isDisable="$store.state.isDisableLockElement"
            :keySearch="$store.state.lock_model_name"
            :list="modelList"
            :isShowSelect="modelShow"
            @clickInput="modelClickInput"
            @searchChange="searchChangeModel"
            @selectKey="selectKeyModel"
          />
          <!-- <input type="text" class="form-control form-control-sm" /> -->
        </div>
        <div class="div-content">
          <span>{{
            $store.state.language == "En" ? "Group name" : "Tên trạm"
          }}</span>
          <input
            :disabled="$store.state.isDisableLockElement"
            v-model="$store.state.lock_group_name"
            type="text"
            class="form-control form-control-sm"
          />
        </div>
        <div class="div-content">
          <span>{{ $store.state.language == "En" ? "Type" : "Loại" }}</span>
          <select
            :disabled="$store.state.isDisableLockElement"
            v-model="$store.state.lock_type"
            class="form-control form-control-sm"
            name=""
            id=""
          >
            <option value="COUNT" selected>COUNT</option>
            <option value="PERCENT">PERCENT</option>
            <option value="X_RAY">X_RAY</option>
            <option value="SAMEERROR">SAMEERROR</option>
            <option value="FINAL_YR">FINAL YR</option>
            <option value="RETEST_YR">RETEST YR</option>
          </select>
        </div>
        <div class="div-content">
          <span>{{ $store.state.language == "En" ? "Value" : "Giá trị" }}</span>
          <input
            v-model="$store.state.lock_condition_value"
            type="text"
            class="form-control form-control-sm"
          />
        </div>

        <template v-if="$store.state.is_lock_edit == 'EDIT'">
          <div style="text-align: center; margin-top: 20px">
            <button @click="EditLockConfig()" class="btnSave btn btn-success">
              {{ $store.state.language == "En" ? "Save" : "Sửa" }}
            </button>
          </div>
        </template>
        <template v-else>
          <div style="text-align: center; margin-top: 20px">
            <button @click="AddLockConfig()" class="btnSave btn btn-success">
              {{ $store.state.language == "En" ? "Create" : "Thêm" }}
            </button>
          </div>
        </template>
        <div class="div-content" style="text-align: center">
          <span style="color: red">{{ text_alert }}</span>
        </div>
        <!-- <button class="button" @click="$emit('hideModal')">Close Modal</button> -->
      </div>
    </transition>
  </div>
</template>
<script>
import Repository from "../../services/Repository";

import SelectionSearch from "../Template/SelectionSearch.vue";
export default {
  props: {
    isShowModelLock: Boolean,
    listDetail: Array,
  },
  components: { SelectionSearch },
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
  methods: {
    changeEmpPass() {
      console.log(this.$store.state.lock_emp_pass);
    },
    async AddLockConfig() {
      if (
        this.$store.state.lock_model_name == 0 ||
        this.$store.state.lock_group_name == 0 ||
        this.$store.state.lock_type == 0 ||
        this.$store.state.lock_condition_value == 0 ||
        this.$store.state.lock_emp_pass == 0
      ) {
        if (this.$store.state.language == "En") {
          this.text_alert = "Have field empty";
        } else {
          this.text_alert = "Không được bỏ trống";
        }
      } else {
        var payload = {
          database_name: localStorage.databaseName,
          model_name: this.$store.state.lock_model_name,
          group_name: this.$store.state.lock_group_name,
          condition: this.$store.state.lock_type,
          condition_value: this.$store.state.lock_condition_value,
          emp_pass: this.$store.state.lock_emp_pass,
          reason: "",
          solution: "",
          ip_address: "",
        };
        var { data } = await Repository.getRepo("LockInsert", payload);
        if (data.result == "ok") {
          this.hideModal();
          if (localStorage.language == "En") {
            this.$swal("", "Add successfully ♥", "success");
          } else {
            this.$swal("", "Thêm thành công ♥", "success");
          }
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Wrong password", "error");
          } else {
            this.$swal("", "Sai mật khẩu", "error");
          }
        } else if (data.result == "exist") {
          this.hideModal();
          if (localStorage.language == "En") {
            this.$swal("", "Record existed", "error");
          } else {
            this.$swal("", "Đã tồn tại", "error");
          }
        }

        this.text_alert = "";
      }
    },
    async EditLockConfig() {
      if (
        this.$store.state.lock_model_name == 0 ||
        this.$store.state.lock_group_name == 0 ||
        this.$store.state.lock_type == 0 ||
        this.$store.state.lock_condition_value == 0 ||
        this.$store.state.lock_emp_pass == 0
      ) {
        if (this.$store.state.language == "En") {
          this.text_alert = "Have field empty";
        } else {
          this.text_alert = "Không được bỏ trống";
        }
      } else {
        var payload = {
          database_name: localStorage.databaseName,
          model_name: this.$store.state.lock_model_name,
          group_name: this.$store.state.lock_group_name,
          condition: this.$store.state.lock_type,
          condition_value: this.$store.state.lock_condition_value,
          emp_pass: this.$store.state.lock_emp_pass,
          reason: "",
          solution: "",
          ip_address: "",
        };
        var { data } = await Repository.getRepo("LockUpdate", payload);
        if (data.result == "ok") {
          this.hideModal();
          if (localStorage.language == "En") {
            this.$swal("", "Update successfully ♥", "success");
          } else {
            this.$swal("", "Cập nhật thành công ♥", "success");
          }
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Wrong password", "error");
          } else {
            this.$swal("", "Sai mật khẩu", "error");
          }
        }

        this.text_alert = "";
      }
    },
    hideModal() {
      this.modelSearch = "";
      this.emp_pass = "";
      this.model_name = "";
      this.group_name = "";
      this.type = "COUNT";
      this.value = "";
      this.text_alert = "";
      this.modelShow = false;
      this.$emit("hideModal");
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
.div-content {
  margin-top: 10px;
  span {
    font-size: 14px;
    font-weight: 55 0;
  }
}
.btnSave {
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
