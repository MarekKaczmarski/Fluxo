<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { storeToRefs } from 'pinia'
import type { VForm } from 'vuetify/components'
import { ChevronDown, Pencil, Plus, Trash2 } from '@lucide/vue'
import CategoryIcon from '@/components/CategoryIcon.vue'
import { useBudgetStore } from '@/stores/budgetStore'
import { categoryColors } from '@/lib/categoryIcons'
import { TransactionType } from '@/api/models'
import { formatMoney } from '@/lib/formatters'
import { required } from '@/lib/validation'

const store = useBudgetStore()
const { categories, error, hasFieldErrors, isLoading, isSaving, primaryCurrency, transactions } =
  storeToRefs(store)

const formRef = ref<VForm | null>(null)

const iconOptions = [
  'balance',
  'groceries',
  'mall',
  'coffee',
  'gas',
  'wallet',
  'utensils',
  'electronics',
  'education',
  'pill',
  'bus',
  'home',
  'car',
  'gift',
  'clothing',
]

const form = reactive({
  icon: 'wallet',
  id: '',
  name: '',
})

const isEditing = computed(() => Boolean(form.id))

const transactionCountByCategory = computed(() => {
  const counts = new Map<string, number>()

  for (const transaction of transactions.value) {
    counts.set(transaction.categoryId, (counts.get(transaction.categoryId) ?? 0) + 1)
  }

  return counts
})

const expandedCategoryId = ref<string | null>(null)

function toggleCategory(categoryId: string) {
  expandedCategoryId.value = expandedCategoryId.value === categoryId ? null : categoryId
}

function categoryTransactions(categoryId: string) {
  return transactions.value
    .filter((transaction) => transaction.categoryId === categoryId)
    .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
}

