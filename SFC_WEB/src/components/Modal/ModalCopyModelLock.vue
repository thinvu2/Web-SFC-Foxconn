<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowCopyModelLock"
        @click="hideModal()"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowCopyModelLock">
        <div class="div-top content-top">
          <b>
            {{
              $store.state.language == "En"
                ? " COPY MODEL LOCK "
                : "COPY MODEL LOCK"
            }}</b
          >
          <span class="closeButton" @click="hideModal()"
            ><Icon style="font-size: 23px; color: red" icon="times-circle"
          /></span>
        </div>
      
     <div class="div-button">
      <button style="width: 70px; height: 35px;"
        class="btn btn-success"
        type="submit"
        @click="SaveData()"
        title="Save"
      >
        Save
      </button>
      <button style="width: 50px; height: 35px;"
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
       
        <div class="col-md-4 mb-4"  >
            <label for="validationDefault02">MODEL_NAME_FROM</label>
            <DropdownSearch
                class="form-control form-control-sm text-element col-md-3"   style="z-index: 100"
                :listAll="ListModel"
                @update-selected-item="UpdateModelOldReceive"
                :textContent="model.MODEL_NAME_FROM"
                type="model"
                textPlaceHolder="Enter Model name"
            />
            </div>
            <div class="col-md-3 mb-3" >
            <label for="validationDefault02" >GROUP_NAME_FROM</label>
            <DropdownSearch
                class="form-control form-control-sm text-element "  style="z-index: 100"
                :listAll="ListMoType_old"
                @update-selected-item="UpdateMoType_Old_Receive"
                :textContent="model.GROUP_NAME_FROM"
                type="model"
                textPlaceHolder="Enter Next group"
            />
            </div>
         <!-- <label for="validationDefault02"> MODEL_NAME_OLD</label>
          <input style="background-color: bisque"
            type="text"
            ref="model_name_old"
            :readonly="1==1"
            class="form-control form-control-sm text-element"
            :value="rowsA"
            @change="updateMo_type"
            required
          />
        </div>-->
        
      </div>
      <div class="form-row">
       
        <div class="col-md-4 mb-4"  > 
            <label for="validationDefault02">MODEL_NAME_TO</label>
            <DropdownSearch
                class="form-control form-control-sm text-element col-md-3"   style="z-index: 99"
                :listAll="ListModel_New"
                @update-selected-item="UpdateModelReceive"
                :textContent="model.MODEL_NAME_TO"
                type="model"
                textPlaceHolder="Enter Model name"
            />
        </div>
         <div class="col-md-3 mb-3" >
            <label for="validationDefault02" >GROUP_NAME_TO</label>
            <DropdownSearch
                class="form-control form-control-sm text-element "  style="z-index: 99"
                :listAll="ListMo_type"
                @update-selected-item="UpdateMoTypeNewReceive"
                :textContent="model.GROUP_NAME_TO"
                type="model"
                textPlaceHolder="Enter Next group"
            />
            </div>
        
      </div>
    </div>
        <!-- <button class="button" @click="$emit('hideModal')">Close Modal</button> -->
      </div>
    </transition>
  </div>
