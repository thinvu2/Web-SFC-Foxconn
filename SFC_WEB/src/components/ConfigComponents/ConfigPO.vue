<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>CONFIG PO:</span>
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
                      d="M16.757 3l-2 2H5v14h14V9.243l2-2V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h12.757zm3.728-.9L21.9 3.516l-9.462 9.462-1.412.003-.002-1.417L20.485 2.1z"
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


        <!-- <div class="col-md-3 mb-3">
          <label for="validationDefault02">MODEL_NAME </label>
          <DropdownSearch
            class="form-control  text-element col-md-3"
            :listAll="ListModel"
            @update-selected-item="UpdateModelReceive"
            :textContent="model.MODEL_NAME"
            type="model"
            textPlaceHolder="Enter Model Name"
          />
        </div> -->
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">INVOICE</label>
          <input
            type="text"
            class="form-control text-element"
            id="validationDefault01"
            v-model="model.MODEL_NAME"            
          />
        </div>   

        <!-- <div class="col-md-3 mb-3">  
          <label for="validationDefault02">TYPE</label>
            <DropdownSearch class="form-control  text-element" 
            type="model"
            :textContent="model.TYPE"
            :listAll="ListATTRIBUTE_VALUE"
            textPlaceHolder="Enter TYPE"
            @update-selected-item="UpdateValueReceive" />
        </div>         -->
        <div class="col-md-3 mb-3">
          <label for="validationDefault01">TYPE</label>
          <input
            type="number"
            class="form-control text-element"
            id="validationDefault01"
            v-model="model.TYPE"            
          />
        </div>   
        <div class="col-md-3 mb-3">
          <label for="validationDefault01">PO_NO</label>
          <input
            type="text"
            class="form-control text-element"
            id="validationDefault01"
            v-model="model.PO_NO"            
          />
        </div>        
        <!-- <div class="col-md-3 mb-3">
          <label for="validationDefault01">TARGET_QTY</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model="model.QTY"            
          />
        </div> -->


      </div>
    </div>
  </div>
</template>
<script>
//import $ from 'jquery';
import Repository from "../../services/Repository";
//import DropdownSearch from "../Template/DropdownSearch.vue";
export default {
  components: {
    //DropdownSearch,
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
      ListModel: [],
      ListATTRIBUTE_VALUE:[],
      ListATTRIBUTE_DESC1:[],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME : "",
        TYPE : "",
        ATTRIBUTE_DESC1 : "",  
        PO_NO : "",
      },     
      LIST_MO_NUMBER:[],
      listChecked: [],
      ListCustomer: [],
      Liststatus:[],
      minvalue: 0,
      maxvalue: 9999,
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
    document.title = "PO";
    this.CheckPrivilege();
    this.GetModel();
    //this.LoadComponent();
  },
  methods: {
    UpdateModelReceive(value) {
        this.model.MODEL_NAME = value;
        this.model.TYPE = "";
        this.model.PO_NO = "";
        this.model.ID = "";     
    },

         

    UpdateValueReceive(value) {
      this.model.TYPE = value;     
    },  
    UpdateDescReceive(value) {
      this.model.ATTRIBUTE_DESC1 = value;     
    },  

    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("ConfigPOModel", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
      data.dtvalue.forEach((element) => {
        this.ListATTRIBUTE_VALUE.push(element.TYPE);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.value = text;
      this.isVisible = false;      
    },
    async SaveData() {
      if(this.model.TYPE < this.minvalue || this.model.TYPE > this.maxvalue){
        if (localStorage.language == "En") {
          this.$swal("", "TYPE invalid", "error");
        } else {
          this.$swal("", "Type không hợp lệ!", "error");
        }
        return;
      }

      if (this.model.MODEL_NAME == null || this.model.PO_NO == null || this.model.TYPE == null  ) {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
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
            ID: this.model.ID,
            EMP: localStorage.username,
            database_name: localStorage.databaseName,
            MODEL_NAME: this.model.MODEL_NAME,
            TYPE: this.model.TYPE,
            PO_NO: this.model.PO_NO,  
          };

          var { data } = await Repository.getRepo("InsertUpdatePO",payload);
          
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
          }else if (data.result == "notexist") {
            await this.QuerySearch();
            if (localStorage.language == "En") {
              this.$swal("", "Not Exist", "error");
            } else {
              this.$swal("", "Đã tồn tại dữ liệu", "error");
            }
          }else if (data.result == "notpass") {
            if (localStorage.language == "En") {
              this.$swal("", "Setup not privilege Confirm", "error");
            } else {
              this.$swal("", "Người thiết lập không có quyền xác nhận", "error");
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
          MODEL_NAME : item.MODEL_NAME,
            PO_NO: this.model.PO_NO,
            TYPE: this.model.TYPE,
        };
        var { data } = await Repository.getRepo("DeletePO", payload);
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
      this.line_name = "";
      this.line_type = "";
      this.line_code = "";
      this.line_desc = "";
    },
    ClearForm() {
        this.model.MODEL_NAME = "";
        this.model.TYPE = "";
        this.model.PO_NO = "";
        this.model.ID = "";
    },
    ShowDetail(detail) {
        this.model.MODEL_NAME = "";
        this.model.TYPE = "";
        this.model.PO_NO = "";
        this.model.ID = "";

        this.model.ID = detail.ID;
        this.model.MODEL_NAME = detail.INVOICE;
        this.model.TYPE = detail.TYPE;
        this.model.PO_NO = detail.PO_NO;
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },

    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "INVOICE_ITEM",        
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
      var { data } = await Repository.getRepo("GetConfigPOContent", payload);
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
        PO_NO: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfigPOContent", payload);
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