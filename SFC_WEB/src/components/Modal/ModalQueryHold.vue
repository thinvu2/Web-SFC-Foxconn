<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowQueryHold"
        @click="hideModal()"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" v-if="$store.state.isShowQueryHold">
        <div class="div-top content-top">
          <b>
            {{
              $store.state.language == "En"
                ? "MO NUMBER"
                : "Tìm kiếm danh sách"
            }}</b
          >
          <span class="closeButton" @click="hideModal()"
            ><Icon style="font-size: 23px; color: red" icon="times-circle"
          /></span>
        </div>
        <div class="content-top col-sm-12">
          <textarea
            v-model="textListSearch"
            class="form-control col-sm-5"
          ></textarea>
          
          <button
            class="btn btn-primary col-sm-1 btn-search"
            @click="SearchList()"
          >
            <Icon icon="search-plus" class="sidenav-icon" />
          </button>
        </div>
        <template v-if="$store.state.listDetailClick.length > 0">
          <div class="content-top-export row">
           
            
            
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
       // var listInput = [];
         debugger;
        //var texter = "";
        this.textListSearch.replace(/\n/,'');
    debugger;
        var payload = {
          database_name: localStorage.databaseName,
          MO_NUMBER: this.textListSearch.trim(),

        };
        var { data } = await Repository.getRepo("QueryHoldContent", payload);
       
        if (data.result == "ok") {
          this.$store.state.listDetailClickAll = data.data;
          this.$store.state.listDetailClick =
            this.$store.state.listDetailClickAll.slice(0, 500);
            if (data.data.length > 0) {
            this.$store.state.isShowQueryHold = true;
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
  width: 80%;
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
