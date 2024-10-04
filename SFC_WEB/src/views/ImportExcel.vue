<template>
  <div id="app">
    <div class="header">
      <div class="header-rainbow">IMPORT EXCEL</div>
    </div>
    <section class="content">
        <h2>Choose table</h2>
      <div class="radio-buttons">
<!-- Sfc -->
        <div class="from-check">
          <p>SFC</p>   
        </div>

        <div class="from-check">
          <input class="checkTable" id="updatertrsn" type="radio" name="fav_language" value="SPDM_SN" v-model="value">
          <label for="updatertrsn">R_SPDM_SN</label>
        </div>

        <div class="from-check" id="rsndetailfrom">
          <!-- <input class="checkTable" id="updatersndetail" type="radio" name="fav_language" value="UPDATERSNDETAIL" v-model="value">
          <label for="updatersndetail">R_SN_DETAIL_T</label> -->
        </div>
<!-- Allpart -->
          <!-- <div class="from-check">
          <p>ALLPART</p>   
        </div>
          <div class="from-check">
           <input class="checkTable" id="updatetrcode" type="radio" name="fav_language" value="WAIBAOTRCODE"  v-model="value">
          <label for="updatetrcode">WAIBAO_TR_CODE_DETAIL</label> 
        </div>

        <div class="from-check">
           <input class="checkTable" id="updatetrproduct" type="radio" name="fav_language" value="WAIBAOTRPRODUCT" v-model="value">
          <label for="updatetrproduct">WAIBAO_TR_PRODUCT_DETAIL</label> 
        </div>

        <div class="from-check">
          <input class="checkTable" id="updatewaibaotrsn" type="radio" name="fav_language" value="WAIBAOTRSN" v-model="value">
          <label for="updatewaibaotrsn">WAIBAO_TR_SN_DETAIL</label> 
        </div> -->
      </div>
      <div class="file-input">
          <button @click="submitFile" class="submit">Submit</button>
          <input id="choosefile" type="file" @change="handleFileUpload"/>
          <button @click="queryTable" class="query">Check Data</button>
          <input id="queryDn" 
            type="text" 
            autocomplete="off"
            @change="handleSearchData" 
            v-model="IN_DN" 
            placeholder="SN/SSN/MODEL..."/>
      </div>
    <div class="status">
      <p id="total">Total: {{ total }}</p>
      <p id="success">Success: {{ success }}</p>
      <p id="exists">Exists: {{ exists }}</p>
      <p id="error" @click="showErrorList">Error: {{ error }}</p>
      <div id="error-list-container" v-if="showErrors">
        <table id="error-table">
            <thead>
              <tr>
                <th>
                  ERROR
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(error, index) in errorList" :key="index">
                <td>{{ error }}</td>
              </tr>
            </tbody>
          </table>
      </div>
    </div>
    <div class="data-table">
      <template v-if="IN_DN !==''">
          <thead>
            <tr>
              <th v-for="(item, index) in ListDataTableSearchHeader" v-bind:key="index">
                {{ item }}
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item1, index1) in ListDataTableSearch" v-bind:key="index1">
              <td v-for="(item2, index2) in item1" v-bind:key="index2">
                {{ item2 }}
              </td>
            </tr>
          </tbody>
      </template>
        <template v-else>
          <table>
        <thead>
          <tr>
            <th v-for="(item, index) in ListDataTableHeader" :key="index">
                {{ item }}
            </th>
          </tr>
        </thead>
        <tbody >
          <tr v-for="(row, rowIndex) in ListDataTable" :key="rowIndex">
            <td v-for="(cell, cellIndex) in row" :key="cellIndex">
              {{ formatCell (cell,cellIndex) }}
            </td>
          </tr>
        </tbody>
      </table>
        </template>
    </div>
    </section>
  </div>
