<template>
    <div class="div-all">
        <div class="row">
            <div class="div-back" @click="BackToParent()">
                <Icon icon="chevron-left" class="back-icon sidenav-icon" />
            </div>
            <div class="div-config-name row">
                <span>Config Employee (7):</span>
            </div>
        </div>
      <div class="menu">
        <router-link 
          to="/Home/ConfigApp/Config7" 
          id="Config7"
          :class = "{ 'hovered' : isHovered }"
          >
          EMPLOYEE
        </router-link>

        <router-link 
          to="/Home/ConfigApp/Privilege" 
          id="Privilege"
          @mouseover = "isHovered = true"
          @mouseleave = "isHovered = false"
          >
          PRIVILEGE
        </router-link>

        <router-link 
          to="/Home/ConfigApp/Ams_Privilege" 
          id="Ams_Privilege"
          @mouseover = "isHovered = true"
          @mouseleave = "isHovered = false"
          >
          AMS_PRIVILEGE
        </router-link>
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
                    ? 'Enter emp no...'
                    : 'Nhập mã thẻ...'
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
                        <th style="width: 1px">
                        {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                        </th>
                        <th style="width: 1px">
                        {{ $store.state.language == "En" ? "Edit" : "Sửa" }}
                        </th>
                        <template v-for="(item, index) in DataTableHeader" :key="index">
                        <th v-if="item == 'EMP_NO' || item=='EMP_NAME' || item=='EMP_NAME' || item=='CLASS_NAME'">
                            {{ item }}
                        </th>
                        </template>
                    </tr>
                    </thead>
                    <template v-for="(item, index) in DataTable" :key="index">
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
                        <td v-if="index1 == 'EMP_NO' || index1=='EMP_NAME' || index1=='EMP_NAME' || index1=='CLASS_NAME' ">{{ item1 }}</td>
                        </template>
                    </tr>
                    </template>
                </table>
                </template>
            </div>
        </div>
        <div class="div-button">
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
        <div >
            <div class="col-md-9">
                <div class="div-bellow">
                    <div class="form-row">
                        <div class="col-md-4 mb-4">
                            <label for="validationDefault01">EMP NO</label>
                            <input type="text"
                                class="form-control form-control-sm text-element"
                                id="validationDefault01"
                                v-model="model.EMP_NO"
                                required
                            >
                        </div>
                        <div class="col-md-4 mb-4">
                            <label for="validationDefault02">EMP NAME</label>
                            <input type="text"
                                class="form-control form-control-sm text-element"
                                id="validationDefault02"
                                v-model="model.EMP_NAME"
                                required
                            >
                        </div>
                        <div class="col-md-4 mb-4">
                            <label for="validationDefault03">CLASS_NAME</label>
                            <input type="text"
                                class="form-control form-control-sm text-element"
                                id="validationDefault03"
                                v-model="model.CLASS_NAME"
                                required
                            >
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-4 mb-4">
                                <label for="validationDefault04">PASSWORD</label>
                                <input type="password"
                                    class="form-control form-control-sm text-element"
                                    id="validationDefault04"
                                    v-model="model.EMP_PASS"
                                    required
                                >
                        </div>
                        <div class="col-md-4 mb-4">
                            <label for="validationDefault05">Quit Date</label>
                            <datepicker
                                class="form-control form-control-sm"
                                v-model="dateFrom"
                            />
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-3">
                <div class="column">
                    <button @click="groupClick()" class="col btn_select_item">
                        Group
                    </button>
                    <div class="div_select_item">
                        <ul>
                            <smooth-scrollbar>
                                <template v-if="$store.state.listSelectDualModel.length>0">
                                    <li
                                    class="li_selected"
                                    v-for="(item, index) in $store.state.listSelectDualModel"
                                    :key="index"
                                    >
                                    {{ item.VALUE }}
                                    </li>
                                </template>
                            </smooth-scrollbar>
                        </ul>
                    </div>
                </div>
            </div>
        
        </div>
    </div>
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      isHovered: false,
      textContent: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      dateFrom: new Date(),
      DataTableHeader: [],
      DataTable: [],
      columnName: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        EMP_NO:"",
        EMP_NAME:"",
        EMP_RANK:"",
        CLASS_NAME:"",
        QUIT_DATE:"",
        EMP_PASS:"",
      },
    };
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.isVisible = false;
      }
    });
  },
 // computed: {},
  mounted() {
    this.CheckPrivilege();
  },
  methods: {
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    groupClick() {
      this.GetGroupConfig7("UpdateListSelectModel", "showModalModel");
    },
    async GetGroupConfig7( listType, modalType) {
      
      let payload = {
        database_name: localStorage.databaseName,
        value: this.textSearch,
      };
      let { data } = await Repository.getRepo("GetGroupConfig7", payload);
      this.$store.commit(listType, data.data);
      this.$store.commit(modalType);
    },
    async SaveData() {
      
      if(this.model.EMP_PASS.length<8){
          this.$swal("", "Password phải lớn hơn 8 ký tự\nPassword  ", "error");
          return;
        }

      if (
        this.model.EMP_NO == "" ||
        this.model.EMP_NAME == "" 
      ) {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      } else {   
        let titleValue = "";
        let textValue = "";
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
          let payload = {
            database_name: localStorage.databaseName,
            ID: this.model.ID,
            EMP: localStorage.username,
            EMP_NO: this.model.EMP_NO,
            EMP_NAME: this.model.EMP_NAME,
            EMP_PASS:this.model.EMP_PASS,
            CLASS_NAME:this.model.CLASS_NAME,
            QuitDate:this.dateFrom,
            listGroup:this.$store.state.listSelectDualModel,
          };
          console.log("payload: ", payload);
          
          let { data } = await Repository.getRepo("InsertOrUpdateConfig7", payload);
          if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else if (data.result == "ok") {
            await this.LoadComponent();
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
      }
    },
    DeleteRecord(item) {
      let titleValue = "";
      let textValue = "";
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
        let payload = {
          database_name: localStorage.databaseName,
          ID: item.ID,
          EMP: localStorage.username,
          EMP_NO: item.EMP_NO,
        };
        let { data } = await Repository.getRepo("DeleteConfig7", payload);
        if (data.result == "ok") {
          await this.LoadComponent();
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
      this.model.ID = "";
      this.model.EMP_NO="";
      this.model.EMP_NAME="";
      this.model.EMP_PASS="";
      this.model.CLASS_NAME="";
      this.dateFrom=new Date();
      this.$store.state.listSelectDualModel=[];
      this.valueSearch="";
    },
    ShowDetail(detail) {
      console.log("detail: ", detail);
      this.ClearForm();
      this.model.ID = detail.ID;
      this.model.EMP_NO=detail.EMP_NO;
      this.model.EMP_NAME=detail.EMP_NAME;
      this.model.EMP_PASS=detail.EMP_PASS;
      this.model.CLASS_NAME=detail.CLASS_NAME;
      this.dateFrom=new Date(detail.QUIT_DATE);
        this.GetGroupOfEmNo(this.model.EMP_NO);
      
    },
    async GetGroupOfEmNo(emp_no){
        let payload={
            database_name: localStorage.databaseName,
            EMP_NO:emp_no,
        }
        let{data}=await Repository.getRepo("GetGroupConfig7",payload);
        
        this.$store.state.listSelectDualModel=data.data;
        if(typeof this.$store.state.listSelectDualModel=="undefined"){
          this.$store.state.listSelectDualModel=[];
        }
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      let payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "EMPLOYEE",
      };
      let { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        this.$router.push({ path: "/Home/ConfigApp" });
      } else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      this.$store.state.listSelectDualModel=[];
      let payload = {
        database_name: localStorage.databaseName,
      };
      let { data } = await Repository.getRepo("GetConfig7", payload);
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
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO: this.valueSearch,
      };
      let { data } = await Repository.getRepo("GetConfig7", payload);
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
      input{
        border-radius:5px;
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
      margin-left: 10px;
      width: 250px;
    }
    button {
      border-radius: 0 10px 10px 0;
      padding: 6px 20px;
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

.tabcontent {
  padding: 6px 50px;
  border-top: none;
  border-left: 1px solid #ccc;
  color: rgb(0, 0, 0);
  cursor: pointer;
}

.menu {
  display: grid;
  grid-template-columns: 150px 150px 150px;
  grid-template-rows: 50px;
  overflow: hidden;
  text-align: center;
  cursor: pointer;
  font-weight: 555;
  width: 450px;
  border: 1px solid rgb(82, 78, 78);
  border-radius: 5px;
  font-size: 18px;
  #Config7 {
    color: #333;
    grid-column: 1 / 2;
    background-color: #9c9c9c;
    transition: 0.3s;
    padding-top: 10px;
   }
  #Config7.hovered {
    background-color: #fff;
  }
  #Privilege {
    color: #333;
    grid-column: 2 / 3;
    background-color: #fff;
    transition: 0.3s;
    padding-top: 10px;
    &:hover {
    color: #fff;
    background-color: #9c9c9c;
    }
  }
  #Ams_Privilege {
    color: #333;
    grid-column: 3 / 4;
    background-color: #fff;
    transition: 0.3s;
    padding-top: 10px;
    &:hover {
    color: #fff;
    background-color: #9c9c9c;
    }
  }
}
</style>