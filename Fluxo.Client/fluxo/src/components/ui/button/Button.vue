<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cva, type VariantProps } from 'class-variance-authority'
import { cn } from '@/lib/utils'

defineOptions({
  name: 'UiButton',
})

const buttonVariants = cva('ui-button', {
  variants: {
    variant: {
      default: 'ui-button--default',
      secondary: 'ui-button--secondary',
      destructive: 'ui-button--destructive',
      outline: 'ui-button--outline',
      ghost: 'ui-button--ghost',
      balance: 'ui-button--balance',
      expense: 'ui-button--expense',
      income: 'ui-button--income',
    },
    size: {
      default: 'ui-button--default-size',
      sm: 'ui-button--sm',
      lg: 'ui-button--lg',
      icon: 'ui-button--icon',
      action: 'ui-button--action',
    },
  },
  defaultVariants: {
    variant: 'default',
    size: 'default',
  },
})

type ButtonVariants = VariantProps<typeof buttonVariants>

const props = withDefaults(
  defineProps<{
    as?: string
    class?: HTMLAttributes['class']
    disabled?: boolean
    size?: ButtonVariants['size']
    type?: 'button' | 'submit' | 'reset'
    variant?: ButtonVariants['variant']
  }>(),
  {
    as: 'button',
    disabled: false,
    size: 'default',
    type: 'button',
    variant: 'default',
  },
)

const classes = computed(() =>
  cn(buttonVariants({ variant: props.variant, size: props.size }), props.class),
)
</script>

<template>
  <v-btn :class="classes" :disabled="disabled" :type="type" variant="text">
    <slot />
  </v-btn>
</template>

<style scoped>
.ui-button {
  align-items: center;
  border: 1px solid transparent;
  border-radius: var(--radius-md);
  cursor: pointer;
  display: inline-flex;
  font-family: inherit;
  font-weight: 700;
  gap: 0.5rem;
  justify-content: center;
  letter-spacing: 0;
  line-height: 1;
  min-width: 0;
  text-decoration: none;
  text-transform: none;
  transition:
    background-color 160ms ease,
    border-color 160ms ease,
    color 160ms ease,
    transform 160ms ease,
    box-shadow 160ms ease;
  user-select: none;
  white-space: nowrap;
}

.ui-button :deep(.v-btn__content) {
  gap: 0.5rem;
}

.ui-button:focus-visible {
  outline: 3px solid color-mix(in srgb, var(--color-primary) 28%, transparent);
  outline-offset: 2px;
}

.ui-button:disabled,
.ui-button.v-btn--disabled {
  cursor: not-allowed;
  opacity: 0.56;
}

.ui-button:not(:disabled):active {
  transform: translateY(1px);
}

.ui-button--default {
  background: var(--color-primary);
  color: white;
}

.ui-button--default:hover {
  background: var(--color-primary-strong);
}

.ui-button--secondary {
  background: var(--color-surface-muted);
  border-color: var(--color-border);
  color: var(--color-foreground);
}

.ui-button--secondary:hover {
  background: var(--color-surface-strong);
}

.ui-button--destructive {
  background: var(--color-expense);
  color: white;
}

.ui-button--destructive:hover {
  background: #b91c1c;
}

.ui-button--outline {
  background: transparent;
  border-color: var(--color-border-strong);
  color: var(--color-foreground);
}

.ui-button--outline:hover {
  background: var(--color-surface-muted);
}

.ui-button--ghost {
  background: transparent;
  color: var(--color-muted-foreground);
}

.ui-button--ghost:hover {
  background: var(--color-surface-muted);
  color: var(--color-foreground);
}

.ui-button--balance {
  background: var(--color-primary);
  border-color: var(--color-primary-strong);
  box-shadow: inset 0 0 0 1px color-mix(in srgb, white 16%, transparent);
  color: white;
}

.ui-button--expense {
  background: color-mix(in srgb, var(--color-expense) 12%, white);
  box-shadow: 0 1px 2px rgb(0 0 0 / 4%);
  color: var(--color-expense);
}

.ui-button--expense:hover {
  background: color-mix(in srgb, var(--color-expense) 20%, white);
  box-shadow: 0 10px 22px color-mix(in srgb, var(--color-expense) 24%, transparent);
  transform: translateY(-2px);
}

.ui-button--income {
  background: color-mix(in srgb, var(--color-income) 12%, white);
  box-shadow: 0 1px 2px rgb(0 0 0 / 4%);
  color: var(--color-income-strong);
}

.ui-button--income:hover {
  background: color-mix(in srgb, var(--color-income) 20%, white);
  box-shadow: 0 10px 22px color-mix(in srgb, var(--color-income) 24%, transparent);
  transform: translateY(-2px);
}

.ui-button--default-size {
  min-height: 2.625rem;
  padding: 0 1rem;
}

.ui-button--sm {
  min-height: 2.25rem;
  padding: 0 0.75rem;
}

.ui-button--lg {
  min-height: 3rem;
  padding: 0 1.25rem;
}

.ui-button--icon {
  aspect-ratio: 1;
  height: 2.5rem;
  padding: 0;
  width: 2.5rem;
}

.ui-button--action {
  aspect-ratio: 1;
  border-radius: 999px;
  font-size: 2.6rem;
  height: clamp(5.5rem, 24vw, 8.25rem);
  padding: 0;
  width: clamp(5.5rem, 24vw, 8.25rem);
}
</style>
