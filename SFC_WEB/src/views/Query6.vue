<template>
  <div class="div-main" style="background-color: #4e7da6">
    <ModalEnterElementQ6 :receiveState="stateFocus" />
    <div class="col-12 row"></div>
    <div v-if="!isHideMenu">
      <div class="col-12 row" style="padding-top: 10px; padding-bottom: 5px">
        <template v-for="(item, index) in menuQ6" :key="index">
          <template v-if="item.children.length > 0">
            <div
              class="item-top col nav-link"
              data-toggle="dropdown"
              aria-haspopup="true"
              aria-expanded="false"
            >
              <div class="div-icon-item">
                <Icon :icon="item.icon" class="sidenav-icon" />
              </div>
              <span>{{ item.name }}</span>
            </div>
            <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
              <a
                v-for="(childItem, childIndex) in item.children"
                :key="childIndex"
                class="dropdown-item text-dropdown"
                @click="EnterElement(childItem.value)"
                href="javascript:void(0)"
                >{{ childItem.name }}</a
              >
            </div>
          </template>
          <template v-else>
            <div class="item-top col nav-link" @click="EnterElement(item.name)">
              <div class="div-icon-item">
                <Icon :icon="item.icon" class="sidenav-icon" />
              </div>
              <span>{{ item.name }}</span>
            </div>
          </template>
        </template>
      </div>
      <div class="top-left div-top col-12 row">
        <div class="col row select-div">
          <!-- <span class="text-title">Type</span> -->
          <select
            v-model="chooseOption"
            name="select"
            id=""
            @change="SortRepair()"
            class="form-control form-control-sm select-form"
          >
            <option value="R107" selected>Wip</option>
            <option value="R117">Detail</option>
            <option value="Z107">FG</option>
            <option value="R109">Repair</option>
            <option value="ATEDATA">TMP_ATEDATA</option>            
            <!-- <option value="Antena">Link Antena</option>
            <option value="WipSMT">Wip SMT</option> -->
          </select>
        </div>
        <div class="col">
          <!-- <span class="text-title">Station</span> -->
          <input
            @click="groupClick()"
            type="text"
            :value="
              $store.state.listSelectDualGroup.length == 0
                ? 'ALL'
                : `${$store.state.listSelectDualGroup[0].VALUE}`
            "
            class="form-control form-control-sm text-element"
            readonly
          />
        </div>
        <div class="col row select-div">
          <select
            v-model="chooseField"
            name="select"
            id=""
            class="form-control form-control-sm select-form"
          >
            <option
              v-for="(item, index) in myTableR107"
              :key="index"
              :value="item.VALUE"
            >
              {{ item.VALUE }}
            </option>
          </select>
        </div>
        <div class="col row div-period">
          <input
            id="checkboxPeriod"
            style="margin-left: 20px; margin-top: 8px; margin-right: 8px"
            type="checkbox"
            v-model="period"
          />
          <label for="checkboxPeriod"><b class="titleType">Period</b></label>
        </div>
        <div class="col">
          <datepicker
            :disabled="!period"
            class="form-control form-control-sm shadow-element"
            v-model="dateFrom"
            :upperLimit="dateTo"
            @change="timeFromChange()"
          />
        </div>
        <div class="col">
          <select
            :disabled="!period"
            @change="timeFromChange()"
            v-model="timeFrom"
            class="form-control form-control-sm shadow-element select-time"
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
            :disabled="!period"
            class="form-control form-control-sm"
            v-model="dateTo"
            :lowerLimit="dateFrom"
            @change="timeToChange()"
          />
        </div>
        <div class="col">
          <template v-if="timeFrom == 'ALL' || !period">
            <select
              @change="timeToChange()"
              v-model="timeTo"
              class="form-control form-control-sm shadow-element select-time"
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
      <div class="col-12 row">
        <div class="col-2 row div-period">
          <input
            id="checkboxAddSymbol"
            style="margin-top: 8px; margin-right: 8px"
            type="checkbox"
            v-model="isAddSymbol"
          />
          <label for="checkboxAddSymbol"><b class="titleType">Add @</b></label>
        </div>
        <div class="col-2 row div-period">
          <input
            id="checkboxSubString"
            style="margin-top: 8px; margin-right: 8px"
            type="checkbox"
            v-model="isSubStr"
          />
          <label for="checkboxSubString"
            ><b class="titleType">Sub string</b></label
          >
        </div>
        <div class="col-2 row div-period">
          <input
            class="form-control form-control-sm"
            type="number"
            min="0"
            v-model="subFrom"
            @change="changeSubFrom()"
          />
        </div>
        <div class="col-1 row div-period" style="margin-left: 20px">
          <label for="checkboxSubString"><b class="titleType">To</b></label>
        </div>
        <div class="col-2 row div-period">
          <input
            class="form-control form-control-sm"
            min="0"
            type="number"
            v-model="subTo"
            @change="changeSubTo()"
          />
        </div>
      </div>
    </div>
    <div class="col-12 row">
        <div class="col-2 row div-period">
          <input
            id="checkboxScanQR"
            style="margin-top: 8px; margin-right: 8px"
            type="checkbox"
            v-model="isScanQR"
          />
          <label for="checkboxScanQR"><b class="titleType">Scan QR</b></label>
        </div>
        <div class="col-2 row div-period">
          <input
            id="checkboxPrevMO"
            style="margin-top: 8px; margin-right: 8px"
            type="checkbox"
            v-model="isPrevMO"
          />
          <label for="checkboxPrevMO"><b class="titleType">Preview MO_NUMBER</b></label>
        </div>
    </div>
    <div class="col-12">
      <div class="div-searchbox">
        <div class="div-searchbox-content">
          <input
            v-on:keyup.enter="querySearch()"
            @change="valueScanQR()"
            v-model="valueSearch"
            type="text"
            ref="input"
            class="form-control"
            @click="selectAll"
            :placeholder="
              $store.state.language == 'En'
                ? 'Enter everything you want...'
                : 'Nhập mọi thứ vào đây...'
            "
          />
          <label @click="querySearch()" class="btn btn-search">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              width="24"
              height="24"
            >
              <path fill="none" d="M0 0h24v24H0z" />
              <path
                fill="#FFF"
                d="M18.031 16.617l4.283 4.282-1.415 1.415-4.282-4.283A8.96 8.96 0 0 1 11 20c-4.968 0-9-4.032-9-9s4.032-9 9-9 9 4.032 9 9a8.96 8.96 0 0 1-1.969 5.617zm-2.006-.742A6.977 6.977 0 0 0 18 11c0-3.868-3.133-7-7-7-3.868 0-7 3.132-7 7 0 3.867 3.132 7 7 7a6.977 6.977 0 0 0 4.875-1.975l.15-.15z"
              />
            </svg>
          </label>
          <label for="ScanImg" class="btn btn-img" @click="TurnCamera()">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              width="24"
              height="24"
            >
              <path fill="none" d="M0 0h24v24H0z" />
              <path
                fill="#000"
                d="M9 3h6l2 2h4a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V6a1 1 0 0 1 1-1h4l2-2zm3 16a6 6 0 1 0 0-12 6 6 0 0 0 0 12zm0-2a4 4 0 1 1 0-8 4 4 0 0 1 0 8z"
              />
            </svg>
          </label>
          <label for="ChooseImg" class="btn btn-img">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              width="24"
              height="24"
            >
              <path fill="none" d="M0 0h24v24H0z" />
              <path
                fill="#000"
                d="M20 5H4v14l9.292-9.294a1 1 0 0 1 1.414 0L20 15.01V5zM2 3.993A1 1 0 0 1 2.992 3h18.016c.548 0 .992.445.992.993v16.014a1 1 0 0 1-.992.993H2.992A.993.993 0 0 1 2 20.007V3.993zM8 11a2 2 0 1 1 0-4 2 2 0 0 1 0 4z"
              />
            </svg>
          </label>
          <qrcode-capture
            id="ChooseImg"
            class="button-chooseimg"
            @decode="onDecode"
            :capture="selected.value"
          />
          <!-- <div class="row border"></div> -->
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
  </div>
  <div class="div-content-bellow">
    <div class="div-export row" v-if="qmDataTable.length > 0">
      <div>
        <span
          ><b class="text-count">
            {{ $store.state.language == "En" ? "Show wip:" : "Hiển thị wip" }}
            <span class="count-number">{{ qmDataTable.length }}</span>
            {{ $store.state.language == "En" ? " of " : " của " }}
            <span class="count-number">{{ allDataTable.length }}</span>
            {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
          ></span
        >
      </div>
      <img
        @click="exportExcel()"
        class="img-excel"
        src="../assets/img/excel_64.png"
        alt=""
      />
    </div>
    <div class="row col-12 div-table">
      <table
        id="tableMain"
        class="table table-condensed table-bordered table-sm"
      >
        <tr>
          <th
            @click="OderBy(item)"
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
                @click="GetInfoDetail(item.SERIAL_NUMBER, index1, item1)"
              >
                {{ item1 }}
              </td>
              <td
                @click="GetInfoDetail(item.SERIAL_NUMBER, index1, item1)"
                v-else-if="index1 == 'WIP_GROUP' || index1 == 'CONTAINER_NO'"
                style="background-color: #ff7a1c; color: #000"
              >
                {{ item1 }}
              </td>
              <td
                @click="GetInfoDetail(item.SERIAL_NUMBER, index1, item1)"
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
    <br />
    <br />
    <template v-if="isSpecialRoute">
      <!-- <div class="div-export row" >        
    </div> -->
      <div class="content" id="content"></div>
    </template>
    <!-- table bellow -->
    <template v-if="subDataTable">
      <div class="div-export row" v-if="subDataTable.length > 0">
        <div>
          <span
            ><b class="text-count">
              {{ $store.state.language == "En" ? "Show:" : "Hiển thị" }}
              <span class="count-number">{{ subDataTable.length }}</span>
              {{ $store.state.language == "En" ? " of " : " của " }}
              <span class="count-number">{{ subDataTable.length }}</span>
              {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
            ></span
          >
        </div>
        <img
          @click="subExportExcel()"
          class="img-excel"
          src="../assets/img/excel_64.png"
          alt=""
        />
      </div>
    </template>
     <div class="row col-sm-12 div-content">
        <template v-if="DataTablePrev">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
                <template v-for="(item, index) in DataTablePrev" :key="index">
                  <th v-if="item != 'ID'">
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <template v-for="(item, index) in DataTableRoute" :key="index">
              <tr>      
                <template v-for="(item1, index1) in item" :key="index1">
                  <td v-if="index1 != 'ID'">{{ item1 }}</td>
                </template>
              </tr>
            </template>
          </table>
        </template>
      </div>
      <div class="row col-sm-12 div-content">
        <template v-if="DataTablePreBom">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
                <template v-for="(item, index) in DataTablePreBom" :key="index">
                  <th v-if="item != 'ID'">
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <template v-for="(item, index) in DataTableBom" :key="index">
              <tr>      
                <template v-for="(item1, index1) in item" :key="index1">
                  <td v-if="index1 != 'ID'">{{ item1 }}</td>
                </template>
              </tr>
            </template>
          </table>
        </template>
      </div>
    <div class="row col-12 div-table">
      <template v-if="subDataTableHeader">
        <table
          id="tableMain"
          class="table table-condensed table-bordered table-sm"
        >
          <tr>
            <th
              @click="subOderBy(item)"
              v-for="(item, index) in subDataTableHeader"
              :key="index"
            >
              {{ item }}
            </th>
          </tr>
          <template v-for="(item, index) in subDataTable" :key="index">
            <tr
              @dblclick="GetLinkData(item.SERIAL_NUMBER)"
              :class="isSelectedRow"
            >
              <template v-for="(item1, index1) in item" :key="index1">
                <td
                  v-if="index1 == 'GROUP_NAME'"
                  style="background-color: #4e7da6; color: #fff"
                >
                  {{ item1 }}
                </td>
                <td
                  v-else-if="index1 == 'WIP_GROUP'"
                  style="background-color: #ff7a1c; color: #000"
                >
                  {{ item1 }}
                </td>
                <td v-else>{{ item1 }}</td>
              </template>
            </tr>
          </template>
        </table>
      </template>
    </div>
  </div>
  <div class="div-scan-all" v-if="!isHideScaner">
    <div class="div-close-scan" @click="HideScanner()">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        width="24"
        height="24"
      >
        <path fill="none" d="M0 0h24v24H0z" />
        <path
          fill="#FFF"
          d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16zm0-9.414l2.828-2.829 1.415 1.415L13.414 12l2.829 2.828-1.415 1.415L12 13.414l-2.828 2.829-1.415-1.415L10.586 12 7.757 9.172l1.415-1.415L12 10.586z"
        />
      </svg>
    </div>
    <div>
      <div id="cccc" class="web-camera-container">
        <div v-show="isCameraOpen && isLoading" class="camera-loading">
          <ul class="loader-circle">
            <li></li>
            <li></li>
            <li></li>
          </ul>
        </div>
        <div
          v-if="isCameraOpen"
          v-show="!isLoading"
          class="camera-box"
          :class="{ flash: isShotPhoto }"
        >
          <div class="camera-shutter" :class="{ flash: isShotPhoto }"></div>
          <video
            v-show="!isPhotoTaken"
            ref="camera"
            :width="450"
            :height="337.5"
            autoplay
          ></video>
          <canvas
            v-show="isPhotoTaken"
            id="photoTaken"
            ref="canvas"
            :width="450"
            :height="337.5"
          ></canvas>
        </div>
        <div
          class="button-take-camera-border"
          v-if="isCameraOpen && !isLoading"
          @click="takePhoto"
        >
          <div class="button-take-camera"></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import Repository from "../services/Repository";

import listTimeFrom from "../data/timeFrom";
import listTimeTo from "../data/timeTo";
import tableR107 from "../data/tableR107";
import tableR109 from "../data/tableR109";
import dataQ6 from "../data/menu_q6";
import ModalEnterElementQ6 from "../components/Modal/ModalEnterElementQ6.vue";

// import { QrcodeCapture, QrcodeStream } from "vue3-qrcode-reader";

import $ from "jquery";
export default {
  name: "QM",
  components: {
    ModalEnterElementQ6,
    // QrcodeCapture,
    // QrcodeStream,
  },
  created() {
    this.myTableR107 = tableR107;
    this.menuQ6 = dataQ6;
    if (typeof localStorage.subFrom == "undefined") {
      localStorage.subFrom = 0;
      this.subFrom = 0;
    } else {
      this.subFrom = localStorage.subFrom;
    }
    if (typeof localStorage.subTo == "undefined") {
      localStorage.subTo = 0;
      this.subTo = 0;
    } else {
      this.subTo = localStorage.subTo;
    }
  },
  data() {
    const options = [
      { text: "rear camera (default)", value: "environment" },
      { text: "front camera", value: "user" },
      { text: "force file dialog", value: false },
    ];
    return {
      result: "",
      options,
      error: "",
      selected: options[0],
      stateFocus: false,
      isHideScaner: true,
      subFrom: 0,
      subTo: 0,
      isAddSymbol: false,
      isSubStr: false,
      menuQ6: [],
      tempValueDetail: "",
      isSelectedRow: true,
      valueSearch: "",
      chooseField: "SERIAL_NUMBER",
      myTableR107: [],
      chooseOption: "R107",
      period: false,
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
      allDataTable: [],
      qmDataTable: [],
      qmDataTableHeader: [],
      subDataTable: [],
      subDataTableHeader: [],
      DataTablePrev : [],
      DataTableRoute: [],
      columnName: [],
      previosSerial: "",
      isSpecialRoute: false,
      isHideMenu: false,
      //Cameraaaa
      isCameraOpen: false,
      isPhotoTaken: false,
      isShotPhoto: false,
      isLoading: false,
      link: "#",
    };
  },
  methods: {
    MaximizeWindow() {
      var docElm = document.documentElement;
      if (docElm.requestFullscreen) {
        docElm.requestFullscreen();
      } else if (docElm.mozRequestFullScreen) {
        docElm.mozRequestFullScreen();
      } else if (docElm.webkitRequestFullScreen) {
        docElm.webkitRequestFullScreen();
      } else if (docElm.msRequestFullscreen) {
        docElm.msRequestFullscreen();
      }

      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitCancelFullScreen) {
        document.webkitCancelFullScreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
    },
    toggleCamera() {
      if (this.isCameraOpen) {
        this.isCameraOpen = false;
        this.isPhotoTaken = false;
        this.isShotPhoto = false;
        this.stopCameraStream();
      } else {
        this.isCameraOpen = true;
        this.createCameraElement();
      }
    },

    createCameraElement() {
      this.isLoading = true;

      const constraints = (window.constraints = {
        audio: false,
        video: true,
      });

      navigator.mediaDevices
        .getUserMedia(constraints)
        .then((stream) => {
          this.isLoading = false;
          this.$refs.camera.srcObject = stream;
        })
        .catch((error) => {
          this.isLoading = false;
          console.log(error);
          alert("May the browser didn't support or there is some errors.");
        });
    },

    stopCameraStream() {
      let tracks = this.$refs.camera.srcObject.getTracks();

      tracks.forEach((track) => {
        track.stop();
      });
    },

    SortRepair(){     
      if(this.chooseOption == "R109"){
        this.myTableR107 = tableR109;
      }else{
        this.myTableR107 = tableR107;
      }   
    },

    async takePhoto() {
      if (!this.isPhotoTaken) {
        this.isShotPhoto = true;

        const FLASH_TIMEOUT = 50;

        setTimeout(() => {
          this.isShotPhoto = false;
        }, FLASH_TIMEOUT);
      }

      this.isPhotoTaken = !this.isPhotoTaken;

      const context = this.$refs.canvas.getContext("2d");
      console.log(this.$refs.camera);
      context.drawImage(this.$refs.camera, 0, 0, 450, 337.5);
      var jpeg = this.$refs.canvas.toDataURL("image/jpeg");
      // console.log(jpeg);
      var payload = {
        database: localStorage.databaseName,
        value: jpeg,
      };
      var { data } = await Repository.getRepo("ScanBarcode", payload);
      if (data.barcode == "null") {
        alert(`Can not detect barcode or no data.`);
        this.isPhotoTaken = !this.isPhotoTaken;
      } else {
        this.selectAll();
        var $elem = $(".div-table");
        this.allDataTable = data.data;
        this.subDataTable = data.data1;
        if (this.allDataTable.length > 0) {
          this.tempValueDetail = this.allDataTable[0].SERIAL_NUMBER;
        } else {
          this.tempValueDetail = "";
        }
        this.qmDataTable = this.allDataTable.slice(0, 500);

        if (this.qmDataTable.length != 0) {
          this.qmDataTableHeader = Object.keys(this.qmDataTable[0]);
          this.qmDataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
        if (this.subDataTable && this.subDataTable.length > 0) {
          this.subDataTableHeader = Object.keys(this.subDataTable[0]);
          this.subDataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
        $elem.scrollLeft = 10;
        if (document.exitFullscreen) {
          document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
          document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
          document.webkitCancelFullScreen();
        } else if (document.msExitFullscreen) {
          document.msExitFullscreen();
        }
        this.HideScanner();
      }
      // console.log(data);
    },

    downloadImage() {
      const download = document.getElementById("downloadPhoto");
      const canvas = document
        .getElementById("photoTaken")
        .toDataURL("image/jpeg")
        .replace("image/jpeg", "image/octet-stream");
      download.setAttribute("href", canvas);
    },

    TurnCamera() {
      var docElm = document.documentElement;
      if (docElm.requestFullscreen) {
        docElm.requestFullscreen();
      } else if (docElm.mozRequestFullScreen) {
        docElm.mozRequestFullScreen();
      } else if (docElm.webkitRequestFullScreen) {
        docElm.webkitRequestFullScreen();
      } else if (docElm.msRequestFullscreen) {
        docElm.msRequestFullscreen();
      }
      this.isHideScaner = false;
      this.toggleCamera();
      $(window).scrollTop(0);
      $("body").addClass("modal-open");
      $("body").addClass("position-fixed");
    },
    HideScanner() {
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitCancelFullScreen) {
        document.webkitCancelFullScreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
      this.isHideScaner = true;
      this.toggleCamera();
      $("body").removeClass("modal-open");
      $("body").removeClass("position-fixed");
    },
    async onInitScan(promise) {
      try {
        await promise;
      } catch (error) {
        if (error.name === "NotAllowedError") {
          this.error = "ERROR: you need to grant camera access permission";
        } else if (error.name === "NotFoundError") {
          this.error = "ERROR: no camera on this device";
        } else if (error.name === "NotSupportedError") {
          this.error = "ERROR: secure context required (HTTPS, localhost)";
        } else if (error.name === "NotReadableError") {
          this.error = "ERROR: is the camera already in use?";
        } else if (error.name === "OverconstrainedError") {
          this.error = "ERROR: installed cameras are not suitable";
        } else if (error.name === "StreamApiNotSupportedError") {
          this.error = "ERROR: Stream API is not supported in this browser";
        } else if (error.name === "InsecureContextError") {
          this.error =
            "ERROR: Camera access is only permitted in secure context. Use HTTPS or localhost rather than HTTP.";
        } else {
          this.error = `ERROR: Camera error (${error.name})`;
        }
        alert(this.error);
      }
    },
    MenuToggle() {
      this.isHideMenu = !this.isHideMenu;
    },
    onDecode(result) {
      this.resultQR = result;
      this.valueSearch = result;
      this.HideScanner();
      this.isHideMenu = true;
      this.querySearch();
    },
    changeValue() {
      console.log("set focus");
    },
    mainSetFocus() {
      console.log("focus");
    },
    selectAll() {
      this.$refs.input.select();
    },
    changeSubFrom() {
      localStorage.subFrom = this.subFrom;
    },
    changeSubTo() {
      localStorage.subTo = this.subTo;
    },
    subExportExcel() {
      const items = this.subDataTable;
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
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
      console.log(csv);
    },
    exportExcel() {
      const items = this.allDataTable;
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
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
      console.log(csv);
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
    subOderBy(key) {
      this.sortByKey(this.subDataTable, key);
    },
    OderBy(key) {
      this.sortByKey(this.qmDataTable, key);
    },
    async valueScanQR(){
      if(this.isScanQR == true){
          var payload = {
          database: localStorage.databaseName,
          field: this.chooseField,
          list_input: [{ input: this.valueSearch }],
        };
        var { data } = await Repository.getRepo("GetValueQR", payload);
        
        if (data.data != null) {
          data.data.forEach((element) => {
            this.valueSearch = element.QR;
          });
        }
      }
    },
    async querySearch() {
      if(this.isPrevMO == true &&(this.valueSearch != ""||this.valueSearch != null)){
        var payloadpre = {
          database_name: localStorage.databaseName,
          MO_NUMBER: this.valueSearch ,
        };
        var  dataprev  = await Repository.getRepo("GetRouteContent", payloadpre);


        this.DataTableRoute = [];
        this.DataTableRoute = dataprev.data.data;
        this.DataTableBom = [];
        this.DataTableBom = dataprev.data.data1;
        if (typeof this.DataTableRoute != "undefined") {
          if (this.DataTableRoute.length != 0) {
            this.DataTablePrev = Object.keys(this.DataTableRoute[0]);

            this.DataTablePrev.forEach((element) => {
              this.columnName.push({
                label: element,
                field: element,
              });
            });
          }
        }
        if (typeof this.DataTableBom != "undefined") {
          if (this.DataTableBom.length != 0) {
            this.DataTablePreBom = Object.keys(this.DataTableBom[0]);

            this.DataTablePreBom.forEach((element) => {
              this.columnName.push({
                label: element,
                field: element,
              });
            });
          }
        }
        return;
      }
      this.isSpecialRoute = false;
      this.valueSearch=this.valueSearch.replace(/[\u200B-\u200D\uFEFF]/g,"");
      if (this.valueSearch.length > 0) {
        if (this.isSubStr == true) {
          this.valueSearch = this.valueSearch.substr(this.subFrom, this.subTo);
        } else {
          if (
            localStorage.databaseName == "ROKU" &&
            this.valueSearch.indexOf(":") > 0
          ) {
            this.valueSearch = this.valueSearch.substring(
              0,
              this.valueSearch.indexOf(":")
            );
          }

          if (
            this.valueSearch.indexOf(",") !== -1 &&
            this.chooseField == "SERIAL_NUMBER"
          ) {
            this.valueSearch = this.valueSearch.split(",")[0];
          }

          if (
            this.valueSearch.length > 20 &&
            this.valueSearch.indexOf(":") !== -1
          ) {
            this.valueSearch = this.valueSearch
              .substring(this.valueSearch.length - 17)
              .replace(/:/g, "");
          }
          
        }

        if (this.isAddSymbol == true && this.chooseField == "SERIAL_NUMBER") {
          if (this.valueSearch.indexOf("@") != 0) {
            this.valueSearch = "@" + this.valueSearch;
          }
        }

        var listGroup = [];
        this.$store.state.listSelectDualGroup.forEach((element) => {
          listGroup.push({ input: element.VALUE });
        });
        var date_from = null;
        var date_to = null;
        if (this.period) {
          if (this.timeFrom == "ALL") {
            date_from = `${this.pad(this.dateFrom.getFullYear())}/${this.pad(
              this.dateFrom.getMonth() + 1
            )}/${this.pad(this.dateFrom.getDate())} 00:00`;
            date_to = `${this.pad(this.dateTo.getFullYear())}/${this.pad(
              this.dateTo.getMonth() + 1
            )}/${this.pad(this.dateTo.getDate())} 23:59`;
          } else {
            date_from = `${this.pad(this.dateFrom.getFullYear())}/${this.pad(
              this.dateFrom.getMonth() + 1
            )}/${this.pad(this.dateFrom.getDate())} ${this.timeFrom}`;
            if (this.timeTo == "") {
              date_to = `${this.pad(this.dateTo.getFullYear())}/${this.pad(
                this.dateTo.getMonth() + 1
              )}/${this.pad(this.dateTo.getDate())} 23:59`;
            } else {
              date_to = `${this.pad(this.dateTo.getFullYear())}/${this.pad(
                this.dateTo.getMonth() + 1
              )}/${this.pad(this.dateTo.getDate())} ${this.timeTo}`;
            }
          }
        }

        var payload = {
          database: localStorage.databaseName,
          table: this.chooseOption,
          field: this.chooseField,
          list_input: [{ input: this.valueSearch }],
          list_group_name: listGroup,
          date_from: date_from,
          date_to: date_to,
        };
        var { data } = await Repository.getRepo("Query", payload);

        this.selectAll();
        var $elem = $(".div-table");
        this.allDataTable = data.data;
        this.subDataTable = data.data1;
        if (this.allDataTable.length > 0) {
          this.tempValueDetail = this.allDataTable[0].SERIAL_NUMBER;
        } else {
          this.tempValueDetail = "";
        }
        this.qmDataTable = this.allDataTable.slice(0, 500);

        if (this.qmDataTable.length != 0) {
          this.qmDataTableHeader = Object.keys(this.qmDataTable[0]);
          this.qmDataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
        if (this.subDataTable && this.subDataTable.length > 0) {
          this.subDataTableHeader = Object.keys(this.subDataTable[0]);
          this.subDataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
        $elem.scrollLeft = 10;
      }
    },
    EnterElement(type) {
      this.$store.state.elementQ6 = type;
      if (type == "MO") {
        this.$store.state.Q6ElementNameEn = "Enter MO number";
        this.$store.state.Q6ElementNameVi = "Nhập công lệnh";
        this.$store.state.isShowQ6Element = true;
      }
      if (type == "Detail") {
        this.showDetailHistory();
      } else if (type == "Serial") {
        this.$store.state.Q6ElementNameEn = "Enter serial number";
        this.$store.state.Q6ElementNameVi = "Nhập số serial";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "BOX_SN") {
        this.$store.state.Q6ElementNameEn = "Enter Box SN (shipping_sn)";
        this.$store.state.Q6ElementNameVi = "Nhập Box SN (shipping_sn)";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Carton") {
        this.$store.state.Q6ElementNameEn = "Enter carton no";
        this.$store.state.Q6ElementNameVi = "Nhập mã carton";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "IMEI") {
        this.$store.state.Q6ElementNameEn = "Enter pallet no, imei";
        this.$store.state.Q6ElementNameVi = "Nhập mã pallet, imei";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Tray") {
        this.$store.state.Q6ElementNameEn = "Enter tray ID";
        this.$store.state.Q6ElementNameVi = "Nhập mã tray";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "License") {
        this.$store.state.Q6ElementNameEn = "Enter License ID";
        this.$store.state.Q6ElementNameVi = "Nhập mã License";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Keypart") {
        this.$store.state.Q6ElementNameEn = "Enter keypart";
        this.$store.state.Q6ElementNameVi = "Nhập keypart";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Route") {
        this.CheckRouteExist();
      } else if (type == "Cpanel") {
        this.$store.state.Q6ElementNameEn = "Enter panel no";
        this.$store.state.Q6ElementNameVi = "Nhập mã panel";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Repair") {
        this.$store.state.Q6ElementNameEn = "Enter repair serial";
        this.$store.state.Q6ElementNameVi = "Nhập bản repair";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Type") {
        this.$store.state.Q6ElementNameEn = "Enter serial number";
        this.$store.state.Q6ElementNameVi = "Nhập serial number";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Query_list") {
        this.$store.state.isShowQ6QueryList = true;
      } else if (type == "System Log") {
        this.$store.state.isShowSystemLog = true;
      } else if (type == "HistoryByDN" || type == "RepairByDN") {
        this.$store.state.Q6ElementNameEn = "Enter DN";
        this.$store.state.Q6ElementNameVi = "Nhập DN";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "COC") {
        this.$store.state.Q6ElementNameEn = "Enter DN, split by , ";
        this.$store.state.Q6ElementNameVi = "Nhập DN, cách nhau bởi dấu ,";
        this.$store.state.isShowQ6Element = true;
      } else if (type == "FailAte") {
        this.$store.state.Q6ElementNameEn =
          "Enter station_name or serial_number ";
        this.$store.state.Q6ElementNameVi = "Nhập tên trạm hoặc serial_number";
        this.$store.state.isShowQ6Element = true;
      } else if (
        type == "GetMoTlinklevel" ||
        type == "NOTLASER" ||
        type == "CHECK MO"
      ) {
        this.$store.state.Q6ElementNameEn = `Enter MO Number`;
        this.$store.state.Q6ElementNameVi = `Nhập Công lệnh`;
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Repallet") {
        this.$store.state.Q6ElementNameEn = `Enter IMEI, Pallet No`;
        this.$store.state.Q6ElementNameVi = `Nhập IMEI, Pallet No`;
        this.$store.state.isShowQ6Element = true;
      } else if (type == "Landis_DATA" || type == "HistoryKPSN") {
        this.$store.state.Q6ElementNameEn = `Enter Serial number`;
        this.$store.state.Q6ElementNameVi = `Nhập Serial number`;
        this.$store.state.isShowQ6Element = true;
      } else {
        this.$store.state.Q6ElementNameEn = `Enter ${type}`;
        this.$store.state.Q6ElementNameVi = `Nhập ${type}`;
        this.$store.state.isShowQ6Element = true;
      }
      this.stateFocus = !this.stateFocus;
    },
    async CheckRouteExist() {
      var payload = {
        database_name: localStorage.databaseName,
        route_code: this.tempValueDetail,
        value: this.tempValueDetail,
      };
      alert("dai");
      var { data } = await Repository.getRepo("CheckRouteExist", payload);
      if (data.result == "ok") {
        this.$store.state.routeCode = data.data[0].ROUTE_CODE;
        this.$store.state.routeName = data.data[0].ROUTE_NAME;
        this.$store.state.isShowQ6Route = true;
      } else {
        //window.open("http://10.224.69.100/sfc/Route");
      }
    },
    async GetLinkData(value) {
      this.subDataTableHeader = [];
      var payload = {
        database: localStorage.databaseName,
        table: "R108",
        field: "SERIAL_NUMBER",
        list_input: [{ input: value }],
        date_from: null,
        date_to: null,
      };
      var { data } = await Repository.getRepo("Query", payload);
      if (data.result == "ok") {
        this.$store.commit("setListAll", data.data);
        var arr = data.data;
        this.subDataTable = arr.slice(0, 500);
        if (this.subDataTable && this.subDataTable.length > 0) {
          Object.keys(this.subDataTable[0]).forEach((element) => {
            this.subDataTableHeader.push(element);
          });
        } else {
          this.subDataTable = [];
          this.subDataTableHeader = [];
        }
      }
    },
    async showDetailHistory() {
      if (this.tempValueDetail.length != 0) {
        this.subDataTableHeader = [];
        var payload = {
          database: localStorage.databaseName,
          table: "R117_",
          field: "SERIAL_NUMBER",
          list_input: [{ input: this.tempValueDetail }],
          date_from: null,
          date_to: null,
        };
        var { data } = await Repository.getRepo("Query", payload);
        if (data.result == "ok") {
          this.$store.commit("setListAll", data.data);
          var arr = data.data;
          this.subDataTable = arr.slice(0, 500);
          if (this.subDataTable && this.subDataTable.length > 0) {
            Object.keys(this.subDataTable[0]).forEach((element) => {
              this.subDataTableHeader.push(element);
            });
          } else {
            this.subDataTable = [];
            this.subDataTableHeader = [];
          }
        }
      }
    },
    async GetInfoDetail(value, column, valueCell) {
      this.tempValueDetail = value;
      this.isSpecialRoute = false;
      
      if (column == "SPECIAL_ROUTE") {
        //ROUTE
        this.isSpecialRoute = true;
        this.subDataTable = [];
        this.subDataTableHeader = [];
        this.$store.state.routeCode = valueCell;
        //this.ShowDetailRoute(valueCell);
        this.CheckRouteExist();
      } else if (
        column == "SERIAL_NUMBER" ||
        column == "MO_NUMBER" ||
        column == "MODEL_NAME" ||
        column == "BOM_NO"
      ) {
        this.subDataTableHeader = [];
        var payload = {
          database: localStorage.databaseName,
          field: column,
          value: valueCell,
        };
      
        var { data } = await Repository.getRepo("GetByClick", payload);
        
      
      
        
        
        if (data.result == "ok") {
          this.$store.commit("setListAll", data.data);
          var arr = data.data;
          this.subDataTable = arr.slice(0, 500);
          if (this.subDataTable && this.subDataTable.length > 0) {
            Object.keys(this.subDataTable[0]).forEach((element) => {
              this.subDataTableHeader.push(element);
            });
          } else {
            this.subDataTable = [];
            this.subDataTableHeader = [];
          }
        }
      }
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
    async GetQMElement(typeSearch, listType, modalType) {
      var dateFrom = `${this.pad(this.dateFrom.getFullYear())}${this.pad(
        this.dateFrom.getMonth() + 1
      )}${this.pad(this.dateFrom.getDate())}`;
      var dateTo = `${this.pad(this.dateTo.getFullYear())}${this.pad(
        this.dateTo.getMonth() + 1
      )}${this.pad(this.dateTo.getDate())}`;

      var timeFrom = this.timeFrom;
      var timeTo = this.timeTo;
      if (this.period == false) {
        timeFrom = "NO";
      }

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
    clickOption(value) {
      this.setAllToUnActive();
      if (value == "opt1") {
        this.opt1 = true;
        this.doQuery("QMYieldRate");
      } else if (value == "opt2") {
        this.opt2 = true;
        this.doQuery("DefectAnalysis");
      } else if (value == "opt3") {
        this.opt3 = true;
        this.doQuery("RepairAnalysis");
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
    async ShowDetailRoute(route_code) {
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: 0,
      };
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          if (data != false && data != null) {
            console.log(`loading...`);
          }
        });
      // $("#content").css("width", "1300px");
      await this.getRoute(this.$store.state.routeCode);
    },
    async getRoute(route_code) {
      var mine = this;
      console.log(route_code);
      $("#content").html("");
      $("#tb_data").remove();
      await $("body,html").css("cursor", "progress");
      var mainArray = [];
      var ReturnArray = [];
      var specialArray = [];
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: 0,
      };
      console.log(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix)
      );

      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          $("#content").html("");
          $("#tb_data").remove();
          if (data != false && data != null) {
            var return_data = await data.data;
            if (return_data.length > 0) {
              await mine.addDataTable();
              var groupName = await return_data[0].GROUP_NEXT;
              //$('#content').html(groupName+' - ');
              await mine.addTdData("line_main", groupName);
              await mainArray.push(groupName);
              var RS = await mine.getSpecialGroup(route_code, groupName);
              if (RS != false) {
                await specialArray.push(RS);
              }
              console.log(specialArray);
              var eor = false;
              var result = false;
              var RG = false;
              // var i = 0;
              await console.log(groupName + " ---luat");
              try {
                while (eor == false) {
                  result = await mine.getGroupNext(route_code, groupName);
                  RG = await mine.getRGroup(route_code, groupName);
                  if (result == false) {
                    eor = await true;
                  } else {
                    groupName = await result[0].GROUP_NEXT;
                    await mainArray.push(groupName);
                  }
                  if (RG != false) {
                    for (var iCount = 0; iCount < RG.length; iCount++) {
                      await ReturnArray.push(RG[iCount]);
                    }
                  }
                  RS = await mine.getSpecialGroup(route_code, groupName);
                  if (RS != false) {
                    await specialArray.push(RS);
                  }
                }
              } catch {
                result = false;
              }
              //Draw
              var vtop = $("#tb_data").offset().top;
              var vleft = $("#tb_data").offset().left;
              $("#myCanvas").attr("height", $("#tb_data").css("height"));
              $("#myCanvas").attr("width", $("#tb_data").css("width"));
              $("#myCanvas").offset({ top: vtop, left: vleft });
              var c = document.getElementById("myCanvas");
              var ctx = c.getContext("2d");
              ctx.font = "40pt Arial";
              ctx.lineWidth = 5;

              ctx.strokeStyle = "#0000ff";
              //------------------------------------------------------------------------------------------------------------------------
              for (let i = 0; i < mainArray.length - 1; i++) {
                await mine.drawLineGroupToGroup(
                  "main",
                  0,
                  ctx,
                  vtop,
                  vleft,
                  mainArray[i],
                  mainArray[i + 1]
                );
              }
              ctx.lineWidth = 2.5;

              ctx.strokeStyle = "#0F0";
              var itemHeight =
                $("#line_main")[0].offsetHeight +
                $("#line_over")[0].offsetHeight;
              //===============================================================================================================================
              for (let i = 0; i < ReturnArray.length; i++) {
                await mine.drawLineGroupToGroup(
                  "re",
                  itemHeight,
                  ctx,
                  vtop,
                  vleft,
                  ReturnArray[i].R,
                  ReturnArray[i].G
                );
                // $("#" + ReturnArray[i].R).attr(
                //   "title",
                //   "-Return to: " + ReturnArray[i].G + " \n"
                // );
              }
              //r_gROUP
              ctx.strokeStyle = "#18c900";
              ctx.strokeStyle = "#F00";
              for (let i = 0; i < ReturnArray.length; i++) {
                await mine.drawLineGroupToGroup(
                  "R",
                  itemHeight,
                  ctx,
                  vtop,
                  vleft,
                  ReturnArray[i].R,
                  ReturnArray[i].F
                );
              }
              //Special line
              ctx.strokeStyle = "#000080";
              for (let i = 0; i < specialArray.length; i++) {
                var sg = specialArray[i];
                for (var j = 0; j < sg.length; j++) {
                  await mine.drawLineGroupToGroup(
                    "S",
                    sg[j].LEVEL,
                    ctx,
                    vtop,
                    vleft,
                    sg[j].G,
                    sg[j].GN
                  );
                  $("#" + sg[j].G).attr(
                    "title",
                    $("#" + sg[j].G).attr("title") +
                      "-Special to: " +
                      sg[j].GN +
                      " \n"
                  );
                }
              }
              //  await  $('#tb_data').css('position', 'absolute');
              await $("#tb_data").css("height", $("#myCanvas").attr("height"));
              await $("#line_over").css("width", "100px");
              await $("#content").css(
                "height",
                parseInt($("#myCanvas").attr("height")) + 50 + "px"
              );
              //End Draw
            } else $("#content").html("No data!");
          } else {
            alert("WebAPI is not response");
            return false;
          }
        });
      await $("body,html").css("cursor", "auto");
    },
    async getGroupNext(route_code, groupName) {
      var mine = this;
      var result = false;
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: groupName,
      };
      //console.log(BASE1 + JSON.stringify(dynamix));
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(async function (response) {
          return await response.json();
        })
        .then(async function (data) {
          if (data != false && data != null) {
            var return_data = await data.data;
            if (return_data.length > 0) {
              await $.each(return_data, function (key, value) {
                console.log(value);
                //    //var gn = return_data[key].GROUP_NEXT;
                //    //$('#content').html($('#content').html()+ gn+' - ');
                mine.addTdData("line_main", return_data[key].GROUP_NEXT);
                //console.log(return_data[key].GROUP_NEXT);
              });
              result = await return_data;
              //await console.log(data);
            }
          } else {
            alert("WebAPI is not response");
          }
        });
      // await console.log(result[0].GROUP_NEXT);
      return await result;
      //await console.log("turn " + result + JSON.stringify(data));
    },
    async drawLineGroupToGroup(
      type,
      topFrom,
      ctx,
      vtop,
      vleft,
      FromRGroupId,
      ToGroupID
    ) {
      if (ToGroupID != "" && FromRGroupId != "") {
        var G = $("#" + ToGroupID);
        var R = $("#" + FromRGroupId);
        var G_X, G_Y, R_X, R_Y;
        if (
          typeof G.offset() === "undefined" ||
          typeof R.offset() === "undefined"
        ) {
          console.log(`....`);
        } else {
          await ctx.beginPath();
          if (type == "S") {
            G_X = parseFloat(G.offset().left - vleft);
            G_Y = parseFloat(G.offset().top - vtop);
            R_X = parseFloat(R.offset().left - vleft);
            R_Y = G_Y;
            var centerG = G_X + parseFloat(G[0].offsetWidth / 2);
            var centerR = R_X + parseFloat(R[0].offsetWidth / 2);
            var level = 0,
              pos = 10;
            if (topFrom == 1) {
              pos = 6;
              level = 45;
              // level=90;
            } else if (topFrom == 2) {
              pos = 8;
              level = 20;
              // level=30;
            } else if (topFrom == 3) {
              pos = 9;
            }
            var c1 = parseFloat((centerR - centerG) / pos);
            if (c1 < 0) c1 = -c1;
            var c2 = centerG - c1;
            c1 = centerR + c1;
            await ctx.moveTo(centerR, R_Y);
            await ctx.bezierCurveTo(c1, level, c2, level, centerG, G_Y);
          } else {
            if (type == "main") {
              G_X = parseInt(G.offset().left - vleft);
              G_Y = parseInt(G.offset().top - vtop + G[0].offsetHeight / 2);
              R_X = parseInt(R.offset().left - vleft + R[0].offsetWidth);
              R_Y = G_Y;
            } else {
              G_X = parseInt(G.offset().left - vleft + G[0].offsetWidth / 2);
              G_Y = parseInt(topFrom); //parseInt(G.offset().top-vtop-G[0].offsetWidth);
              R_X = parseInt(R.offset().left - vleft + R[0].offsetWidth / 2);
              R_Y = parseInt(R.offset().top - vtop);
              if (type == "R") {
                G_X = G_X - 2;
                R_X = R_X - 2;
              }
            }
            await ctx.moveTo(G_X + 1, G_Y);
            await ctx.lineTo(R_X - 1, R_Y);
          }
          await ctx.stroke();
        }
      }
    },
    async getSpecialGroup(route_code, groupName) {
      var result = false;
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-SPECIAL-GROUP",
        route: route_code,
        group_name: groupName,
      };
      // console.log(BASE1 + JSON.stringify(dynamix));
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          if (data != false && data != null) {
            var return_data = data.data;
            if (return_data.length > 0) {
              result = [];
              var level = 1;
              $.each(return_data, function (key, value) {
                result.push({
                  G: value.GROUP_NAME,
                  GN: value.GROUP_NEXT,
                  LEVEL: level,
                });
                level++;
              });
            }
          } else {
            alert("WebAPI is not response");
          }
        });
      return await result;
    },
    async getRGroup(route_code, groupName) {
      var mine = this;
      var result = false;

      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-R-GROUP",
        route: route_code,
        group_name: groupName,
      };
      // console.log(BASE1 + JSON.stringify(dynamix));
      var list_Return_group = [];
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          var rGroup = "";
          // returnGroup = "",
          // same = false;
          if (data != false && data != null) {
            var return_data = data.data;
            if (return_data.length > 0) {
              rGroup = await return_data[0].GROUP_NEXT;
              list_Return_group = await mine.getReturnRGroup(
                route_code,
                rGroup
              );
            }
          } else {
            alert("WebAPI is not response");
          }
          if (rGroup != "") {
            for (let iCount = 0; iCount < list_Return_group.length; iCount++) {
              //same=false;
              if (list_Return_group[iCount] == groupName) {
                await mine.addTdData("line_relation", "", "relation_return");
              } else {
                await mine.addTdData("line_relation", "", "relation");
              }
            }
          } else {
            await mine.addTdData("line_relation", "");
          }
          await mine.addTdData("line_r", rGroup);
          if (rGroup != "") {
            if (list_Return_group.length > 0) {
              result = [];
              for (
                let iCount = 0;
                iCount < list_Return_group.length;
                iCount++
              ) {
                result[iCount] = await {
                  R: rGroup,
                  G: list_Return_group[iCount],
                  F: groupName,
                };
              }
            }
          }
        });
      return await result;
    },
    async getReturnRGroup(route_code, groupName) {
      var rGroup = "";
      var list_R_group = [];
      var dynamix = {
        db: localStorage.databaseName,
        function_name: "GET-NEXT-GROUP",
        route: route_code,
        group_name: groupName,
      };
      console.log(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix)
      );
      await fetch(
        this.$store.state.apiAddress +
          "Get/GetRoute?dynamic=" +
          JSON.stringify(dynamix),
        {
          method: "get",
        }
      )
        .then(function (response) {
          return response.json();
        })
        .then(async function (data) {
          if (data != false && data != null) {
            var return_data = data.data;
            // var iCount = 0;
            if (return_data.length > 0) {
              for (let iCount = 0; iCount < return_data.length; iCount++) {
                rGroup = await return_data[iCount].GROUP_NEXT;
                list_R_group[iCount] = await rGroup;
              }
            }
          } else {
            alert("WebAPI is not response");
          }
        });
      //return rGroup;
      return await list_R_group;
    },
    addDataTable() {
      var table = document.createElement("table");
      table.id = "tb_data";
      var line_over = document.createElement("tr");
      line_over.id = "line_over";
      var line_main = document.createElement("tr");
      line_main.id = "line_main";
      var line_relation = document.createElement("tr");
      line_relation.id = "line_relation";
      var line_r = document.createElement("tr");
      line_r.id = "line_r";
      table.appendChild(line_over);
      table.appendChild(line_main);
      table.appendChild(line_relation);
      table.appendChild(line_r);
      document.getElementById("content").appendChild(table);
      var canvas = document.createElement("canvas");
      canvas.id = "myCanvas";
      canvas.innerHTML = "Your browser does not support the HTML5 canvas tag.";
      document.getElementById("content").appendChild(canvas);
    },
    addTdData(line_name, tdContent, relation) {
      var f_relation = relation == undefined ? false : true;
      var td = document.createElement("td");
      // <img src="@Url.Content("~/Pc2.ico")" height="25px" width="25px"/>
      //~/Content/img/Pc2.ico
      if (tdContent != "")
        td.innerHTML =
          '<div class="border" id="' +
          tdContent +
          '" title="" onclick="showGroupInfo(this);"><div class="contentRoute"><label>' +
          tdContent +
          "</label></div></div>";
      else td.innerHTML = "<span>" + tdContent + "</span>";
      if (f_relation == true) {
        td.className = relation;
      } else {
        if (tdContent != undefined && tdContent != "") {
          td.className = "have_data";
        }
      }
      // $('#tdContent').addClass("border");
      document.getElementById(line_name).appendChild(td);
      var td_space = document.createElement("td");
      td_space.innerHTML = "";
      td_space.className = "space";
      document.getElementById(line_name).appendChild(td_space);
    },
  },
  mounted() {
    document.title = "Query6";
    this.timeToChange();
    this.timeFromChange();
    this.$store.commit("RefreshState");
  },
};
</script>

