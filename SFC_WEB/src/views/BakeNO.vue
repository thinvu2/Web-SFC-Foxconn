<template>
  <div class="content" style="min-height: 700px">
    <div class="row">
      <div class="col-2 select-div">
        <!-- <span class="text-title">Type</span> -->
        <select
          v-model="MODEL_SERIAL"
          name="select"
          class="form-control form-control-sm select-form"
          v-on:change="UpdateModelSerial"
        >
          <option value="SONY">SONY & SEQUANS</option>
          <option value="THALES">TELIT</option>
        </select>
      </div>
    </div>
    <div class="row">
      <h3 style="color: #00e500">BAKE</h3>
      <h3 style="margin-left: 30%; color: #00e500">
        HỆ THỐNG KIỂM SOÁT THỜI GIAN LÒ NUNG
      </h3>
    </div>
    <div class="row">
      <span
        class="col-sm-3 stySection"
        style="background: red; border: solid 1px #dddd"
        >{{ RED }}</span
      >
      <span
        class="col-sm-3 stySection"
        style="background: yellow; border: solid 1px #dddd"
        >{{ YEL }}
      </span>
      <span
        class="col-sm-3 stySection"
        style="background: #00e500; border: solid 1px #dddd"
        >{{ GRE }}
      </span>
      <span
        class="col-sm-3 stySection"
        style="background: #ffff; border: solid 1px #dddd"
      >
        No product (The Bake is empty)</span
      >
    </div>

    <div class="row">
      <table
        id="tableMain"
        class="table mytable"
        style="border: solid 1px #dddd"
      >
        <thead>
          <tr>
            <th class="stySection1">Section</th>
            <th class="stySection1">Location</th>
            <template v-for="(item_colums, Colums) in DataColums" :key="Colums">
              <th class="stySection">{{ item_colums }}</th>
            </template>
          </tr>
        </thead>

        <template v-for="(item, index) in DataTable" :key="index">
          <tr>
            <td v-if="item.value != ''" class="stySection1" rowspan="2">
              {{ item.value }}
            </td>
            <td class="stySection1">{{ item.key }}</td>

            <template v-for="(item_colums, Colums) in DataColums" :key="Colums">
              <template v-for="(item1, index1) in dataListBake" :key="index1">
                <template v-if="item1.SUB.slice(2, 3) == item_colums">
                  <template v-if="item1.SUB.slice(0, 1) == item.value">
                    <td
                      v-if="item1.COLOR == 'RED'"
                      class="stydata1"
                      style="background: red; border: 1px solid #dddd"
                    >
                      <div>Model: {{ item1.MODEL_NAME }}</div>
                      <div>Tray: {{ item1.TRAY }}</div>
                      <div>Qty: {{ item1.QTY }}</div>
                      <div>RoastIn: {{ item1.TIME }}</div>
                      <button
                        class="button"
                        @click="EnterElement(item1.LOCATION)"
                      >
                        Bake{{ item1.SUB }}
                      </button>
                    </td>
                    <td
                      v-else-if="item1.COLOR == 'YELLOW'"
                      class="stydata1"
                      style="background: yellow; border: 1px solid #dddd"
                    >
                      <div>Model: {{ item1.MODEL_NAME }}</div>
                      <div>Tray: {{ item1.TRAY }}</div>
                      <div>Qty: {{ item1.QTY }}</div>
                      <div>RoastIn: {{ item1.TIME }}</div>
                      <button
                        class="button"
                        @click="EnterElement(item1.LOCATION)"
                      >
                        Bake{{ item1.SUB }}
                      </button>
                    </td>
                    <td
                      v-else-if="item1.COLOR == 'GREEN'"
                      class="stydata1"
                      style="background: #00e500; border: 1px solid #dddd"
                    >
                      <div>Model: {{ item1.MODEL_NAME }}</div>
                      <div>Tray: {{ item1.TRAY }}</div>
                      <div>Qty: {{ item1.QTY }}</div>
                      <div>RoastIn: {{ item1.TIME }}</div>
                      <button
                        class="button"
                        @click="EnterElement(item1.LOCATION)"
                      >
                        Bake{{ item1.SUB }}
                      </button>
                    </td>
                    <td v-else class="stydata1" style="border: 1px solid #ddd">
                      <div></div>
                    </td>
                  </template>
                </template>
              </template>
            </template>
          </tr>
        </template>
      </table>
    </div>
  </div>
</template>

<script>
//import { mapState, mapMutations } from 'vuex';
import Repository from "../services/Repository";

