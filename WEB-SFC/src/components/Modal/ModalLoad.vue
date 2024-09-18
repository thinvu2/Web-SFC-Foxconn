<template>
  <div class="modal-load">
    <transition name="fade" appear>
      <div class="modal-overlay"></div>
    </transition>
    <transition name="slide" appear>
      <div class="my_modal" ref="draggableContainer" id="draggable-container">
        <div class="main">
          <span class="text-wait">
            {{
              $store.state.language == "En"
                ? "Wait a moment ♥..."
                : "Chờ xíu nha ♥ ..."
            }}</span
          >
          <div class="circle"></div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
export default {
  methods: {
    // hideModal() {
    //   this.$store.commit("closeModal");
    //   $("body").removeClass("modal-open");
    // },
  },
  mounted() {},
};
</script>

<style lang="scss" scoped>
body {
  text-align: center;
}
.text-wait {
  color: #fff;
  position: absolute;
  font-size: 19px;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  margin: 0;
}
.main {
  width: 330px;
  height: 330px;
  border: 1px solid #ccc;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  border-radius: 50%;
}
.circle {
  width: 30px;
  height: 30px;
  background: cyan;
  border-radius: 50%;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  overflow: hidden;
  margin: auto;
  animation: circle 2s linear infinite;
}

@keyframes circle {
  0% {
    transform: rotate(0deg) translate(-165px) rotate(0deg);
  }
  100% {
    transform: rotate(360deg) translate(-165px) rotate(-360deg);
  }
}

.label-head {
  color: #fff;
  margin: 2px;
  font-size: 100%;
  font-family: "Courier New", monospace;
  font-weight: 400;
}

.content-text-search {
  position: relative;
  #textSearch {
    background: #f2f2f2;
    border-radius: 15px;
    pseudo-class {
      background: #fff;
    }
  }

  .clearButton {
    position: absolute;
    right: 5px;
    top: 2px;
    &:hover {
      cursor: pointer;
    }
  }
}

.content-top {
  position: relative;
  //top: 0;
  height: 30px;
  background: #226b96;
  // padding: 5px;
  display: flex;
  border-radius: 10px 10px 0 0;
  justify-content: space-between;
}
.closeButton {
  height: 30px;
  z-index: 99;
  margin-top: 0px;
  margin-right: 5px;
  &:hover {
    cursor: pointer;
  }
}
.content-modal {
  margin: 20px;
}
.sidenav-icon {
  font-size: 25px;
  color: #000;
}
.div-center-move {
  //padding: 0 -10px;
  vertical-align: middle;
}
.div-content-button-move {
  margin: 40px 0px;
  text-align: center;
  a {
    color: #0a7954;
    text-decoration: none;
    font-size: 25px;
  }
}
a {
  text-decoration: none;
}
.select-model {
  height: 50px;
}
.select-model li {
  border: 1px solid #cdc;
  padding: 5px;
  font-size: 15px;
  font-weight: 400;
  &:hover {
    background: #3d6297;
    color: #fff;
  }
}
.content-bellow {
  // border: 1px solid #bababa;
  // height: 200px;
  overflow: auto;
}
ul li {
  list-style: none;
  border: 1px solid #bababa;
  &:hover {
    cursor: pointer;
  }
  user-select: none;
}
.div-title {
  margin-top: 10px;
}
.div-content {
  height: 100%;
}
.img-excel {
  height: 45px;
  width: 70px;
  &:hover {
    cursor: pointer;
  }
}

.count-number {
  color: red;
}

.div-table {
  width: 92vw;
  height: 80vh;
}
thead {
  background: #418bca;
  tr {
    th {
      color: #fff;
    }
  }
}
.table-condensed {
  font-size: 12px;
  margin-top: 10px;
}
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: "montserrat", sans-serif;
}

#app {
  position: absolute;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
  min-height: 100vh;
  overflow-x: hidden;
}
.button {
  appearance: none;
  border: none;
  background: none;
  cursor: pointer;
  display: inModel-block;
  padding: 15px 25px;
  background-image: Modelar-gradient(to right, #cc2e5d, #ff5858);
  border-radius: 8px;
  color: #fff;
  font-size: 18px;
  font-weight: 700;

  box-shadow: 3px 3px rgba(0, 0, 0, 0.4);
  transition: 0.4s ease-out;

  &:hover {
    box-shadow: 6px 6px rgba(0, 0, 0, 0.6);
  }
}

.modal-overlay {
  position: fixed;
  z-index: 99990 !important;
  height: 100vh;
  width: 100vw;
  // position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 99;
  background-color: rgba(0, 0, 0, 0.3);
}

.my_modal {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 99991 !important;
  width: 40%;
  //max-width: 35vw;
  background-color: #fff;
  border-radius: 16px;
  // padding: 25px;
  h1 {
    color: #222;
    font-size: 32px;
    font-weight: 900;
    margin-bottom: 15px;
  }

  p {
    color: #666;
    font-size: 18px;
    font-weight: 400;
    margin-bottom: 15px;
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.slide-enter-active,
.slide-leave-active {
  transition: transform 0.5s;
}

.slide-enter,
.slide-leave-to {
  transform: translateY(-50%) translateX(100vw);
}
</style>