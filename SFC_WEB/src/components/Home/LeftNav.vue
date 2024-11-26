<template>
  <div class="col-md-3 left_col">
    <div class="left_col scroll-view">
      <div class="navbar nav_title" style="border: 0">
        <a href="/" class="site_title"
          ><i class="fa fa-paw"></i> <span>Shopfloor System</span></a
        >
      </div>

      <div class="clearfix"></div>

      <!-- menu profile quick info -->
      <div class="profile clearfix">
        <div class="profile_pic">
          <img
            src="assets/img/avatars/1.png"
            alt="..."
            class="img-circle profile_img"
          />
        </div>
        <div class="profile_info">
          <span>{{
            $store.state.language == "En" ? "Welcome," : "Xin chào,"
          }}</span>
          <h2>{{ empname }}</h2>
        </div>
      </div>
      <br />
      <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">
        <div class="menu_section">
          <h3>{{ $store.state.language == "En" ? "General" : "Chung" }}</h3>
          <ul class="nav side-menu">
            <li>
              <router-link to="/">
                <i class="fa fa-home"></i>Home
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>
            <li>
              <router-link to="/Home/Applications">
                <i class="fa fa-desktop"></i>
                {{
                  $store.state.language == "En" ? "Applications" : "Ứng dụng"
                }}
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>
            <li>
              <router-link to="/Home/Qualcomm_Aplication">
                <i class="fa fa-space-shuttle"></i>Qualcomm
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>
          </ul>
        </div>
      </div>
      <!-- <div class="sidebar-footer hidden-small">
        <a
          data-toggle="tooltip"
          data-placement="top"
          title="Logout"
          @click="logout"
        >
          <i class="fa fa-sign-out"></i>
        </a>
      </div> -->
    </div>
  </div>
</template>

<script>
import data from "../../data/menu_data";
export default {
  name: "LeftNav",
  data() {
    return {
      listNav: [],
      index: 0,
      index1: 0,
      index2: 9999,
      empname: "",
      databaseName: "",
    };
  },
  mounted() {
    this.listNav = data;
  },
  created() {
    for (let i = 0; i < this.listNav.length; i++) {
      for (let j = 0; j < this.listNav[i].NavList.length; j++) {
        if (
          this.listNav[i].NavList[j].route == this.$route.path &&
          this.$route.path != "/"
        ) {
          this.listNav[i].NavList[j].selected = true;
          this.index = i;
          this.index1 = j;
          this.index2 = 9999;
        } else {
          this.listNav[i].NavList[j].selected = false;
        }
      }
    }
    this.empname = localStorage.empname;
    this.databaseName = localStorage.databaseName;
  },
  methods: {
    MaximizeWindow() {
      var docElm = document.documentElement;
      if (docElm.requestFullscreen) {
        docElm.requestFullscreen();
      } else if (docElm.mozRequestFullScreen) {
        docElm.mozRequestFullScreen();
      } else if (docElm.webkitRequestFullScreen) {
        docElm.webkitRequestFullScreen();
      } else if (docElm.msRequestFullscreen) {
        docElm.msRequestFullscreen();
      }

      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitCancelFullScreen) {
        document.webkitCancelFullScreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
    },
    ChangeLanguage(lang) {
      this.$store.commit("changeLanguage", lang);
      localStorage.language = lang;
    },
    logout() {
      localStorage.username = "";
      localStorage.password = "";
      this.$router.push({ name: "Login" });
    },
    changeSelect(index, index1) {
      if (this.index2 != 9999) {
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = false;
        this.listNav[index].NavList[index1].selected = true;
      } else {
        for (let i = 0; i < this.listNav.length; i++) {
          for (let j = 0; j < this.listNav[i].NavList.length; j++) {
            this.listNav[i].NavList[j].selected = false;
          }
        }
        this.listNav[index].NavList[index1].selected = true;
      }
      this.index = index;
      this.index1 = index1;
      this.index2 = 9999;
    },
    removeAllSelect() {
      for (let i = 0; i < this.listNav.length; i++) {
        for (let j = 0; j < this.listNav[i].NavList.length; j++) {
          this.listNav[i].NavList[j].selected = false;
        }
      }
    },
    changeSubSelect(index, index1, index2) {
      if (this.index2 == 9999) {
        this.listNav[this.index].NavList[this.index1].selected = false;
        this.index = index;
        this.index1 = index1;
        this.index2 = index2;
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = true;
      } else {
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = false;
        this.index = index;
        this.index1 = index1;
        this.index2 = index2;
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = true;
      }
    },
  },
};
</script>
<style scoped lang="scss">
.left_col {
  background: #0a5381 !important;
  .nav_title {
    background: #0a5381 !important;
  }
  .sidebar-footer {
    background: #0a5381 !important;
  }
  z-index: 0;
}
.side-menu li {
  font-size: 18px;
  &:hover{
    background-color: #073e61;
  }
}
</style>