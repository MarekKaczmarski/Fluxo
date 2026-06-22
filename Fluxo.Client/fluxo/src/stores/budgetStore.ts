import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { ApiError } from '@/api/apiClient'
import { 
  createAccount, 
  deleteAccount, 
  getAccounts, 
  updateAccount,
} from '@/api/accountService'

import {
  createCategory,
  deleteCategory,
  getCategories,
  updateCategory,
} from '@/api/categoryService'

import {
  createTransaction,
  deleteTransaction,
  getTransactions,
  updateTransaction,
} from '@/api/transactionService'

import type {
  AccountDto,
  CategoryDto,
  CreateAccountRequest,
  CreateCategoryRequest,
  CreateTransactionRequest,
  TransactionDto,
  UpdateAccountRequest,
  UpdateCategoryRequest,
  UpdateTransactionRequest,
} from '@/api/models'

interface ApiValidationError {
  message: string
  fields: Record<string, string[]>
}

export const useBudgetStore = defineStore('budget', () => {
  const transactions = ref<TransactionDto[]>([])
  const categories = ref<CategoryDto[]>([])
  const accounts = ref<AccountDto[]>([])
  const isLoading = ref(false)
  const isSaving = ref(false)
  const error = ref<ApiValidationError | null>(null)

  const totalBalance = computed(() =>
    accounts.value.reduce((sum, account) => sum + account.balance, 0),
  )

  const primaryCurrency = computed(
    () => accounts.value[0]?.currency ?? transactions.value[0]?.currency ?? 'PLN',
  )

  async function runSaving(task: () => Promise<void>) {
    isSaving.value = true
    error.value = null

    try {
      await task()
      await fetchAll()
    } catch (err: unknown) {
      console.log('Error during saving:', err)
      if (err instanceof ApiError && err.status === 400) {
        const serverResponse = err.data as {
          title?: string
          errors?: Record<string, string[]>
        }

        error.value = {
          message: serverResponse?.title || 'Validation Error',
          fields: serverResponse?.errors || {},
        }
      } else {
        error.value = {
          message: err instanceof Error ? err.message : 'An unexpected error occurred.',
          fields: {},
        }
      }
    } finally {
      isSaving.value = false
    }
  }

  async function fetchAll() {
    isLoading.value = true
    error.value = null

    try {
      const [nextTransactions, nextCategories, nextAccounts] = await Promise.all([
        getTransactions(),
        getCategories(),
        getAccounts(),
      ])

      transactions.value = nextTransactions
      categories.value = nextCategories
      accounts.value = nextAccounts
    } catch (err) {
      error.value = {
        message: 'Failed to load budget data',
        fields: {},
      }
    } finally {
      isLoading.value = false
    }
  }

  async function addTransaction(data: CreateTransactionRequest) {
    await runSaving(async () => {
      await createTransaction(data)
    })
  }

  async function saveTransaction(data: UpdateTransactionRequest) {
    await runSaving(async () => {
      await updateTransaction(data.id, data)
    })
  }

  async function removeTransaction(id: string) {
    await runSaving(async () => {
      await deleteTransaction(id)
    })
  }

  async function addCategory(data: CreateCategoryRequest) {
    await runSaving(async () => {
      await createCategory(data)
    })
  }

  async function saveCategory(data: UpdateCategoryRequest) {
    await runSaving(async () => {
      await updateCategory(data.id, data)
    })
  }

  async function removeCategory(id: string) {
    await runSaving(async () => {
      await deleteCategory(id)
    })
  }

  async function addAccount(data: CreateAccountRequest) {
    await runSaving(async () => {
      await createAccount(data)
    })
  }

  async function saveAccount(data: UpdateAccountRequest) {
    await runSaving(async () => {
      await updateAccount(data.id, data)
    })
  }

  async function removeAccount(id: string) {
    await runSaving(async () => {
      await deleteAccount(id)
    })
  }

  return {
    transactions,
    categories,
    accounts,
    isLoading,
    isSaving,
    error,
    totalBalance,
    primaryCurrency,
    fetchAll,
    addTransaction,
    saveTransaction,
    removeTransaction,
    addCategory,
    saveCategory,
    removeCategory,
    addAccount,
    saveAccount,
    removeAccount,
  }
})
