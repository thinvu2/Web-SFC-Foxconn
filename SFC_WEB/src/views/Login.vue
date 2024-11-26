<template>
  <div class="div-all">
    <div class="div-portrait">
      <div class="div-portrait-center">
        <svg
          id="pleaserotate-graphic-login"
          xmlns:xlink="http://www.w3.org/1999/xlink"
          viewBox="0 0 250 250"
        >
          <g transform="rotate(0 125 125)">
            <path
              fill="#fffe"
              d="M190.5,221.3c0,8.3-6.8,15-15,15H80.2c-8.3,0-15-6.8-15-15V28.7c0-8.3,6.8-15,15-15h95.3c8.3,0,15,6.8,15,15V221.3zM74.4,33.5l-0.1,139.2c0,8.3,0,17.9,0,21.5c0,3.6,0,6.9,0,7.3c0,0.5,0.2,0.8,0.4,0.8s7.2,0,15.4,0h75.6c8.3,0,15.1,0,15.2,0s0.2-6.8,0.2-15V33.5c0-2.6-1-5-2.6-6.5c-1.3-1.3-3-2.1-4.9-2.1H81.9c-2.7,0-5,1.6-6.3,4C74.9,30.2,74.4,31.8,74.4,33.5zM127.7,207c-5.4,0-9.8,5.1-9.8,11.3s4.4,11.3,9.8,11.3s9.8-5.1,9.8-11.3S133.2,207,127.7,207z"
            ></path>
          </g>
        </svg>
      </div>
    </div>
    <div class="login-app">
      <body class="img js-fullheight">
        <section class="ftco-section">
          <div class="container">
            <div class="animation-wrapper" style="z-index: -1 !important">
              <div class="particle particle-1"></div>
              <div class="particle particle-2"></div>
              <div class="particle particle-3"></div>
              <div class="particle particle-4"></div>
            </div>
            <div class="row justify-content-center">
              <div class="col-md-12 text-center mb-5">
                <p
                  href="#"
                  class="intro-banner-vdo-play-btn pinkBg"
                  target="_blank"
                >
                  <i
                    class="glyphicon glyphicon-play whiteText"
                    aria-hidden="true"
                  ></i>
                  <span class="ripple pinkBg"></span>
                  <span class="ripple pinkBg"></span>
                </p>
              </div>
              <div class="col-md-12 text-center">
                <span class="heading-section">
                  {{
                    $store.state.language == "En"
                      ? "Welcome to Smart Shopfloor System"
                      : "Chào mừng đến hệ thống Shopfloor"
                  }}
                </span>
              </div>
            </div>
            <div class="row justify-content-center">
              <div class="col-md-6 col-lg-4">
                <div class="login-wrap p-0">
                  <h3 class="mb-4 text-center"></h3>
                  <form @submit.prevent="submitLogin">
                    <div class="form-group">
                      <select v-model="database" class="select-database" id="">
                        <option disabled value="">
                          {{
                            $store.state.language == "En"
                              ? "Please select one"
                              : "Chọn database"
                          }}
                        </option>
                        <option
                          v-for="(item, index) in databaseList"
                          :key="index"
                        >
                          {{ item.value }}
                        </option>
                      </select>
                    </div>
                    <div class="form-group">
                      <input
                        v-model="username"
                        type="text"
                        class="form-control"
                        :placeholder="
                          $store.state.language == 'En'
                            ? 'Username'
                            : 'Tài khoản'
                        "
                        required
                      />
                    </div>
                    <div class="form-group">
                      <input
                        v-model="password"
                        id="password-field"
                        type="password"
                        class="form-control"
                        :placeholder="
                          $store.state.language == 'En'
                            ? 'Password'
                            : 'Mật khẩu'
                        "
                        required
                      />
                    </div>
                    <div class="form-group">
                      <button
                        type="submit"
                        class="form-control btn btn-primary submit px-3"
                      >
                        {{
                          $store.state.language == "En"
                            ? "Sign In"
                            : "Đăng nhập"
                        }}
                      </button>
                    </div>
                  </form>
                  <p class="text-alert">{{ state_alert }}</p>
                </div>
              </div>
            </div>
          </div>
          <div class="footer">
            <span class="copyright">
              {{
                $store.state.language == "En"
                  ? "© Copyright 2021 - Copyright of IT Shopfloor VN"
                  : "© Bản quyền 2021 - Bản quyền thuộc về IT Shopfloor VN"
              }}
            </span>
            <div class="select-language">
              <template v-if="$store.state.language == 'Vi'">
                <a
                  @click="changeLanguage('En')"
                  href="javascript:void(0)"
                  class="text-lang"
                >
                  <img
                    src="../assets/img/vietnam_96px.png"
                    class="image-language"
                    alt=""
                  />
                  &nbsp; Tiếng Việt</a
                >
              </template>
              <template v-else>
                <a
                  @click="changeLanguage('Vi')"
                  href="javascript:void(0)"
                  class="text-lang"
                >
                  <img
                    src="../assets/img/great_britain_96px.png"
                    class="image-language"
                    alt=""
                  />
                  &nbsp; English</a
                >
              </template>
            </div>
          </div>
        </section>
      </body>
    </div>
  </div>
</template>
 
<script>
import Repository from "../services/Repository";

