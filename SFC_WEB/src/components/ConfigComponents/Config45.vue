<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>BUFFER TRIGGER (45):</span>
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
      </div>
    </div>
    <div class="main-contain">
      <div class="row col-sm-12 div-content">
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
                <template v-for="(item, index) in DataTableHeader" :key="index">
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
                  <td v-if="index1 != 'ID'">{{ item1 }}</td>
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
        <div class="col-md-3 mb-3" v-show="!show" > 
          <label for="validationDefault02">MODEL_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="ListModel"
            @update-selected-item="UpdateModelReceive"
            :textContent="model.MODEL_NAME"
            type="model"
            textPlaceHolder="Enter Model name"
          />
        </div>
        <div class="col-md-3 mb-3" v-show="show" >
          <label for="validationDefault02"> MODEL_NAME</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.MODEL_NAME"
            :readonly="isReadonly==true"
            required
          />
        </div>
        <div class="col-md-3 mb-3" >
          <label for="validationDefault02"> VERSION_CODE</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.VERSION_GROUP"
            :readonly="isReadonly==true"
            required
          />
        </div>
        <div class="col-md-3 mb-3" v-show="!show" >
          <label for="validationDefault02">PREGROUP</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="ListPreGroup"
            @update-selected-item="UpdatePreGroupReceive"
            :textContent="model.PRE_GROUP"
            type="model"
            textPlaceHolder="Enter Pre group"
          />
        </div>
         <div class="col-md-3 mb-3" v-show="show" >
          <label for="validationDefault02"> PRE GROUP</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.PRE_GROUP"
            :readonly="isReadonly==true"
            required
          />
        </div>
        <div class="col-md-3 mb-3"  v-show="!show" >
          <label for="validationDefault02">NEXT_GROUP</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="ListNextGroup"
            @update-selected-item="UpdateNextGroupReceive"
            :textContent="model.NEXT_GROUP"
            type="model"
            textPlaceHolder="Enter Next group"
          />
        </div>
        <div class="col-md-3 mb-3" v-show="show" >
          <label for="validationDefault02"> NEXT_GROUP</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.NEXT_GROUP"
            :readonly="isReadonly==true"
            required
          />
        </div>
      </div>
      <!-- row 2 -->
      <div class="form-row">
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">BUFFER_QTY</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.BUFFER_QTY"
            
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
      ListModel: [],
      ListPreGroup:[],
      ListNextGroup :[],
      isReadonly :false,
      show: false,
      model: {
       
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME: "",
        VERSION_GROUP: "",
        PRE_GROUP :"",
        NEXT_GROUP :"",
        BUFFER_QTY : "",
        ACTION_TYPE:"INSERT",

        
      },
      listChecked: [],
      ListCustomer: [],
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
    this.CheckPrivilege();
    this.GetModel();
    this.GetPreGroup();
    this.GetNextGroup();
    this.GetCustomer();
  },
  methods: {
    UpdatePreGroupReceive(value) {
   
      this.model.PRE_GROUP = value;
    },

    UpdateModelReceive(value) {
      this.model.MODEL_NAME = value;
     
    },
    UpdateNextGroupReceive(value) {
      this.model.NEXT_GROUP = value;
     
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetallModel_CF45", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },

     async GetPreGroup() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetallPreGroupCF45", payload);
      data.data.forEach((element) => {
        this.ListPreGroup.push(element.PRE_GROUP);
      });

      
    },

     
    async GetNextGroup() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetallNextGroupCF45", payload);
      data.data.forEach((element) => {
        this.ListNextGroup.push(element.NEXT_GROUP);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    async GetCustomer() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config19GetCustomer", payload);
      this.ListCustomer = data.data;
    },
    ChangeModelName() {
      this.model.PALLET_LAB_NAME = `P_${this.model.MODEL_NAME}.LAB`;
      this.model.CARTON_LAB_NAME = `${this.model.MODEL_NAME}.LAB`;
    },
    async SaveData() {
      if (this.model.MODEL_NAME == "" || this.model.VERSION_GROUP == "" || this.model.NEXT_GROUP == ""
      || this.model.PRE_GROUP == "" ||  this.model.BUFFER_QTY == ""
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
            "InsertUpdateBufger_trigger",
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
         
          EMP: localStorage.username,

          MODEL_NAME: item.MODEL_NAME,
          VERSION_GROUP: item.VERSION_CODE,
          PRE_GROUP : item.PRE_GROUP,
          NEXT_GROUP : item.NEXT_GROUP,
          BUFFER_QTY : item.BUFFER_QTY,
          ACTION_TYPE : 'DELETE'
        };
        var { data } = await Repository.getRepo("InsertUpdateBufger_trigger", payload);
        if (data.result == "ok") {
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
    ClearForm() {
      this.show= false;
      this.isReadonly =false;
     this.model.MODEL_NAME= '';
     this.model.VERSION_GROUP ='';
     this.model.PRE_GROUP= '';
     this.model.NEXT_GROUP= '';
     this.model.BUFFER_QTY='';
     this.model.ACTION_TYPE='INSERT';
    },
    ShowDetail(detail) {
        this.isReadonly= true;
        this.show = true;
       this.model.MODEL_NAME= detail.MODEL_NAME ;
       this.model.PRE_GROUP= detail.PRE_GROUP;
       this.model.VERSION_GROUP = detail.VERSION_CODE;
       this.model.NEXT_GROUP= detail.NEXT_GROUP;
       this.model.BUFFER_QTY= detail.BUFFER_QTY;
       this.model.ACTION_TYPE='UPDATE';
     
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "BUFFER_TRIGGER",
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
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig45Content", payload);
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
        model_name: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig45Content", payload);
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
  height: 250px;
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