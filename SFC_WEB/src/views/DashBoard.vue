<template>
  <div class="div-content">
    <div class="col-md-6 col-sm-5 contact-sec-1 div-contact">
      <ul class="contact-form">
        <li>
          <h5 class="text-white">CONTACT IN<span>FO</span></h5>
        </li>
        <li>
          <h6 class="text-white">
            <i class="fa fa-location-arrow pull-left"></i>
            <strong>Address:</strong> IT Shopfloor Vietnam
          </h6>
        </li>
        <li>
          <h6 class="text-white">
            <i class="fa fa-envelope pull-left"></i>
            <strong>Mail Us:</strong>
            <a href="javascript:void(0)">cpe-vngz-nic-sfc@mail.foxconn.com</a>
          </h6>
        </li>
        <li>
          <h6 class="text-white">
            <i class="fa fa-phone pull-left"></i><strong>Phone:</strong> (535)
            31529
          </h6>
        </li>
        <li>
          <h6 class="text-white">
            <i class="fa fa-globe pull-left"></i>
            <strong>Website:</strong>
            <a href="https://sfcnic-cns.myfiinet.com/">https://sfcnic-cns.myfiinet.com/</a>
          </h6>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import dataApplications from "../data/data_applications";
export default {
  name: "DashBoard",
  data() {
    return {
      dataApp: [],
    };
  },
  mounted() {
    document.title = "Welcome";
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
.text-white {
  color: #fff;
  a {
    color: antiquewhite;
  }
}
.img-contact {
  height: 30%;
  width: 30%;
  position: absolute;
  top: 50%;
  left: 50%;
}
ul {
  list-style: none;
  li {
    margin: 8px;
  }
}
.div-contact {
  position: absolute;
  bottom: 20px;
}
.div-content {
  min-height: 95vh;
  background: url("../../public/assets/img/BG_1.jpg");
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
.text-config-name {
  //font-family: "Roboto", sans-serif;
  text-align: center;
  //border: 1px solid #cdc;
  align-content: center;
  font-size: 90%;
  font-weight: 666;
  display: flex;
  user-select: none;
  justify-content: center;
  color: #1b2b38;
  margin-top: 4px;
  margin-bottom: 10px;
}
</style>