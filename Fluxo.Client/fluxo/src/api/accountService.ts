import { apiRequest } from './apiClient'
import type { AccountDto, CreateAccountRequest, UpdateAccountRequest } from './models'

export async function getAccounts(): Promise<AccountDto[]> {
  return apiRequest<AccountDto[]>('/accounts')
}

export async function createAccount(data: CreateAccountRequest): Promise<string> {
  return apiRequest<string>('/accounts', {
    method: 'POST',
    body: JSON.stringify(data),
  })
}

export async function updateAccount(id: string, data: UpdateAccountRequest): Promise<void> {
  await apiRequest<void>(`/accounts/${id}`, {
    method: 'PUT',
    body: JSON.stringify(data),
  })
}

export async function deleteAccount(id: string): Promise<void> {
  await apiRequest<void>(`/accounts/${id}`, {
    method: 'DELETE',
  })
}
