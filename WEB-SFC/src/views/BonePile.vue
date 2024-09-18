<template>
  <div>
    <div class="div-main" style="background-color: #4e7da6">
      <div class="row col-12">
        <div class="top-left col-12 row">
          <div class="col-3 row">
            <span class="col-2 text-element">From</span>
            <datepicker
              class="col-10 form-control form-control-sm"
              v-model="dateFrom"
              :upperLimit="dateTo"
            />
          </div>
          <div class="col-3 row">
            <span class="col-2 text-element">To</span>
            <datepicker
              class="col-10 form-control form-control-sm"
              v-model="dateTo"
              :lowerLimit="dateFrom"
            />
          </div>
          <div class="col-3 row">
            <span class="col-4 text-element">Model</span>
            <input
              @click="SearchModel()"
              type="text"
              class="col-8 form-control form-control-sm element-input"
              :value="
                $store.state.listSelectDualModel.length == 0
                  ? 'ALL'
                  : `${$store.state.listSelectDualModel[0].VALUE} ...`
              "
              readonly
            />
          </div>
          <div class="col-3 row div-button">
            <button class="button-17" role="button" @click="BonePileSearch()">
              <Icon icon="search" class="sidenav-icon" />
            </button>
          </div>
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
            {{ $store.state.language == "En" ? "records" : "bản ghi" }}</b
          ></span
        >
      </div>
      <div class="row">
        <template v-if="qmDataTable.length > 0">
          <div>
            <img
              class="img-excel"
              @click="ExportExcel()"
              src="../assets/img/excel_64.png"
              alt=""
            />
          </div>
        </template>
      </div>
    </div>
    <div class="row col-12 div-content">
      <div class="div-table">
        <table id="tableMain" class="mytable table table">
          <thead>
            <template v-for="(item, index) in qmDataTableHeader" :key="index">
              <th>
                {{ item }}
              </th>
            </template>
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
  </div>
</template>

<script>
import Repository from "../services/Repository";

export default {
  name: "BONEPILE",
  data() {
    return {
      dateFrom: new Date(),
      dateTo: new Date(),
      qmDataTableHeader: [],
      qmDataTable: [],
      qmDataTableAll: [],
    };
  },

  methods: {
    ExportExcel() {
      const items = this.qmDataTable;
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
    pad(num) {
      num = num.toString();
      while (num.length < 2) num = "0" + num;
      return num;
    },
    async BonePileSearch() {
      var dateFrom = `${this.pad(this.dateFrom.getFullYear())}-${this.pad(
        this.dateFrom.getMonth() + 1
      )}-${this.pad(this.dateFrom.getDate())}`;
      var dateTo = `${this.pad(this.dateTo.getFullYear())}-${this.pad(
        this.dateTo.getMonth() + 1
      )}-${this.pad(this.dateTo.getDate())}`;

      var payload = {
        database_name: localStorage.databaseName,
        dateFrom: dateFrom,
        dateTo: dateTo,
        listModel: this.$store.state.listSelectDualModel,
      };
      var { data } = await Repository.getRepo("GetBonePileData", payload);
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
      }
    },
    async SearchModel() {
      var dateFrom = `${this.pad(this.dateFrom.getFullYear())}-${this.pad(
        this.dateFrom.getMonth() + 1
      )}-${this.pad(this.dateFrom.getDate())}`;
      var dateTo = `${this.pad(this.dateTo.getFullYear())}-${this.pad(
        this.dateTo.getMonth() + 1
      )}-${this.pad(this.dateTo.getDate())}`;

      var payload = {
        database_name: localStorage.databaseName,
        dateFrom: dateFrom,
        dateTo: dateTo,
        listModel: this.$store.state.listSelectDualModel,
      };
      var { data } = await Repository.getRepo("GetBonePileElement", payload);
      this.$store.commit("UpdateListSelectModel", data.data);
      this.$store.commit("showModalModel");
    },
  },
  mounted() {
    document.title = "BonePile Search";
    this.$store.commit("RefreshState");
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
.div-export {
  margin-top: 10px;
  margin-left: 20px;
  display: flex;
  justify-content: space-between;
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
.div-table {
  min-height: 76vh;
  max-height: 76vh;
  margin-left: 10px;
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
.div-main {
  padding: 40px;
}
.text-element {
  color: antiquewhite;
  font-size: 14px;
}
.div-button {
  margin: auto;
}
.button-17 {
  padding: 0px 60px;
  align-items: center;
  appearance: none;
  background-color: #e85f04;
  border-radius: 24px;
  border-style: none;
  box-shadow: rgba(0, 0, 0, 0.2) 0 3px 5px -1px,
    rgba(0, 0, 0, 0.14) 0 6px 10px 0, rgba(0, 0, 0, 0.12) 0 1px 18px 0;
  box-sizing: border-box;
  color: #fff;
  cursor: pointer;
  display: inline-flex;
  fill: currentcolor;
  font-family: "Google Sans", Roboto, Arial, sans-serif;
  font-size: 14px;
  font-weight: 500;
  height: 38px;
  justify-content: center;
  letter-spacing: 0.25px;
  line-height: normal;
  max-width: 100%;
  overflow: visible;
  position: relative;
  text-align: center;
  text-transform: none;
  transition: box-shadow 280ms cubic-bezier(0.4, 0, 0.2, 1),
    opacity 15ms linear 30ms, transform 270ms cubic-bezier(0, 0, 0.2, 1) 0ms;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  width: auto;
  will-change: transform, opacity;
  z-index: 0;
}

.button-17:hover {
  background: #f77727;
  color: #fff;
}

.button-17:active {
  box-shadow: 0 4px 4px 0 rgb(60 64 67 / 30%),
    0 8px 12px 6px rgb(60 64 67 / 15%);
  outline: none;
}

.button-17:focus {
  outline: none;
  border: 2px solid #4285f4;
}

.button-17:not(:disabled) {
  box-shadow: rgba(60, 64, 67, 0.3) 0 1px 3px 0,
    rgba(60, 64, 67, 0.15) 0 4px 8px 3px;
}

.button-17:not(:disabled):hover {
  box-shadow: rgba(60, 64, 67, 0.3) 0 2px 3px 0,
    rgba(60, 64, 67, 0.15) 0 6px 10px 4px;
}

.button-17:not(:disabled):focus {
  box-shadow: rgba(60, 64, 67, 0.3) 0 1px 3px 0,
    rgba(60, 64, 67, 0.15) 0 4px 8px 3px;
}

.button-17:not(:disabled):active {
  box-shadow: rgba(60, 64, 67, 0.3) 0 4px 4px 0,
    rgba(60, 64, 67, 0.15) 0 8px 12px 6px;
}

.button-17:disabled {
  box-shadow: rgba(60, 64, 67, 0.3) 0 1px 3px 0,
    rgba(60, 64, 67, 0.15) 0 4px 8px 3px;
}
</style>