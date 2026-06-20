<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

const props = defineProps<{
  class?: HTMLAttributes['class']
}>()

const classes = computed(() => cn('ui-card-header', props.class))
</script>

<template>
  <div :class="classes">
    <slot />
  </div>
</template>

<style scoped>
.ui-card-header {
  display: grid;
  gap: 0.35rem;
  padding: 1rem 1rem 0;
}
</style>
