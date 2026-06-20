<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

const props = defineProps<{
  class?: HTMLAttributes['class']
}>()

const classes = computed(() => cn('ui-card-content', props.class))
</script>

<template>
  <div :class="classes">
    <slot />
  </div>
</template>

<style scoped>
.ui-card-content {
  padding: 1rem;
}
</style>
