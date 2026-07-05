<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { ChevronLeft, ChevronRight, Minus, Plus } from '@lucide/vue'
import SpendingOrbit, { type OrbitItem } from '@/components/SpendingOrbit.vue'
import { TransactionType } from '@/api/models'
import { useBudgetStore } from '@/stores/budgetStore'
import { addMonths, formatMonth, isSameMonth, latestTransactionMonth, parseDate } from '@/lib/dates'
import { categoryColors } from '@/lib/categoryIcons'
import { formatMoney } from '@/lib/formatters'

const store = useBudgetStore()
const {
  categories,
  error,
  hasFieldErrors,
  isLoading,
  primaryCurrency,
  totalBalance,
  transactions,
} = storeToRefs(store)

const selectedMonth = ref(new Date(new Date().getFullYear(), new Date().getMonth(), 1))
const hasAutoSelectedMonth = ref(false)

onMounted(() => {
  void store.fetchAll()
})

watch(transactions, (items) => {
  if (hasAutoSelectedMonth.value || items.length === 0) {
    return
  }

  const hasCurrentMonthTransactions = items.some((transaction) =>
    isSameMonth(parseDate(transaction.date), selectedMonth.value),
  )

  if (!hasCurrentMonthTransactions) {
    selectedMonth.value = latestTransactionMonth(items)
  }

  hasAutoSelectedMonth.value = true
})

const previousMonth = computed(() => addMonths(selectedMonth.value, -1))
const nextMonth = computed(() => addMonths(selectedMonth.value, 1))

function isAfterMonth(a: Date, b: Date) {
  return (
    a.getFullYear() > b.getFullYear() ||
    (a.getFullYear() === b.getFullYear() && a.getMonth() > b.getMonth())
  )
}

function currentMonthStart() {
  const now = new Date()
  return new Date(now.getFullYear(), now.getMonth(), 1)
}

const isNextMonthDisabled = computed(() => isAfterMonth(nextMonth.value, currentMonthStart()))

const monthlyTransactions = computed(() =>
  transactions.value.filter((transaction) =>
    isSameMonth(parseDate(transaction.date), selectedMonth.value),
  ),
)

const monthlyIncome = computed(() =>
  monthlyTransactions.value
    .filter((transaction) => transaction.type === TransactionType.Income)
    .reduce((sum, transaction) => sum + transaction.amount, 0),
)

const monthlyExpenses = computed(() =>
  monthlyTransactions.value
    .filter((transaction) => transaction.type === TransactionType.Expense)
    .reduce((sum, transaction) => sum + transaction.amount, 0),
)

const INCOME_CATEGORY_NAME = 'income'

const orbitItems = computed<OrbitItem[]>(() => {
  const expenseCategories = categories.value.filter(
    (category) => category.name.trim().toLowerCase() !== INCOME_CATEGORY_NAME,
  )

  const totalsByCategory = new Map<string, number>()
  const categoryNames = new Map<string, string>()

  for (const category of expenseCategories) {
    totalsByCategory.set(category.id, 0)
    categoryNames.set(category.id, category.name)
  }

  for (const transaction of monthlyTransactions.value) {
    if (transaction.type !== TransactionType.Expense) {
      continue
    }

    if (!totalsByCategory.has(transaction.categoryId)) {
      continue
    }

    totalsByCategory.set(
      transaction.categoryId,
      (totalsByCategory.get(transaction.categoryId) ?? 0) + transaction.amount,
    )
    categoryNames.set(transaction.categoryId, transaction.categoryName)
  }

  return Array.from(totalsByCategory.entries()).map(([categoryId, amount], index) => {
    const category = expenseCategories.find((item) => item.id === categoryId)
    const color = categoryColors[index % categoryColors.length] ?? '#75c58f'

    return {
      amount,
      color,
      icon: category?.icon ?? null,
      id: categoryId,
      name: category?.name ?? categoryNames.get(categoryId) ?? 'Category',
      percent: monthlyExpenses.value > 0 ? (amount / monthlyExpenses.value) * 100 : 0,
    }
  })
})

function changeMonth(amount: number) {
  const target = addMonths(selectedMonth.value, amount)

  if (isAfterMonth(target, currentMonthStart())) {
    return
  }

  selectedMonth.value = target
}
</script>

