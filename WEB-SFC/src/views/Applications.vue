<template>
  <div class="div-content">
    <div class="div-top">{{
                    $store.state.language == "En"
                      ? "All applications"
                      : "Kho ứng dụng"
                  }}</div>
    <div class="div-app col-md-12 row">
      <template v-for="(item, index) in dataApp" :key="index">
        <div class="icon-app col-sm-2" :title="item.Name">
          <div
            class="icon-content"
            :style="item.Style"
            @click="GotoRoute(item.Route)"
          >
            <img :src="item.Image" class="image-app" alt="" />
          </div>
          <span class="text-config-name">{{ item.Name }}</span>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import dataApplications from "../data/data_applications";
export default {
  data() {
    return {
      dataApp: [],
    };
  },
  mounted() {
    this.dataApp = dataApplications;
  },
  methods: {
    GotoRoute(route) {
      this.$router.push({ path: route });
    },
  },
};
</script>

<style scoped lang="scss">
@media only screen and (hover: none) and (pointer: coarse) {
  .text-config-name {
    font-size: 12px !important;
  }
}
.text-config-name { 
  text-align: center;  
  align-content: center;
  font-size: 14px;
  font-weight: 666;
  display: flex;
  user-select: none;
  justify-content: center;
  color: #1b2b38;
  margin-top: 4px;
  margin-bottom: 10px;
}

.div-content {
  min-height: 95vh;
  background: url("../../public/assets/img/background.jpg");
  background-size: 100% auto;
  //background: #eef1ff;
}
.div-app {
  padding-top: 20px;
}
.image-app {
  height: 100%;
  width: 100%;
}
.div-top {
  font-size: 1.1rem;
  color: rgb(43, 42, 42);
  //   margin: 10px;
}
.div-main {
  padding-left: 20px;
}
.config-content {
  margin-top: 30px;
  height: 90vh;
  overflow: auto;
}
.icon-app-disable {
  height: 50px;
  width: 50px;
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
  height: 70%;
  width: 70%;
}
.icon-content {
  user-select: none;
  background: #1b2b38;
  display: flex;
  justify-content: center;
  margin: auto;
  width: 60%;
  height: 60%;
  border-radius: 20%;
  border: 3px solid #0ac9ff;
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
</style>