</template>
<script>
import Repository from "../services/Repository";
import XLSX from 'xlsx';
import Swal from 'sweetalert2';
export default {
  data() {
    return {
      selectedFile: null,
      value:"SPDM_SN",
      ListDataTableHeader:[],
      SPDM_SN_Detail:[
      "SERIAL_NUMBER","SHIPPING_SN","MODEL_NAME","ASIC_PART","BOARD","COMPONENT_REPLACED","DATE_REPLACED","FAILED_LOGFILE",
      "FIRST_TEST_FAILURE","NEXT_STEP","SERVER_SIGN_TRANSACTIONID","FACTORY_RESULT","DATA1","DATA2","DATA3","DATA4"
      ],
      ListDataTable:[],
      model: {
        database_name: localStorage.databaseName,
        databaseAp:"ALLPART",
        EMP_NO: localStorage.username,
      },
      total: 0,
      exists: 0,
      success: 0,
      error:0,
      errorList: [],
      showErrors:false,
      IN_DN:"",
      ListDataTableSearch:[],
      ListDataTableSearchHeader:[],
    };
  },
methods: {
  showErrorList(){
    if(this.errorList.length == 0){
      Swal.fire("", "No errors found.", "info");
      return;
    }
    this.showErrors = !this.showErrors;
  },
  handleSearchData(){
    this.ListDataTable =[];
    this.ListDataTableHeader = [];
  },
    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
      event.target.value ='';
      const reader = new FileReader();
      reader.onload = async (e) => {
        const data = new Uint8Array(e.target.result);
        const workbook = XLSX.read(data, { type: 'array' });
        const sheetName = workbook.SheetNames[0];
        const sheet = workbook.Sheets[sheetName];
        const excelData = XLSX.utils.sheet_to_json(sheet, { header: 1 });
        this.ListDataTableHeader = excelData[0];
        this.ListDataTable = excelData.slice(1);
        this.convertDatesInTable();
        console.log("ListDataTable: ", this.ListDataTable);
      };
      reader.readAsArrayBuffer(this.selectedFile);
    },
    formatCell(cell, cellIndex) {
      if (typeof cell === 'number' && (this.ListDataTableHeader[cellIndex].toLowerCase().includes('time')||
      this.ListDataTableHeader[cellIndex].toLowerCase().includes('date'))) {
        // Convert Excel date to JavaScript date
        const jsDate = new Date(Math.round((cell - 25569) * 86400 * 1000));
        // Format the date to 'YYYY/MM/DD HH:mm:ss' format using 24-hour clock
        const formattedDate = jsDate.toLocaleString('en-GB', { hour12: false }).replace(',', '');
        return formattedDate;
      } else if (typeof cell === 'string' && (this.ListDataTableHeader[cellIndex].toLowerCase().includes('time')||
      this.ListDataTableHeader[cellIndex].toLowerCase().includes('date'))) {
        // Parse and convert AM/PM time to 24-hour format
        const dateParts = cell.split(' ');
        let [date, time, period] = dateParts;
        if (period) {
          let [hours, minutes, seconds] = time.split(':');
          if (period.toLowerCase() === 'pm' && hours !== '12') {
            hours = (parseInt(hours, 10) + 12).toString();
          } else if (period.toLowerCase() === 'am' && hours === '12') {
            hours = '00';
          }
          time = [hours, minutes, seconds].join(':');
        }
        return `${date} ${time}`;       
      }
      return cell;
    },
    convertDatesInTable() {
      this.ListDataTable = this.ListDataTable.map(row => {
        return row.map((cell, cellIndex) => {
          if (this.ListDataTableHeader[cellIndex].toLowerCase().includes('time')) {
            return this.formatCell(cell, cellIndex);
          }
          if (this.ListDataTableHeader[cellIndex].toLowerCase().includes('date')) {
            return this.formatCell(cell, cellIndex);
          }
          return cell;
        });
      });
    },
    async submitFile() {
      if (!this.selectedFile) {
        Swal.fire("", "Please select a file first.", "warning");
        return;
      }
      if (this.ListDataTable.length > 1000) {
        Swal.fire("", "Can't update more than 1000 rows.", "warning");
        return;
      }
     if (this.value === 'SPDM_SN' && JSON.stringify(this.SPDM_SN_Detail) !== JSON.stringify(this.ListDataTableHeader)) {
        Swal.fire("", "The updated data format is incorrect.", "warning");
        return;
      }

      this.convertDatesInTable();
        let payload = {
          database_name: localStorage.databaseName,
          databaseAp: this.model.databaseAp,
          EMP_NO: this.model.EMP_NO,
          value: this.value,
          data: this.ListDataTable.map(row =>{
            let rowData = {};
            this.ListDataTableHeader.forEach((header, index) =>{
              rowData[header] = row[index];
            });
            return rowData;
          }),
        };
        console.log("payload: ", payload);
        try {
          const response = await Repository.getRepo("ImportExcel", payload);

          console.log("response: ", response);
          if (response.status === 200) {
            const data = response.data;
            this.total = data.total;
            this.exists = data.exists;
            this.success = data.success;
            this.error = data.error;
            this.errorList = data.errorList;
            if (data.result === "ok") {
              Swal.fire("", "File uploaded successfully.", "success");
            } else {
              Swal.fire("", data.result, "error");
            }
          } else {
            const errorData = response.data;
            Swal.fire("", errorData.message, "error");
          }
        } catch (error) {
          console.error('Error:', error);
          Swal.fire("", error.message, "error");
        }
    },
    async queryTable(){
      let payload = {
        database_name: localStorage.databaseName,
        //CHECK_BFIH_DN: "CHECK_SPDM_SN",
        CHECK_BFIH_DN: this.IN_DN,
        
      }
      console.log("payload: ",payload);
      try {
        const response = await Repository.getRepo("QuerySPDM_SN", payload);
        console.log("response: ", response);

        if (response.status === 200){
          const data = response.data;
          this.ListDataTableSearch = response.data.data;
          
          if(typeof this.ListDataTableSearch !== "undefined"){
            if(this.ListDataTableSearch.length !== 0){
              this.ListDataTableSearchHeader = Object.keys(this.ListDataTableSearch[0]);
            }
          }

          if(data.result === "ok"){
            Swal.fire("", "OK", "success");
          }else{
            Swal.fire("", data.result, "error");
          }
        }else {
            const errorData = response.data;
            Swal.fire("", errorData.message, "error");
          }
        } catch (error) {
          console.error('Error:', error);
          if(error.response){
            Swal.fire("Error ex", error.response.data.message || "An error", "error");
          }else if(error.request){
            Swal.fire("Error", "No response received from the server", "error");
          }else{
            Swal.fire("Error ex1", error.message, "error");
          }
        }
    }
  },
};
</script>
<style scoped>

