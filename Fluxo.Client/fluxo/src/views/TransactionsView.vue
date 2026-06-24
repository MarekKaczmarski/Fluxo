<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { useRoute } from 'vue-router'
import { ArrowDownCircle, ArrowUpCircle, Pencil, Plus, Trash2 } from '@lucide/vue'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Select } from '@/components/ui/select'
import CategoryIcon from '@/components/CategoryIcon.vue'
import { TransactionType, type TransactionDto } from '@/api/models'
import { useBudgetStore } from '@/stores/budgetStore'
import { formatDate, toDateInputValue } from '@/lib/dates'
import { formatMoney } from '@/lib/formatters'
import { categoryColors } from '@/lib/categoryIcons'

const route = useRoute()
const store = useBudgetStore()
const { accounts, categories, error, isLoading, isSaving, transactions } = storeToRefs(store)

type FilterType = 'all' | 'expense' | 'income'

const filter = ref<FilterType>('all')

const form = reactive({
  accountId: '',
  amount: '',
  categoryId: '',
  date: toDateInputValue(new Date()),
  description: '',
  id: '',
  type: TransactionType.Expense,
})

onMounted(() => {
  void store.fetchAll()
})

watch(
  () => route.query.type,
  (type) => {
    if (type === 'income') {
      form.type = TransactionType.Income
      filter.value = 'income'
    }

    if (type === 'expense') {
      form.type = TransactionType.Expense
      filter.value = 'expense'
    }
  },
  { immediate: true },
)

watch(
  [categories, accounts],
  () => {
    form.categoryId ||= categories.value[0]?.id ?? ''
    form.accountId ||= accounts.value[0]?.id ?? ''
  },
  { immediate: true },
)

const sortedTransactions = computed(() =>
  [...transactions.value].sort(
    (first, second) => new Date(second.date).getTime() - new Date(first.date).getTime(),
  ),
)

const visibleTransactions = computed(() => {
  if (filter.value === 'income') {
    return sortedTransactions.value.filter(
      (transaction) => transaction.type === TransactionType.Income,
    )
  }

  if (filter.value === 'expense') {
    return sortedTransactions.value.filter(
      (transaction) => transaction.type === TransactionType.Expense,
    )
  }

  return sortedTransactions.value
})

const categoriesById = computed(
  () => new Map(categories.value.map((category, index) => [category.id, { ...category, index }])),
)

const isEditing = computed(() => Boolean(form.id))

function resetForm() {
  form.accountId = accounts.value[0]?.id ?? ''
  form.amount = ''
  form.categoryId = categories.value[0]?.id ?? ''
  form.date = toDateInputValue(new Date())
  form.description = ''
  form.id = ''
  form.type = route.query.type === 'income' ? TransactionType.Income : TransactionType.Expense
}

function editTransaction(transaction: TransactionDto) {
  form.accountId = accounts.value[0]?.id ?? ''
  form.amount = String(transaction.amount)
  form.categoryId = transaction.categoryId
  form.date = toDateInputValue(new Date(transaction.date))
  form.description = transaction.description
  form.id = transaction.id
  form.type = transaction.type
}

function toApiDate(date: string) {
  return `${date}T12:00:00.000Z`
}

async function submitForm() {
  const amount = Number(form.amount)

  if (!Number.isFinite(amount) || amount <= 0 || !form.categoryId) {
    return
  }

  if (isEditing.value) {
    await store.saveTransaction({
      amount,
      categoryId: form.categoryId,
      date: toApiDate(form.date),
      description: form.description.trim(),
      id: form.id,
      type: form.type,
    })
  } else if (form.accountId) {
    await store.addTransaction({
      accountId: form.accountId,
      amount,
      categoryId: form.categoryId,
      date: toApiDate(form.date),
      description: form.description.trim(),
      type: form.type,
    })
  }

  resetForm()
}

async function removeTransaction(transaction: TransactionDto) {
  if (window.confirm(`Delete transaction "${transaction.description}"?`)) {
    await store.removeTransaction(transaction.id)
  }
}

function categoryColor(categoryId: string) {
  const category = categoriesById.value.get(categoryId)
  return categoryColors[(category?.index ?? 0) % categoryColors.length] ?? '#75c58f'
}
</script>

