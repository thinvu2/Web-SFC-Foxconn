<template>
  <div class="">
    <div class="row config-content">
      <template v-for="(item, index) in dataList" :key="index">
        <div class="icon-app-disable col-sm-2" v-if="item.IsDisable == true">
          <div class="icon-content">
            <span>{{ item.Id }}</span>
          </div>
          <span class="text-config-name">{{ item.Name }}</span>
        </div>
        <div v-else class="icon-app col-sm-2" @click="GotoRoute(item.Route)">
          <template v-if="item.Route != ''">
            <div class="icon-content">
              <span>{{ item.Id }}</span>
            </div>
            <span class="text-config-name">{{ item.Name }}</span>
          </template>
          <template v-else>
            <div class="icon-content">
              <!-- <span>{{ item.Id }}</span> -->
            </div>
            <!-- <span class="text-config-name">{{ item.Name }}</span> -->
          </template>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import dataConfig from "../data/data_config1";
import Repository from "../services/Repository";

export default {
  data() {
    return {
      dataList: [],
      dataCheckPrivilege: [],
    };
  },
  created() {
    document.title = "Config Application";
    this.dataList = dataConfig;
  },
  mounted() {
    this.CheckAllPrivilege();
  },
  methods: {
    GotoRoute(route) {
      this.$router.push({ path: route });
    },
    async CheckAllPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
      };
      var { data } = await Repository.getRepo("GetConfigPrivilge", payload);
      this.dataCheckPrivilege = data.data;
      this.dataList.forEach((element) => {
        element.IsDisable = true;
        if (typeof this.dataCheckPrivilege != "undefined") {
          if (this.dataCheckPrivilege.length != 0) {
            if (element.ReplaceName != "") {
              if (
                this.dataCheckPrivilege.some(
                  (p) => p.FUN == element.ReplaceName
                )
              ) {
                element.IsDisable = false;
              } else {
                element.IsDisable = true;
              }
            } else {
              if (this.dataCheckPrivilege.some((p) => p.FUN == element.Name)) {
                element.IsDisable = false;
              } else {
                element.IsDisable = true;
              }
            }
          }
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.div-main {
  padding-left: 20px;
  
}
.config-content {
  //margin-top: 30px;  
  height: 90vh;
  overflow: auto;
  background: #9fc6f2;
}
.icon-app-disable {
  height: 100px;
  width: 100px;
  .icon-content {
    background: #dddddd !important;
    border: 5px solid #737677;
    color: #737677 !important;
    &:hover {
      border: 5px solid #737677;
      cursor: default;
    }
    span {
      // color: #000;
      color: #737677;
      margin: auto;
      text-align: center;
      font-size: 20px;
      font-weight: 555;
      justify-content: center;
    }
  }
  .text-config-name-disable {
    color: #737677 !important;
  }
}
.icon-app {
  height: 100px;
  width: 100px;
}
.icon-content {
  user-select: none;
  background: #1b2b38;
  display: flex;
  justify-content: center;
  margin: auto;
  width: 70px;
  height: 70px;
  border-radius: 50%;
  border: 3px solid #057999;
  span {
    // color: #000;
    color: #0ac9ff;
    margin: auto;
    text-align: center;
    font-size: 20px;
    font-weight: 555;
    justify-content: center;
  }
  &:hover {
    border: 5px solid #0ac9ff;
    cursor: pointer;
    span {
      font-size: 25px;
    }
  }
}
.text-config-name {
  font-family: "Roboto", sans-serif;
  text-align: center;
  font-size: 12px;
  font-weight: 666;
  display: flex;
  user-select: none;
  justify-content: center;
  color: #1b2b38;
}
</style>