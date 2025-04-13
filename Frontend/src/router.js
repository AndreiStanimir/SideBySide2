import { createRouter, createWebHashHistory } from 'vue-router'

// Import views
// For now, we only have a simple single page app, but this can be expanded as needed

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('./App.vue')
    }
  ]
})

export default router 