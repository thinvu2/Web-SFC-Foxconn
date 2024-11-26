<template>
  <div class="div-all">
    <div class="row">
        <div class="div-back" @click="BackToParent()">
            <Icon icon="chevron-left" class="back-icon sidenav-icon" />
        </div>
        <div class="div-config-name row">
            <span>AMS_PRIVILEGE</span>
        </div>
    </div>

    <div class="menu">
        <router-link 
            to="/Home/ConfigApp/Config7" 
            id="Config7"
            @mouseover = "isHovered = true"
            @mouseleave = "isHovered = false"
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
            :class = "{ 'hovered' : isHovered }"
            >
            AMS_PRIVILEGE
        </router-link>
    </div>
        <!-- Main -->
    <div class="container-title">
      <div class="right-title">
        <input
          type="text"
          v-model="searchEmp"
          placeholder="Enter emp_name..."
        />
      </div>
      <div class="left-title">
        <h2 style="font-weight: 555;">AMS_PRIVILEGE</h2>
      </div>
    </div>

    <div class="container">
      <div class="right-area">
        <template v-if="ListDataTableHeader">
          <table>
            <thead>
              <tr>
                <template v-for="(item, index) in ListDataTableHeader" :key="index">
                  <th v-if="item == 'EMP_NO'">
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in filteredEmp" :key="index" >
                <template v-for="(item1, index1) in item" :key="index1">
                  <td v-if="index1 == 'EMP_NO'" @click="getAmsPrivilege(item1)">
                    {{ item1 }}
                  </td>
                </template>
              </tr>
            </tbody>
            </table>
        </template>
      </div>
         <!-- save button -->
      <div class="save-button">
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
      <!-- left-area -->
      <div class="left-area">
            <label v-for="(item, index) in radioOptions" :key="index">
              <input
                type="checkbox"
                :value="item.value"
                @change="updateCloneDataAms(item.value, $event)"
                :checked="ListDataAms.some(ams => ams.AP_GROUP === item.value)"
              />
              {{ item.label }}
            </label>
      </div>
    </div>
  </div>
</template>

