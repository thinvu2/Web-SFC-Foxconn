<template>
  <div class="app">
    <transition name="fade" appear>
      <div
        class="modal-overlay"
        v-if="$store.state.isShowQMLine"
        @click="$emit('hideModal')"
      ></div>
    </transition>
    <transition name="slide" appear>
      <div
        class="my_modal"
        v-if="$store.state.isShowQMLine"
        ref="draggableContainer"
        id="draggable-container"
      >
        <div
          class="content-top"
          id="draggable-header"
          @mousedown="dragMouseDown"
        >
          <label class="label-head" for="textSearch">{{
            $store.state.language == "En" ? "Search line" : "Tìm kiếm line"
          }}</label>
          <span class="closeButton" @click="HideModal()">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              width="24"
              height="24"
            >
              <path fill="none" d="M0 0h24v24H0z" />
              <path
                fill="#FFF"
                d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-11.414L9.172 7.757 7.757 9.172 10.586 12l-2.829 2.828 1.415 1.415L12 13.414l2.828 2.829 1.415-1.415L13.414 12l2.829-2.828-1.415-1.415L12 10.586z"
              />
            </svg>
          </span>
        </div>

        <div class="content-modal">
          <div class="content-text-search">
            <input
              id="textSearch"
              type="text"
              v-model="textSearch"
              @keyup="searchKeyWord"
              autocomplete="off"
              class="form-control form-control-sm"
            />
            <span
              v-if="!isHideClearButton"
              class="clearButton"
              @click="ClearTextSearch()"
              @mouseenter="MouseEnterClear()"
              @mouseleave="MouseLeaveClear()"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                width="24"
                height="24"
              >
                <path fill="none" d="M0 0h24v24H0z" />
                <path
                  :fill="colorFill"
                  d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-11.414L9.172 7.757 7.757 9.172 10.586 12l-2.829 2.828 1.415 1.415L12 13.414l2.828 2.829 1.415-1.415L13.414 12l2.829-2.828-1.415-1.415L12 10.586z"
                />
              </svg>
            </span>
          </div>
          <div class="row div-title col-12">
            <div class="col-5">
              <b>{{
                $store.state.language == "En" ? "Selected" : "Đã chọn"
              }}</b>
            </div>
            <div class="col-2"></div>
            <div class="col-5">
              <b>{{ $store.state.language == "En" ? "Avaiable" : "Có sẵn" }}</b>
            </div>
          </div>
          <div class="row div-content col-12">
            <div class="col-5 content-bellow">
              <smooth-scrollbar>
                <ul class="select-model">
                  <li
                    @click="MoveAtoB(item)"
                    v-for="(item, index) in $store.state.listSelectDualLine"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </ul>
              </smooth-scrollbar>
            </div>
            <div class="col-2 div-center-move">
              <div @click="MoveAllAToB()" class="div-content-button-move">
                <a href="javascript:void(0)"
                  ><Icon icon="angle-double-right"
                /></a>
              </div>
              <div class="div-content-button-move">
                <Icon icon="exchange-alt" class="sidenav-icon" />
              </div>
              <div @click="MoveAllBToA()" class="div-content-button-move">
                <a href="javascript:void(0)"
                  ><Icon icon="angle-double-left"
                /></a>
              </div>
            </div>
            <div class="col-5 content-bellow">
              <smooth-scrollbar>
                <ul class="select-model">
                  <li
                    @mousemove="move"
                    @click="MoveBtoA(item)"
                    v-for="(item, index) in $store.state
                      .listAvaiDualLineSearch"
                    :key="index"
                  >
                    {{ item.VALUE }}
                  </li>
                </ul>
              </smooth-scrollbar>
            </div>
          </div>
        </div>
      </div></transition
    >
  </div>
</template>

<script>
import $ from "jquery";

export default {
  props: {},
  components: {},
  data() {
    return {
      textSearch: "",
      showModal: false,
      listSelectLine: [],
      positions: {
        clientX: undefined,
        clientY: undefined,
        movementX: 0,
        movementY: 0,
      },
      colorFill: "#969696",
      isHideClearButton: true,
    };
  },
  methods: {
    HideModal() {
      this.textSearch = "";
      this.isHideClearButton = true;
      this.colorFill = "#969696";
      this.$emit("hideModal");
    },
    ClearTextSearch() {
      this.textSearch = "";
      this.searchKeyWord();
      $("#textSearch").focus();
    },
    MouseEnterClear() {
      this.colorFill = "#0085f2";
    },
    MouseLeaveClear() {
      this.colorFill = "#969696";
    },
    dragMouseDown: function (event) {
      event.preventDefault();
      // get the mouse cursor position at startup:
      this.positions.clientX = event.clientX;
      this.positions.clientY = event.clientY;
      document.onmousemove = this.elementDrag;
      document.onmouseup = this.closeDragElement;
      //console.log(`move`);
    },
    elementDrag: function (event) {
      event.preventDefault();
      this.positions.movementX = this.positions.clientX - event.clientX;
      this.positions.movementY = this.positions.clientY - event.clientY;
      this.positions.clientX = event.clientX;
      this.positions.clientY = event.clientY;
      // set the element's new position:
      this.$refs.draggableContainer.style.top =
        this.$refs.draggableContainer.offsetTop -
        this.positions.movementY +
        "px";
      this.$refs.draggableContainer.style.left =
        this.$refs.draggableContainer.offsetLeft -
        this.positions.movementX +
        "px";
    },
    closeDragElement() {
      document.onmouseup = null;
      document.onmousemove = null;
    },
    searchKeyWord() {
      if (this.textSearch.trim().length == 0) {
        this.isHideClearButton = true;
        this.$store.commit("QMListSearchEqualAvaiLine");
      } else {
        this.isHideClearButton = false;
        this.$store.commit(
          "QMFilterLine",
          this.textSearch.trim().toUpperCase()
        );
      }
    },
    MoveAllBToA() {
      this.$store.commit("moveAllListAvaiToSelectLine");
    },
    MoveAllAToB() {
      this.$store.commit("moveAllListSelectToAvaiLine");
    },
    MoveAtoB(item) {
      this.$store.commit("moveItemToAvaiLine", item);
    },
    MoveBtoA(item) {
      this.$store.commit("moveItemToSelectLine", item);
    },
  },
};
</script>
<style lang="scss" scoped>
@media only screen and (hover: none) and (pointer: coarse) {
  .label-head {
    font-size: 10px !important;
  }
  .select-model li {
    font-size: 10px !important;
  }
  .my_modal {
    width: 60% !important;
    height: 90% !important;
    // top: 10% !important;
    // bottom: 60% !important;
    // left: 50% !important;
    // transform: translate(-50%, -50%) !important;
  }
}
.label-head {
  color: #fff;
  margin: 2px;
  font-size: 13px;
  font-weight: 400;
}

.select-model {
  height: 50px;
}
.select-model li {
  border: 1px solid #cecece;
  padding: 5px;
  font-size: 13px;
  font-weight: 400;
  color: #000;
  &:hover {
    background: #3d6297;
    color: #fff;
  }
}

.content-text-search {
  position: relative;
  margin: 5px;
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
// .content-modal {
//   margin: 20px;
// }
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

.content-bellow {
  padding-bottom: 5px;
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
  z-index: 99;
  width: 30%;
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