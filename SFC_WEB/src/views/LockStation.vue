<template>
  <div class="div-main">
    <div class="div-top row col-lg-12" style="background-color: #4e7da6">
      <div class="col-1">
        <button @click="showModalAddEdit('ADD')" class="button btn btn-success">
          {{ $store.state.language == "En" ? "Add" : "Thêm" }}
        </button>
      </div>
      <div class="col-2">
        <div class="form-check">
          <input
            class="form-check-input"
            type="radio"
            name="flexRadioDefault"
            id="flexRadioDefault1"
            v-model="sortBy"
            value="netgear"
            checked
          />
          <label class="form-check-label" for="flexRadioDefault1">
            Netgear
          </label>
        </div>
        <div class="form-check">
          <input
            class="form-check-input"
            type="radio"
            name="flexRadioDefault"
            id="flexRadioDefault2"
            v-model="sortBy"
            value="arlo"
          />
          <label class="form-check-label" for="flexRadioDefault2"> Arlo </label>
        </div>
        <div class="form-check">
          <input
            class="form-check-input"
            type="radio"
            name="flexRadioDefault"
            id="flexRadioDefault3"
            v-model="sortBy"
            value="eero"
          />
          <label class="form-check-label" for="flexRadioDefault3"> EERO </label>
        </div>
        <div class="form-check">
          <input
            class="form-check-input"
            type="radio"
            name="flexRadioDefault"
            id="flexRadioDefault4"
            v-model="sortBy"
            value="PPLES"
          />
          <label class="form-check-label" for="flexRadioDefault4">
            PPLES
          </label>
        </div>
        <div class="form-check">
          <input
            class="form-check-input"
            type="radio"
            name="flexRadioDefault"
            id="flexRadioDefault5"
            v-model="sortBy"
            value="OTHERS"
          />
          <label class="form-check-label" for="flexRadioDefault5">
            OTHERS
          </label>
        </div>
      </div>
      <div class="col-2 select-div">
        <span class="text-title">Model</span>
        <input
          @click="modelClick()"
          type="text"
          :value="
            $store.state.listSelectDualModel.length == 0
              ? 'ALL'
              : `${$store.state.listSelectDualModel.length} items`
          "
          class="form-control form-control-sm text-element"
          readonly
        />
      </div>
      <div class="col-2 select-div">
        <span class="text-title">Group name</span>
        <input
          @click="groupClick()"
          type="text"
          :value="
            $store.state.listSelectDualGroup.length == 0
              ? 'ALL'
              : `${$store.state.listSelectDualGroup.length} items`
          "
          class="form-control form-control-sm text-element"
          readonly
        />
      </div>
      <div class="col-1"></div>
      <div class="col-4 row div-button">
        <div
          class="div-option-uncheck text-title"
          :class="{ option_active: opt1 }"
          @click="clickOption('opt1')"
        >
          {{ $store.state.language == "En" ? "All" : "Tất cả" }}
        </div>
        <div
          class="div-option-uncheck text-title"
          :class="{ option_active: opt2 }"
          @click="clickOption('opt2')"
        >
          {{ $store.state.language == "En" ? "Normal" : "Bình thường" }}
        </div>
        <div
          class="div-option-uncheck text-title"
          :class="{ option_active: opt3 }"
          @click="clickOption('opt3')"
        >
          {{ $store.state.language == "En" ? "Locking" : "Đang khóa" }}
        </div>
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
      </div>
    </div>
    <div class="row col-12 div-table">
      <table id="tableMain" class="mytable">
        <thead>
          <tr>
            <template v-for="(item, index) in qmDataTableHeader" :key="index">
              <th>
                {{ item }}
              </th>
            </template>
          </tr>
        </thead>
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
      <v-contextmenu ref="contextmenuLock">
        <v-contextmenu-item @click="EditLock()">
          <Icon icon="edit" class="sidenav-icon" />
          <span class="text-context">{{
            $store.state.language == "En" ? "Edit" : "Sửa"
          }}</span>
        </v-contextmenu-item>
        <v-contextmenu-item @click="UnLock()">
          <Icon icon="key" class="sidenav-icon" />
          <span class="text-context">{{
            $store.state.language == "En" ? "Unlock" : "Mở khóa"
          }}</span>
        </v-contextmenu-item>
        <v-contextmenu-item @click="DeleteLock()">
          <Icon icon="trash-alt" class="sidenav-icon" />
          <span class="text-context">{{
            $store.state.language == "En" ? "Delete" : "Xóa"
          }}</span>
        </v-contextmenu-item>
      </v-contextmenu>
      <v-contextmenu ref="contextmenuNormal">
        <v-contextmenu-item @click="EditLock()">
          <Icon icon="edit" class="sidenav-icon" />
          <span class="text-context">{{
            $store.state.language == "En" ? "Edit" : "Sửa"
          }}</span>
        </v-contextmenu-item>
        <v-contextmenu-item @click="LockLock()">
          <Icon icon="lock" class="sidenav-icon" />
          <span class="text-context">{{
            $store.state.language == "En" ? "Lock station" : "Khóa trạm"
          }}</span>
        </v-contextmenu-item>
        <v-contextmenu-item @click="DeleteLock()">
          <Icon icon="trash-alt" class="sidenav-icon" />
          <span class="text-context">{{
            $store.state.language == "En" ? "Delete" : "Xóa"
          }}</span>
        </v-contextmenu-item>
      </v-contextmenu>
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
      dateFrom: new Date(),
      dateTo: new Date(),
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
      sortBy: "netgear",
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
      //do stuff
      this.lockItem = item;
      console.log(this.lockItem);
      this.$store.commit("changeLockEmpPass", "");
      this.$store.commit("changeLockModelName", item.MODEL_NAME);
      this.$store.commit("changeLockType", item.TYPE);
      this.$store.commit("changeLockValue", item.CONDITION);
      this.$store.commit("changeLockGroupName", item.GROUP_NAME);

      e.preventDefault();
    },
    async modelClick() {
      this.timeToChange();
      this.timeFromChange();
      var payload = {
        database_name: localStorage.databaseName,
        customer: this.sortBy,
        type: "model",
        listModel: this.$store.state.listSelectDualModel,
        listGroup: this.$store.state.listSelectDualGroup,
      };
      var { data } = await Repository.getRepo("GetLockElement", payload);
      this.$store.commit("UpdateListSelectModel", data.data);
      this.$store.commit("showModalModel");
    },
    async groupClick() {
      this.timeToChange();
      this.timeFromChange();
      var payload = {
        database_name: localStorage.databaseName,
        customer: this.sortBy,
        type: "group",
        listModel: this.$store.state.listSelectDualModel,
        listGroup: this.$store.state.listSelectDualGroup,
      };
      var { data } = await Repository.getRepo("GetLockElement", payload);
      this.$store.commit("UpdateListSelectGroup", data.data);
      this.$store.commit("showModalGroup");
    },
    async doQuery() {
      this.$store.commit("ClearListLock");
      this.qmDataTableHeader = [];
      var type_search = "";
      if (this.opt1) {
        type_search = "ALL";
      } else if (this.opt2) {
        type_search = "NORMAL";
      } else {
        type_search = "LOCK";
      }
      var payload = {
        database_name: localStorage.databaseName,
        customer: this.sortBy,
        type: type_search,
        listModel: this.$store.state.listSelectDualModel,
        listGroup: this.$store.state.listSelectDualGroup,
      };
      var { data } = await Repository.getRepo("GetLockDetail", payload);
      this.qmDataTable = data.data;

      this.qmDataTable = this.qmDataTable.filter((p) => p.ERROR_CODE != "AQL");
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
    timeFromChange() {
      if (this.timeFrom == "ALL") {
        this.timeTo = "";
        var updateTime1 = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
          this.dateFrom.getMonth() + 1
        )}${this.pad(this.dateFrom.getDate())}0000`;
        //console.log(`${updateTime1}`);
        this.$store.commit("updateDateTimeFromQM", updateTime1);
      } else {
        var updateTime = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
          this.dateFrom.getMonth() + 1
        )}${this.pad(this.dateFrom.getDate())}${this.timeFrom
          .toString()
          .replace(":", "")}`;
        //console.log(`${updateTime}`);
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
        // console.log(`${updateTime1}`);
        this.$store.commit("updateDateTimeToQM", updateTime1);
      } else {
        var updateTime = `${this.pad(this.dateTo.getFullYear())}${this.pad(
          this.dateTo.getMonth() + 1
        )}${this.pad(this.dateTo.getDate())}${this.timeTo
          .toString()
          .replace(":", "")}`;
        //console.log(`${updateTime}`);
        this.$store.commit("updateDateTimeToQM", updateTime);
      }
    },
  },
  mounted() {
    document.title = "Lock Station";
    this.timeToChange();
    this.timeFromChange();
    this.$store.commit("RefreshState");
  },
};
</script>

