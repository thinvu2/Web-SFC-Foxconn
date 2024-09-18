<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config BOM (12):</span>
      </div>
    </div>
  
    <div class="div-searchbox row">
      <div class="div-searchbox-content">
        <input
        v-on:keyup.enter="QuerySearch()"
          v-model="searchQuery"
          type="text"
          class="form-control"
          @input="filterDataTable"
          :placeholder="
            $store.state.language == 'En'
              ? 'Enter BOM NO...'
              : 'Nh?p tên bom...'
          "
        />
        <!-- <button @click="filterDataTable" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button> -->
      </div>
    </div>

      <!-- <div class="div-searchbox row">
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
              ? 'Enter BOM NO...'
              : 'Nh?p tên bom...'
          "
        />
         <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
    </div>  -->

    <div class="main-contain">
      <div class="col-md-3">

        <template v-if="DataTableHeader">
      <table class="table mytable_1">
        <thead>
          <tr>
            <th v-for="(item2, index2) in DataTableHeader" :key="index2">
              {{ item2 }}
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-for="(item3, index3) in filteredDataTable" :key="index3">
            <tr :class="isSelectedRow" @click="ShowBomDetail(item3)">
              <template v-for="(item4, index4) in item3" :key="index4">
                <td v-if="index4 !== 'ID'">{{ item4 }}</td>
              </template>
            </tr>
          </template>
        </tbody>
      </table>
    </template>


   <!-- <template v-if="DataTableHeader">
          <table class="table mytable_1">
            <thead>
              <tr>
                <th>
                  <template
                    v-for="(item2, index2) in DataTableHeader"
                    :key="index2"
                  >
                    {{ item2 }}
                  </template>
                </th>
              </tr>
              <template v-for="(item3, index3) in DataTable" :key="index3">
                <tr :class="isSelectedRow" @click="ShowBomDetail(item3)">
                  <template v-for="(item4, index4) in item3" :key="index4">
                    <td v-if="index4 != 'ID'">{{ item4 }}</td>
                  </template>
                </tr>
              </template>
            </thead>
          </table>
        </template>  -->

      </div>
      <div class="col-sm-1"></div>
      <div class="main-contain row col-sm-8">
        <div class="div-content">
          <template v-if="DataTableHeaderBOM">
         <table id="tableMain" class="table mytable">
              <thead>
                <tr>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                  </th>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Edit" : "S?a" }}
                  </th>
                  <template
                    v-for="(item, index) in DataTableHeaderBOM"
                    :key="index"
                  >
                    <th v-if="item != 'ID'">
                      {{ item }}
                    </th>
                  </template>
                  
                </tr>
              </thead>
              <template v-for="(item, index) in DataTableBom" :key="index">
                <tr>
                  <td class="td-delete" @click="DeleteRecord(item)">
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
                  <td class="td-edit" @click="ShowDetail(item)">
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

    <div class="div-button">
      <template v-if="add_bom==='add'">
        <button
        class="btn btn-primary"
        type="submit"
        title="add"
        @click="Addnewbom()"
      >
        Add New Bom
      </button>
      </template>
      <button
        class="btn btn-success"
        type="submit"
        @click="SaveData()"
        title="Save"
      >
        Save Data
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

    <div class="div-bellow">
      <template v-if="type == 'add'">
        <div class="form-row">
          <div class="col-md-3 mb-3">
            <label for="validationDefault01">BOM NAME</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              v-model="model.BOM_NAME"
              required
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="validationDefault02">BOM COPY</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filterBom"
              @update-selected-item="UpdateBomReceive"
              :textContent="model.BOM_COPY"
              type="model"
              textPlaceHolder="Enter model name"
            />
          </div>
          
        </div>
      </template>

      <template v-else>
        <div class="form-row">
          <div class="col-md-3 mb-3">
            <label for="validationDefault03">BOM NO</label>
            <input
              type="text"
              class="form-control form-control-sm text-element"
              v-model="model.BOM_NO"
              disabled
              required
            />
          </div>
        </div>

        <div class="form-row">
          <div class="col-md-3 mb-3">
            <label for="validationDefault04">KEY PART NO</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filterKeyPart"
              @update-selected-item="UpdateKeypartReceive"
              :textContent="model.KEY_PART_NO"
              type="model"
              textPlaceHolder="Enter key part no"
            />
          </div>

          <div class="col-md-3 mb-3">
            <label for="validationDefault05">GROUP NAME</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filterGroupName"
              @update-selected-item="UpdateGroupNameReceive"
              :textContent="model.GROUP_NAME"
              type="model"
              textPlaceHolder="Enter group name"/>
          </div>
          

          <template v-if="model.GROUP_NAME === 'CPS'">
          <div class="col-md-3 mb-3">
            <label for="validationDefault0cps">VERSION_CODE</label>
            <DropdownSearch
              class="form-control form-control-sm text-element"
              :listAll="filterOptionLink"
              @update-selected-item="UpdateOptionLinkReceive"
              :textContent="model.OPTION_LINK"
              type="model"
              textPlaceHolder="Enter version code"
            />
            </div>
          </template>
     
          <div class="col-md-3 mb-3">
            <label for="validationDefault06">KP COUNT</label>
            <input
              type="number"
              class="form-control form-control-sm text-element"
              required
              v-model="model.KP_COUNT"
            />
          </div>
        </div>
      </template>
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
//isDropdownVisible: false,
searchQuery: "",

      textContent: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      DataTableHeaderBOM: [],
      DataTableHeader: [],
      DataTable: [],
      DataTableBom: [],
      columnName: [],
      columnNamebom: [],
      valueSearch: "",
      add_bom:"add",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      type: "edit",
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        BOM_NO: "",
        BOM_COPY: "",
        KEY_PART_NO: "",
        KP_RELATION: 0,
        KP_COUNT: 1,
        GROUP_NAME: "",
        OPTION_LINK:"",
        BOM_NAME: "",
      },
      listChecked: [],
      ListRoute: [],
      ListKeyPartNo: [],
      ListBom: [],
      ListGroupName: [],
      ListModelSerial: [],
      ListOptionLink:[],
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

    filteredDataTable() {
      if (!this.searchQuery) {
        return this.DataTable;
      }
      const query = this.searchQuery.toLowerCase();
      return this.DataTable.filter((item) => {
        for (const key in item) {
          if (key !== "ID" && item[key].toString().toLowerCase().includes(query)) {
            return true;
          }
        }
        return false;
      });
    },

    filterBom: function () {
      const query = this.searchText.toLowerCase();
     // if (!query) return this.ListBom;
     if (query.length < 3) return this.ListBom;
      if (this.searchText === "") {
        return this.ListBom;
      }
      return this.ListBom.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },

    filterKeyPart: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListKeyPartNo;
      if (this.searchText === "") {
        return this.ListKeyPartNo;
      }
      return this.ListKeyPartNo.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
    filterGroupName: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListGroupName;
      if (this.searchText === "") {
        return this.ListGroupName;
      }
      return this.ListGroupName.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
    filterOptionLink: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListOptionLink;
      if (this.searchText === "") {
        return this.ListOptionLink;
      }
      return this.ListOptionLink.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },
  },


  mounted() {
    this.CheckPrivilege();
    this.GetBom();
    this.GetKeyPartNo();
    this.GetGroupName();
    //this.checkDisable();
    this.GetOptionLink();
  },
  methods: {

    checkVersionCode() {
      if (this.type === "add") {
      if (this.model.GROUP_NAME === 'CPS' && this.model.OPTION_LINK === '') {
      if (localStorage.language === 'En') {
        this.$swal('', 'Version_Code cannot be empty for Group_Name CPS', 'error');
      } else {
        this.$swal('', 'Version_Code không th? d? tr?ng cho Group_Name CPS', 'error');
      }
      return false;
    }
  }else if (this.type === "edit") {
  if (this.model.GROUP_NAME === 'CPS' && (this.model.OPTION_LINK === '')) {
    if (localStorage.language === 'En') {
      this.$swal('', 'Version_Code cannot be empty for Group_Name CPS', 'error');
    } else {
      this.$swal('', 'Version_Code không th? d? tr?ng cho Group_Name CPS', 'error');
    }
    return false;
  }
}
    return true;
  },

    showDropdownList() {
          this.isDropdownVisible = true;
        },
        hideDropdownList() {
          // Use a timeout to allow selection before hiding the dropdown
          setTimeout(() => {
            this.isDropdownVisible = false;
          }, 100);
        },
        select_GroupName(GroupName) {
          this.model.GROUP_NAME = GroupName;
          this.hideDropdownList();
        },

    UpdateBomReceive(value) {
      this.model.BOM_COPY = value;
    },
    UpdateKeypartReceive(value) {
      this.model.KEY_PART_NO = value;
    },
    UpdateGroupNameReceive(value) {
      this.model.GROUP_NAME = value;
    },
    UpdateOptionLinkReceive(value){
      this.model.OPTION_LINK=value;
    },
    async GetBom() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig12Content", payload);
      data.data.forEach((element) => {
        this.ListBom.push(element.BOM_NO);
      });
    },
    async GetKeyPartNo() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetKeypartContent", payload);
      data.data.forEach((element) => {
        this.ListKeyPartNo.push(element.KEY_PART_NO);
      });
    },
    async GetGroupName() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetGroupConfig7", payload);
      data.data.forEach((element) => {
        this.ListOptionLink.push(element.VALUE);
      });
    },
    async GetOptionLink(){
      var payload={
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetOptionLink", payload);
      data.data.forEach((element) => {
        this.ListGroupName.push(element.GROUP_NAME);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    groupClick() {
      this.GetGroupConfig7("UpdateListSelectConfig7Group", "showConfig7Group");
    },
    async GetGroupConfig7(listType, modalType) {
      var payload = {
        database_name: localStorage.databaseName,
        value: this.textSearch,
      };
      var { data } = await Repository.getRepo("GetGroupConfig7", payload);
      this.$store.commit(listType, data.data);
      this.$store.commit(modalType);
    },

    async SaveData() {
        if (this.type === "add") {
        if (
          this.model.BOM_COPY === "" ||
          this.model.BOM_NAME === ""
        ) {
          if (localStorage.language === "En") {
            this.$swal("", "Empty fields", "error");
          } else {
            this.$swal("", "Không du?c b? tr?ng", "error");
          }
          return;
        }
      }
      else if (this.type === "edit"){
        if (
          this.model.GROUP_NAME ==="CPS" && this.model.OPTION_LINK === "" ||
          this.model.BOM_NO === "" ||
          this.model.KEY_PART_NO === "" ||
          this.model.GROUP_NAME === "" ||
          this.model.KP_COUNT <=0
        ){
          if (localStorage.language === "En"){
            this.$swal("", "Empty fields or invalid KP_COUNT", "error");
          }else{
            this.$swal("", "Không du?c b? tr?ng ho?c KP_COUNT không h?p l?", "error");
          }
          return;
        }
      }

      if (!this.checkVersionCode()) {
      return; 
    }

      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure edit?";
        textValue = "Once OK, data will be updated!";
      } else {
        titleValue = "Ch?c ch?n s?a?";
        textValue = "D? li?u s? du?c c?p nh?t";
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
          ID: this.model.ID,
          EMP: localStorage.username,
          type:this.type,
          BOM_NO: this.model.BOM_NO,
          BOM_NAME: this.model.BOM_NAME,
          BOM_COPY: this.model.BOM_COPY,
          KEY_PART_NO: this.model.KEY_PART_NO,
          KP_COUNT: this.model.KP_COUNT,
          GROUP_NAME: this.model.GROUP_NAME,
          OPTION_LINK:this.model.OPTION_LINK,
        };
        var { data } = await Repository.getRepo(
          "InsertUpdateConfig12",
          payload
        );
        if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "B?n không có quy?n thêm s?a", "error");
          }
        } else if (data.result == "ok") {
          await this.QuerySearch();
          this.ShowBomDetail(payload);

          this.ClearForm1();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "C?p nh?t thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },

    DeleteRecord(item) {
      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure?";
        textValue =
          "Once deleted, you will not be able to recover this record!";
      } else {
        titleValue = "Ch?c ch?n xóa?";
        textValue = "Sau khi xóa s? không th? khôi ph?c!";
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
          ID: item.ID,
          EMP: localStorage.username,
          EMP_NO: item.EMP_NO,
          BOM_NO: item.BOM_NO,
          KEY_PART_NO: item.KEY_PART_NO,
          VERSION_CODE: item.VERSION_CODE,
          GROUP_NAME: item.GROUP_NAME,
          KP_COUNT: item.KP_COUNT,
          KP_RELATION: item.KP_RELATION,
          OPTION_LINK: item.OPTION_LINK,
        };

        var { data } = await Repository.getRepo("DeleteConfig12", payload);
        if (data.result == "ok") {
         await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "C?p nh?t thành công", "success");
          }

          this.ShowBomDetail(payload);
          this.ClearForm();
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "B?n không có quy?n thêm s?a", "error");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },

    Addnewbom() {
      this.ClearForm();
      this.type = "add";
    },
    ClearForm() {
      this.type = "edit";
      this.model.ID = "";
      this.model.BOM_NO = "";
      this.model.BOM_COPY = "";
      this.model.KP_COUNT = 1;
      this.model.BOM_NAME = "";
      this.model.GROUP_NAME = "";
      this.model.KEY_PART_NO = "";
      this.model.OPTION_LINK="";
      this.add_bom="add";
    },
    ClearForm1() {
      this.type = "edit";
      this.model.ID = "";
    //  this.model.BOM_NO = "";
      this.model.BOM_COPY = "";
      this.model.KP_COUNT = 1;
      this.model.BOM_NAME = "";
      this.model.GROUP_NAME = "";
      this.model.KEY_PART_NO = "";
      this.model.OPTION_LINK="";
      this.add_bom="add";
    },
    ShowDetail(detail) {
      this.model.ID = detail.ID;
      this.model.BOM_NO = detail.BOM_NO;
      this.model.KEY_PART_NO = detail.KEY_PART_NO;
      this.model.KP_COUNT = detail.KP_COUNT;
      this.model.GROUP_NAME = detail.GROUP_NAME;
      this.model.OPTION_LINK=detail.VERSION_CODE;
      this.add_bom="";
    },
    async ShowBomDetail(detail) {
      var payload = {
        database_name: localStorage.databaseName,
        BOM_NO: detail.BOM_NO,
      };
      this.model.BOM_NO=detail.BOM_NO;
      var { data } = await Repository.getRepo("GetConfig12Content", payload);
      this.DataTable = [];
      this.DataTableBom = [];
      this.DataTable = data.data;
      this.DataTableBom = data.data1;
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
      if (typeof this.DataTableBom != "undefined") {
        if (this.DataTableBom.length != 0) {
          this.DataTableHeaderBOM = Object.keys(this.DataTableBom[0]);
          this.DataTableHeaderBOM.forEach((element) => {
            this.columnNamebom.push({
              label: element,
              field: element,
            });
          });
        }
      }
    },
    async GetGroupOfEmNo(emp_no) {
      var payload = {
        database_name: localStorage.databaseName,
        EMP_NO: emp_no,
      };
      var { data } = await Repository.getRepo("GetGroupConfig7", payload);
      this.$store.state.listSelectDualGroupConfig7 = data.data;
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "BOM",
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
      var { data } = await Repository.getRepo("GetConfig12Content", payload);
      this.DataTable = [];
      this.DataTableBom = [];
      this.DataTable = data.data;
      this.DataTableBom = data.data1;
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
      if (typeof this.DataTableBom != "undefined") {
        if (this.DataTableBom.length != 0) {
          this.DataTableHeaderBOM = Object.keys(this.DataTableBom[0]);
          this.DataTableHeaderBOM.forEach((element) => {
            this.columnNamebom.push({
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
        BOM_NO: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig12Content", payload);
      this.DataTable = [];
      this.DataTableBom = [];
      this.DataTable = data.data;
      this.DataTableBom = data.data1;
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

      if(this.valueSearch==""){
        this.model.BOM_NO="";
      }else{

        if (this.DataTable && this.DataTable.length > 0) {
            this.model.BOM_NO = this.DataTable[0].BOM_NO;
          } else {
            this.model.BOM_NO = ""; // or set it to an appropriate default value
          }

        //this.model.BOM_NO=this.DataTable && this.DataTable[0].BOM_NO;
       // const value = obj[0];

        // After
        //const value = obj && obj[0];
      }
      if (typeof this.DataTableBom != "undefined") {
        if (this.DataTableBom.length != 0) {
          this.DataTableHeaderBOM = Object.keys(this.DataTableBom[0]);
          this.DataTableHeaderBOM.forEach((element) => {
            this.columnNamebom.push({
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
      // input {
      //   border-radius: 5px;
      // }
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
      padding: 6 20px;
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
      padding: 0.5rem 1rem;
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
      padding: 1px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 13px;
      font-weight: 555;
    }
  }
}
.mytable_1 {
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



input{
  transition: 0.2s;
  width:300px;
  height: 35px;
  margin:0;
  box-sizing:border-box;
  padding:8px;
  //border:1px solid #fff;
  outline:none;
  border-radius:4px;
  //background:#fff;
  color:#000;
  position: relative;
  z-index: 2;
}

input:focus{
  border: 1px solid #db9c2e; 
  box-shadow: 0px 0px 3px 0px #f2f2f2;
  border-radius:4px 4px 0 0;}


.input-datalist {
      position: relative;
      display: inline-block;
      margin-left: 5px;
    }

    .input-datalist input[type="text"] {
      width: 200px;
      padding: 5px;
      border: 1px solid #ccc;

      // border-radius: 4px;
    }

    .input-datalist ul {
      position: absolute;
      top: 100%;
      left: 0;
      z-index: 1;
      width: 100%;
      padding: 0;
      margin: 5px 0;
      list-style: none;
      background-color: #9ff9c8;
      border: 1px solid #ccc;
      border-radius: 4px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
      color: #000;
      max-height: 10rem;
      overflow: auto;
    }

    .input-datalist li {
      padding: 8px 10px;
      cursor: pointer;
    }

    .input-datalist li:hover {
      background-color: #f2f2f2;
    }

    .input-datalist li:last-child {
      border-bottom-left-radius: 4px;
      border-bottom-right-radius: 4px;
    }
</style>