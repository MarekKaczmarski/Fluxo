export interface TransactionDto {
  id: string
  description: string
  amount: number
  currency: string
  date: string
  categoryId: string
  type: number
}

const API_BASE_URL = import.meta.env.VITE_API_URL

export async function getTransactions(): Promise<TransactionDto[]> {
  const response = await fetch(`${API_BASE_URL}/transactions`)

  if (!response.ok) {
    throw new Error('An error occurred while fetching transactions')
  }

  return response.json()
}

export async function updateTransaction(id: string, data: Partial<TransactionDto>): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/transactions/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  })

  if (!response.ok) {
    throw new Error('An error occurred while updating the transaction')
  }
}