export default {
  name: "BakeNo",
  data() {
    return {
      DataColums: ["1", "2", "3", "4", "5", "6", "7", "8", "9"],
      DataRows: [
        "A1",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        "O",
        "P",
      ],
      dataList: [],
      dataListBake: [],
      rowA: [],
      rowB: [],
      rowC: [],
      rowD: [],
      rowE: [],
      rowF: [],
      rowG: [],
      rowH: [],
      rowI: [],
      rowJ: [],
      rowK: [],
      rowL: [],
      rowM: [],
      rowN: [],
      rowO: [],
      rowP: [],
      DataTable: [
        { value: "A", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "B", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "C", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "D", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "E", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "F", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "G", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "H", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "I", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "J", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "K", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "L", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "M", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "N", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "O", key: "Inside" },
        { value: "", key: "Outside" },
        { value: "P", key: "Inside" },
        { value: "", key: "Outside" },
      ],
      dialog: false,
      stateFocus: false,
    };
  },
  components: {
    // QrcodeCapture,
    // QrcodeStream,
  },
  created() {
    //document.title = "Config Application";
    this.MODEL_SERIAL = "SONY";
    this.RED = "Baking < 10h";
    this.YEL = "10h < Baking < 12h";
    this.GRE = "Baking > 12h (Baking OK)";
  },
  mounted() {
    document.title = "Bake No";
    this.GetDatalist();
  },
  methods: {
    async EnterElement(type) {
      this.$store.state.elementQ6 = this.MODEL_SERIAL;
      console.log(this.MODEL_SERIAL + type);

      // this.$store.state.lock_emp_pass = type;
      // this.$store.state.elementQ6 = this.MODEL_SERIAL;
      // this.$store.state.isShowBAKEElement = true;

      // this.stateFocus = !this.stateFocus;

      //this.$store.state.lock_emp_pass = this.$store.state.lock_emp_pass.replace(/[\u200B-\u200D\uFEFF]/g, "");
      var payload = {
        database: localStorage.databaseName,
        model_serial: this.MODEL_SERIAL,
        BakeNo: type,
      };
      var { data } = await Repository.getRepo("GetTrayinBake", payload);

      console.log(data.message);
      if (data.message == "ok") {
        this.$store.state.listDetailClickAll = data.data;
        this.$store.state.listDetailClick =
          this.$store.state.listDetailClickAll.slice(0, 500);
        if (data.data.length > 0) {
          this.$store.commit("showModal");
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
      this.text_alert = "";
    },
    showModal() {
      this.dialog = true;
    },
    UpdateModelSerial(res) {
      this.GetDatalist();
      if (res.target.value == "SONY") {
        this.RED = "Baking < 10h";
        this.YEL = "10h < Baking < 12h";
        this.GRE = "Baking > 12h (Baking OK)";
      } else {
        this.RED = "Baking < 20h";
        this.YEL = "20h < Baking < 24h";
        this.GRE = "Baking > 24h (Baking OK)";
      }
    },
    GotoRoute(route) {
      this.$router.push({ path: route });
    },
    BackToParent() {
      this.$router.push({ path: "/Home/Applications" });
    },
    async GetDatalist() {
      var payload = {
        database: localStorage.databaseName,
        model_serial: this.MODEL_SERIAL,
      };
      var { data } = await Repository.getRepo("GetAllBake", payload);
      this.dataList = [];
      this.dataList = data.data;

      this.dataListBake = [];
      if (typeof this.dataList != "undefined") {
        if (this.dataList.length != 0) {
          this.dataList.forEach((element) => {
            this.dataListBake.push(element);
          });

          //console.log(this.dataListBake);
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.config-content {
  display: grid;
  height: 90vh;

  background: #7cb5f5;
  grid-template-columns: 25% 25% 25% 25%;
  grid-template-rows: 10% 10% 10% 10%;
  grid-column-gap: 200px;
  grid-row-gap: 10px;
  //margin-left: 2px;
}
ol {
  //counter-reset: li; /* Initiate a counter */
  list-style: none; /* Remove default numbering */
  // *list-style: decimal; /* Keep using default numbering for IE6/7 */
  font: 20px "trebuchet MS", "lucida sans";
  padding: 0;
  margin-bottom: 4em;
  margin-left: 20px;
  text-shadow: 0 1px 0 rgba(187, 184, 184, 0.5);
}

.rounded-list span {
  position: relative;
  display: grid;
  padding: 0.4em 0.4em 0.4em 2em;
  *padding: 0.4em;
  margin: 0.5em 0;
  background: rgb(202, 217, 236);
  color: rgb(12, 11, 11);

  text-align: left;
  border-radius: 0.3em;
  transition: all 0.3s ease-out;
  box-shadow: 10px 5px 5px rgb(223, 231, 231);
  width: 230px;
}

.rounded-list span:hover {
  background: rgb(33, 229, 243);
  cursor: pointer;
}

.rounded-list span:hover:before {
  transform: rotate(360deg);
}

.rounded-list span:before {
  //content: counter(li);
  content: "";
  //counter-increment: li;
  position: absolute;
  left: -1.3em;
  top: 50%;
  margin-top: -1.3em;
  background: #6967f7;
  height: 2em;
  width: 2em;
  line-height: 2em;
  margin-left: 15px;
  border: 0.3em solid #7cb5f5;
  text-align: left;
  font-weight: bold;
  border-radius: 2em;
  transition: all 0.3s ease-out;
}
.config-border {
  border: 1px solid #ada4a4;
  background: #fff;
  text-color: #0000;
}
.font-text {
  font-size: 20px;
}
.div-back {
  background: #eae1e1;
  cursor: pointer;
  margin: 10px 0;
  display: flex;
  align-content: center;
  align-items: center;
  width: 5%;
  border-radius: 10%;
}
.div-back:hover {
  background: #b7b7b7;
}
.stybody {
  background-color: Black;
  width: 100%;
}
.styleMemo {
  width: 25%;
}
.stytable {
  color: #00e500;
  text-align: left;
  border-color: Green;
}
.stySection {
  width: 8%;
  text-align: center;
}
.stySection1 {
  width: 4%;
  text-align: center;
}
.stydata1 {
  max-width: 20%;
}
.stydata {
  width: 15%;
  color: #000000;
}

.button {
  padding: 10px 25px;
  cursor: pointer;
  text-align: center;
  text-decoration: none;
  color: #06033b;
  border: none;
  border-radius: 8px;
  box-shadow: 0 9px #999;
}
// .button:hover {
//   background-color: #838383;
// }
// .button:active {
//   background-color: #f8f8f8;
//   box-shadow: 0 5px #666;
//   transform: translateY(4px);
// }
</style>