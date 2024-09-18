<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config Class Work (4):</span>
      </div>
    </div>
    <div class="row col-md-12">
      <!-- left content -->
      <div class="content-left col-md-3 border rounded mt-5">
        <div class="row">
          <div class="col-md-12 mb-3">
            <label for="validationDefault02"><b>Line name</b></label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="ListModel"
              @update-selected-item="UpdateModelReceive"
              :textContent="model.LINE_NAME"
              type="model"
              textPlaceHolder="Enter line name"
            />
          </div>
        </div>

        <div class="row">
          <div class="col-md-6">
            <label for="validationDefault01" class="title-form">Class</label>
            <select
              name=""
              id=""
              class="form-control form-control-sm text-element"
              v-model="model.CUSTOMER"
            >
              <option value="">Choose class...</option>
              <option value="D">Day</option>
              <option value="N">Night</option>
            </select>
          </div>
          <div class="col-md-6">
            <label for="validationDefault01" class="title-form">Day</label>
            <select
              name=""
              id=""
              class="form-control form-control-sm text-element"
              v-model="model.DAY"
            >
              <option value="">Choose day...</option>
              <option value="YESTERDAY">YESTERDAY</option>
              <option value="TODAY">TODAY</option>
              <option value="TOMORROW">TOMORROW</option>
            </select>
          </div>
        </div>

        <div class="row">
          <div class="col-md-6">
            <label for="validationDefault01" class="title-form"
              >Start time</label
            >
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault01"
              v-model="model.START_TIME"
              required
            />
          </div>
          <div class="col-md-6">
            <label for="validationDefault01" class="title-form">End time</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault01"
              v-model="model.END_TIME"
              required
            />
          </div>
        </div>
        <div class="row mt-5">
          <div class="col-sm-3"></div>
          <div class="col-sm-6">
            <button class="btn btn-success">Add to queue</button>
          </div>
          <div class="col-sm-3"></div>
        </div>
      </div>
      <!-- center content -->
      <div class="content-center col-md-5 mt-4">
        <div class="row">
          <div class="col-sm-3"></div>
          <div class="col-sm-3">
            <button class="btn btn-primary" @click="CoppyFromLine()">
              Coppy from
            </button>
          </div>
          <div class="col-sm-3">
            <input
              type="text"
              class="form-control form-control-sm text-element"
              id="validationDefault01"
              v-model="model.LINE_COPPY"
              required
            />
          </div>
          <div class="col-sm-3"></div>
        </div>
        <div class="main-contain mt-3">
          <!-- <div class="row col-sm-12 div-content">
            <template v-if="DataTableHeader">
              <table id="tableMain" class="table mytable">
                <thead>
                  <tr>
                    <th>LINE_NAME</th>
                    <th>START_TIME</th>
                    <th>END_TIME</th>
                    <th>CLASS</th>
                    <th>SERIAL</th>
                    <th>DAY_DISTINCT</th>
                  </tr>
                </thead>
                <template v-for="(item, index) in ListQueue" :key="index">
                  <tr>
                    <template v-for="(item1, index1) in item" :key="index1">
                      <td v-if="index1 != 'ID' && index1 != 'SECTION_NAME' && index1 != 'MAP_WORK_SECTION' ">{{ item1 }}</td>
                    </template>
                  </tr>
                </template>
              </table>
            </template>
          </div> -->
        </div>
        <div class="row mt-3">
          <div class="col-sm-6"></div>
          <div class="col-sm-3">
            <!-- <button class="btn btn-success">Apply</button> -->
          </div>
          <div class="col-sm-6"></div>
        </div>
      </div>
      <!-- right content -->
      <div class="content-right col-md-4">
        <div class="div-searchbox row">
          <div class="div-searchbox-content">
            <input
              v-on:keyup.enter="QuerySearch()"
              v-model="valueSearch"
              type="text"
              ref="input"
              class="form-control"
              @click="selectAll"
              :placeholder="
                $store.state.language == 'En'
                  ? 'Enter line name...'
                  : 'Nhập tên line...'
              "
            />
            <button @click="QuerySearch()" class="btn">
              <Icon icon="search" class="sidenav-icon" />
            </button>
          </div>
        </div>
        <div class="main-contain">
          <div class="row col-sm-12 div-content">
            <template v-if="DataTableHeader">
              <table id="tableMain" class="table mytable">
                <thead>
                  <tr>
                    <template
                      v-for="(item, index) in DataTableHeader"
                      :key="index"
                    >
                      <th v-if="item != 'ID'">
                        {{ item }}
                      </th>
                    </template>
                  </tr>
                </thead>
                <template v-for="(item, index) in DataTable" :key="index">
                  <tr @click="SetCoppyLine(item)">
                    <template v-for="(item1, index1) in item" :key="index1">
                      <td v-if="index1 != 'ID'">{{ item1 }}</td>
                    </template>
                  </tr>
                </template>
              </table>
            </template>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
