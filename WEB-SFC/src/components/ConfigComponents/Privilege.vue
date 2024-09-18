<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>PRIVILEGE</span>
      </div>
    </div>
    <div class="menu">
      <router-link to="/Home/ConfigApp/Config7" id="Config7">EMPLOYEE</router-link>
      <router-link to="/Home/ConfigApp/Privilege" id="Privilege">PRIVILEGE</router-link>
    </div>
    <!-- grid-container -->
    <div id="grid-container">
      <template v-if="value === true">
        <div class="emp-no-name">
          <div class="input-Emp">
            <input type="text"
              v-model.trim="model.EMP_NO_NAME"
              placeholder="Enter emp no..."
              autocomplete="off"
              list="datalistOptions"
              id="datalistEmp"
              @input="filterEmp"
              @change="onEmpChange"
            />
          </div>
          <datalist id="datalistOptions">
            <option v-for="(option, index) in limitedOptions" :key="index">
              {{ option.EMP_NO_NAME }}
            </option>
          </datalist>
        </div>
      </template>
      <!-- Check-box -->
      <template v-else>
        <!-- Copy NotDf -->
        <div class="copy-Emp-NotDf">
          <label for="datalistEmpCopyNotDf">Emp not define</label>
          <input type="text"
            v-model.trim="model.EMP_COPYNOTDF"
            placeholder="Emp not define..."
            autocomplete="off"
            list="datalistOptionsCopyNotDf"
            id="datalistEmpCopyNotDf"
            @input="filterListEmpCopyNotDf"
            @change="onEmpChangeCopyNotDf"
          />
        </div>
        <datalist id="datalistOptionsCopyNotDf">
          <option v-for="(option, index) in limitedOptionsCopyNotDf" :key="index">
            {{ option.EMP_COPYNOTDF }}
          </option>
        </datalist>

        <!-- Check list -->
        <div class="inputList">
          <label for="inputList">Input list</label>
          <input type="checkbox" id="inputList" v-model="valueInput" @click="ClearDefApp">
        </div>
        <!-- Copy Df -->
         <template v-if="valueInput === false">
          <div class="copy-EmpDf">
            <label for="datalistEmpCopyDf">Emp define</label>
            <input type="text"
              v-model.trim="model.EMP_COPYDF"
              placeholder="Emp define..."
              autocomplete="off"
              list="datalistOptionsCopyDf"
              id="datalistEmpCopyDf"
              @input="filterListEmpCopyDf"
              @change="onEmpChangeCopyDf"
            />
          </div>
          <datalist id="datalistOptionsCopyDf">
            <option v-for="(option, index) in limitedOptionsCopyDf" :key="index">
              {{ option.EMP_COPYDF }}
            </option>
          </datalist>
         </template>
         <template v-else>
          <div class="copy-EmpDf">
            <p style="display: inline-block;">Multiline EMP</p>
            <button @click="onEmpChangeMultiple" type="submit">add</button>
            <textarea
              style="display: inline-block;" 
              v-model="LISTINPUTEMP"
              placeholder="add multiple emp"
              >
            </textarea>
          </div>
         </template>
      </template>
      <!-- check-box -->
      <div class="copyEmp">
        <label for="copyEmp">All privilege</label>
        <input type="checkbox" id="copyEmp" v-model="value" @click="clearTable()">
      </div>
      <!--select APlication  -->
      <div class="not-defineApp" v-if="show">
        <label for="">Not Define Application</label>
        <select v-model="selectedNotDf" @change="selectNotDfApp" class="custom-select">
          <option v-for="(item, index) in  selectListNotDefineApp" v-bind:key ="index" class="custom-option">
            {{ item.SPRG_NAME }}
          </option>
        </select>
      </div>
      <div class="defineApp" v-if="show">
        <label for="">Define Application</label>
        <select v-model="selectedDf" @change ="selectDfApp" class="custom-select">
          <option v-for="(item, index) in selectListDefineApp" v-bind:key ="index" class="custom-option">
            {{ item.DPRG_NAME }}
          </option>
        </select>
      </div>
      <!-- tableNotDefineApp -->
      <div class="tableNotDefineApp">
        <template v-if="ListNotDefineAppHeader">
          <table>
            <thead>
            <tr>
              <template v-for="(item, index) in ListNotDefineAppHeader" :key="index">
              <th v-if="item == 'PRG_NAME' || item =='FUN' || item=='PASSW'">
                {{ item }}
              </th>
              </template>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(item, index) in ListNotDefineApp" :key="index" @click="toggleSelect(item)" :class="{ 'selected': item.isSelected }">
              <template v-for="(item1, index1) in item" :key="index1">
              <td v-if="index1 == 'PRG_NAME' || index1 =='FUN' || index1=='PASSW' ">
                {{ item1 }}
              </td>
              </template>
            </tr>
            </tbody>
          </table>
        </template>
      </div>
        <!-- Button save-->
      <div class="moveAndSaveData" v-if="show">
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
        <button class="saveData" type="submit" @click="saveData()" title="Save">
          Save
        </button>
      </div>
        <!-- tableDefineApp -->
      <div class="tableDefineApp">
        <template v-if="valueInput === false">
          <table>
              <thead>
              <tr>
                  <template v-for="(item, index) in ListDefineAppHeader" :key="index">
                  <th v-if="item == 'PRG_NAME' || item =='FUN' || item=='PASSW' || item=='PRIVILEGE'">
                      {{ item }}
                  </th>
                  </template>
              </tr>
              </thead>
              <tbody >
              <tr v-for="(item, index) in ListDefineApp" :key="index" @click="toggleSelect(item)" :class="{ 'selected': item.isSelected }">
                  <template v-for="(item1, index1) in item" :key="index1">
                  <td v-if="index1 == 'PRG_NAME'|| index1 =='FUN' || index1=='PASSW' || index1=='PRIVILEGE'">
                    {{ item1 }}
                  </td>
                  </template>
              </tr>
              </tbody>
          </table>
        </template>
        <template v-if="valueInput === true">
            <table>
            <thead>
              <tr>
                <template v-for="(item, index) in ListDefineAppHeader" :key="index">
                <th v-if="item == 'PRG_NAME' || item =='FUN' || item=='PASSW' || item=='PRIVILEGE' || item == 'EMP'">
                    {{ item }}
                </th>
                </template>
              </tr>
            </thead>
            <tbody >
              <tr v-for="(item, index) in ListDefineApp" :key="index" @click="toggleSelect(item)" :class="{'selected': item.isSelected }">
                <template v-for="(item1, index1) in item" :key="index1">
                  <td v-if="index1 =='PRG_NAME'|| index1 =='FUN' || index1=='PASSW' || index1=='PRIVILEGE' || index1 == 'EMP'">
                    {{ item1 }}
                  </td>
                </template>
              </tr>
            </tbody>
          </table>
        </template>
      </div>
        <!-- Radio button -->
      <div class="radio-button" v-if="show">
          <h2> Picked: {{ picked }}</h2>
      <form>
      <label for="none">None:</label>
      <input type="radio" id="none" name="name" value="0" v-model="picked" @change="updatePrivilege(0)"><br><br>

      <label for="query">Query:</label>
      <input type="radio" id="query" name="name" value="1" v-model="picked" @change="updatePrivilege(1)"><br><br>
    
      <label for="modify">Modify:</label>
      <input type="radio" id="modify" name="name" value="2" v-model="picked" @change="updatePrivilege(2)"><br>
    </form>
      </div>
      <!-- END grid -->
    </div>
  </div>
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      textListSearch: "",
      show: false,
      isDropdownVisible: false,
      textContent: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      ListEmp: [],
      ListEmpCopyDf:[],
      ListEmpCopyNotDf:[],
      ListEmpMultiple:[],
      ListEmpMultipleHeader:[],
      filteredOptions: [],
      filteredOptionsCopyDf:[],
      filteredOptionsCopyNotDf:[],
      ListNotDefineApp: [],
      ListNotDefineAppHeader: [],
      ListDefineApp: [],
      multipleEmpHeader:[],
      multipleEmp:[],
      TEST:[],
      TEST1:[],
      LISTGROUPDF:[],
      LISTGROUPNOTDF:[],
      ListDefineAppHeader: [],
      selectListNotDefineApp:[],
      selectListDefineApp:[],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP_NO: localStorage.username,
        EMP_NO_NAME:"",
        EMP_COPYDF:"",
        EMP_COPYNOTDF:"",
        PASSW:"",
        FUN:"",
        PRIVILEGE:"",
        PRG_NAME:"",
        SPRG_NAME:"",
        DPRG_NAME:"",
        LISTINPUTEMP:"",
      },
      picked:"2",
      selectedNotDf: "ALL",
      selectedDf: "ALL",
      EMP:[],
      LISTEMP:[],
      value:false,
      valueInput: false,
      listResult: [],
      listHeader: [],
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
    limitedOptionsCopyDf() {
      return this.filteredOptionsCopyDf.slice(0, 4);
    },
    limitedOptionsCopyNotDf(){
      return this.filteredOptionsCopyNotDf.slice(0, 4);
    }
  },
  mounted() {
    this.GetEmp();
    this.GetEmpCopyDf();
    this.GetEmpCopyNotDf();
  },
  methods: {
    async GetEmp() {
      let payload = {
        database_name: localStorage.databaseName,
      };
      let { data } = await Repository.getRepo("getEmpNo", payload);
      this.ListEmp = data.data;
      },
    //copy-emp-Df
    async GetEmpCopyDf() {
      let payload = {
        database_name: localStorage.databaseName,
      };
      try{
        let { data } = await Repository.getRepo("getEmpCopyDf", payload);
        this.ListEmpCopyDf = data.data;
      }catch(error){
        console.error("data: ", error);
      }
    },
    // Copy-Emp-NotDf
    async GetEmpCopyNotDf() {
      let payload = {
        database_name: localStorage.databaseName,
      };
      try{
        let { data } = await Repository.getRepo("getEmpCopyNotDf", payload);
        this.ListEmpCopyNotDf = data.data;
      }catch(error){
        console.error("data: ", error);
      }
    },
    //Emp-List
    async GetEmpList() {
      let payload = {
        database_name: localStorage.databaseName,
        LISTINPUTEMP: this.LISTINPUTEMP,
        valueInput: this.valueInput,
      };
      try{
        let  response  = await Repository.getRepo("selectMultipleEmp", payload);
        this.ListDefineApp = response.data.data;
        this.EMP = response.data.EMP;
        if(this.ListDefineApp.length > 0){
        this.ListDefineAppHeader = Object.keys(this.ListDefineApp[0]);
        }
      }catch(error){
        console.error("data: ", error);
      }
    },
    async filterEmp(){
      const self = this;
        this.filteredOptions = this.ListEmp.filter(function(option){
          if(option && option.EMP_NO_NAME && self.model.EMP_NO_NAME)
          {
            return option.EMP_NO_NAME.toLowerCase().includes(self.model.EMP_NO_NAME.toLowerCase());
          }
          return false;
        });
      
      const allIndex = this.filteredOptions.findIndex(option => option.EMP_NO_NAME ==="ALL");
      if(allIndex > -1){
        const allOption = this.filteredOptions.splice(allIndex, 1) [0];
        this.filteredOptions.unshift(allOption);
      }
    },
    async filterListEmpCopyDf(){
      const self = this;
      this.filteredOptionsCopyDf = this.ListEmpCopyDf.filter(function(option){
        if(option && option.EMP_COPYDF && self.model.EMP_COPYDF){
          return option.EMP_COPYDF.toLowerCase().includes(self.model.EMP_COPYDF.toLowerCase());
        }
        return false;
      });
    },
    //NotDfCopy
    async filterListEmpCopyNotDf(){
      const self = this;
      this.filteredOptionsCopyNotDf = this.ListEmpCopyNotDf.filter(function(option){
        if(option && option.EMP_COPYNOTDF && self.model.EMP_COPYNOTDF){
          return option.EMP_COPYNOTDF.toLowerCase().includes(self.model.EMP_COPYNOTDF.toLowerCase());
        }
        return false;
      });
    },
    async selectNotDefineApp(){
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO_NAME: this.model.EMP_NO_NAME,
        SPRG_NAME: this.selectedNotDf,
        EMP_COPYNOTDF: this.model.EMP_COPYNOTDF,
        VALUE: this.value,
      };
      try{
        let { data } = await Repository.getRepo("selectGetNotDefineApp", payload);
        this.selectListNotDefineApp = data.data;
      }catch(error){
        console.error("data: ", error);
      }
    },
    async selectDefineApp(){
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO_NAME: this.model.EMP_NO_NAME,
        DPRG_NAME: this.selectedDf,
        EMP_COPYDF: this.model.EMP_COPYDF,
        VALUE: this.value,
        valueInput: this.valueInput,
        LISTINPUTEMP: this.LISTINPUTEMP,
      };
      try{
        let { data } = await Repository.getRepo("selectGetDefineApp", payload);
        this.selectListDefineApp = data.data;
      }catch(error){
        console.error("data: ", error);
      }
    },
    async notDefineApp(){
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO_NAME: this.model.EMP_NO_NAME,
        SPRG_NAME: this.selectedNotDf,
        EMP_COPYNOTDF: this.model.EMP_COPYNOTDF,
        VALUE: this.value,
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
        EMP_NO_NAME: this.model.EMP_NO_NAME,
        DPRG_NAME: this.selectedDf,
        PRIVILEGE: this.model.PRIVILEGE,
        EMP_COPYDF: this.model.EMP_COPYDF,
        VALUE: this.value,
        valueInput: this.valueInput,
        LISTINPUTEMP: this.LISTINPUTEMP,
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
    async saveData() {
      let titleValue = "";
      let textValue = "";
      titleValue = "Are you sure to save?";
      textValue = "Once confirmed, data will be updated!";
      this.$swal({
        title: titleValue,
        text: textValue,
        icon: "warning",
        buttons: true,
        dangerMode: true,
      }).then(async (willSave) => {
          if (willSave.isConfirmed == false) return;
          this.LISTGROUPDF = this.TEST
          this.LISTGROUPNOTDF = this.TEST1
          let payload = {
            database_name: localStorage.databaseName,
            EMP_NO: localStorage.username,
            EMP_NO_NAME: this.model.EMP_NO_NAME,
            value: this.value,
            EMP_COPYDF: this.model.EMP_COPYDF,
            EMP_COPYNOTDF: this.model.EMP_COPYNOTDF,
            LISTGROUPDF: this.LISTGROUPDF,
            LISTGROUPNOTDF: this.LISTGROUPNOTDF,
            valueInput: this.valueInput,
            LISTEMP: this.EMP,
          };
          try{
            let { data }  = await Repository.getRepo("InsertDeleteDefineApp", payload);
              if (data.result == "ok") {
                  this.$swal("", "Successfully applied", "success");
              } else {
                  this.$swal("", data.result, "error");
              } this.clearForm();
          }catch(error){
            this.$swal ("", error, "error");
          }
      });
    },
    onEmpChange(){
    this.notDefineApp();
    this.defineApp();
    this.selectNotDefineApp();
    this.selectDefineApp();
    this.show = true;
  },
  onEmpChangeCopyDf(){
    this.selectDefineApp();
    this.defineApp();
    this.show = true;
  },
  onEmpChangeCopyNotDf(){
    this.selectNotDefineApp();
    this.notDefineApp();
  },
  onEmpChangeMultiple(){
    this.GetEmpList();
    this.show = true;
    this.selectDefineApp();
    //this.defineApp();
  },
  selectNotDfApp(){
    this.selectNotDefineApp();
    this.notDefineApp();
  },
  selectDfApp(){
    this.selectDefineApp();
    this.defineApp();
  },
  moveSelectedItemsToDefinedApp() {
    const selectedItems = this.ListNotDefineApp.filter(item => item.isSelected);
    selectedItems.forEach(item => {
      item.PRIVILEGE = this.picked;
    });
    this.ListDefineApp.push(...selectedItems);
    this.TEST.push(...selectedItems);
    this.ListNotDefineApp = this.ListNotDefineApp.filter(item => !item.isSelected);
    selectedItems.forEach(item => {
      item.isSelected = false;
    });
    return this.TEST;
  },
  moveSelectedItemsToNotDefinedApp(){
    const selectedItems = this.ListDefineApp.filter(item => item.isSelected);
    const initiallyInListDefineApp = selectedItems.filter(item => this.TEST1.includes(item) || !this.TEST.includes(item));
    this.ListNotDefineApp.push(...selectedItems);
    this.ListDefineApp = this.ListDefineApp.filter(item => !item.isSelected);
    selectedItems.forEach(item =>{
      item.isSelected = false;
    });
    this.TEST = this.TEST.filter(item => !selectedItems.includes(item));
    this.TEST1.push(...initiallyInListDefineApp);
  },
  toggleSelect(item) {
  item.isSelected = !item.isSelected;
},
  updatePrivilege(value){
    this.ListDefineApp.forEach(item =>{
      if(item.isSelected){
        item.PRIVILEGE = value;
      }
    });
  },
  clearForm(){
    this.TEST = [];
    this.TEST1 = [];
  },
  clearTable(){
    this.valueInput = false;
    this.ListDefineApp = [];
    this.ListDefineAppHeader = [];
    this.ListNotDefineApp = [];
    this.ListNotDefineAppHeader = [];
    this.selectListDefineApp = [];
    this.selectListNotDefineApp = [];
    this.model.EMP_COPYDF = "";
    this.model.EMP_COPYNOTDF = "";
    this.model.EMP_NO_NAME = "";
    this.show = false;
    this.picked = "0";
  },
  ClearDefApp(){
    this.ListDefineAppHeader = [];
    this.ListDefineApp = [];
  },
  BackToParent() {
    this.$router.push({ path: "/Home/ConfigApp" });
  },
  },
};
</script>

