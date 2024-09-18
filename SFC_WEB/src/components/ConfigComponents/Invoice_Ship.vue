<template>
    <div class="div-all">
      <div class="row">
        <div class="div-back" @click="BackToParent()">
          <Icon icon="chevron-left" class="back-icon sidenav-icon" />
        </div>
        <div class="div-config-name row">
          <span> Address_Ship </span>
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
                ? 'Enter PO_NO...'
                : 'Nhập PO_NO...'
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
        <!-- row-1 -->
        <div class="form-row">
          <div class="col-md-3 mb-3">
            <label for="validationDefault01">DELIVERY_NO</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault01"
              autocomplete="off"
              v-model="model.PO_NO"
              :disabled="isDisabled"
              required
              placeholder="DELIVERY_NO"
            />
          </div>

          <div class="col-md-3 mb-3">
            <label for="validationDefault02">AddrTo1</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault02"
              autocomplete="off"
              v-model="model.AddrTo1"
              placeholder="Enter Address"
              required
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="validationDefault03">AddrTo2</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault03"
              autocomplete="off"
              v-model="model.AddrTo2"
              placeholder="Address (optional)"
              required
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="validationDefault04">AddrTo3</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault04"
              autocomplete="off"
              v-model="model.AddrTo3"
              placeholder="Address (optional)"
              required
            />
          </div>
        </div>
        <!-- row 2 -->
        <div class="form-row">
          <div class="col-md-3 mb-3">
            <label for="validationDefault05">AddrTo4</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault05"
              autocomplete="off"
              v-model="model.AddrTo4"
              placeholder="Address (optional)"
              required
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="validationDefault06">AddrTo5</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault06"
              autocomplete="off"
              v-model="model.AddrTo5"
              placeholder="Address (optional)"
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="validationDefault07">AddrTo6</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault07"
              autocomplete="off"
              v-model="model.AddrTo6"
              placeholder="Address (optional)"
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="validationDefault08">AddrTo7</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault08"
              autocomplete="off"
              v-model="model.AddrTo7"
              placeholder="Address (optional)"
            />
          </div>       
          
          <!-- <div class="col-md-3 mb-3">
            <label for="validationDefault02">DN_NO</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault02"
              autocomplete="off"
              v-model="model.DELIVERY_NO"
              placeholder="Enter DELIVERY_NO"
              required
            />
          </div>  -->
          <div class="col-md-3 mb-3"> 
            <label for="validationDefault02">DN_NO</label>
            <input
              class="form-control form-control-sm text-element" 
              type="number"
              :textContent="model.DELIVERY_NO" 
              :value="model.DELIVERY_NO"
              placeholder="Enter DN_NO"
              list="Vercode"
              v-on:change="UpdateDNReceive" 
              min="0"
              step="1"
              @keydown="checkInput($event)"
              @input="limitInputLength($event, 15)"
            />
        
            <dataList id="Vercode">
              <template v-for="(item2, index2) in ListVersion" :key="index2">
                <option :value="item2"></option>  
              </template>           
            </dataList>        
        </div>  
          
          <div class="col-md-3 mb-3">
            <label for="validationDefault02">DATA1</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault02"
              autocomplete="off"
              v-model="model.DATA1"
              placeholder="Enter DATA1"
              required
            />
          </div>
          
          <div class="col-md-3 mb-3">
            <label for="validationDefault02">DATA2</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault02"
              autocomplete="off"
              v-model="model.DATA2"
              placeholder="Enter DATA2"
              required
            />
          </div>
        </div>
      </div>
    </div>
  </template>
  <script>

  import Repository from "../../services/Repository";
  export default {
    data() {
      return {
        isDisabled:false,
        readonly: false,
        textContent: "",
        searchText: "",
        selectedItem: null,
        isVisible: false,
        DataTableHeader: [],
        ListVersion: [],
        DataTable: [],
        valueSearch: "",
        model: {
          database_name: localStorage.databaseName,
          EMP: localStorage.username,
          PO_NO: "",
          AddrTo1: "",
          AddrTo2: "",
          AddrTo3: "",
          AddrTo4: "",
          AddrTo5: "",
          AddrTo6: "",
          AddrTo7: "",
          SHIP_ADDRESS: "",
          DELIVERY_NO: "",
          DATA1: "",
          DATA2: "",
        },
      };
    },
    created() {
      window.addEventListener("click", (e) => {
        if (!this.$el.contains(e.target)) {
          this.isVisible = false;
        }
      });
    },
    mounted(){
        this.GetDN();
        this.LoadComponent();
    },

    methods: {
      checkInput(event) {
    if (event.key === 'e') {
      event.preventDefault();
    }
  },
  limitInputLength(event, maxLength){
    if(event.target.value.length >= maxLength){
      event.target.value = event.target.value.slice(0, maxLength);
    }
  },
    UpdateDNReceive: function(event) {
      this.model.DELIVERY_NO = event.target.value;
    },
    async GetDN() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetDN_NO", payload);
      this.ListVersion = [];
      data.data.forEach((element) => {
        this.ListVersion.push(element.DN);
      });
    },
      async SaveData() {
    if (
        !this.model.PO_NO ||
        !this.model.AddrTo1||
        !this.model.DELIVERY_NO
    ) {
        if (localStorage.language == "En") {
            this.$swal("", "Empty fields or data input null", "error");
        } else {
            this.$swal("", "Khong duoc bo trong hoac bi null", "error");
        }
    } else {
        let titleValue = "";
        let textValue = "";
        if (localStorage.language == "En") {
            titleValue = "Are you sure to save?";
            textValue = "Once confirmed, data will be updated!";
        } else {
            titleValue = "Ban chac chan muon luu?";
            textValue = "Sau khi xac nhan, du lieu se duoc cap nhat!";
        }
        this.$swal({
            title: titleValue,
            text: textValue,
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then(async (willSave) => {
            if (willSave.isConfirmed == false) return;
            let payload = {
                database_name: localStorage.databaseName,
                EMP: localStorage.username,
                PO_NO: this.model.PO_NO,
                SHIP_ADDRESS: this.getAddrValue(),    
            };
            let { data } = await Repository.getRepo("InsertUpdatePoShip", payload);

            if (data.result == "ok") {
                await this.QuerySearch();
                if (localStorage.language == "En") {
                    this.$swal("", "Successfully applied", "success");
                } else {
                    this.$swal("", "Ap dung thanh cong", "success");
                }
            } else {
                this.$swal("", data.result, "error");
            }
            this.ClearForm();
        });
    }
},
      DeleteRecord(item) {
        let titleValue = "";
        let textValue = "";
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
          let payload = {
            database_name: localStorage.databaseName,
            EMP: localStorage.username,
            PO_NO: item.PO_NO,
            SHIP_ADDRESS: this.getAddrValue(),
          };
          let { data } = await Repository.getRepo("DeletePoShip", payload);
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
          this.ClearForm();
        });
      },
      ClearForm() {
        this.isDisabled = false;
        this.model.PO_NO = "";
        this.model.AddrTo1 = "";
        this.model.AddrTo2 = "";
        this.model.AddrTo3 = "";
        this.model.AddrTo4 = "";
        this.model.AddrTo5 = "";
        this.model.AddrTo6 = "";
        this.model.AddrTo7 = "";
        this.model.Delivery_no = "";
        this.model.DATA1 = "";
        this.model.DATA2 = "";
      },
      ShowDetail(item) {
        this.isDisabled = true;
        this.model.PO_NO = item.DELIVERY_NO;
        this.model.AddrTo1 = item.ADDRTO1;
        this.model.AddrTo2 = item.ADDRTO2;
        this.model.AddrTo3 = item.ADDRTO3;
        this.model.AddrTo4 = item.ADDRTO4;
        this.model.AddrTo5 = item.ADDRTO5;
        this.model.AddrTo6 = item.ADDRTO6;
        this.model.AddrTo7 = item.ADDRTO7;
        this.model.DELIVERY_NO = item.DN_NO;
        this.model.DATA1 = item.DATA1;
        this.model.DATA2 = item.DATA2;
    },

getAddrValue() {
  var a;
  let SHIP_ADDRESS = "|";
  const addTos = [
    this.model.AddrTo1,
    this.model.AddrTo2,
    this.model.AddrTo3,
    this.model.AddrTo4,
    this.model.AddrTo5,
    this.model.AddrTo6,
    this.model.AddrTo7,        
    this.model.DELIVERY_NO,
    this.model.DATA1,
    this.model.DATA2
  ]
  let encounteredNull = false;
addTos.forEach((addr, index) => {
  if (encounteredNull) {
    encounteredNull = false;
      if(index == 7){
          a = 0;
      } else if(index == 8){
          a = 0;
      } else if(index == 9){
          a = 0;
      }else{
    SHIP_ADDRESS += "AddrTo" + (index + 1) + ":";
    }
  } else {
    if (addr === null || addr === undefined || addr.trim() === "") {
      if(index == 7){
          a = 0;
      } else if(index == 8){
          a = 0;
      } else if(index == 9){
          a = 0;
      }else{
      SHIP_ADDRESS += "AddrTo" + (index + 1) + ":";      
      }
    } else {
      if(index == 7){
          a = 0;
      } else if(index == 8){
          a = 0;
      } else if(index == 9){
          a = 0;
      }else {
      SHIP_ADDRESS += "AddrTo" + (index + 1) + ":" + addr.trim();      
      }
    }
  }
  if(index == 7){
    if (addr === null || addr === undefined || addr.trim() === "") {
      SHIP_ADDRESS += "DN_NO:";
    }
    else{
      SHIP_ADDRESS += "DN_NO:" + addr.trim();
    }
  } else if(index == 8){
    if (addr === null || addr === undefined || addr.trim() === "") {
      SHIP_ADDRESS += "DATA1:";
    }
    else{
      SHIP_ADDRESS += "DATA1:" + addr.trim();
    }
  } else if(index == 9){
    if (addr === null || addr === undefined || addr.trim() === "") {
      SHIP_ADDRESS += "DATA2:";
    }
    else{
      SHIP_ADDRESS += "DATA2:" + addr.trim();
    }
  }
  if (index !== addTos.length - 1) {
    SHIP_ADDRESS += "|";
  }
});
console.log("ffffff", a);
return SHIP_ADDRESS;
},
      async LoadComponent() {
        this.valueSearch = "";
        let payload = {
          database_name: localStorage.databaseName,
          // AddrTo1: this.AddrTo1,
        };
       this.DataTable = [];
       this.DataTableHeader = [];
       try{
        let { data } = await Repository.getRepo("GetPoShip", payload);
        if(data.data){
          this.DataTable = data.data;
          if(this.DataTable.length > 0){
            this.DataTableHeader = Object.keys(this.DataTable[0]);
          }
        }else{
          this.$swal("", "not found data");
        }
       }catch(error){
        console.error("data: ", error);
       }
      },

      async QuerySearch() {
        let payload = {
          database_name: localStorage.databaseName,
          PO_NO: this.valueSearch,
        };
        this.DataTable = [];
        this.DataTableHeader = [];
        try{
          let { data } = await Repository.getRepo("GetPoShip", payload);
          if(data.data){
            this.DataTable = data.data;
            if(this.DataTable.length > 0){
              this.DataTableHeader = Object.keys(this.DataTable[0]);
            }
          }else{
            this.$swal("", "not found data");
          }
        }catch(error){
          console.error("data: ", error);
        }
      },

      BackToParent() {
        this.$router.push({ path: "/Home/ConfigApp" });
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
        padding: 6px 20px;
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