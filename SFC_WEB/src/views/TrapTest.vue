<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>TRAPTEST:</span>
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
              ? 'Enter model name/mo/sn...'
              : 'Nhập tên model/mo/sn...'
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
                <template v-for="(item, index) in DataTableHeader" :key="index">                  
                  <th v-if="item == 'ACTION'"> CONFIRM_ACTION</th>
                  <th v-if="item != 'ID' && item != 'ACTION'"> {{ item }}  </th>
                </template>
              </tr>
            </thead>
            <template v-for="(item, index) in DataTable" :key="index">
              <tr>                
                <template v-for="(item1, index1) in item" :key="index1">
                  <td v-if="index1 == 'ACTION'">{{ item1 }}</td>
                  <td v-if="index1 != 'ID' && index1 != 'ACTION'">{{ item1 }}</td>
                </template>
              </tr>
            </template>
          </table>
        </template>
      </div>
    </div>

    <div class="div-bellow">
      <div class="form-row">
       
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">ACTIONS</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="Liststatus"
            @update-selected-item="GetStatus"
            :textContent="model.ACTION"
            type="model"
            textPlaceHolder="Enter actions"  />
        </div>

        <div class="col-md-3 mb-3">
          <label for="SERIAL_NUMBER">SERIAL_NUMBER</label>
          <input
            class="form-control form-control-sm text-element"
            id="SERIAL_NUMBER"
            :value="model.SERIAL_NUMBER"
            type="text"
            @input="updateSERIAL_NUMBER"
            v-on:keyup.enter="checkSN"
            textPlaceHolder="Enter SERIAL_NUMBER"  />
        </div>
        
        <div class="col-md-3 mb-3">
          <label for="MO_NUMBER" hidden>MO_NUMBER</label>
          <input
            class="form-control form-control-sm text-element"
            id="MO_NUMBER"
            :value="model.MO_NUMBER"
            type="text"
            @input="updateMO_NUMBER"
            textPlaceHolder="Enter MO_NUMBER"  disabled hidden/>
        </div>
        
        <div class="col-md-3 mb-3">
          <label for="MODEL_NAME" hidden>MODEL_NAME</label>
          <input
            class="form-control form-control-sm text-element"
            id="MODEL_NAME"
            :value="model.MODEL_NAME"
            type="text"
            @input="updateMODEL_NAME"
            textPlaceHolder="Enter MODEL_NAME"
            disabled hidden />
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
    </div>
  </div>
</template>
<script>
//import $ from 'jquery';
import Repository from "../services/Repository";
import DropdownSearch from "../components/Template/DropdownSearch.vue";
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
      ListMo: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME: "",
        MO_NUMBER: "",
        SERIAL_NUMBER: "",
        ACTION: "",
      },      
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
  },
  methods: {     
    updateSERIAL_NUMBER(event) {
      this.model.SERIAL_NUMBER = event.target.value;
      //this.SN_check(event.target.value);
      this.model.ACTION = "";
    },  
    checkSN:function(){      
      this.SN_check(this.model.SERIAL_NUMBER);
    } , 
    updateMO_NUMBER(event) {
      this.model.MO_NUMBER = event.target.value;
    },    
    updateMODEL_NAME(event) {
      this.model.MODEL_NAME = event.target.value;
    },  
    GetStatus(value){
      this.model.ACTION = value;
    },

    async SN_check(value) {
      var payload = {
        database_name: localStorage.databaseName,
        SERIAL_NUMBER:value,
      };
      var { data } = await Repository.getRepo("CheckSNTrapTest", payload);
      
      this.Liststatus = [];
      if(data.result =="exist"){
        if (localStorage.language == "En") {
          this.$swal("", "Data already exists R_SN_EXCEPT_HOLD_T or passed PACK_CTN in R107", "error");
        } else {
          this.$swal("", "Dữ liệu đã tồn tại trong R_SN_EXCEPT_HOLD_T or R107 đã qua trạm PACK_CTN", "error");
        }
      }else if(data.result =="fail"){
        if (localStorage.language == "En") {
          this.$swal("", "Data not exist in R107", "error");
        } else {
          this.$swal("", "Dữ liệu không tồn tại trong R107", "error");
        }
      }else{
        data.data.forEach((element) => {
            this.ListModel.push(element.MODEL_NAME);
            this.ListMo.push(element.MO_NUMBER);
        });
        this.model.MODEL_NAME = this.ListModel[0];
        this.model.MO_NUMBER = this.ListMo[0];
        
        if(data.action){
          this.Liststatus.push("CHECK");
        }else{
          this.Liststatus.push("CONFIRM");
        }
      }
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.value = text;
      this.isVisible = false;      
    },
    async SaveData() {
      if (this.model.SERIAL_NUMBER == "" || this.model.ACTION == "") {
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
            MO_NUMBER: this.model.MO_NUMBER,
            SERIAL_NUMBER: this.model.SERIAL_NUMBER,
            ACTION: this.model.ACTION == "CHECK" ? "N" : "Y",        
          };
          var { data } = await Repository.getRepo("InsertUpdateTrapTest",payload);
          
          if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else if (data.result == "ok") {
            await this.QuerySearch();
            this.model.ACTION = "";
            this.model.SERIAL_NUMBER = "";
            this.Liststatus = [];
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
    ClearForm() {
      this.model.ID = "";
      this.model.MODEL_NAME = "";
      this.model.MO_NUMBER = "";
      this.model.SERIAL_NUMBER = "";
      this.model.ACTION = "";
    },
    BackToParent() {
      this.$router.push({ path: "/Home/Applications" });
    },

    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "SN_C",        
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        if (localStorage.language == "En") {
          this.$swal("", "Not privilege", "error");
        } else {
          this.$swal("", "Bạn không có quyền!", "error");
        }
        this.$router.push({ path: "/Home/Applications" });       

      } else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetTrapTestContent", payload);
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
      var { data } = await Repository.getRepo("GetTrapTestContent", payload);
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