<template>
  <main class="entity-view">
    <header class="entity-view__header">
      <div>
        <h1 class="page-title">Transactions</h1>
      </div>
    </header>

    <!-- <p v-if="error" class="state-message state-message--error">{{ error.message }}</p> -->
    <div v-if="error" class="state-message state-message--error">
      <strong>{{ error.message }}</strong>

      <ul v-if="error.fields" class="mt-2 list-disc pl-4 text-xs">
        <template v-for="(messages, fieldName) in error.fields" :key="fieldName">
          <li v-for="(msg, index) in messages" :key="index">
            {{ msg }}
          </li>
        </template>
      </ul>
    </div>

    <section class="transactions-layout">
      <Card>
        <v-card-title>{{ isEditing ? 'Edit Transaction' : 'New Transaction' }}</v-card-title>
        <v-card-text>
          <form class="form-grid" @submit.prevent="submitForm">
            <div class="transaction-type-toggle" role="group" aria-label="Transaction Type">
              <button
                type="button"
                :class="{
                  'transaction-type-toggle__button--active': form.type === TransactionType.Expense,
                }"
                @click="form.type = TransactionType.Expense"
              >
                <ArrowDownCircle :size="18" />
                Expense
              </button>
              <button
                type="button"
                :class="{
                  'transaction-type-toggle__button--active': form.type === TransactionType.Income,
                }"
                @click="form.type = TransactionType.Income"
              >
                <ArrowUpCircle :size="18" />
                Income
              </button>
            </div>

            <div class="field">
              <label for="transaction-description">Description</label>
              <Input id="transaction-description" v-model="form.description" required />
            </div>

            <div class="transaction-form__amount-date-group">
              <div class="field">
                <label for="transaction-amount">Amount</label>
                <Input
                  id="transaction-amount"
                  v-model="form.amount"
                  min="0.01"
                  step="0.01"
                  type="number"
                  required
                />
              </div>
              <div class="field">
                <label for="transaction-date">Date</label>
                <Input id="transaction-date" v-model="form.date" type="date" required />
              </div>
            </div>

            <div class="form-grid form-grid--two">
              <div class="field">
                <label for="transaction-category">Category</label>
                <Select id="transaction-category" v-model="form.categoryId" required>
                  <option v-for="category in categories" :key="category.id" :value="category.id">
                    {{ category.name }}
                  </option>
                </Select>
              </div>
              <div class="field">
                <label for="transaction-account">Account</label>
                <Select
                  id="transaction-account"
                  v-model="form.accountId"
                  :disabled="isEditing"
                  required
                >
                  <option v-for="account in accounts" :key="account.id" :value="account.id">
                    {{ account.name }}
                  </option>
                </Select>
              </div>
            </div>

            <div class="form-actions">
              <Button
                type="submit"
                :disabled="isSaving || categories.length === 0 || accounts.length === 0"
              >
                <Plus v-if="!isEditing" :size="18" />
                {{ isSaving ? 'Saving...' : isEditing ? 'Save Changes' : 'Add Transaction' }}
              </Button>
              <Button v-if="isEditing" type="button" variant="secondary" @click="resetForm">
                Cancel
              </Button>
            </div>
          </form>
        </v-card-text>
      </Card>

      <Card>
        <v-card-title>Transaction List</v-card-title>
        <v-card-subtitle>{{ visibleTransactions.length }} positions</v-card-subtitle>
        <v-card-text>
          <div class="filter-tabs" role="group" aria-label="Transaction Filter">
            <button
              type="button"
              :class="{ 'filter-tabs__button--active': filter === 'all' }"
              @click="filter = 'all'"
            >
              All
            </button>
            <button
              type="button"
              :class="{ 'filter-tabs__button--active': filter === 'expense' }"
              @click="filter = 'expense'"
            >
              Expenses
            </button>
            <button
              type="button"
              :class="{ 'filter-tabs__button--active': filter === 'income' }"
              @click="filter = 'income'"
            >
              Income
            </button>
          </div>

          <p v-if="isLoading && transactions.length === 0" class="state-message">
            Loading transactions...
          </p>
          <p v-else-if="visibleTransactions.length === 0" class="state-message">No transactions.</p>

          <ul v-else class="transaction-list">
            <li
              v-for="transaction in visibleTransactions"
              :key="transaction.id"
              class="transaction-list__item"
            >
              <CategoryIcon
                :color="categoryColor(transaction.categoryId)"
                :icon="categoriesById.get(transaction.categoryId)?.icon ?? transaction.categoryName"
                :size="28"
              />
              <div class="transaction-list__copy">
                <strong>{{ transaction.description }}</strong>
                <span>{{ transaction.categoryName }} · {{ formatDate(transaction.date) }}</span>
              </div>
              <div class="transaction-list__amount">
                <Badge
                  :variant="transaction.type === TransactionType.Income ? 'income' : 'expense'"
                >
                  {{ transaction.type === TransactionType.Income ? 'Income' : 'Expense' }}
                </Badge>
                <strong
                  :class="
                    transaction.type === TransactionType.Income ? 'amount-income' : 'amount-expense'
                  "
                >
                  {{ transaction.type === TransactionType.Income ? '+' : '-'
                  }}{{ formatMoney(transaction.amount, transaction.currency) }}
                </strong>
              </div>
              <div class="transaction-list__actions">
                <Button
                  size="icon"
                  variant="ghost"
                  type="button"
                  aria-label="Edit"
                  @click="editTransaction(transaction)"
                >
                  <Pencil :size="18" />
                </Button>
                <Button
                  size="icon"
                  variant="ghost"
                  type="button"
                  aria-label="Delete"
                  @click="removeTransaction(transaction)"
                >
                  <Trash2 :size="18" />
                </Button>
              </div>
            </li>
          </ul>
        </v-card-text>
      </Card>
    </section>
  </main>
