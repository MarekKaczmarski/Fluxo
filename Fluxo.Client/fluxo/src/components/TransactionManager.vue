<script setup lang="ts">
import { onMounted } from 'vue'
import { useTransactionStore } from '../stores/transactionStore'

const transactionStore = useTransactionStore()

onMounted(() => {
  transactionStore.fetchTransactions()
})

const handleSubmit = () => {
  if (transactionStore.selectedTransaction) {
    transactionStore.saveTransaction(transactionStore.selectedTransaction)
  }
}
</script>

<template>
  <div class="transaction-manager">
    <div v-if="transactionStore.error" class="error-message">
      ⚠️ Error: {{ transactionStore.error }}
    </div>

    <section v-if="transactionStore.selectedTransaction" class="edit-form-box">
      <h3>Edit Transaction</h3>
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label>Description:</label>
          <input v-model="transactionStore.selectedTransaction.description" type="text" required />
        </div>

        <div class="form-group">
          <label>Amount:</label>
          <input
            v-model.number="transactionStore.selectedTransaction.amount"
            type="number"
            step="0.01"
            required
          />
        </div>

        <div class="form-buttons">
          <button type="submit" class="btn btn-success" :disabled="transactionStore.isLoading">
            {{ transactionStore.isLoading ? 'Saving...' : 'Save Changes' }}
          </button>
          <button type="button" class="btn btn-secondary" @click="transactionStore.cancelEdit">
            Cancel
          </button>
        </div>
      </form>
    </section>

    <section>
      <p v-if="transactionStore.isLoading && transactionStore.transactions.length === 0">
        Loading transactions...
      </p>

      <table v-else class="transaction-table">
        <thead>
          <tr>
            <th>Description</th>
            <th>Amount</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="transaction in transactionStore.transactions" :key="transaction.id">
            <td>{{ transaction.description }}</td>
            <td>{{ transaction.amount }} {{ transaction.currency }}</td>
            <td>
              <button
                class="btn btn-primary"
                @click="transactionStore.selectTransaction(transaction)"
              >
                Edit
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </section>
  </div>
</template>

<style scoped>
.transaction-manager {
  width: 100%;
}
.transaction-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}
.transaction-table th,
.transaction-table td {
  border: 1px solid #ddd;
  padding: 12px;
  text-align: left;
}
.edit-form-box {
  background: #f9f9f9;
  border: 1px solid #ccc;
  padding: 1.5rem;
  border-radius: 6px;
  margin-bottom: 2rem;
}
.form-group {
  margin-bottom: 1rem;
  display: flex;
  flex-direction: column;
}
.form-group label {
  font-weight: bold;
  margin-bottom: 0.5rem;
}
.form-group input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.form-buttons {
  display: flex;
  gap: 10px;
}
.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: bold;
}
.btn-primary {
  background: #007bff;
  color: white;
}
.btn-success {
  background: #28a745;
  color: white;
}
.btn-secondary {
  background: #6c757d;
  color: white;
}
.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.error-message {
  color: #dc3545;
  padding: 1rem;
  background: #f8d7da;
  border-radius: 4px;
  margin-bottom: 1rem;
}
</style>
