<script setup lang="ts">
import { computed } from 'vue'
import { formatMoney } from '@/lib/formatters'

export interface DonutSegment {
  color: string
  id: string
  label: string
  percent: number
  value: number
}

const props = defineProps<{
  currency: string
  expenses: number
  income: number
  segments: DonutSegment[]
}>()

const drawableSegments = computed(() => {
  let offset = 0

  return props.segments
    .filter((segment) => segment.value > 0 && segment.percent > 0)
    .map((segment) => {
      const next = {
        ...segment,
        dashOffset: -offset,
        dashValue: Math.max(segment.percent, 0.6),
      }

      offset += segment.percent
      return next
    })
})
</script>

<template>
  <div class="donut-chart">
    <svg class="donut-chart__svg" viewBox="0 0 186 186" role="img" aria-label="Expense chart">
      <circle class="donut-chart__track" cx="93" cy="93" r="72" pathLength="100" />
      <circle
        v-for="segment in drawableSegments"
        :key="segment.id"
        class="donut-chart__segment"
        cx="93"
        cy="93"
        r="72"
        pathLength="100"
        :stroke="segment.color"
        :stroke-dasharray="`${segment.dashValue} ${100 - segment.dashValue}`"
        :stroke-dashoffset="segment.dashOffset"
      >
        <title>{{ segment.label }}: {{ formatMoney(segment.value, currency) }}</title>
      </circle>
    </svg>

    <div class="donut-chart__center">
      <strong class="amount-income">{{ formatMoney(income, currency) }}</strong>
      <strong class="amount-expense">-{{ formatMoney(expenses, currency) }}</strong>
    </div>
  </div>
</template>

<style scoped>
.donut-chart {
  aspect-ratio: 1;
  display: grid;
  isolation: isolate;
  place-items: center;
  position: relative;
  width: min(62vw, 13.5rem);
}

.donut-chart__svg {
  height: 100%;
  overflow: visible;
  transform: rotate(-90deg);
  width: 100%;
}

.donut-chart__track,
.donut-chart__segment {
  fill: none;
  stroke-width: 42;
}

.donut-chart__track {
  stroke: var(--color-surface-strong);
}

.donut-chart__segment {
  stroke-linecap: butt;
  transition:
    stroke-dasharray 220ms ease,
    stroke-dashoffset 220ms ease;
}

.donut-chart__center {
  align-items: center;
  background: var(--color-card);
  border-radius: 999px;
  box-shadow: inset 0 0 0 1px var(--color-border);
  display: flex;
  flex-direction: column;
  font-size: clamp(0.78rem, 4vw, 1.05rem);
  gap: 0.1rem;
  height: 45%;
  justify-content: center;
  left: 50%;
  line-height: 1.1;
  position: absolute;
  top: 50%;
  transform: translate(-50%, -50%);
  width: 45%;
  z-index: 2;
}

@media (max-width: 640px) {
  .donut-chart {
    width: min(58vw, 15rem);
  }
}
</style>
