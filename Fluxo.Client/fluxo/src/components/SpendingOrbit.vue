<script setup lang="ts">
import { computed } from 'vue'
import CategoryIcon from './CategoryIcon.vue'
import { formatMoney } from '@/lib/formatters'

export interface OrbitItem {
  amount: number
  color: string
  icon: string | null
  id: string
  name: string
  percent: number
}

const props = defineProps<{
  currency: string
  expenses: number
  income: number
  items: OrbitItem[]
}>()

const pieItems = computed(() =>
  props.items
    .filter((item) => item.amount > 0)
    .map((item) => ({
      id: item.id,
      title: item.name,
      value: Math.round(item.percent),
      color: item.color,
      icon: item.icon ?? item.name,
    })),
)

const iconById = computed(() => new Map(pieItems.value.map((item) => [item.id, item.icon])))

function iconFor(id: string) {
  return iconById.value.get(id) ?? null
}

const chartKey = computed(() => `${props.income}-${props.expenses}-${pieItems.value.length}`)
</script>

<template>
  <v-card class="spending-summary overflow-visible pa-4 pa-md-6" elevation="2" rounded="xl">
    <v-card-title class="d-flex align-center justify-space-between pa-0 mb-2">
      <span class="text-truncate">Expenses</span>
    </v-card-title>

    <v-pie
      :key="chartKey"
      :items="pieItems"
      :legend="{ position: $vuetify.display.mdAndUp ? 'right' : 'bottom' }"
      :tooltip="{
        subtitleFormat: '[value]%',
      }"
      :size="$vuetify.display.smAndDown ? 260 : 320"
      class="pa-2 mt-2 justify-center"
      gap="2"
      inner-cut="70"
      item-key="id"
      rounded="2"
      animation
      hide-slice
      reveal
    >
      <template #center>
        <div class="text-center">
          <div class="font-weight-bold amount-income">{{ formatMoney(income, currency) }}</div>
          <div class="font-weight-bold amount-expense">-{{ formatMoney(expenses, currency) }}</div>
        </div>
      </template>

      <template #tooltip="{ item }">
        <div class="d-flex align-center" style="gap: 12px">
          <div
            :style="{ backgroundColor: item.color }"
            style="width: 16px; height: 16px; border-radius: 50%; flex-shrink: 0"
          ></div>

          <div class="d-flex flex-column text-left font-weight-medium" style="line-height: 1.2">
            <span class="text-body-2 text-grey-lighten-2">{{ item.title }}</span>
            <span class="text-caption font-weight-bold text-white">{{ item.value }}%</span>
          </div>
        </div>
      </template>

      <template #legend="{ items: legendItems, toggle, isActive }">
        <v-list class="py-0 mb-n3 mb-md-0 bg-transparent" density="compact" width="260">
          <v-list-item
            v-for="item in legendItems"
            :key="item.key"
            :class="['my-1', { 'opacity-40': !isActive(item) }]"
            :title="item.title"
            rounded="lg"
            link
            @click="toggle(item)"
          >
            <template #prepend>
              <span
                class="spending-summary__legend-icon mr-3"
                :style="{ '--legend-item-color': item.color }"
              >
                <CategoryIcon :icon="iconFor(String(item.key))" :color="item.color" :size="18" />
              </span>
            </template>
            <template #append>
              <div class="font-weight-bold">{{ item.value }}%</div>
            </template>
          </v-list-item>
        </v-list>
      </template>
    </v-pie>

    <p v-if="pieItems.length === 0" class="text-center text-medium-emphasis ma-0 pb-2">
      No expenses recorded for this month yet.
    </p>
  </v-card>
</template>

<style scoped>
.spending-summary {
  margin: 1.5rem auto;
  max-width: 48rem;
  min-height: 26rem;
  width: calc(100% - 2 * clamp(1rem, 4vw, 2rem));
}

.spending-summary__legend-icon {
  align-items: center;
  background: color-mix(
    in srgb,
    var(--legend-item-color, var(--color-foreground)) 16%,
    var(--color-card)
  );
  border-radius: 999px;
  display: inline-flex;
  flex-shrink: 0;
  height: 1.8rem;
  justify-content: center;
  width: 1.8rem;
}
</style>

<style>
.v-pie__tooltip-content {
  background-color: #424242 !important;
  color: white !important;
  border-radius: 8px !important;
  padding: 8px 12px !important;
  border: none !important;
  box-shadow:
    0px 2px 4px -1px rgba(0, 0, 0, 0.2),
    0px 4px 5px 0px rgba(0, 0, 0, 0.14),
    0px 1px 10px 0px rgba(0, 0, 0, 0.12) !important;
}
</style>
