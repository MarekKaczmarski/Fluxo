import type { TransactionDto } from '@/api/models'

export function parseDate(value: string) {
  return new Date(value)
}

export function monthKey(date: Date) {
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`
}

export function addMonths(date: Date, amount: number) {
  return new Date(date.getFullYear(), date.getMonth() + amount, 1)
}

export function isSameMonth(date: Date, month: Date) {
  return date.getFullYear() === month.getFullYear() && date.getMonth() === month.getMonth()
}

export function formatMonth(date: Date, variant: 'long' | 'short' = 'long') {
  return new Intl.DateTimeFormat('en-US', {
    month: variant,
  }).format(date)
}

export function formatDate(value: string) {
  return new Intl.DateTimeFormat('en-US', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  }).format(parseDate(value))
}

export function toDateInputValue(date: Date) {
  return date.toISOString().slice(0, 10)
}

export function latestTransactionMonth(transactions: TransactionDto[]) {
  const latest = transactions
    .map((transaction) => parseDate(transaction.date))
    .sort((a, b) => b.getTime() - a.getTime())[0]

  return latest ? new Date(latest.getFullYear(), latest.getMonth(), 1) : new Date()
}
