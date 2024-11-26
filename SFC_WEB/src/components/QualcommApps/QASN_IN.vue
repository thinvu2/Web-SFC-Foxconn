<template>
    <div class="div-all">
      <div class="row">
        <div class="div-back" @click="isShowForm ? ReturnForm() : BackToParent()">
          <Icon icon="chevron-left" class="back-icon sidenav-icon" />
        </div>
        <div class="div-config-name row">
          <span>QASN_IN</span>
        </div>
      </div>
      <div class="div-searchbox row" v-if="isShowForm === false">
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
                ? 'Enter PACKSLIP_NO...'
                : 'Nhập PACKSLIP_NO...'"
          />
          <button @click="QuerySearch()" class="btn">
            <Icon icon="search" class="sidenav-icon" />
          </button>
        </div>
      </div>
      <!-- Form input data -->
      <div class="container" v-if="isShowForm === true">
            <div class="title-class">
                <p>Advance Shipment Notice</p>
            </div>
            
 
            <div class="form-row">
                <label for="ship_mcmn_creattime">From:</label>
                <input type="text"
                    class="text-input"
                    id="ship_mcmn_creattime"
                    name="ship_mcmn_creattime"
                    readonly
                    v-model="model.MSG_SENDER_NAME"
                />
            </div>
            <div class="form-row">
                <label for="msg_sender_name">To:</label>
                <input type="text"
                    class="text-input"
                    id="msg_sender_name"
                    name="msg_sender_name"
                    readonly
                    v-model="model.MSG_RECEIVER_NAME"
                />
            </div>
             <div class="form-row">
                <label for="msg_receiver_name">Time:</label>
                <input type="text"
                    class="text-input"
                    id="msg_receiver_name"
                    name="msg_receiver_name"
                    readonly
                    v-model="model.SHIP_MCMN_CREATTIME"
                />
            </div>
            <div class="form-row">
                <label for="po_no">Pack Slip No:</label>
                <input type="text"
                    class="text-input"
                    id="po_no"
                    name="po_no"
                    readonly
                    v-model="model.PACKSLIP_NO"
                />
            </div>
            <div class="form-row">
                <label for="poline_no">Airway Bill:</label>
                <input type="text"
                    class="text-input"
                    id="poline_no"
                    name="poline_no"
                    readonly
                    v-model="model.AIRWAYBILL"
                />
            </div>
            <div class="form-row">
                <label for="item_no">Invoice:</label>
                <input type="text"
                    class="text-input"
                    id="item_no"
                    name="item_no"
                    readonly
                    v-model="model.PO_NO"
                />
            </div>
            <div class="actual-ship-from">
                <span class="title-actual-ship">Actual ship from</span>
                <div class="form-row-actual">
                <label for="item_shippedqty">Country Code:</label>
                <input type="text"
                    class="text-input"
                    id="item_shippedqty"
                    name="item_shippedqty"
                    readonly
                    v-model="model.COUNTRY_CODE"
                />
                    <!-- <p>{{ model.COUNTRY_CODE }}</p> -->
                </div>
                <div class="form-row-actual">
                    <label for="item_unitofmeasure">City:</label>
                    <input type="text"
                        class="text-input"
                        id="item_unitofmeasure"
                        name="item_unitofmeasure"
                        readonly
                        v-model="model.SHIP_MCMN_CITY"
                    />
                    <!-- <p>{{ model.SHIP_MCMN_CITY }}</p> -->
                </div>
                <div class="form-row-actual">
                <label for="item_unitofmeasure">Postal Code:</label>
                <input type="text"
                    class="text-input"
                    id="item_unitofmeasure"
                    name="item_unitofmeasure"
                    readonly
                    v-model="model.POSTAL_CODE"
                    />
                    <!-- <p>{{ model.POSTAL_CODE }}</p> -->
                </div>

                <div class="form-row-actual-all">
                    <div class="form-row-actual-all-label">
                        <label for="packslip_no">Address:</label>
                    </div>
                    <div class="form-row-actual-all-input">
                        <!-- <input type="text"
                        id="packslip_no"
                        name="packslip_no"
                        readonly
                        v-model="model.SHIP_MCMN_STREET1"
                    /> -->
                    <p>{{ model.SHIP_MCMN_STREET1 }}</p>
                    </div>
                    <div class="form-row-actual-all-input">
                        <!-- <input type="text"
                        id="packslip_no"
                        name="packslip_no"
                        readonly
                        v-model="model.SHIP_MCMN_STREET2"
                    /> -->
                    <p>{{ model.SHIP_MCMN_STREET2 }}</p>
                    </div>
                    <div class="form-row-actual-all-input">
                        <!-- <input type="text"
                        id="packslip_no"
                        name="packslip_no"
                        readonly
                        v-model="model.SHIP_MCMN_STREET3"
                        /> -->
                        <p>{{ model.SHIP_MCMN_STREET3 }}</p>
                    </div>
                </div>
            </div>

            <div class="form-row">
                <label for="item_unitofmeasure">Pallet No:</label>
                <input type="text"
                    class="text-input"
                    id="item_unitofmeasure"
                    name="item_unitofmeasure"
                    readonly
                    v-model="model.SHIP_MCMN_LPN"
                />
            </div>
            <div class="form-row">
                <label for="item_mpn">GrossWeight:</label>
                <input type="text"
                    class="text-input"
                    id="item_mpn"
                    name="item_mpn"
                    readonly
                    v-model="model.PAL_GROSSWEIGHT"
                />
            </div>
            <div class="form-row">
                <label for="item_description">NetWeight:</label>
                <input type="text"
                    class="text-input"
                    id="item_description"
                    name="item_description"
                    readonly
                    v-model="model.PAL_NETWEIGHT"
                />
            </div>
            <div class="form-row">
                <label for="lot_no">Width:</label>
                <input type="text"
                    id="lot_no"
                    class="text-input"
                    name="lot_no"
                    readonly
                    v-model="model.PAL_WIDTH"
                />
            </div>
            <div class="form-row">
                <label for="receiver_locationname">Length:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_locationname"
                    name="receiver_locationname"
                    readonly
                    v-model="model.PAL_LENGTH"
                />
            </div>
            <div class="form-row">
                <label for="receiver_name">Height:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_name"
                    name="receiver_name"
                    readonly
                    v-model="model.PAL_HEIGHT"
                />
            </div>
            <!-- itemno -->
            <div class="form-row">
                <label for="lot_no">Items No:</label>
                <input type="text"
                    id="lot_no"
                    class="text-input"
                    name="lot_no"
                    readonly
                    v-model="model.ITEM_NO"
                />
            </div>
            <div class="form-row">
                <label for="receiver_locationname">QTY:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_locationname"
                    name="receiver_locationname"
                    readonly
                    v-model="model.ITEM_SHIPPEDQTY"
                />
            </div>
            <div class="form-row">
                <label for="receiver_name">MPN:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_name"
                    name="receiver_name"
                    readonly
                    v-model="model.ITEM_MPN"
                />
            </div>
            <!-- end -->
            <div class="form-row-table">
                <table class="table-form">
                    <template v-for="(item, index) in ShowDataTableOuterLPN" :key="index">
                        <thead>
                            <tr>
                                <th></th>
                                <th>LPN</th>
                                <th>Gross Weight({{ item.OUTERBOX_WEIGHTUNITOFMEASURE }})</th>
                                <th>Net Weight({{ item.INNERBOX_WEIGHTUNITOFMEASURE }})</th>
                                <th>Quantity</th>
                                <th>Receive</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{{ item.OUTERBOX_LPN }}</td>
                                <td>LPN</td>
                                <td>{{ item.OUTERBOX_GROSSWEIGHT }}</td>
                                <td>{{ item.OUTERBOX_NETWEIGHT }}</td>
                                <td>{{ item.OUTERBOX_QTY }}</td>
                                <td></td>
                                <td></td>
                            </tr>
                            <!-- Inner box -->
                            <tr v-for="(itemInnerLPN, indexInnerLPN) in ShowDataTableInnerLPN" :key="indexInnerLPN">
                                <template v-for="(itemInnerLPN1, indexInnerLPN1) in itemInnerLPN" :key="indexInnerLPN1">
                                    <td v-if="indexInnerLPN1 == 'INNERBOX_LPN' || indexInnerLPN1 == 'LPN' || indexInnerLPN1 == 'INNERBOX_GROSSWEIGHT' || indexInnerLPN1 == 'INNERBOX_NETWEIGHT' || indexInnerLPN1 == 'INNERBOX_QTY'">
                                        {{ itemInnerLPN1 }}
                                    </td>
                                </template>
                                <td>
                                    <input type="checkbox">
                                </td>
                                <td>
                                    <select>
                                        <option>Accept</option>
                                        <option>Reject</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </template>
                </table>
            </div>
            <div class="form-row-actual-address">
                <label for="box_received">Receive address:</label>
                <select>
                    <option v-for="(item, index) in ShowDataReceiveAddress" :key="index">
                        {{ item.SHIP_ADDRESS }}
                    </option>
                </select>
            </div>
            <div class="form-row-center">
                <p>Country: Viet Nam, City: Bac Giang, Postal Code: 230000</p>
            </div>
        </div>
        <div class="submit-form" v-if="isShowForm === true">
            <input type="button" 
                id="submit-form" 
                value="Submit"
                @click="SubmitForm()"
            />
            <!-- <input type="button"
                id="return-form"
                value="Return"
                @click="ReturnForm()"
            /> -->
        </div>
      <div class="main-contain" v-if="isShowForm === false">
        <div class="row col-sm-12 div-content">
          <template v-if="DataTableHeader">
            <table id="tableMain" class="table mytable">
              <thead>
                <tr>
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
                <tbody>
                    <template v-for="(item, index) in DataTable" :key="index">
                        <tr>
                            <td class="td-edit" @click="ShowDetail(index)">
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
                </tbody>
            </table>
          </template>
        </div>
      </div>
    </div>
  </template>
  <script>
  import Repository from "../../services/Repository";
  export default {
    data() {
      return {
        isShowForm: false,
        isVisible: false,
        DataTableHeader: [],
        DataTable: [],
        ShowDataDetail:[],
        ShowDataTableOuterLPN: [],
        ShowDataTableInnerLPN: [],
        ShowDataReceiveAddress: [],
        columnName: [],
        valueSearch: "",
        model: {
            database_name: localStorage.databaseName,
            EMP_NO: localStorage.username,
            Total_Received_Qty: "",
            Total_Accepted_Qty: "",
            Inner_LPN: "",
            Box_Received_Qty: "",
            Box_Accepted_Qty: "",
            PACKSLIP_NO: "",
            SHIP_MCMN_CREATTIME: "", 
            MSG_SENDER_NAME: "", 
            MSG_RECEIVER_NAME: "", 
            PO_NO: "", 
            POLINE_NO: "", 
            ITEM_NO: "", 
            ITEM_SHIPPEDQTY: "",
            ITEM_UNITOFMEASURE: "", 
            ITEM_MPN: "", 
            ITEM_DESCRIPTION: "",
            LOT_NO: "",
            RECEIVER_LOCATIONNAME: "",
            RECEIVER_NAME: "", 
            FLAG: "",
            F_ID: "",
            AIRWAYBILL: "", 
            COUNTRY_CODE: "", 
            POSTAL_CODE: "", 
            SHIP_MCMN_CITY: "", 
            SHIP_MCMN_STREET1: "", 
            SHIP_MCMN_STREET2: "", 
            SHIP_MCMN_STREET3: "",
            SHIP_MCMN_LPN: "", 
            PAL_GROSSWEIGHT: "", 
            PAL_NETWEIGHT: "", 
            PAL_WIDTH: "", 
            PAL_LENGTH: "", 
            PAL_HEIGHT: "", 
        }, 
        arrTest: [
            {
                "OUTERBOX_LPN": "outer LPN",
                "OUTERBOX_GROSSWEIGHT": "18.0",
                "OUTERBOX_NETWEIGHT": "15.0",
                "OUTERBOX_WEIGHTUNITOFMEASURE": "kg",
                "QuantityOuter": "2",
                "INNERBOX_LPN": "Inner LPN",
                "INNERBOX_GROSSWEIGHT": "10.0",
                "INNERBOX_NETWEIGHT": "8.0",
                "INNERBOX_WEIGHTUNITOFMEASURE": "kg",
                "QuantityInner": "1"
            },
            {   "OUTERBOX_LPN": "outer LPN",
                "OUTERBOX_GROSSWEIGHT": "18.0",
                "OUTERBOX_NETWEIGHT": "15.0",
                "OUTERBOX_WEIGHTUNITOFMEASURE": "kg",
                "QuantityOuter": "2",
                "INNERBOX_LPN": "Inner LPN",
                "INNERBOX_GROSSWEIGHT": "8",
                "INNERBOX_NETWEIGHT": "7.0",
                "INNERBOX_WEIGHTUNITOFMEASURE": "kg",
                "QuantityInner": "1"
            }
        ]
      }
    },
    created() {
      window.addEventListener("click", (e) => {
        if (!this.$el.contains(e.target)) {
          this.isVisible = false;
        }
      });
    },
    computed: {},
    mounted() {
        this.LoadComponent();
    },
    methods: {
      async SubmitForm() {
        if(
            !this.model.Total_Received_Qty ||
            !this.model.Total_Accepted_Qty ||
            !this.model.Inner_LPN ||
            !this.model.Box_Received_Qty ||
            !this.model.Box_Accepted_Qty
        ){
            if (localStorage.language == "En") {
            this.$swal("", "Empty fields", "error");
            } else {
                this.$swal("", "Không được bỏ trống", "error");
            }
        }else{
            let titleValue = "";
            let textValue = "";
            titleValue = "Are you sure edit?";
            textValue = "Once OK, data will be updated!";
            this.$swal({
            title: titleValue,
            text: textValue,
            icon: "warning",
            buttons: true,
            dangerMode: true,
          }).then(async (willDelete) => {
            if (willDelete.isConfirmed == false) return;
            let payload = {
                EMP_NO: localStorage.username,
                database_name: localStorage.databaseName,
                Total_Received_Qty: this.model.Total_Received_Qty,
                Total_Accepted_Qty: this.model.Total_Accepted_Qty,
                Inner_LPN: this.model.Inner_LPN,
                Box_Received_Qty: this.model.Box_Received_Qty,
                Box_Accepted_Qty: this.model.Box_Accepted_Qty
            };
            try {
                let { data } = await Repository.getRepo("InsertQasnIn", payload);
                if (data.result == "ok") {
                this.$swal("", "Successfully applied", "success")
                } else {
                    this.$swal("", data.result, "error")
                }
            }catch(error) {
                if(error.response && error.response.data) {
                this.$swal("", error.response.data.error, "error");
                }else {
                this.$swal ("", error.Message, "error")
                }
            }
        })
        }
      },
    async LoadComponent() {
        let databaseName = localStorage.databaseName;
        let PACKSLIP_NO = this.model.PACKSLIP_NO;
        try {
            let { data } = await Repository.getApiServer(`GetQASN_IN?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}`);
            this.DataTable = [];
            this.DataTable = data.data;
            console.log("this.DataTable: ", this.DataTable);
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
        }catch(error) {
            if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
            }
        }
    },
      async QuerySearch() {
        let databaseName =  localStorage.databaseName;
        let PACKSLIP_NO = this.model.PACKSLIP_NO;
        try {
            let { data } = await Repository.getApiServer(`GetQASN_IN?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}`);
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
        }catch(error) {
            if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
            }
        }
    },
    async ShowDetail(index) {
        console.log("index: ", index, "DataTable: ", this.DataTable[index].PACKSLIP_NO);
        let databaseName = localStorage.databaseName;
        // Lấy giá trị từ DataTable
        let PACKSLIP_NO = this.DataTable[index].PACKSLIP_NO;
        let FLAG = this.DataTable[index].FLAG;
        let F_ID = this.DataTable[index].F_ID;

        console.log("PACKSLIP_NO:" ,PACKSLIP_NO);
        console.log("FLAG:" ,FLAG);
        console.log("F_ID:" ,F_ID);
        try {
            let responseData = await Repository.getApiServer(`GetShowDetail?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&FLAG=${FLAG}&F_ID=${F_ID}`);
             this.ShowDataDetail = [];
             this.ShowDataDetail = responseData.data.data;
             this.ShowDataTableOuterLPN = responseData.data.dataTableOuterLPN;
             this.ShowDataTableInnerLPN = responseData.data.dataTableInnerLPN;
             this.ShowDataReceiveAddress = responseData.data.dataReceiveAddress;
             console.log("responseData", responseData);
            // this.total = data.total;
            // this.exists = data.exists;
            // this.success = data.success;
            // this.error = data.error;
            // this.errorList = data.errorList;
            if (this.ShowDataDetail.length > 0) {
            let firstItem = this.ShowDataDetail[0]; // Lấy phần tử đầu tiên
            console.log("firstItem: ", firstItem);

            // Gán giá trị vào model
            this.model.F_ID = firstItem.F_ID;
            this.model.PACKSLIP_NO = firstItem.PACKSLIP_NO;
            this.model.SHIP_MCMN_CREATTIME = firstItem.SHIP_MCMN_CREATTIME;
            this.model.MSG_SENDER_NAME = firstItem.MSG_SENDER_NAME;
            this.model.MSG_RECEIVER_NAME = firstItem.MSG_RECEIVER_NAME;
            this.model.PO_NO = firstItem.PO_NO;
            this.model.POLINE_NO = firstItem.POLINE_NO;
            this.model.ITEM_NO = firstItem.ITEM_NO;
            this.model.ITEM_SHIPPEDQTY = firstItem.ITEM_SHIPPEDQTY;
            this.model.ITEM_UNITOFMEASURE = firstItem.ITEM_UNITOFMEASURE;
            this.model.ITEM_MPN = firstItem.ITEM_MPN;
            this.model.ITEM_DESCRIPTION = firstItem.ITEM_DESCRIPTION;
            this.model.LOT_NO = firstItem.LOT_NO;
            this.model.RECEIVER_LOCATIONNAME = firstItem.RECEIVER_LOCATIONNAME;
            this.model.RECEIVER_NAME = firstItem.RECEIVER_NAME;
            //new
            this.model.AIRWAYBILL = firstItem.AIRWAYBILL;
            this.model.COUNTRY_CODE = firstItem.COUNTRY_CODE;
            this.model.POSTAL_CODE = firstItem.POSTAL_CODE;
            this.model.SHIP_MCMN_CITY = firstItem.SHIP_MCMN_CITY;
            this.model.SHIP_MCMN_STREET1 = firstItem.SHIP_MCMN_STREET1;
            this.model.SHIP_MCMN_STREET2 = firstItem.SHIP_MCMN_STREET2;
            this.model.SHIP_MCMN_STREET3 = firstItem.SHIP_MCMN_STREET3;
            this.model.SHIP_MCMN_LPN = firstItem.SHIP_MCMN_LPN;
            this.model.PAL_GROSSWEIGHT = firstItem.PAL_GROSSWEIGHT;
            this.model.PAL_NETWEIGHT = firstItem.PAL_NETWEIGHT;
            this.model.PAL_WIDTH = firstItem.PAL_WIDTH;
            this.model.PAL_LENGTH = firstItem.PAL_LENGTH;
            this.model.PAL_HEIGHT = firstItem.PAL_HEIGHT;

            this.isShowForm = true;
        } else {
            console.log("No data available in ShowDataDetail.");
            this.isShowForm = false;
        }
        }catch(error) {
            if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
            }
        }
      },
      ReturnForm() {
        this.isShowForm = false
      },
      BackToParent() {
        this.$router.push({ path: "/Home/Qualcomm_Aplication" });
      }
    }
}
  </script>
  
