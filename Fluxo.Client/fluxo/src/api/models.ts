export enum TransactionType {
  Expense = 0,
  Income = 1,
}

export interface TransactionDto {
  id: string
  description: string
  amount: number
  currency: string
  date: string
  categoryId: string
  categoryName: string
  type: TransactionType
}

export interface CategoryDto {
  id: string
  name: string
  icon: string | null
}

export interface AccountDto {
  id: string
  name: string
  balance: number
  currency: string
}

export interface CreateTransactionRequest {
  description: string
  amount: number
  date: string
  categoryId: string
  accountId: string
  type: TransactionType
}

export interface UpdateTransactionRequest {
  id: string
  description: string
  amount: number
  date: string
  categoryId: string
  type: TransactionType
}

export interface CreateCategoryRequest {
  name: string
  icon: string | null
}

export interface UpdateCategoryRequest extends CreateCategoryRequest {
  id: string
}

export interface CreateAccountRequest {
  name: string
  initialBalance: number
  currency: string
}

export interface UpdateAccountRequest {
  id: string
  name: string
}
