<template>
  <v-app class="app-shell">
    <TopNav />
    <RouterView />
    <MobileNav />
  </v-app>
</template>

<script setup lang="ts">
import MobileNav from './MobileNav.vue'
import TopNav from './TopNav.vue'
</script>

<style scoped>
.app-shell {
  min-height: 100vh;
}
</style>
