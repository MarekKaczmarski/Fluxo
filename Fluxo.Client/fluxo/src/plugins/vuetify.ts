import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

import { createVuetify } from 'vuetify'
import { aliases, mdi } from 'vuetify/iconsets/mdi'

export default createVuetify({
  icons: {
    aliases,
    defaultSet: 'mdi',
    sets: {
      mdi,
    },
  },
  theme: {
    defaultTheme: 'fluxo',
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
  },
})
