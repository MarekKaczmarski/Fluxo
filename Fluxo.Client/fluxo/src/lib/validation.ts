export function required(value: string) {
  return value.trim().length > 0 || 'This field is required.'
}

export function positiveAmount(value: string) {
  const amount = Number(value)
  return (Number.isFinite(amount) && amount > 0) || 'Enter a positive amount.'
}
