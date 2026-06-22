<template>
  <v-app class="app-shell">
    <RouterView />
    <MobileNav />
  </v-app>
</template>

<script setup lang="ts">
import MobileNav from './MobileNav.vue'
</script>

<style scoped>
.app-shell {
  min-height: 100vh;
}
</style>
