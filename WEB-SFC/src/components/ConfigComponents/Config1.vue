<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row"><span>Config Line:</span></div>
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
                <th style="width: 4px"></th>
                <template v-for="(item, index) in DataTableHeader" :key="index">
                  <th>
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <template v-for="(item, index) in DataTable" :key="index">
              <tr :class="isSelectedRow" @dblclick="ShowDetail(item)">
                <td class="td-checkbox">
                  <input type="checkbox" v-model="listChecked" :value="item" />
                </td>
                <template v-for="(item1, index1) in item" :key="index1">
                  <td>{{ item1 }}</td>
                </template>
              </tr>
            </template>
          </table>
        </template>
      </div>
    </div>
    <div class="div-bellow">
      <div class="form-row">
        <div class="col-md-4 mb-3">
          <label for="validationDefault01">Line name</label>
          <input
            type="text"
            class="form-control"
            id="validationDefault01"
            placeholder="Line name"
            v-model="line_name"
            required
          />
        </div>
        <div class="col-md-4 mb-3">
          <label for="validationDefault02">Line type</label>
          <input
            type="text"
            class="form-control"
            id="validationDefault02"
            placeholder="Line type"
            v-model="line_type"
            required
          />
        </div>
        <div class="col-md-4 mb-3">
          <label for="validationDefault02">Line code</label>
          <input
            type="text"
            class="form-control"
            id="validationDefault02"
            placeholder="Line code"
            v-model="line_code"
            required
          />
        </div>
      </div>
      <div class="form-row">
        <div class="col-md-6 mb-3">
          <label for="validationDefault03">Line description</label>
          <input
            type="text"
            class="form-control"
            id="validationDefault03"
            v-model="line_desc"
            placeholder="Line description"
            required
          />
        </div>
      </div>
      <button class="btn btn-success" type="submit" @click="SaveData()">
        Apply
      </button>
      <button class="btn btn-warning" type="button" @click="ClearForm()">
        Clear
      </button>
      <button class="btn btn-danger" type="button" @click="DeleteRecord()">
        Delete
      </button>
    </div>
  </div>
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      DataTableHeader: [],
      DataTable: [],
      columnName: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      listChecked: [],
    };
  },
  methods: {
    async SaveData() {
      if (
        this.line_name == "" ||
        this.line_type == "" ||
        this.line_code == "" ||
        this.line_desc == ""
      ) {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      } else {
        var payload = {
          database_name: localStorage.databaseName,
          line_name: this.line_name,
          line_type: this.line_type,
          line_code: this.line_code,
          line_desc: this.line_desc,
        };
        var { data } = await Repository.getRepo("InsertUpdateConfig1", payload);
        if (data.result == "ok") {
          this.ClearForm();
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      }
    },
    DeleteRecord() {
      if (typeof this.listChecked == "undefined") return;
      if (this.listChecked.length == 0) return;
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
        showCloseButton: true,
        showCancelButton: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        console.log(willDelete.isConfirmed);
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          EMP: localStorage.username,
          listLine: this.listChecked,
        };
        var { data } = await Repository.getRepo("DeleteConfig1", payload);
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
      this.line_name = "";
      this.line_type = "";
      this.line_code = "";
      this.line_desc = "";
    },
    ShowDetail(detail) {
      this.line_name = detail.LINE_NAME;
      this.line_type = detail.LINE_TYPE;
      this.line_code = detail.LINE_CODE;
      this.line_desc = detail.LINE_DESC;
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "LINE",
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
      var { data } = await Repository.getRepo("GetConfig1Content", payload);
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
        fun: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig1Content", payload);
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
  mounted() {
    this.CheckPrivilege();
  },
};
</script>

<style lang="scss" scoped>
.td-checkbox {
  display: flex;
  justify-content: center;
  background: #89cfed;
  height: 25px;
}
.btn-warning {
  margin-left: 20px;
}
.btn-danger {
  margin-left: 20px;
}
.div-bellow {
  margin-top: 20px;
  background: #1c87b5;
  color: #fff;
  padding: 15px;
  margin-right: 20px;
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
  height: 500px;
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