<style lang="scss" scoped>
    .div-all {
        margin-left: 35px;
    }
    .container {
        display: grid;
       // row-gap: 5px;
        grid-template-rows: 50px 40px 40px auto 40px 40px 40px auto 40px 40px;
        grid-template-columns: 1fr 1fr 1fr;
        align-content: space-around;
        box-sizing: border-box;
        background: #e6e6e2;
        padding: 0 20px 0px 20px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        position: relative;
        font-size: 16px;
        border-radius: 5px;
        //background-color: #cec7c7;
       // padding: 20px;
        width: 80%;
        overflow: auto;
        min-height: 450px;
        .text-input {
            width: 50%;
            padding: 5px;
            border: 1px solid #e6e6e2;
            border-radius: 5px;
            box-sizing: border-box;
            margin-bottom: 10px;
            margin-left: 2px;
            resize: vertical;
        }
        label {
            color: #141414;
            font-size: 16px;
            margin-left: 10px;
        }
        p {
            color: #141414;
        }
    }
    .title-class {
        grid-column-start: 1;
        grid-column-end: 4;
        text-align: center;
        font-size: 35px;
        color: #0e0d0d;
        font-family: serif;
        font-style: italic;
    }
    .submit-form{
       // width: 100%;
        text-align: center;
        height: 45px;
    }
    .submit-form input {
            margin-right: 5px;
        }
        input#submit-form {
            background-color: #04AA6D;
            color: #000;
            padding: 8px 12px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            &:hover {
                background-color: #45a049;
            }
        }
        input#clear-form {
            background-color: #f8c323;
            color: #000;
            padding: 8px 12px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            &:hover {
                background-color: #e0a800;
            }
        }
        input#return-form {
            background-color: #f77225;
            color: #000;
            padding: 8px 12px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            &:hover {
                background-color: #f76613;
            }
        }
    .form-row {
        display: flex;
        justify-content: space-between;
    }
    .actual-ship-from {
        display: grid;
        grid-template-rows: 40px 40px 40px 40px;
        grid-template-columns: 1fr 1fr 1fr;
        grid-column-start: 1;
        grid-column-end: 4;
        border-radius: 5px;
        position: relative;
        align-content: space-around;
        border: 2px solid #9f998b;
        margin-bottom: 5px;
        padding: 10px 0px 5px 0px;
        .form-row-actual {
            display: flex;
            justify-content: space-between;
        }
    }
    .title-actual-ship {
        position: absolute;
        top: -12px;
        left: 10%;
        transform: translateX(-50%);
        background-color: #e6e6e2;
       // padding: 0 1px;
        font-size: 16px;
        font-style: italic;
       // font-family: sans-serif;
        color: #565353;
    }
    .form-row-actual-all {
        display: grid;
        grid-template-rows: 40px 40px 40px;
        grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr;
         grid-row-start: 2;
         grid-row-end: 5;
        grid-column-start: 1;
        grid-column-end: 4;
    }

    .form-row-actual-all-label {
        grid-column-start: 1;
        grid-column-end: 2;
        grid-row-start: 1;
        grid-row-end: 4;
        display: flex;
        justify-content: flex-start;
        align-items: center;
    }

    .form-row-actual-all-input {
        grid-column-start: 2;
        grid-column-end: 6;
        display: flex;
        justify-content: flex-start;
    }
    .form-row-actual-all-input input {
        width: 80%;
        border: none;
        margin-bottom: 2px;
        border-radius: 5px;
    }

    .form-row-table {
        display: grid;
        grid-template-rows: auto;
        grid-column-start: 1;
        grid-column-end: 4;
    }
    .table-form {
        width: 100%;
        border-collapse: collapse;
        margin-bottom: 20px;
    }

    .table-form th, td {
            border: 1px solid #0f0f0f;
            padding: 5px;
            text-align: center;
            color: #0f0f0f;
        }

        .table-form th {
            color: #0f0f0f;
            font-weight: bold;
            background-color: #bbbb9c;
        }
    
    .form-row-center {
        grid-column-start: 1;
        grid-column-end: 4;
        text-align: center;
        color: #423d3d;
        font-size: 16px;
    }

    .form-row-actual-address {
        grid-column-start: 1;
        grid-column-end: 4;
    }
    .form-row-actual-address select {
        width: 80%;
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
  .div-searchbox {
    margin-top: 15px;
    height: 40px;
    display: flex;
    align-content: center;
    justify-content: left;
    .div-searchbox-content {
      display: flex;
      margin-bottom: 10px;
      text-align: center;
      height: 40px;
    }
    .div-searchbox-content input {
        border-radius: 5px;
        width: 300px;
        height: 40px;
      }
      .div-searchbox-content button {
        border-radius: 5px;
        padding: 6px 20px;
        height: 40px;
        background: #ff7a1c;
        color: #fff;
        box-sizing: 0;
        &:hover {
          background: #f88838;
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
    height: 500px;
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
        font-size: 16px;
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
        font-size: 16px;
        font-weight: 555;
      }
    }
  }
  </style>