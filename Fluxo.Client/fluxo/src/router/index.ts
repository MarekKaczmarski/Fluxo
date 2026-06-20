import { createRouter, createWebHistory } from 'vue-router'
import AccountsView from '@/views/AccountsView.vue'
import CategoriesView from '@/views/CategoriesView.vue'
import DashboardView from '@/views/DashboardView.vue'
import TransactionsView from '@/views/TransactionsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      component: DashboardView,
      path: '/',
    },
    {
      component: TransactionsView,
      path: '/transactions',
    },
    {
      component: CategoriesView,
      path: '/categories',
    },
    {
      component: AccountsView,
      path: '/accounts',
    },
  ],
})

export default router
