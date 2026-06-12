import { defineStore } from 'pinia'
import { ref } from 'vue'
import { getTransactions, updateTransaction, type TransactionDto } from '../api/transactionService'

export const useTransactionStore = defineStore('transaction', () => {
  const transactions = ref<TransactionDto[]>([])
  const selectedTransaction = ref<TransactionDto | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  async function fetchTransactions() {
    isLoading.value = true
    error.value = null
    try {
      transactions.value = await getTransactions()
    } catch (err: unknown) {
      error.value = (err as Error).message || 'An error occurred while loading'
    } finally {
      isLoading.value = false
    }
  }

  function selectTransaction(transaction: TransactionDto) {
    selectedTransaction.value = { ...transaction }
  }

  function cancelEdit() {
    selectedTransaction.value = null
  }

  async function saveTransaction(updatedData: TransactionDto) {
    isLoading.value = true
    try {
      await updateTransaction(updatedData.id, updatedData)

      const index = transactions.value.findIndex((t) => t.id === updatedData.id)
      if (index !== -1) {
        transactions.value[index] = updatedData
      }

      selectedTransaction.value = null
    } catch (err: unknown) {
      error.value = (err as Error).message || 'An error occurred while saving'
    } finally {
      isLoading.value = false
    }
  }

  return {
    transactions,
    selectedTransaction,
    isLoading,
    error,
    fetchTransactions,
    selectTransaction,
    cancelEdit,
    saveTransaction,
  }
})
