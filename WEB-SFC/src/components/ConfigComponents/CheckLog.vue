<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>CHECK LOG CONFIG(74):</span>
      </div>
    </div>
    <div class="col-md-12">
        <div class="div-searchbox">
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
    </div>
  <div class="row">
      <div class="col-md-12">
        <div class="main-contain">
          <template v-if="DataTableHeader">
            <table id="tableMain" class="table mytable">
              <thead>
                <tr>
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
    this.GetDataType();
    this.GetModel();
    this.LoadComponent();
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
        this.ListType.push(element.VR_NAME);
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
      this.$router.push({ path: "/Home/ConfigApp/Config74" });
    },


    async LoadComponent() {
      this.valueSearch = "";
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig74Content", payload);
      this.DataTable = [];
      this.DataTable = data.data1;      

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
      this.DataTable = data.data1;
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