<script>
import Repository from "../../services/Repository";
export default {
    data() {
        return {
          radioOptions: [
            { value: 'ALL', label: 'ALL' },
            { value: 'ALL_PART', label: 'ALL_PART' },
            { value: 'LABELROOM', label: 'LABELROOM' },
            { value: 'PD', label: 'PD' },
            { value: 'QA', label: 'QA' },
            { value: 'RE', label: 'RE' },
            { value: 'SFIS', label: 'SFIS' },
            { value: 'SHARE', label: 'SHARE' },
            { value: 'WH', label: 'WH' },
          ],
            searchEmp: "",
            selectRadioButton: [],
            isHovered: false,
            ListDataTableHeader: [],
            ListDataTable: [],
            ListDataAms: [],
            cloneListDataAms: [],
           selectedEmpNo: '',
            model: {
              EMP_NO: '',
            }
        }
    },
    watch: {
      searchEmp() {
        this.GetEmpList();
      }
    },

    mounted() {
      this.GetEmpList();
    },

    computed: {
      filteredEmp() {
        if(!Array.isArray(this.ListDataTable)) {
          console.error('ListDataTable is not an array: ', this.ListDataTable);
         return [];
        }
          const result = this.ListDataTable.filter(listData => {

           return listData.EMP_NO.toLowerCase().includes(this.searchEmp.toLowerCase())
          });
          return result;
      }
    },
    methods:{
      // show Emp
      async GetEmpList() {
        let databaseName = localStorage.databaseName;
        try{
          let  { data }  = await Repository.getApiServer(`getEmpNoAms?database_name=${databaseName}`);
          if(data && data.data) {
            this.ListDataTable = data.data;
            if(this.ListDataTable.length > 0){
            this.ListDataTableHeader = Object.keys(this.ListDataTable[0]);
            }else {
              this.ListDataTable = [];
            }
          }else {
            this.ListDataTable = [];
          }
        }catch(error){
          console.error("data: ", error);
        }
      },

      async getAmsPrivilege(emp_no) {
        console.log("emp_no: ", emp_no);
       this.searchEmp = emp_no;
        let databaseName = localStorage.databaseName;
        try {
          let { data } = await Repository.getApiServer(`getAmsPrivilege?database_name=${databaseName}&EMP_NO=${emp_no}`);
          this.ListDataAms = data.data || [];
        }catch(error) {
          console.error("data: ", error);
         }
      },
    //save data send data to back end will compare two array from front- end and back-end if different then insert or delete 
      async SaveData() {
        let result = this.searchEmp.match(/^[^_]+/);
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
            let payload = {
              database_name: localStorage.databaseName,
              EMP_NO: result[0],
              ListDataAms: this.ListDataAms
            };
            try{
              let { data }  = await Repository.getRepo("insertDeleteAms", payload);
              if (data.result == "ok") {
                  this.$swal("", "Successfully applied", "success");
              } else {
                this.$swal("", data.result, "error");
              } 
              this.ClearForm();
            }catch(error){
              this.$swal ("", error, "error");
            }
        });
      },

      updateCloneDataAms(value, event) {
        if(event.target.checked) {
          if(!this.ListDataAms.some(ams => ams.AP_GROUP === value)) {
            this.ListDataAms.push({ AP_GROUP: value});
          }
        }else {
          this.ListDataAms = this.ListDataAms.filter(
            ams => ams.AP_GROUP !== value
          );
        }
      },

      ClearForm() {
        this.selectedEmpNo = '';
        this.model.EMP_NO ='';
        this.searchEmp ='';
        this.ListDataAms = []
      },

      BackToParent() {
        this.$router.push({ path: "/Home/ConfigApp" });
      },
    },
  }
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
}
.div-back {
  float: left;
  background: #e6e2e2;
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
  grid-template-columns: 150px 150px 150px;
  grid-template-rows: 50px;
  overflow: hidden;
  text-align: center;
  cursor: pointer;
  font-weight: 400;
  font-family: "Helvetica Neue", Roboto, Arial, "Droid Sans", sans-serif;
  width: 450px;
  border: 1px solid rgb(82, 78, 78);
  border-radius: 5px;

  #Config7 {
    grid-column: 1 / 2;
    background-color: #fff;
    transition: 0.3s;
    padding-top: 10px;
    color: #333;
    &:hover{
      color: #fff;
      background-color: #9c9c9c;
    }
  }
  #Privilege {
    grid-column: 2 / 3;
    background-color: #fff;
    transition: 0.3s;
    padding-top: 10px;
    color: #333;
    &:hover{
      color: #fff;
      background-color: #9c9c9c;
    }
  }

  #Ams_Privilege {
    grid-column: 3 / 4;
    background-color: #9c9c9c;
    transition: 0.3s;
    padding-top: 10px;
    color: #333;
  }
  #Ams_Privilege.hovered {
    background-color: #fff;
  }
}

.container-title {
  display:  grid;
  grid-template-columns: 45% 55%;
  grid-template-rows: 42px;
  margin-top: 2px;
  .right-title {
    width: 200px;
    height: 50px;
    border-radius: 5px;
  }
}

.container {
  display: grid;
  grid-template-columns: 30% 15% 55%;
  grid-template-rows: 420px;
  max-height: 420px;
  width:  auto;
  // overflow: auto;
  color: rgb(34, 33, 33);

}

.right-area{
    overflow: auto;
    max-height: 420px;
    max-width: 300px;
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
  tr {
    &:hover {
      background: #89cfed;
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

.save-button {
  height: 420px;
  line-height: 420px;
  white-space: nowrap;
}

.left-area {
  background-color: #acd4ed;
  border-radius: 5px;
  max-height: 420px;
  width: auto;
  input {
    width: 20px;
    height: 20px;
    margin-left: 10px;
  }
  label {
    font-size: 20px;
    display: block;
    margin: 15px 0 0 15px;
  }
}

.ams-border {
  width: 150px;
  height: 40px;
  line-height: 40px;
  white-space: nowrap;
  border: 1px solid black;
  border-radius: 5px;
  margin-left: 10px;
  margin-bottom: 1px;
  &:hover {
    background-color: #2499bd;
    transition: 0.2s;
  }
}
</style>