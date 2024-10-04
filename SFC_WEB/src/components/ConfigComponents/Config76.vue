<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>

      <div class="div-config-name row">
        <span>NPI MO Config(76):</span>
      </div>
      <!-- <div class="div-back" @click="GoToFA()">
        <Icon icon="chevron-right" class="back-icon sidenav-icon" />
      </div> -->
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
              ? 'Enter Mo number...'
              : 'Nh?p c獼g l?nh...'
          "
        />
        <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button>

      </div>
  <!-- <div class="div-button1">
          <button @click="GotoRoute('/Home/ConfigApp/Config76_')" 
          class="btn btn-success" style="border : 1px" >
          {{ $store.state.language == "En" ? "Config FA" : "Config FA" }}
        </button>
        </div> -->
       
    </div>
    
    <div class="main-contain">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
                <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Delete" : "X鏇" }}
                </th>
                <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Edit" : "S?a" }}
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
    <div>
    <div class="div-bellow">
      <div class="form-row">
        <div class="col-md-4 mb-4">
          <label for="validationDefault02">MO_NUMBER</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="ListModel"
            @update-selected-item="UpdateModelReceive"
            :textContent="model.MO_NUMBER"
            type="model"
            textPlaceHolder="Enter MO NUMBER"
          />
        </div>

       <div class="col-md-4 mb-4">
          <label for="validationDefault02">GROUP_NAME</label>
          <DropdownSearch
            type="model"
            :listAll="ListGroup"
            @update-selected-item="UpdateGroupReceive"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model="model.GROUP_NAME"
            textPlaceHolder="Enter group name"
            required
          />
        </div>
      </div>
              
      <div class="form-row">        
        <div class="col-md-4 mb-3">
      
         <label for="Yes">
            <input type="radio" name="radio" value="PASS"  id="Yes"  :checked="model.STATUS=='PASS'"
                     class="radio1" @change="onChange($event)"
             />
           PASS
        </label>
       
               
        </div>
        <div class="col-md-4 mb-3">

           <label for="No">
            <input type="radio" name="radio"  value="NG" id="No" :checked="model.STATUS=='NG'"
            @change="onChange($event)"  active-class="active"/>

            NG </label>

        </div>
    
      </div>
      </div>

      <div v-show="show">
        <template  v-if="DataTableFAHeader">
          <table id="tableMain_" class="table mytable"  >
            <thead>
              <tr>
                
                <template v-for="(item, index) in DataTableFAHeader" :key="index">
                  <th v-if="item != 'ID'">
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <template v-for="(item, index) in DataTableFA" :key="index">
              <tr :style="{color: ' white'}" >
                
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
</template>
<script>
//import $ from 'jquery';
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
//import { BTabs } from 'bootstrap-vue';
//Vue.component('b-tabs', BTabs);
export default {
  components: {
    DropdownSearch,
  },
  data() {
    return {
      selectedItem: null,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      DatatableFA:[],
      DataTableFAHeader:[],
      columnName: [],
      columnFAName: [],
      status : "",
      show : false,
      model: {
     
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_CODE: "",
        MO_NUMBER: "",
        GROUP_NAME: "",
        FA_NUMBER :"",
        FA_VERSION: "",
        STATUS: ""
    
      },
      ListModel: [],
      ListGroup:[],
      searchText: "",
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
  },
  mounted() {
    this.CheckPrivilege();
    this.GetModel();
    this.GetGroup_Name();
  },
  methods: {

     GotoRoute(route) {
      this.$router.push({ path: route });
    },

     onChange(event)
  {
    //var optionText = event.target.value;
    this.model.STATUS= event.target.value;
    this.status= event.target.value;
              console.log(this.model.STATUS);
  },
    UpdateModelReceive(value) {
      this.model.MO_NUMBER = value;  
      // get TA_verion;
      this.GetTA_Ver();
      // get FA_number;
      this.GetFA_INFOR();

    },
    
    UpdateGroupReceive(value) {
      this.model.GROUP_NAME = value;  
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config76GetAllMonumber", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MO_NUMBER);
      });
    },
    
    async GetGroup_Name() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfigFAContent", payload);
      data.data.forEach((element) => {
        this.ListGroup.push(element.VALUE);
      });
    },

    async SaveData() {
      if (this.model.MO_NUMBER == "" || this.model.GROUP_NAME == "") {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields MO_NUMBER, GROUP_NAME , STATUS (PASS/NG) ", "error");
        } else {
          this.$swal("", "Kh獼g du?c b? tr?ng MO_NUMBER, GROUP_NAME , STATUS (PASS/NG)", "error");
        }
      } else {
        if(this.status=="")
        {
        this.$swal("", "Please choose STATUS (PASS / NG) ", "error");
        }else 
        {
          var titleValue = "";
          var textValue = "";
          if (localStorage.language == "En") {
            titleValue = "Are you sure edit?";
            textValue = "Once OK, data will be updated!";
          } else {
            titleValue = "Ch?c ch?n s?a?";
            textValue = "D? li?u s? du?c c?p nh?t";
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
              "InsertUpdateConfirmTA_",
              this.model
            );
            if (data.result == "privilege") {
              if (localStorage.language == "En") {
                this.$swal("", "Not privilege", "error");
              } else {
                this.$swal("", "B?n kh獼g c?quy?n th瘱 s?a", "error");
              }
            } else if (data.result == "ok") {
              await this.QuerySearch();
              if (localStorage.language == "En") {
                this.$swal("", "Apply successfully", "success");
              } else {
                this.$swal("", "C?p nh?t th跣h c獼g", "success");
              }
            } else {
              this.$swal("", data.result, "error");
            }
          });
        }
        
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
        titleValue = "Ch?c ch?n x鏇?";
        textValue = "Sau khi x鏇 s? kh獼g th? kh灁 ph?c!";
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
          MO_NUMBER: item.MO_NUMBER,
          GROUP_NAME : item.GROUP_NAME,
          STATUS : item.STATUS
        };
        var { data } = await Repository.getRepo("DeleteConfigTA_", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "C?p nh?t th跣h c獼g", "success");
          }
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "B?n kh獼g c?quy?n x鏇", "error");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },
    ClearForm() {
     this.model.MO_NUMBER="";
      this.model.GROUP_NAME="" ;
      this.status="";
      this.$store.state.listSelectDualModel=[];
      this.show=false;     
    },
    ShowDetail(detail) {
 
      this.model.MO_NUMBER = detail.MO_NUMBER;
      this.model.GROUP_NAME = detail.GROUP_NAME;
      /// this.model.STATUS = detail.ITEM_CODE;
      this.GetGroupOfEmNo(detail.MO_NUMBER);

      ///console.log(detail.STATUS);

    if(detail.STATUS=="PASS")
    {
      this.status="PASS";
      this.model.STATUS= "PASS";
    }
    else
    {
      this.status="NG";
      this.model.STATUS="NG";
    }
     // this.model.ITEM_TYPE = detail.ITEM_TYPE;

    },
    

    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },

     GotoFA() {
      this.GotoRoute('/Home/ConfigApp/Config76_');
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "NPI MO CONFIG"
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
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig76Content", payload);
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
        MO_NUMBER: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig76Content", payload);
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
.btn_select_item {
  font-size: 13px;
  margin-top: 0px;
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

.active .trucksicons {
  border: 2px solid green;
}
.div-button1{
 margin-left: 50px;
}
.lb_checked {
  color: #efef8d !important;
}
</style>