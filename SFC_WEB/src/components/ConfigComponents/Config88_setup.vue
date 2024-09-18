<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config Setup Table :</span>
      </div>
    </div>
    <div class="div-searchbox row">
  
        <label for="validationDefault02">MODEL_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterModel"
            @update-selected-item="UpdateModelNameReceive"
            :textContent="model.MODEL_NAME_S"
            type="model"
            v-model="model.MODEL_NAME_S"
            textPlaceHolder="Enter MODEL_NAME "
             required
          />
          <label for="validationDefault02">GROUP_NAME</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-3"
            :listAll="filterColumn"
            @update-selected-item="UpdateGroupNameReceive"
            :textContent="model.GROUP_NAME_S"
            type="model"
            v-model="model.GROUP_NAME_S"
            textPlaceHolder="Enter GROUP_NAME "
             required
          />
       
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
        <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button>
         
    </div>
    
    <br/>
<!-- <div class="div-searchbox row">
      <div class="div-searchbox-content">-->
  <div class="div-button">
          <button @click="GotoRoute('/Home/ConfigApp/Config88')" 
          class="btn btn-success" style="border : 1px" >
          {{ $store.state.language == "En" ? "Param Config" : "Param Config" }}
        </button>
        <button @click="GotoRoute('/Home/ConfigApp/Config88_combine')" 
        class="btn btn-info" style="border : 1px">
          {{ $store.state.language == "En" ? "Combine Config" : "Combine Config" }}
        </button>
          <button  @click="Delete()" class="btn btn-danger1" style="border : 1px; hight: 36px" >
             {{ $store.state.language == "En" ? "Delete" : "Delete" }}
        </button>
         <button  @click="CopyModel()" class="btn btn-primary"   style="border : 1px">
             {{ $store.state.language == "En" ? "CopyModel" : "CopyModel" }}
        </button>
         <button  @click="Clear()" class="btn btn-warning1"  style="border : 1px" >
             {{ $store.state.language == "En" ? "Clear" : "Clear" }}
        </button>

      </div>
  <!--  </div>-->

     <br/>
    <div class="main-contain">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
                <th style="width: 1px">
                  <input type="checkbox" v-model="selectAll" @click="select" >
               <!--{{ $store.state.language == "En" ? "Delete" : "Xóa" }}!-->   
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
                <td class="td-delete">
              
                  
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
            id="validationDefault01"
            :maxlength="35"
            v-model="model.MODEL_NAME"
            required
           :disabled="disabled == 1"
          />
        </div>
        <div class="col-md-3 mb-3">
           <label for="validationDefault02">GROUP_NAME</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="35"
            v-model="model.GROUP_NAME"
            required
          />
        </div>
        <div class="col-md-3 mb-3">
           <label for="validationDefault02">PORT</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="1"
            v-model="model.PORT"
            required
          />
        </div>
        
      </div>
      <!-- row 2 -->
      <div class="form-row">

        <div class="col-md-3 mb-3">
          <label for="validationDefault01">FORMAT</label>

            <textarea class="pa5 ma3" v-model="model.FORMAT"
            rows="2" cols="200"
             style="width:920px;  font-weight: bold;"
                :maxlength="1000"
            ></textarea>
        
        </div>
      
       
      </div>

      <!--row 3!-->
      <div class="form-row">
        <div class="col-md-3 mb-3">
      
              <label for="validationDefault01">ACTION</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="10"
            v-model="model.ACTION"
          
            />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault02">LENGTH</label>
          <input
            type="number"
            class="form-control form-control-sm text-element"
            id="validationDefault02"
            v-model="model.LENGTH"
          
          />
        </div>
        <div class="col-md-3 mb-3">
          <label for="validationDefault01">INPUT</label>
          <input
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            :maxlength="15"
            v-model="model.INPUT"
          
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
//import Vue from 'vue';
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
//import DropDownListPlugin  from "@syncfusion/ej2-vue-dropdowns";

