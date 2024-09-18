<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowQ6QueryList"
        @click="hideModal()"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowQ6QueryList">
        <div class="div-top content-top">
          <b>
            {{
              $store.state.language == "En"
                ? "Query list data"
                : "Tìm kiếm danh sách"
            }}</b
          >
          <span class="closeButton" @click="$emit('hideModal')"
            ><Icon style="font-size: 23px; color: red" icon="times-circle"
          /></span>
        </div>
        <div class="content-top col-sm-12">
          <textarea
            v-model="textListSearch"
            class="form-control col-sm-5"
          ></textarea>
          <select
            @keyup="keyUpData()"
            v-model="conditionSearch"
            class="select-condition form-control form-control-sm col-sm-2"
          >
            <option value="WIP" selected>WIP</option>
            <option value="DETAIL">Detail</option>
            <option value="DETAILSSN">Detail SSN</option>
            <option value="CUSTSN_DATA">Cust SN Data</option>
            <option value="DATA_INPUT">DATA INPUT</option>
            <option value="REPAIR_DATA">Repair data</option>
            <option value="LINK_DATA(SN)">Link Data(SN)</option>
            <option value="LINK_DATA(KP)">Link Data(KP)</option>
            <option value="HISTORY_LINK_DATA(SN)">History Link Data(SN)</option>
            <option value="HISTORY_LINK_DATA(KP)">History Link Data(KP)</option>
            <option value="LINK_H(SN)">Link H(SN)</option>
            <option value="LINK_H(KP)">Link H(KP)</option>
            <option value="DN(SHIP_NO)">DN (Ship No)</option>
            <option value="FG(SERIAL_NUMBER)">FG (Serial number)</option>
            <option value="FG(SHIPPING_SN)">FG (Shipping SN)</option>
            <option value="R_SYSTEM_HOLD_T">Hold Reason</option>
            <!--<option value="VSC1000">OBA PTM File VSC1000</option>-->
            <option value="VSC2000">Verisure</option>
            <option value="LINKED_WIP_SI">LINKED_WIP_SI</option>
            <option value="SI_PRODUCTION_IN_LINE">SI_PRODUCTION_IN_LINE</option>
            <option value="FAIL_HISTORY">FAIL_HISTORY</option>
            <option value="Labelroom_Data">Labelroom Data</option>
            <option value="Tru_PGI">Trừ PGI</option>
            <option value="BC9M">BC9M</option>
            <option value="RATE_SMT_ER">RATE_SMT_ER</option>
            <option value="Detail_Link_Sensor">Detail_Link_Sensor</option>
            <option value="Link_Main_History">Link_Main_History</option>
            <option value="RFID_DETAIL">RFID_DETAIL</option>
            <option value="DETAIL_LINK_OOBA1">DETAIL_LINK_OOBA1</option>
            <option value="REWORK_NO">REWORK_NO</option>
             <option value="BC8M">BC8M InOut</option>
        </select>
          <button
            class="btn btn-primary col-sm-1 btn-search"
            @click="SearchList()"
          >
            <Icon icon="search-plus" class="sidenav-icon" />
          </button>
        </div>
        <template v-if="$store.state.listDetailClick.length > 0">
          <div class="content-top-export row">
            <b>
              {{ $store.state.language == "En" ? "Show:" : "Hiển thị" }}
              <span class="count-number">{{
                $store.state.listDetailClick.length
              }}</span>
              {{ $store.state.language == "En" ? " of " : " của " }}
              <span class="count-number">{{
                $store.state.listDetailClickAll.length
              }}</span>
              {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
            >
            <div class="float-right">
                 <img @click="exportexcelxlsx()"
              class="img-excel"
              src="../../assets/img/xlsx-icon.jpg"

              />
                <img
                @click="exportExcelCSV()"
                class="img-excel"
                src="../../assets/img/excel_64.png"
                alt=""
              />
              <img src="../../assets/img/txt.png"
              @click="exporttxt()"
              class="img-excel"
              alt=""
              />

           
           </div>
            
            <!-- <vue-excel-xlsx
              class="btnExport"
              :data="$store.state.listDetailClickAll"
              :columns="$store.state.listDetailClickHeader"
              :filename="'data'"
              :sheetname="'luatdz'"
            >
              <img
                class="img-excel"
                src="../../assets/img/excel_64.png"
                alt=""
              />
            </vue-excel-xlsx> -->
          </div>
          <div class="div-table">
            <!-- <smooth-scrollbar>  -->
            <table
              id="tableMain"
              class="table table-condensed table-bordered table-sm"
            >
              <thead>
                <tr>
                  <th
                    @click="OderBy(item)"
                    v-for="(item, index) in Object.keys(
                      $store.state.listDetailClick[0]
                    )"
                    :key="index"
                  >
                    <b> {{ item }}</b>
                  </th>
                </tr>
              </thead>
              <tr
                v-for="(item, index) in $store.state.listDetailClick"
                :key="index"
              >
                <td v-for="(itemContent, index1) in item" :key="index1">
                  <b>{{ itemContent }}</b>
                </td>
              </tr>
            </table>
            <!-- </smooth-scrollbar>  -->
          </div>
        </template>

        <!-- <button class="button" @click="$emit('hideModal')">Close Modal</button> -->
      </div>
    </transition>
  </div>
