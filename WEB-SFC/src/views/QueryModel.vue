<template>
  <div class="div-main border">
    <div class="div-top row col-12">
      <div class="row col-12">
        <div class="col row select-div" style="padding-bottom: 10px">
         
          <span class="text-title">TABLE</span> 
          <select
            v-model="chooseOption"
            name="select"
            id=""
            class="form-control form-control-sm select-form"
             >
            <option value="Z" selected>Z</option>
            <option value="R">R</option>
            <option value="H">H</option>
            <option value="ZHIS">ZHIS</option>
            <option value="RHIS">RHIS</option>
          
          </select>
        </div>
        
             <div class="col row select-div">
                <input style="margin-left: 10px"
                  class="form-check-input"
                  type="radio"
                  name="flexRadioDefault"
                  id="flexRadioDefault1"
                  v-model="sortBy"
                  value="WHS"
                  @change="onChangeSort($event)"
                  checked
                />
                <label class="form-check-label" style="margin-left: 30px" for="flexRadioDefault1">
                 WHS
                </label>
            </div>
             <div class="col row select-div">
                <input
                  class="form-check-input"
                  type="radio"
                  name="flexRadioDefault"
                  id="flexRadioDefault2"
                  v-model="sortBy"
                  value="CQA"
                  @change="onChangeSort($event)"
                />
                <label class="form-check-label" for="flexRadioDefault2">
                  CQA
                </label>
            </div>
            <div class="col row select-div">
                <input
                  class="form-check-input"
                  type="radio"
                  name="flexRadioDefault"
                  id="flexRadioDefault3"
                  v-model="sortBy"
                  value="PC"
                  @change="onChangeSort($event)"
                />
                <label class="form-check-label" for="flexRadioDefault3">
                  PC
                </label>
            </div>
           <div class="col row select-div">
                <input
                  class="form-check-input"
                  type="radio"
                  name="flexRadioDefault"
                  id="flexRadioDefault4"
                  v-model="sortBy"
                  @change="onChangeSort($event)"
                  value="STOCKIN"
                />
                <label class="form-check-label" for="flexRadioDefault4">
                  STOCKIN
                </label>
            </div>
             <div class="col row select-div">
                <input
                class="form-check-input"
                id="checkboxPeriod"
                type="checkbox"
                v-model="planCode_check"
             />
                <label for="checkboxPeriod" ><b class="titleType">PlanCode</b></label>
              </div>
            <div class="col row select-div">
     
                <input
                class="form-check-input"
                id="checkboxPeriod"
                type="checkbox"
                v-model="period"
             />
                <label for="checkboxPeriod" ><b class="titleType">Time</b></label>
              
            </div>  
          
          <div class="col row select-div" v-show ="period" id="datepick1">
         
             <span class="text-title">Date from</span>
            <datepicker
              class="form-control form-control-sm"
              v-model="dateFrom"
              :upperLimit="dateTo"
            />
          </div>
         <div class="col row select-div" v-show ="period">
               <span class="text-title">Time from</span> 
                <select
              v-model="timeFrom"
              class="form-control form-control-sm select-time"
              name=""
              id=""
                >
              <option v-for="(item, index) in listTimeFrom" :key="index">
                {{ item.VALUE }}
              </option>
            </select>
          </div>
          <div class="col row select-div" v-show ="period" id="datepick1">

               <span class="text-title">Date To</span> 
            <datepicker id="datepick1"
              class="form-control form-control-sm"
              v-model="dateTo"
              :lowerLimit="dateFrom"
            />
          </div>
          <div class="col row select-div" v-show ="period">
             <span class="text-title">Time To</span> 
            <template v-if="timeFrom == 'ALL'">
                  
              <select
                v-model="timeTo"
                class="form-control form-control-sm shadow-element"
                disabled
                name=""
                id=""
              >
                <option v-for="(item, index) in listTimeTo" :key="index">
                  {{ item.VALUE }}
                </option>
              </select>
            </template>
            <template v-else>
              <select
                v-model="timeTo"
                class="form-control form-control-sm select-time"
                name=""
                id=""
              >
                <option v-for="(item, index) in listTimeTo" :key="index">
                  {{ item.VALUE }}
                </option>
              </select>
            </template>
          </div>
    
       </div>

      </div>

     <div class="row col-12" style="padding-bottom: 10px">
        <div class="col row div-period" >
         <span class="text-title">MODEL NAME</span>
          <DropdownSearch
            class="form-control form-control-sm select-form" id="dropdowns"
            :listAll="ListModelName"
            @update-selected-item="UpdateModelReceive"
            :textContent="MODEL_NAME"
            type="model"
            textPlaceHolder="Enter MO NUMBER"
          />
        
        </div>
         <div class="col row div-period" >
         <span class="text-title">{{LABEL_MD}}</span>
          <DropdownSearch
            class="form-control form-control-sm select-form" id="dropdowns"
            :listAll="ListVersion"
            @update-selected-item="UpdateVersionReceive"
            :textContent="VERSION"
            type="model"
            textPlaceHolder="Enter version "
          />
        
        </div>
         <div class="col row div-period" >
         <span class="text-title">GROUP</span>
          <DropdownSearch 
            class="form-control form-control-sm select-form" id="dropdowns2"
            :listAll="listCategory"
            @update-selected-item="UpdateGroupReceive"
            :textContent="GROUP_NAME"
            type="model"
            textPlaceHolder="Enter MO NUMBER"
          />
        
        </div>
         <div class="col row div-period" >
         <span class="text-title">LOCATION</span>
          <DropdownSearch 
            class="form-control form-control-sm select-form" id="dropdowns2"
            :listAll="ListLocation"
            @update-selected-item="UpdateLocReceive"
            :textContent="LOCATION"
            type="model"

            textPlaceHolder="Enter Loc"
          />
        
        </div>
        <div class="col row div-period" v-show="planCode_check"  >
         <span class="text-title">PLANCODE</span>
          <DropdownSearch 
            class="form-control form-control-sm select-form" id="dropdowns2"
            :listAll="listPlanCode"
            @update-selected-item="UpdatePlanCodeReceive"
            :textContent="PLANCODE"
            type="model"

            textPlaceHolder="Enter Plancode"
          />
        
        </div>
        
        <div
        style="margin-top: 10px">

         <button @click="QuerySearch()" class="btn btn-warning" style="width: 80px">
          <Icon icon="search" class="sidenav-icon" />
        </button>
        
      </div>
    </div>
    <div class="row col-12" style="padding-bottom: 10px">
        <div class="col row div-period">
          <span class="text-title"> MO_NUMBER </span>
           <DropdownSearch
            class="form-control form-control-sm select-form" id="dropdowns1"
            :listAll="ListMONUMBER"
            @update-selected-item="UpdateMOReceive"
            :textContent="MO_NUMBER"
            type="model"
            textPlaceHolder="Enter MO"
          />
        </div>
        <div class="col row div-period">
          <span class="text-title">QTY: </span>
          <input
            class="form-control form-control-sm"
            disabled= '1==1'
            id="qty-number"
            type="number"
            v-model="moQty"

          />
        </div>
          <div class="col row div-period" >
          <div class="row col-12">
                <input
                  id="checkboxShowLine"
                  style="margin-top: 8px; margin-right: 8px"
                  type="checkbox"
                  v-model="checkboxbyMO"
                  @change="onChange()"
                />
                <label for="checkboxShowLine"
                  ><b class="titleType">By MO </b></label
                >
              </div>
        
        </div>
          <div class="col row div-period" >
          <div class="row col-12">
                <input
                  id="checkboxShowLine"
                  style="margin-top: 8px; margin-right: 8px"
                  type="checkbox"
                  v-model="checkboxMO_tech"
                   @change="onChange2()"
                />
                <label for="checkboxShowLine"
                  ><b class="titleType">MO TECH </b></label
                >
              </div>
        
        </div>
         <div class="col row div-period" >
          <div class="row col-12">
                <input
                  id="checkboxShowLine"
                  style="margin-top: 8px; margin-right: 8px"
                  type="checkbox"
                  v-model="checkboxShowLoc"
                   @change="onChange3()"
                />
                <label for="checkboxShowLine"
                  ><b class="titleType">ShowLocation </b></label
                >
              </div>
        
        </div>
        <div class="col row div-period"  v-show ="checkboxMO_tech" >
            <span class="text-title">CUSTPO</span>
           <DropdownSearch
            class="form-control form-control-sm select-form" id="dropdowns1"
            :listAll="ListCustPO"
            @update-selected-item="UpdateCustReceive"
            :textContent="CUSTPO"
            type="model"
            textPlaceHolder="Enter MO NUMBER"
          />
        
        </div>
        <div class="col row div-period" v-show="checkboxMO_tech" >
            <span class="text-title">Visible</span>
           <DropdownSearch
            class="form-control form-control-sm select-form" id="dropdowns1"
            :listAll="ListVis"
            @update-selected-item="UpdateVisReceive"
            :textContent="VISIBALE"
            type="model"
            textPlaceHolder="Enter Visiable"
          />
        
        </div>
        
        <div>
     
      </div>
    </div>
    </div>

  <div class="div-export row col-12">
    <div v-if="qmDataTable.length > 0">
      <span
        ><b class="text-count">
          {{ $store.state.language == "En" ? "Show:" : "Hiển thị" }}
          <span class="count-number">{{ qmDataTable.length }}</span>
          {{ $store.state.language == "En" ? " of " : " của " }}
          <span class="count-number">{{ qmDataTableAll.length }}</span>
          {{ $store.state.language == "En" ? "records" : "bản ghi" }}

           {{ $store.state.language == "En" ? "TotalQty:" : "Tổng Số lượng:" }}
          <span class="total-number">{{Totalqty}}</span>

          
          </b></span
      >
    </div>
    <div class="row">
      <template v-if="qmDataTable.length > 0">
        <div>
          <div>
            <img
              class="img-excel"
              @click="ExportExel()"
              src="../assets/img/excel_64.png"
              alt=""
            />
          </div>
        </div>       
      </template>
    </div>
  </div>
 <div class="row col-12 div-table">
