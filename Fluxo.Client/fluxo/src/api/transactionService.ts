import { apiRequest } from './apiClient'
import type { CreateTransactionRequest, TransactionDto, UpdateTransactionRequest } from './models'

export type { TransactionDto } from './models'

export async function getTransactions(): Promise<TransactionDto[]> {
  return apiRequest<TransactionDto[]>('/transactions')
}

export async function createTransaction(data: CreateTransactionRequest): Promise<string> {
  return apiRequest<string>('/transactions', {
    method: 'POST',
    body: JSON.stringify(data),
  })
}

export async function updateTransaction(id: string, data: UpdateTransactionRequest): Promise<void> {
  await apiRequest<void>(`/transactions/${id}`, {
    method: 'PUT',
    body: JSON.stringify(data),
  })
}

export async function deleteTransaction(id: string): Promise<void> {
  await apiRequest<void>(`/transactions/${id}`, {
    method: 'DELETE',
  })
}
