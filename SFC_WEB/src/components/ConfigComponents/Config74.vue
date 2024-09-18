<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>ETE CONFIG Config(74):</span>
      </div>
    </div>
    <div class="col-md-12">
        <div class="row">
            <div class="div-searchbox col-md-6">
            <div class="div-searchbox-content">
                <input
                v-on:keyup.enter="QuerySearch()"
                v-model="valueSearch"
                type="text"
                ref="input"
                class="form-control"
                @click="selectAll"
                :placeholder="
                    $store.state.language == 'En'
                    ? 'Enter model name...'
                    : 'Nhập tên hàng...'
                "
                />
                <button @click="QuerySearch()" class="btn">
                <Icon icon="search" class="sidenav-icon" />
                </button>
            </div>
            </div>

            
            <div class="div-searchbox col-md-6" style="justify-content: right;">
                <div class="div-searchbox-content">                    
                    <button @click="ToCheckLog()" class="btn" style=" border-radius: 10px 10px 10px 10px;">
                        CHECK_LOG
                    </button>
                </div>
            </div>
        </div>
    </div>
  <div class="row">
      <div class="col-md-12">
        <div class="main-contain">
          <template v-if="DataTableHeader">
            <table id="tableMain" class="table mytable">
              <thead>
                <tr>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                  </th>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Detail" : "Chi tiết" }}
                  </th>
                  <!-- <th style="width: 1px">
                    {{ $store.state.language == "En" ? "COPY" : "COPY" }}
                  </th> -->
                  <template
                    v-for="(item, index) in DataTableHeader"
                    :key="index"
                  >
                    <th v-if="item != 'ID'">
                      {{ item }}
                    </th>
                  </template>
                </tr>
              </thead>
              <template v-for="(item, index) in DataTable" :key="index">
                <tr>
                  <td class="td-delete" @click="DeleteRecord(item)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path fill="none" d="M0 0h24v24H0z" />
                      <path
                        fill="#FFF"
                        d="M17 6h5v2h-2v13a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V8H2V6h5V3a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v3zm1 2H6v12h12V8zm-9 3h2v6H9v-6zm4 0h2v6h-2v-6zM9 4v2h6V4H9z"
                      />
                    </svg>
                  </td>
                  <td class="td-edit" @click="ShowDetail(item)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path fill="none" d="M0 0h24v24H0z" />
                      <path
                        fill="#FFF"
                        d="M16.757 3l-2 2H5v14h14V9.243l2-2V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h12.757zm3.728-.9L21.9 3.516l-9.192 9.192-1.412.003-.002-1.417L20.485 2.1z"
                      />
                    </svg>
                  </td>
                  <!-- <td class="td-copy" @click="AddnewCust(item)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path
                        d="M16,20H8a3,3,0,0,1-3-3V7A1,1,0,0,0,3,7V17a5,5,0,0,0,5,5h8a1,1,0,0,0,0-2ZM21,8.94a1.31,1.31,0,0,0-.06-.27l0-.09a1.07,1.07,0,0,0-.19-.28h0l-6-6h0a1.07,1.07,0,0,0-.28-.19l-.09,0L14.06,2H10A3,3,0,0,0,7,5V15a3,3,0,0,0,3,3h8a3,3,0,0,0,3-3V9S21,9,21,8.94ZM15,5.41,17.59,8H16a1,1,0,0,1-1-1ZM19,15a1,1,0,0,1-1,1H10a1,1,0,0,1-1-1V5a1,1,0,0,1,1-1h3V7a3,3,0,0,0,3,3h3Z"
                      />
                    </svg>
                  </td> -->
                  <template v-for="(item1, index1) in item" :key="index1">
                    <td v-if="index1 != 'ID'">{{ item1 }}</td>
                  </template>
                </tr>
              </template>
            </table>
          </template>
        </div>
      </div>
