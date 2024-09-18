<template>
  <div class="div-main border">
    <div class="div-top row col-12">
      <div class="row col-12">
        <ul class="top-level-menu" style="z-index: 1000">
          <li>
            <a class="text-select-QM" href="javascript:void(0)">{{
              dataSelectQM.main_field
            }}</a>
            <Icon icon="chevron-down" class="sidenav-icon icon_check" />
            <ul class="second-level-menu">
              <li v-for="(item, index) in dataSelectQM.arr" :key="index">
                <template v-if="item.type == 'radio'">
                  <div class="div-selected">
                    <a class="text-select-QM" href="javascript:void(0)">{{
                      item.main_field
                    }}</a>
                    <Icon
                      icon="chevron-right"
                      class="sidenav-icon icon_check"
                    />
                  </div>
                  <ul class="third-level-menu">
                    <li
                      style="color: #fff"
                      v-for="(item2, index2) in item.arr"
                      :key="index2"
                      @click="changeOperate(index2)"
                    >
                      <template v-if="item2.selected">
                        <div
                          class="div-selected"
                          style="background-color: #0b6623"
                        >
                          <a class="text-select-QM" href="javascript:void(0)">{{
                            item2.main_field
                          }}</a>
                          <Icon icon="circle" class="sidenav-icon icon_check" />
                        </div>
                      </template>
                      <template v-else>
                        <a class="text-select-QM" href="javascript:void(0)">{{
                          item2.main_field
                        }}</a>
                      </template>
                    </li>
                  </ul>
                </template>
                <template v-else>
                  <a class="text-select-QM" href="javascript:void(0)"
                    >{{ item.main_field }}
                  </a>
                  <Icon icon="chevron-right" class="sidenav-icon icon_check" />
                  <ul class="third-level-menu">
                    <li
                      v-for="(item1, index1) in item.arr"
                      :key="index1"
                      @click="changeState(index, index1)"
                    >
                      <template v-if="item1.arr.length == 0">
                        <template v-if="item1.selected">
                          <div
                            class="div-selected"
                            style="background-color: #0b6623"
                          >
                            <a
                              class="text-select-QM"
                              href="javascript:void(0)"
                              style="
                                font-family: 'Open Sans', sans-serif !important;
                              "
                              >{{ item1.main_field }}</a
                            >
                            <Icon
                              icon="check"
                              class="sidenav-icon icon_check"
                            />
                          </div>
                        </template>
                        <template v-else>
                          <a class="text-select-QM" href="javascript:void(0)">{{
                            item1.main_field
                          }}</a>
                        </template>
                      </template>
                      <template v-else>
                        <div class="div-selected">
                          <a class="text-select-QM" href="javascript:void(0)">{{
                            item1.main_field
                          }}</a>
                          <Icon
                            icon="chevron-right"
                            class="sidenav-icon icon_check"
                          />
                        </div>
                        <ul class="third-level-menu">
                          <li
                            style="color: #fff"
                            v-for="(item2, index2) in item1.arr"
                            :key="index2"
                            @click="changeMOState(index2)"
                          >
                            <template v-if="item2.selected">
                              <div
                                class="div-selected"
                                style="background-color: #0b6623"
                              >
                                <a
                                  class="text-select-QM"
                                  href="javascript:void(0)"
                                  >{{ item2.main_field }}</a
                                >
                                <Icon
                                  icon="circle"
                                  class="sidenav-icon icon_check"
                                />
                              </div>
                            </template>
                            <template v-else>
                              <a
                                class="text-select-QM"
                                href="javascript:void(0)"
                                >{{ item2.main_field }}</a
                              >
                            </template>
                          </li>
                        </ul>
                      </template>
                    </li>
                  </ul>
                </template>
              </li>
            </ul>
          </li>
        </ul>
        <div class="top-left col row">
          <div class="col-3">
            <datepicker
              class="form-control form-control-sm"
              v-model="dateFrom"
              :upperLimit="dateTo"
            />
          </div>
          <div class="col-3">
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
          <div class="col-3">
            <datepicker
              class="form-control form-control-sm"
              v-model="dateTo"
              :lowerLimit="dateFrom"
            />
          </div>
          <div class="col-3">
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

      <div></div>
    </div>
    <div class="row col-12" style="padding-bottom: 5px">
      <div class="col-3 column select-div">
        <div class="column">
          <button @click="modelClick()" class="col btn_select_item">
            Model
          </button>
          <div class="div_select_item">
            <ul>
              <smooth-scrollbar>
                <template v-if="$store.state.listSelectDualModel.length == 0">
                  <li style="text-align: center">ALL</li>
                </template>
                <template v-else>
                  <li
                    class="li_selected"
                    v-for="(item, index) in $store.state.listSelectDualModel"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </template>
              </smooth-scrollbar>
            </ul>
          </div>
        </div>
      </div>
      <div class="col-3 select-div">
        <div class="column">
          <button @click="moClick()" class="col btn_select_item">MO</button>
          <div class="div_select_item">
            <ul>
              <smooth-scrollbar>
                <template v-if="$store.state.listSelectDualMO.length == 0">
                  <li style="text-align: center">ALL</li>
                </template>
                <template v-else>
                  <li
                    class="li_selected"
                    v-for="(item, index) in $store.state.listSelectDualMO"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </template>
              </smooth-scrollbar>
            </ul>
          </div>
        </div>
      </div>
      <div class="col-3">
        <div class="column">
          <button @click="lineClick()" class="col btn_select_item">
            Line
          </button>
          <div class="div_select_item">
            <ul>
              <smooth-scrollbar>
                <template v-if="$store.state.listSelectDualLine.length == 0">
                  <li style="text-align: center">ALL</li>
                </template>
                <template v-else>
                  <li
                    class="li_selected"
                    v-for="(item, index) in $store.state.listSelectDualLine"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </template>
              </smooth-scrollbar>
            </ul>
          </div>
        </div>
      </div>
      <div class="col-3">
        <div class="column">
          <button @click="groupClick()" class="col btn_select_item">
            Group
          </button>
          <div class="div_select_item">
            <ul>
              <smooth-scrollbar>
                <template v-if="$store.state.listSelectDualGroup.length == 0">
                  <li style="text-align: center">ALL</li>
                </template>
                <template v-else>
                  <li
                    class="li_selected"
                    v-for="(item, index) in $store.state.listSelectDualGroup"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </template>
              </smooth-scrollbar>
            </ul>
          </div>
        </div>
      </div>
    </div>
    <div class="row col-12" style="padding-bottom: 5px">
      <div
        class="div-option-uncheck text-title col"
        :class="{ option_active: opt1 }"
        @click="clickOption('opt1')"
      >
        Yield Rate
      </div>
      <div
        class="div-option-uncheck text-title col"
        :class="{ option_active: opt2 }"
        @click="clickOption('opt2')"
      >
        Defect Analysis
      </div>
      <div
        class="div-option-uncheck text-title col"
        :class="{ option_active: opt4 }"
        @click="clickOption('opt4')"
      >
        List SN First-fail
      </div>
      <div
        class="div-option-uncheck text-title col"
        :class="{ option_active: opt5 }"
        @click="clickOption('opt5')"
      >
        List SN Second-fail
      </div>
      <div class="col"></div>
      <div class="col"></div>
      <div class="col"></div>
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
          {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
        ></span
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
  <div class="row col-12 div-content">
    <div class="div-table">
      <table id="tableMain" class="mytable table table">
        <thead>
          <tr>
            <template v-for="(item, index) in qmDataTableHeader" :key="index">
              <th>
                {{ item }}
              </th>
            </template>
          </tr>
        </thead>
        <template v-for="(item, index) in qmDataTable" :key="index">
          <template v-if="item.QA_RESULT == 'PASS'">
            <tr class="tr-green hover-tr">
              <template v-for="(item1, index1) in item" :key="index1">
                <td>{{ item1 }}</td>
              </template>
            </tr>
          </template>
          <template v-else-if="item.QA_RESULT == 'REJECT'">
            <tr class="tr-yellow hover-tr">
              <template v-for="(item1, index1) in item" :key="index1">
                <td>{{ item1 }}</td>
              </template>
            </tr>
          </template>
          <template
            v-else-if="
              (item.YIELD_RATE < 94 && item.YIELD_RATE != 0) ||
              item.PASS_QTY < item.FAIL_QTY
            "
          >
            <tr class="tr-red hover-tr">
              <template v-for="(item1, index1) in item" :key="index1">
                <td v-if="index1 == 'YIELD_RATE' || index1 == 'F.P.Y.Rate'">
                  <b>{{ item1 }}</b> %
                </td>
                <td v-else>{{ item1 }}</td>
              </template>
            </tr>
          </template>
          <template v-else-if="item.YIELD_RATE < 98 && item.YIELD_RATE != 0">
            <tr class="tr-yellow hover-tr" style="background-color: #e2cb6c">
              <template v-for="(item1, index1) in item" :key="index1">
                <td v-if="index1 == 'YIELD_RATE' || index1 == 'F.P.Y.Rate'">
                  <b>{{ item1 }}</b> %
                </td>
                <td v-else>{{ item1 }}</td>
              </template>
            </tr>
          </template>
          <template v-else-if="item.QTY > 10">
            <tr class="tr-red hover-tr">
              <template v-for="(item1, index1) in item" :key="index1">
                <td>{{ item1 }}</td>
              </template>
            </tr>
          </template>
          <template v-else-if="item.QTY > 5">
            <tr class="tr-yellow hover-tr">
              <template v-for="(item1, index1) in item" :key="index1">
                <td>{{ item1 }}</td>
              </template>
            </tr>
          </template>
          <template v-else>
            <tr class="hover-tr">
              <template v-for="(item1, index1) in item" :key="index1">
                <td v-if="index1 == 'YIELD_RATE' || index1 == 'F.P.Y.Rate'">
                  <b>{{ item1 }}</b> %
                </td>
                <td v-else>{{ item1 }}</td>
              </template>
            </tr>
          </template>
        </template>
      </table>
    </div>
  </div>
</template>

<script>
import Repository from "../services/Repository";

import data_select_QM from "../data/data_select_TCQS";
import listTimeFrom from "../data/qmTimeFrom";
import listTimeTo from "../data/qmTimeTo";
export default {
  name: "QM",
  components: {},
  data() {
    return {
      dateFrom: new Date(),
      dateTo: new Date(),
      qmDateFrom: "",
      qmDateTo: "",
      qmTimeFrom: "",
      qmTimeTo: "",
      listTimeFrom: listTimeFrom,
      listTimeTo: listTimeTo,
      timeFrom: "ALL",
      timeTo: "",
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
      dataSelectQM: [],
      oldModel: "",
      hideSelectModel: false,
      textModel: "ALL",
      listTotalQTY: [],
      listYieldRate: [],
      listCategory: [],
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
    changeOperate(index) {
      this.dataSelectQM.arr[0].arr[0].selected = false;
      this.dataSelectQM.arr[0].arr[1].selected = false;
      if (index == 0) {
        this.operate = "Auto";
      } else {
        this.operate = "Manual";
      }
      this.dataSelectQM.arr[0].arr[index].selected = true;
    },
    changeState(v1, v2) {
      if (!this.dataSelectQM.arr[v1].arr[v2].selected) {
        this.dataSelectQM.arr[v1].arr[v2].selected = true;
      } else {
        this.dataSelectQM.arr[v1].arr[v2].selected = false;
      }
    },
    ShowChart() {
      this.ClearChart();
      if (this.opt1) {
        this.$store.state.mixChartSeries[1].name = "QTY";
        this.$store.state.mixChartSeries[0].name = "Yield rate (%)";

        this.qmDataTable.forEach((element) => {
          this.listYieldRate.push(element.YIELD_RATE);
          this.listTotalQTY.push(element.TOTAL_QTY);
          this.listCategory.push(element.GROUP_NAME);
        });
        this.$store.state.mixChartSeries[0].data = this.listYieldRate;
        this.$store.state.mixChartSeries[1].data = this.listTotalQTY;
        this.$store.state.mixChartOption.labels = this.listCategory;
        this.$store.state.mixChartOption.yaxis[1].title.text = "Total QTY";
        this.$store.state.mixChartOption.title.text = "Yield rate chart";
        this.$store.state.mixChartOption.yaxis[0].title.text = "Yield rate (%)";
        this.$store.state.isShowMixChart = true;
      } else if (this.opt2) {
        this.$store.state.mixChartSeries[1].name = "Test rate (%)";
        this.$store.state.mixChartSeries[0].name = "QTY";

        this.qmDataTable.forEach((element) => {
          this.listYieldRate.push(element.QTY);
          this.listTotalQTY.push(element.Rate);
          this.listCategory.push(element.TEST_CODE);
        });
        this.$store.state.mixChartSeries[0].data = this.listYieldRate;
        this.$store.state.mixChartSeries[1].data = this.listTotalQTY;

        this.$store.state.mixChartOption.labels = this.listCategory;
        this.$store.state.mixChartOption.yaxis[1].title.text = "Test rate (%)";
        this.$store.state.mixChartOption.title.text = "Defect analysis chart";
        this.$store.state.mixChartOption.yaxis[0].title.text = "Total QTY";
        this.$store.state.isShowMixChart = true;
      }
    },
    ClearChart() {
      this.listYieldRate = [];
      this.listTotalQTY = [];
      this.listCategory = [];
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
    // ExportExel(idName) {
    //   var tab_text = "<table border='2px'><tr bgcolor='#418BCA'>";
    //   var j = 0;
    //   var tab = document.getElementById(idName); // id of table
    //   for (j = 0; j < tab.rows.length; j++) {
    //     tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    //     //tab_text=tab_text+"</tr>";
    //   }
    //   tab_text = tab_text + "</table>";
    //   tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
    //   tab_text = tab_text.replace(/<img[^>]*>/gi, "");
    //   tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");
    //   var sa = window.open(
    //     "data:application/vnd.ms-excel," + encodeURIComponent(tab_text)
    //   );
    //   return sa;
    // },
    async GetQMElement(typeSearch, listType, modalType) {
      var dateFrom = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
        this.dateFrom.getMonth() + 1
      )}${this.pad(this.dateFrom.getDate())}`;
      var dateTo = `${this.pad(this.dateTo.getFullYear())}${this.pad(
        this.dateTo.getMonth() + 1
      )}${this.pad(this.dateTo.getDate())}`;

      var timeFrom = this.timeFrom;
      // if (timeFrom == "0000") {
      //   timeFrom = "2400";
      //   var date = new Date(this.dateFrom.valueOf() - 1000 * 60 * 60 * 24);
      //   dateFrom = `${this.pad(date.getFullYear())}${this.pad(
      //     date.getMonth() + 1
      //   )}${this.pad(date.getDate())}`;
      // }
      var timeTo = this.timeTo;
      var payload = {
        database_name: localStorage.databaseName,
        type: typeSearch,
        value: this.textSearch,
        dateFrom: dateFrom,
        dateTo: dateTo,
        timeFrom: timeFrom,
        timeTo: timeTo,
        listModel: this.$store.state.listSelectDualModel,
        listMo: this.$store.state.listSelectDualMO,
        listLine: this.$store.state.listSelectDualLine,
        listGroup: this.$store.state.listSelectDualGroup,
      };
      var { data } = await Repository.getRepo("GetQMElement", payload);
      this.$store.commit(listType, data.data);
      this.$store.commit(modalType);
    },
    modelClick() {
      this.GetQMElement("model", "UpdateListSelectModel", "showModalModel");
    },
    moClick() {
      this.GetQMElement("mo", "UpdateListSelectMO", "showModalMO");
    },
    lineClick() {
      this.GetQMElement("line", "UpdateListSelectLine", "showModalLine");
    },
    groupClick() {
      this.GetQMElement("group", "UpdateListSelectGroup", "showModalGroup");
    },
    async doQuery(typeSearch) {
      var iOperate = "";
      var iDate = false;
      var iModelName = false;
      var iVersionCode = false;
      var iMoNumber = false;
      var iLineName = false;
      var iGroupName = true;
      var iStationName = true;
      var iByHour = true;
      this.typeSearch = typeSearch;
      if (typeSearch == "TCQS-YieldRate") {
        var ownList = this.dataSelectQM.arr[1].arr;
        iDate = ownList[0].selected;
        iModelName = ownList[1].selected;
        iVersionCode = ownList[2].selected;
        iMoNumber = ownList[3].selected;
        iLineName = ownList[4].selected;
        iGroupName = ownList[5].selected;
        iStationName = ownList[6].selected;
        iByHour = ownList[7].selected;
      } else if (typeSearch == "TCQS-Defect") {
        var ownListDefect = this.dataSelectQM.arr[2].arr;
        iStationName = ownListDefect[0].selected;
        iDate = true;
        iModelName = true;
        iVersionCode = true;
        iMoNumber = true;
        iLineName = true;
        iGroupName = true;
        iByHour = true;
      }
      var dateFrom = "";
      var dateTo = "";
      var timeFrom = "";
      var timeTo = "";

      dateFrom = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
        this.dateFrom.getMonth() + 1
      )}${this.pad(this.dateFrom.getDate())}`;
      dateTo = `${this.pad(this.dateTo.getFullYear())}${this.pad(
        this.dateTo.getMonth() + 1
      )}${this.pad(this.dateTo.getDate())}`;
      var dateFromOrigin = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
        this.dateFrom.getMonth() + 1
      )}${this.pad(this.dateFrom.getDate())}`;
      dateTo = `${this.pad(this.dateTo.getFullYear())}${this.pad(
        this.dateTo.getMonth() + 1
      )}${this.pad(this.dateTo.getDate())}`;

      if (typeSearch == "TCQS-YieldRate") {
        timeFrom = this.timeFrom;
        timeTo = this.timeTo;
      } else if (typeSearch == "TCQS-Defect") {
        timeFrom = this.timeFrom;
        timeTo = this.timeTo;
      } else if (
        typeSearch == "ListSNFirstFail" ||
        typeSearch == "ListSNSecondFail"
      ) {
        timeFrom = this.timeFrom + "00";
        timeTo = this.timeTo + "00";
      } else {
        timeFrom = dateFrom + this.timeFrom.replace(":", "") + "00";
        timeTo = dateTo + this.timeTo.replace(":", "") + "00";
      }
      this.qmDataTableHeader = [];
      this.qmDataTable = [];
      this.qmDataTableAll = [];
      this.qmDataTableHeaderAll = [];
      var payload = {
        database_name: localStorage.databaseName,
        value: this.textSearch,
        dateFrom: dateFrom,
        dateFromOrigi: dateFromOrigin,
        dateTo: dateTo,
        timeFrom: timeFrom,
        moState: this.moState,
        timeTo: timeTo,
        listModel: this.$store.state.listSelectDualModel,
        listMo: this.$store.state.listSelectDualMO,
        listLine: this.$store.state.listSelectDualLine,
        listGroup: this.$store.state.listSelectDualGroup,
        iOperate: iOperate,
        iDate: iDate,
        iModelName: iModelName,
        iVersionCode: iVersionCode,
        iMoNumber: iMoNumber,
        iLineName: iLineName,
        iGroupName: iGroupName,
        iStationName: iStationName,
        iByHour: iByHour,
      };
      var { data } = await Repository.getRepo(typeSearch, payload);
      if (data.result == "fail") {
        this.qmDataTableHeader = [];
        this.qmDataTable = [];
        this.qmDataTableAll = [];
        this.qmDataTableHeaderAll = [];
      } else {
        this.qmDataTableAll = data.data;
        this.qmDataTable = this.qmDataTableAll.slice(0, 500);
        if (this.qmDataTable.length != 0) {
          this.qmDataTableHeader = Object.keys(this.qmDataTable[0]);
          Object.keys(this.qmDataTable[0]).forEach((element) => {
            this.qmDataTableHeaderAll.push({
              label: element,
              field: element,
            });
          });
        }
        if (typeSearch == "QMYieldRate") {
          this.withoutDataGroup = data.allGroup;
          this.fpRate = data.fpRate;
          this.ttlRate = data.ttlRate;
        }
      }
    },
    clickOption(value) {
      this.setAllToUnActive();
      if (value == "opt1") {
        this.opt1 = true;
        this.doQuery("TCQS-YieldRate");
      } else if (value == "opt2") {
        this.opt2 = true;
        this.doQuery("TCQS-Defect");
      } else if (value == "opt3") {
        this.opt3 = true;
        this.doQuery("RepairAnalysis");
      } else if (value == "opt4") {
        this.opt4 = true;
        this.doQuery("ListSNFirstFail");
      } else if (value == "opt5") {
        this.opt5 = true;
        this.doQuery("ListSNSecondFail");
      } else if (value == "opt6") {
        this.opt6 = true;
        this.doQuery("QCAnalysis");
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
    pad(num) {
      num = num.toString();
      while (num.length < 2) num = "0" + num;
      return num;
    },
  },
  mounted() {
    document.title = "TCQS";
    this.dataSelectQM = data_select_QM;
    this.timeFrom = "00:00";
    this.timeTo = "00:30";
    this.$store.commit("RefreshState");
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

.div-table {
  min-height: 76vh;
  max-height: 76vh;
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
  width: 100%;
  max-height: 700px;
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
.div-top {
  min-height: 35px;
  // padding-top: 5px;
  // padding-bottom: 5px;
  // display: flex;
  // justify-content: space-between;
}
</style>
