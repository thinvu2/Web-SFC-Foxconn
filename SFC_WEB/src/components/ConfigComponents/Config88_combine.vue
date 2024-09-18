<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config Combine table:</span>
      </div>
    </div>
    <div class="div-searchbox row">
     

         <label for="validationDefault02">MODEL_NAME</label>
          <DropdownSearch
            class="form-control2 form-control2-sm text-element"
            
            :listAll="filterModel"
            @update-selected-item="UpdateModelNameReceive"
            :textContent="model.MODEL_NAME_S"
            type="model"
            v-model="model.MODEL_NAME_S"
            textPlaceHolder="Enter MODEL_NAME "
             required
          />

          <label for="validationDefault02">MO_TYPE</label>
          <DropdownSearch
            class="form-control1 form-control1-sm text-element"
            :listAll="filterMoType"
            @update-selected-item="UpdateMoTypeReceive"
            :textContent="model.MO_TYPE_S"
            type="model"
            v-model="model.MO_TYPE_S"
            textPlaceHolder="Enter MO_TYPE"
             required
          />

          <label for="validationDefault02">GROUP_NAME</label>
          <DropdownSearch
            class="form-control1 form-control-sm1"
            :listAll="filterGroup"
            @update-selected-item="UpdateGroupNameReceive"
            :textContent="model.GROUP_NAME_S"
            type="model"
            v-model="model.GROUP_NAME_S"
            textPlaceHolder="Enter GROUP_NAME "
             required
          />
       <button @click="QuerySearch()" class="btn btn-secondary">
          <Icon icon="search" class="sidenav-icon" />
        </button>
       <!-- <input
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
        /> !-->
    
     <!--   <input
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
        />-->
    
    </div>
  <div class="div-button">
    
  <button @click="GotoRoute('/Home/ConfigApp/Config88')" class=" btn btn-success">
          {{ $store.state.language == "En" ? "Param Config" : "Param Config" }}
        </button>
        <button @click="GotoRoute('/Home/ConfigApp/Config88_setup')" class="btn btn-info">
          {{ $store.state.language == "En" ? "Setup Config" : "Setup Config" }}
        </button>
          <button  @click="Delete()" class="btn btn-danger1" >
             {{ $store.state.language == "En" ? "Delete" : "Delete" }}
        </button>
         <button  @click="CopyModel()" class="btn btn-primary" >
             {{ $store.state.language == "En" ? "CopyModel" : "Delete" }}
         
        </button>
         <button  @click="Clear()" class="btn btn-warning1"  style="border : 1px" >
             {{ $store.state.language == "En" ? "Clear" : "Clear" }}
        </button>
  </div>
  <br/>
    <div class="main-contain">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
                <th style="width: 1px">

                   <input type="checkbox" v-model="selectAll" @click="select" >
                 <!-- {{ $store.state.language == "En" ? "Delete" : "Xóa" }}!-->
                </th>
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
                <td class="td-delete" >
                    <input type="checkbox" :value="item" v-model="selected"  @change="check($event)" style="height=20px;">
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
          <label for="validationDefault02">MODEL_NAME</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="model_name"
            :maxlength="30"
            v-model="model.MODEL_NAME"
            :disabled="disabled == 1"
            required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">MO_TYPE</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
             :maxlength="35"
            v-model="model.MO_TYPE"
            required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">GROUP_NAME</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
             :maxlength="30"
            v-model="model.GROUP_NAME"
            required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">TABLE_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterTable"
            @update-selected-item="UpdateTableNameReceive"
            :textContent="model.TABLE_NAME"
            type="model"
            textPlaceHolder="Enter table_name"
             required
          />
        </div>

      </div>
      <!-- row 2 -->
      <div class="form-row">
       
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">PASS_LENGTH</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.PASS_LENGTH"
              required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">FAIL_LENGTH</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.FAIL_LENGTH"
            required
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">FIELD_DESC</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterColumn"
            @update-selected-item="UpdateField_Desc_Receive"
            :textContent="model.FIELD_DESC"
            type="model"
            textPlaceHolder="Enter FIELD NAME"
             required
          />
        </div>
        

        <div class="col-md-3 mb-3">
          <label for="validationDefault01">TABLE_BAK</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model="model.TABLE_BAK"
           
          />
        </div>
      
       
      </div>

      <!--row 3!-->
      <div class="form-row">

        <div class="col-md-3 mb-3">
          <label for="validationDefault02">COLUMN_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterColumn"
            @update-selected-item="UpdateColumnReceive"
            :textContent="model.COLUMN_NAME"
            type="model"
            textPlaceHolder="Enter COLUMN NAME"
             required
          />
        </div>
        <div class="col-md-3 mb-3">
      
            <label for="validationDefault01">SUB_POSTION</label>
            <input
                type="number"
                class="form-control form-control-sm text-element"
                id="validationDefault01"
                v-model="model.SUB_POSTION"
                  required
                />
        </div>
        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">SUB_LENG</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model="model.SUB_LENG"
            required
            />
        </div>

        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">STORAGES</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="1"
            v-model="model.STORAGES"
            required
            />
        </div>
      </div>

      <!--row 4!-->
      <div class="form-row">
        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">CHECK_DUP</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="1"
            v-model="model.CHECK_DUP"
            required
            />
        </div>
       <div class="col-md-3 mb-3">
          <label for="validationDefault02">WHERE_FIELD</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterColumn"
            @update-selected-item="UpdateWhere_Field_Receive"
            :textContent="model.WHERE_FIELD"
            type="model"
            textPlaceHolder="Enter FIELD NAME"
             required
          />
        </div>
         <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">CHECK_SP</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="50"
            v-model="model.CHECK_SP"
            required
            />
        </div>

        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">DATA1</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="50"
            v-model="model.DATA1"
            />
        </div>
      </div>
      <!--row 5!-->
      <div class="form-row">
        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">DATA2</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="100"
            v-model="model.DATA2"
            />
        </div>
        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">DATA3</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="150"
            v-model="model.DATA3"
            />
        </div>
         <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">DATA4</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="150"
            v-model="model.DATA4"
            />
        </div>

        <div class="col-md-3 mb-3">
      
        <label for="validationDefault01">DATA5</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
           :maxlength="150"
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
      
       
      model: {
      //  ID: "",
        //TABLE_NAME: "",
        EMP: localStorage.username,
        database_name: localStorage.databaseName,
        MODEL_NAME: "",
        MO_TYPE: "",
        GROUP_NAME: "",
        MODEL_NAME_S: "",
        MO_TYPE_S: "",
        GROUP_NAME_S: "",
        TABLE_NAME: "",
        TABLE_BAK: "",
        PASS_LENGTH: "",
        FAIL_LENGTH: "",
        FIELD_DESC: "",
        COLUMN_NAME: "",
        SUB_POSTION: "",
        SUB_LENG: "",
        STORAGES: "",
        CHECK_DUP: "",
        WHERE_FIELD: "",
        CHECK_SP: "",

        DATA1: "",
        DATA2: "",
        DATA3: "",
        DATA4: "",
        DATA5: "",
      },

        MODEL_NAME_OLD: "",
        MO_TYPE_OLD: "",
        GROUP_NAME_OLD: "",
        //MODEL_NAME_S: "",
        //MO_TYPE_S: "",
        // GROUP_NAME_S: "",
        TABLE_NAME_OLD: "",
        TABLE_BAK_OLD: "",
        PASS_LENGTH_OLD: "",
        FAIL_LENGTH_OLD: "",
        FIELD_DESC_OLD: "",
        COLUMN_NAME_OLD: "",
        SUB_POSTION_OLD: "",
        SUB_LENG_OLD: "",
        STORAGES_OLD: "",
        CHECK_DUP_OLD: "",
        WHERE_FIELD_OLD: "",
        CHECK_SP_OLD: "",
      disabled: 0,
      Listdata : [],
      ListModel: [],
      listChecked: [],
      ListRoute: [],
      ListInputGroup: [],
      ListBom: [],
      ListOutputGroup: [],
      ListModelSerial: [],
      ListModelSelect :[],
      ListTable : [],
      ListColumn : [],
      ListMotype: [],
      ListGroupName: [],
      selected: [],
      Data_New :"",
      Data_Old :"",
      selectAll: false
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
      if (query.length < 3) return this.ListMotype;
      if (this.searchText === "") {
        return this.ListMotype;
      }
      return this.ListMotype.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
     },

       filterGroup: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListGroupName;
      if (this.searchText === "") {
        return this.ListGroupName;
      }
      return this.ListGroupName.filter((user) => {
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

  filterTable: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListTable;
      if (this.searchText === "") {
        return this.ListTable;
      }
      return this.ListTable.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },

  },
  mounted() {
    this.CheckPrivilege();
    this.GetModel();
    this.GetTable();
  },
  methods: {

     GotoRoute(route) {
      this.$router.push({ path: route });
    },
// get all modelname to load in drodownlist model_name
  async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config88_LoadModel_Name", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME_S);
      });
    },