</div>

    <div class="div-button">
      <button
        class="btn btn-danger"
        type="submit"
        @click="LockData()"
        id="lock"
        title="Lock"
      >
        Lock
      </button>
      <button
        class="btn btn-primary"
        type="submit"
        @click="UnLockData()"
        id="unlock"
        title="UnLock"
      >
        UnLock
      </button>
      <button
        class="btn btn-success"
        type="submit"
        @click="SaveData()"
        title="Save"
      >
        Save
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


    <div class="div-bellow">
      <div class="form-row">
        <div class="col-md-3 mb-3" v-show="!show">
          <label for="validationDefault02">MODEL NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterModel"
            @update-selected-item="UpdateModelNameReceive"
            :textContent="model.MODEL_NAME"
            type="model"
            textPlaceHolder="Enter model name"
          />
        </div>
        <div class="col-md-3 mb-3" v-show="show">
          <label for="validationDefault01">MODEL NAME</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            v-model="model.MODEL_NAME"
            :readonly="isReadonly1 == true"
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">Group Name </label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model="model.GROUP_NAME"
            :readonly="isReadonly1 == true"
            required
          />
        </div>
        <div class="col-md-3 mb-3" v-show="!show">
          <label for="vaildationDefault03">Type</label>
          <!--<input
            type="text"
            class="form-control form-control-sm text-element"
            id="vaildationDefault03"
            v-model="model.TYPE"
            required
          />-->
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterType"
            @update-selected-item="UpdateTypeReceive"
            :textContent="model.TYPE"
            type="model"
            textPlaceHolder="Enter TYPE"
          />
        </div>
        <div class="col-md-3 mb-3" v-show="show">
          <label for="vaildationDefault03">Type</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            v-model="model.TYPE"
            :readonly="isReadonly1 == true"
            required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="vaildationDefault04">Condition %</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="vaildationDefault04"
            v-model="model.CONDITION"
            required
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
//import $ from 'jquery';
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
export default {
  components: {
    DropdownSearch,
  },
  data() {
    return {
      textContent: "",
      type_check: "",
      searchText: "",
      valueSearchDetail:"",
      selectedItem: null,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      DataTableRoleName: [],
      columnName: [],
      columnNamebom: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      show : false,
      isReadonly: false,
      isReadonly1: false,
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME: "",
        MODEL_CODE: "",
        MO_NUMBER: "",
        GROUP_NAME: "",
        FA_NUMBER: "",
        FA_VERSION: "",
        TYPE: "",
        CONDITION: "",
        STATUS: "",
        REASON: "",
        DATA: "",
        ACTION_TYPE: "INSERT",
        SOLUTION: "",
      },
      ListModel: [],
      ListType: [],
      
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
    filterModel: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListModel;
      if (this.searchText === "") {
        return this.ListModel;
      }
      return this.ListModel.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
    filterType: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListType;
      if (this.searchText === "") {
        return this.ListType;
      }
      return this.ListType.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },
  mounted() {
    this.CheckPrivilege();
    //this.CheckPrivilege_();
    this.GetDataType();
    this.GetModel();
  },
  methods: {
    GotoRoute(route) {
      this.$router.push({ path: route });
    },
    UpdateModelNameReceive(value) {
      this.model.MODEL_NAME = value;

    },
    UpdateTypeReceive(value) {
      this.model.TYPE = value;
    },
    onChange(event) {
      //var optionText = event.target.value;
      this.model.STATUS = event.target.value;
      this.status = event.target.value;
      console.log(this.model.STATUS);
    },
    UpdateModelReceive(value) {
      this.model.MO_NUMBER = value;
      this.model.GROUP_NAME = "";
      this.isReadonly = false;

      // get TA_verion;
      //this.GetTA_Ver();
      // get FA_number;
      //this.GetFA_INFOR();
    },

    async CheckPrivilege_(fun, prg_name) {
      debugger;
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: fun,
        prg_name: prg_name,
      };
      var { data } = await Repository.getRepo("CheckPrivilege", payload);
      if (data.result != "ok") {
        this.show = false;
        this.isReadonly = false;
      } else {
        this.show = false;
        this.isReadonly = true;
      }
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config6GetAllModel", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },
    async GetDataType() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetTypeConfig74", payload);
      data.data.forEach((element) => {
        this.ListType.push(element.TYPE);
      });
    },
    async UnLockData() {
      if (
        this.model.MODEL_NAME == "" ||
        this.model.GROUP_NAME == "" ||
        this.model.TYPE == ""
      ) {
        if (localStorage.language == "En") {
          this.$swal(
            "",
            "Empty fields MODEL_NAME, GROUP_NAME , TYPE  ",
            "error"
          );
        } else {
          this.$swal(
            "",
            "Không được bỏ trống MODEL_NAME, GROUP_NAME , TYPE",
            "error"
          );
        }
        return;
      }

      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure edit?";
        textValue =
          "Once OK, data will be updated! Please input reason UnLock ";
      } else {
        titleValue = "Chắc chắn sửa?";
        textValue = "Dữ liệu sẽ được cập nhật! Nhập lý do UnLock";
      }
      this.$swal({
        title: textValue,
        text: titleValue,
        icon: "warning",
        html: `
          <input id="swal-input1" class="swal2-input" style="width:80% !important;" placeholder="Input Reason">
          <input id="swal-input2" class="swal2-input" style="width:80% !important" placeholder="Input Solution">
        `,
        buttons: true,
        dangerMode: true,
        preConfirm: () => {
          return [
            document.getElementById('swal-input1').value,
            document.getElementById('swal-input2').value
          ]
        }
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        if (willDelete.value[0] == "" || willDelete.value[1] == "") {
          this.$swal("", "Không được bỏ trống", "error");
          return;
        }
        this.model.REASON = willDelete.value[0];
        this.model.SOLUTION = willDelete.value[1];
        this.model.ACTION_TYPE = "UNLOCK";
        //begin
        var { data } = await Repository.getRepo(
          "InsertOrUpdateConfig74",
          this.model
        );
        if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege UnLock", "error");
          } else {
            this.$swal("", "Bạn không có quyền UnLock", "error");
          }
        } else if (data.result == "ok") {
          await this.QuerySearch();
          this.ClearForm();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
        //end
      });
    },
    async LockData() {
      if (
        this.model.MODEL_NAME == "" ||
        this.model.GROUP_NAME == "" ||
        this.model.TYPE == ""
      ) {
        if (localStorage.language == "En") {
          this.$swal(
            "",
            "Empty fields MODEL_NAME, GROUP_NAME , TYPE  ",
            "error"
          );
        } else {
          this.$swal(
            "",
            "Không được bỏ trống MODEL_NAME, GROUP_NAME , TYPE",
            "error"
          );
        }
        return;
      }

      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure edit?";
        textValue = "Once OK, data will be updated! Please input reason Lock ";
      } else {
        titleValue = "Chắc chắn sửa?";
        textValue = "Dữ liệu sẽ được cập nhật! Nhập lý do Lock";
      }
      this.$swal({
        title: titleValue,
        text: textValue,
        icon: "warning",
        input: "text",
        buttons: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        if (willDelete.value == "") {
          this.$swal("", "Phải nhập lý do Lock", "error");
          return;
        }
        this.model.REASON = willDelete.value;
        this.model.ACTION_TYPE = "LOCK";
        //begin
        var { data } = await Repository.getRepo(
          "InsertOrUpdateConfig74",
          this.model
        );
        if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege Lock", "error");
          } else {
            this.$swal("", "Bạn không có quyền Lock", "error");
          }
        } else if (data.result == "ok") {
          await this.QuerySearch();
          this.ClearForm();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
        //end
      });
    },
    async SaveData() {
      if (
        this.model.MODEL_NAME == "" ||
        this.model.GROUP_NAME == "" ||
        this.model.TYPE == ""
      ) {
        if (localStorage.language == "En") {
          this.$swal(
            "",
            "Empty fields MODEL_NAME, GROUP_NAME , TYPE  ",
            "error"
          );
        } else {
          this.$swal(
            "",
            "Không được bỏ trống MODEL_NAME, GROUP_NAME , TYPE",
            "error"
          );
        }
      } else {
        var titleValue = "";
        var textValue = "";
        if (localStorage.language == "En") {
          titleValue = "Are you sure edit?";
          textValue = "Once OK, data will be updated!";
        } else {
          titleValue = "Chắc chắn sửa?";
          textValue = "Dữ liệu sẽ được cập nhật";
        }
        this.$swal({
          title: titleValue,
          text: textValue,
          icon: "warning",
          buttons: true,
          dangerMode: true,
        }).then(async (willDelete) => {
          if (willDelete.isConfirmed == false) return;
          //begin
          var { data } = await Repository.getRepo(
            "InsertOrUpdateConfig74",
            this.model
          );
          if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege insert update", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else if (data.result == "ok") {
            await this.QuerySearch();
            this.ClearForm();
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
          } else {
            this.$swal("", data.result, "error");
          }
          //end
        });
      }
    },
    DeleteRecord(item) {
      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure?";
        textValue =
          "Once deleted, you will not be able to recover this record!";
      } else {
        titleValue = "Chắc chắn xóa?";
        textValue = "Sau khi xóa sẽ không thể khôi phục!";
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
          database_name: localStorage.databaseName,
          EMP: localStorage.username,
          MODEL_NAME: item.MODEL_NAME,
          GROUP_NAME: item.GROUP_NAME,
          TYPE: item.TYPE,
          ACTION_TYPE: "DELETE",
        };
        var { data } = await Repository.getRepo(
          "InsertOrUpdateConfig74",
          payload
        );
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege delete", "error");
          } else {
            this.$swal("", "Bạn không có quyền xóa", "error");
          }
          
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },
    ClearForm() {
      this.model.MODEL_NAME = "";
      this.model.GROUP_NAME = "";
      this.model.TYPE = "";
      this.model.CONDITION = "";
      this.DataTableFAHeader = [];
      this.DatatableFA = [];
      this.show = false;
      this.isReadonly = false;
      this.isReadonly1 = false;
      this.model.ACTION_TYPE = "INSERT";
      document.getElementById("lock").style.visibility = "visible";
      //document.getElementById("unlock").style.visibility="visible";
      /* if (typeof this.DataTableFA != "undefined") {
        if (this.DataTableFA.length != 0) {
          this.DataTableFAHeader = Object.keys(this.DataTableFA[0]);
          this.DataTableFAHeader.forEach((element) => {
            this.columnName.push(
              {
              label: element,
              field: element,
            });
          });
        }
      }*/

    },
    ShowDetail(detail) {
      this.isReadonly1 = true;
      this.model.MODEL_NAME = detail.MODEL_NAME;
      this.model.GROUP_NAME = detail.GROUP_NAME;
      this.model.TYPE = detail.TYPE;
      this.model.CONDITION = detail.CONDITION;
      this.show = true;
      this.model.ACTION_TYPE = "UPDATE";
      if (detail.STATUS == "LOCK") {
        document.getElementById("lock").style.visibility = "hidden";
        document.getElementById("unlock").style.visibility = "visible";
      } else {
        document.getElementById("unlock").style.visibility = "hidden";
        document.getElementById("lock").style.visibility = "visible";
      }
    },

    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    
    ToCheckLog() {
      this.$router.push({ path: "/Home/ConfigApp/Config74/CheckLog" });
    },

    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "ETE CONFIG",
        // prg_name :"CONFIG"
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        this.$router.push({ path: "/Home/ConfigApp" });
      } else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      this.CheckPrivilege_("ETE CONFIG", "CONFIG");
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig74Content", payload);
      this.DataTable = [];
      this.DataTable = data.data;      

      // console.log(data.data);
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
    async QuerySearch() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig74Content", payload);
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
.td-copy {
  text-align: center;
  justify-content: center;
  background: #84fa40;
  color: #fff;
  height: 30px;
  align-items: center;
  align-content: center;
  cursor: pointer;
  margin: 10px;
  &:hover {
    background: #0fdb08;
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
  height: auto;
  background: #1c87b5;
  color: #fff;
  padding: 15px;
  margin-right: 20px;
  div {
    div {
      div {
        margin-top: 10px;
        label {
          font-size: 13px;
          font-weight: bold;
          color: #9ff9c8;
          text-align: right;
        }
        input {
          border-radius: 5px;
        }
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
  overflow: scroll;
}
.maincontain {
  height: 300px;
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

.btn_select_item {
  font-size: 13px;
  margin-top: 10px;
  border-radius: 10px 10px 0 0;
  background-color: #024873;
  color: #fff;
  appearance: none;
  outline: none;
  border: none;
}
.div_select_item {
  height: 60px;
  ul {
    border-radius: 0 0 10px 10px;
    border: 1px solid #9e9e9e;
    padding: 0;
    width: 100%;
    height: 150px;

    overflow: auto;
    box-sizing: border-box;
    -moz-box-sizing: border-box;
    -webkit-box-sizing: border-box;
    background-color: #a0d0ff;
    li {
      background-color: #a0d0ff;
      width: 100%;
      font-weight: bold;
    }
    li:hover {
      background-color: #59aaf7;
      color: #fff;
    }
  }
}
#dropdowns
{
  z-index: 100;
  //  width: 220px;
}
#dropdowns2
{
z-index: 99 inherit; 
}
.li_selected {
  padding-left: 40px;
}
.lb_checked {
  color: #efef8d !important;
}
</style>