<div class="div-table">
      <table
        id="tableMain"
        class="table table-condensed table-bordered table-sm"
        >
        <tr>
          <th
         
            v-for="(item, index) in qmDataTableHeader"
            :key="index"
          >
            {{ item }}
          </th>
        </tr>
        <template v-for="(item, index) in qmDataTable" :key="index">
          <tr :class="isSelectedRow">
            <template v-for="(item1, index1) in item" :key="index1">
              <td
                v-if="index1 == 'GROUP_NAME'"
                style="background-color: #4e7da6; color: #fff"
              
              >
                {{ item1 }}
              </td>
              <td
                v-else-if="index1 == 'WIP_GROUP' || index1 == 'CONTAINER_NO'"
                style="background-color: #ff7a1c; color: #000"
              >
                {{ item1 }}
              </td>
              <td
             
                v-else-if="
                  index1 == 'SERIAL_NUMBER' ||
                  index1 == 'MO_NUMBER' ||
                  index1 == 'MODEL_NAME' ||
                  index1 == 'SPECIAL_ROUTE'
                "
                style="background-color: #d8d4d4; color: #000"
              >
                {{ item1 }}
              </td>
              <td
                @click="GetInfoDetail(item.SERIAL_NUMBER, index1, item1)"
                v-else-if="index1 == 'TEST_CODE' || index1 == 'ERROR_DESC'"
                style="background-color: #ff7a1c; color: #000"
              >
                {{ item1 }}
              </td>
              <td
                v-else
                @click="GetInfoDetail(item.SERIAL_NUMBER, index1, item1)"
              >
                {{ item1 }}
              </td>
            </template>
          </tr>
        </template>
      </table>
    </div>
  </div>
