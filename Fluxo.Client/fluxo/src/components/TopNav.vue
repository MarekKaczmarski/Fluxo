<script setup lang="ts">
import { computed } from 'vue'
import { useTheme } from 'vuetify'
import { Moon, Sun } from '@lucide/vue'

const THEME_STORAGE_KEY = 'fluxo-theme'

const theme = useTheme()
const isDark = computed(() => theme.global.name.value === 'fluxoDark')

function toggleTheme() {
  const next = isDark.value ? 'fluxo' : 'fluxoDark'
  theme.global.name.value = next
  localStorage.setItem(THEME_STORAGE_KEY, next)
}
</script>

<template>
  <header class="top-nav">
    <div class="top-nav__brand">
      <h1>Fluxo</h1>
      <span>Your budget app</span>
    </div>
    <v-btn
      icon
      variant="text"
      :aria-label="isDark ? 'Switch to light mode' : 'Switch to dark mode'"
      @click="toggleTheme"
    >
      <Sun v-if="isDark" :size="22" />
      <Moon v-else :size="22" />
    </v-btn>
  </header>
</template>

<style scoped>
.top-nav {
  align-items: center;
  background: var(--color-primary);
  color: white;
  display: flex;
  gap: 0.75rem;
  justify-content: space-between;
  padding: calc(env(safe-area-inset-top) + 1rem) clamp(1rem, 4vw, 2rem) 0.9rem;
}

.top-nav__brand {
  min-width: 0;
}

.top-nav__brand h1 {
  font-size: clamp(1.6rem, 5vw, 2rem);
  font-style: italic;
  font-weight: 900;
  letter-spacing: 0;
  line-height: 1;
  margin: 0;
}

.top-nav__brand span {
  display: block;
  font-size: 0.85rem;
  line-height: 1.2;
  opacity: 0.88;
}
</style>