// get all modelname to load in drodownlist Table_name
     async GetTable () {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config88_LoadTatble_Name", payload);
      data.data.forEach((element) => {
        this.ListTable.push(element.TABLE_NAME);
      });
    },
// get all Column_name by Table_name to load in drodownlist ColumnName
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

// get all Mo_type by MODEL_NAME to load in drodownlist Mo_type
  async GetMoType() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME : this.model.MODEL_NAME_S,
        MO_TYPE : this.model.MO_TYPE_S,
      };
    
      var { data } = await Repository.getRepo("Config88_Combine_Load_MoType", payload);
      this.ListMotype=[];
      if(data.data.length > 0)
      {
         data.data.forEach((element) => {
        this.ListMotype.push(element.MO_TYPE_S);
      });
      }
     
    },
// get all Group_name by MODEL_NAME, MO_TYPE to load in drodownlist Group_Name
    async GetGroupName() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME : this.model.MODEL_NAME_S,
        MO_TYPE : this.model.MO_TYPE_S,
        GROUP_NAME: this.model.GROUP_NAME_S
        
      };
    
      var { data } = await Repository.getRepo("Config88_Combine_Load_GroupName", payload);
      this.ListGroupName=[];
      if(data.data.length > 0)
      {
         data.data.forEach((element) => {
        this.ListGroupName.push(element.GROUP_NAME_S);
      });
      }
     
    },

