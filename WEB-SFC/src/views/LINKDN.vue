<template>
  <div class="all">
    <header>
      <h1>IMPORT DN</h1>
    </header>
    <main>
      <div class="container">
        <form @submit.prevent="SaveData">
          <label for="bfid-id">BFIH DN</label>
          <input type="text"
            id="bfid-id"
            name="bfih-dn"
            v-model="model.BFIH_DN"
            required
            autocomplete="off"
            maxlength="20" 
            placeholder="BFIH DN.."
            />
            <label for="fti-dn">FTI DN</label>
            <input type="text" 
              id="fti-dn"
              name="fti-dn"
              required
              autocomplete="off"
              v-model="model.FTI_DN"
              placeholder="FTI DN.."
              maxlength="20" 
              />

            <label for="note-id">QUANTITY</label>
            <input type="text"
              id="note-id"
              name="note-dn"
              autocomplete="off"
              v-model="model.NOTE_DN"
              maxlength="10"
              pattern="[1-9]{1}[0-9]{9}"
              onkeydown="javascript: return ['Backspace','Delete','ArrowLeft','ArrowRight'].includes(event.code) ? true : !isNaN(Number(event.key)) && event.code!=='Space'"
              placeholder="Quantity.."/>

            <button @click="SaveData" class="submit" type="button">Submit</button>
        </form>
      </div>
    <!-- <table> -->
      <div class="query-table">
        <div class="searchbox">
           <input
            v-on:keyup.enter="QuerySearch()"
            v-model="valueSearch"
            type="text"
            placeholder = "Search BFIH DN.."
          />
        </div>
        <div class="data-table">
          <table class="tableMain">
              <thead>
                <tr>
                  <th v-for="(item, index) in DataTableHeader" :key="index">
                    {{ item }}
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr  v-for="(item2, index2) in DataTable" :key="index2">
                  <td v-for="(item3, index3) in item2" :key="index3">
                    {{ item3 }}
                  </td>
                </tr>
              </tbody>
          </table>
        </div>
      </div>
    </main>
  </div>
</template>
<script>
import Repository from "../services/Repository";
  export default {
    data() {
      return {
        valueSearch:'',
        DataTableHeader:[],
        DataTable:[],
        model: {
          database_name: localStorage.databaseName,
          EMP_NO: localStorage.username,
          BFIH_DN:"",
          FTI_DN:"",
          NOTE_DN:"",
        },
      };
    },

  mounted(){
    this.QuerySearch();
  },
  methods: {
  async SaveData() {
    if(!this.model.BFIH_DN || !this.model.FTI_DN || !this.model.NOTE_DN){
      this.$swal("", "Empty fields.", "info")
      return;
    }
      let payload = {
        database_name: localStorage.databaseName,
        EMP_NO: this.model.EMP_NO,
        BFIH_DN:this.model.BFIH_DN,
        FTI_DN: this.model.FTI_DN,
        NOTE_DN: this.model.NOTE_DN,
        LINK_DN: "LINK_DN",
      };
      try{
        let { data }  = await Repository.getRepo("GetLinkDnImport", payload);

          if (data.result == "ok") {
              this.$swal("", "Successfully applied", "success");
          } else {
              this.$swal("", data.data, "error");
          }
          await this.QuerySearch();
          await this.ClearForm();
      }catch(error){
        this.$swal ("error ex", error, "error");
      }
    },

    async QuerySearch(){
      let payload = {
        database_name: localStorage.databaseName,
        BFIH_DN: this.valueSearch,
      };
      try{
        let { data } = await Repository.getRepo("GetDnContent", payload);
        this.DataTable = data.data;
        if (typeof this.DataTable != "undefined") {
        if (this.DataTable.length != 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
        }
      }
      }catch(error){
        this.$swal ("error ex", error, "error");
      }
    },
    ClearForm(){
      this.model.BFIH_DN = "";
      this.model.FTI_DN ="";
      this.model.NOTE_DN = "";
    },
  }
};
</script>
<style scoped>
body {
  font-family: Arial, Helvetica, sans-serif;
}
* {
  box-sizing: border-box;
}

.all{
  background-color: rgb(235 235 235);
  padding: 50px 0 80px 0;
}

h1{
    text-align: center;
    color: #333330;
    font-weight: bold;
}
main{
  display: grid;
  justify-content: center;
  max-height: 400px;
  grid-template-columns: 20% 40%;
  gap: 20px;

}
form input{
  width: 100%;
  padding: 12px;
  font-size: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
  margin-top: 6px;
  margin-bottom: 16px;
  resize: vertical;
}

label{
    color:rgb(255, 255, 255);
    font-size: 1rem;
    font-weight: bold;
}

.container {
  border-radius: 5px;
  background-color: #333330;
  padding: 20px;
  width: 100%;
}

.submit {
  background-color: #a19d9d;
  color: rgb(255 253 253);
  padding: 8px 16px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
  font-size: 18px;
}

.submit:hover {
  background-color: #050505;
  color: rgb(255, 255, 255);
  transition: 1s;
}
.query-table{
  width: 100%;
  max-height: 400px;
}

/* start */
.data-table {
  margin-top: 0px;
  overflow-y: auto;
  height: 357px;
  max-width: 525px;
}

table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}
table td, th {
  text-align: left;
  padding: 8px;
  font-size: 1rem;
  
}
thead th{
  background-color: #333330;
  color: #fff;
  position: sticky;
  top: 0;
  z-index: 2;
}
tbody td{
  color: #000;
  white-space: nowrap;
  z-index: 1;
}

tbody tr:hover{
  background-color: #a19e9e;
}
/* end */
.searchbox{
  height: 40px;
  margin-bottom: 3px;
}
.searchbox input{
  width: 50%;
  height: 40px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
.btn-query{
  border: 1px solid rgb(26, 25, 25);
  border-radius: 5px;
  height: 40px;
  width: 40px;
}
</style>