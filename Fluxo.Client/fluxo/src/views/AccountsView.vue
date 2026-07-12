<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { storeToRefs } from 'pinia'
import type { VForm } from 'vuetify/components'
import { CreditCard, Pencil, Plus, Trash2 } from '@lucide/vue'
import { useBudgetStore } from '@/stores/budgetStore'
import { formatMoney } from '@/lib/formatters'
import { required } from '@/lib/validation'

const store = useBudgetStore()
const { accounts, error, hasFieldErrors, isLoading, isSaving, primaryCurrency, totalBalance } =
  storeToRefs(store)

const formRef = ref<VForm | null>(null)

const form = reactive({
  currency: 'PLN',
  id: '',
  initialBalance: '0',
  name: '',
})

const isEditing = computed(() => Boolean(form.id))

onMounted(() => {
  void store.fetchAll()
})

function resetForm() {
  form.currency = 'PLN'
  form.id = ''
  form.initialBalance = '0'
  form.name = ''
}

function editAccount(accountId: string) {
  const account = accounts.value.find((item) => item.id === accountId)

  if (!account) {
    return
  }

  form.currency = account.currency
  form.id = account.id
  form.initialBalance = String(account.balance)
  form.name = account.name
}

async function submitForm() {
  const { valid } = await formRef.value!.validate()

  if (!valid) {
    return
  }

  const name = form.name.trim()

  const saved = isEditing.value
    ? await store.saveAccount({
        id: form.id,
        name,
      })
    : await store.addAccount({
        currency: form.currency.trim().toUpperCase(),
        initialBalance: Number(form.initialBalance) || 0,
        name,
      })

  if (saved) {
    resetForm()
  }
}

async function removeAccount(accountId: string) {
  const account = accounts.value.find((item) => item.id === accountId)

  if (account && window.confirm(`Delete account "${account.name}"?`)) {
    await store.removeAccount(accountId)
  }
}
</script>

<template>
  <main class="entity-view">
    <header class="entity-view__header">
      <div>
        <h1 class="page-title">Accounts</h1>
      </div>
      <div class="accounts-total balance-pill">
        <span>Balance</span>
        <strong>{{ formatMoney(totalBalance, primaryCurrency) }}</strong>
      </div>
    </header>

    <v-alert v-if="error && !hasFieldErrors" type="error" variant="tonal">
      {{ error.message }}
    </v-alert>

    <section class="accounts-layout">
      <v-card>
        <v-card-title>{{ isEditing ? 'Edit Account' : 'New Account' }}</v-card-title>
        <v-card-text>
          <v-form
            ref="formRef"
            class="form-grid"
            validate-on="submit"
            novalidate
            @submit.prevent="submitForm"
          >
            <div class="field">
              <label for="account-name">Name</label>
              <v-text-field
                id="account-name"
                v-model="form.name"
                maxlength="100"
                :rules="[required]"
                :error-messages="store.fieldError('Name')"
              />
            </div>
            <div class="form-grid form-grid--two">
              <div class="field">
                <label for="account-balance">Initial Balance</label>
                <v-text-field
                  id="account-balance"
                  v-model="form.initialBalance"
                  :disabled="isEditing"
                  step="0.01"
                  type="number"
                  :error-messages="store.fieldError('InitialBalance')"
                />
              </div>
              <div class="field">
                <label for="account-currency">Currency</label>
                <v-text-field
                  id="account-currency"
                  v-model="form.currency"
                  :disabled="isEditing"
                  maxlength="3"
                  :error-messages="store.fieldError('Currency')"
                />
              </div>
            </div>
            <div class="form-actions">
              <v-btn type="submit" color="primary" :disabled="isSaving">
                <Plus v-if="!isEditing" :size="18" />
                {{ isSaving ? 'Saving...' : isEditing ? 'Save Name' : 'Add Account' }}
              </v-btn>
              <v-btn v-if="isEditing" type="button" variant="tonal" @click="resetForm">
                Cancel
              </v-btn>
            </div>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card>
        <v-card-title>Account List</v-card-title>
        <v-card-subtitle>{{ accounts.length }} positions</v-card-subtitle>
        <v-card-text>
          <p v-if="isLoading && accounts.length === 0" class="state-message">Loading accounts...</p>
          <ul v-else class="account-list">
            <li v-for="account in accounts" :key="account.id" class="account-list__item">
              <span class="account-list__icon">
                <CreditCard :size="28" />
              </span>
              <div>
                <strong>{{ account.name }}</strong>
                <span>{{ account.currency }}</span>
              </div>
              <strong>{{ formatMoney(account.balance, account.currency) }}</strong>
              <div class="account-list__actions">
                <v-btn
                  icon
                  size="small"
                  variant="text"
                  type="button"
                  aria-label="Edit"
                  @click="editAccount(account.id)"
                >
                  <Pencil :size="18" />
                </v-btn>
                <v-btn
                  icon
                  size="small"
                  variant="text"
                  type="button"
                  aria-label="Delete"
                  @click="removeAccount(account.id)"
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
.accounts-total {
  align-items: end;
  display: grid;
  min-width: 12rem;
  padding: 0.8rem 1rem;
}

.accounts-total span {
  font-weight: 800;
  opacity: 0.86;
}

.accounts-total strong {
  font-size: 1.2rem;
}

.accounts-layout {
  display: grid;
  gap: 1rem;
  grid-template-columns: minmax(18rem, 0.75fr) minmax(0, 1.25fr);
}

.account-list__item {
  align-items: center;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  display: grid;
  gap: 0.75rem;
  grid-template-columns: auto 1fr auto auto;
  padding: 0.8rem;
}

.account-list__icon {
  align-items: center;
  background: color-mix(in srgb, var(--color-primary) 14%, white);
  border-radius: 999px;
  color: var(--color-on-light);
  display: inline-flex;
  height: 3rem;
  justify-content: center;
  width: 3rem;
}

.account-list__item div:nth-child(2) {
  display: grid;
  gap: 0.1rem;
}

.account-list__item span {
  color: var(--color-muted-foreground);
  font-size: 0.86rem;
}

.account-list__actions {
  display: flex;
}

@media (max-width: 780px) {
  .entity-view__header,
  .accounts-layout,
  .account-list__item {
    align-items: stretch;
    grid-template-columns: 1fr;
  }

  .entity-view__header {
    display: grid;
  }
}
</style>