<style lang="scss" scoped>
* {
  color: #000;
}
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
}
.form-check-label {
  font-size: 13px;
  font-weight: 550;
  color: #fffbf7;
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
  background-color: #c11313;
  td {
    color: #fff;
  }
}
.div-table {
  min-height: 76vh;
  max-height: 76vh;
  width: 100%;
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

.table-condensed tr {
  user-select: none;
  &:hover {
    background-color: #a00c0c;
    color: #fff;
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
  font-size: 14px;
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
.div-button {
  margin-top: 10px;
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
  line-height: 40px;
  border-radius: 10px;
  color: #000f15;
  &:hover {
    cursor: pointer;
  }
}
.button {
  margin-top: 10px;
  appearance: none;
  outline: none;
  border: none;
  cursor: pointer;
  height: 40px;
  width: 60px;
  display: inline-block;
  background: rgb(13, 206, 171);
  //background-image: linear-gradient(to right, rgb(28, 255, 55), #ec8236);
  border-radius: 10px;
  color: #fff;
  font-size: 13px;
  font-weight: 700;

  box-shadow: 0.5px 0.5px rgba(0, 0, 0, 0.4);
  transition: 0.4s ease-out;

  &:hover {
    box-shadow: 1.5px 1.5px rgba(0, 0, 0, 0.6);
  }
}
.div-main {
  margin-left: 20px;
}
// .div-top {
//   padding-top: 5px;
//   padding-bottom: 5px;
//   display: flex;
//   justify-content: space-between;
// }
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
    &:hover {
      background: cornsilk;
    }
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
</style>