<template>
    <div class="div-all">
        <div class="row">
            <div class="div-back" @click="BackToParent()">
                <Icon icon="chevron-left" class="back-icon sidenav-icon" />
            </div>
            <div class="div-config-name row">
                <span>Privilege</span>
            </div>
        </div>
      <div class="menu">
          <router-link to="/Home/ConfigApp/Config7" id="Config7" class="tabcontent">EMPLOYEE</router-link>
          <router-link to="/" id="Privilege" class="tabcontent">PRIVILEGE</router-link>
       </div>
       <div class="seacrch-emp">
            <div class="input-datalist">
                <input type="text" 
                v-model.trim="model.EMP_NO_NAME" 
                placeholder="Enter name"
                autocomplete="off"
                list="datalistOptions"
                id="datalistEmp"
                @input="filterList"
                />
              </div>
                <datalist id="datalistOptions">
                    <option v-for="(option, index) in limitedOptions" :key="index">
                        {{ option.EMP_NO_NAME }}
                    </option>
                </datalist>
        </div>
        <div class="aplication">
          <div class="not-defineApp">
            <label for="">Not Define Application</label>
            <select v-model="selected">
              <option v-for="(option, index) in ListNotDefineApp" :value="option.value" :key ="index">
              {{ option.PRG_NAME }}
              </option>
            </select>
          </div>
          <div class="defineApp">
            <label for="">Define Application</label>
            <select v-model="selected">
              <option v-for="(option, index) in ListDefineApp" :value="option.value" :key ="index">
              {{ option.PRG_NAME }}
              </option>
            </select>
          </div>
        </div>
        <!-- table All -->
        <div class="tableAll">
          <!-- table1 -->
          <div class="tableNotDefineApp">
                <template v-if="ListNotDefineAppHeader">
                <table>
                    <thead>
                    <tr>
                        <template v-for="(item, index) in ListNotDefineAppHeader" :key="index">
                        <th v-if="item == 'PRG_NAME'">
                            {{ item }}
                        </th>
                        </template>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(item, index) in ListNotDefineApp" :key="index" @click="toggleSelect(item)" :class="{ 'selected': item.isSelected }">
                        <template v-for="(item1, index1) in item" :key="index1">
                        <td v-if="index1 == 'PRG_NAME'">
                          {{ item1 }}
                        </td>
                        </template>
                    </tr>
                    </tbody>
                </table>
                </template>
            </div>

            <button class="notDefineButton" @click="moveSelectedItemsToDefinedApp">
              <svg class="svg-inline--fa fa-angle-double-right fa-w-14" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="angle-double-right" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-v-6887bfd2="">
                <path class="" fill="currentColor" d="M224.3 273l-136 136c-9.4 9.4-24.6 9.4-33.9 0l-22.6-22.6c-9.4-9.4-9.4-24.6 0-33.9l96.4-96.4-96.4-96.4c-9.4-9.4-9.4-24.6 0-33.9L54.3 103c9.4-9.4 24.6-9.4 33.9 0l136 136c9.5 9.4 9.5 24.6.1 34zm192-34l-136-136c-9.4-9.4-24.6-9.4-33.9 0l-22.6 22.6c-9.4 9.4-9.4 24.6 0 33.9l96.4 96.4-96.4 96.4c-9.4 9.4-9.4 24.6 0 33.9l22.6 22.6c9.4 9.4 24.6 9.4 33.9 0l136-136c9.4-9.2 9.4-24.4 0-33.8z">
              </path>
            </svg>
            </button>

            <button class="defineButton" @click="moveSelectedItemsToNotDefinedApp">
              <svg class="svg-inline--fa fa-angle-double-left fa-w-14" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="angle-double-left" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-v-6887bfd2="">
                <path class="" fill="currentColor" d="M223.7 239l136-136c9.4-9.4 24.6-9.4 33.9 0l22.6 22.6c9.4 9.4 9.4 24.6 0 33.9L319.9 256l96.4 96.4c9.4 9.4 9.4 24.6 0 33.9L393.7 409c-9.4 9.4-24.6 9.4-33.9 0l-136-136c-9.5-9.4-9.5-24.6-.1-34zm-192 34l136 136c9.4 9.4 24.6 9.4 33.9 0l22.6-22.6c9.4-9.4 9.4-24.6 0-33.9L127.9 256l96.4-96.4c9.4-9.4 9.4-24.6 0-33.9L201.7 103c-9.4-9.4-24.6-9.4-33.9 0l-136 136c-9.5 9.4-9.5 24.6-.1 34z">
                </path>
            </svg>
            </button>
            <!-- table2 -->
            <div class="tableDefineApp">
                <template v-if="ListDefineAppHeader">
                <table>
                    <thead>
                    <tr>
                        <template v-for="(item, index) in ListDefineAppHeader" :key="index">
                        <th v-if="item == 'PRG_NAME' || item=='PRIVILEGE' || item=='PASSW'">
                            {{ item }}
                        </th>
                        </template>
                    </tr>
                    </thead>
                    <tbody >
                    <tr v-for="(item, index) in ListDefineApp" :key="index" @click="toggleSelect(item)" :class="{ 'selected': item.isSelected }">
                        <template v-for="(item1, index1) in item" :key="index1">
                        <td v-if="index1 == 'PRG_NAME' || index1=='PRIVILEGE' || index1=='PASSW' ">
                          {{ item1 }}
                        </td>
                        </template>
                    </tr>
                    </tbody>
                </table>
                </template>
            </div>
        </div> 

       </div>
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      isDropdownVisible: false,
      textContent: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      ListEmp: [],
      filteredOptions: [],
      ListNotDefineApp: [],
      ListNotDefineAppHeader: [],
      ListDefineApp: [],
      ListDefineAppHeader: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP_NO: localStorage.username,
        EMP_NO_NAME:"",
        PASSW:"",
        FUN:"",
        PRIVILEGE:"",
        PRG_NAME:"",
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
  computed: {
    limitedOptions() {
            return this.filteredOptions.slice(0, 4);
        },
  },

  mounted() {
    this.GetEmp();
    this.notDefineApp();
    this.defineApp();
  },
  methods: {
    moveSelectedItemsToDefinedApp() {
  this.ListNotDefineApp.forEach(item => {
    if (item.isSelected) {
      this.ListDefineApp.push(item);
    }
  });
  this.ListNotDefineApp = this.ListNotDefineApp.filter(item => !item.isSelected);
},
moveSelectedItemsToNotDefinedApp() {
  this.ListDefineApp.forEach(item => {
    if (item.isSelected) {
      this.ListNotDefineApp.push(item);
    }
  });
  this.ListDefineApp = this.ListDefineApp.filter(item => !item.isSelected);
},

toggleSelect(item) {
  item.isSelected = !item.isSelected;
},

    async GetEmp() {
      let payload = {
        database_name: localStorage.databaseName,
      };
      let { data } = await Repository.getRepo("getEmpNo", payload);
      this.ListEmp = data.data;
        },
      async filterList(){
      // debugger;
      const self = this;
      this.filteredOptions = this.ListEmp.filter(function(option){
        return option.EMP_NO_NAME.toLowerCase().includes(self.model.EMP_NO_NAME.toLowerCase());
      });
     console.log("filteredOptions: ", this.filteredOptions, "listEMP: ",this.ListEmp);
    },
    async notDefineApp(){
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO: this.model.EMP_NO,
      };
      try{
        let { data } = await Repository.getRepo("getNotDefineApp", payload);
        this.ListNotDefineApp = data.data;
        if(this.ListNotDefineApp.length > 0){
          this.ListNotDefineAppHeader = Object.keys(this.ListNotDefineApp[0]);
        }
      }catch(error){
        console.error("data: ", error);
      }
    },
    async defineApp(){
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO: this.model.EMP_NO,
      };
      try{
        let { data } = await Repository.getRepo("getDefineApp", payload);
        this.ListDefineApp = data.data;
      if(this.ListDefineApp.length > 0 ){
        this.ListDefineAppHeader = Object.keys(this.ListDefineApp[0]);
      }
      }catch(error){
        console.error("data: ", error);
      }

    },
    // test

    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
      },

};
</script>
<style lang="scss" scoped>
.div-all {
  margin-left: 35px;
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
.tabcontent {
  padding: 6px 50px;
  border-top: none;
  border-left: 1px solid #ccc;
  color: rgb(0, 0, 0);
  cursor: pointer;
}
.menu {
  overflow: hidden;
  border: 1px solid #ccc;
  background-color: #f1f1f1;
  padding: 10px;
  width: 25%;
  font-size: 16px;
}
input{
    height: 30px;
    width: 250px;
}
.aplication{
 display: block;
 .not-defineApp{
  display: inline;
  padding-right: 50px;
 }
 .defineApp{
  display: inline;
  padding-left: 50px;
 }
}
.tableAll{
  max-height: 250px;
  max-width: auto;
  overflow: auto;
}
.tableNotDefineApp{
  display: inline-block;
  overflow-y: auto;
  // padding-right: 50px;
  max-height: 250px;
  max-width: 350px;
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
      overflow-y: auto;
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
.tableDefineApp {
  margin-top: 0px;
  display: inline-block;
  overflow-y: auto;
  max-height: 250px;
  max-width: auto;
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
      overflow-y: auto;
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
.notDefineButton{
  display: inline-block;
}
.defineButton{
  display: inline-block;
}
.selected {
  background-color: #ccc;
}
</style>