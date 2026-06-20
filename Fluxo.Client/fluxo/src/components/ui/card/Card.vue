<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

defineOptions({
  name: 'UiCard',
})

const props = defineProps<{
  class?: HTMLAttributes['class']
}>()

const classes = computed(() => cn('ui-card', props.class))
</script>

<template>
  <section :class="classes">
    <slot />
  </section>
</template>

<style scoped>
.ui-card {
  background: var(--color-card);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-card);
  color: var(--color-card-foreground);
}
</style>