</template>
<script>
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
export default {

  props:['rowsA', 'rowsB'],
//,'CopyMotypeData','CopyVersionData'
  components: {DropdownSearch},
  data() {
  return {
     
      columnName: [],
      ListMo_type : ['ALL'],
      ListMoType_old : ['ALL'],
     // ListMo_type_To : []
      valueSearch: "",
      model: {
         database_name: localStorage.databaseName,
         EMP: localStorage.username,
         MODEL_NAME_FROM:"",
         MODEL_NAME_TO :"",
         GROUP_NAME_FROM: "",
         GROUP_NAME_TO : ""
      },
      ListModel : [],
      ListModel_New : [],
      listChecked: [],
      ListCustomer: [],
    };
  },
 
   mounted() {
   // this.QuerySearch();
    this.ClearForm();
    this.GetModel();
    this.GetModelNew();
    this.getAllGroup_Name();

  },

   watch: {
    rowsA: [{
      handler: 'getSubscriptions'
    }]
  }, 
 
  created() {
  //  this.GetMoType();
   // console.log("dataA: ")  ;
   // alert(this.rowsA);
  },

  methods: {

 async getSubscriptions(el) {
     // console.log(el);
      this.MODEL_NAME_OLD = el;
    var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME_OLD : el ,
       // MO_TYPE : this.model.MO_TYPE_S,
      };
    
      var { data } = await Repository.getRepo("Config88_Copy_Combine_Load_MoType", payload);
      this.ListMoType_old=['ALL'];
      if(data.result=="ok")
      {
          if(data.data.length > 0)
            {
              data.data.forEach((element) => {
                this.ListMoType_old.push(element.MO_TYPE_S);
                });
            }
      }
    
    },

    hideModal() {
      this.valueSearch = "";
      this.ClearForm();
      this.props=[];
     // this.QuerySearch();
      this.$emit("hideModal");
      this.$store.state.listDetailClick = [];
    },
     
updateMo_type()
{
this.GetMoType();
},
    async GetMoType() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME_OLD : this.MODEL_NAME_OLD ,
       // MO_TYPE : this.model.MO_TYPE_S,
      };
    
      var { data } = await Repository.getRepo("Config88_Copy_Combine_Load_MoType", payload);
      this.ListMoType_old=[];
      if(data.result=="ok")
      {
          if(data.data.length > 0)
            {
              data.data.forEach((element) => {
                this.ListMoType_old.push(element.MO_TYPE_S);
                });
            }
      }
      
     
    },

     async SaveData() {
       
      if (this.model.MODEL_NAME_FROM == "" || this.model.MODEL_NAME_TO == "") {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields MODEL_NEW", "error");
        } else {
          this.$swal("", "Không được bỏ trống MODEL_NEW", "error");
        }
        
      } else {
         // neu mo_type_old va new co 1 trong 2 la ALL ==> canh bao phai 2 ALL hoac 2 value
         if  (
            (this.model.GROUP_NAME_FROM == "ALL" && this.model.GROUP_NAME_TO !="ALL") 
            || (this.model.GROUP_NAME_FROM != "ALL" && this.model.GROUP_NAME_TO =="ALL"))

          {
            if (localStorage.language == "En") {
              this.$swal("Invalid", "Invalid GROUP_NAME_FROM && TO must chosen ALL to both of them if need", "error");
            } else {
              this.$swal("Invalid", "GROUP_NAME_FROM và TO cần chọn ALL cho cả 2 ", "error");
            }
             
          }
          else 
          {
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
              var payload ={
                  database_name: localStorage.databaseName,
                EMP: localStorage.username,
                MODEL_NAME_FROM : this.model.MODEL_NAME_FROM,
                GROUP_NAME_FROM : this.model.GROUP_NAME_FROM,
                MODEL_NAME_TO : this.model.MODEL_NAME_TO,
                GROUP_NAME_TO : this.model.GROUP_NAME_TO
              }

              var { data } = await Repository.getRepo(
                "LockCopy",
                payload
              );
              if (data.result == "privilege") {
                if (localStorage.language == "En") {
                  this.$swal("", "Not privilege", "error");
                } else {
                  this.$swal("", "Bạn không có quyền thêm thêm", "error");
                }
              } else if (data.result == "ok") {
            
                if (localStorage.language == "En") {
                  this.$swal("", "Apply successfully", "success");
                } else {
                  
                  this.$swal("", "Cập nhật thành công", "success");
                }
              } else {

                  if(data.result == "areadly setup")
                  {
                    this.$swal("ERROR", "Đã setup Model_Name  này rồi !!", "error");
                  }
                  else 
                  {
                    this.$swal("", data.result, "error");
                  }
              }
            });
          }
         
        
      }
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("CopyModelLock_LoadModel_Name", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },

