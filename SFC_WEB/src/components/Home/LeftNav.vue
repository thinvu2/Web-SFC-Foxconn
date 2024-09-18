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
      <!-- /menu profile quick info -->

      <br />

      <!-- sidebar menu -->
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
          </ul>
        </div>
      </div>
      <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">
        <div class="menu_section">
          <ul class="nav side-menu">
            <li>
              <a @click="logout()"
                ><i class="fa fa-sign-out"></i
                >{{ $store.state.language == "En" ? "Logout" : "Đăng xuất" }}
                <span class="fa fa-chevron-right"></span
              ></a>
              <ul class="nav child_menu">
                <li><a href="fixed_sidebar.html">Fixed Sidebar</a></li>
                <li><a href="fixed_footer.html">Fixed Footer</a></li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
      <!-- /sidebar menu -->
      <!-- /menu footer buttons -->
      <div class="sidebar-footer hidden-small">
        <a data-toggle="tooltip" data-placement="top" title="Setting">
          <i class="fa fa-gear"></i>
        </a>
        <a
          data-toggle="tooltip"
          data-placement="top"
          title="FullScreen"
          @click="MaximizeWindow()"
        >
          <i class="fa fa-expand"></i>
        </a>
        <a data-toggle="tooltip" data-placement="top" title="Lock">
          <i class="fa fa-lock"></i>
        </a>
        <a
          data-toggle="tooltip"
          data-placement="top"
          title="Logout"
          @click="logout"
        >
          <i class="fa fa-sign-out"></i>
        </a>
      </div>
      <!-- /menu footer buttons -->
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
    // console.log(this.$route.path+"--"+this.$route.path.length);
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

      // this.listNav[index].NavList[index1].NavList[index2].selected = true;
    },
  },
};
</script>
<style scoped lang="scss">
.left_col {
  background: #024873 !important;
  .nav_title {
    background: #0b364f !important;
  }
  .sidebar-footer {
    background: #0b364f !important;
  }
  z-index: 0;
}
.image-language {
  height: 25px;
  width: 25px;
}
.selected-link {
  background: #e85f04;
}
.sidenav_text {
  font-size: 13px;
}
.dropdown-toggle::after {
  color: #ffff;
  position: absolute !important;
  right: 20px;
  font-size: 25px;
  /* margin-left: 100px !important; */
}
#layout-sidenav {
  height: 100%;
  background-image: linear-gradient(to right, #024873, #024873) !important;
}
.sidenav-item a:hover {
  background: #d88856;
}
/* #layout-sidenav {
 
  height: auto;
  /* position: relative;  
    height: 100vh;
    overflow-y: auto; */

.left-menu {
  background-image: linear-gradient(to right, #024873, #024873) !important;
  color: #fffbf7 !important;
  &:hover {
    color: #1f1f1f;
  }
  height: 100%;
}
.left-menu li a div {
  color: #fffbf7;
}
.left-menu li a i {
  color: #fffbf7;
}
.left-menu li a svg {
  color: #fffbf7;
}
.sidenav-divider {
  background: #fffbf7;
}
.sidenav-header {
  font-size: 12px !important;
  color: #fffbf7 !important;
}
.sidenav-toggle::after {
  color: #fffbf7 !important;
}
/* .sidenav-toggle{
  color: #FFF !important;
  background: #FFF;
} */
</style>
<style src="../../../public/template/vendors/bootstrap/dist/css/bootstrap.min.css"></style>
<style src="../../../public/template/vendors/font-awesome/css/font-awesome.min.css"></style>
<style src="../../../public/template/vendors/nprogress/nprogress.css"></style>
<style src="../../../public/template/vendors/iCheck/skins/flat/green.css"></style>
<style src="../../../public/template/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css"></style>
<style src="../../../public/template/vendors/jqvmap/dist/jqvmap.min.css"></style>
<style src="../../../public/template/vendors/bootstrap-daterangepicker/daterangepicker.css"></style>
<style src="../../../public/template/build/css/custom.min.css"></style>