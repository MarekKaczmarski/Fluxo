<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

const props = defineProps<{
  class?: HTMLAttributes['class']
}>()

const classes = computed(() => cn('ui-card-title', props.class))
</script>

<template>
  <h2 :class="classes">
    <slot />
  </h2>
</template>

<style scoped>
.ui-card-title {
  color: var(--color-foreground);
  font-size: 1rem;
  font-weight: 800;
  letter-spacing: 0;
  line-height: 1.25;
  margin: 0;
}
</style>
