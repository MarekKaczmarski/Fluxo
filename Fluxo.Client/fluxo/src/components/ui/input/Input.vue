<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

defineOptions({
  name: 'UiInput',
})

const model = defineModel<string | number>()

const props = withDefaults(
  defineProps<{
    class?: HTMLAttributes['class']
    disabled?: boolean
    max?: string | number
    min?: string | number
    placeholder?: string
    required?: boolean
    step?: string | number
    type?: string
  }>(),
  {
    disabled: false,
    placeholder: '',
    required: false,
    type: 'text',
  },
)

const classes = computed(() => cn('ui-input', props.class))
</script>

<template>
  <input
    v-model="model"
    :class="classes"
    :disabled="disabled"
    :max="max"
    :min="min"
    :placeholder="placeholder"
    :required="required"
    :step="step"
    :type="type"
  />
</template>

<style scoped>
.ui-input {
  background: var(--color-card);
  border: 1px solid var(--color-border-strong);
  border-radius: var(--radius-md);
  color: var(--color-foreground);
  font: inherit;
  min-height: 2.625rem;
  outline: none;
  padding: 0 0.8rem;
  transition:
    border-color 160ms ease,
    box-shadow 160ms ease;
  width: 100%;
}

.ui-input:focus {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px color-mix(in srgb, var(--color-primary) 20%, transparent);
}

.ui-input::placeholder {
  color: var(--color-muted-foreground);
}

.ui-input:disabled {
  cursor: not-allowed;
  opacity: 0.58;
}
</style>
