<template>
  <div class="app">
    <div class="top_nav">
      <div class="nav_menu">
        <div class="nav toggle">
          <a id="menu_toggle" @click="MenuToggle()"
            ><i class="fa fa-bars" style="color: #fff"></i
          ></a>
        </div>
        <nav class="nav navbar-nav">
          <ul class="navbar-right">
            <li
              class="nav-item dropdown open dropleft"
              style="padding-left: 15px"
            >
              <a
                href="javascript:;"
                class="user-profile dropdown-toggle"
                aria-haspopup="true"
                id="navbarDropdown"
                data-toggle="dropdown"
                aria-expanded="false"
                style="color: #fff !important"
              >
                <img src="assets/img/avatars/1.png" alt="" />{{ empno }}(
                {{ databaseName }} )
              </a>
              <div
                class="dropdown-menu dropdown-usermenu pull-right"
                aria-labelledby="navbarDropdown"
              >
                <!-- <a
                  class="dropdown-item"
                  href="javascript:;"
                  @click="ChangeLanguage()"
                >
                  <template v-if="$store.state.language == 'En'">
                    <img
                      src="../../assets/img/vietnam_96px.png"
                      class="image-language pull-right"
                      alt=""
                    />
                    <span>Tiếng Việt</span>
                  </template>
                  <template v-else>
                    <img
                      src="../../assets/img/great_britain_96px.png"
                      class="image-language pull-right"
                      alt=""
                    />
                    <span>English</span>
                  </template>
                </a> -->
                <!-- <a class="dropdown-item" href="javascript:;">Help</a> -->
                <!-- <a
                  href="javascript:void(0)"
                  @click="GetSOP()"                
                  class="dropdown-item"
                  ><i class="text-primary fa fa-file pull-right"></i>
                  {{ $store.state.language == "En" ? "SOP" : "SOP" }}
                </a> -->
                <a
                  class="dropdown-item"
                  href="javascript:void(0)"
                  @click="logout"
                  ><i class="fa fa-sign-out pull-right"></i>
                  {{
                    $store.state.language == "En" ? "Logout" : "Đăng xuất"
                  }}</a
                >
              </div>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  </div>

</template>

<script>
import $ from "jquery";
export default {
  name: "TopNav",
  data() {
    return {
      empname: "",
      empno: "",
      databaseName: "",
    };
  },
  methods: {
    GetSOP(){
      window.open(window.location.origin+window.location.pathname+"/file/upload/SFC-WEB-book_guide.pdf");
    },
    MenuToggle() {
      if ($("body").attr("class") == "nav-md") {
        $("body").removeClass("nav-md");
        $("body").addClass("nav-sm");
      } else {
        $("body").removeClass("nav-sm");
        $("body").addClass("nav-md");
      }
    },
    logout() {
      localStorage.username = "";
      localStorage.password = "";
      this.$router.push({ name: "Login" });
    },
    ChangeLanguage() {
      var language = "";
      if (this.$store.state.language == "En") {
        language = "Vi";
      } else {
        language = "En";
      }
      this.$store.commit("changeLanguage", language);
      localStorage.language = language;
    },
  },
  created() {
    this.empname = localStorage.empname;
    this.empno = localStorage.username;
    this.databaseName = localStorage.databaseName;
  },
};
</script>

<style lang="scss" scoped>
.nav_menu {
  background: #0b364f !important;
}
.image-language {
  height: 20px;
  width: 20px;
}
.layout-navbar {
  height: 40px !important;
}
.nav-top {
  background-image: linear-gradient(to right, #024873, #024873) !important;
  color: #fffbf7 !important;
}
.nav-link {
  i {
    color: #fffbf7;
    span {
      color: #fffbf7;
    }
  }
}
.app-brand-text {
  color: #fffbf7 !important;
}
a > span > span {
  color: #fffbf7;
}
#layout-navbar-collapse > div > div.demo-navbar-user.nav-item.dropdown > a {
  color: #fffbf7;
}
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
  text-align: center;
}
</style>
<style src="../../../public/template/vendors/bootstrap/dist/css/bootstrap.min.css"></style>
<style src="../../../public/template/vendors/font-awesome/css/font-awesome.min.css"></style>
<style src="../../../public/template/vendors/nprogress/nprogress.css"></style>
<style src="../../../public/template/vendors/iCheck/skins/flat/green.css"></style>
<style src="../../../public/template/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css"></style>
<style src="../../../public/template/vendors/jqvmap/dist/jqvmap.min.css"></style>
<style src="../../../public/template/vendors/bootstrap-daterangepicker/daterangepicker.css"></style>
<style src="../../../public/template/build/css/custom.min.css"></style>