</template>

<style scoped>
.entity-view {
  margin: 0 auto;
  max-width: 74rem;
  min-height: 100vh;
  padding: 2rem clamp(1rem, 4vw, 2rem) 7.75rem;
}

.entity-view__header {
  align-items: end;
  display: flex;
  justify-content: space-between;
  margin-bottom: 1.25rem;
}

.transactions-layout {
  display: grid;
  gap: 1rem;
  grid-template-columns: minmax(19rem, 0.8fr) minmax(0, 1.2fr);
}

.form-grid--two {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.form-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
}

.transaction-type-toggle,
.filter-tabs {
  background: var(--color-surface-muted);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  display: grid;
  gap: 0.25rem;
  padding: 0.25rem;
}

.transaction-type-toggle {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.filter-tabs {
  grid-template-columns: repeat(3, minmax(0, 1fr));
  margin-bottom: 1rem;
}

.transaction-type-toggle button,
.filter-tabs button {
  align-items: center;
  background: transparent;
  border: 0;
  border-radius: calc(var(--radius-md) - 0.15rem);
  color: var(--color-muted-foreground);
  cursor: pointer;
  display: inline-flex;
  font: inherit;
  font-weight: 800;
  gap: 0.35rem;
  justify-content: center;
  min-height: 2.3rem;
}

.transaction-type-toggle__button--active,
.filter-tabs__button--active {
  background: white !important;
  box-shadow: 0 6px 16px rgb(0 0 0 / 8%);
  color: var(--color-primary-strong) !important;
}

.transaction-list {
  display: grid;
  gap: 0.6rem;
  list-style: none;
  margin: 0;
  padding: 0;
}

.transaction-list__item {
  position: relative;
  align-items: center;
  background: var(--color-surface);
  border: 1px solid color-mix(in srgb, var(--color-primary-strong) 30%, var(--color-border));
  box-shadow: 0 0 12px color-mix(in srgb, var(--color-primary-strong) 6%, transparent);
  border-radius: var(--radius-lg);
  display: grid;
  gap: 0.75rem;
  grid-template-columns: auto minmax(0, 1fr) auto;
  padding: 0.75rem;
  padding-right: 4.5rem;
}

.transaction-list__copy {
  display: grid;
  gap: 0.15rem;
  min-width: 0;
}

.transaction-list__copy strong,
.transaction-list__copy span {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.transaction-list__copy span {
  color: var(--color-muted-foreground);
  font-size: 0.86rem;
}

.transaction-list__amount {
  align-items: end;
  display: grid;
  gap: 0.35rem;
  justify-items: end;
}

.transaction-list__actions {
  position: absolute;
  top: 0.5rem;
  right: 0.5rem;
  display: flex;
  gap: 0.25rem;
  z-index: 2;
}

@media (max-width: 860px) {
  .transactions-layout {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 620px) {
  .transaction-form__row--multi {
    display: flex !important;
    flex-direction: column !important;
    gap: 0.75rem !important;
  }

  .transaction-form__amount-date-group {
    display: flex !important;
    flex-direction: row !important;
    justify-content: space-between !important;
    width: 100% !important;
    gap: 0.75rem !important;
  }

  .transaction-form__input,
  .transaction-form__amount-date-group input {
    min-width: 0 !important;
    max-width: 100% !important;
  }

  .transaction-form__amount-date-group input[type='number'] {
    flex: 1 1 auto !important;
  }

  .transaction-form__amount-date-group input[type='date'] {
    flex: 0 0 auto !important;
    text-align: right !important;
  }

  :deep(.transaction-form__row--multi button[role='combobox']) {
    width: 100% !important;
  }
}
</style>
