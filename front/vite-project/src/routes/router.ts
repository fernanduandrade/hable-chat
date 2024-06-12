import { createWebHistory, createRouter, RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
    { path: '/login', name: 'login', component: () => import('../Auth/views/Login.vue') },
    { path: '/register', name: 'register', component: () => import('../Auth/views/Register.vue') },
    { path: '/', name: 'index', component: () => import('../HableChat/views/index.vue')},
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

router.beforeEach((to, from, next) => {
    const getUser = sessionStorage.getItem('user')
    const getToken = sessionStorage.getItem('token')
    const isAuthenticate = getUser && getToken
    if (!isAuthenticate && to.name !== 'login' && to.name !== 'register') {
        next({ name: 'login' })
    }

    if(isAuthenticate && to.name === 'login')
        next({name: 'index'})
    else if(isAuthenticate && to.name === 'register')
        next({name: 'index'})
    else next()
})

export default router