//Vue.useCssVars(DropDownListPlugin);
export default {
  components: {
    DropdownSearch,
   // DropDownListPlugin
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
      MODEL_NAME_S: "",
      GROUP_NAME_S: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
     
      model: {
     
        EMP: localStorage.username,
        database_name: localStorage.databaseName,
        MODEL_NAME: "",
        GROUP_NAME: "",
        MODEL_NAME_S: "",
        GROUP_NAME_S: "",
        PORT: "",
        FORMAT: "",
        ACTION: "",
        LENGTH: "",
        INPUT: ""
       // DATA5: "",
      },
           disabled: 0,
      ListModel: [],
      listChecked: [],
      ListRoute: [],
      ListInputGroup: [],
      ListBom: [],
      ListOutputGroup: [],
      ListModelSerial: [],
      ListColumn : [],
      ListModelSelect :[],
      selected: [],
      selectAll: false,
     frmLoginPopup: {}
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

    select() {
      
      this.selected = [];
      if (!this.selectAll) {
      
       this.DataTable.forEach((value) =>
       {
        this.selected.push(value);
       }
       );

      }
     // console.log(this.selected)
    },

    selectCat(item){
          //this.selected = [];
          this.selected.push(item);
          console.log(this.selected)
    },

  async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config88_LoadModel_Name", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME_S);
      });
    },

      UpdateModelNameReceive(value) {

       // console.log("model: " + value);
        if(this.model.MODEL_NAME_S != value)
        {
          this.model.MODEL_NAME_S = value;
       
          this.GetGroup();
        }
     
    },
    UpdateGroupNameReceive(value)
    {
        this.model.GROUP_NAME_S= value;
    },

    async GetGroup() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME : this.model.MODEL_NAME_S,
        GROUP_NAME: this.model.GROUP_NAME
      };
    
      var { data } = await Repository.getRepo("Config88_Setup_LoadGroup_Name", payload);
      this.ListColumn=[];
      if(data.data.length > 0)
      {
         data.data.forEach((element) => {
        this.ListColumn.push(element.GROUP_NAME_S);
      });
      }
     
    },

    async SaveData() {
      if (
        this.model.MODEL_NAME == "" || this.model.GROUP_NAME == "" || this.model.FORMAT == "" 
        || this.model.PORT=="" || this.model.LENGTH =="" || this.model.ACTION=="" || this.model.INPUT==""
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
            "InsertUpdateConfig88_Setup",
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
          var payload={
              EMP: localStorage.username,
              database_name: localStorage.databaseName,
              MODEL_NAME: this.selected[i].MODEL_NAME,
              GROUP_NAME: this.selected[i].GROUP_NAME,
              MODEL_NAME_NEW:  formValues[0]

          };
          var { data } = await Repository.getRepo(
            "Duplicate_Config88_Setup",
            payload
          );
            debugger;
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
          }
          else if (data.result == "areadly setup")
          {
             this.$swal("", "Đã set up Model này rồi", "warning");
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

        html: "Model_name: " + item.MODEL_NAME +" && " + "GROUP_NAME: "+ item.GROUP_NAME+
            "<br>" +
             "<br>" +
            '<button type="button" role="button" tabindex="0" style="background-color: rgba(214,130,47,1.00); border: 0;border-radius: 3px; box-shadow: none;color: #fff; cursor: pointer;font-size: 17px;font-weight: 500 ; height:35px  ">' + 'Delete by model' + '</button>' +
            '<button type="button" role="button" tabindex="0" style="background-color: rgb(18, 52, 240); border: 0;border-radius: 3px; box-shadow: none;color: #fff; cursor: pointer;font-size: 17px;font-weight: 500 ; height:35px  ">' + 'Delete record' + '</button>',
        showCancelButton: true,
        showConfirmButton: false
      })
      .then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          ID: item.ID,
          EMP: localStorage.username,
          MODEL_NAME: item.MODEL_NAME,
          VERSION_CODE: item.VERSION_CODE,
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
    Clear()
    {
        this.model.MODEL_NAME_S="";
        this.model.GROUP_NAME_S="";
    },
    ClearForm() {
    this.model.MODEL_NAME = "";
    this.model.GROUP_NAME = "";
    this.model.PORT = "";
    this.model.FORMAT = "";
    this.model.ACTION = "";
    this.model.LENGTH = "";
    this.model.INPUT = "";

    },
    ShowDetail(detail) {
        this.model.MODEL_NAME =  detail.MODEL_NAME;
        this.disabled = (this.disabled + 1) % 2;
        this.model.GROUP_NAME =  detail.GROUP_NAME;
        this.model.PORT =  detail.PORT;
        this.model.FORMAT =  detail.FORMAT;
        this.model.ACTION =  detail.ACTION;
        this.model.LENGTH =  detail.LENGTH;
        this.model.INPUT =  detail.INPUT;
   
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
      var { data } = await Repository.getRepo("GetConfig88_setup_Content", payload);
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
        GROUP_NAME: this.model.GROUP_NAME_S
      };

    //console.log(this.model.MODEL_NAME_S);
      var { data } = await Repository.getRepo("GetConfig88_setup_Content", payload);
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
  background: #eb2e15;
  //cursor: pointer;
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

  //display: flex;
  margin-top: 10px;
  margin-left: 100px;
  position: relative;
 
  //position: right;
  right: 50px;
  text-align: right;
  justify-content: space-between;
  .div-button{
     height: 35px;
  }
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
  height: 15px;
  //display: flex;
  align-content: center;
  justify-content: left;
    button {
     // align-items: center;
      margin-left: 100px;
      border-radius: 0 10px 10px 0;
      height: 35px;
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

    widows: 200px;
  

    input {
      border-radius: 10px 0 0 10px;
      // padding: 0px 5px;
      width: 200px;
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
p {
  margin: 15px 0 15px 0;
}
.dropdown-wrapper {
  z-index: 1000!important;
}
.customSwalBtn{
    background-color: rgb(18, 52, 240);
    border-left-color: rgba(214,130,47,1.00);
    border-right-color: rgba(214,130,47,1.00);
    border: 0;
    border-radius: 3px;
    box-shadow: none;
    color: #fff;
    cursor: pointer;
    font-size: 17px;
    font-weight: 500;
    margin: 30px 5px 0px 5px;
    padding: 10px 32px;
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

</style>