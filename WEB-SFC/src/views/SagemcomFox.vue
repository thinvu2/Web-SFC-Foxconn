<template>
  <div class="div-main" style="background-color: #4e7da6">
    <div class="div-top row col-12">
      <div class="top-left col-12 row">
        <div class="col">
          <datepicker
            class="form-control form-control-sm shadow-element"
            v-model="dateFrom"
            :upperLimit="dateTo"
            @change="timeFromChange()"
          />
        </div>
        <div class="col">
          <select
            @change="timeFromChange()"
            v-model="timeFrom"
            class="form-control form-control-sm shadow-element"
            name=""
            id=""
          >
            <option v-for="(item, index) in listTimeFrom" :key="index">
              {{ item.VALUE }}
            </option>
          </select>
        </div>
        <div class="col">
          <datepicker
            class="form-control form-control-sm"
            v-model="dateTo"
            :lowerLimit="dateFrom"
            @change="timeToChange()"
          />
        </div>
        <div class="col">
          <template v-if="timeFrom == 'ALL'">
            <select
              @change="timeToChange()"
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
              @change="timeToChange()"
              v-model="timeTo"
              class="form-control form-control-sm"
              name=""
              id=""
            >
              <option v-for="(item, index) in listTimeTo" :key="index">
                {{ item.VALUE }}
              </option>
            </select>
          </template>
        </div>

        <div class="col column">
          <!-- <span class="text-title">Model</span> -->
          <input
            @click="customermodelClick()"
            type="text"
            :value="
              $store.state.listSelectDualModel.length == 0
                ? 'All Foxconn model'
                : `${$store.state.listSelectDualModel[0].VALUE}...`
            "
            class="form-control form-control-sm text-element"
            readonly
          />
        </div>
      </div>
    </div>
    <div class="row col-12" style="padding-bottom: 5px">
      <div
        class="div-option-uncheck text-title"
        :class="{ option_active: opt2 }"
        @click="clickOption('opt2')"
      >
        Yield Rate
      </div>
      <div
        class="div-option-uncheck text-title"
        :class="{ option_active: opt3 }"
        @click="clickOption('opt3')"
      >
        Defect Analysis
      </div>
    </div>
  </div>
  <div class="div-export row">
    <div v-if="qmDataTable.length > 0">
      <span
        ><b class="text-count">
          {{ $store.state.language == "En" ? "Show:" : "Hiển thị" }}
          <span class="count-number">{{ qmDataTable.length }}</span>
          {{ $store.state.language == "En" ? " of " : " của " }}
          <span class="count-number">{{ qmDataTableAll.length }}</span>
          {{ $store.state.language == "En" ? "records" : "bản ghi" }}
          {{ $store.state.language == "En" ? "records" : "bản ghi" }}
          </b>
          <div class="profile_info">
          <span style="font-size: 20px">{{name2 == "1" ? "Sagemcom Query" : "Defect Query"}}</span>
        </div>
          </span
      >
    </div>
    <div class="row">
      <template v-if="qmDataTable.length > 0">
        <div v-if="opt2 == true || opt3 == true"></div>
        <div v-if="opt6 == false && opt7 == false">
          <img
            class="img-excel"
            @click="ExportExel"
            src="../assets/img/excel_64.png"
            alt=""
          />
        </div>
        <div v-else>
          <vue-excel-xlsx
            class="btnExport"
            :data="qmDataTableAll"
            :columns="qmDataTableHeaderAll"
            :filename="'data'"
            :sheetname="'ListFail'"
          >
            <img class="img-excel" src="../assets/img/excel_64.png" alt="" />
          </vue-excel-xlsx>
        </div>
      </template>
    </div>
  </div>
  <div class="row col-12 div-table">
    <table class="table table-condensed table-bordered table-sm">
      <tr>
        <th v-for="(item, index) in qmDataTableHeader" :key="index">
          {{ item }}
        </th>
      </tr>
      <template v-for="(item, index) in qmDataTable" :key="index">
        <template v-if="item.F_P_RATE < 97">
          <tr class="tr-red">
            <td v-for="(item1, index1) in item" :key="index1">{{ item1 }}</td>
          </tr>
        </template>
        <template v-else>
          <tr>
            <td v-for="(item1, index1) in item" :key="index1">{{ item1 }}</td>
          </tr>
        </template>
      </template>
    </table>
  </div>
  <div class="div-export row">
    <div v-if="subDataTable.length > 0">
      <span
        ><b class="text-count">
          {{ $store.state.language == "En" ? "Show:" : "Hiển thị" }}
          <span class="count-number">{{ subDataTable.length }}</span>
          {{ $store.state.language == "En" ? " of " : " của " }}
          <span class="count-number">{{ subAllDataTable.length }}</span>
          {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
        ></span
      >
    </div>
    <div class="row">
      <template v-if="subDataTable.length > 0">
        <div v-if="opt1 == true || opt2 == true"></div>
        <div v-if="opt4 == false && opt5 == false">
          <img
            class="img-excel"
            @click="ExportExel"
            src="../assets/img/excel_64.png"
            alt=""
          />
        </div>
      </template>
    </div>
  </div>
  <div class="row col-12 div-table">
    <table class="table table-condensed table-bordered table-sm">
      <tr>
        <th v-for="(item, index) in subDataTableHeader" :key="index">
          {{ item }}
        </th>
      </tr>
      <template v-for="(item, index) in subDataTable" :key="index">
        <template v-if="item.F_P_RATE < 97">
          <tr class="tr-red">
            <td v-for="(item1, index1) in item" :key="index1">{{ item1 }}</td>
          </tr>
        </template>
        <template v-else>
          <tr>
            <td v-for="(item1, index1) in item" :key="index1">{{ item1 }}</td>
          </tr>
        </template>
      </template>
    </table>
  </div>
</template>

<script>
import Repository from "../services/Repository";

import listTimeFrom from "../data/timeFrom";
import listTimeTo from "../data/timeTo";
export default {
  name: "Sagemcom",
  components: {},
  data() {
    return {
      dateFrom: new Date(),
      dateTo: new Date(),
      listTimeFrom: listTimeFrom,
      listTimeTo: listTimeTo,
      timeFrom: "ALL",
      timeTo: "",
      name2: "",
      value: null,
      valueNull: null,
      listModelSearch: [],
      valueModel: [],
      valueMO: ["ALL"],
      valueLine: ["ALL"],
      valueGroup: ["ALL"],
      opt1: true,
      opt2: false,
      opt3: false,
      opt4: false,
      opt5: false,
      opt6: false,
      opt7: false,
      opt8: false,
      oldModel: "",
      hideSelectModel: false,
      textModel: "ALL",
      isModel: false,
      isMo: false,
      isLine: false,
      isGroup: true,
      isVersionCode: false,
      isModelSerial: false,
      isRepassQty: false,
      isFirstFailQty: true,
      qmDataTable: [],
      qmDataTableHeader: [],
      subDataTable: [],
      subAllDataTable: [],
      subDataTableHeader: [],
    };
  },
  methods: {
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
    },
    async customermodelClick() {
      var myTimeFrom = "";
      var myTimeTo = "";
      var myDateFrom = `${this.pad(this.dateFrom.getFullYear())}/${this.pad(
        this.dateFrom.getMonth() + 1
      )}/${this.pad(this.dateFrom.getDate())}`;
      var myDateTo = `${this.pad(this.dateTo.getFullYear())}/${this.pad(
        this.dateTo.getMonth() + 1
      )}/${this.pad(this.dateTo.getDate())}`;
      if (this.timeFrom == "ALL") {
        myTimeFrom = "00:00";
        myTimeTo = "23:59";
      } else {
        myTimeFrom = this.timeFrom;
        myTimeTo = this.timeTo;
      }

      var payload = {
        database_name: localStorage.databaseName,
        type: "Foxmodel",
        dateFrom: myDateFrom,
        timeFrom: myTimeFrom,
        dateTo: myDateTo,
        timeTo: myTimeTo,
      };
      var { data } = await Repository.getRepo("GetSagemcomElement", payload);
      this.$store.commit("UpdateListSelectModel", data.data);
      this.$store.commit("showModalModel");
    },
    queryQM() {
      if (this.opt2) {
        this.doQuery("SagemcomQuery");
      }else if (this.opt3) {
        this.doQuery("SagemcomQuery1");
      }
    },
    async doQuery(typeSearch) {
      var myDateFrom = `${this.pad(this.dateFrom.getFullYear())}/${this.pad(
        this.dateFrom.getMonth() + 1
      )}/${this.pad(this.dateFrom.getDate())}`;
      var myDateTo = `${this.pad(this.dateTo.getFullYear())}/${this.pad(
        this.dateTo.getMonth() + 1
      )}/${this.pad(this.dateTo.getDate())}`;
      var myTimeFrom = "";
      var myTimeTo = "";
      if (this.timeFrom == "ALL") {
        myTimeFrom = "00:00";
        myTimeTo = "23:59";
      } else {
        myTimeFrom = this.timeFrom;
        myTimeTo = this.timeTo;
      }
      var payload = {
        database: localStorage.databaseName,
        date_from: myDateFrom,
        date_to: myDateTo,
        option: typeSearch,
        timeFrom: myTimeFrom,
        timeTo: myTimeTo,
        group_mo: this.$store.state.listSelectDualGroup,
        group_model: this.$store.state.listSelectDualModel,
        group_by: [
          {
            key: "SERIAL_NUMBER",
          },
        ],
      };
      if(this.$store.state.listSelectDualModel != ""){
        var { data } = await Repository.getRepo("GetFoxSagemcom", payload);
              this.qmDataTableAll = data.data;
              this.qmDataTable = this.qmDataTableAll.slice(0, 500);
              if (this.qmDataTable.length != 0) {
                this.qmDataTableHeader = Object.keys(this.qmDataTable[0]);
                this.subAllDataTable = data.data1;
                this.subDataTable = this.subAllDataTable.slice(0, 500);
                this.subDataTableHeader = Object.keys(this.subAllDataTable[0]);
              }
          }else{
            alert("Model Name is not null!");
          }
        },
    clickOption(value) {
      
      this.setAllToUnActive();
      if (value == "opt2") {
        this.opt2 = true;
        this.doQuery("SagemcomQuery");
        this.name2 = "1";
        
      }else if (value == "opt3") {
        this.opt3 = true;
        this.doQuery("SagemcomQuery1");
        this.name2 = "2";
      }
    },
    
    setAllToUnActive() {
      this.opt1 = false;
      this.opt2 = false;
      this.opt3 = false;
      this.opt4 = false;
      this.opt5 = false;
      this.opt6 = false;
      this.opt7 = false;
      this.opt8 = false;
    },
    timeFromChange() {
      if (this.timeFrom == "ALL") {
        this.timeTo = "";
        var updateTime1 = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
          this.dateFrom.getMonth() + 1
        )}${this.pad(this.dateFrom.getDate())}0000`;
        this.$store.commit("updateDateTimeFromQM", updateTime1);
      } else {
        var updateTime = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
          this.dateFrom.getMonth() + 1
        )}${this.pad(this.dateFrom.getDate())}${this.timeFrom
          .toString()
          .replace(":", "")}`;
        this.$store.commit("updateDateTimeFromQM", updateTime);
      }
    },
    pad(num) {
      num = num.toString();
      while (num.length < 2) num = "0" + num;
      return num;
    },
    timeToChange() {
      if (this.timeTo == "") {
        var updateTime1 = `${this.pad(this.dateTo.getFullYear())}${this.pad(
          this.dateTo.getMonth() + 1
        )}${this.pad(this.dateTo.getDate())}2359`;
        this.$store.commit("updateDateTimeToQM", updateTime1);
      } else {
        var updateTime = `${this.pad(this.dateTo.getFullYear())}${this.pad(
          this.dateTo.getMonth() + 1
        )}${this.pad(this.dateTo.getDate())}${this.timeTo
          .toString()
          .replace(":", "")}`;
        this.$store.commit("updateDateTimeToQM", updateTime);
      }
    },
  },
  mounted() {
    document.title = "Sagemcom Query";
    this.timeToChange();
    this.timeFromChange();
    this.$store.commit("RefreshState");
  },
};
</script>

