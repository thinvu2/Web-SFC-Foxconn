<template>
  <div class="footer_nav">  
    <nav class="bg-white">
      <div
        class="        
          justify-content-between
          text-center
          container-p-x
          pb-3">
        <div class="pt-3">
          <span class="footer-text font-weight-semibold">{{
            $store.state.language == "En"
              ? "@Copyright IT Shopfloor Vietnam. Phone: (535) 31529"
              : "Bản quyền thuộc IT Shopfloor Vietnam. Phone: (535) 31529"
          }}</span>
        </div>
        <div>
          <div class="footer-link pt-3">
            <select
              v-model="language"
              @change="changeLanguage(language)"
              class="select-language-home"
              name=""
              id="">
              <option>En</option>
              <option>Vi</option>
            </select>
          </div>
        </div>
      </div>
    </nav>
  </div>
</template>

<script>
export default {
  name: "FooterNav",
  methods: {
    changeLanguage(lang) {
      this.$store.commit("changeLanguage", lang);
      localStorage.language = lang;
    },
  },
  created() {
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
};
</script>

<style>
.select-language-home {
  padding: 0px 20px;
  border-radius: 10%;
}
.justify-content-between{
  display: flex;
}
</style>
<style scoped  src="../../assets/template/dashboard/fonts/fontawesome.css"></style>
<style scoped src="../../assets/template/dashboard/fonts/ionicons.css"></style>
<style scoped src="../../assets/template/dashboard/fonts/linearicons.css"></style>
<style scoped src="../../assets/template/dashboard/fonts/open-iconic.css"></style>
<style scoped src="../../assets/template/dashboard/fonts/pe-icon-7-stroke.css"></style>
<style scoped src="../../assets/template/dashboard/fonts/feather.css"></style>
<style scoped src="../../assets/template/dashboard/css/bootstrap-material.css"></style>
<style scoped src="../../assets/template/dashboard/css/shreerang-material.css"></style>
<style scoped src="../../assets/template/dashboard/css/uikit.css"></style>
<style scoped src="../../assets/template/dashboard/libs/perfect-scrollbar/perfect-scrollbar.css"></style>
<style scoped src="../../assets/template/dashboard/libs/flot/flot.css"></style>