//  after chosen model_name ==> Get_Motype load Mo_Type by Model_Name

     UpdateModelNameReceive(value) {

       // console.log("model: " + value);
        if(this.model.MODEL_NAME_S != value)
        {
          this.model.MODEL_NAME_S = value;
          this.model.MO_TYPE_S="";
          this.model.GROUP_NAME_S="";
          this.GetMoType();
        }
    },

//  after chosen Mo_type ==> GetGroupName load Group_name by Model_Name, Mo_type
      UpdateMoTypeReceive(value) {
        if(this.model.MO_TYPE_S != value)
        {
          this.model.MO_TYPE_S = value;
          this.model.GROUP_NAME_S="";
          this.GetGroupName();
        }
    },
//  after chosen Table_name ==> GetColumn load Column_name Table_name 
    UpdateTableNameReceive(value) {

        if(this.model.TABLE_NAME != value)
        {
        this.model.TABLE_NAME = value;
        this.model.FIELD_DESC='';
        this.model.COLUMN_NAME='';
        this.model.WHERE_FIELD='';
        this.GetColumn();
        }
    },
// 
     UpdateField_Desc_Receive(value) {

      this.model.FIELD_DESC=value;
   
    },
    
     UpdateWhere_Field_Receive(value) {

      this.model.WHERE_FIELD=value;
   
    },

    UpdateColumnReceive(value) {
      this.model.COLUMN_NAME = value;
     // this.GetColumn();
    }, 

     UpdateGroupNameReceive(value){
      this.model.GROUP_NAME_S= value;
    },
    
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
   select() {
      
      this.selected = [];
      if (!this.selectAll) {
      
       this.DataTable.forEach((value) =>
       {
        this.selected.push(value);
       }
       );

      }
      //console.log(this.selected)
    },
    async SaveData() {
      if (
        this.model.MODEL_NAME == "" ||
        this.model.TABLE_NAME == "" ||
        this.model.GROUP_NAME == "" || this.model.MO_TYPE=="" || this.model.TABLE_NAME ==""||
        this.model.PASS_LENGTH == "" || this.model.FAIL_LENGTH=="" || this.model.FIELD_DESC =="" ||
        this.model.COLUMN_NAME == "" || this.model.SUB_POSTION=="" || this.model.SUB_LENG =="" ||
        this.model.STORAGES == "" || this.model.CHECK_DUP=="" || this.model.CHECK_SP =="" || this.model.WHERE_FIELD =="" 
      ) {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields || Không được bỏ trống ", "error");
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

        // neu co olda data ==> update 
        
           
              this.Data_New = this.model.MODEL_NAME +","+this.model.MO_TYPE+","
            +this.model.GROUP_NAME +","+this.model.TABLE_NAME+","+ this.model.TABLE_BAK+"/,"+ this.model.PASS_LENGTH
            +","+ this.model.FAIL_LENGTH+","+ this.model.FIELD_DESC+","+this.model.COLUMN_NAME
            +","+ this.model.SUB_POSTION+","+this.model.SUB_LENG+","+ this.model.STORAGES+","+this.model.CHECK_DUP
            +","+this.model.WHERE_FIELD+","+this.model.CHECK_SP+","+this.model.DATA1+"/,"+this.model.DATA2
            +"/,"+ this.model.DATA3+"/,"+ this.model.DATA4+"/,"+this.model.DATA5+"/"
           // goi den update 
                   debugger;
                      var type ;
                       if(this.Data_Old !="")
                       {
                        type= "UPDATE";
                       }
                       else 
                       {
                        type="INSERT";
                       }
                   var payload = {
                      database_name: localStorage.databaseName,
                      emp : localStorage.username,
                      Data_Old: this.Data_Old,
                      Data_New : this.Data_New,
                      Action_Type: type
                    };
                      var { data} = await Repository.getRepo(
                          "InsertUpdateConfig88_Combine",
                          payload
                        );
                        if (data.result == "privilege") {
                          if (localStorage.language == "En") {
                            this.$swal("", "Not privilege", "error");
                          } else {
                            this.$swal("", "Bạn không có quyền thêm sửa", "error");
                          }
                        }
                          else if( data.result=='error_pass')
                        {
                            this.$swal("Wrong Passlength & FailLength", "Passlength hoặc Failength không đúng với Model và trạm ", "warning");
                        }
                        else if (data.result == "ok") {
                          await this.QuerySearch();
                          if (localStorage.language == "En") {
                            this.$swal("", "Apply successfully", "success");
                          } else {
                            this.$swal("", "Cập nhật thành công", "success");
                          }
                        } else if(data.result == "exists") {
                          
                          this.$swal("", "Areadly exists / Đã tồn tại dòng config này rồi", "error");
                        }
                        else 
                        {
                          this.$swal("", data.result, "error");
                        }
              
          
          // start var1 
          // end var1 
        });
      }
    },
    
    ClearForm() {
      this.disabled= (this.disabled+1) %2;
        this.model.MODEL_NAME = "";
        this.model.MO_TYPE = "";
        this.model.GROUP_NAME = "";
        this.model.TABLE_NAME = "";
        this.model.TABLE_BAK = "";
        this.model.PASS_LENGTH = "";
        this.model.FAIL_LENGTH = "";
        this.model.FIELD_DESC = "";
        this.model.COLUMN_NAME = "";
        this.model.SUB_POSTION = "";
        this.model.SUB_LENG = "";
        this.model.STORAGES = "";
        this.model.CHECK_DUP = "";
        this.model.WHERE_FIELD = "";
        this.model.CHECK_SP = "";
        this.model.DATA1 = "";
        this.model.DATA2 = "";
        this.model.DATA3 = "";
        this.model.DATA4 = "";
        this.model.DATA5 = "";
        this.Data_Old="";
        this.Data_New="";
     
    },
    ShowDetail(detail) {
    this.model.MODEL_NAME =  detail.MODEL_NAME;
    this.disabled = (this.disabled + 1) % 2;
    this.model.MO_TYPE    =  detail.MO_TYPE;
    this.model.GROUP_NAME =  detail.GROUP_NAME;
    this.model.TABLE_NAME =  detail.TABLE_NAME;
    this.model.TABLE_BAK  =  detail.TABLE_BAK;
    this.model.PASS_LENGTH =  detail.PASS_LENGTH;
    this.model.FAIL_LENGTH =  detail.FAIL_LENGTH;
    this.model.FIELD_DESC   =  detail.FIELD_DESC;
    this.model.COLUMN_NAME =  detail.COLUMN_NAME;
    this.model.SUB_POSTION =  detail.SUB_POSTION;
    this.model.SUB_LENG    =  detail.SUB_LENG;
    this.model.STORAGES   =  detail.STORAGES;
    this.model.CHECK_DUP  =  detail.CHECK_DUP;
    this.model.WHERE_FIELD =  detail.WHERE_FIELD;
    this.model.CHECK_SP =  detail.CHECK_SP;
    this.model.DATA1 =  detail.DATA1;
    this.model.DATA2 =  detail.DATA2;
    this.model.DATA3 =  detail.DATA3;
    this.model.DATA4 =  detail.DATA4;
    this.model.DATA5 =  detail.DATA5;

    this.Data_Old = detail.MODEL_NAME +","+detail.MO_TYPE+","
    +detail.GROUP_NAME +","+detail.TABLE_NAME+","+ detail.TABLE_BAK+","+ detail.PASS_LENGTH
    +","+ detail.FAIL_LENGTH+","+ detail.FIELD_DESC+","+detail.COLUMN_NAME
    +","+ detail.SUB_POSTION+","+detail.SUB_LENG+","+ detail.STORAGES+","+detail.CHECK_DUP
    +","+detail.WHERE_FIELD+","+detail.CHECK_SP;

    console.log(this.Data_Old);

    
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
      var { data } = await Repository.getRepo("GetConfig88_Combine_Content", payload);
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
        MODEL_NAME: this.model.MODEL_NAME_S,
        MO_TYPE : this.model.MO_TYPE_S,
        GROUP_NAME: this.model.GROUP_NAME_S
        
      };
      var { data } = await Repository.getRepo("GetConfig88_Combine_Content", payload);
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

 Delete()
    {
    
      if (this.selected.length==0)
      {
      this.$swal("Must chosen at least one record", " Bạn phải chọn ít nhất một bản ghi", "warning");
      }
      else 
      {
        this.ListModelSelect=[];
      
        for(let i=0 ; i <=this.selected.length -1 ; i++)
        {
          if(this.ListModelSelect.indexOf(this.selected[i].MODEL_NAME) == -1 )
          {
            this.ListModelSelect.push(this.selected[i].MODEL_NAME);
          }
         
        }
       
        if (this.ListModelSelect.length >1)
        {
          this.$swal("You are chose over 1 MODEL_NAME, Please check again/", " Bạn đang chọn nhiều hơn 1 MODEL_NAME, hãy kiểm tra lại", "warning");
        }

        else 
        {
          //this.Delete_New();
          this.sweetAlert();

         // this.demo();
        }
       
      }
    },
     Clear()
    {
        this.model.MODEL_NAME_S="";
        this.model.GROUP_NAME_S="";
        this.model.MO_TYPE_S="";
    },

    CopyModel()
    {
      if (this.selected.length==0)
      {
      this.$swal("Must chosen at least one record", " Bạn phải chọn ít nhất một bản ghi", "warning");
      }
      else 
      {
        this.ListModelSelect=[];
      
        for(let i=0 ; i <= this.selected.length -1 ; i++)
        {
          
          if(this.ListModelSelect.indexOf(this.selected[i].MODEL_NAME) == -1 )
          {
            this.ListModelSelect.push(this.selected[i].MODEL_NAME);
          }
         
        }
       console.log(this.ListModelSelect);
        if (this.ListModelSelect.length >1)
        {
          this.$swal("You are chose over 1 MODEL_NAME, Please check again/", " Bạn đang chọn nhiều hơn 1 MODEL_NAME, hãy kiểm tra lại", "warning");
        }

        else 
        {
         this.CopyModel_Confirm();
        }
       
      }
    },

   
