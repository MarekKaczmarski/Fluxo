<script setup lang="ts">
import { computed } from 'vue'
import { getCategoryIcon } from '@/lib/categoryIcons'

const props = withDefaults(
  defineProps<{
    color?: string
    icon?: string | null
    muted?: boolean
    size?: number
  }>(),
  {
    color: '#75c58f',
    muted: false,
    size: 34,
  },
)

const iconComponent = computed(() => getCategoryIcon(props.icon))
const iconSize = computed(() => `${props.size}px`)
</script>

<template>
  <span class="category-icon" :class="{ 'category-icon--muted': muted }">
    <component :is="iconComponent" :size="iconSize" :stroke-width="1.8" />
  </span>
</template>

<style scoped>
.category-icon {
  align-items: center;
  color: v-bind(color);
  display: inline-flex;
  filter: drop-shadow(0 7px 10px color-mix(in srgb, v-bind(color) 24%, transparent));
  justify-content: center;
}

.category-icon--muted {
  filter: none;
  opacity: 0.42;
}
</style>