export default {
  name: "Login",
  data() {
    return {
      database: "NIC",
      databaseList: [
        { text: "NIC", value: "NIC" },
        { text: "UBEE", value: "UBEE" },
        { text: "TEST", value: "TEST" },
      ],
      username: "",
      password: "",
      state_alert: "",
      copyright: "",
      language: "",
    };
  },
  methods: {
    changeLanguage(lang) {
      this.$store.commit("changeLanguage", lang);
      localStorage.language = lang;
      console.log(lang);
    },
    async submitLogin() {
      if (typeof this.database == "undefined" || this.database.length == 0) {
        if (this.$store.state.language == "En") {
          this.state_alert = "You must choose database";
        } else {
          this.state_alert = "Bạn phải chọn database";
        }
      } else {
        var payload = {
          DatabaseName: this.database,
          UserName: this.username,
          PassWord: this.password,
        };
        var { data } = await Repository.getRepo("CheckLogin", payload);
        if (data.result == "ok") {
          localStorage.databaseName = this.database;
          localStorage.username = this.username;
          localStorage.password = this.password;
          localStorage.empname = data.emp_name;
          // if (window.location.protocol == "http:") {
          //   window.location.href = window.location.origin + "/#/Home/Applications/";
          // } else {
            window.location.href = window.location.origin + "/#/Home/Applications/";
          // }
        } else
        {
          if (this.$store.state.language == "En") {
            this.state_alert = "Wrong username or password";
          } else {
            this.state_alert = "Sai tài khoản hoặc mật khẩu";
          }
        }
      }
    },
  },
  created() {
    document.title = "Login";
    if (typeof localStorage.databaseName != "undefined") {
      if (localStorage.databaseName.length != 0) {
        this.database = localStorage.databaseName;
      }
    }
    if (
      typeof localStorage.username == "undefined" ||
      typeof localStorage.password == "undefined"
    ) 
    {
      this.$router.push({ name: "Login" });
    } else {
      if (localStorage.username.length == 0) {
        this.$router.push({ name: "Login" });
      } else {
        // window.location.href =  window.location.origin+"/sfc";
        if (window.location.protocol == "http:") {
          window.location.href = window.location.origin + "/sfc";
        } else {
          window.location.href = window.location.origin;
        }
      }
    }
    if (typeof localStorage.language == "undefined") {
      localStorage.language = this.$store.state.language = this.language = "En";
    } else {
      if (localStorage.language == "En") {
        localStorage.language =
          this.$store.state.language =
          this.language =
            "En";
      } else {
        localStorage.language =
          this.$store.state.language =
          this.language =
            "Vi";
      }
    }
  },
  mounted() {
    // var host = window.location.protocol + "//" + window.location.host;
    // console.log(host);
  },
};
</script>

<style scoped src="../assets/template/login/css/style.css">
</style>
<style scoped lang="scss"  src="../assets/template/login/css/animation.scss">
</style>
<style lang="scss" scoped>
.ftco-section {
  height: 100vh;
  overflow: hidden;
}
#pleaserotate-graphic-login {
  width: 20%;
  top: 30%;
  left: 40%;
  position: absolute;
  transform-origin: 50% 50% 0px;
  animation-name: rotate;
  animation-duration: 2s;
  animation-iteration-count: infinite;
  transition-timing-function: linear;
}
@keyframes rotate {
  0% {
    -webkit-transform: rotate(0deg);
  }
  40% {
    -webkit-transform: rotate(180deg);
  }
}
.div-portrait {
  position: absolute;
  height: 100%;
  width: 100%;
  z-index: 1000 !important;
  background: #000;
  display: none;
  //display: flex;
  align-content: center;
  justify-content: center;
  align-items: center;
}

@media only screen and (hover: none) and (pointer: coarse) and (orientation: landscape) {
  .div-portrait {
    display: block;
  }
}

.image-language {
  height: 20px;
  width: 20px;
}
.heading-section {
  z-index: 1 !important;
  font-size: 200%;
}
.js-fullheight {
  max-height: 100vh;
}
.footer {
  position: absolute;
  display: flex;
  bottom: 0;
  width: 100%;
  justify-content: space-between;
}
.copyright {
  color: #fff;
  font-size: 14px;
  // position: fixed;
  // left: 40%;
  // bottom: 15px;
}
.text-lang {
  color: #fff !important;
}
.select-language {
  // background: #024873;
  color: #fff;
  margin: 10px;
  padding: 0px 20px;
  border-radius: 10%;

  // position: fixed;
  // right: 20px;
  // bottom: 10px;
}
.text-alert {
  text-align: center;
  color: red;
}
.img {
  background-image: url("../assets/img/myBg.jpg");
  height: 100vh;
}
.select-database {
  option {
    background-color: #024873;
  }
  box-sizing: border-box;
  margin: 0;
  font-family: inherit;
  overflow: visible;
  display: block;
  width: 100%;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  font-weight: 400;
  line-height: 1.5;
  height: 50px;
  color: white !important;
  border: 1px solid transparent;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 40px;
  padding-left: 20px;
  padding-right: 20px;
  transition: 0.3s;
}
select option {
  margin: 40px;
  background: rgba(0, 0, 0, 0.3);
  color: #fff;
  text-shadow: 0 1px 0 rgba(0, 0, 0, 0.4);
}
</style>