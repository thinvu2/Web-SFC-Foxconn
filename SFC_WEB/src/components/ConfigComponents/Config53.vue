<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>{{ $store.state.language == "En" ? "CHANGE PASSWORD" : "ĐỔI MẬT KHẨU" }}</span>
      </div>
    </div>
 
    <div class="div-bellow">
      <div class="form-row">


        <div class="col-md-4 mb-4">
          <label for="EMP_NO">{{ $store.state.language == "En" ? "Emp no" : "Mã thẻ nhân viên" }}</label>
          <input
            class="form-control form-control-sm text-element"
            id="EMP_NO"
            :value="model.EMP_NO"
            type="text"
            @input="updateEMP_NO"
            textPlaceHolder="Enter EMP_NO"  />
        </div>
        <div class="col-md-4 mb-4">
        </div>
        <div class="col-md-4 mb-4">
        </div>
        <div class="col-md-4 mb-4">
          <label for="OLD_PASS">{{ $store.state.language == "En" ? "Old password" : "Mật khẩu cũ" }}</label>
          <input
            class="form-control form-control-sm text-element"
            id="OLD_PASS"
            :value="model.OLD_PASS"
            type="password"
            @input="updateOLD_PASS"
            :disabled="isDisabled"
            textPlaceHolder="Enter OLD_PASS"  />
        </div>
        <div class="col-md-4 mb-4">
        </div>
        <div class="col-md-4 mb-4">
        </div>
        <div class="col-md-4 mb-4">
          <label for="NEW_PASS">{{ $store.state.language == "En" ? "New password" : "Mật khẩu mới" }}</label>
          <input
            class="form-control form-control-sm text-element"
            id="NEW_PASS"
            :value="model.NEW_PASS"
            type="password"
            @input="updateNEW_PASS"
            :disabled="isDisabled"
            textPlaceHolder="Enter NEW_PASS"  />
        </div>
        <div class="col-md-4 mb-4">
        </div>
        <div class="col-md-4 mb-4">
        </div>
        <div class="col-md-4 mb-4">
          <label for="CONFIRM_PASS">{{ $store.state.language == "En" ? "Confirm password" : "Nhập lại mật khẩu" }}</label>
          <input
            class="form-control form-control-sm text-element"
            id="CONFIRM_PASS"
            :value="model.CONFIRM_PASS"
            type="password"
            @input="updateCONFIRM_PASS"
            :disabled="isDisabled"
            textPlaceHolder="Enter CONFIRM_PASS"  />
        </div>



      </div>
    </div>

    <div class="div-button">
      <button
        class="btn btn-success"
        type="submit"
        @click="SaveData()"
        title="Save"
      >
        {{ $store.state.language == "En" ? "Save" : "Lưu" }}
      </button>
      <button
        class="btn btn-warning"
        type="button"
        @click="ClearForm()"
        title="Refresh form"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          width="24"
          height="24"
        >
          <path fill="none" d="M0 0h24v24H0z" />
          <path
            fill="red"
            d="M5.463 4.433A9.961 9.961 0 0 1 12 2c5.523 0 10 4.477 10 10 0 2.136-.67 4.116-1.81 5.74L17 12h3A8 8 0 0 0 6.46 6.228l-.997-1.795zm13.074 15.134A9.961 9.961 0 0 1 12 22C6.477 22 2 17.523 2 12c0-2.136.67-4.116 1.81-5.74L7 12H4a8 8 0 0 0 13.54 5.772l.997 1.795z"
          />
        </svg>
      </button>
    </div>
  </div>