<style lang="scss" scoped>
@media only screen and (hover: none) and (pointer: coarse) {
  .form-check-label {
    font-size: 10px !important;
  }
  .text-title {
    font-size: 10px !important;
  }
  .div-option-uncheck {
    height: 30px !important;
    width: 120px !important;
    line-height: 30px !important;
  }
  .table-condensed {
    font-size: 10px !important;
  }
}
.mytable {
  min-width: 900px;
  table-layout: fixed;
  margin-top: 10px;
  tr {
    th {
      position: sticky;
      top: 0;
      z-index: 2;
      background-color: #418bca;
      color: #fff;
      min-width: 60px;
      padding: 3px;
      font-size: 13px;
      border: 0.5px solid #cdc;
    }
  }
  tr {
    td {
      z-index: 1;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 13px;
      height: 40px;
    }
  }
}
td {
  white-space: nowrap;
  overflow: hidden;
}
.text-count {
  font-size: 13px;
  .count-number {
    color: #4e7da6;
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
.div-export {
  margin-top: 10px;
  margin-left: 20px;
  display: flex;
  justify-content: space-between;
}
.tr-red {
  background-color: red;
  td {
    color: #fff;
  }
}
.div-table {
  min-height: 40vh;
  max-height: 40vh;
  margin-left: 5px;
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
  font-size: 14px;
  color: #fffbf7;
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
  box-shadow: 1px 1px 1px #6b6b6b;
  margin: 2px;
  height: 40px;
  width: 130px;
  text-align: center;
  line-height: 40px;
  border-radius: 10px;
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
  padding-left: 20px;
}
.div-top {
  padding-top: 5px;
  padding-bottom: 5px;
  display: flex;
  justify-content: space-between;
}
</style>
<style src="@vueform/multiselect/themes/default.css"></style>