CopyModel_Confirm(){

  debugger;
  (async () => {

    
      const { value: formValues } = await this.$swal({
            title: 'Please Input New ModelName',
            html:
              '<input id="swal-input1" class="swal2-input" placeholder= "Model_Name" type="text">' +
             
              '<br/> <label id="lblMes" style="text-color: red" value="confirm">',
            focusConfirm: false,
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            confirmButtonText: 'Yes & Copy!',
            preConfirm: () => {
              return [
                document.getElementById('swal-input1').value
               
              ]
            }
        })

      if (formValues) {
        for(let i=0 ; i <this.selected.length  ; i++)
        {
          this.Data_New=[];
          this.Data_New = formValues[0] +","+this.selected[i].MO_TYPE+","
            +this.selected[i].GROUP_NAME +","+this.selected[i].TABLE_NAME+","+ this.selected[i].TABLE_BAK+"/,"+ this.selected[i].PASS_LENGTH
            +","+ this.selected[i].FAIL_LENGTH+","+ this.selected[i].FIELD_DESC+","+this.selected[i].COLUMN_NAME
            +","+ this.selected[i].SUB_POSTION+","+this.selected[i].SUB_LENG+","+ this.selected[i].STORAGES+","+this.selected[i].CHECK_DUP
            +","+this.selected[i].WHERE_FIELD+","+this.selected[i].CHECK_SP+","+this.selected[i].DATA1+"/,"+this.model.DATA2
            +"/,"+ this.selected[i].DATA3+"/,"+ this.selected[i].DATA4+"/,"+this.selected[i].DATA5+"/"


          var payload={
              EMP: localStorage.username,
              database_name: localStorage.databaseName,
              Data_Old : "",
              Data_New : this.Data_New,
              Model_old : this.selected[i].MODEL_NAME,
              Action_Type : 'COPY'

          };
          var { data } = await Repository.getRepo(
            "Duplicate_Config88_Combine",
            payload
          );
            debugger;
           if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          }
          else if( data.result=='error_pass')
          {
              this.$swal("Wrong Passlength & FailLength", "Passlength hoặc Failength không đúng với Model và trạm ", "warning");
          }
          else if (data.result == "ok") {
            await this.QuerySearch();
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
          }
          else if (data.result == "areadly setup")
          {
             this.$swal("", "Đã set up Model, Group, Mo_type  này rồi", "warning");
          }

          else {
            this.$swal("", data.result, "error");
          }

        }

        
      }

  })()
},
  sweetAlert(){

  debugger;
  (async () => {

    
      const { value: formValues } = await this.$swal({
            title: 'Must login again to confirm delete',
            html:
              '<input id="swal-input1" class="swal2-input" placeholder= "UserName" type="text">' +
              '<input id="swal-input2" class="swal2-input" placeholder= "Password" type="password">'+
              '<br/> <label id="lblMes" style="text-color: red" value="confirm">',
            focusConfirm: false,
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            confirmButtonText: 'Login & delete it!',
            preConfirm: () => {
              return [
                document.getElementById('swal-input1').value,
                document.getElementById('swal-input2').value
              ]
            }
        })

      if (formValues) {
    
        var payload = {
              DatabaseName: localStorage.databaseName,
              UserName: formValues[0],
              PassWord: formValues[1],
            };
          var { data } = await Repository.getRepo("CheckLogin", payload);
            if (data.result == "ok") {
              // check tiep xem co quyen xoa hay ko 
                 var payload2 ={
                 DatabaseName: localStorage.databaseName,
                  UserName: formValues[0],
                  PassWord: formValues[1],

                 };
                 var { data2 } =  await Repository.getRepo("CheckPermisstionDelete_Setup", payload2);
                  if(data2.result == "ok")
                  {
                  this.$swal('','User nay co quyen xoa','warning');
                  }
                  else 
                  {
                    this.$swal('','User nay khong co quyen xoa','warning');
                  }
                } 
                else
                {
                  if (this.$store.state.language == "En") {
                  
                    this.$swal('','User Name or Pass Incorrect', 'warning');

                  } else {
                    this.$swal('','Tên đăng nhập hoặc mật khẩu không đúng', 'warning');
                  }
            }
      }

  })()
},
  },

   
};
</script>

