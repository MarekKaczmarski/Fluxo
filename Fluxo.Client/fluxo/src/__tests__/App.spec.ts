import { describe, it, expect } from 'vitest'

import { mount } from '@vue/test-utils'
import App from '../App.vue'

describe('App', () => {
  it('renders the application shell', () => {
    const wrapper = mount(App, {
      global: {
        stubs: ['RouterLink', 'RouterView'],
      },
    })

    expect(wrapper.find('.app-shell').exists()).toBe(true)
  })
})
