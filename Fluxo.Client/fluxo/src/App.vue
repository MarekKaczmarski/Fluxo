<script setup lang="ts">
import TransactionManager from './components/TransactionManager.vue'
</script>

<template>
  <main class="app-container">
    <header class="app-header">
      <h1>Fluxo App</h1>
    </header>

    <TransactionManager />
  </main>
</template>

<style scoped>
.app-container {
  max-width: 800px;
  margin: 2rem auto;
  padding: 0 1rem;
  font-family: sans-serif;
}
.app-header {
  border-bottom: 2px solid #eee;
  margin-bottom: 2rem;
  padding-bottom: 0.5rem;
}
h1 {
  color: #333;
  margin: 0;
}
</style>