body {
  font-family: 'Arial', sans-serif;
  background-color: #f5f5f5;
  margin: 0;
  padding: 0;
}

#app {
  width: 80%;
  margin: 0 auto;
  padding-bottom: 40px;
}

#error-list-container{
  overflow: auto;
  max-height: 200px;
  max-width: 400px;
}
#error-table {
  max-width: 400px;
}

#error-table th, #error-table td {
  color: #ccc;
  word-wrap: break-word;
  white-space: nowrap;
  /* overflow: hidden; */
  text-overflow: ellipsis;
}
#error-table th{
  background-color: #3d9941;
  color: #fff;
}
#error-table td{
  background-color: #ccc;
  color: #0c0c0c;
}

.header{
  background-color: #5fad63;
}

.header-rainbow {
  padding: 10px 0;
  text-align: center;
  margin-bottom: 5px;
  border-radius: 5px;
  font-family:arial black;
  font-size:3rem;
  background-image:
  linear-gradient(to right, rgb(76, 171, 235), rgb(241, 159, 6), rgb(243, 243, 6), rgb(7, 182, 7), rgb(30, 30, 240), rgb(110, 8, 184), rgb(238, 110, 238), rgb(228, 35, 35));
  -webkit-background-clip: text;
          background-clip: text;
  -webkit-text-fill-color: transparent;
  /* animation: rainbow-animation 240s linear infinite; */
}
@keyframes rainbow-animation{
  to{
    background-position: 4500vh;
  }
}

.content {
  display: grid;
  background: white;
  padding: 0 20px 20px 20px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  border-radius: 5px;
  grid-template-rows: 40px 80px 50px 40px 350px;
  min-height: 400px;
}

h2 {
  margin-bottom: 20px;
}
/* // */
.radio-buttons {
  display: grid;
  grid-template-rows: 40px 40px;
  grid-template-columns: 10% 30% 30% 30%;
  margin-bottom: 20px;
}
.from-check {
  display: flex;
  align-items: center;
}
.from-check p{
  color: #4b4848;
  margin-bottom: 0.1rem;
}
#rsndetailfrom{
  grid-column: 3 / 5;
}
.checkTable {
  margin-right: 10px;
  height: 16px;
  width: 16px;
}
label {
  display: inline-block;
  margin-bottom: 0.1rem;
  font-size: 14px;
  color: #292121;
}

.file-input {
  display: flex;
  align-items: center;
  gap: 10px;
}

#choosefile {
  padding: 10px;
}

.submit {
  background-color: #61b664;
  color: rgb(240, 235, 235);
  padding: 8px 16px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
  font-size: 18px;
}

.submit:hover {
  background-color: #359c3a;
  color: rgb(216, 216, 216);
  transition: 1s;
}

.status {
  z-index: 2;
  margin-bottom: 5px;
  background-color: #e7f3e7;
  padding: 5px 10px 0 10px;
  border-radius: 5px;
  border: 1px solid #ccc;
}

.data-table {
  overflow: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 5px;
  max-height: 350px;
}

thead {
  background-color: #4CAF50;
  color: white;
  font-size: 14px;
}
tbody{
  font-size: 14px;
  color: #292121;
}

th {
  border: 1px solid #ddd;
  padding: 8px;
}
td{
  border: 1px solid #ddd;
}
tbody tr:hover{
  background-color: #e0dbdb;
  color: #000;
}

th {
  text-align: left;
}

p {
  display: inline-block;
  margin-left: 10px;
  font-size: 18px;
}

#total {
  color: rgb(51, 50, 33);
}

#success {
  color: rgb(36, 134, 36);
}

#exists {
  color: rgb(175, 172, 5);
}
#error{
  color: rgb(170, 20, 20);
}
</style>