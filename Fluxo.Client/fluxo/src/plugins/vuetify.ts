import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

import { createVuetify } from 'vuetify'
import { aliases, mdi } from 'vuetify/iconsets/mdi'

const THEME_STORAGE_KEY = 'fluxo-theme'

export default createVuetify({
  icons: {
    aliases,
    defaultSet: 'mdi',
    sets: {
      mdi,
    },
  },
  theme: {
    defaultTheme: localStorage.getItem(THEME_STORAGE_KEY) === 'fluxoDark' ? 'fluxoDark' : 'fluxo',
    themes: {
      fluxo: {
        dark: false,
        colors: {
          background: '#f4f4f5',
          surface: '#ffffff',
          'surface-variant': '#e4e4e7',
          primary: '#18181b',
          'primary-darken-1': '#000000',
          secondary: '#52525b',
          error: '#dc2626',
          success: '#16a34a',
          warning: '#d97706',
          info: '#2563eb',
        },
        variables: {
          'border-color': '#0a0a0a',
          'border-opacity': 0.12,
        },
      },
      fluxoDark: {
        dark: true,
        colors: {
          background: '#0a0a0b',
          surface: '#18181b',
          'surface-variant': '#27272a',
          primary: '#3f3f46',
          'primary-darken-1': '#000000',
          secondary: '#a1a1aa',
          error: '#dc2626',
          success: '#16a34a',
          warning: '#d97706',
          info: '#2563eb',
        },
        variables: {
          'border-color': '#e4e4e7',
          'border-opacity': 0.12,
        },
      },
    },
  },
  defaults: {
    VCard: {
      rounded: 'lg',
      elevation: 0,
      border: true,
    },
    VBtn: {
      rounded: 'lg',
    },
    VChip: {
      rounded: 'lg',
      variant: 'tonal',
    },
    VTextField: {
      variant: 'outlined',
      density: 'compact',
      hideDetails: 'auto',
    },
    VSelect: {
      variant: 'outlined',
      density: 'compact',
      hideDetails: 'auto',
    },
  },
})