</template>
<script>
//import $ from 'jquery';
import Repository from "../../services/Repository";
export default {
  components: {
  },
  data() {
    return {
      isDisabled:true,
      textContent: "",      
      version: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      columnName: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      ListMo_number: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        EMP_NO : "",
        EMP_NAME : "",
        EMP_RANK : "",
        CLASS_NAME : "",
        EMP_PASS : "",
        EMP_BC : "",
        OLD_PASS : "",
        NEW_PASS:"",
        CONFIRM_PASS:"",
      },     
      LIST_MO_NUMBER:[],
      listChecked: [],
      ListCustomer: [],
      Liststatus:[],
    };
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.isVisible = false;
      }
    });
  },
  computed: {
    filterUser: function () {
      const query = this.searchText.toLowerCase();
      if (this.searchText === "") {
        return this.ListModel;
      }
      return this.ListModel.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },
  mounted() {
    //this.CheckPrivilege();
  },
  methods: {
   
     updateEMP_NO(event) {
      this.model.EMP_NO = event.target.value;
        if(event.target.value.trim()=="" || event.target.value.trim() == null){
            this.isDisabled = true;
        }else{
            this.isDisabled = false;
        }
    },

     updateOLD_PASS(event) {
      this.model.OLD_PASS = event.target.value;
     },

     updateNEW_PASS(event) {
      this.model.NEW_PASS = event.target.value;
     },

     updateCONFIRM_PASS(event) {
      if(this.model.NEW_PASS == event.target.value){
        this.model.EMP_BC = this.model.NEW_PASS;
      }else{
        this.model.EMP_BC = "";

      }
     },
    


    SetTextDropDown(text) {
      this.textContent = text;
      this.value = text;
      this.isVisible = false;      
    },
    async SaveData() {
      if (this.model.EMP_NO == null || this.model.OLD_PASS == null || this.model.NEW_PASS == null) {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      }else if(this.model.EMP_BC == ""){
        if (localStorage.language == "En") {
          this.$swal("", "Password incorrect", "error");
        } else {
          this.$swal("", "Nhập lại mật khẩu không đúng!", "error");
        }
      } else {
        var titleValue = "";
        var textValue = "";
        if (localStorage.language == "En") {
          titleValue = "Are you sure save?";
          textValue = "Once OK, data will be save!";
        } else {
          titleValue = "Chắc chắn lưu?";
          textValue = "Dữ liệu sẽ được lưu";
        }
        this.$swal({
          title: titleValue,
          text: textValue,
          icon: "warning",
          buttons: true,
          dangerMode: true,
        }).then(async (willDelete) => {
          if (willDelete.isConfirmed == false) return;

          var payload = {
            EMP: localStorage.username,
            database_name: localStorage.databaseName,
            EMP_NO: this.model.EMP_NO,
            OLD_PASS: this.model.OLD_PASS,
            EMP_PASS: this.model.EMP_BC? this.model.EMP_BC:"",
            EMP_BC: this.model.EMP_BC? this.model.EMP_BC:"",        
          };

          var { data } = await Repository.getRepo("UpdateConfig53",payload);
          
          if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else if (data.result == "ok") {
            await this.QuerySearch();
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
          }else {
            this.$swal("", data.result, "error");
          }
        });
      }
    },

    ClearForm() {
        this.model.EMP_NO = "";
        this.model.OLD_PASS = "";
        this.model.NEW_PASS = "";
        this.model.CONFIRM_PASS = "";
    },

    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },

    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "AQL SETUP",        
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        if (localStorage.language == "En") {
          this.$swal("", "Not privilege", "error");
        } else {
          this.$swal("", "Bạn không có quyền thêm sửa", "error");
        }
        this.$router.push({ path: "/Home/ConfigApp" });        

      } else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig23Content", payload);
      this.DataTable = [];
      this.DataTable = data.data;
      if (typeof this.DataTable != "undefined") {
        if (this.DataTable.length != 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
          this.DataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
      }
    },

  },
};
</script>

<style lang="scss" scoped>
.dropdown-wrapper {
  max-width: 100%;
  position: relative;
  margin: 0 auto;
}

.text-element {
  color: #000;
  font-weight: 555;
}
.td-delete {
  display: flex;
  justify-content: center;
  background: #e73a23;
  cursor: pointer;
  height: 30px;
  align-items: center;
}
.td-edit {
  text-align: center;
  justify-content: center;
  background: #f88838;
  color: #fff;
  height: 30px;
  align-items: center;
  align-content: center;
  cursor: pointer;
  margin: 10px;
  &:hover {
    background: #db6008;
  }
}
.btn-warning {
  margin-left: 20px;
}
.btn-danger {
  margin-left: 20px;
  height: 40px;
}
.div-button {
  margin-top: 10px;
  position: relative;
  right: 50px;
  text-align: right;
}
.div-bellow {
  margin-top: 5px;
  background: #1c87b5;
  color: #fff;
  padding: 15px;
  margin-right: 20px;
  div {
    div {
      label {
        font-size: 13px;
        font-weight: bold;
        color: #9ff9c8;
      }
    }
  }
}
.div-all {
  margin-left: 35px;
}
.div-searchbox {
  margin-top: 15px;
  height: 60px;
  display: flex;
  align-content: center;
  justify-content: left;
  .div-searchbox-content {
    // position: absolute;
    display: flex;
    // bottom: 0;
    margin-bottom: 10px;
    // left: 50%;
    text-align: center;
    input {
      border-radius: 10px 0 0 10px;
      // padding: 0px 5px;
      width: 400px;
    }
    button {
      border-radius: 0 10px 10px 0;
      padding: 0 20px;
      background: #ff7a1c;
      color: #fff;
      box-sizing: 0;
      &:hover {
        background: #f88838;
      }
    }
  }
}
.div-config-name {
  margin-left: 20px;
  line-height: 50px;
  span {
    font-weight: 555;
    font-size: 17px;
  }
}
.main-contain {
  height: 350px;
  overflow: auto;
}
.div-back {
  float: left;
  background: #eae1e1;
  cursor: pointer;
  margin: 10px 0;
  display: flex;
  align-content: center;
  align-items: center;
  width: 3%;
  border-radius: 10%;
  &:hover {
    background: #b7b7b7;
  }
  .back-icon {
    height: 20px;
    width: 20px;
  }
}

.mytable {
  margin-top: 0px;
  overflow: auto;
  thead {
    th:first {
      border-radius: 20%;
    }
    th {
      background-color: #024873;
      position: sticky;
      top: 0;
      z-index: 2;
      color: #fff;
      min-width: 60px;
      padding: 3px;
      font-size: 13px;
      padding: 0.5rem 1.5rem;
    }
  }
  tr {
    &:hover {
      background: #89cfed;
    }
    td {
      overflow-x: auto;
      white-space: nowrap;
      z-index: 1;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 13px;
      font-weight: 555;
    }
  }
}
</style>