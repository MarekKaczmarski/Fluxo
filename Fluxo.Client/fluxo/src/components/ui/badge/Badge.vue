<script setup lang="ts">
import { computed, type HTMLAttributes } from 'vue'
import { cva, type VariantProps } from 'class-variance-authority'
import { cn } from '@/lib/utils'

defineOptions({
  name: 'UiBadge',
})

const badgeVariants = cva('ui-badge', {
  variants: {
    variant: {
      default: 'ui-badge--default',
      expense: 'ui-badge--expense',
      income: 'ui-badge--income',
      outline: 'ui-badge--outline',
    },
  },
  defaultVariants: {
    variant: 'default',
  },
})

type BadgeVariants = VariantProps<typeof badgeVariants>

const props = withDefaults(
  defineProps<{
    class?: HTMLAttributes['class']
    variant?: BadgeVariants['variant']
  }>(),
  {
    variant: 'default',
  },
)

const classes = computed(() => cn(badgeVariants({ variant: props.variant }), props.class))
</script>

<template>
  <span :class="classes">
    <slot />
  </span>
</template>

<style scoped>
.ui-badge {
  align-items: center;
  border: 1px solid transparent;
  border-radius: 999px;
  display: inline-flex;
  font-size: 0.75rem;
  font-weight: 800;
  line-height: 1;
  min-height: 1.5rem;
  padding: 0 0.55rem;
  white-space: nowrap;
}

.ui-badge--default {
  background: color-mix(in srgb, var(--color-primary) 14%, white);
  color: var(--color-primary-strong);
}

.ui-badge--expense {
  background: color-mix(in srgb, var(--color-expense) 13%, white);
  color: var(--color-expense);
}

.ui-badge--income {
  background: color-mix(in srgb, var(--color-income) 16%, white);
  color: var(--color-income-strong);
}

.ui-badge--outline {
  background: var(--color-card);
  border-color: var(--color-border);
  color: var(--color-muted-foreground);
}
</style>
