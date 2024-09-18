<template>
  <div class="content_main">
    <div class="div-content-top">
      <div v-show="!isHideMenu" class="row col-12">
        <table class="col-7">
          <tr class="row" style="margin-bottom: 5px; margin-left: 2px">
            <td class="col-3">
              <div class="row col-12">
                <input
                  id="checkboxShowLine"
                  style="margin-top: 8px; margin-right: 8px"
                  type="checkbox"
                  v-model="checkboxShowLine"
                />
                <label for="checkboxShowLine"
                  ><b class="titleType">Show line</b></label
                >
              </div>
            </td>
            <td class="col-4">
              <div class="row">
                <div class="col-4">
                  <b class="titleType">MO TYPE</b>
                </div>
                <div class="col-8">
                  <SelectionSearch
                    :isDisable="false"
                    :keySearch="motypeSearch"
                    :list="motypeList"
                    @lostFocus="lostFocusMoType"
                    @hide-modal="HideModal"
                    :isShowSelect="motypeShow"
                    @clickInput="motypeClickInput"
                    @searchChange="searchChangeMoType"
                    @selectKey="selectKeyMoType"
                  />
                </div>
              </div>
            </td>
            <td class="col-4">
              <div class="row">
                <div class="col-4">
                  <b class="titleType">MO</b>
                </div>
                <div class="col-8">
                  <input
                    @click="moClick()"
                    type="text"
                    :value="
                      $store.state.listSelectDualMO.length == 0
                        ? 'ALL'
                        : `${$store.state.listSelectDualMO[0].VALUE} ...`
                    "
                    class="form-control form-control-sm text-element"
                    readonly
                  />
                </div>
              </div>
            </td>
          </tr>
          <tr class="row" style="margin-left: 2px">
            <td class="col-3">
              <div class="form-check">
                <input
                  class="form-check-input"
                  type="radio"
                  name="flexRadioDefault"
                  id="flexRadioDefault1"
                  v-model="sortBy"
                  value="mo"
                  checked
                />
                <label class="form-check-label" for="flexRadioDefault1">
                  {{
                    $store.state.language == "En"
                      ? "Sort by MO"
                      : "Sắp xếp theo MO"
                  }}
                </label>
              </div>
              <div class="form-check">
                <input
                  class="form-check-input"
                  type="radio"
                  name="flexRadioDefault"
                  id="flexRadioDefault2"
                  v-model="sortBy"
                  value="model"
                />
                <label class="form-check-label" for="flexRadioDefault2">
                  {{
                    $store.state.language == "En"
                      ? "Sort by Model"
                      : "Sắp xếp theo Model"
                  }}
                </label>
              </div>
            </td>
            <td class="col-4">
              <div class="row">
                <div class="col-4">
                  <b class="titleType">MODEL</b>
                </div>
                <div class="col-8">
                  <input
                    @click="modelClick()"
                    type="text"
                    :value="
                      $store.state.listSelectDualModel.length == 0
                        ? 'ALL'
                        : `${$store.state.listSelectDualModel[0].VALUE}`
                    "
                    class="form-control form-control-sm text-element"
                    readonly
                  />
                </div>
              </div>
            </td>
            <td class="col-4">
              <div class="row">
                <div class="col-4">
                  <b class="titleType">SECTION</b>
                </div>
                <div class="col-8">
                  <SelectionSearch
                    :isDisable="isDisableSection"
                    :keySearch="sectionSearch"
                    :list="sectionList"
                    @lostFocus="lostFocusSection"
                    :isShowSelect="sectionShow"
                    @clickInput="sectionClickInput"
                    @searchChange="searchChangeSection"
                    @selectKey="selectKeySection"
                  />
                </div>
              </div>
            </td>
          </tr>
        </table>
        <div class="col-1">
          <div class="form-check">
            <input
              class="form-check-input"
              type="radio"
              name="flexRadioMOType"
              id="flexRadioMOType"
              v-model="moType"
              value="All"
              checked
            />
            <label class="form-check-label" for="flexRadioMOType"> All </label>
          </div>
          <div class="form-check">
            <input
              class="form-check-input"
              type="radio"
              name="flexRadioMOType"
              id="flexRadioMOType1"
              v-model="moType"
              value="online"
            />
            <label class="form-check-label" for="flexRadioMOType1">
              MO online
            </label>
          </div>
          <div class="form-check">
            <input
              class="form-check-input"
              type="radio"
              name="flexRadioMOType"
              id="flexRadioMOType2"
              v-model="moType"
              value="closed"
            />
            <label class="form-check-label" for="flexRadioMOType2">
              MO closed
            </label>
          </div>
        </div>
        <div class="col-2 div-search">
          <button @click="queryAmbit" class="btn-query btn col-12">
            <span class="title-button-search">{{
              $store.state.language == "En" ? "Query" : "Tìm"
            }}</span>
          </button>
        </div>
        <div class="col">
          <div class="column">
            <div>
              <input
                class="checkbox-group"
                v-model="isFG"
                id="checkMo"
                type="checkbox"
              />
              <label class="label-group" for="checkMo">FG</label>
            </div>
            <div>
              <input
                class="checkbox-group"
                v-model="isLink"
                id="notLink"
                type="checkbox"
              />
              <label class="label-group" for="notLink">Not link</label>
            </div>
            <div>
              <input
                class="checkbox-group"
                v-model="isORTWip"
                id="ortWip"
                type="checkbox"
              />
              <label class="label-group" for="ortWip">ORT Wip</label>
            </div>
          </div>
        </div>
      </div>
      <div class="border arrow-hide" @click="MenuToggle()">
        <template v-if="!isHideMenu">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            width="24"
            height="24"
          >
            <path fill="none" d="M0 0h24v24H0z" />
            <path
              fill="#FFF"
              d="M12 10.828l-4.95 4.95-1.414-1.414L12 8l6.364 6.364-1.414 1.414z"
            />
          </svg>
        </template>
        <template v-else>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            width="24"
            height="24"
          >
            <path fill="none" d="M0 0h24v24H0z" />
            <path
              fill="#FFF"
              d="M12 13.172l4.95-4.95 1.414 1.414L12 16 5.636 9.636 7.05 8.222z"
            />
          </svg>
        </template>
      </div>
    </div>
    <!-- <div class="div-export row col-12">
      <div></div>      
    </div> -->
    <div class="col-12 row" v-if="listResult.length > 0">
      <div class="col-3">
        <p class="text-total">
          MO Count = <span class="text-total-count"> {{ moQTY }} </span>
        </p>
      </div>
      <div class="col-3">
        <p class="text-total">
          Model Count = <span class="text-total-count"> {{ modelQTY }} </span>
        </p>
      </div>
      <div class="col">
        <p class="text-total">
          Query Time <span class="text-total-count"> {{ executeTime }} </span>
        </p>
      </div>
      <div class="pull-right" v-if="listResult.length > 0">
        <img
          class="img-excel"
          @click="ExportExel"
          src="../assets/img/excel_64.png"
          alt=""
        />
      </div>
    </div>
    <div class="div-table">
      <table class="mytable" id="tableMain">
        <thead>
          <tr>
            <template v-for="(item, index) in listHeader" :key="index">
              <th>
                {{ item }}
              </th>
            </template>
          </tr>
        </thead>
        <tr v-for="(item, index) in listResult" :key="index">
          <template v-for="(itemContent, index1) in item" :key="index1">
            <template
              v-if="
                item.ON_LINE >= 3 &&
                item.ON_LINE < 8 &&
                index != listResult.length - 1 &&
                index1 != 'MO' &&
                index1 != 'MODEL' &&
                index1 != 'MO QTY' &&
                index1 != 'VERSION' &&
                index1 != 'TA' &&
                index1 != 'LINE' &&
                index1 != 'ON_LINE'
              "
            >
              <td
                v-if="itemContent > 0"
                style="background: green; color: #fff"
                @click="ShowDetail(item.MO, index1, item.LINE, index1)"
              >
                <a class="red" href="javascript:void(0)"
                  ><b>{{ itemContent }}</b></a
                >
              </td>
              <!-- <td v-else-if="itemContent==0" style="color: #fff">0</td> -->
              <td v-else style="background: green; color: #fff"></td>
            </template>
            <template
              v-else-if="
                item.ON_LINE >= 6 &&
                item.ON_LINE < 9 &&
                index != listResult.length - 1 &&
                index1 != 'MO' &&
                index1 != 'MODEL' &&
                index1 != 'MO QTY' &&
                index1 != 'VERSION' &&
                index1 != 'TA' &&
                index1 != 'LINE' &&
                index1 != 'ON_LINE'
              "
            >
              <td
                v-if="itemContent > 0"
                style="background: #fff72e; border: 0.5px solid #cdc"
                @click="ShowDetail(item.MO, index1, item.LINE, index1)"
              >
                <a style="color: #c42b1b" href="javascript:void(0)"
                  ><b>{{ itemContent }}</b></a
                >
              </td>
              <td
                v-else-if="itemContent == 0"
                style="background: #fff72e; border: 0.5px solid #cdc"
              ></td>
              <td
                v-else
                style="
                  background: #fff72e;
                  color: #c42b1b;
                  border: 0.5px solid #cdc;
                "
              ></td>
            </template>
            <template
              v-else-if="
                item.ON_LINE >= 9 &&
                index != listResult.length - 1 &&
                index1 != 'MO' &&
                index1 != 'MODEL' &&
                index1 != 'MO QTY' &&
                index1 != 'VERSION' &&
                index1 != 'TA' &&
                index1 != 'LINE' &&
                index1 != 'ON_LINE'
              "
            >
              <td
                v-if="itemContent > 0"
                style="background: #c11313; color: #000"
                @click="ShowDetail(item.MO, index1, item.LINE, index1)"
              >
                <a class="red" href="javascript:void(0)"
                  ><b>{{ itemContent }}</b></a
                >
              </td>
              <!-- <td v-else-if="itemContent==0" style="background: #c11313; color: #FFF">0</td> -->
              <td v-else style="background: #c11313; color: #000"></td>
            </template>
            <template
              v-else-if="
                index != listResult.length - 1 &&
                index1 != 'MO' &&
                index1 != 'MODEL' &&
                index1 != 'MO QTY' &&
                index1 != 'VERSION' &&
                index1 != 'TA' &&
                index1 != 'LINE' &&
                index1 != 'ON_LINE'
              "
            >
              <td @click="ShowDetail(item.MO, index1, item.LINE, index1)">
                <a style="color: #000" href="javascript:void(0)"
                  ><b v-if="itemContent != '0'">{{ itemContent }}</b></a
                >
              </td>
            </template>
            <template v-else>
              <td>
                <b v-if="itemContent != '0'" style="color: #000">{{
                  itemContent
                }}</b>
              </td>
            </template>
          </template>
        </tr>
      </table>
    </div>
    <!-- <table class="table">
        
      </table> -->
  </div>