</template>

<script>
import Repository from "../services/Repository";
import DropdownSearch from "../components/Template/DropdownSearch.vue";
import data_select_QM from "../data/data_select_TCQS";
import listTimeFrom from "../data/qmTimeFrom";
import listTimeTo from "../data/qmTimeTo";

import $ from "jquery";
export default {
  name: "QM",
  components: {
     DropdownSearch,
  },
  data() {
    return {

      LABEL_MD : 'VERSION',
        period: false,
      dateFrom: new Date(),
      dateTo: new Date(),
      qmDateFrom: "",
      qmDateTo: "",
      qmTimeFrom: "",
      qmTimeTo: "",
      MODEL_NAME : "",
      VERSION : "",
      VISIBALE : "",
      CUSTPO : "",
      GROUP_NAME: "",
      columnName: [],
      LOCATION: "",
      PLANCODE :"ALL",
      MO_NUMBER :"",
      ListModelName: ["ALL"],
      ListVersion :["ALL"],
      ListVis :["ALL","CSQ","NOCSQ"],
      ListMONUMBER :[],
      ListCustPO :[],
      listCategory: [],
      listTimeFrom: listTimeFrom,
      listTimeTo: listTimeTo,
      timeFrom: "ALL",
      timeTo: "",
      sortby: 'WHS',
      sortbyCQA : '',
      sortbyPC :'',
      sortbySTOCKIN :'',
      value: null,
      valueNull: null,
      listModelSearch: [],
      valueModel: [],
      valueMO: ["ALL"],
      valueLine: ["ALL"],
      valueGroup: ["ALL"],
      planCode_check : false,
      checkboxbyMO : false,
      checkboxMO_tech : false,
      checkboxShowLoc : false,
      dataSelectQM: [],
      oldModel: "",
      hideSelectModel: false,
      textModel: "ALL",
      listTotalQTY: [],
      listYieldRate: [],
      listPlanCode :["ALL"],
      moQTY : 0,
      qmDataTable: [],
      qmDataTableAll: [],
      qmDataTableHeader: [],
      qmDataTableHeaderAll: [],
      operate: "Manual",
      ttlRate: 0,
      fpRate: 0,
      withoutDataGroup: [],
      typeSearch: "TCQS-YieldRate",
    };
  },
  methods: {

     async GetModel() {
      var payload = {
        database: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("sp8_AllModel", payload);
      data.data.forEach((element) => {
        this.ListModelName.push(element.MODEL_NAME);
      });
    },
      async Getversion() {
       var payload = {
           database: localStorage.databaseName,
           model :  this.MODEL_NAME
                  };
       var { data } = await Repository.getRepo("sp8_QueryByModel", payload);
       if(data.version != undefined)
       {
        this.ListVersion = [];
         data.version.forEach((element) => {
           this.ListVersion.push(element.VERSION_CODE
           );
         });
       }
     },
 

   
    async GetMO() {
      var payload = {
          database: localStorage.databaseName,
          model :  this.MODEL_NAME,
          table: this.table
      };
      var { data } = await Repository.getRepo("sp8_QueryByModel", payload);
      if(data.dt_mo != undefined)
      {
        this.ListMONUMBER = [];
        data.dt_mo.forEach((element) => {
          this.ListMONUMBER.push(element.MO_NUMBER);
        });
      }
    },

    async GetCUSTMO() {
      var payload = {
          database: localStorage.databaseName,
          model :  this.MODEL_NAME,
      };
      var { data } = await Repository.getRepo("sp8_QueryByModel", payload);
       if(data.cust_po != undefined)
      {
        this.ListCustPO =[];
        data.cust_po.forEach((element) => {
        this.ListCustPO.push(element.CUST_PO);
        });
      }
    },
    async GetGroup() {

        this.listCategory.push('ALL');
        this.listCategory.push('FG');
        this.listCategory.push('SHIPPING');
        this.listCategory.push('STOCKIN');
        this.listCategory.push('REWORK');
        this.listCategory.push('CHECK_OUT');
        this.listCategory.push('FGHOLD');
        this.listCategory.push('HOLD-FG');
        this.listCategory.push('FG_HOLD');
        this.listCategory.push('FG-HOLD');
        this.listCategory.push('FG_CHECK');
        this.listCategory.push('PAHOLD');
        this.listCategory.push('BHYM');
      // var { data } = await Repository.getRepo("GetGroup_QueryModel", payload);
      // if(data.data != undefined)
      // {
      //    data.data.forEach((element) => {
      //   this.listCategory.push(element.WIPGROUP);
      // });
      // }
     
    },

    async GetPlanCode() {
      var payload = {
          database: localStorage.databaseName,
         // MODEL_NAME :  this.MODEL_NAME,
      };
      var { data } = await Repository.getRepo("GetSite_Plancode", payload);
       if(data.data != undefined)
      {
        data.data.forEach((element) => {
        this.listPlanCode.push(element.SITE);
        });
      }
    },

    UpdateMOReceive(value)
    {
      this.MO_NUMBER= value;
    },
    UpdateVersionReceive(value)
    {
     this.VERSION = value;
    },

    UpdateCustReceive(value)
    {
     this.CUSTPO = value;
    },
      UpdateVisReceive(value)
    {
     this.VISIBALE = value;
    },
    
      UpdateModelReceive(value) {
        this.MODEL_NAME = value;
        //console.log(this.MODEL_NAME.substring(0,3));
        if( (this.MODEL_NAME.substring(0,3)=='U12')
           || (this.MODEL_NAME.substring(0,6)== 'WPN802') 
          || (this.MODEL_NAME.substring(0,6)== 'WGB111'))
         {
          this.LABEL_MD= 'TA_NO';
         }
         this.GetGroup();
       this.Getversion();
      // this.get
       this.GetMO();
       this.GetCUSTMO();
       // this();
    },

     UpdateGroupReceive(value) {
        this.GROUP_NAME = value;
       
    },

    UpdatePlanCodeReceive(value){
      this.PLANCODE= value;
    }
    ,

     onChangeSort(event)
  {
    //var optionText = event.target.value;
    this.sortby= event.target.value;
   
  },
    onChange()
    {
      if(this.checkboxbyMO)
      {
        this.checkboxMO_tech= false;
      }
     
     
    },
     onChange3()
    {
       if(this.checkboxShowLoc)
      {
        this.checkboxbyMO=true;
       // this.checkboxMO_tech= false;
      }
    },

    onChange2()
   {
    if(this.checkboxMO_tech)
        {
          this.checkboxbyMO= false;
        }
   },
  
   async QuerySearch(){
     var fromdate ="";
      var to_date ="";
     // get from date 
      const dateObj = new Date(this.dateFrom);

      const year = dateObj.getFullYear();
      const month = (dateObj.getMonth()+1).toString().padStart(2, '0');
      const date = dateObj.getDate().toString().padStart(2, '0');
      

      const result = `${year}-${month}-${date}`;
      fromdate = result.toString();
  // get to_date 
 
    const dateObj2 = new Date(this.dateTo);

      const year2 = dateObj2.getFullYear();
      const month2 = (dateObj2.getMonth()+1).toString().padStart(2, '0');
      const date2 = dateObj2.getDate().toString().padStart(2, '0');
     
      const result2 = `${year2}-${month2}-${date2}`;
      to_date = result2.toString();
   // console.log("date:" + fromdate+"to_date: "+to_date)
   // GET DATA FROM ROW1 ( TABLE, KIND OF WHS, TIME)
      var string1 ='';
      string1 = this.chooseOption+",";
      if(this.sortby=='WHS')
      {
        string1 = string1 +'WHS,F,F,F'
      }
       if(this.sortby=='CQA')
      {
        string1 = string1 +'F,CQA,F,F'
      }
      if(this.sortby=='PC')
      {
        string1 = string1 +'F,F,PC,F'
      }
        if(this.sortby=='STOCKIN')
      {
        string1 = string1 +'F,F,F,STOCKIN'
      }
      if(this.planCode_check)
      {
         string1= string1 +',T';
      }
      else
      {
         string1= string1 +',F';
      }
    
      if(this.period)
      {
        string1 = string1 +',T,'+fromdate+','+this.timeFrom+','+to_date+','+this.timeTo
      }
      else
      {
        string1 = string1 +',F,F,F,F,F';
      }
    // get data row2  ( MODEL NAME, VERSION, GROUP, LOCATION)

      var string2 ='';
      if(this.planCode_check)
      {
        if(this.PLANCODE=='' || this.PLANCODE=='ALL')
        {
          string2 = this.MODEL_NAME+','+this.VERSION +','+ this.GROUP_NAME+','+ this.LOCATION+',ALL';
        }
        else
        {
            string2 = this.MODEL_NAME+','+this.VERSION +','+ this.GROUP_NAME+','+ this.LOCATION +','+this.PLANCODE;
        }
        
      }
      else{
        string2 = this.MODEL_NAME+','+this.VERSION +','+ this.GROUP_NAME+','+ this.LOCATION+',F';
      }
     
    // get data row3 ( mo_number, check bymo, motech,showloc, custpo, visiable)
      var CheckMO='';
      var CheckMOTECH='';
      var CheckShowLoc='';
      if(this.checkboxbyMO==true)
      {
        CheckMO=',T'
      }else
      {
        CheckMO=',F'
      }

      if(this.checkboxMO_tech==true)
      {
        CheckMOTECH=',T'
      }else
      {
        CheckMOTECH=',F'
      }

       if(this.checkboxShowLoc ==true)
      {
        CheckShowLoc=',T'
      }else
      {
        CheckShowLoc=',F'
      }

    var string3 ='';
    if(this.MO_NUMBER==''  || this.MO_NUMBER== undefined)
    {
      string3= 'F'+CheckMO+ CheckMOTECH+CheckShowLoc;
    }else
    {
    string3= this.MO_NUMBER +CheckMO+ CheckMOTECH+CheckShowLoc;
    }
    // cusst,PO
      if(this.checkboxMO_tech)
      {
        if(this.CUSTPO=='')
        {
          string3 =  string3+ ',F,'+this.VISIBALE;
        }
        else
        {
           string3 =  string3+ ','+this.CUSTPO+','+this.VISIBALE;
        }
         
      }
      else 
      {
       string3 =  string3+ ',F,F';
      }

      var payload = {

          database: localStorage.databaseName,
          model :  this.MODEL_NAME,
          where_fiel1 : string1,
          where_fiel2 : string2,
           where_fiel3 : string3
        };

        var { data } = await Repository.getRepo("Query_ModelContent", payload);
          this.qmDataTableAll=[];
          this.qmDataTable=[];
          this.qmDataTableHeader=[];
         console.log(data);
       // this.selectAll();
        debugger;
         var $elem = $(".div-table");
          this.qmDataTableAll = data.data;
          this.Totalqty = 0;
          this.moQTY = 0;
         
          var moQty_ =  data.mo_QTY;
          this.moQty= moQty_;
        
          data.data.forEach((element) => {
          if(element.TOTAL_COUNT)
          {
          this.Totalqty = this.Totalqty + element.TOTAL_COUNT;
          }
          if(element.SERIAL_NUMBER)
          {
            this.Totalqty= this.qmDataTableAll.length;
            //this.moQTY = this.Totalqty;
          }
          if(element.TOTAL)
          {
            this.Totalqty = this.Totalqty + element.TOTAL;
            //this.moQTY = this.Totalqty;
          }
           
          
          
      });
    
        this.qmDataTable = this.qmDataTableAll.slice(0, 500);

        if (this.qmDataTable.length != 0) {
          this.qmDataTableHeader = Object.keys(this.qmDataTable[0]);
          this.qmDataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
     $elem.scrollLeft = 15;
    },
    
    
    ExportExel() {
      const items = this.qmDataTableAll;
      const replacer = (key, value) => (value === null ? "" : value); // specify how you want to handle null values here
      const header = Object.keys(items[0]);
      const csv = [
        header.join(","), // header row first
        ...items.map((row) =>
          header
            .map((fieldName) => JSON.stringify(row[fieldName], replacer))
            .join(",")
        ),
      ].join("\r\n");

      var downloadLink = document.createElement("a");
      var blob = new Blob(["\ufeff", csv]);
      var url = URL.createObjectURL(blob);
      downloadLink.href = url;
      downloadLink.download = "data.csv";

      /*
       * Actually download CSV
       */
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
      // console.log(csv);
    },
   
    
  },
  mounted() {
    this.GetModel();
    //this.GetPlanCode();
    this.chooseOption='Z';
    this.MODEL_NAME='ALL';
    this.VERSION= 'ALL';
    this.GROUP_NAME='ALL';
    this.LOCATION ='ALL';
    this.CUSTPO='ALL';
    this.VISIBALE= 'ALL';

    document.title = "Query Model";
    this.dataSelectQM = data_select_QM;
    this.timeFrom = "00:00";
    this.timeTo = "00:30";
    this.$store.commit("RefreshState");

   // this.get 
  },
};
</script>

<style lang="scss" scoped>
* {
  color: #000;
}

@media only screen and (hover: none) and (pointer: coarse) {
  .text-title {
    font-size: 10px !important;
  }
}


.btn_select_item {
  font-size: 13px;
  margin-top: 10px;
  border-radius: 10px 10px 0 0;
  background-color: #024873;
  color: #fff;
  appearance: none;
  outline: none;
  border: none;
}
.arrow-hide {
  position: absolute;
  right: 5px;
  bottom: 5px;
  border-radius: 100%;
  cursor: pointer;
}
.form-check-label-menu {
  color: #fff;
  font-size: 13px;
  font-weight: 555;
}
.text-without-group {
  font-size: 12px;
  font-weight: 555;
}
.div-rate-value {
  span {
    font-weight: 555;
  }
  border-right: 1px solid #cdc;
  border-left: 1px solid #cdc;
  border-bottom: 1px solid #cdc;
}
.control-type {
  background: #cdc;
  color: #000;
}
.div-all-serial {
  background: #77281a;
  label {
    font-size: 12px;
    color: #32e809;
  }
  span {
    font-size: 12px;
    color: #32e809;
  }
}
.div-pass-rate {
  background: #0000c4;
  label {
    font-size: 12px;
    color: yellow;
  }
  span {
    font-size: 12px;
    color: yellow;
  }
}
.div-group-data {
  max-height: 300px;
  width: 100%;
  overflow: auto;
  ul li {
    list-style: none;
  }
}
.div-without-data {
  padding: 0.5rem 1.5rem;
  background: #0b7c66;
  margin-top: 10px;
  span {
    color: yellow;
  }
}
.text-select-QM {
  font-family: "Open Sans", sans-serif !important;
  font-size: 13px !important;
}
.select-time {
  option {
    background: #024873;
    color: #fff;
  }
}
.div-selected {
  color: #fffbf7;
  display: flex;
  justify-content: space-between;
  position: relative;
  user-select: none;
}
.icon_check {
  color: #fff;
  height: 12px;
  width: 12px;
  margin: 0;
  position: absolute;
  top: 50%;
  right: 5px;
  -ms-transform: translateY(-50%);
  transform: translateY(-50%);
}
.third-level-menu {
  position: absolute;
  top: 0;
  right: -150px;
  width: 150px;
  list-style: none;
  padding: 0;
  margin: 0;
  display: none;
}
.third-level-menu > li {
  // height: 30px;
  background: #15628e;
}
.third-level-menu > li:hover {
  background: #0b537a;
}

.second-level-menu {
  li {
    border-bottom: 0.1px solid #666666;
    border-left: 0.1px solid #666666;
    border-right: 0.1px solid #666666;
  }
  position: absolute;
  top: 30px;
  left: 0;
  width: 150px;
  list-style: none;
  padding: 0;
  margin: 0;
  display: none;
}

.second-level-menu > li {
  position: relative;
  height: 30px;
  background: #15628e;
}
.second-level-menu > li:hover {
  background: #0b537a;
}

.top-level-menu {
  list-style: none;
  padding: 0;
  margin: 0;
}

.top-level-menu > li {
  position: relative;
  float: left;
  height: 30px;
  width: 150px;
  background: #15628e;
}
.top-level-menu > li:hover {
  background: #0b537a;
}

.top-level-menu li:hover > ul {
  /* On hover, display the next level's menu */
  display: inline;
}

/* Menu Link Styles */

.top-level-menu a /* Apply to all links inside the multi-level menu */ {
  font: bold 14px Arial, Helvetica, sans-serif;
  color: #ffffff;
  text-decoration: none;
  padding: 0 0 0 10px;

  /* Make the link cover the entire list item-container */
  display: block;
  line-height: 30px;
}
.top-level-menu a:hover {
  color: #fff;
}
.top-left {
  margin-top: 0px;
}

.div_select_item {
  height: 60px;
  ul {
    border-radius: 0 0 10px 10px;
    border: 1px solid #9e9e9e;
    padding: 0;
    width: 100%;
    height: 60px;
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

.tableDefectSerialAll {
  width: 100%;
  thead {
    th {
      background-color: #024873;
      position: sticky;
      top: 0;
      z-index: 2;
      color: #fff;
      border: 1px solid #fff;
      padding: 3px;
      font-size: 13px;
    }
  }
  tr {
    td {
      z-index: 1;
      padding: 2px;
      border: 0.5px solid #cdc;
      font-size: 13px;
      overflow: hidden;
      white-space: nowrap;
    }
  }
}
.tableDefectSerial {
  border: 1px solid red;
  thead {
    th {
      background-color: #024873;
      position: sticky;
      top: 0;
      z-index: 2;
      color: #fff;
      border: 1px solid #fff;
      min-width: 60px;
      font-size: 13px;
    }
  }
  tr {
    td {
      z-index: 1;
      padding: 2px;
      border: 0.5px solid #cdc;
      font-size: 13px;
      overflow: hidden;
      white-space: nowrap;
    }
  }
}

.mytable {
  // table-layout: fixed;
  margin-top: 10px;
  overflow: auto;
  width: 100%;
  thead {
    // box-shadow: 2px 2px 2px 2px rgba(0, 0, 0, 0.4);
    //border: 1px solid red;

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

      //border: 0.5px solid #cdc;
    }
  }
  tr {
    td {
      overflow-x: auto;
      white-space: nowrap;
      z-index: 1;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 13px;
    }
  }
}
.text-count {
  font-size: 13px;
  .count-number {
    color: #4e7da6;
  }
}
.btnExport {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;
}
.tr-green {
  background-color: #34790c;
  color: #f7e031;
}
.tr-yellow {
  background-color: #e2cb6c;
  color: #000;
}
.hover-tr {
  &:hover {
    background-color: #b1c3df;
    td {
      color: #000;
    }
  }
}
.div-export {
  margin-top: 10px;
  margin-left: 20px;
  display: flex;
  justify-content: space-between;
}
.img-chart {
  height: 40px;
  width: 40px;
  margin-right: 10px;
  cursor: pointer;
  &:hover {
    box-shadow: 0 3px 10px rgb(0 0 0 / 0.5);
  }
}
.img-excel {
  height: 40px;
  width: 40px;
  margin-right: 50px;
  cursor: pointer;
  &:hover {
    box-shadow: 0 3px 10px rgb(0 0 0 / 0.5);
  }
}
.tr_yellow {
  background-color: #d6b619;
  td {
    color: #000;
  }
}
.tr-red {
  background-color: #bc2525;
  td {
    color: #fff;
  }
}

.div-table {
  min-height: 76vh;
  max-height: 76vh;
}

.div-table {
 
  width: 100%;
    max-height: 350px;
  
  overflow: auto;
  table tr th {
    position: sticky;
    top: 0;
    z-index: 2;
    overflow-x: auto;
    white-space: nowrap;
    table tr td {
      overflow-x: auto;
      white-space: nowrap;
    }
  }
}
.table-condensed {
  font-size: 12px;
  margin-top: 10px;
  tr {
    td {
      font-weight: 550;
    }
  }
}
.table-condensed tr th {
  background: #418bca;
  color: #fff;
}
.checkbox-group:first-child {
  margin-top: 3px;
}
.checkbox-group:not(:first-child) {
  margin-top: 3px;
  margin-left: 50px;
  text-align: center;
}
.label-group {
  font-size: 13px;
  user-select: none;
  margin-left: 10px;
  color: #fffbf7;
  font-weight: 600;
  &:hover {
    cursor: pointer;
  }
}
.text-element {
  cursor: pointer;
}
.text-title {
  font-size: 13px;
  color: #8af6f0;
  font-weight: 550;
}
.option_active {
  background-color: #ff7a1c !important;
  color: #fffbf7 !important;
}
.shadow-element {
  box-shadow: 1px 2px 2px #fffbf7;
}
.div-option-uncheck {
  text-align: center;
  user-select: none;
  background-color: #a0d0ff;
  box-shadow: 1px 2px 2px #4c4c4c;
  border-radius: 3%;
  margin: 5px;
  padding: 5px 20px;
  color: #000f15;
  &:hover {
    cursor: pointer;
  }
}
.button {
  margin-left: 20px;
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;

  display: inline-block;
  padding: 10px 10px;
  background-image: linear-gradient(to right, #0f56c1, #588cd8);
  border-radius: 8px;

  color: #fff;
  font-size: 18px;
  font-weight: 700;

  box-shadow: 2px 2px rgba(0, 0, 0, 0.4);
  transition: 0.4s ease-out;

  &:hover {
    box-shadow: 4px 4px rgba(0, 0, 0, 0.6);
  }
}
.div-main {
  background: #4E7DA6 !important;
  border: 1px solid red;
}
.total-number{
  color: red;
}
.div-top {
  min-height: 35px;
  // padding-top: 5px;
  // padding-bottom: 5px;
  // display: flex;
  // justify-content: space-between;
}
.col-3{
   z-index: 200;
}
#dropdowns{
  z-index: 99;
}
#dropdowns1{
  z-index: 98;
}
#datepick1
{
 z-index: 200;
}
#dropdowns2
{
  z-index: 100;
}
#qty-number
{
  z-index: 5;
  background-color: blanchedalmond;
   font-size: 15px;
   font-weight: bold;

  color: red;
  //width: 120px;
}
</style>
