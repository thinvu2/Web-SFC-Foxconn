<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config PACK SCAN STEP (43):</span>
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
              : 'Nhập tên hàng...'
          "
        />
        <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
    </div>

    <div class="col-md-12">
      <div class="col-md-6">
        <div class="main-contain">
          <template v-if="DataTableHeader">
            <table id="tableMain" class="table mytable">
              <thead>
                <tr>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                  </th>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Edit" : "Sửa" }}
                  </th>
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
                  <template v-for="(item1, index1) in item" :key="index1">
                    <td v-if="index1 != 'ID'"  @click="ShowBomDetail(item)">{{ item1 }}</td>
                  </template>
                </tr>
              </template>
            </table>
          </template>
        </div>
      </div>
      <div class="col-md-2"></div>
      <div class="col-md-4">
        <template v-if="DataTableHeaderDetail">
          <table class="table mytable">
            <thead>
              <tr>
                <template
                  v-for="(item2, index2) in DataTableHeaderDetail"
                  :key="index2"
                >
                  <th>
                    {{ item2 }}
                  </th>
                </template>
              </tr>
              <template
                v-for="(item3, index3) in DataTableDetail"
                :key="index3"
              >
                <tr>
                  <template v-for="(item4, index4) in item3" :key="index4">
                    <td v-if="index4 != 'ID'">{{ item4 }}</td>
                  </template>
                </tr>
              </template>
            </thead>
          </table>
        </template>
      </div>
    </div>

    <div class="div-button col-md-12">
      
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

    <div class="div-bellow col-md-12">
      <div class="col-md-8">
        <div >
          <div class="col-md-4 mb-4" v-show="!show">
            <label for="validationDefault03">MODEL NAME</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filterModel"
              @update-selected-item="UpdateModelNameReceive"
              :textContent="model.MODEL_NAME"
              type="model"
              textPlaceHolder="Enter model name"
            />
          </div>
          <div class="col-md-4 mb-4"  v-show="show">
            <label for="validationDefault03" v-show="show">MODEL NAME</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              v-model="model.MODEL_NAME"
              required
              readonly
            />
          </div>

          <div class="col-md-4 mb-4" v-show="!show">
            <label for="validationDefault04">VERSION CODE</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              v-model="model.VERSION_CODE"
              required
            />
          </div>
          <div class="col-md-4 mb-4" v-show="show">
            <label for="validationDefault04">VERSION CODE</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              v-model="model.VERSION_CODE"
              readonly
              required
            />
          </div>
          <div class="col-md-4 mb-4" v-show="!show">
            <label for="validationDefault05" >MO TYPE</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filterMoType"
              @update-selected-item="UpdateMoTypeReceive"
              :textContent="model.MO_TYPE"
              type="model"
              textPlaceHolder="Enter mo type"
            />
          </div>
          <div class="col-md-4 mb-4" v-show="show">
            <label for="validationDefault04">MO TYPE</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              v-model="model.MO_TYPE"
              readonly
              required
            />
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="column col-md-8">
          <button @click="groupClick()" class="col btn_select_item">
            CUSTSN NAME
          </button>
          <div class="div_select_item">
            <ul>
              <smooth-scrollbar>
                <template
                  v-if="$store.state.listSelectDualModel.length == 0"
                >
                  <li style="text-align: center"></li>
                </template>
                <template v-else>
                  <li
                    class="li_selected"
                    v-for="(item, index) in $store.state
                      .listSelectDualModel"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </template>
              </smooth-scrollbar>
            </ul>
          </div>
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
      DataTableHeaderDetail: [],
      DataTableHeader: [],
      DataTable: [],
      DataTableDetail: [],
      columnName: [],
      columnNamebom: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      show : false,
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME: "",
        VERSION_CODE: "",
        MO_TYPE: "",
        SCAN_SEQ: 0,
        CUSTSN_NAME: "",
      },
      listChecked: [],
      ListRoute: [],
      ListKeyPartNo: [],
      ListModel: [],
      ListGroupName: [],
      ListMoType: [],
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
    filterMoType: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListMoType;
      if (this.searchText === "") {
        return this.ListMoType;
      }
      return this.ListMoType.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },
  mounted() {
    this.CheckPrivilege();
    this.GetModel();
    this.GetMoType();
    //this.checkDisable();
  },
  methods: {
    
    
    UpdateModelNameReceive(value) {
      this.model.MODEL_NAME = value;
    },
    UpdateMoTypeReceive(value){
      this.model.MO_TYPE=value;
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
    async GetMoType(){
      var payload= {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetMoTypeR105", payload);
      data.data.forEach((element) => {
        this.ListMoType.push(element.MO_TYPE);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    groupClick() {
     this.GetGroupConfig43("UpdateListSelectModel", "showModalModel");
    },
    
    async SaveData() {
      if (this.type == "add") {
        if (this.model.BOM_NAME == "") {
          if (localStorage.language == "En") {
            this.$swal("", "Empty fields", "error");
          } else {
            this.$swal("", "Không được bỏ trống", "error");
          }
          return;
        }
      }

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
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          ID: this.model.ID,
          EMP: localStorage.username,
          MODEL_NAME:this.model.MODEL_NAME,
          VERSION_CODE:this.model.VERSION_CODE,
          MO_TYPE:this.model.MO_TYPE,
          listCUSTSN_NAME:this.$store.state.listSelectDualModel,
        };
        var { data } = await Repository.getRepo(
          "InsertUpdateConfig43",
          payload
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
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          EMP:localStorage.username,
          MODEL_NAME:item.MODEL_NAME,
          VERSION_CODE:item.VERSION_CODE,
          MO_TYPE:item.MO_TYPE,
        };
        var { data } = await Repository.getRepo("DeleteConfig43", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
          this.ClearForm();
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
    },
    ClearForm() {
      this.type = "edit";
      this.show=false;
      this.model.ID = "";
      this.model.MODEL_NAME = "";
      this.model.VERSION_CODE = "";
      this.model.MO_TYPE = "";
      this.$store.state.listSelectDualModel=[];
    },
    ShowDetail(detail) {

      this.model.ID = detail.ID;
      this.model.MODEL_NAME = detail.MODEL_NAME;
      this.model.VERSION_CODE = detail.VERSION_CODE;
      this.model.MO_TYPE = detail.MO_TYPE;
      this.show=true;
      this.ShowBomDetail(detail);
      
    },
    async ShowBomDetail(detail) {
      debugger;
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME: detail.MODEL_NAME,
        VERSION_CODE: detail.VERSION_CODE,
        MO_TYPE: detail.MO_TYPE,
      };

      var { data } = await Repository.getRepo("GetConfig43DataDetail", payload);

      this.DataTableDetail = [];
      this.DataTableDetail = data.data;

      if (typeof this.DataTableDetail != "undefined") {
        if (this.DataTableDetail.length != 0) {
          this.DataTableHeaderDetail = Object.keys(this.DataTableDetail[0]);
          this.DataTableHeaderDetail.forEach((element) => {
            this.columnNamebom.push({
              label: element,
              field: element,
            });
          });
        }
      }
      this.$store.state.listSelectDualModel=data.data1;
    },
    async GetGroupConfig43( listType, modalType) {
      
      var payload = {
        database_name: localStorage.databaseName,
        value: this.textSearch,
      };
      var { data } = await Repository.getRepo("GetCustSnName", payload);
      this.$store.commit(listType, data.data);
      this.$store.commit(modalType);
      this.$store.commit(modalType);


    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "PACK SCAN STEP",
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
      this.$store.state.listSelectDualModel=[];
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig43Content", payload);
      this.DataTable = [];
      this.DataTableHeaderDetail = [];
      this.DataTable = data.data;
      this.DataTableHeaderDetail = data.data1;
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
      var { data } = await Repository.getRepo("GetConfig43Content", payload);
      this.DataTable = [];
      this.DataTableDetail = [];
      this.DataTable = data.data;
      this.DataTableDetail = data.data1;
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
  height: 200px;
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
      input {
        border-radius: 5px;
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
.li_selected {
  padding-left: 40px;
}
.lb_checked {
  color: #efef8d !important;
}
</style>