<style lang="scss" scoped>
.button-take-camera-border {
  position: absolute;
  z-index: 9998 !important;
  left: 50%;
  bottom: -20px;
  height: 60px;
  width: 60px;
  border-radius: 100%;
  background: #ffff;
  left: 50%;
  transform: translate(-50%, -50%);
  // display: flex;
  // justify-content: center;
  // align-items: center;
}
.button-take-camera {
  height: 60px;
  width: 60px;
  border-radius: 100%;
  border: 3px solid #000f15;
  background: #fff;
}

.camera-box {
  // height: 200px;
  // width: 350px;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  //position: absolute;
}
.div-scan-all {
  z-index: 9999 !important;
  height: 100vh;
  width: 100vw;
  position: fixed;

  left: 0;
  top: 0px;
  background: #000f15;
  .div-close-scan {
    position: absolute;
    right: 2px;
    top: 5px;
  }
  overflow: hidden;
}
.div-content-bellow {
  min-height: 76vh;
  //max-height: 76vh;
}
.camera-box {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}
.div-top {
  min-height: 9vh;
}
.arrow-hide {
  //display: none;
  position: absolute;
  right: 5px;
  bottom: 5px;
  border-radius: 100%;
  cursor: pointer;
}

.btn-img {
  visibility: hidden;
}
@media only screen and (hover: none) and (pointer: coarse) {
  .btn-img {
    visibility: visible;
  }
  .btn-search {
    display: none;
  }
  .div-searchbox {
    height: 40px !important;
  }
  .item-top {
    height: 40px !important;
    span {
      font-size: 10px !important;
    }
  }
  .table-condensed {
    font-size: 10px !important;
  }
  .img-excel {
    height: 30px !important;
    width: 35px !important;
  }
  .text-count {
    font-size: 10px !important;
  }
}

