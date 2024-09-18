<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="isShowModal"
        @click="$emit('hideModal')"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="isShowModal">
        <template v-if="$store.state.listDetailClick.length > 0">
          <div class="content-top">
            <b>
              {{
                $store.state.language == "En"
                  ? "Information detail"
                  : "Thông tin chi tiết"
              }}</b
            >
            <span class="closeButton" @click="$emit('hideModal')"
              ><Icon style="font-size: 23px; color: red" icon="times-circle"
            /></span>
          </div>
          <div class="content-top">
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
             <!-- class="btnExport" -->
             <!-- <div>
            <VueExcelXlsx
            
              :data="$store.state.listDetailClickAll"
              :columns="$store.state.listDetailClickHeader"
              :filename="'data'"
              :file-type="'xlsx'"
              :sheetname="'data'"
            >
              <img
                class="img-excel"
                src="../../assets/img/xlsx-icon.jpg"
                alt=""
              /> 
            </VueExcelXlsx> 
             </div> -->
          </div>
          <div class="div-table">
            <!-- <smooth-scrollbar> -->
            <table
              id="tableMain"
              class="table table-condensed table-bordered table-sm"
              ref="exportable_table"
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
              <tr v-for="(item, index) in $store.state.listDetailClick" :key="index">
                <td v-for="(itemContent, index1) in item" :key="index1">
                  <b>{{ itemContent }}</b>
                </td>
              </tr>
            </table>
            <!-- </smooth-scrollbar> -->
          </div>

          <!--<div class="div-table" v-show="false">
            
             <table
              id="tableMain1"
              class="table table-condensed table-bordered table-sm"
              ref="exportable_table1"
            >
              <thead>
                <tr>
                  <th
                    @click="OderBy(item)"
                    v-for="(item, index) in Object.keys(
                      $store.state.listDetailClickAll[0]
                    )"
                    :key="index"
                  >
                    <b> {{ item }}</b>
                  </th>
                </tr>
              </thead>
              <tr
                v-for="(item, index) in $store.state.listDetailClickAll"
                :key="index"
              >
                <td v-for="(itemContent, index1) in item" :key="index1">
                  <b>{{ itemContent }}</b>
                </td>
              </tr>
            </table> 
            
          </div>-->


        </template>
        <!-- <button class="button" @click="$emit('hideModal')">Close Modal</button> -->
      </div>
    </transition>
  </div>
</template>
<script>
import xlsx from 'xlsx';
//import Vueexelxlsx from 'vue-excel-xlsx';
//import VueExcelXlsx from 'vue-excel-xlsx';
export default {
  props: {
    isShowModal: Boolean,
    listDetail: Array,
    listDetailAll: Array,
  },
  components: {},
  data() {
    return {
      showModal: false,
      isASC: false,
    };
  },
  methods: {
    exportExcelCSV() {
      const items = this.$store.state.listDetailClickAll;

      //const replacer = (key, value) => (value === null ? "" : "\u200C" + value); // specify how you want to handle null values here
      const replacer = (key, value) => (value === null ? "" :  value);

      //const replacer = (key, value) => (value === null ? "" : "\u200C" + value); // specify how you want to handle null values here
      

      //const replacer = (key, value) => (value === null ? "" : "" + value); // specify how you want to handle null values here

      //const replacer = (key, value) => (value === null ? "" : "\u200C" + value); // specify how you want to handle null values here
      

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
      //exportexcelxlsx(type, fn, dl){
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
    exportExcel() {
      const items = this.$store.state.listDetailClickAll;
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
      downloadLink.download = "data.xlsx";

      /*
       * Actually download CSV
       */
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
      // console.log(csv);
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
    ExportExel() {
      var tab_text = "<table border='2px'><tr bgcolor='#418BCA'>";
      var j = 0;
      var tab = document.getElementById("tableMain1"); // id of table

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
@media only screen and (hover: none) and (pointer: coarse) {
  * {
    font-size: 10px !important;
  }
  .img-excel {
    height: 35px !important;
    width: 40px !important;
  }
}
.btnExport {
  appearance: none;
  outline: none;
  border: none;
  background: none;
  cursor: pointer;
}
.img-excel {
  height: 45px;
  width: 60px;
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
.div-table {
  max-width: 92vw;
  height: 80%;
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
  thead tr th {
    position: sticky;
    background-color: #418bca;
    top: 0;
    z-index: 2;
    overflow-x: auto;
    white-space: nowrap;
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