function formatTransactionDate(dateIso: string) {
  return new Intl.DateTimeFormat('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  }).format(new Date(dateIso))
}

onMounted(() => {
  void store.fetchAll()
})

function resetForm() {
  form.icon = 'wallet'
  form.id = ''
  form.name = ''
}

function editCategory(categoryId: string) {
  const category = categories.value.find((item) => item.id === categoryId)

  if (!category) {
    return
  }

  form.icon = category.icon ?? 'wallet'
  form.id = category.id
  form.name = category.name
}

async function submitForm() {
  const { valid } = await formRef.value!.validate()

  if (!valid) {
    return
  }

  const payload = {
    icon: form.icon || null,
    name: form.name.trim(),
  }

  if (isEditing.value) {
    await store.saveCategory({ ...payload, id: form.id })
  } else {
    await store.addCategory(payload)
  }

  resetForm()
}

async function removeCategory(categoryId: string) {
  const category = categories.value.find((item) => item.id === categoryId)

  if (category && window.confirm(`Delete category "${category.name}"?`)) {
    await store.removeCategory(categoryId)
  }
}
</script>

<template>
  <main class="entity-view">
    <header class="entity-view__header">
      <div>
        <h1 class="page-title">Categories</h1>
      </div>
    </header>

    <v-alert v-if="error && !hasFieldErrors" type="error" variant="tonal">
      {{ error.message }}
    </v-alert>

    <section class="categories-layout">
      <v-card>
        <v-card-title>{{ isEditing ? 'Edit Category' : 'New Category' }}</v-card-title>
        <v-card-text>
          <v-form
            ref="formRef"
            class="form-grid"
            validate-on="submit"
            novalidate
            @submit.prevent="submitForm"
          >
            <div class="field">
              <label for="category-name">Name</label>
              <v-text-field
                id="category-name"
                v-model="form.name"
                maxlength="50"
                :rules="[required]"
                :error-messages="store.fieldError('Name')"
              />
            </div>
            <div class="field">
              <label for="category-icon">Icon</label>
              <v-select
                id="category-icon"
                v-model="form.icon"
                :items="iconOptions"
                :error-messages="store.fieldError('Icon')"
              />
            </div>
            <div class="category-preview">
              <CategoryIcon :icon="form.icon" :color="categoryColors[1]" :size="38" />
              <strong>{{ form.name || 'Category' }}</strong>
            </div>
            <div class="form-actions">
              <v-btn type="submit" color="primary" :disabled="isSaving">
                <Plus v-if="!isEditing" :size="18" />
                {{ isSaving ? 'Saving...' : isEditing ? 'Save Changes' : 'Add Category' }}
              </v-btn>
              <v-btn v-if="isEditing" type="button" variant="tonal" @click="resetForm">
                Cancel
              </v-btn>
            </div>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card>
        <v-card-title>Category List</v-card-title>
        <v-card-subtitle>{{ categories.length }} positions</v-card-subtitle>
        <v-card-text>
          <p v-if="isLoading && categories.length === 0" class="state-message">
            Loading categories...
          </p>
          <ul v-else class="category-list">
            <li
              v-for="(category, index) in categories"
              :key="category.id"
              class="category-list__item"
            >
              <div class="category-list__row">
                <button
                  type="button"
                  class="category-list__summary"
                  :aria-expanded="expandedCategoryId === category.id"
                  @click="toggleCategory(category.id)"
                >
                  <CategoryIcon
                    :color="categoryColors[index % categoryColors.length]"
                    :icon="category.icon ?? category.name"
                    :size="34"
                  />
                  <span class="category-list__info">
                    <strong>{{ category.name }}</strong>
                    <span>{{ transactionCountByCategory.get(category.id) ?? 0 }} transactions</span>
                  </span>
                  <ChevronDown
                    :size="18"
                    class="category-list__chevron"
                    :class="{ 'category-list__chevron--open': expandedCategoryId === category.id }"
                  />
                </button>

                <div class="category-list__actions">
                  <v-btn
                    icon
                    size="small"
                    variant="text"
                    type="button"
                    aria-label="Edit"
                    @click="editCategory(category.id)"
                  >
                    <Pencil :size="18" />
                  </v-btn>
                  <v-btn
                    icon
                    size="small"
                    variant="text"
                    type="button"
                    aria-label="Delete"
                    @click="removeCategory(category.id)"
                  >
                    <Trash2 :size="18" />
                  </v-btn>
                </div>
              </div>

              <div v-if="expandedCategoryId === category.id" class="category-list__transactions">
                <p
                  v-if="categoryTransactions(category.id).length === 0"
                  class="category-list__empty"
                >
                  No transactions in this category yet.
                </p>
                <ul v-else class="category-list__transaction-list">
                  <li
                    v-for="transaction in categoryTransactions(category.id)"
                    :key="transaction.id"
                  >
                    <span class="category-list__transaction-description">{{
                      transaction.description
                    }}</span>
                    <span class="category-list__transaction-date">{{
                      formatTransactionDate(transaction.date)
                    }}</span>
                    <span
                      class="category-list__transaction-amount"
                      :class="
                        transaction.type === TransactionType.Income
                          ? 'amount-income'
                          : 'amount-expense'
                      "
                    >
                      {{ transaction.type === TransactionType.Income ? '+' : '-'
                      }}{{ formatMoney(transaction.amount, primaryCurrency) }}
                    </span>
                  </li>
                </ul>
              </div>
            </li>
          </ul>
        </v-card-text>
      </v-card>
    </section>
  </main>
</template>

<style scoped>
.categories-layout {
  display: grid;
  gap: 1rem;
  grid-template-columns: minmax(18rem, 0.75fr) minmax(0, 1.25fr);
}

.category-preview {
  align-items: center;
  background: var(--color-surface);
  border: 1px dashed var(--color-border-strong);
  border-radius: var(--radius-lg);
  display: flex;
  gap: 0.75rem;
  padding: 0.85rem;
}

.category-list__item {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  overflow: hidden;
}

.category-list__row {
  align-items: center;
  display: grid;
  gap: 0.75rem;
  grid-template-columns: 1fr auto;
  padding: 0.8rem;
}

.category-list__summary {
  align-items: center;
  background: transparent;
  border: 0;
  color: inherit;
  cursor: pointer;
  display: grid;
  font: inherit;
  gap: 0.75rem;
  grid-template-columns: auto 1fr auto;
  padding: 0;
  text-align: left;
  width: 100%;
}

.category-list__info {
  display: grid;
  gap: 0.1rem;
}

.category-list__info span {
  color: var(--color-muted-foreground);
  font-size: 0.86rem;
}

.category-list__chevron {
  color: var(--color-muted-foreground);
  transition: transform 0.15s ease;
}

.category-list__chevron--open {
  transform: rotate(180deg);
}

.category-list__actions {
  display: flex;
}

.category-list__transactions {
  background: color-mix(in srgb, var(--color-surface) 88%, var(--color-foreground) 4%);
  border-top: 1px solid var(--color-border);
  padding: 0.7rem 0.9rem 0.9rem;
}

.category-list__empty {
  color: var(--color-muted-foreground);
  font-size: 0.86rem;
  margin: 0;
}

.category-list__transaction-list {
  display: grid;
  gap: 0.5rem;
  list-style: none;
  margin: 0;
  padding: 0;
}

.category-list__transaction-list li {
  align-items: baseline;
  display: grid;
  gap: 0.75rem;
  grid-template-columns: 1fr auto auto;
}

.category-list__transaction-description {
  font-weight: 600;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.category-list__transaction-date {
  color: var(--color-muted-foreground);
  font-size: 0.82rem;
  white-space: nowrap;
}

.category-list__transaction-amount {
  font-weight: 700;
  white-space: nowrap;
}

@media (max-width: 780px) {
  .categories-layout,
  .category-list__row {
    grid-template-columns: 1fr;
  }

  .category-list__transaction-list li {
    grid-template-columns: 1fr;
  }
}
</style>