.btn-img {
  background: #f1f1f1;
}
// .button-scanimg {
//   display: none;
// }
.button-chooseimg {
  display: none;
}
.button-29 {
  align-items: center;
  appearance: none;
  background-image: radial-gradient(
    100% 100% at 100% 0,
    #5adaff 0,
    #5468ff 100%
  );
  border: 0;
  border-radius: 6px;
  box-shadow: rgba(45, 35, 66, 0.4) 0 2px 4px,
    rgba(45, 35, 66, 0.3) 0 7px 13px -3px, rgba(58, 65, 111, 0.5) 0 -3px 0 inset;
  box-sizing: border-box;
  color: #fff;
  cursor: pointer;
  display: inline-flex;
  font-family: "JetBrains Mono", monospace;
  height: 48px;
  justify-content: center;
  line-height: 1;
  list-style: none;
  overflow: hidden;
  padding-left: 16px;
  padding-right: 16px;
  position: relative;
  text-align: left;
  text-decoration: none;
  transition: box-shadow 0.15s, transform 0.15s;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  white-space: nowrap;
  will-change: box-shadow, transform;
  font-size: 18px;
}

.button-29:focus {
  box-shadow: #3c4fe0 0 0 0 1.5px inset, rgba(45, 35, 66, 0.4) 0 2px 4px,
    rgba(45, 35, 66, 0.3) 0 7px 13px -3px, #3c4fe0 0 -3px 0 inset;
}

