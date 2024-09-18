<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config Param (88):</span>
      </div>
    </div>
    <div class="div-searchbox row">
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
              : 'Nhập tên model...'
          "
        />
        <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button>
       

        <button @click="GotoRoute('/Home/ConfigApp/Config88_setup')" class="button btn btn-success">
          {{ $store.state.language == "En" ? "Setup Config" : "Setup Config" }}
        </button>

        <button @click="GotoRoute('/Home/ConfigApp/Config88_combine')" class="button btn btn-success">
          {{ $store.state.language == "En" ? "Combine Config" : "Setup Config" }}
        </button>
        
      </div>
    </div>
    <div class="main-contain">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
               
                <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Edit" : "Sửa" }}
                </th>
                <template v-for="(item, index) in DataTableHeader" :key="index">
                  <th 
                  >
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <template v-for="(item, index) in DataTable" :key="index">
              <tr>
               
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
                <template v-for="(item1, index1) in item" :key="index1">
                 <!-- <td v-if="index1 != 'ID'">{{ item1 }}</td>!--> 
                  <td >{{ item1 }}</td>
                </template>
              </tr>
            </template>
          </table>
        </template>
      </div>
    </div>
    <div class="div-button">
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
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">ID</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="50"
            v-model="model.ID"

            required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">TABLE_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterModel"
            @update-selected-item="UpdateModelNameReceive"
            :textContent="model.TABLE_NAME"
            
            type="model"
            textPlaceHolder="Enter table_name"
             required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">PK_COLUMN</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterColumn"
            @update-selected-item="UpdateColumnReceive"
            :textContent="model.PK_COLUMN"
            type="model"
            textPlaceHolder="Enter Column Name"
             required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">TYPE</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="5"
            v-model="model.TYPE"
            required
          />
        </div>
      </div>
      <!-- row 2 -->
      <div class="form-row">

        <div class="col-md-3 mb-3">
          <label for="validationDefault01">DATA1</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="50"
            v-model="model.DATA1"
            required
          />
        </div>
      
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">DATA2</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            :maxlength="100"
            v-model="model.DATA2"
            
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">DATA3</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            :maxlength="200"
            v-model="model.DATA3"
          
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault01">DATA4</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
             :maxlength="300"
            v-model="model.DATA4"
          
          />
        </div>
       
      </div>

      <!--row 3!-->
      <div class="form-row">
        <div class="col-md-3 mb-3">
      
              <label for="validationDefault01">DATA5</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
             :maxlength="500"
            v-model="model.DATA5"
          
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
      model: {
        ID: "",
        TABLE_NAME: "",
        EMP: localStorage.username,
        database_name: localStorage.databaseName,
        PK_COLUMN: "",
        TYPE: "",
        DATA1: "",
        DATA2: "",
        DATA3: "",
        DATA4: "",
        DATA5: "",
      },
      ListModel: [],
      listChecked: [],
      ListRoute: [],
      ListInputGroup: [],
      ListBom: [],
      ListOutputGroup: [],
      ListModelSerial: [],
      ListColumn : []
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

     filterColumn: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListColumn;
      if (this.searchText === "") {
        return this.ListColumn;
      }
      return this.ListColumn.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },
  mounted() {
    this.CheckPrivilege();
    this.GetModel();
  },
  methods: {

     GotoRoute(route) {
      this.$router.push({ path: route });
    },
    UpdateModelNameReceive(value) {

      this.model.TABLE_NAME = value;
      this.model.PK_COLUMN='';
      this.GetColumn();
     

    },

    UpdateColumnReceive(value) {
      this.model.PK_COLUMN = value;
     // this.GetColumn();
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config88_LoadTatble_Name", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.TABLE_NAME);
      });
    },
      async GetColumn() {
      var payload = {
        database_name: localStorage.databaseName,
        table_name : this.model.TABLE_NAME
      };
      var { data } = await Repository.getRepo("Config88_LoadColumn_Name", payload);
      this.ListColumn=[];
      data.data.forEach((element) => {
        this.ListColumn.push(element.COLUMN_NAME);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    ChangeModelName() {
      this.model.PALLET_LAB_NAME = `P_${this.model.MODEL_NAME}.LAB`;
      this.model.CARTON_LAB_NAME = `${this.model.MODEL_NAME}.LAB`;
    },
    async SaveData() {
      if (
        this.model.ID == "" ||
        this.model.TABLE_NAME == "" ||
        this.model.PK_COLUMN == "" || this.model.TYPE=="" || this.model.DATA1 ==""
      ) {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
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

          var { data } = await Repository.getRepo(
            "InsertUpdateConfig88_Param",
            this.model
          );
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
          } else {
            this.$swal("", data.result, "error");
          }
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
          ID: item.ID,
          EMP: localStorage.username,
          MODEL_NAME: item.MODEL_NAME,
          
        };
        var { data } = await Repository.getRepo("DeleteConfig88", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "Bạn không có quyền thêm sửa", "error");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
      this.line_name = "";
      this.line_type = "";
      this.line_code = "";
      this.line_desc = "";
    },
    ClearForm() {
      this.model.ID = "";
      this.model.TABLE_NAME = "";
      this.model.PK_COLUMN = "";
      this.model.TYPE = "";
      this.model.DATA1 = "";
      this.model.DATA2 = "";
      this.model.DATA3 = "";
      this.model.DATA4 = "";
      this.model.DATA5 = "";
     
    },
    ShowDetail(detail) {
      this.model.ID = detail.ID;
      this.model.TABLE_NAME = detail.TABLE_NAME;
      this.GetColumn();
      this.model.PK_COLUMN = detail.PK_COLUMN;
      this.model.TYPE = detail.TYPE;
      this.model.DATA1 = detail.DATA1;
      this.model.DATA2 = detail.DATA2;
      this.model.DATA3 = detail.DATA3;
      this.model.DATA4 = detail.DATA4;
      this.model.DATA5 = detail.DATA5;
    
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "PARAM CONFIG",
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        this.$router.push({ path: "/Home/ConfigApp" });
      } 
      else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig88Content", payload);
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
    async QuerySearch() {
      var payload = {
        database_name: localStorage.databaseName,
        ID: this.valueSearch
      };
      var { data } = await Repository.getRepo("GetConfig88Content", payload);
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
.lb_checked {
  color: #efef8d !important;
}

</style>