<style lang="scss" scoped>
.div-all {
  margin-left: 35px;
  font-family: sans-serif;
  font-size: 18px;
}
.div-config-name {
  margin-left: 20px;
  line-height: 50px;
  span {
    font-weight: 555;
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

.menu {
  display: grid;
  grid-template-columns: 150px 150px;
  grid-template-rows: 50px;
  overflow: hidden;
  text-align: center;
  cursor: pointer;
  font-weight: 555;
  width: 300px;
  border: 1px solid rgb(82, 78, 78);
  border-radius: 5px;
  #Config7 {
    grid-column: 1 / 2;
    background-color: #f1f1f1;
    transition: 0.3s;
    padding-top: 10px;
    &:hover{
      color: #fff;
      background-color: #9c9c9c;
    }
   }
  #Privilege{
    color: #333;
    grid-column: 2 / 3;
    background-color: #9c9c9c;
    transition: 0.3s;
    padding-top: 10px;
  }
}
  #datalistEmp{
    height: 40px;
    width: 250px;
    border-radius: 5px;
  }
  #copyEmp{
    grid-column: 4 / 5;
    height: 15px;
    width: 15px;
  }
  .copy-Emp-NotDf{
    margin: 0 10px 0 10px;
    #datalistEmpCopyNotDf{
      height: 40px;
      width: 250px;
      border-radius: 5px;
    }
  }
  .copy-EmpDf{
    grid-column:  3/4;
    margin: 0 10px 0 10px;
    #datalistEmpCopyDf{
      height: 40px;
      width: 250px;
      border-radius: 5px;
    }
  }