<style lang="scss" scoped>
.dropdown-wrapper {
  max-width: 100%;
  position: relative;
  margin: 0 auto;

   z-index: 1000!important;
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

.btn-danger1 {
  color: #fff;
  background-color: #dc3545;
  border-color: #dc3545;
  height: 36px;
}

.btn-danger1:hover {
  color: #fff;
  background-color: #c82333;
  border-color: #bd2130;
}

.btn-danger1:focus, .btn-danger.focus {
  box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.5);
}


.btn-warning1 {
  color: #212529;
  background-color: #ffc107;
  border-color: #ffc107;
}

.btn-warning1:hover {
  color: #212529;
  background-color: #e0a800;
  border-color: #d39e00;
}

.btn-warning1:focus, .btn-warning.focus {
  box-shadow: 0 0 0 0.2rem rgba(255, 193, 7, 0.5);
}

.form-control-sm1 {
  height: calc(1.8125rem + 2px);
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
  line-height: 1.5;
  border-radius: 0.2rem;
}

.form-control1 {
  display: block;
  width: 180px;
  height: calc(2.25rem + 2px);
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  line-height: 1.5;
  color: #495057;
  background-color: #fff;
  background-clip: padding-box;
  border: 1px solid #ced4da;
  border-radius: 0.25rem;
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}

.form-control-sm2 {
  height: calc(1.8125rem + 2px);
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
  line-height: 1.5;
  border-radius: 0.2rem;
}

.form-control2 {
  display: block;
  width: 200px;
  height: calc(2.25rem + 2px);
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  line-height: 1.5;
  color: #495057;
  background-color: #fff;
  background-clip: padding-box;
  border: 1px solid #ced4da;
  border-radius: 0.25rem;
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}
</style>