</template>
<script>
import xlsx from 'xlsx';
import Repository from "../../services/Repository";
export default {
  props: {},
  components: {},
  data() {
    return {
      conditionSearch: "WIP",
      textListSearch: "",
      showModal: false,
      isASC: false,
      listDetail: [],
    };
  },
  methods: {
    keyUpData() {
      console.log(this.textListSearch);
    },
    hideModal() {
      this.textListSearch = "";
      this.$emit("hideModal");
      this.$store.state.listDetailClick = [];
    },
    async SearchList() {
      if (this.textListSearch.length != 0) {
        var listInput = [];
        var texter = "";
        this.textListSearch.split(/\n/).forEach((element) => {
          if (
            element.trim().length > 20 &&
            element.trim().indexOf(":") !== -1
          ) {
            element = element
              .trim()
              .substring(element.trim().length - 17)
              .replace(/:/g, "");
            texter += element + "\n";
          } else {
            texter += element + "\n";
          }
          if (texter != "") {
            this.textListSearch = texter;
          }
          listInput.push({ input: element.trim().replace(/[\u200B-\u200D\uFEFF]/g,'') });
        });

        var payload = {
          database: localStorage.databaseName,
          table: this.conditionSearch,
          list_input: listInput,
        };
        var { data } = await Repository.getRepo("QueryList", payload);
        console.log("data: " +data);
        if (data.result == "ok") {
          this.$store.state.listDetailClickAll = data.data;
          this.$store.state.listDetailClick = this.$store.state.listDetailClickAll.slice(0, 500);
            if (data.data.length > 0) {
            this.$store.state.isShowQ6QueryList = true;
            var arr = [];
            Object.keys(this.$store.state.listDetailClick[0]).forEach(
              (element) => {
                arr.push({
                  label: element,
                  field: element,
                });
              }
            );
            this.$store.state.listDetailClickHeader = arr;
          }
        } else {
          console.log("fail");
        }
      }
    },
    sortByKey(array, key) {
      this.isASC = !this.isASC;
      if (this.isASC) {
        return array.sort(function (a, b) {
          var x = a[key];
          var y = b[key];
          return x < y ? -1 : x > y ? 1 : 0;
        });
      } else {
        return array.sort(function (a, b) {
          var x = a[key];
          var y = b[key];
          return x > y ? -1 : x < y ? 1 : 0;
        });
      }
    },
    OderBy(key) {
      console.log(key);
      this.sortByKey(this.$store.state.listDetailClick, key);
    },

    exportexcelxlsx(){
        // var elt = this.$refs.exportable_table1;

        // // var elt = this.$store.state.listDetailClickAll;
        // var wb = xlsx.utils.table_to_book(elt, {sheet:"Sheet JS"});

       // return dl ?
       //   xlsx.write(wb, {bookType:type, bookSST:true, type: 'base64'}) :
       // xlsx.writeFile(wb, fn || (( 'dowload.'|| 'SheetJSTableExport.') + (type || 'xlsx')));

       //if(typeof xlsx == 'undefined') xlsx = require('xlsx');

        /* make the worksheet */
        var ws = xlsx.utils.json_to_sheet(this.$store.state.listDetailClickAll);

        /* add to workbook */
        var wb = xlsx.utils.book_new();
        xlsx.utils.book_append_sheet(wb, ws, "data");

        /* generate an XLSX file */
        xlsx.writeFile(wb, "download.xlsx");
    
    },
    exportExcelCSV() {
 
 debugger;
      const items = this.$store.state.listDetailClickAll;
      //const replacer = (key, value) => (value === null ? "" : "\u200C"+ value.tostring()); // specify how you want to handle null values here
       const replacer = (key, value) => (value === null ? "" :  value);
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
           console.log("download");

      var blob = new Blob(["\ufeff", csv]);
      var url = URL.createObjectURL(blob);
      // console.log("creat_link");
      downloadLink.href = url;
      downloadLink.download = "data.csv";
      //console.log("down_fn");
      /*
       * Actually download CSV
       */
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);

        //console.log("down_fn_dh");
      // console.log(csv);
    },
    exporttxt(){
      const items = this.$store.state.listDetailClickAll;
      const replacer = (key, value) => (value === null ? "" : value); // specify how you want to handle null values here
      
      const header = Object.keys(items[0]);
      const txt = [
        header.join(","), // header row first
        ...items.map((row) =>
          header
            .map((fieldName) => JSON.parse(JSON.stringify(row[fieldName], replacer)))
            .join(",")
        ),
      ].join("\r\n");
       var downloadLink = document.createElement("a");
      var blob = new Blob(["\ufeff", txt]);
      var url=URL.createObjectURL(blob);
      downloadLink.href=url;
      downloadLink.download="data.txt";

      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
    },
    ExportExelOLD() {
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
  height: 60vh;
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
  z-index: 99990 !important;
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
  z-index: 99991 !important;
  height: 85vh;
  width: 100%;
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
</style>