//import $ from 'jquery';
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
export default {
  components: {
    DropdownSearch,
  },
  data() {
    return {
      textContent: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      columnName: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      ListModel: [],
      ListQueue: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        LINE_COPPY: "",
        LINE_NAME: "",
        fun: "CUST SNRULE_",
        CUST_NAME: "",
        CUST_CODE: "",
        MODEL_NAME: "",
        VERSION_CODE: "",
        CUST_MODEL_NAME: "",
        CUST_MODEL_DESC: "",
        CARTON_LAB_NAME: "C_DEFAULT.LAB",
        UPCEANDATA: "",
        CUST_SN_PREFIX: "",
        CUST_VENDER_CODE: "",
        CUST_SN_POSTFIX: "",
        CUST_SN_LENG: "",
        CUST_SN_STR: "",
        CUST_CARTON_PREFIX: "AMBIT",
        CUST_CARTON_POSTFIX: "",
        CUST_CARTON_LENG: "6",
        CUST_CARTON_STR: "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
        CUST_PALLET_PREFIX: "AMBIT",
        CUST_PALLET_POSTFIX: "",
        CUST_PALLET_LENG: "6",
        CUST_PALLET_STR: "",
        CUST_LAST_SN: "",
        CUST_LAST_CARTON: "",
        CUST_LAST_PALLET: "",
        EMP_NO: "",
        D1: "",
        IN_STATION_TIME: "",
        PALLET_LAB_NAME: "P_DEFAULT.LAB",
        MACID_PREFIX: "",
        CUST_BOX_PREFIX: "",
        CUST_BOX_LENG: "0",
        CUST_LAST_BOX: "",
        CUST_BOX_STR: "",
        FINISH_GOOD: "",
      },
      listChecked: [],
      ListCustomer: [],
    };
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.isVisible = false;
      }
    });
  },
  computed: {
    filterUser: function () {
      const query = this.searchText.toLowerCase();
      if (this.searchText === "") {
        return this.ListModel;
      }
      return this.ListModel.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },
  mounted() {
    this.CheckPrivilege();
    this.GetModel();
    this.GetCustomer();
  },
  methods: {
    async CoppyFromLine() {
      if (this.model.LINE_NAME == "" || this.model.LINE_COPPY == "") {
        if (localStorage.language == "En") {
          this.$swal("", "Choose line please", "error");
        } else {
          this.$swal("", "Chưa chọn line", "error");
        }
      } else {
        if (this.model.LINE_NAME == this.model.LINE_COPPY) {
          if (localStorage.language == "En") {
            this.$swal("", "Dupplicate line", "error");
          } else {
            this.$swal("", "Bị trùng line", "error");
          }
        } else {
          var payload = {
            database_name: localStorage.databaseName,
            NEW_LINE_NAME: this.model.LINE_NAME,
            OLD_LINE_NAME: this.model.LINE_COPPY,
          };
          var { data } = await Repository.getRepo(
            "CoppyTemplateSection",
            payload
          );
          if (data.result == "ok") {
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
          } else {
            this.$swal("", "Line not exist", "error");
          }
        }
      }
    },
    SetCoppyLine(item) {
      this.model.LINE_COPPY = item.LINE_NAME;
    },
    UpdateCustReceive(value) {
      this.model.CUST_NAME = value.CUSTOMER;
      this.model.CUST_CODE = value.CUST_CODE;
    },
    UpdateModelReceive(value) {
      this.model.LINE_NAME = value;
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config4GetAllModel", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    async GetCustomer() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config4GetCustomer", payload);
      this.ListCustomer = data.data;
    },
    ChangeModelName() {
      this.model.PALLET_LAB_NAME = `P_${this.model.MODEL_NAME}.LAB`;
      this.model.CARTON_LAB_NAME = `${this.model.MODEL_NAME}.LAB`;
    },
    async SaveData() {
      if (this.model.MODEL_NAME == "" || this.model.VERSION_CODE == "") {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      } else {
        var titleValue = "";
        var textValue = "";
        if (localStorage.language == "En") {
          titleValue = "Are you sure edit?";
          textValue = "Once OK, data will be updated!";
        } else {
          titleValue = "Chắc chắn sửa?";
          textValue = "Dữ liệu sẽ được cập nhật";
        }
        this.$swal({
          title: titleValue,
          text: textValue,
          icon: "warning",
          buttons: true,
          dangerMode: true,
        }).then(async (willDelete) => {
          if (willDelete.isConfirmed == false) return;

          var { data } = await Repository.getRepo(
            "InsertUpdateConfig4",
            this.model
          );
          if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else if (data.result == "ok") {
            await this.QuerySearch();
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
          } else {
            this.$swal("", data.result, "error");
          }
        });
      }
    },
    DeleteRecord(item) {
      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure?";
        textValue =
          "Once deleted, you will not be able to recover this record!";
      } else {
        titleValue = "Chắc chắn xóa?";
        textValue = "Sau khi xóa sẽ không thể khôi phục!";
      }
      this.$swal({
        title: titleValue,
        text: textValue,
        icon: "warning",
        buttons: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          ID: item.ID,
          EMP: localStorage.username,
          MODEL_NAME: item.MODEL_NAME,
          VERSION_CODE: item.VERSION_CODE,
        };
        var { data } = await Repository.getRepo("DeleteConfig4", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
      this.line_name = "";
      this.line_type = "";
      this.line_code = "";
      this.line_desc = "";
    },
    ClearForm() {
      this.model.ID = "";
      this.model.CUST_CODE = "";
      this.model.MODEL_NAME = "";
      this.model.VERSION_CODE = "";
      this.model.CUST_MODEL_NAME = "";
      this.model.CUST_MODEL_DESC = "";
      this.model.CARTON_LAB_NAME = "C_DEFAULT.LAB";
      this.model.UPCEANDATA = "";
      this.model.CUST_SN_PREFIX = "";
      this.model.CUST_VENDER_CODE = "";
      this.model.CUST_SN_POSTFIX = "";
      this.model.CUST_SN_LENG = "";
      this.model.CUST_SN_STR = "";
      this.model.CUST_CARTON_PREFIX = "AMBIT";
      this.vCUST_CARTON_POSTFIX = "";
      this.model.CUST_CARTON_LENG = "6";
      this.model.CUST_CARTON_STR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      this.model.CUST_PALLET_PREFIX = "AMBIT";
      this.model.CUST_PALLET_POSTFIX = "";
      this.model.CUST_PALLET_LENG = "6";
      this.model.CUST_PALLET_STR = "";
      this.model.CUST_LAST_SN = "";
      this.model.CUST_LAST_CARTON = "";
      this.model.CUST_LAST_PALLET = "";
      this.model.EMP_NO = "";
      this.model.D1 = "";
      this.model.IN_STATION_TIME = "";
      this.model.PALLET_LAB_NAME = "P_DEFAULT.LAB";
      this.model.MACID_PREFIX = "";
      this.model.CUST_BOX_PREFIX = "";
      this.model.CUST_BOX_LENG = "0";
      this.model.CUST_LAST_BOX = "";
      this.model.CUST_BOX_STR = "";
      this.model.FINISH_GOOD = "";
    },
    ShowDetail(detail) {
      this.model.ID = detail.ID;
      this.model.CUST_NAME = detail.CUSTOMER;
      this.model.CUST_CODE = detail.CUST_CODE;
      this.model.MODEL_NAME = detail.MODEL_NAME;
      this.model.VERSION_CODE = detail.VERSION_CODE;
      this.model.CUST_MODEL_NAME = detail.CUST_MODEL_NAME;
      this.model.CUST_MODEL_DESC = detail.CUST_MODEL_DESC;
      this.model.CARTON_LAB_NAME = detail.CARTON_LAB_NAME;
      this.model.UPCEANDATA = detail.UPCEANDATA;
      this.model.CUST_SN_PREFIX = detail.CUST_SN_PREFIX;
      this.model.CUST_VENDER_CODE = detail.CUST_VENDER_CODE;
      this.model.CUST_SN_POSTFIX = detail.CUST_SN_POSTFIX;
      this.model.CUST_SN_LENG = detail.CUST_SN_LENG;
      this.model.CUST_SN_STR = detail.CUST_SN_STR;
      this.model.CUST_CARTON_PREFIX = detail.CUST_CARTON_PREFIX;
      this.model.CUST_CARTON_POSTFIX = detail.CUST_CARTON_POSTFIX;
      this.model.CUST_CARTON_LENG = detail.CUST_CARTON_LENG;
      this.model.CUST_CARTON_STR = detail.CUST_CARTON_STR;
      this.model.CUST_PALLET_PREFIX = detail.CUST_PALLET_PREFIX;
      this.model.CUST_PALLET_POSTFIX = detail.CUST_PALLET_POSTFIX;
      this.model.CUST_PALLET_LENG = detail.CUST_PALLET_LENG;
      this.model.CUST_PALLET_STR = detail.CUST_PALLET_STR;
      this.model.CUST_LAST_SN = detail.CUST_LAST_SN;
      this.model.CUST_LAST_CARTON = detail.CUST_LAST_CARTON;
      this.model.CUST_LAST_PALLET = detail.CUST_LAST_PALLET;
      this.model.EMP_NO = detail.EMP_NO;
      this.model.D1 = detail.D1;
      this.model.IN_STATION_TIME = detail.IN_STATION_TIME;
      this.model.PALLET_LAB_NAME = detail.PALLET_LAB_NAME;
      this.model.MACID_PREFIX = detail.MACID_PREFIX;
      this.model.CUST_BOX_PREFIX = detail.CUST_BOX_PREFIX;
      this.model.CUST_BOX_LENG = detail.CUST_BOX_LENG;
      this.model.CUST_LAST_BOX = detail.CUST_LAST_BOX;
      this.model.CUST_BOX_STR = detail.CUST_BOX_STR;
      this.model.FINISH_GOOD = detail.FINISH_GOOD;
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "WORK CLASS_",
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        this.$router.push({ path: "/Home/ConfigApp" });
      } else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig4Content", payload);
      this.DataTable = [];
      this.DataTable = data.data;
      if (typeof this.DataTable != "undefined") {
        if (this.DataTable.length != 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
          this.DataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
      }
    },
    async QuerySearch() {
      var payload = {
        database_name: localStorage.databaseName,
        LINE_NAME: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig4Content", payload);
      this.DataTable = [];
      this.DataTable = data.data;
      if (typeof this.DataTable != "undefined") {
        if (this.DataTable.length != 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
          this.DataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.title-form {
  font-size: 12px;
  font-weight: bold;
}
.dropdown-wrapper {
  max-width: 100%;
  position: relative;
  margin: 0 auto;
}

.text-element {
  color: #000;
  font-weight: 555;
}
.td-delete {
  display: flex;
  justify-content: center;
  background: #e73a23;
  cursor: pointer;
  height: 30px;
  align-items: center;
}
.td-edit {
  text-align: center;
  justify-content: center;
  background: #f88838;
  color: #fff;
  height: 30px;
  align-items: center;
  align-content: center;
  cursor: pointer;
  margin: 10px;
  &:hover {
    background: #db6008;
  }
}
.btn-warning {
  margin-left: 20px;
}
.btn-danger {
  margin-left: 20px;
  height: 40px;
}
.div-button {
  margin-top: 10px;
  position: relative;
  right: 50px;
  text-align: right;
}
.div-bellow {
  margin-top: 5px;
  background: #1c87b5;
  color: #fff;
  padding: 15px;
  margin-right: 20px;
  div {
    div {
      label {
        font-size: 13px;
        font-weight: bold;
        color: #9ff9c8;
      }
    }
  }
}
.div-all {
  margin-left: 35px;
}
.div-searchbox {
  margin-top: 15px;
  height: 60px;
  display: flex;
  align-content: center;
  justify-content: left;
  .div-searchbox-content {
    // position: absolute;
    display: flex;
    // bottom: 0;
    margin-bottom: 10px;
    // left: 50%;
    text-align: center;
    input {
      border-radius: 10px 0 0 10px;
      // padding: 0px 5px;
      width: 400px;
    }
    button {
      border-radius: 0 10px 10px 0;
      padding: 0 20px;
      background: #ff7a1c;
      color: #fff;
      box-sizing: 0;
      &:hover {
        background: #f88838;
      }
    }
  }
}
.div-config-name {
  margin-left: 20px;
  line-height: 50px;
  span {
    font-weight: 555;
    font-size: 17px;
  }
}
.main-contain {
  height: 450px;
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