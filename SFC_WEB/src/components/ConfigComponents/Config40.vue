<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>CUST_MODEL:</span>
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
                <!-- <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                </th> -->
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
                <!-- <td class="td-delete" @click="DeleteRecord(item)">
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
                </td>                -->
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

        <!--<div class="col-md-3 mb-3">
          <label for="validationDefault02">CUST_MODEL_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="ListModel"
            @update-selected-item="UpdateModelReceive"
            :textContent="model.CUST_MODEL_NAME"
            type="model"
            textPlaceHolder="Enter Model Name"
          />
        </div>-->

        <div class="col-md-3 mb-3">
          <label for="validationDefault02">MODEL_NAME </label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="ListModel"
            @update-selected-item="UpdateModelReceive"
            :textContent="model.MODEL_NAME"
            type="model"
            textPlaceHolder="Enter Model Name"
          />
        </div>

        <!--<div class="col-md-4 mb-4" style="position: relative;">
          <label for="validationDefault02">Version</label>
          <input type="text" class="form-control form-control-sm " 
          style="width: 89%; position: absolute; z-index: 1;" 
          :value="model.VERSION_CODE"
          textPlaceHolder="Enter Version"    
          v-on:change="UpdateVersionReceive"      
          >
          <select class="form-control form-control-sm " 
          style="background: #6fc3ffdd;"
          v-on:change="UpdateVersionReceive">
              <template v-for="(item2, index2) in ListVersion" :key="index2">
                <option :value="item2">{{item2}}</option>  
              </template>
          </select>
        </div>-->

        <div class="col-md-3 mb-3">    
          <label for="validationDefault02">VERSION_CODE</label>
          <input
            class="form-control form-control-sm text-element" 
            type="text"
            :value="model.VERSION_CODE"
            textPlaceHolder="Enter Version"
            list="Vercode"    
            v-on:change="UpdateVersionReceive" 
          />          
            <dataList id="Vercode">
              <template v-for="(item2, index2) in ListVersion" :key="index2">
                <option :value="item2"></option>  
              </template>           
            </dataList>  
          </div> 

        <div class="col-md-3 mb-3">
          <label for="SITE">SITE</label>
          <input
            class="form-control form-control-sm text-element"
            id="SITE"
            :value="model.SITE"
            type="text"
            @input="updateSITE"
            textPlaceHolder="Enter SITE"  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="BRCM_NAME">BRCM_NAME</label>
          <input
            class="form-control form-control-sm text-element"
            id="BRCM_NAME"
            :value="model.BRCM_NAME"
            type="text"
            @input="updateBRCM_NAME"
            textPlaceHolder="Enter BRCM_NAME"  />
        </div>

        <!--<div class="col-md-3 mb-3">
          <label for="BRCM_CODE">BRCM_CODE</label>
          <input
            class="form-control form-control-sm text-element"
            id="BRCM_CODE"
            :value="model.BRCM_CODE"
            type="text"
            textPlaceHolder="Enter BRCM_CODE"  />
        </div>-->
        <div class="col-md-3 mb-3">
          <label for="BRCM_VER">BRCM_VER</label>
          <input
            class="form-control form-control-sm text-element"
            id="BRCM_VER"
            :value="model.BRCM_VER"
            type="text"
            @input="updateBRCM_VER"
            textPlaceHolder="Enter BRCM_VER"  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="DEV">DEV </label>
          <input
            class="form-control form-control-sm text-element"
            id="DEV"
            :value="model.DEV"
            type="text"
            @input="updateDEV"
            textPlaceHolder="Enter DEV "  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="DATA1">VRT</label>
          <input
            class="form-control form-control-sm text-element"
            id="DATA1"
            :value="model.DATA1"
            type="text"
            @input="updateDATA1"
            textPlaceHolder="Enter DATA1"  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="DATA2">DATA2</label>
          <input
            class="form-control form-control-sm text-element"
            id="DATA2"
            :value="model.DATA2"
            type="text"
            @input="updateDATA2"
            textPlaceHolder="Enter DATA2"  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="DATA3">DATA3</label>
          <input
            class="form-control form-control-sm text-element"
            id="DATA3"
            :value="model.DATA3"
            type="text"            
            @input="updateDATA3"
            textPlaceHolder="Enter DATA3"  />
        </div>
        <div class="col-md-3 mb-3">
          <label for="DATA4">DATA4</label>
          <input
            class="form-control form-control-sm text-element"
            id="DATA4"
            :value="model.DATA4"
            type="text"
            @input="updateDATA4"
            textPlaceHolder="Enter DATA4"  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="DATA5">DATA5</label>
          <input
            class="form-control form-control-sm text-element"
            id="DATA5"
            :value="model.DATA5"
            type="text"            
            @input="updateDATA5"
            textPlaceHolder="Enter DATA5"  />
        </div>
        <div class="col-md-3 mb-3">
          <label for="DATA6">DATA6</label>
          <input
            class="form-control form-control-sm text-element"
            id="DATA6"
            :value="model.DATA6"
            type="text"
            @input="updateDATA6"
            textPlaceHolder="Enter DATA6" />
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
      ListVersion: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME : "",
        CUST_MODEL_NAME : "",
        VERSION_CODE : "",
        BRCM_NAME : "",
        BRCM_VER : "",
        SITE : "",
        DEV : "",
        DATA1 : "",
        DATA2 : "",
        DATA3 : "",
        DATA4 : "",
        DATA5 : "",
        DATA6 : ""
      },     
      // ID: '',
      // database_name: localStorage.databaseName,
      // EMP: localStorage.username,
      // MODEL_NAME : '',
      // CUST_MODEL_NAME : '',
      // VERSION_CODE : '',
      // BRCM_NAME : '',
      // SITE : '',
      // DEV : '',
      // DATA1 : '',
      // DATA2 : '',
      // DATA3 : '',
      // DATA4 : '', 
      LIST_VERSION_CODE:[],
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
    this.CheckPrivilege();
    this.GetModel();
  },
  methods: {
    UpdateModelReceive(value) {
        this.model.MODEL_NAME = value;
        this.model.VERSION_CODE = "";
        this.model.BRCM_NAME = "";
        this.model.BRCM_VER = "";
        this.model.SITE = "";
        this.model.DEV = "";
        this.model.DATA1 = "";
        this.model.DATA2 = "";
        this.model.DATA3 = "";
        this.model.DATA4 = "";
        this.model.DATA5 = "";
        this.model.DATA6 = "";
        this.GetVersion(value);
      
    },      
    UpdateVersion(event){
      this.model.VERSION_CODE = event.target.value;
      debugger;
    },  
    UpdateVersionReceive: function(event) {
      this.model.VERSION_CODE = event.target.value;     
    },  
    updateBRCM_NAME(event) {
      this.model.BRCM_NAME = event.target.value;
    },
    updateBRCM_VER(event) {
      this.model.BRCM_VER = event.target.value;
    },
    updateSITE(event) {
      this.model.SITE = event.target.value;
    },
    updateDEV(event) {
      this.model.DEV = event.target.value;
    },
    updateDATA1(event) {
      this.model.DATA1 = event.target.value;
    },
    updateDATA2(event) {
      this.model.DATA2 = event.target.value;
    },
    updateDATA3(event) {
      this.model.DATA3 = event.target.value;
    },
    updateDATA4(event) {
      this.model.DATA4 = event.target.value;
    },
    updateDATA5(event) {
      this.model.DATA5 = event.target.value;
    },
    updateDATA6(event) {
      this.model.DATA6 = event.target.value;
    },


    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config40_MODEL", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },
    async GetVersion(value) {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME:value,
      };
      var { data } = await Repository.getRepo("Config40GetVersion", payload);
      if(this.model.MODEL_NAME != ""){
        this.ListVersion = [];  
        data.data.forEach((element) => {
          this.ListVersion.push(element.VERSION_CODE);
        });
      }    
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.value = text;
      this.isVisible = false;      
    },
    async SaveData() {
      //console.log("1 "+this.model.MODEL_NAME+ "2 " + this.model.VERSION_CODE+ "3 "+ this.model.BRCM_NAME+"4 "+ this.model.BRCM_CODE);||this.model.BRCM_NAME == "" || this.model.BRCM_CODE == ""
      if (this.model.MODEL_NAME == "" || this.model.VERSION_CODE == ""||this.model.BRCM_NAME == "" || this.model.BRCM_VER == "") {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      } else {
        const regex = /[|]/;
        var test = this.model.BRCM_NAME + this.model.BRCM_VER+this.model.SITE +this.model.DEV +this.model.DATA1 +this.model.DATA2
         +this.model.DATA3 +this.model.DATA4 +this.model.DATA5 +this.model.DATA5;
        if(regex.test(test)){
          if (localStorage.language == "En") {
            this.$swal("", "Cannot input character '|'", "error");
          } else {
            this.$swal("", "Các trường không được chứa kí tự '|'", "error");
          }
          return;
        }

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
            CUST_MODEL_NAME: this.model.BRCM_NAME,
            MODEL_NAME: this.model.MODEL_NAME,
            VERSION_CODE: this.model.VERSION_CODE,
            CUSTMODEL_DESC5: this.model.BRCM_VER,
            CUSTMODEL_DESC1: this.model.SITE.toString(),
            CUSTMODEL_DESC2: this.model.DEV? this.model.DEV:"",
            CUSTMODEL_DESC3: this.model.DATA1? this.model.DATA1:"",
            CUSTMODEL_DESC4: this.model.DATA2? this.model.DATA2:"",
            CUSTMODEL_DESC6: this.model.DATA3? this.model.DATA3:"",
            CUSTMODEL_DESC7: this.model.DATA4? this.model.DATA4:"",
            CUSTMODEL_DESC8: this.model.DATA5? this.model.DATA5:"",
            CUSTMODEL_DESC9: this.model.DATA6? this.model.DATA6:"", 
            CUSTOMER_NAME:"",
     
          };

          var { data } = await Repository.getRepo("InsertUpdateConfig40",payload);
          
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
          MODEL_NAME: item.MODEL_NAME,
          VERSION_CODE: item.VERSION_CODE,
        };
        var { data } = await Repository.getRepo("DeleteConfig40", payload);
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
        this.model.CUST_MODEL_NAME = "";
        this.model.VERSION_CODE = "";
        this.model.BRCM_NAME = "";
        this.model.BRCM_VER = "";
        this.model.SITE = "";
        this.model.DEV = "";
        this.model.DATA1 = "";
        this.model.DATA2 = "";
        this.model.DATA3 = "";
        this.model.DATA4 = ""; 
        this.model.DATA5 = "";
        this.model.DATA6 = "";
    },
    ShowDetail(detail) {
        this.model.MODEL_NAME = "";
        this.model.VERSION_CODE = "";
        this.model.BRCM_NAME = "";
        this.model.BRCM_VER = "";
        this.model.SITE = "";
        this.model.DEV = "";
        this.model.DATA1 = "";
        this.model.DATA2 = "";
        this.model.DATA3 = "";
        this.model.DATA4 = "";     
        this.model.DATA5 = "";
        this.model.DATA6 = "";

        this.model.ID = detail.ID;
        this.model.MODEL_NAME = detail.MODEL_NAME;
        this.model.VERSION_CODE = detail.VERSION_CODE;
        this.model.BRCM_NAME = detail.BRCM_NAME;
        this.model.BRCM_VER = detail.BRCM_VER;
        this.model.SITE = detail.SITE;
        this.model.DEV = detail.DEV;
        this.model.DATA1 = detail.VRT;
        this.model.DATA2 = detail.DATA2;
        this.model.DATA3 = detail.DATA3;
        this.model.DATA4 = detail.DATA4;     
        this.model.DATA5 = detail.DATA5;
        this.model.DATA6 = detail.DATA6;  
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },

    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "CUST MODEL",        
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
      var { data } = await Repository.getRepo("GetConfig40Content", payload);
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
      var { data } = await Repository.getRepo("GetConfig40Content", payload);
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