<script setup lang="ts">
import { computed, onMounted, reactive } from 'vue'
import { storeToRefs } from 'pinia'
import { CreditCard, Pencil, Plus, Trash2 } from '@lucide/vue'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { useBudgetStore } from '@/stores/budgetStore'
import { formatMoney } from '@/lib/formatters'

const store = useBudgetStore()
const { accounts, error, isLoading, isSaving, primaryCurrency, totalBalance } = storeToRefs(store)

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
  const name = form.name.trim()

  if (!name) {
    return
  }

  if (isEditing.value) {
    await store.saveAccount({
      id: form.id,
      name,
    })
  } else {
    await store.addAccount({
      currency: form.currency.trim().toUpperCase(),
      initialBalance: Number(form.initialBalance) || 0,
      name,
    })
  }

  resetForm()
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
      <div class="accounts-total">
        <span>Balance</span>
        <strong>{{ formatMoney(totalBalance, primaryCurrency) }}</strong>
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

    <section class="accounts-layout">
      <Card>
        <v-card-title>{{ isEditing ? 'Edit Account' : 'New Account' }}</v-card-title>
        <v-card-text>
          <form class="form-grid" @submit.prevent="submitForm">
            <div class="field">
              <label for="account-name">Name</label>
              <Input id="account-name" v-model="form.name" maxlength="100" required />
            </div>
            <div class="form-grid form-grid--two">
              <div class="field">
                <label for="account-balance">Initial Balance</label>
                <Input
                  id="account-balance"
                  v-model="form.initialBalance"
                  :disabled="isEditing"
                  step="0.01"
                  type="number"
                />
              </div>
              <div class="field">
                <label for="account-currency">Currency</label>
                <Input
                  id="account-currency"
                  v-model="form.currency"
                  :disabled="isEditing"
                  maxlength="3"
                />
              </div>
            </div>
            <div class="form-actions">
              <Button type="submit" :disabled="isSaving">
                <Plus v-if="!isEditing" :size="18" />
                {{ isSaving ? 'Saving...' : isEditing ? 'Save Name' : 'Add Account' }}
              </Button>
              <Button v-if="isEditing" type="button" variant="secondary" @click="resetForm">
                Cancel
              </Button>
            </div>
          </form>
        </v-card-text>
      </Card>

      <Card>
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
                <Button
                  size="icon"
                  variant="ghost"
                  type="button"
                  aria-label="Edit"
                  @click="editAccount(account.id)"
                >
                  <Pencil :size="18" />
                </Button>
                <Button
                  size="icon"
                  variant="ghost"
                  type="button"
                  aria-label="Delete"
                  @click="removeAccount(account.id)"
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
  max-width: 66rem;
  min-height: 100vh;
  padding: 2rem clamp(1rem, 4vw, 2rem) 7.75rem;
}

.entity-view__header {
  align-items: end;
  display: flex;
  gap: 1rem;
  justify-content: space-between;
  margin-bottom: 1.25rem;
}

.accounts-total {
  align-items: end;
  background: var(--color-primary);
  border-radius: var(--radius-md);
  color: white;
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

.form-grid--two {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.form-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
}

.account-list {
  display: grid;
  gap: 0.65rem;
  list-style: none;
  margin: 0;
  padding: 0;
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
  color: var(--color-primary-strong);
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