<template>
  <main class="dashboard-view">
    <header class="dashboard-view__topbar">
      <div class="dashboard-view__brand">
        <h1>Fluxo</h1>
        <span>Your budget app</span>
      </div>
    </header>

    <section class="dashboard-view__month" aria-label="Month selection">
      <button type="button" @click="changeMonth(-1)">
        <ChevronLeft :size="18" />
        {{ formatMonth(previousMonth) }}
      </button>
      <strong>{{ formatMonth(selectedMonth) }}</strong>
      <button type="button" :disabled="isNextMonthDisabled" @click="changeMonth(1)">
        {{ formatMonth(nextMonth) }}
        <ChevronRight :size="18" />
      </button>
    </section>

    <v-alert v-if="error && !hasFieldErrors" type="error" variant="tonal">
      {{ error.message }}
    </v-alert>
    <p v-else-if="isLoading && transactions.length === 0" class="state-message">Loading data...</p>

    <SpendingOrbit
      :currency="primaryCurrency"
      :expenses="monthlyExpenses"
      :income="monthlyIncome"
      :items="orbitItems"
    />

    <section class="dashboard-view__summary" aria-label="Account summary">
      <v-btn class="dashboard-view__balance balance-pill" size="large" :to="'/accounts'">
        <span>Balance</span>
        <strong>{{ formatMoney(totalBalance, primaryCurrency) }}</strong>
      </v-btn>
    </section>

    <section class="dashboard-view__actions" aria-label="Quick adding actions">
      <div class="dashboard-view__action">
        <v-btn
          class="dashboard-view__action-button"
          icon
          color="error"
          variant="tonal"
          :to="{ path: '/transactions', query: { type: 'expense' } }"
          aria-label="Add expense"
        >
          <Minus :size="42" />
        </v-btn>
        <span>Expense</span>
      </div>

      <div class="dashboard-view__action">
        <v-btn
          class="dashboard-view__action-button"
          icon
          color="success"
          variant="tonal"
          :to="{ path: '/transactions', query: { type: 'income' } }"
          aria-label="Add income"
        >
          <Plus :size="42" />
        </v-btn>
        <span>Income</span>
      </div>
    </section>
  </main>
</template>

<style scoped>
.dashboard-view {
  background: var(--color-background);
  min-height: 100vh;
  padding: 0 0 7.75rem;
}

.dashboard-view__topbar {
  align-items: center;
  background: var(--color-primary);
  color: white;
  display: grid;
  gap: 0.75rem;
  grid-template-columns: auto 1fr auto;
  padding: calc(env(safe-area-inset-top) + 1rem) clamp(1rem, 4vw, 2rem) 0.9rem;
}

.dashboard-view__brand {
  min-width: 0;
}

.dashboard-view__brand h1 {
  font-size: clamp(2rem, 8vw, 3rem);
  font-style: italic;
  font-weight: 900;
  letter-spacing: 0;
  line-height: 1;
  margin: 0;
}

.dashboard-view__brand span {
  display: block;
  font-size: 1rem;
  line-height: 1.2;
  opacity: 0.88;
}

.dashboard-view__month {
  align-items: center;
  display: grid;
  gap: 0.75rem;
  grid-template-columns: 1fr auto 1fr;
  padding: 1.15rem clamp(1rem, 4vw, 2rem) 0;
}

.dashboard-view__month button {
  align-items: center;
  background: transparent;
  border: 0;
  color: var(--color-primary-strong);
  cursor: pointer;
  display: inline-flex;
  font: inherit;
  font-weight: 800;
  gap: 0.1rem;
  padding: 0;
}

.dashboard-view__month button:first-child {
  justify-content: flex-start;
  opacity: 1;
}

.dashboard-view__month button:last-child {
  justify-content: flex-end;
  opacity: 1;
}

.dashboard-view__month button:disabled {
  visibility: hidden;
  opacity: 0;
  cursor: default;
}

.dashboard-view__month strong {
  color: var(--color-primary-strong);
  font-size: clamp(1.35rem, 5vw, 1.85rem);
  font-weight: 900;
  text-transform: capitalize;
}

.dashboard-view__summary {
  align-items: center;
  display: flex;
  justify-content: center;
  gap: clamp(0.75rem, 4vw, 2rem);
  grid-template-columns: 1fr auto 1fr;
  margin: 0 auto;
  max-width: 52rem;
  padding: 0 clamp(1rem, 5vw, 2rem);
}

.dashboard-view__balance {
  font-size: clamp(1.25rem, 5vw, 2.1rem);
  min-width: min(76vw, 24rem);
}

.dashboard-view__balance :deep(.v-btn__content) {
  gap: 0.5rem;
}

.dashboard-view__balance span {
  font-weight: 900;
}

.dashboard-view__balance strong {
  font-weight: 500;
}

.dashboard-view__actions {
  align-items: center;
  display: flex;
  justify-content: space-around;
  margin: clamp(1.75rem, 6vw, 3.2rem) auto 0;
  max-width: 46rem;
  padding: 0 clamp(1rem, 6vw, 2rem);
}

.dashboard-view__action {
  align-items: center;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.dashboard-view__action-button {
  aspect-ratio: 1;
  border-radius: 999px;
  font-size: 2.6rem;
  height: clamp(5.5rem, 24vw, 8.25rem);
  width: clamp(5.5rem, 24vw, 8.25rem);
}

.dashboard-view__action span {
  color: var(--color-muted-foreground);
  font-size: 0.8rem;
  font-weight: 700;
}
</style>
