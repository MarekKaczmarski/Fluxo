<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

const props = defineProps<{
  class?: HTMLAttributes['class']
}>()

const classes = computed(() => cn('ui-card-description', props.class))
</script>

<template>
  <p :class="classes">
    <slot />
  </p>
</template>

<style scoped>
.ui-card-description {
  color: var(--color-muted-foreground);
  font-size: 0.86rem;
  line-height: 1.35;
  margin: 0;
}
</style>