.button-29:hover {
  box-shadow: rgba(45, 35, 66, 0.4) 0 4px 8px,
    rgba(45, 35, 66, 0.3) 0 7px 13px -3px, #3c4fe0 0 -3px 0 inset;
  transform: translateY(-2px);
}

.button-29:active {
  box-shadow: #3c4fe0 0 3px 7px inset;
  transform: translateY(2px);
}
.select-time {
  option {
    background: #024873;
    color: #fff;
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
.text-dropdown {
  color: #024873;
  font-size: 12px;
  font-weight: 555;
  &:hover {
    background-color: #e8dede;
  }
}
ul li {
  list-style: none;
}
.text_total {
  font-size: 13px;
}
.img-excel {
  z-index: 100;
  height: 40px;
  width: 40px;
  margin-right: 30px;
  cursor: pointer;
}
.div-export {
  margin-left: 20px;
  display: flex;
  justify-content: space-between;
}
.select-form {
  font-size: 12px;
  option {
    background: #024873;
    color: #fff;
  }
}
.div-searchbox {
  height: 60px;
  display: flex;
  align-content: center;
  justify-content: center;
  .div-searchbox-content {
    position: absolute;
    display: flex;
    bottom: 0;
    // margin-bottom: 10px;
    // left: 50%;
    text-align: center;
    input {
      border-radius: 10px 0 0 10px;
      padding: 0px 5px;
      width: 400px;
    }
    .btn-search {
      border-radius: 0 10px 10px 0;
      //padding: 0 20px;
      background: #ff7a1c;
      color: #fff;
      box-sizing: 0;
      &:hover {
        background: #f88838;
      }
    }
    button {
      border-radius: 0 10px 10px 0;
      //padding: 0 20px;
      background: #ff7a1c;
      color: #fff;
      box-sizing: 0;
      &:hover {
        background: #f88838;
      }
    }
  }
}
.div-icon-item {
  text-align: center;
  margin-top: 10px;
}
.item-top {
  padding-top: 5px;
  align-content: center;
  justify-items: center;
  justify-content: center;
  text-align: center;
  cursor: pointer;
  user-select: none;
  height: 50px;
  // padding: 2px 20px;
  border-radius: 10px;
  margin: 2px;
  background-color: #fffbf7;
  box-shadow: 1px 1px 1px #666666;

  display: flex;
  flex-direction: column;
  color: #0e2755;
  span {
    color: #0e2755;
    font-size: 13px;
    font-weight: 555;
  }
  &:hover {
    background-color: #d7ebff;
    color: #0e2755;
    span {
      color: #0e2755;
      font-size: 13px;
      font-weight: 555;
    }
  }
}
.div-top-option {
  padding-top: 5px;
  margin-bottom: 10px;
  display: flex;
}
.div-period {
  display: flex;
}
.titleType {
  font-size: 12px;
  color: #fff;
}
.tr-red {
  background-color: red;
  td {
    color: #fff;
  }
}
.div-table {
  max-height: 250px;
  margin-left: 5px;
  overflow: auto;
  table tr th {
    position: sticky;
    top: 0;
    z-index: 2;
    overflow-x: auto;
    white-space: nowrap;
  }
  table tr {
    &:hover {
      background-color: #e08860;
      cursor: pointer;
    }
  }
  table tr td {
    white-space: nowrap;
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
  font-size: 13px;
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
  user-select: none;
  background-color: #a0d0ff;
  box-shadow: 1px 2px 2px #fffbf7;
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
// .div-main {
//   //padding-left: 20px;
// }
.div-top {
  padding-top: 5px;
  padding-bottom: 5px;
  display: flex;
  justify-content: space-between;
}

//-----------------------------------------
.contentRoute {
  border: 1px solid #0959a2;
  border-radius: 10%;
  font-size: 13px;
  padding: 0 10px;
  font-weight: 555;
  background-color: #d0e9fd;
  margin: auto;
  text-align: center;
  padding-top: 5px;
}
#line_relation {
  // height: 500px !important;
  min-height: 100px !important;
  td {
    height: 250px !important;
    // min-height: 500px !important;
  }
}
#tb_data tr {
  text-align: left;
  /* line-height: 4em; */
}

#tb_data tr td {
  /* padding: 0 10px; */
  text-align: center;
  /* padding-right:1em;; 
	border:1px solid #ccc; */
  white-space: nowrap;
}

#tb_data tr .space {
  min-width: 1em;
}

