<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

defineOptions({
  name: 'UiSelect',
})

const model = defineModel<string | number>()

const props = withDefaults(
  defineProps<{
    class?: HTMLAttributes['class']
    disabled?: boolean
    required?: boolean
  }>(),
  {
    disabled: false,
    required: false,
  },
)

const classes = computed(() => cn('ui-select', props.class))
</script>

<template>
  <select v-model="model" :class="classes" :disabled="disabled" :required="required">
    <slot />
  </select>
</template>

<style scoped>
.ui-select {
  appearance: none;
  background:
    linear-gradient(45deg, transparent 50%, var(--color-muted-foreground) 50%) calc(100% - 18px)
      50% / 6px 6px no-repeat,
    linear-gradient(135deg, var(--color-muted-foreground) 50%, transparent 50%) calc(100% - 12px)
      50% / 6px 6px no-repeat,
    var(--color-card);
  border: 1px solid var(--color-border-strong);
  border-radius: var(--radius-md);
  color: var(--color-foreground);
  font: inherit;
  min-height: 2.625rem;
  outline: none;
  padding: 0 2.3rem 0 0.8rem;
  transition:
    border-color 160ms ease,
    box-shadow 160ms ease;
  width: 100%;
}

.ui-select:hover:not(:disabled) {
  border-color: var(--color-primary);
}

.ui-select:focus {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px color-mix(in srgb, var(--color-primary) 14%, transparent);
}

.ui-select:disabled {
  cursor: not-allowed;
  opacity: 0.56;
}
</style>