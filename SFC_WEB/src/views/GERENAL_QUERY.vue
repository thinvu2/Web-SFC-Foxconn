<template>
  <div class="content_main">
    <div class="div-content-top">
      <div v-show="!isHideMenu" class="row col-12">
        <table class="col-9">
          <tr class="row" style="margin-bottom: 5px; margin-left: 2px">
             <td class="col-3" >
              <div class="row">          
                <label  class="text-element"  for="validationDefault02">TYPE</label>
                <DropdownSearch
                  class="form-control form-control-sm col-sm-15 text-element"
                  :listAll="listType"
                  @update-selected-item="UpdateType"
                  :textContent="model.TYPE"
                  type="model"
                  textPlaceHolder="Enter TYPE"
                />
              </div>
            </td>
            <td class="col-3" >
              <div class="row">
                <label class="text-element" for="validationDefault02">ID</label>
                  <input
                    v-model="textListSearch"
                    class="form-control form-control-sm col-sm-15 text-element"
                >
              </div>
            </td>
          </tr>
        </table>
        <div class="col-2 div-search">
          <button @click="queryShiptoFile" class="btn-query btn col-12">
            <span class="title-button-search">{{
              $store.state.language == "En" ? "Query" : "Tìm"
            }}</span>
          </button>
        </div>
     </div>
    </div>
    <!-- <div class="div-export row col-12">
      <div></div>      
    </div> -->
    <div class="col-12 row" v-if="listResult.length > 0">
          <div class="col-3" style="top:30px">
          <div class="content-top">
            <b>
              {{ $store.state.language == "En" ? "Show:" : "Hiển thị" }}
              <span class="count-number">{{
               Query_qty
              }}</span>
              {{ $store.state.language == "En" ? " of " : " của " }}
              <span class="count-number">{{
                moQTY
              }}</span>
              {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
            >            
           </div>
           </div>
          <div class="col-3" style="top:30px">
           <b> {{ $store.state.language == "En" ? "Query Time" : "Thời gian" }}
              <span class="count-number">{{
                executeTime
              }}</span>
              </b>
          </div>
          <div class="col-5">
            </div>

      <div class="pull-right" v-if="listResult.length > 0">
        
      </div> 
      <div class="float-right" style ="margin-top:10px">      
        <img @click="exportExcel()"
        class="img-excel"
        src="../assets/img/xlsx-icon.jpg"
        />
        <img
        @click="exportExcelCSV()"
        class="img-excel"
        src="../assets/img/excel_64.png"
        alt=""
        />
      </div>
      
    </div>
    <div class="div-table">

      <template v-if="listHeader">
        <table
          id="tableMain"
          class="mytable table table-condensed table-bordered table-sm"
          ref="exportable_table"
        >
          <tr>
            <th
              @click="subOderBy(item)"
              v-for="(item, index) in listHeader"
              :key="index"
            >
              {{ item }}
            </th>
          </tr>
          <template v-for="(item, index) in limitResults" :key="index">
            <tr
              @dblclick="GetLinkData(item.SERIAL_NUMBER)"
              :class="isSelectedRow"
            >
              <template v-for="(item1, index1) in item" :key="index1">            
                  <td><b>{{ item1 }}</b></td>
              </template>
            </tr>
          </template>
        </table>
      </template>
    </div>
    <!-- <table class="table">
        
      </table> -->
  </div>
</template>

<script>
import Repository from "../services/Repository";
import DropdownSearch from "../components/Template/DropdownSearch.vue";

function swap(arra) {
  [arra[0], arra[arra.length - 1]] = [arra[arra.length - 1], arra[0]];
  return arra;
}
import xlsx from 'xlsx';
export default {  
  components: {
    DropdownSearch,
  },
  name: "GERENAL QUERY",
  created() {},
  data() {
    return {
      isHideMenu: false,
      timer: "",
      textListSearch: "",
      isORTWip: false,
      isLink: false,
      isFG: false,
      moSearch: "ALL",
      modelSearch: "ALL",
      sectionSearch: "ALL",
      motypeList: [],
      moList: [],
      modelList: [],
      sectionList: [],
      motypeShow: false,
      moShow: false,
      modelShow: false,
      sectionShow: false,
      basemotypeList: [
        { VALUE: "ALL" },
        { VALUE: "CONSIGNED" },
        { VALUE: "CONTROL RUN" },
        { VALUE: "NORMAL" },
        { VALUE: "OTHER" },
        { VALUE: "PILOT RUN" },
        { VALUE: "REWORK" },
        { VALUE: "RMA" },
        { VALUE: "SUBCONTRACT" },
      ],
      model:{
        database_name: localStorage.databaseName,
        TYPE: "",
        ID: "",
      },
      moNumberList: [],
      modelNameList: [],
      sectionNameList: [],
      checkboxShowLine: false,
      sortBy: "mo",
      moType: "All",
      listResult: [],
      listHeader: [],
      listType: [],
      showModal: false,
      listDetailClick: [],
      isShowModal: false,
      isDisableSection: true,
      moQTY: 0,
      modelQTY: 0,
      executeTime: "",
      limit: 250,  
      Query_qty:0,    
    };
  },
  computed:{
    limitResults(){
      return this.listResult.slice(0,this.limit);
    }
  },
  keyUpData() {
      console.log(this.textListSearch);
    },
    hideModal() {
      this.textListSearch = "";
      this.$emit("hideModal");
      this.$store.state.listDetailClick = [];
    },
  mounted() {
    document.title = "GERENAL QUERY";
    this.motypeList = this.basemotypeList;
    this.$store.commit("RefreshState");
    this.GetType();
  },
  unmounted() {
    ////this.cancelAutoUpdate();
  },
  methods: {       
    UpdateType(value) {
      this.model.TYPE = value;  
    },   
    async GetType() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Type_Query_ID", payload);
      data.data.forEach((element) => {
        this.listType.push(element.OUTPUT);
      });
    }, 
    exportexcelxlsx(type, fn, dl){
      //var elt = this.$refs.exportable_table;
      var elt = this.listResult;
      var wb = xlsx.utils.table_to_book(elt, {sheet:"Sheet JS"});
      console.log(elt)
      return dl ?
      xlsx.write(wb, {bookType:type, bookSST:true, type: 'base64'}) :
      xlsx.writeFile(wb, fn || (( 'data.'|| 'SheetJSTableExport.') + (type || 'xlsx')));
    },
    exportExcelCSV() {
      const items = this.listResult;
      const replacer = (key, value) => (value === null ? "" : "\u200C" + value); // specify how you want to handle null values here
      
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
    exportExcel() {
      const items = this.listResult;
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
    MenuToggle() {
      this.isHideMenu = !this.isHideMenu;
    },
    HideModal() {
      this.motypeShow = false;
    },
    subOderBy(key) {
      this.sortByKey(this.subDataTable, key);
    },
    ExportExel() {
      var tab_text = "<table border='2px'><tr bgcolor='#418BCA'>";
      var j = 0;
      var tab = document.getElementById("tableMain"); // id of table

      for (j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
      }
      tab_text = tab_text + "</table>";
      tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
      tab_text = tab_text.replace(/<img[^>]*>/gi, "");
      tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");
      var sa = window.open(
        "data:application/vnd.ms-excel," + encodeURIComponent(tab_text)
      );
      return sa;
    },
    hideModal() {
      this.isShowModal = false;
    },
    
    async ClickSearch(type) {
      var payload = {
        database_name: localStorage.databaseName,
        type: type,
        isNotLink: this.isLink,
        mo_value: this.$store.state.listSelectDualMO,
        model_value: this.$store.state.listSelectDualModel,
        motype_value: this.motypeSearch,
        section_value: this.sectionSearch,
      };

      var { data } = await Repository.getRepo("GetShipping", payload);
      if (data.result == "ok") {
        if (type == "model") {
          this.$store.commit("UpdateListSelectModel", data.data);
          this.$store.commit("showModalModel");
        } else if (type == "section") {
          this.sectionList = data.data;
          this.sectionList.push({ VALUE: "ALL" });
          swap(this.sectionList);
          this.sectionShow = true;
        } else if (type == "mo") {
          this.$store.commit("UpdateListSelectMO", data.data);
          this.$store.commit("showModalMO");
        }

        


      } else {
        if (type == "model") {
          this.moList = [];
          this.moShow = false;
        } else if (type == "section") {
          this.moList = [];
          this.moShow = false;
        } else if (type == "mo") {
          this.$store.commit("UpdateListSelectMO", []);
          this.moList = [];
          this.moShow = false;
        }
      }
    },
    motypeClickInput() {
      if (!this.motypeShow) {
        this.motypeShow = true;
        this.moShow = false;
        this.modelShow = false;
        this.sectionShow = false;
      }
    },
    modelClick() {
      this.ClickSearch("model");
    },
    moClick() {
      this.ClickSearch("mo");
    },
    sectionClickInput() {
      if (!this.sectionShow) {
        this.sectionSearch = "";
        this.sectionShow = true;
        this.motypeShow = false;
        this.moShow = false;
        this.modelShow = false;
      }
    },
    async queryShiptoFile() {
        
      if(this.model.TYPE == "" || this.model.TYPE == null){
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      }else{

        var payload = {
          database_name: localStorage.databaseName,
          VR_NAME : this.model.TYPE,
          VR_VALUE: this.textListSearch
        };

        var { data } = await Repository.getRepo("Query_ID", payload);
        this.motypeShow = false;
        this.moShow = false;
        this.modelShow = false;
        this.sectionShow = false;
        if (data.result == "ok") {

          this.listResult = data.data;
          
          this.moQTY = data.moQTY;
          this.modelQTY = data.modelQTY; 
          this.executeTime = data.queryTime;
          this.listHeader = Object.keys(this.listResult[0]).filter(
            (p) => p != "RED"
          );
          this.Query_qty = parseInt(this.moQTY) < this.limit ? this.moQTY:this.limit;
        } else {
          if (localStorage.language == "En") {
            this.$swal("", "No records", "error");
          } else {
            this.$swal("", "không có dữ liệu", "error");
          }
          this.moList = [];
          this.moShow = false;
          this.listResult = [];
          this.listHeader = [];
        }
        }

    },
    lostFocusMo() {
      if (this.moShow) {
        console.log("showing");
      }
    },
    lostFocusModel() {
      console.log("lostFocusModel");
    },
    lostFocusSection() {
      console.log("lostFocusSection");
    },
    selectKeyMoType(type) {
      console.log("selectKeyMoType");
      this.motypeSearch = type;
      this.motypeShow = false;
    },
    selectKeyMo(type) {
      console.log("selectKeyMo" + type);
      this.moSearch = type;
      this.moShow = false;
    },
    selectKeyModel(type) {
      console.log("selectKeyMoType" + type);
      this.modelSearch = type;
      this.modelShow = false;
    },
    selectKeySection(type) {
      console.log("selectKeyMoSection" + type);
      this.sectionSearch = type;
      this.sectionShow = false;
    },
    lostFocusMoType() {},
  },
};
</script>

<style scoped lang="scss">
.div-content-top {
  background: #4e7da6;
  padding-top: 10px;
  // position: relative;
}
@media only screen and (hover: none) and (pointer: coarse) {
  //For mobile screen
  .arrow-hide {
    display: block !important;
  }
  .titleType {
    font-size: 10px !important;
  }
  .form-check-label {
    font-size: 10px !important;
  }
  .label-group {
    font-size: 10px !important;
  }
  .div-top {
    min-height: 35px;
  }
  .title-button-search {
    font-size: 10px !important;
  }
  .mytable {
    tr {
      td {
        font-size: 10px !important;
      }
    }
    thead {
      tr {
        th {
          font-size: 10px !important;
        }
      }
    }
  }
  .text-total {
    font-size: 10px !important;
  }

  //Mobile
  .mytable {
    //table-layout: fixed;
    tr {
      th {
        min-width: 40px !important;
      }
      th:nth-child(1) {
        min-width: 80px !important;
        left: 0px;
      }
      th:nth-child(2) {
        min-width: 120px !important;
        max-width: 120px !important;
        left: 80px !important;
      }
      th:nth-child(3) {
        position: sticky;
        min-width: 60px !important;
        max-width: 60px !important;
        left: 200px !important;
      }
      th:nth-child(4) {
        min-width: 60px !important;
        max-width: 60px !important;
        left: 260px !important;
      }
      th:nth-child(5) {
        min-width: 40px !important;
        max-width: 40px !important;
        left: 320px !important;
      }
      th:nth-child(6) {
        min-width: 50px !important;
        max-width: 50px !important;
        left: 360px !important;
      }
    }
    tr {
      td {
        min-width: 40px;
      }
    }
    tr {
      td:nth-child(1) {
        min-width: 80px !important;
        left: 0px;
      }
      td:nth-child(2) {
        min-width: 120px !important;
        max-width: 120px !important;
        left: 80px !important;
      }
      td:nth-child(3) {
        position: sticky;
        min-width: 60px !important;
        max-width: 60px !important;
        left: 200px !important;
      }
      td:nth-child(4) {
        min-width: 60px !important;
        max-width: 60px !important;
        left: 260px !important;
      }
      td:nth-child(5) {
        min-width: 40px !important;
        max-width: 40px !important;
        left: 320px !important;
      }
      td:nth-child(6) {
        min-width: 50px !important;
        max-width: 50px !important;
        left: 360px !important;
      }
    }
  }
}
.arrow-hide {
  display: none;
}
.titleType {
  font-size: 13px;
  color: #fffbf7;
}
.form-check-label {
  font-size: 13px;
  color: #fffbf7;
}
.label-group {
  cursor: pointer;
  font-size: 13px;
  user-select: none;
  margin-left: 20px;
  margin-right: 10px;
  color: #fff;
}

.title-button-search {
  font-size: 13px;
}

.div-top {
  position: relative;
  // min-height: 9vh;
}
.arrow-hide {
  //display: none;
  position: absolute;
  right: 5px;
  bottom: 5px;
  border-radius: 100%;
  cursor: pointer;
}
.text-total {
  font-size: 13px;
  font-weight: 555;
}
.text-total-count {
  color: #e73131;
}
.text-element {
  color: #000;
  font-weight: bold;
}
.div-export {
  display: flex;
  justify-content: space-between;
}
.img-excel {
  height: 40px;
  width: 40px;
  //margin-right: 30px;
  cursor: pointer;
}
.mytable {
  //table-layout: fixed;
  tr {
    th {
      position: sticky;
      top: 0;
      z-index: 2;
      background-color: #024873;
      color: #fff;
      min-width: 60px;
      padding: 3px;
      font-size: 13px;
      border: 0.5px solid #cdc;
    }
    th:nth-child(1) {
      position: -webkit-sticky;
      position: sticky;
      z-index: 3 !important;
      min-width: 100px;
      left: 0px;
    }
    th:nth-child(2) {
      position: -webkit-sticky;
      position: sticky;
      z-index: 3 !important;
      min-width: 150px;
      left: 100px;
    }
    th:nth-child(3) {
      position: -webkit-sticky;
      position: sticky;
      border: 0.5px solid #cdc;
      z-index: 3 !important;
      min-width: 80px;
      max-width: 80px;
      left: 250px;
    }
    th:nth-child(4) {
      position: -webkit-sticky;
      position: sticky;
      border: 0.5px solid #cdc;
      z-index: 3 !important;
      min-width: 80px;
      max-width: 80px;
      left: 330px;
    }
    th:nth-child(5) {
      position: -webkit-sticky;
      position: sticky;
      border: 0.5px solid #cdc;
      z-index: 3 !important;
      min-width: 80px;
      max-width: 80px;
      left: 410px;
    }
    th:nth-child(6) {
      position: -webkit-sticky;
      position: sticky;
      border: 0.5px solid #cdc;
      z-index: 3 !important;
      min-width: 80px;
      max-width: 80px;
      left: 490px;
    }
  }
  tr {
    td {
      z-index: 1;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 13px;
    }
  }
  tr {
    td:nth-child(1) {
      z-index: 2 !important;
      position: -webkit-sticky;
      position: sticky;
      background: #fff;
      border: 0.5px solid #cdc;
      min-width: 100px;
      left: 0px;
    }
    td:nth-child(2) {
      z-index: 2 !important;
      position: -webkit-sticky;
      position: sticky;
      background: #fff;
      border: 0.5px solid #cdc;
      min-width: 150px;
      left: 100px;
    }
    td:nth-child(3) {
      z-index: 2 !important;
      position: -webkit-sticky;
      position: sticky;
      background: #fff;
      border: 0.5px solid #cdc;
      min-width: 80px;
      max-width: 80px;
      left: 250px;
    }
    td:nth-child(4) {
      z-index: 2 !important;
      position: -webkit-sticky;
      position: sticky;
      background: #fff;
      border: 0.5px solid #cdc;
      min-width: 80px;
      max-width: 80px;
      left: 330px;
    }
    td:nth-child(5) {
      z-index: 2 !important;
      position: -webkit-sticky;
      position: sticky;
      background: #fff;
      border: 0.5px solid #cdc;
      min-width: 80px;
      max-width: 80px;
      left: 410px;
    }
    td:nth-child(6) {
      z-index: 1 !important;
      position: -webkit-sticky;
      position: sticky;
      background: #fff;
      border: 0.5px solid #cdc;
      min-width: 80px;
      max-width: 80px;
      left: 490px;
    }
  }
}
.div-search {
  display: flex;
  justify-content: center;
  align-content: center;
  align-items: center;
  //height: 70px;
  //line-height: 70px;
  .btn-query {
    text-align: center;
    vertical-align: middle;
    background-color: #ff7a1c;
    //padding: 5px 30px;
    color: #fff;
    &:hover {
      background-color: #e85f04;
    }
  }
}

.div-table {
  min-height: 76vh;
  max-height: 76vh;
}
.div-table table thead tr th {
  overflow-x: auto;
  white-space: nowrap;
}
.div-table table tr td {
  overflow-x: auto;
  white-space: nowrap;
}
.div-table {
  overflow: auto;
}
.table-condensed {
  font-size: 12px;
  margin-top: 10px;
}
.table-condensed thead {
  background: #418bca;
  color: #fff;
}
.red b {
  color: #fff;
}
.black b {
  color: #000;
}

.content_main {
  margin-top: 0;
  //overflow: auto;
  // height: 84vh;
  // margin-left: 20px;
}
#scroll-area {
  width: 500px;
  height: 500px;
}

#example-content {
  width: 2000px;
  height: 2000px;
}

.div-select {
  height: 200px;
  overflow-y: auto;
}
.footer-navigation {
  list-style: none;
  padding-left: 0px;
  display: inline-block;
  width: 100%;
}
.footer-navigation li {
  width: 100%;
  box-sizing: border-box;
}
a {
  text-decoration: none;
}
.footer-navigation a {
  padding: 5px;
  color: #000000;
  display: block;
  border: 1px solid #000;
  background-color: #fac900;
}
.footer-navigation a:hover {
  padding: 5px;
  color: #ffffff;
  background-color: #fac900;
}
.count-number {
  color: red;
}
</style>
