<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowLockUnLock"
        @click="hideModal()"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowLockUnLock">
        <div class="div-content" style="text-align: center"></div>
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
            $store.state.language == "En" ? "Reason Lock" : "Nguyên nhân"
          }}</span>
          <input
            v-model="$store.state.lock_reason"
            autocomplete="false"
            type="text"
            class="form-control form-control-sm"
          />
        </div>
        <div class="div-content" v-if="!$store.state.isLockStation">
          <span>{{
            $store.state.language == "En" ? "Solution" : "Giải pháp"
          }}</span>
          <input
            v-model="$store.state.lock_solution"
            autocomplete="false"
            type="text"
            class="form-control form-control-sm"
          />
        </div>
        <div class="div-content" style="text-align: center">
          <template v-if="$store.state.isLockStation">
            <button @click="LockStation()" class="btnSave btn btn-danger">
              {{ $store.state.language == "En" ? "Lock" : "Khóa" }}
            </button>
          </template>
          <template v-else>
            <button @click="UnlockStation()" class="btnSave btn btn-success">
              {{ $store.state.language == "En" ? "Unlock" : "Mở khóa" }}
            </button>
          </template>
        </div>

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

export default {
  props: {},
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
  methods: {
    async LockUnlock(type) {
      var payload = {
        database_name: localStorage.databaseName,
        model_name: this.$store.state.lock_model_name,
        group_name: this.$store.state.lock_group_name,
        condition: this.$store.state.lock_type,
        condition_value: this.$store.state.lock_condition_value,
        emp_pass: this.$store.state.lock_emp_pass,
        reason: this.$store.state.lock_reason,
        solution: this.$store.state.lock_solution,
        ip_address: "",
      };
      var { data } = await Repository.getRepo(type,payload);
      if (data.result == "privilege") {
        if (localStorage.language == "En") {
          this.$swal("", "Wrong password", "error");
        } else {
          this.$swal("", "Sai mật khẩu", "error");
        }
      } else if (data.result == "ok") {
        this.hideModal();
        if (type == "LockStation") {
          if (localStorage.language == "En") {
            this.$swal("", "Lock successfully ♥", "success");
          } else {
            this.$swal("", "Khóa trạm thành công ♥", "success");
          }
        } else if (type == "UnLockStation") {
          if (localStorage.language == "En") {
            this.$swal("", "Unlock successfully ♥", "success");
          } else {
            this.$swal("", "Mở khóa thành công ♥", "success");
          }
        }
      }
    },
    LockStation() {
      if (
        this.$store.state.lock_emp_pass == 0 ||
        this.$store.state.lock_reason == 0
      ) {
        if (this.$store.state.language == "En") {
          this.text_alert = "Have field empty";
        } else {
          this.text_alert = "Không được bỏ trống";
        }
      } else {
        this.text_alert = "";
        this.LockUnlock('LockStation');                
      }
    },
    UnlockStation() {
      if (
        this.$store.state.lock_emp_pass == 0 ||
        this.$store.state.lock_reason == 0 ||
        this.$store.state.lock_solution == 0
      ) {
        if (this.$store.state.language == "En") {
          this.text_alert = "Have field empty";
        } else {
          this.text_alert = "Không được bỏ trống";
        }
      } else {
        this.text_alert = "";
        this.LockUnlock('UnLockStation');                    
      }
    },
    changeEmpPass() {
      console.log(this.$store.state.lock_emp_pass);
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
      if (!this.modelShow) {
        this.modelShow = true;
        this.clickSearch();
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.div-content {
  margin-top: 10px;
  span {
    font-size: 13px;
    font-weight: 550;
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
