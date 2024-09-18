<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>

      <div class="div-config-name row">
        <span> Config(2):</span>
      </div>
    </div>
    <div class="main-contain">
      <div class="row col-md-12">
        <div class="col-md-4 mb-4">
          <div class="col-md-4">
            <label for="label1">SECTION NAME</label>
          </div>
          <div class="col-md-8">
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filterModel"
              @update-selected-item="UpdateModelNameReceive"
              :textContent="model.SECTION_NAME"
              type="model"
              textPlaceHolder="Enter section name"
            />
          </div>
        </div>
        <div class="col-md-4 mb-4">
          <button
            class="btn btn-success"
            title="ADD SECTION"
            @click="AddSection()"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="#12e412"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M3 3h18v18H3zM12 8v8m-4-4h8" />
            </svg>
          </button>
          <button
            type="submit"
            class="btn btn-primary"
            title="Edit Section"
            @click="EditSection()"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="#12e412"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path
                d="M20 14.66V20a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h5.34"
              ></path>
              <polygon points="18 2 22 6 12 16 8 16 8 12 18 2"></polygon>
            </svg>
          </button>

          <button
            type="submit"
            class="btn btn-success"
            title="Add Group"
            @click="AddGroup()"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              fill="currentColor"
              class="bi bi-node-plus"
              viewBox="0 0 16 16"
            >
              <path
                fill-rule="evenodd"
                d="M11 4a4 4 0 1 0 0 8 4 4 0 0 0 0-8zM6.025 7.5a5 5 0 1 1 0 1H4A1.5 1.5 0 0 1 2.5 10h-1A1.5 1.5 0 0 1 0 8.5v-1A1.5 1.5 0 0 1 1.5 6h1A1.5 1.5 0 0 1 4 7.5h2.025zM11 5a.5.5 0 0 1 .5.5v2h2a.5.5 0 0 1 0 1h-2v2a.5.5 0 0 1-1 0v-2h-2a.5.5 0 0 1 0-1h2v-2A.5.5 0 0 1 11 5zM1.5 7a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5h-1z"
              />
            </svg>
          </button>
          <button
            type="submit"
            class="btn btn-primary"
            title="Edit Group"
            @click="EditGroup()"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
            >
              <g>
                <path fill="none" d="M0 0H24V24H0z" />
                <path
                  d="M10 2c.552 0 1 .448 1 1v4c0 .552-.448 1-1 1H8v2h5V9c0-.552.448-1 1-1h6c.552 0 1 .448 1 1v4c0 .552-.448 1-1 1h-6c-.552 0-1-.448-1-1v-1H8v6h5v-1c0-.552.448-1 1-1h6c.552 0 1 .448 1 1v4c0 .552-.448 1-1 1h-6c-.552 0-1-.448-1-1v-1H7c-.552 0-1-.448-1-1V8H4c-.552 0-1-.448-1-1V3c0-.552.448-1 1-1h6zm9 16h-4v2h4v-2zm0-8h-4v2h4v-2zM9 4H5v2h4V4z"
                />
              </g>
            </svg>
          </button>
        </div>
      </div>
      <div class="row col-md-12">
        <div class="content-left col-md-3 border rounded mt-5 div-content">
          <ul class="tree">
            <li>
              <input type="checkbox" checked="checked" id="c1" />
              <label class="tree_label" for="c1"
                ><h5>{{ model.SECTION_NAME }}</h5></label
              >
              <ul>
                <template v-for="(item, index) in ListType" :key="index">
                  <li @click="ShowDetail(item)">
                    <span class="tree_label">{{ item }}</span>
                  </li>
                </template>
              </ul>
            </li>
          </ul>
        </div>
        <!-- center content -->
        <div class="content-center col-md-5 mt-4">
          <div class="row div-bellow">
            <div class="col-sm-3"></div>
            <div class="col-sm-8">
              <div class="col-sm-12">
                <label for="validationDefault01">SECTION NAME</label>
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.SECTION_NAME_NEW"
                  :readonly="isReadonly1 == false"
                  id="validationDefault01"
                  required
                />
              </div>
              <div class="col-sm-12">
                <label for="valdationDefault02">SECTION NAME DESC</label>
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.SECTION_NAME_DESC"
                  id="valdationDefault02"
                  :readonly="isReadonly1 == false"
                  required
                />
              </div>
              <div class="col-sm-12 div-button" v-show="show">
                <div class="col-sm-3">
                  <button
                    type="submit"
                    class="btn btn-primary"
                    @click="InsertSection()"
                  >
                    OK
                  </button>
                </div>
                <div class="col-sm-3"></div>
                <div class="col-sm-3">
                  <button
                    type="submit"
                    class="btn btn-warning"
                    @click="CloseSection()"
                  >
                    Cancel
                  </button>
                </div>
              </div>
              <div class="col-md-12">
                <label for="valdationDefault03">GROUP NAME</label>
                <input
                  type="text"
                  class="form-control form-control-sm text-element"
                  v-model="model.GROUP_NAME_NEW"
                  id="valdationDefault03"
                  :readonly="isReadonly == false"
                  required
                />
              </div>
              <div class="col-md-12">
                <label for="valdationDefault04">GROUP DESCRIPTION</label>
                <input
                  type="text"
                  id="valdationDefault04"
                  class="form-control form-control-sm text-element"
                  v-model="model.GROUP_DESCRIPTION"
                  :readonly="isReadonly == false"
                  required
                />
              </div>
              <div class="col-md-12 div-button" v-show="showgroup">
                <div class="col-sm-3">
                  <button
                    type="submit"
                    class="btn btn-primary"
                    @click="InsertGroup()"
                  >
                    OK
                  </button>
                </div>
                <div class="col-sm-3"></div>
                <div class="col-sm-3">
                  <button
                    type="submit"
                    class="btn btn-warning"
                    @click="CloseGroup()"
                  >
                    Cancel
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
export default {
  components: {
    DropdownSearch,
  },
  data() {
    return {
      selectedItem: null,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      DatatableFA: [],
      DataTableFAHeader: [],
      columnName: [],
      columnFAName: [],
      status: "",
      show: false,
      showgroup: false,
      isReadonly: false,
      isReadonly1: false,
      model: {
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        SECTION_NAME: "",
        SECTION_NAME_NEW: "",
        GROUP_NAME: "",
        GROUP_NAME_NEW:"",
        SECTION_NAME_DESC: "",
        GROUP_DESCRIPTION: "",
        POST_FILE: "",
        ACTION_TYPE: "INSERT",
      },
      ListModel: [],
      ListType: [],

      searchText: "",
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
  },
  mounted() {
    this.CheckPrivilege();
    //this.CheckPrivilege_();

    this.GetSectionName();
  },
  methods: {
    UpdateModelNameReceive(value) {
      this.model.SECTION_NAME = value;
      this.model.SECTION_NAME_NEW = this.model.SECTION_NAME;
      this.GetGroupName();
      this.ShowDetailSection(this.model.SECTION_NAME);
    },
    async GetSectionName() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetSectionNameConfig2", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.SECTION_NAME);
      });
    },
    async GetGroupName() {
      var payload = {
        database_name: localStorage.databaseName,
        SECTION_NAME: this.model.SECTION_NAME,
      };
      this.ListType = [];
      var { data } = await Repository.getRepo("GetTreeViewConfig2", payload);
      data.data.forEach((element) => {
        this.ListType.push(element.GROUP_NAME);
      });
      //console.log(this.ListType);
    },
    AddSection() {
      this.show = true;
      this.isReadonly1 = true;
      this.model.SECTION_NAME_NEW = "";
      this.model.SECTION_NAME_DESC = "";
      this.model.ACTION_TYPE = "INSERT_SECTION";
    },
    EditSection() {
      if (this.model.SECTION_NAME != "") {
        this.show = true;
        this.isReadonly1 = true;
        this.model.ACTION_TYPE = "UPDATE_SECTION";
      }
    },
    AddGroup() {
      if (this.model.SECTION_NAME != "") {
        this.showgroup = true;
        this.isReadonly = true;
        this.model.GROUP_NAME = "";
        this.model.GROUP_DESCRIPTION = "";
        this.model.ACTION_TYPE = "INSERT_GROUP";
        this.model.GROUP_NAME_NEW="";
      }
    },
    EditGroup() {
      if (this.model.GROUP_NAME != "") {
        this.showgroup = true;
        this.isReadonly = true;
        this.model.ACTION_TYPE = "UPDATE_GROUP";
      }
    },
    CloseSection() {
      this.show = false;
      this.isReadonly1 = false;
    },
    CloseGroup() {
      this.showgroup = false;
      this.isReadonly = false;
    },
    async InsertSection() {
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
        //begin
        var payload = {
          database_name: localStorage.databaseName,
          EMP:localStorage.username,
          SECTION_NAME: this.model.SECTION_NAME,
          ACTION: this.model.ACTION_TYPE,
          SECTION_NAME_NEW: this.model.SECTION_NAME_NEW,
          SECTION_NAME_DESC: this.model.SECTION_NAME_DESC,
        };

        var { data } = await Repository.getRepo("InsertUpdateConfig2", payload);
        if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege insert update", "error");
          } else {
            this.$swal("", "Bạn không có quyền thêm sửa", "error");
          }
        } else if (data.result == "ok") {
          await this.GetSectionName();

          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
        //end
      });
    },
    async InsertGroup(){
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
        //begin
          var payload ;
        if (this.model.ACTION_TYPE =="INSERT_GROUP")
        {
        payload = {
                  database_name: localStorage.databaseName,
                  EMP:localStorage.username,
                  SECTION_NAME: this.model.SECTION_NAME,
                  ACTION: this.model.ACTION_TYPE,
                  GROUP_NAME_NEW: this.model.GROUP_NAME_NEW,
                  GROUP_NAME:this.model.GROUP_NAME_NEW,
                  GROUP_NAME_DESC:this.model.GROUP_DESCRIPTION,
         }
        }
        else
        {
             payload = {
                  database_name: localStorage.databaseName,
                  EMP:localStorage.username,
                  SECTION_NAME: this.model.SECTION_NAME,
                  ACTION: this.model.ACTION_TYPE,
                  GROUP_NAME_NEW: this.model.GROUP_NAME_NEW,
                  GROUP_NAME:this.model.GROUP_NAME,
                  GROUP_NAME_DESC:this.model.GROUP_DESCRIPTION,
             }
        }
        
       

        var { data } = await Repository.getRepo("InsertUpdateConfig2", payload);
        if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege insert update", "error");
          } else {
            this.$swal("", "Bạn không có quyền thêm sửa", "error");
          }
        } else if (data.result == "ok") {
          await this.GetSectionName();

          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
        //end
      });
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "SECTION/GROUP",
        // prg_name :"CONFIG"
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        this.$router.push({ path: "/Home/ConfigApp" });
      }
    },
    async ShowDetail(item) {
      this.show = false;
      this.isReadonly1 = false;
      this.showgroup = false;
      this.isReadonly = false;
      var payload = {
        database_name: localStorage.databaseName,
        GROUP_NAME: item,
        SECTION_NAME: this.model.SECTION_NAME,
        ACTION: "DETAIL",
      };

      var { data } = await Repository.getRepo("GetTreeViewConfig2", payload);
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
      this.model.GROUP_NAME = this.DataTable[0].GROUP_NAME;
      this.model.GROUP_DESCRIPTION = this.DataTable[0].GROUP_DESC;
      this.model.GROUP_NAME_NEW=this.model.GROUP_NAME;
    },
    async ShowDetailSection(str_section_name) {
      this.show = false;
      this.isReadonly1 = false;
      this.showgroup = false;
      this.isReadonly = false;
      var payload = {
        database_name: localStorage.databaseName,
        SECTION_NAME: str_section_name,
        ACTION: "DETAIL",
      };
      var { data } = await Repository.getRepo("GetSectionNameConfig2", payload);
      this.DatatableFA = [];
      this.DatatableFA = data.data;
      if (typeof this.DatatableFA != "undefined") {
        if (this.DatatableFA.length != 0) {
          this.DataTableHeader = Object.keys(this.DatatableFA[0]);
          this.DataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
      }
      this.model.SECTION_NAME_DESC = this.DatatableFA[0].SECTION_DESC;
      this.model.GROUP_NAME = "";
      this.model.GROUP_DESCRIPTION = "";
      this.model.GROUP_NAME_NEW="";
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
//tree view

.div-content {
  height: 350px;
  overflow: auto;
  ul {
    list-style: none;
    padding-inline-start: 0;
    padding: -1 0 0 0;
  }
}
.tree {
  margin: 1em;
}
.tree input {
  position: absolute;
  clip: rect(0, 0, 0, 0);
}
.tree input ~ ul {
  display: none;
}

.tree input:checked ~ ul {
  display: block;
}

/* ————————————————————–
  Tree rows
*/
.tree li {
  line-height: 1.2;
  position: relative;
  padding: 0 0 1em 1em;
}

.tree ul li {
  padding: 1em 0 0 1em;
}

.tree > li:last-child {
  padding-bottom: 0;
}

/* ————————————————————–
  Tree labels
*/
.tree_label {
  position: relative;
  display: inline-block;
  background: #fff;
}

label.tree_label {
  cursor: pointer;
  margin: 0;
}

label.tree_label:hover {
  color: #666;
}

/* ————————————————————–
  Tree expanded icon
*/
label.tree_label:before {
  background: #000;
  color: #fff;
  position: relative;
  z-index: 1;
  float: left;
  margin: 0 1em 0 -2em;
  width: 1em;
  height: 1em;
  border-radius: 1em;
  content: "+";
  text-align: center;
  line-height: 0.9em;
}

:checked ~ label.tree_label:before {
  content: "–";
}

/* ————————————————————–
  Tree branches
*/
.tree li:before {
  position: absolute;
  top: 0;
  bottom: 0;
  left: -0.55em;
  display: block;
  width: 0;
  border-left: 1px solid #777;
  content: "";
}

.tree_label:after {
  position: absolute;
  top: 0;
  left: -1.55em;
  display: block;
  height: 0.5em;
  width: 1.1em;
  border-bottom: 1px solid #777;
  border-left: 1px solid #777;
  border-radius: 0 0 0 0.3em;
  content: "";
}

label.tree_label:after {
  border-bottom: 0;
}

:checked ~ label.tree_label:after {
  border-radius: 0 0.3em 0 0;
  border-top: 1px solid #777;
  border-right: 1px solid #777;
  border-bottom: 0;
  border-left: 0;
  bottom: 0;
  top: 0.5em;
  height: auto;
}
.tree_label {
  cursor: pointer;
}
.tree li:last-child:before {
  height: 1em;
  bottom: auto;
}

.tree > li:last-child:before {
  display: none;
}

.tree_custom {
  display: block;
  background: #eee;
  padding: 1em;
  border-radius: 0.3em;
}
//end tree view
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
  //right: 50px;
  //text-align: right;
}
.div-bellow {
  margin-top: 5px;

  color: #fff;
  padding: 15px;
  margin-right: 20px;
  div {
    div {
      label {
        font-size: 13px;
        font-weight: bold;
        color: #080808;
      }
      input {
        border-radius: 5px;
        margin-bottom: 5px;
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

.active .trucksicons {
  border: 2px solid green;
}
.div-button1 {
  margin-left: 50px;
}
.lb_checked {
  color: #efef8d !important;
}
</style>