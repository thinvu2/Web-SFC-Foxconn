<template>
    <datepicker v-model="picked" />
</template>
<script setup>

import { ref } from 'vue'
const picked = ref(new Date())
</script>