#tb_data tr .have_data {
  /* background-color:#ccc;	 */
  /* border:1px dotter #8a8a8a; */
  padding: 5px 0;
}

#tb_data tr .have_data span {
  cursor: pointer;
}

#tb_data tr .relation,
#tb_data tr .relation_return {
  background-repeat: no-repeat;
  // background-repeat-y: initial;
  background-position: center;
  height: 100%;
  width: 100%;
  line-height: 100%;
}
#tb_data {
  //height: 400px;
  /* max-height:100px; */
  width: 1%;
  z-index: 100;
}
#line_main {
  position: relative;
  /* background-co
    lor:#ccc; */
  /* background-image: url(bg_line_main.png); */
  background-repeat: no-repeat;
  // background-repeat-x: initial;
  background-position: center;
}
#line_over {
  height: 80px !important;
}
#tb_data {
  z-index: 1000 !important;
  height: 350px !important;
}
#content {
  height: 70vh !important;
  width: 90vw;
  overflow-y: hidden;
}
.space {
  width: 60px !important;
}
</style>
<style lang="scss" scoped>
.text-search {
  text-decoration: underline;
  font-size: 14px;
  color: #fa255d;
  font-weight: 555;
  cursor: pointer;
  user-select: none;
  &:hover {
    color: #ba375a;
  }
}
.text-route {
  color: #418bca;
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

body {
  font-family: "montserrat", sans-serif;
}

#app {
  position: absolute;
  display: flex;
  justify-content: center;
  align-items: center;
  //height: 100vh;
  overflow: auto;
  // width: 100vw;
  // min-height: 100vh;
  // overflow-x: hidden;
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
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 98;
  background-color: rgba(0, 0, 0, 0.3);
}

.my_modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 99;
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

.main-contain {
  height: 350px;
  overflow: auto;
}
.div-back {
  float: left;
  background: #eae1e1;
  cursor: pointer;
  margin: 10px 0;
  display: flex;
  align-content: center;
  align-items: center;
  width: 3%;
  border-radius: 10%;
  &:hover {
    background: #b7b7b7;
  }
  .back-icon {
    height: 20px;
    width: 20px;
  }
}
.mytable {
  margin-top: 0px;
  overflow: auto;
  thead {
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
    }
  }
  tr {
    &:hover {
      background: #89cfed;
    }
    td {
      overflow-x: auto;
      white-space: nowrap;
      z-index: 1;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 13px;
      font-weight: 555;
    }
  }
}
</style>
<style src="@vueform/multiselect/themes/default.css"></style>

