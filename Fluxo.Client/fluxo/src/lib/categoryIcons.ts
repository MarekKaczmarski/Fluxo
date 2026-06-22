import type { Component } from 'vue'
import {
  Book,
  Bus,
  Car,
  Cat,
  CircleDollarSign,
  CircleHelp,
  Coffee,
  CreditCard,
  Fuel,
  Gift,
  GlassWater,
  Home,
  Hospital,
  Phone,
  Pill,
  ReceiptText,
  Shirt,
  ShoppingBag,
  ShoppingCart,
  Smartphone,
  Tags,
  Thermometer,
  Train,
  Utensils,
  Wallet,
} from '@lucide/vue'

const icons: Record<string, Component> = {
  account: CreditCard,
  balance: CircleDollarSign,
  bus: Bus,
  car: Car,
  cat: Cat,
  clothing: Shirt,
  coffee: Coffee,
  education: Book,
  electronics: Smartphone,
  food: Utensils,
  gas: Fuel,
  gift: Gift,
  glass: GlassWater,
  groceries: ShoppingCart,
  health: Thermometer,
  home: Home,
  mall: ShoppingBag,
  phone: Phone,
  pharmacy: Hospital,
  pill: Pill,
  receipt: ReceiptText,
  shopping: Tags,
  smartphone: Smartphone,
  train: Train,
  transfer: Wallet,
  transport: Bus,
  utensils: Utensils,
  wallet: Wallet,
}

export const categoryColors = [
  '#6bb7e2',
  '#6fd097',
  '#ef6f7b',
  '#d89f32',
  '#a66bc8',
  '#78899d',
  '#5fbea9',
  '#e879a8',
  '#8aa55f',
  '#4f9ed5',
]

export function getCategoryIcon(name?: string | null) {
  if (!name) {
    return CircleHelp
  }

  return icons[name.toLowerCase()] ?? CircleHelp
}
