<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config STEP RULE (44):</span>
      </div>
    </div>
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
              ? 'Enter model name...'
              : 'Nhập tên hàng...'
          "
        />
        <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
    </div>

    <div class="col-md-12">
      <div class="col-md-6">
        <div class="main-contain">
          <template v-if="DataTableHeader">
            <table id="tableMain" class="table mytable">
              <thead>
                <tr>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                  </th>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Detail" : "Chi tiết" }}
                  </th>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "COPY" : "COPY" }}
                  </th>
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
                <tr>
                  <td class="td-delete" @click="DeleteModel(item)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path fill="none" d="M0 0h24v24H0z" />
                      <path
                        fill="#FFF"
                        d="M17 6h5v2h-2v13a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V8H2V6h5V3a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v3zm1 2H6v12h12V8zm-9 3h2v6H9v-6zm4 0h2v6h-2v-6zM9 4v2h6V4H9z"
                      />
                    </svg>
                  </td>
                  <td class="td-edit" @click="ShowCustSnDetail(item)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path fill="none" d="M0 0h24v24H0z" />
                      <path
                        fill="#FFF"
                        d="M16.757 3l-2 2H5v14h14V9.243l2-2V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h12.757zm3.728-.9L21.9 3.516l-9.192 9.192-1.412.003-.002-1.417L20.485 2.1z"
                      />
                    </svg>
                  </td>
                  <td class="td-copy" @click="AddnewCust(item)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path
                        d="M16,20H8a3,3,0,0,1-3-3V7A1,1,0,0,0,3,7V17a5,5,0,0,0,5,5h8a1,1,0,0,0,0-2ZM21,8.94a1.31,1.31,0,0,0-.06-.27l0-.09a1.07,1.07,0,0,0-.19-.28h0l-6-6h0a1.07,1.07,0,0,0-.28-.19l-.09,0L14.06,2H10A3,3,0,0,0,7,5V15a3,3,0,0,0,3,3h8a3,3,0,0,0,3-3V9S21,9,21,8.94ZM15,5.41,17.59,8H16a1,1,0,0,1-1-1ZM19,15a1,1,0,0,1-1,1H10a1,1,0,0,1-1-1V5a1,1,0,0,1,1-1h3V7a3,3,0,0,0,3,3h3Z"
                      />
                    </svg>
                  </td>
                  <template v-for="(item1, index1) in item" :key="index1">
                    <td v-if="index1 != 'ID'">{{ item1 }}</td>
                  </template>
                </tr>
              </template>
            </table>
          </template>
        </div>
      </div>

      <div class="col-md-6 main-contain">
        <template v-if="DataTableHeaderDetail">
          <table class="table mytable">
            <thead>
              <tr>
                <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                </th>
                <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Edit" : "Sửa" }}
                </th>
                <template
                  v-for="(item2, index2) in DataTableHeaderDetail"
                  :key="index2"
                >
                  <th
                    v-if="
                      item2 != 'MODEL_NAME' &&
                      item2 != 'VERSION_CODE' &&
                      item2 != 'ID'
                    "
                    style="width: 1px"
                  >
                    {{ item2 }}
                  </th>
                </template>
              </tr>
              <template
                v-for="(item3, index3) in DataTableDetail"
                :key="index3"
              >
                <tr>
                  <td class="td-delete" @click="DeleteRecord(item3)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path fill="none" d="M0 0h24v24H0z" />
                      <path
                        fill="#FFF"
                        d="M17 6h5v2h-2v13a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V8H2V6h5V3a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v3zm1 2H6v12h12V8zm-9 3h2v6H9v-6zm4 0h2v6h-2v-6zM9 4v2h6V4H9z"
                      />
                    </svg>
                  </td>
                  <td class="td-edit" @click="ShowDetail(item3)">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path fill="none" d="M0 0h24v24H0z" />
                      <path
                        fill="#FFF"
                        d="M16.757 3l-2 2H5v14h14V9.243l2-2V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h12.757zm3.728-.9L21.9 3.516l-9.192 9.192-1.412.003-.002-1.417L20.485 2.1z"
                      />
                    </svg>
                  </td>
                  <template v-for="(item4, index4) in item3" :key="index4">
                    <td
                      v-if="
                        index4 != 'MODEL_NAME' &&
                        index4 != 'VERSION_CODE' &&
                        index4 != 'ID'
                      "
                    >
                      {{ item4 }}
                    </td>
                  </template>
                </tr>
              </template>
            </thead>
          </table>
        </template>
      </div>
    </div>

    <div class="div-button col-md-12">
      
      <button
        class="btn btn-success"
        type="submit"
        @click="SaveData()"
        title="Save"
      >
        Save
      </button>
      <button
        class="btn btn-warning"
        type="button"
        @click="ClearForm()"
        title="Refresh form"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          width="24"
          height="24"
        >
          <path fill="none" d="M0 0h24v24H0z" />
          <path
            fill="red"
            d="M5.463 4.433A9.961 9.961 0 0 1 12 2c5.523 0 10 4.477 10 10 0 2.136-.67 4.116-1.81 5.74L17 12h3A8 8 0 0 0 6.46 6.228l-.997-1.795zm13.074 15.134A9.961 9.961 0 0 1 12 22C6.477 22 2 17.523 2 12c0-2.136.67-4.116 1.81-5.74L7 12H4a8 8 0 0 0 13.54 5.772l.997 1.795z"
          />
        </svg>
      </button>
    </div>
    <template v-if="type_check == 'add'">
      <div class="col-md-12 div-bellow">
        <div class="col-md-6">
          <fieldset>
            <legend>ADD</legend>
            <div class="col-md-12">
              <div class="col-md-2 text-right">
                <label for="valiDefault01">Model Name</label>
              </div>
              <div class="col-md-8">
                <DropdownSearch
                  class="form-control form-control-sm text-element col-md-3"
                  :listAll="filterModelAdd"
                  @update-selected-item="UpdateModelNameAddReceive"
                  :textContent="model.MODEL_NAME_ADD"
                  type="model"
                  textPlaceHolder="Enter model name"
                />
              </div>
            </div>
            <div class="col-md-12">
              <div class="col-md-2 text-right">
                <label for="valiDefault01">Version Code</label>
              </div>
              <div class="col-md-8">
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.VERSION_CODE_ADD"
                />
              </div>
            </div>
            <div class="col-md-12">
              <div class="col-md-2 text-right">
                <label for="valiDefault01">Mo Type</label>
              </div>

              <div class="col-md-8">
                
                <select
                  class="form-control form-control-sm text-element"
                  v-model="model.MO_TYPE_ADD"
                >
                  <option v-for="item_add in ListMoType_Add" :key="item_add">
                    {{ item_add }}
                  </option>
                </select>
              </div>
            </div>
          </fieldset>
        </div>
        <div class="col-md-6">
          <!--style="border: 1px solid;border-radius: 4px;margin: 8px;"-->
          <fieldset>
            <legend>Copy:</legend>
            <div class="col-md-12">
              <div class="col-md-2">
                <label for="valiDefault02">Model Name</label>
              </div>
              <div class="col-md-8">
                <!--<DropdownSearch
                  class="form-control form-control-sm text-element col-md-3"
                  :listAll="filterModelCopy"
                  @update-selected-item="UpdateModelNameCopyReceive"
                  :textContent="model.MODEL_NAME_COPY"
                  type="model"
                  textPlaceHolder="Enter model name"
                />-->
                <input type="text" class="form-control form-control-sm text-element"
                  v-model="model.MODEL_NAME_COPY"
                  disabled
                >
              </div>
            </div>
            <div class="col-md-12">
              <div class="col-md-2">
                <label for="vaiDefault02">Version Code</label>
              </div>
              <div class="col-md-8">
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.VERSION_CODE_COPY"
                  disabled
                />
              </div>
            </div>
            <div class="col-md-12">
              <div class="col-md-2">
                <label for="vailDefault02">Mo Type</label>
              </div>
              <div class="col-md-8">
                <!--<select
                  class="form-control form-control-sm text-element"
                  v-model="model.MO_TYPE_COPY"
                >
                  <option v-for="item_copy in ListMoType_Copy" :key="item_copy">
                    {{ item_copy }}
                  </option>
                </select>-->
                <input type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.MO_TYPE_COPY"
                  disabled
                >
              </div>
            </div>
          </fieldset>
        </div>
      </div>
    </template>
    <template v-else>
      <div class="col-md-12 div-bellow">
        <div class="col-md-6">
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault01">Model Name :</label>
            </div>
            <div class="col-md-8">
              <input
                type="text"
                class="form-control form-control-sm text-element"
                v-model="model.MODEL_NAME"
                disabled
              />
            </div>
          </div>

          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault02">Version Code :</label>
            </div>
            <div class="col-md-8">
              <input
                type="text"
                class="form-control form-control-sm text-element"
                disabled
                v-model="model.VERSION_CODE"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="">CustSN Code</label>
            </div>
            <div class="col-md-6">
              <DropdownSearch
                class="form-control form-control-sm text-element"
                :listAll="ListCustSNCode"
                @update-selected-item="UpdateCustSNCodeReceive"
                :textContent="model.CUSTSN_CODE"
                type="model"
                textPlaceHolder="Enter cust sn"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault03">Cust SN Prefix :</label>
            </div>
            <div class="col-md-8">
              <input
                type="text"
                class="form-control form-control-sm text-element"
                v-model="model.CUSTSN_PREFIX"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault03">Cust SN Postfix</label>
            </div>
            <div class="col-md-8">
              <input
                type="text"
                class="form-control form-control-sm text-element"
                v-model="model.CUSTSN_POSTFIX"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault03">Cust SN Length</label>
            </div>
            <div class="col-md-4">
              <input
                type="number"
                class="form-control form-control-sm text-element"
                v-model="model.CUSTSN_LENG"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault04">Cust SN String</label>
            </div>
            <div class="col-md-8">
              <input
                type="text"
                class="form-control form-control-sm text-element"
                v-model="model.CUSTSN_STR"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault05">Check SSN :</label>
            </div>
            <div class="col-md-2">
              <select
                v-model="model.CHECK_SSN"
                class="form-control form-control-sm text-element"
              >
                <option value="N">N</option>
                <option value="Y">Y</option>
              </select>
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault06">Check Range :</label>
            </div>
            <div class="col-md-2">
              <select
                v-model="model.CHECK_RANGE"
                class="form-control form-control-sm text-element"
              >
                <option value="N">N</option>
                <option value="Y">Y</option>
              </select>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault06">Check Rule Name</label>
            </div>
            <div class="col-md-6">
              <DropdownSearch
                class="form-control form-control-sm text-element"
                :listAll="ListRoleName"
                @update-selected-item="UpdateRoleNameReceive"
                :textContent="model.CHECK_RULE_NAME"
                type="model"
                textPlaceHolder="Enter rule name"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault06">Shipping SN Code</label>
            </div>
            <div class="col-md-6">
              <input
                type="text"
                class="form-control form-control-sm text-element"
                v-model="model.SHIPPINGSN_CODE"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault07">Shipping SN Char</label>
            </div>
            <div class="col-md-6">
              <input
                type="text"
                class="form-control form-control-sm text-element"
              />
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right" style="height: 60px;">              
            </div>
            <div class="col-md-6">              
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-2 text-right">
              <label for="validationDefault07">Compare SN :</label>
            </div>
            <div class="col-md-6">
              <DropdownSearch
                class="form-control form-control-sm text-element"
                :listAll="ListCOMPARE_SN"
                @update-selected-item="UpdateCOMPARE_SNReceive"
                :textContent="model.COMPARE_SN"
                type="model"
                textPlaceHolder="Enter rule name"
              />
              <!-- <input
                type="text"
                class="form-control form-control-sm text-element"
                v-model="model.COMPARE_SN"
              /> -->
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-6">
              <div class="col-md-4 text-right">
                <label for="validationDefault07">Compare SN Start</label>
              </div>
              <div class="col-md-6">
                <input
                  type="number"
                  class="form-control form-control-sm text-element"
                  v-model="model.COMPARE_SN_START"
                />
              </div>
            </div>
            <div class="col-md-6">
              <div class="col-md-4 text-right">
                <label for="validationDefault07">Compare SN Flow Number</label>
              </div>
              <div class="col-md-6">
                <input
                  type="number"
                  class="form-control form-control-sm text-element"
                  v-model="model.COMPARE_SN_END"
                />
              </div>
            </div>
          </div>
          <div class="col-md-12">
            <div class="col-md-6">
              <div class="col-md-4 text-right">
                <label for="validationDefault07">Cust SN Start</label>
              </div>
              <div class="col-md-6">
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.CUSTSN_START"
                />
              </div>
            </div>
            <div class="col-md-6">
              <div class="col-md-4 text-right">
                <label for="validationDefault07">Cust SN Flow Number</label>
              </div>
              <div class="col-md-6">
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.CUSTSN_END"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>
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
      type_check: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      DataTableHeaderDetail: [],
      DataTableHeader: [],
      DataTable: [],
      DataTableDetail: [],
      DataTableRoleName: [],
      columnName: [],
      columnNamebom: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        MODEL_NAME: "",
        VERSION_CODE: "",
        CUSTSN_CODE: "",
        CUSTSN_PREFIX: "",
        CUSTSN_POSTFIX: "",
        CUSTSN_LENG: 0,
        CUSTSN_STR: "",
        CHECK_SSN: "",
        CHECK_RANGE: "",
        CHECK_RULE_NAME: "",
        SHIPPINGSN_CODE: "",
        COMPARE_SN: "",
        COMPARE_SN_START: 0,
        COMPARE_SN_END: 0,
        CUSTSN_START: 0,
        CUSTSN_END: 0,
        MO_TYPE: "",
        MODEL_NAME_ADD: "",
        VERSION_CODE_ADD: "",
        MO_TYPE_ADD: "",
        MODEL_NAME_COPY: "",
        VERSION_CODE_COPY: "",
        MO_TYPE_COPY: "",
      },
      listChecked: [],
      ListRoute: [],
      ListRoleName: [],
      ListModel: [],
      ListModel_Add: [],
      ListModel_Copy: [],
      ListMoType_Add: [],
      ListMoType_Copy: [],
      ListCustSNCode: [],
      ListMoType: [],
      ListCOMPARE_SN:[],
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
    filterModel: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListModel;
      if (this.searchText === "") {
        return this.ListModel;
      }
      return this.ListModel.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
    filterModelAdd: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListModel_Add;
      if (this.searchText === "") {
        return this.ListModel_Add;
      }
      return this.ListModel_Add.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
    filterModelCopy: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListModel_Copy;
      if (this.searchText === "") {
        return this.ListModel_Copy;
      }
      return this.ListModel_Copy.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },
  mounted() {
    this.CheckPrivilege();
    this.GetModel_Add();
    this.GetModel_Copy();
    //this.checkDisable();
  },
  methods: {
    UpdateRoleNameReceive(value) {
      this.model.CHECK_RULE_NAME = value;
    },
    UpdateCOMPARE_SNReceive(value) {
      this.model.COMPARE_SN = value;
    },
    UpdateCustSNCodeReceive(value) {
      this.model.CUSTSN_CODE = value;
      this.GetRoleName();
    },
    async GetCustSNCode() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME: this.model.MODEL_NAME,
        VERSION_CODE: this.model.VERSION_CODE,
        MO_TYPE: this.model.MO_TYPE,
      };
      var { data } = await Repository.getRepo("GetInsertCustSNCode", payload);

      this.ListCustSNCode = [];
      data.data.forEach((element) => {
        this.ListCustSNCode.push(element.CUSTSN_CODE);
      });

      this.ListCOMPARE_SN = [];
      this.ListCOMPARE_SN.push("");
      data.data1.forEach((element) => {
        this.ListCOMPARE_SN.push(element.CUSTSN_CODE);
      });
    },
    async GetRoleName() {
      var payload = {
        database_name: localStorage.databaseName,
        CUSTSN_CODE: this.model.CUSTSN_CODE,
      };
      var { data } = await Repository.getRepo("Get_Rull_Name", payload);

      this.ListRoleName = [];
      data.data.forEach((element) => {
        this.ListRoleName.push(element.CHECK_RULE_NAME);
      });
    },
    UpdateModelNameAddReceive(value) {
      this.model.MODEL_NAME_ADD = value;
      this.Get_Model_Type();
    },

    async GetModel_Add() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetModel_Config43", payload);
      data.data.forEach((element) => {
        this.ListModel_Add.push(element.MODEL_NAME);
      });
    },
    UpdateModelNameCopyReceive(value) {
      this.model.MODEL_NAME_COPY = value;
      this.Get_Mo_type_Copy();
    },
    async GetModel_Copy() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetModelCopy", payload);
      data.data.forEach((element) => {
        this.ListModel_Copy.push(element.MODEL_NAME);
      });
    },
    async Get_Model_Type() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME_ADD: this.model.MODEL_NAME_ADD,
      };
      var { data } = await Repository.getRepo("GetMoTypeAdd", payload);
      this.ListMoType_Add=[];
      data.data.forEach((element) => {
        this.ListMoType_Add.push(element.MO_TYPE);
      });
    },
    async Get_Mo_type_Copy() {
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME_COPY: this.model.MODEL_NAME_COPY,
      };
      var { data } = await Repository.getRepo("GetMoTypeCopy", payload);
      data.data.forEach((element) => {
        this.ListMoType_Copy.push(element.MO_TYPE);
      });
    },
    async SaveData() {
      if (this.type_check == "add") {
        if (
          this.model.MODEL_NAME_ADD == "" ||
          this.model.MODEL_NAME_COPY == "" ||
          this.model.VERSION_CODE_ADD == "" ||
          this.VERSION_CODE_COPY == "" ||
          this.model.MO_TYPE_ADD == "" ||
          this.model.MO_TYPE_COPY == ""
        ) {
          if (localStorage.language == "En") {
            this.$swal("", "Empty fields", "error");
          } else {
            this.$swal("", "Không duợc bỏ trống", "error");
          }
          return;
        }
      } else {
        if (this.model.MODEL_NAME == "" || this.model.CUSTSN_CODE == "") {
          if (localStorage.language == "En") {
            this.$swal("", "Empty fields", "error");
          } else {
            this.$swal("", "Không được bỏ trống", "error");
          }
          return;
        }
      }

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
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;

        var payload = "";

        if (this.type_check == "add") {
          payload = {
            database_name: localStorage.databaseName,
            EMP: localStorage.username,
            MODEL_NAME_ADD: this.model.MODEL_NAME_ADD,
            MODEL_NAME_COPY: this.model.MODEL_NAME_COPY,
            VERSION_CODE_ADD: this.model.VERSION_CODE_ADD,
            VERSION_CODE_COPY: this.model.VERSION_CODE_COPY,
            MO_TYPE_ADD: this.model.MO_TYPE_ADD,
            MO_TYPE_COPY: this.model.MO_TYPE_COPY,
            TYPE_CHECK: this.type_check,
          };
        }else{
          payload=this.model;
        }
          var { data } = await Repository.getRepo(
            "InsertUpdateConfig44",
            payload
          );
          if (data.result != "ok") {
            if (data.result == "privilege") {
              if (localStorage.language == "En") {
                this.$swal("", "Not privilege", "error");
              } else {
                this.$swal("", "Bạn không có quyền thêm sửa", "error");
              }
            } else {
              this.$swal("", data.result, "error");
            }
          } else if (data.result == "ok") {
            await this.QuerySearch();
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
            this.ClearForm();
          } else {
            this.$swal("", data.result, "error");
          }
        
      });
    },
    DeleteRecord(detail) {
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
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          ID: detail.ID,
          EMP: localStorage.username,
        };
        var { data } = await Repository.getRepo("DeleteConfig44", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
          this.ClearForm();
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "Bạn không có quyền thêm sửa", "error");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },
    async DeleteModel(detail){
      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure? Delete Model:"+detail.MODEL_NAME +" Ver:"+detail.VERSION_CODE+" Mo_type:"+detail.MO_TYPE;
        textValue =
          "Once deleted, you will not be able to recover this record!";
      } else {
        titleValue = "Chắc chắn xóa? Xóa Model:"+detail.MODEL_NAME+" Ver:"+detail.VERSION_CODE+" Mo_type:"+detail.MO_TYPE;
        textValue = "Sau khi xóa sẽ không thể khôi phục!";
      }
      this.$swal({
        title: titleValue,
        text: textValue,
        icon: "warning",
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          MODEL_NAME:detail.MODEL_NAME,
          VERSION_CODE:detail.VERSION_CODE,
          MO_TYPE:detail.MO_TYPE,
          EMP: localStorage.username,
          TYPE_CHECK:"MODEL",
        };
        var { data } = await Repository.getRepo("DeleteConfig44", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
          this.ClearForm();
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "Bạn không có quyền thêm sửa", "error");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },
    ClearForm() {
      this.type = "";
      this.model.ID = "";
      this.model.MODEL_NAME = "";
      this.model.VERSION_CODE = "";
      this.model.MO_TYPE = "";
      this.model.CHECK_SSN = "";
      this.model.CHECK_RANGE = "";
      this.model.CHECK_RULE_NAME = "";
      this.model.SHIPPINGSN_CODE = "";
      this.model.COMPARE_SN = "";
      this.model.COMPARE_SN_START = 0;
      this.model.COMPARE_SN_END = 0;
      this.model.CUSTSN_START = 0;
      this.model.CUSTSN_END = 0;
      this.model.CUSTSN_PREFIX = "";
      this.model.CUSTSN_POSTFIX = "";
      this.model.CUSTSN_STR = "";
      this.model.CUSTSN_LENG = "";
      this.model.CUSTSN_CODE = "";
      this.model.MODEL_NAME_ADD = "";
      this.model.MODEL_NAME_COPY = "";
      this.model.VERSION_CODE_ADD = "";
      this.model.VERSION_CODE_COPY = "";
      this.model.MO_TYPE_ADD = "";
      this.model.MO_TYPE_COPY = "";
      this.type_check = "";
      
      //this.GetRoleName();
      this.ListRoleName = [];
    },
    ShowDetail(detail) {
      this.model.ID = detail.ID;

      this.model.MODEL_NAME = detail.MODEL_NAME;
      this.model.VERSION_CODE = detail.VERSION_CODE;
      this.model.MO_TYPE = detail.MO_TYPE;
      this.model.CHECK_SSN = detail.CHECK_SSN;
      this.model.CHECK_RANGE = detail.CHECK_RANGE;
      this.model.CUSTSN_LENG = detail.CUSTSN_LENG;
      this.model.CUSTSN_PREFIX = detail.CUSTSN_PREFIX;
      this.model.CUSTSN_STR = detail.CUSTSN_STR;

      this.model.CHECK_RULE_NAME = detail.CHECK_RULE_NAME;
      this.model.SHIPPINGSN_CODE = detail.SHIPPINGSN_CODE;
      this.model.COMPARE_SN = detail.COMPARE_SN;
      this.model.COMPARE_SN_START = detail.COMPARE_SN_START;
      this.model.COMPARE_SN_END = detail.COMPARE_SN_END;
      this.model.CUSTSN_START = detail.CUSTSN_START;
      this.model.CUSTSN_END = detail.CUSTSN_END;
      this.model.MO_TYPE = detail.MO_TYPE;
      this.model.CUSTSN_POSTFIX = detail.CUSTSN_POSTFIX;
      this.model.CUSTSN_CODE = detail.CUSTSN_CODE;

      this.GetRoleName();
      

    },
    async ShowCustSnDetail(detail_1) {
      this.ClearForm();
      var payload = {
        database_name: localStorage.databaseName,
        MODEL_NAME: detail_1.MODEL_NAME,
        VERSION_CODE: detail_1.VERSION_CODE,
        MO_TYPE: detail_1.MO_TYPE,
      };

      var { data } = await Repository.getRepo("GetConfig44Aside", payload);

      this.DataTableDetail = [];
      this.DataTableDetail = data.data;

      if (typeof this.DataTableDetail != "undefined") {
        if (this.DataTableDetail.length != 0) {
          this.DataTableHeaderDetail = Object.keys(this.DataTableDetail[0]);
          this.DataTableHeaderDetail.forEach((element) => {
            this.columnNamebom.push({
              label: element,
              field: element,
            });
          });
        }

        this.model.MODEL_NAME = detail_1.MODEL_NAME;
        this.model.VERSION_CODE = detail_1.VERSION_CODE;
        this.model.MO_TYPE = detail_1.MO_TYPE;
        this.GetCustSNCode();

        this.model.CUSTSN_STR = "0123456789";
        this.model.CUSTSN_LENG = "0";
        this.model.CHECK_RANGE = "Y";
        this.model.CHECK_SSN = "Y";
      }
    },

    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    AddnewCust(detail) {
      
        this.type_check = "add";
        this.model.MODEL_NAME_COPY=detail.MODEL_NAME;
        this.model.VERSION_CODE_COPY=detail.VERSION_CODE;
        this.model.MO_TYPE_COPY=detail.MO_TYPE;
      
      //this.ClearForm();
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "PACK SCAN STEP",
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
      this.$store.state.listSelectDualConfig43 = [];
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig44Content", payload);
      this.DataTable = [];
      this.DataTableHeaderDetail = [];
      this.DataTable = data.data;
      this.DataTableHeaderDetail = data.data1;
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
        MODEL_NAME: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig44Content", payload);
      this.DataTable = [];
      this.DataTableDetail = [];
      this.DataTable = data.data;
      this.DataTableDetail = data.data1;
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
.td-copy {
  text-align: center;
  justify-content: center;
  background: #84fa40;
  color: #fff;
  height: 30px;
  align-items: center;
  align-content: center;
  cursor: pointer;
  margin: 10px;
  &:hover {
    background: #0fdb08;
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
  height: auto;
  background: #1c87b5;
  color: #fff;
  padding: 15px;
  margin-right: 20px;
  div {
    div {
      div {
        margin-top: 10px;
        label {
          font-size: 13px;
          font-weight: bold;
          color: #9ff9c8;
          text-align: right;
        }
        input {
          border-radius: 5px;
        }
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
  height: 350px;
  overflow: scroll;
}
.maincontain {
  height: 300px;
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
.div_select_item {
  height: 60px;
  ul {
    border-radius: 0 0 10px 10px;
    border: 1px solid #9e9e9e;
    padding: 0;
    width: 100%;
    height: 150px;

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
.lb_checked {
  color: #efef8d !important;
}
</style>