async GetModelNew() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("CopyModelLock_LoadModel_Name_New", payload);
      data.data.forEach((element) => {
        this.ListModel_New.push(element.MODEL_NAME_S);
      });
    },
     UpdateModelReceive(value) {
      this.model.MODEL_NAME_TO = value;
    },
    async getAllGroup_Name()
    {
        var payload = {
            database_name: localStorage.databaseName,
        };

         var { data } = await Repository.getRepo("CopyModelLock_GetGroup_Type_ALL", payload);
         this.ListMo_type=['ALL'];
      data.data.forEach((element) => {
        this.ListMo_type.push(element.GROUP_NAME);
      });
            

    },

       UpdateMoType_Old_Receive(value)
       {
        this.model.GROUP_NAME_FROM = value
        this.model.GROUP_NAME_TO= value;
       },
        UpdateMoTypeReceive(value)
       {
        this.model.GROUP_NAME_FROM = value
       },

       UpdateMoTypeNewReceive(value)
       {
         this.model.GROUP_NAME_TO= value;
       },
  async UpdateModelOldReceive(value) {

      this.model.MODEL_NAME_FROM = value;

         var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME_FROM : value,
       // MO_TYPE : this.model.MO_TYPE_S,
      };
    
      var { data } = await Repository.getRepo("CopyModelLock_GetGroup_Type_Old", payload);
      this.ListMoType_old=['ALL'];
      if(data.result=="ok")
      {
          if(data.data.length > 0)
            {
              data.data.forEach((element) => {
                this.ListMoType_old.push(element.GROUP_NAME);
                });
            }
      }

    
    },
 

    
        ClearForm() {
        this.show= false;
        this.isReadonly =false;
        this.ListMoType_old = ['ALL'];
        //this.ListMoType_old = ['ALL'];
        this.model.MODEL_NAME_FROM ='';
        this.model.MODEL_NAME_TO= '';
        this.model.GROUP_NAME_FROM= '';
        this.model.GROUP_NAME_TO='';
    },
    

    
  },
};
</script>
<style lang="scss" scoped>
.btnExport {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;
}
.btn-search {
  height: 40px !important;
}
.select-condition {
  option {
    background-color: #429eee;
    padding: 5px 40px;
    color: #fff;
  }
}
.img-excel {
  height: 45px;
  width: 70px;
  &:hover {
    cursor: pointer;
  }
}
.closeButton {
  margin-top: -15px;
  margin-right: -15px;
  &:hover {
    cursor: pointer;
  }
}
.count-number {
  color: red;
}
.content-top {
  display: flex;
  justify-content: space-between;
}
.content-top-export {
  display: flex;
  justify-content: space-between;
}
.div-table {
  border: 1px solid #cdc;
  max-width: 92vw;
  height: 30vh;
  overflow: auto;
}
thead {
  background: #418bca;
  tr {
    &:hover {
      cursor: pointer;
    }
    th {
      color: #fff;
    }
  }
}
#tableMain {
  thead {
    tr {
      th {
        overflow-x: auto;
        white-space: nowrap;
      }
    }
  }
  tr {
    td {
      white-space: nowrap;
      border: 0.5px solid rgb(14, 14, 14);
    }
  }
}
.table-condensed {
  font-size: 12px;
  margin-top: 10px;
}
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: "montserrat", sans-serif;
}

#app {
  position: absolute;

  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
  min-height: 100vh;
  overflow-x: hidden;
}

.button {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;

  display: inline-block;
  padding: 15px 25px;
  background-image: linear-gradient(to right, #cc2e5d, #ff5858);
  border-radius: 8px;

  color: #fff;
  font-size: 18px;
  font-weight: 700;

  box-shadow: 3px 3px rgba(0, 0, 0, 0.4);
  transition: 0.4s ease-out;

  &:hover {
    box-shadow: 6px 6px rgba(0, 0, 0, 0.6);
  }
}

.modal-overlay {
  position: fixed;
  z-index: 1000 !important;
  height: 100vh;
  width: 100vw;
  // position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 99;
  background-color: rgba(0, 0, 0, 0.3);
}

.my_modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 1000 !important;
  height: 50vh;
  width: 65%;
  max-width: 95vw;
  background-color: #fff;
  border-radius: 16px;

  padding: 25px;

  h1 {
    color: #222;
    font-size: 32px;
    font-weight: 900;
    margin-bottom: 15px;
  }

  p {
    color: #666;
    font-size: 18px;
    font-weight: 400;
    margin-bottom: 15px;
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.5s;
}

.slide-enter,
.slide-leave-to {
  transform: translateY(-50%) translateX(100vw);
}


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
  height: 30px;
  display: flex;
  align-content: center;
  justify-content: center;
 
  .div-searchbox-content {
    color:#024873 ; 
    font-size:  20px;
    span{
  text-decoration-color: #024873;
  }
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
  height: 200px;
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
 
 .swal2-container {
   display: -webkit-box;
  display: flex;
  position: fixed;
  z-index: 300000;
}
#dropdowns
{
  z-index:  2000 !important;
  
}
#dropdowns2
{  z-index: 1900 !important;
   
}
}
</style>
