<script setup lang="ts">
import { CreditCard, LayoutGrid, ReceiptText, Tags } from '@lucide/vue'

const navItems = [
  { icon: LayoutGrid, label: 'Start', to: '/' },
  { icon: ReceiptText, label: 'Transactions', to: '/transactions' },
  { icon: Tags, label: 'Categories', to: '/categories' },
  { icon: CreditCard, label: 'Accounts', to: '/accounts' },
]
</script>

<template>
  <nav class="mobile-nav" aria-label="Main navigation">
    <RouterLink
      v-for="item in navItems"
      :key="item.to"
      class="mobile-nav__item"
      active-class="mobile-nav__item--active"
      :to="item.to"
    >
      <component :is="item.icon" :size="22" />
      <span>{{ item.label }}</span>
    </RouterLink>
  </nav>
</template>

<style scoped>
.mobile-nav {
  align-items: center;
  background: color-mix(in srgb, var(--color-card) 92%, transparent);
  backdrop-filter: blur(12px);
  border: 1px solid var(--color-border);
  border-radius: 1.1rem;
  bottom: calc(env(safe-area-inset-bottom) + 0.85rem);
  box-shadow: var(--shadow-card);
  display: grid;
  gap: 0.25rem;
  grid-template-columns: repeat(4, 1fr);
  left: 50%;
  max-width: 32rem;
  padding: 0.45rem;
  position: fixed;
  transform: translateX(-50%);
  width: calc(100% - 1.25rem);
  z-index: 20;
}

.mobile-nav__item {
  align-items: center;
  border-radius: 0.75rem;
  color: var(--color-muted-foreground);
  display: flex;
  flex-direction: column;
  font-size: 0.7rem;
  font-weight: 700;
  gap: 0.16rem;
  min-height: 3.1rem;
  justify-content: center;
  line-height: 1;
  text-decoration: none;
  transition:
    background-color 160ms ease,
    color 160ms ease;
}

.mobile-nav__item--active {
  background: var(--color-primary);
  color: white;
}
</style>
