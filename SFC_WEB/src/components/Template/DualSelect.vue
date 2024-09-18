<template>
  <div>
      <v-multiselect-listbox :options="['Alabama', 'California']"></v-multiselect-listbox>
  </div>
</template>

<script>
import Vue from 'vue'
import vMultiselectListbox from 'vue-multiselect-listbox'

Vue.component('v-multiselect-listbox', vMultiselectListbox)
export default {

}
</script>

<style>

</style>