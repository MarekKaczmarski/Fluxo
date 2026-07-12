<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { useRoute } from 'vue-router'
import type { VForm } from 'vuetify/components'
import { ArrowDownCircle, ArrowUpCircle, Pencil, Plus, Trash2 } from '@lucide/vue'
import CategoryIcon from '@/components/CategoryIcon.vue'
import { TransactionType, type TransactionDto } from '@/api/models'
import { useBudgetStore } from '@/stores/budgetStore'
import { formatDate, toDateInputValue } from '@/lib/dates'
import { formatMoney } from '@/lib/formatters'
import { categoryColors } from '@/lib/categoryIcons'
import { required, positiveAmount } from '@/lib/validation'

const route = useRoute()
const store = useBudgetStore()
const { accounts, categories, error, hasFieldErrors, isLoading, isSaving, transactions } =
  storeToRefs(store)

const formRef = ref<VForm | null>(null)

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
  const { valid } = await formRef.value!.validate()

  if (!valid) {
    return
  }

  const amount = Number(form.amount)

  const saved = isEditing.value
    ? await store.saveTransaction({
        amount,
        categoryId: form.categoryId,
        date: toApiDate(form.date),
        description: form.description.trim(),
        id: form.id,
        type: form.type,
      })
    : await store.addTransaction({
        accountId: form.accountId,
        amount,
        categoryId: form.categoryId,
        date: toApiDate(form.date),
        description: form.description.trim(),
        type: form.type,
      })

  if (saved) {
    resetForm()
  }
}

function blockMinusKey(event: KeyboardEvent) {
  if (event.key === '-') {
    event.preventDefault()
  }
}

function openDatePicker() {
  const input = document.getElementById('transaction-date') as HTMLInputElement
  input.showPicker()
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

    <v-alert v-if="error && !hasFieldErrors" type="error" variant="tonal">
      {{ error.message }}
    </v-alert>

    <section class="transactions-layout">
      <v-card>
        <v-card-title>{{ isEditing ? 'Edit Transaction' : 'New Transaction' }}</v-card-title>
        <v-card-text>
          <v-form
            ref="formRef"
            class="form-grid"
            validate-on="submit"
            novalidate
            @submit.prevent="submitForm"
          >
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
              <v-text-field
                id="transaction-description"
                v-model="form.description"
                :rules="[required]"
                :error-messages="store.fieldError('Description')"
              />
            </div>

            <div class="transaction-form__amount-date-group">
              <div class="field">
                <label for="transaction-amount">Amount</label>
                <v-text-field
                  id="transaction-amount"
                  v-model="form.amount"
                  step="0.01"
                  type="number"
                  :rules="[required, positiveAmount]"
                  :error-messages="store.fieldError('Amount')"
                  @keydown="blockMinusKey"
                />
              </div>
              <div class="field">
                <label for="transaction-date">Date</label>
                <v-text-field
                  id="transaction-date"
                  v-model="form.date"
                  type="date"
                  append-inner-icon="mdi-calendar"
                  :rules="[required]"
                  :error-messages="store.fieldError('Date')"
                  @click="openDatePicker"
                />
              </div>
            </div>

            <div class="form-grid form-grid--two">
              <div class="field">
                <label for="transaction-category">Category</label>
                <v-select
                  id="transaction-category"
                  v-model="form.categoryId"
                  :items="categories"
                  item-title="name"
                  item-value="id"
                  :rules="[required]"
                  :error-messages="store.fieldError('CategoryId')"
                />
              </div>
              <div class="field">
                <label for="transaction-account">Account</label>
                <v-select
                  id="transaction-account"
                  v-model="form.accountId"
                  :items="accounts"
                  item-title="name"
                  item-value="id"
                  :disabled="isEditing"
                  :rules="[required]"
                  :error-messages="store.fieldError('AccountId')"
                />
              </div>
            </div>

            <div class="form-actions">
              <v-btn
                type="submit"
                color="primary"
                :disabled="isSaving || categories.length === 0 || accounts.length === 0"
              >
                <Plus v-if="!isEditing" :size="18" />
                {{ isSaving ? 'Saving...' : isEditing ? 'Save Changes' : 'Add Transaction' }}
              </v-btn>
              <v-btn v-if="isEditing" type="button" variant="tonal" @click="resetForm">
                Cancel
              </v-btn>
            </div>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card>
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
                <v-chip
                  :color="transaction.type === TransactionType.Income ? 'success' : 'error'"
                  size="small"
                >
                  {{ transaction.type === TransactionType.Income ? 'Income' : 'Expense' }}
                </v-chip>
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
                <v-btn
                  icon
                  size="small"
                  variant="text"
                  type="button"
                  aria-label="Edit"
                  @click="editTransaction(transaction)"
                >
                  <Pencil :size="18" />
                </v-btn>
                <v-btn
                  icon
                  size="small"
                  variant="text"
                  type="button"
                  aria-label="Delete"
                  @click="removeTransaction(transaction)"
                >
                  <Trash2 :size="18" />
                </v-btn>
              </div>
            </li>
          </ul>
        </v-card-text>
      </v-card>
    </section>
  </main>
</template>

<style scoped>
.entity-view {
  max-width: 74rem;
}

.transactions-layout {
  display: grid;
  gap: 1rem;
  grid-template-columns: minmax(19rem, 0.8fr) minmax(0, 1.2fr);
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

.transaction-type-toggle button.transaction-type-toggle__button--active,
.filter-tabs button.filter-tabs__button--active {
  background: white;
  box-shadow: 0 6px 16px rgb(0 0 0 / 8%);
  color: var(--color-on-light);
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
  .transaction-form__amount-date-group {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 100%;
    gap: 0.75rem;
  }

  .transaction-form__amount-date-group > .field {
    min-width: 0;
    max-width: 100%;
  }

  .transaction-form__amount-date-group > .field:first-child {
    flex: 1 1 auto;
  }

  .transaction-form__amount-date-group > .field:last-child {
    flex: 0 0 auto;
  }
}
</style>