#grid-container{
  display: grid;
  margin-top: 5px;
  grid-template-columns: 40% 10% 40% 10%;
  grid-template-rows: 100px 40px 140px 140px 140px;
  height: auto;
  width: auto;
  overflow: auto;
  color: rgb(34, 33, 33);

 .not-defineApp{
    grid-column: 1 / 3;
 }
 .defineApp{
   grid-column: 3 /5;
 }
  .custom-select {
    width: 200px;
    border: 1px solid #ccc;
    border-radius: 4px;
    background-color:  rgb(255, 255, 255);
    font-size: 16px;
    color: #333;
    margin-left: 5px;
    &:focus{
      border-color: #66afe9;
      outline: none;
      box-shadow: 0 0 8px rgba(102, 175, 233, 0.6);
    }
  }
  .custom-option {
    padding: 10px;
    background-color: rgb(255, 255, 255);
    color: #333;
    &:hover{
      background-color: #c99696;
    }
  }
// }
  .selected {
  background-color: #89cfed;
  }
  .tableNotDefineApp{
    overflow-y: auto;
    height: 400px;
    width: auto;
    grid-row-start: 3;
    grid-row-end: 6;
   thead {
    th {
      background-color: #024873;
      position: sticky;
      top: 0;
      z-index: 2;
      color: #fff;
      min-width: 60px;
      padding: 0.5rem 1.5rem;
    }
  }
   tbody {
    td {
      overflow-y: auto;
      white-space: nowrap;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 16px;
    }
  }
}
.moveAndSaveData{
  grid-row-start: 4;
  grid-row-end: 5;
  .notDefineButton {
    height: 40px;
    width: 45px;
    border-radius: 5px;
    background-color: rgb(79, 167, 162);
    &:hover{
      background-color: rgb(0, 128, 0);
    }
  }
  .defineButton{
    height: 40px;
    width: 45px;
    border-radius: 5px;
    background-color: rgb(79, 167, 162);
    &:hover{
      background-color: rgb(0, 128, 0);
    }
  }
  .saveData{
    background-color:greenyellow;
    border-radius: 5px;
    border: 47px;
    height: 35px;
    width: 95px;
    font-size: 20px;
    &:hover{
      background-color: rgb(0, 128, 0);
    }
  }
}
.tableDefineApp {
  margin-top: 0px;
  overflow-y: auto;
  height: 400px;
  width: auto;
  grid-row-start: 3;
  grid-row-end: 6;
  thead {
    th {
      background-color: #024873;
      position: sticky;
      top: 0;
      z-index: 2;
      color: #fff;
      min-width: 60px;
      padding: 0.5rem 1.5rem;
    }
  }
  tbody {
    td {
      overflow-y: auto;
      white-space: nowrap;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 16px;
    }
  }
}
  .radio-button{
    grid-row-start: 4;
    grid-row-end: 6;
    h2{
      color: rgb(0, 128, 0);
    }
  }
}
</style>