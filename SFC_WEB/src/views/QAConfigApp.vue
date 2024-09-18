<template>
  <div class="content" style="min-height:700px">
    <div class="div-back" style="font-size:24px" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />Back     
                         
    </div>

    <div class="row">
      <template v-for="(item, index) in dataList1" :key="index">
        <div class="icon-app-disable col-sm-4" style="padding: 0 0px 8px 35px;border: 1px solid #Black" v-if="item.IsDisable == 0">
          <div class="icon-content">
                <span class= "config-border font-text">[{{ item.Id }}] . </span>
                <span class="text-config-name font-text">{{ item.Name }}</span>
          </div>          
        </div>
        <div v-else class="icon-app col-sm-4" style="padding: 0 0px 8px 35px;cursor:pointer;border: 1px solid #Black" @click="GotoRoute(item.Route)">
          <template v-if="item.Route != null ">
            <div class="icon-content">
                <span class ="config-border font-text">[{{ item.Id }}] . </span>
                <span class="text-config-name font-text text-primary">{{ item.Name }}</span>
             </div>  
            <!-- <span class="text-config-name ">{{ item.Name.toString() assets/img/config.png }}</span>-->
          </template>
        
        </div>
      </template>
    </div>
  </div>
</template>

<script>
//import { mapState, mapMutations } from 'vuex';
import dataConfigQA from "../data/data_QAconfig";
import Repository from "../services/Repository";

export default {
  data() {
    return {
      dataList: [],
      dataCheckPrivilege: [],
      dataList1: [],
    };
  },
  created() {
    document.title = "Config QA";
    this.dataList1 = dataConfigQA;
  },
  mounted() {
    this.GetDatalist();
    /*this.CheckAllPrivilege();*/
  },
  methods: {
    GotoRoute(route) {
      this.$router.push({ path: route });
    },
    BackToParent() {
      this.$router.push({ path: "/Home/Applications" });
    },
    async CheckAllPrivilege() {
      console.log(this.dataList1)
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
      };

      var {data } = await Repository.getRepo("GetConfigPrivilge_Group", payload);
      this.dataList = [];
      this.dataList = data.data;
      console.log(this.dataList);

      
      if (this.dataList.length >0 )
      {
      
        var payload1 = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
          };
     

        var  data1  = await Repository.getRepo("GetConfigPrivilge_Group", payload1);
        this.dataCheckPrivilege = data1.data;

        this.dataList1.forEach((element) => {
        element.IsDisable = 1;
        debugger;
        if (typeof this.dataCheckPrivilege != "undefined") {
          if (this.dataCheckPrivilege.length != 0) {
            if (element.Name != "") {
              if (
                this.dataCheckPrivilege.some(
                  (p) => p.FUN == element.Name
                )
              ) 
              {
                element.IsDisable = 2;
              } else {
                element.IsDisable = 1;
              }
            } else {
              if (this.dataCheckPrivilege.some((p) => p.FUN == element.ReplaceName)) {
                element.IsDisable = 1;
              } else 
              {
                element.IsDisable = 2;
              }
            }
          }
        }
      });
      }
      //console.log("listconifg: "+this.dataList.length);
      
    },


      async GetDatalist() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username
      };
      var { data } = await Repository.getRepo("GetConfigPrivilge_Group", payload);
      this.dataList = [];
      this.dataList = data.data;

      console.log(this.dataList);
      if (typeof this.dataList != "undefined") {
        if (this.dataList.length != 0) {
          this.dataList1.forEach((element) => {
            if(this.dataList.some((p) => p.FUN == element.ReplaceName))
            {
              element.IsDisable = 1;
            } else {
              element.IsDisable = 0;
            }
          });
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>

.config-content {

 display: grid;
  height: 90vh;
 
  background: #7cb5f5;
  grid-template-columns: 25% 25% 25% 25%;
  grid-template-rows: 10% 10% 10% 10%;
  grid-column-gap:200px;
  grid-row-gap :10px; 
 //margin-left: 2px;
  
}
 ol {
    //counter-reset: li; /* Initiate a counter */
   list-style: none;  /* Remove default numbering */
   // *list-style: decimal; /* Keep using default numbering for IE6/7 */
    font: 20px 'trebuchet MS', 'lucida sans';
    padding: 0;
    margin-bottom: 4em;
    margin-left:  20px;
    text-shadow: 0 1px 0 rgba(187, 184, 184, 0.5);
  }

 
  .rounded-list span{
    position: relative;
    display: grid;
    padding: .4em .4em .4em 2em;
    *padding: .4em;
    margin: .5em 0;
    background: rgb(202, 217, 236);
    color: rgb(12, 11, 11);
    
    text-align: left;
    border-radius: .3em;
    transition: all .3s ease-out;
    box-shadow: 10px 5px 5px rgb(223, 231, 231);
    width: 230px;
  }

  .rounded-list span:hover{
    background: rgb(33, 229, 243);
     cursor: pointer;
  }

  .rounded-list span:hover:before{
    transform: rotate(360deg);
  }

  .rounded-list span:before{
    //content: counter(li);
    content: '';
    //counter-increment: li;
    position: absolute;
    left: -1.3em;
    top: 50%;
    margin-top: -1.3em;
    background: #6967f7;
    height: 2em;
    width: 2em;
    line-height: 2em;
    margin-left: 15px;
    border: .3em solid  #7cb5f5;
    text-align:left;
    font-weight: bold;
    border-radius: 2em;
    transition: all .3s ease-out;
  }
  .config-border {
    border: 1px solid #ada4a4;
    background: #FFF;
    text-color: #0000;
  }
  .font-text{
    font-size: 20px;
    
  }
  .div-back {
    background: #eae1e1;
    cursor: pointer;
    margin: 10px 0;
    display: flex;
    align-content: center;
    align-items: center;
    width: 5%;
    border-radius: 10%;
  }
  .div-back:hover {
    background: #b7b7b7;
  }
</style>