<template>
  <div class="div-main">
    <div
      class="div-top row col-12"
      style="background-color: #4e7da6; padding-top: 20px"
    >
      <div class="col-1">
        <input
          type="checkbox"
          id="checkboxAllTime"
          v-model="isAllTime"
          :checked="isAllTime"
        />
        <label style="color: #fffbf7" for="checkboxAllTime">All</label>
      </div>
      <div class="col-7 row">
        <div class="col-3">
          <datepicker
            class="form-control form-control-sm shadow-element"
            v-model="dateFrom"
            :upperLimit="dateTo"
          />
        </div>
        <div class="col-3">
          <select
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
        <div class="col-3">
          <datepicker
            class="form-control form-control-sm"
            v-model="dateTo"
            :lowerLimit="dateFrom"
            @change="timeToChange()"
          />
        </div>
        <div class="col-3">
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
      </div>
      <div class="col-2 row select-div">
        <span class="text-title col-4">Model</span>
        <input
          @click="modelClick()"
          type="text"
          :value="
            $store.state.listSelectDualModel.length == 0
              ? 'ALL'
              : `${$store.state.listSelectDualModel.length} items`
          "
          class="form-control form-control-sm text-element col-8"
          readonly
        />
      </div>
      <div class="col-2 select-div row">
        <span class="text-title col-4">Station</span>
        <input
          @click="groupClick()"
          type="text"
          :value="
            $store.state.listSelectDualGroup.length == 0
              ? 'ALL'
              : `${$store.state.listSelectDualGroup.length} items`
          "
          class="form-control form-control-sm text-element col-8"
          readonly
        />
      </div>
    </div>
    <div
      class="row col-12"
      style="background-color: #4e7da6; padding-bottom: 10px; padding-top: 10px"
    >
      <div
        class="div-option-uncheck text-title btn"
        :class="{ option_active: opt1 }"
        @click="clickOption('opt1')"
      >
        {{ $store.state.language == "En" ? "Lock" : "Khóa" }}
      </div>
      <div
        class="div-option-uncheck text-title btn"
        :class="{ option_active: opt2 }"
        @click="clickOption('opt2')"
      >
        {{ $store.state.language == "En" ? "Unlock" : "Mở khóa" }}
      </div>
      <div
        class="div-option-uncheck text-title btn"
        :class="{ option_active: opt3 }"
        @click="clickOption('opt3')"
      >
        {{ $store.state.language == "En" ? "Insert" : "Thêm" }}
      </div>
      <div
        class="div-option-uncheck text-title btn"
        :class="{ option_active: opt4 }"
        @click="clickOption('opt4')"
      >
        {{ $store.state.language == "En" ? "Update" : "Cập nhật" }}
      </div>
      <div
        class="div-option-uncheck text-title btn"
        :class="{ option_active: opt5 }"
        @click="clickOption('opt5')"
      >
        {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
      </div>
    </div>
    <div
      class="row col-12"
      style="background-color: #4e7da6; padding-bottom: 5px"
    ></div>
    <div class="div-export row">
      <div><span></span></div>
      <div v-if="$store.state.listLock.length > 0">
        <img
          class="img-excel"
          @click="ExportExel"
          src="../assets/img/excel_64.png"
          alt=""
        />
        <img
        @click="exportExcelCSV()"
        class="img-excel"
        src="../assets/img/xlsx-icon.jpg"
        alt=""
        />
      </div>
    </div>
    <div class="row col-12 div-table">
      <table
        id="tableMain"
        class="table table-condensed table-bordered table-sm"
      >
        <tr>
          <th v-for="(item, index) in qmDataTableHeader" :key="index">
            {{ item }}
          </th>
        </tr>
        <template v-for="(item, index) in $store.state.listLock" :key="index">
          <template v-if="item.STATUS == 'LOCK'">
            <tr
              v-contextmenu:contextmenuLock
              class="tr-red"
              @contextmenu="handleRightClick($event, item)"
            >
              <td v-for="(item1, index1) in item" :key="index1">{{ item1 }}</td>
            </tr>
          </template>
          <template v-else>
            <tr
              v-contextmenu:contextmenuNormal
              @contextmenu="handleRightClick($event, item)"
            >
              <td v-for="(item1, index1) in item" :key="index1">{{ item1 }}</td>
            </tr>
          </template>
        </template>
      </table>
    </div>
  </div>
</template>

<script>
import Repository from "../services/Repository";

import listTimeFrom from "../data/timeFrom";
import listTimeTo from "../data/timeTo";
export default {
  name: "QM",
  components: {},
  data() {
    return {
      isAllTime: true,
      dateFrom: new Date(),
      dateTo: new Date(),
      listTimeFrom: listTimeFrom,
      listTimeTo: listTimeTo,
      timeFrom: "ALL",
      timeTo: "",
      value: null,
      options: [
        "Batman",
        "Robin",
        "Joker",
        "Robin1",
        "Joker1",
        "Robin2",
        "Joker3",
        "Robin4",
        "Joker5",
      ],
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
      lockItem: {},
    };
  },
  methods: {
    ExportExel() {
      var tab_text = "<table border='2px'><tr bgcolor='#418BCA'>";
      var j = 0;
      var tab = document.getElementById("tableMain");
      for (j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
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
    exportExcelCSV() {
      const items = this.qmDataTable;
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
    showModalAddEdit(type) {
      this.$store.commit("changeLockModalState", type);
      this.$store.commit("closeModal");
      this.$store.commit("showModalLock");
    },
    EditLock() {
      this.$store.commit("disableElementLock");
      this.$store.commit("changeLockModalState", "EDIT");
      this.$store.commit("showModalLock");
    },
    UnLock() {
      this.$store.commit("lockStatusModal", false);
      this.$store.commit("showModalLockUnlock");
    },
    DeleteLock() {
      this.$store.commit("showModalEmpPass");
    },
    LockLock() {
      this.$store.commit("lockStatusModal", true);
      this.$store.commit("showModalLockUnlock");
    },
    handleRightClick: function (e, item) {
      this.lockItem = item;
      console.log(this.lockItem);
      this.$store.commit("changeLockEmpPass", "");
      this.$store.commit("changeLockModelName", item.MODEL_NAME);
      this.$store.commit("changeLockType", item.TYPE);
      this.$store.commit("changeLockValue", item.CONDITION);
      this.$store.commit("changeLockGroupName", item.GROUP_NAME);

      e.preventDefault();
    },
    async GetLockElement(typeSearch, dataCommit, modalSearch) {
      this.$store.commit("selectOneElement");
      var payload = {
        database_name: localStorage.databaseName,
        type: typeSearch,
        listModel: this.$store.state.listSelectDualModel,
        listGroup: this.$store.state.listSelectDualGroup,
      };
      var { data } = await Repository.getRepo("GetLockElement", payload);
      this.$store.commit(dataCommit, data.data);
      this.$store.commit(modalSearch);
    },
    modelClick() {
      this.GetLockElement("model", "UpdateListSelectModel", "showModalModel");
    },
    groupClick() {
      this.GetLockElement("model", "UpdateListSelectGroup", "showModalGroup");
    },
    async doQuery() {
      this.$store.commit("ClearListLock");
      var type_search = "";
      if (this.opt1) {
        type_search = "LOCK";
      } else if (this.opt2) {
        type_search = "UNLOCK";
      } else if (this.opt3) {
        type_search = "INSERT";
      } else if (this.opt4) {
        type_search = "UPDATE";
      } else if (this.opt5) {
        type_search = "DELETE";
      }

      console.log(type_search);
      if (this.isAllTime) {
        var start_datetime = "";
        var end_datetime = "";
      } else {
        if (this.timeFrom == "ALL") {
          this.timeFrom = "00:00";
          this.timeTo = "23:59";
        } else {
          if (this.timeTo == "") {
            this.timeTo = "23:59";
          }
        }
        start_datetime = `${this.pad(this.dateFrom.getFullYear())}-${this.pad(
          this.dateFrom.getMonth() + 1
        )}-${this.pad(this.dateFrom.getDate())} ${this.timeFrom}`;
        end_datetime = `${this.pad(this.dateTo.getFullYear())}-${this.pad(
          this.dateTo.getMonth() + 1
        )}-${this.pad(this.dateTo.getDate())} ${this.timeTo}`;
      }
      var model_name = "";
      var group_name = "";
      if (this.$store.state.listSelectDualModel.length > 0) {
        model_name = this.$store.state.listSelectDualModel[0].VALUE;
      }
      if (this.$store.state.listSelectDualGroup.length > 0) {
        model_name = this.$store.state.listSelectDualGroup[0].VALUE;
      }
      var payload = {
        database_name: localStorage.databaseName,
        model_name: model_name,
        group_name: group_name,
        action_type: type_search,
        start_time: start_datetime,
        end_time: end_datetime,
      };
      var { data } = await Repository.getRepo("LockHistory", payload);
      this.qmDataTable = data.data;
      this.$store.commit("updateListLock", this.qmDataTable);
      if (this.qmDataTable.length != 0) {
        this.qmDataTableHeader = Object.keys(this.qmDataTable[0]);
      }
    },
    clickOption(value) {
      this.setAllToUnActive();
      if (value == "opt1") {
        this.opt1 = true;
      } else if (value == "opt2") {
        this.opt2 = true;
      } else if (value == "opt3") {
        this.opt3 = true;
      } else if (value == "opt4") {
        this.opt4 = true;
      } else if (value == "opt5") {
        this.opt5 = true;
      }
      this.doQuery();
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
    document.title = "Lock History";
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
    width: 60px !important;
    line-height: 30px !important;
  }
  .table-condensed {
    font-size: 10px !important;    
  }
}

.div-export {
  display: flex;
  justify-content: space-between;
}
.img-excel {
  height: 40px;
  width: 40px;
  margin-right: 30px;
  cursor: pointer;
}
.text-context {
  margin-left: 20px;
}
.tr-red {
  background-color: red;
  td {
    color: #fff;
  }
}
.div-table table tr th {
  position: sticky;
  top: 0;
  z-index: 2;
  overflow-x: auto;
  white-space: nowrap;
}
.div-table table tr td {
  overflow-x: auto;
  white-space: nowrap;
}
.div-table {
  overflow: auto;
  min-height: 76vh;
  max-height: 76vh;
}
.table-condensed {
  font-size: 13px;
  margin-top: 10px;
  tr {
    td {
      font-weight: 550;
    }
  }
}
.table-condensed tr {
  user-select: none;
  &:hover {
    background-color: #f1baf2;
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
  color: #000;
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
  box-shadow: 1px 2px 2px #9b9696;
}
.div-option-uncheck {
  text-align: center;
  user-select: none;
  background-color: #a0d0ff;
  box-shadow: 1px 1px 1px #6b6b6b;
  margin: 2px;
  height: 40px;
  width: 100px;
  text-align: center;
  line-height: 30px;
  border-radius: 10px;
  color: #000f15;
  &:hover {
    cursor: pointer;
  }
}
.div-main {
  margin-left: 20px;
}
</style>
<style src="@vueform/multiselect/themes/default.css"></style>