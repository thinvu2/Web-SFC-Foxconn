<template>
  <div>
    <input
      :disabled="isDisable"
      class="col-sm-12 form-control form-control-sm text-element"
      :value="keySearch"
      type="text"     
      @keyup="sendKeyUp"
      readonly
      @click="$emit('clickInput')"
      v-on:blur="$emit('lostFocus')"
    />
    <div v-show="isShowSelect" class="div-select col-sm-12 row" @focusout="handleFocusOut">
      <smooth-scrollbar class="">
        <ul class="footer-navigation">
          <li class="" v-for="(item, index) in list" :key="index">
            <a
              href="javascript:void(0)"
              @click="$emit('selectKey', item.VALUE)"
            >
              {{ item.VALUE }}
            </a>
          </li>
        </ul>
      </smooth-scrollbar>
    </div>
  </div>
</template>

<script>
export default {
  name: "SelectionSearch",
  props: {
    list: Array,
    keySearch: String,
    isShowSelect: Boolean,
    isDisable: Boolean,
  },
  data() {
    return {
      isShowModal: true,
    };
  },
  methods: {
    sendKeyUp(event) {
      console.log(event.key);
      if (event.key == "Enter") {
        this.$emit("searchChange", event.target.value);
      }
    },   
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.$emit('hide-modal');
      }
    });
  },
};
</script>

<style scoped>
.text-element{
  font-weight: bold;
  color: #000;
}
.content_main {
  margin: 10px;
}
#scroll-area {
  width: 500px;
  height: 500px;
}

#example-content {
  width: 2000px;
  height: 2000px;
}

.div-select {
  height: 200px;
  position: absolute;
  z-index: 99;
  overflow-y: auto;
}
.footer-navigation {
  list-style: none;
  padding-left: 0px;
  display: inline-block;
  width: 100%;
}
.footer-navigation li {
  width: 100%;
  box-sizing: border-box;
}
a {
  text-decoration: none;
}
.footer-navigation a {
  user-select: none;
  padding: 5px;
  color: #fff;
  display: block;
  border: 0.5px solid #fff;
  background-color: #3d6297;
}
.footer-navigation a:hover {
  text-decoration: none;
  user-select: none;
  padding: 5px;
  color: #fff;
  background-color: #38547c;
}
</style>