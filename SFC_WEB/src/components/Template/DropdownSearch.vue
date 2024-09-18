<template>
  <section class="dropdown-wrapper">
    <div @click="isVisible = !isVisible" class="selected-item">
      <span>{{ textContent }}</span>
      <svg
        :class="isVisible ? 'dropdown' : ''"
        class="drop-down-icon"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        width="24"
        height="24"
      >
        <path fill="none" d="M0 0h24v24H0z" />
        <path
          d="M12 13.172l4.95-4.95 1.414 1.414L12 16 5.636 9.636 7.05 8.222z"
        />
      </svg>
    </div>
    <template v-if="type == 'model'">
      <div :class="isVisible ? 'visible' : ''" class="row dropdown-popover">
        <input
          type="text"
          v-model="searchText"
          :placeholder="textPlaceHolder"
        />
        <div class="options">
          <ul>
            <li
              v-for="(item, index) in filterUser"
              :key="index"
              @click="SetTextDropDown(item)"
              :value="item"
            >
              {{ item }}
            </li>
          </ul>
        </div>
      </div>
    </template>
    <template v-if="type == 'customer'">
      <div :class="isVisible ? 'visible' : ''" class="row dropdown-popover">
        <input
          type="text"
          v-model="searchText"
          :placeholder="textPlaceHolder"
        />
        <div class="options">
          <ul>
            <li
              v-for="(item, index) in filterUser"
              :key="index"
              @click="SetTextDropDown(item)"
              :value="item.CUST_CODE"
            >
              {{ item.CUSTOMER }}
            </li>
          </ul>
        </div>
      </div>
    </template>
    <template v-if="type == 'route'">
      <div :class="isVisible ? 'visible' : ''" class="row dropdown-popover">
        <input
          type="text"
          v-model="searchText"
          :placeholder="textPlaceHolder"
        />
        <div class="options">
          <ul>
            <li
              v-for="(item, index) in filterUser"
              :key="index"
              @click="SetTextDropDown(item)"
              :value="item.ROUTE_CODE"
            >
              {{ item.ROUTE_NAME + ` _ (${item.ROUTE_CODE})` }}
            </li>
          </ul>
        </div>
      </div>
    </template>
  </section>
</template>

<script>
export default {
  props: ["listAll", "textContent", "textPlaceHolder", "type"],
  data() {
    return {
      isVisible: false,
      searchText: "",
    };
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.searchText = "";
        this.isVisible = false;
      }
    });
  },
  computed: {
    filterUser: function () {
      const query = this.searchText.toLowerCase();
      if (this.searchText === "") {
        return this.listAll;
      }
      if (this.type == "route") {
        return this.listAll.filter((user) => {
          return String(user.ROUTE_NAME+user.ROUTE_CODE).toLowerCase().includes(query);
        });
      }else if(this.type == "customer"){
          console.log(this.listAll);
          return this.listAll.filter((user) => {
          return String(user.CUSTOMER+user.CUST_CODE).toLowerCase().includes(query);
        });
      } else {
        return this.listAll.filter((user) => {
          return String(user).toLowerCase().includes(query);
        });
      }
    },
  },
  mounted() {},
  methods: {
    SetTextDropDown(text) {
      this.$emit("update-selected-item", text);
      this.isVisible = false;
    },
  },
};
</script>

<style lang="scss" scoped>
.options {
  ul {
    li {
      font-size: 15px;
    }
  }
}
.dropdown-wrapper {
  z-index: 2;
}
.selected-item {
  height: 100%;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
  font-weight: 555;
  display: flex;
  .drop-down-icon {
    transform: rotate(180deg);
    transition: all 0.2s ease;
    &.dropdown {
      transform: rotate(0deg);
    }
  }
}
.dropdown-popover {
  z-index: 1;
  position: sticky;
  border: 2px solid lightgray;
  top: 46px;
  left: 18px;
  right: 0;
  background: #fff;
  width: 100%;
  max-width: 100%;
  padding: 5px;
  visibility: hidden;
  transition: all 0.1s linear;
  overflow: hidden;
  &.visible {
    visibility: visible;
  }
  input {
    width: 100%;
    height: 30px;
    border: 2px solid lightgray;
    font-size: 16px;
    padding-left: 8px;
  }
  .options {
    width: 100%;
    ul {
      list-style: none;
      text-align: left;
      padding-left: 8px;
      max-height: 150px;
      overflow-y: auto;
      overflow-x: hidden;
    }
    li {
      width: 100%;
      border-bottom: 1px solid rgb(77, 75, 75);
      padding: 10px;
      &:hover {
        background: #70878a;
        color: #fff;
        font-weight: bold;
      }
    }
  }
}
</style>