</template>

<script>
import Repository from "../services/Repository";

import SelectionSearch from "../components/Template/SelectionSearch.vue";

function swap(arra) {
  [arra[0], arra[arra.length - 1]] = [arra[arra.length - 1], arra[0]];
  return arra;
}

export default {
  name: "Ambit",
  components: {
    SelectionSearch,
  },
  created() {},
  data() {
    return {
      isHideMenu: false,
      timer: "",
      isORTWip: false,
      isLink: false,
      isFG: false,
      motypeSearch: "ALL",
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
      moNumberList: [],
      modelNameList: [],
      sectionNameList: [],
      checkboxShowLine: false,
      sortBy: "mo",
      moType: "All",
      listResult: [],
      listHeader: [],
      showModal: false,
      listDetailClick: [],
      isShowModal: false,
      isDisableSection: true,
      moQTY: 0,
      modelQTY: 0,
      executeTime: "",
    };
  },
  mounted() {
    document.title = "Ambit";
    this.motypeList = this.basemotypeList;
    this.$store.commit("RefreshState");
  },
  unmounted() {
    this.cancelAutoUpdate();
  },
  methods: {
    MenuToggle() {
      this.isHideMenu = !this.isHideMenu;
    },
    HideModal() {
      this.motypeShow = false;
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
    async ShowDetail(mo, group, line, column) {
      var cc = false;
      if (column == "Not link QTY") {
        cc = true;
      }
      var lineName = "";
      if (typeof line == "undefined") {
        lineName = "";
      } else {
        lineName = line;
      }
      var payload = {
        database_name: localStorage.databaseName,
        isNotlink: cc,
        mo_number: mo,
        line_name: lineName,
        group_value: group,
        isFG: this.isFG,
      };
      var { data } = await Repository.getRepo("GetDetailAmbit", payload);
      if (data.result == "ok") {
        this.$store.commit("setListAll", data.data);
        var arr = data.data;
        this.$store.state.listDetailClick = arr.slice(0, 500);
        if (this.$store.state.listDetailClick.length > 0) {
          Object.keys(this.$store.state.listDetailClick[0]).forEach(
            (element) => {
              this.$store.state.listDetailClickHeader.push({
                label: element,
                field: element,
              });
            }
          );
          this.$store.commit("showModal");
        }
      }
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

      var { data } = await Repository.getRepo("GetAmbitElement", payload);
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
    async queryAmbit() {
      var listModel = this.$store.state.listSelectDualModel;
      var model_value = "ALL";
      if (listModel.length > 0) {
        model_value = listModel[0].VALUE;
      }
      var listMo = this.$store.state.listSelectDualMO;
      var mo_value = "ALL";
      if (listMo.length > 0) {
        mo_value = listMo[0].VALUE;
      }
      if (model_value == "ALL" && mo_value == "ALL") {
        if (localStorage.language == "En") {
          this.$swal(
            "",
            "Model and MO can not choose at the same time!",
            "error"
          );
        } else {
          this.$swal("", "Model và Mo không thể cùng là ALL", "error");
        }
      } else {
        this.listResult = [];
        this.listHeader = [];

        var payload = {
          database_name: localStorage.databaseName,
          type: "wip",
          isORTWip: this.isORTWip,
          mo_value: this.$store.state.listSelectDualMO,
          model_value: this.$store.state.listSelectDualModel,
          motype_value: this.motypeSearch,
          section_value: this.sectionSearch,
          mo_state: this.moType,
          isNotlink: this.isLink,
          isFG: this.isFG,
          sort_by: this.sortBy,
          show_line: this.checkboxShowLine,
        };

        var { data } = await Repository.getRepo("GetAmbitElement", payload);
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
    cancelAutoUpdate() {
      clearInterval(this.timer);
    },
    searchChangeMoType(value) {
      this.motypeSearch = value;
      if (value.length != 0) {
        this.motypeList = this.basemotypeList.filter((p) =>
          p.VALUE.toUpperCase().includes(value.toUpperCase())
        );
        this.motypeShow = true;
      } else {
        this.motypeList = this.basemotypeList;
      }
    },
    async searchChangeMo(value) {
      this.moSearch = value;
      var payload = {
        database_name: localStorage.databaseName,
        type: "mo",
        mo_number: this.moSearch,
        model_value: this.$store.state.listSelectDualModel,
        motype_value: this.motypeSearch,
        section_value: this.sectionSearch,
      };
      var { data } = await Repository.getRepo("GetAmbitElement", payload);
      if (data.result == "ok") {
        this.moList = data.data;
        this.moList.push({ VALUE: "ALL" });
        console.log(this.moList);
        swap(this.moList);
        this.moShow = true;
      } else {
        this.moList = [];
        this.moShow = false;
      }
    },
    async searchChangeModel(value) {
      this.modelSearch = value;
      var payload = {
        database_name: localStorage.databaseName,
        type: "model",
        mo_value: this.$store.state.listSelectDualMO,
        model_name: this.modelSearch,
        motype_value: this.motypeSearch,
        section_value: this.sectionSearch,
      };
      var { data } = await Repository.getRepo("GetAmbitElement", payload);
      if (data.result == "ok") {
        this.modelList = data.data;
        this.modelList.push({ VALUE: "ALL" });
        swap(this.modelList);
        this.modelShow = true;
      } else {
        this.modelList = [];
        this.modelShow = false;
      }
    },
    async searchChangeSection(value) {
      this.sectionSearch = value;
      var payload = {
        database_name: localStorage.databaseName,
        type: "section",
        mo_value: this.moSearch,
        model_value: this.modelSearch,
        motype_value: this.motypeSearch,
        section_value: this.sectionSearch,
      };
      var { data } = await Repository.getRepo("GetAmbitElement", payload);
      if (data.result == "ok") {
        this.sectionList = data.data;
        this.sectionList.push({ VALUE: "ALL" });
        swap(this.sectionList);
        this.sectionShow = true;
      } else {
        this.sectionList = [];
        this.sectionShow = false;
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
</style>
