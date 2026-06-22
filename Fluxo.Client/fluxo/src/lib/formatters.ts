export function formatMoney(amount: number, currency = 'PLN', options?: { sign?: 'auto' | 'never' }) {
  const formatter = new Intl.NumberFormat('pl-PL', {
    currency,
    maximumFractionDigits: 2,
    minimumFractionDigits: 2,
    signDisplay: options?.sign === 'auto' ? 'exceptZero' : 'never',
    style: 'currency',
  })

  return formatter.format(amount)
}

export function formatPercent(value: number) {
  return new Intl.NumberFormat('pl-PL', {
    maximumFractionDigits: 0,
    style: 'percent',
  }).format(value / 100)
}
