<script lang="ts" setup>
import { useRouter } from 'vue-router';
import VButton from './VButton.vue'
const router = useRouter()
const user = JSON.parse(sessionStorage.getItem('user') || '')

function logout() {
    sessionStorage.removeItem('user')
    sessionStorage.removeItem('token')
    router.push({ name: 'login' })
}
</script>

<template>
    <header class="header justify-between gap-3">
      <h1 class="text-2xl font-bold">HableChat </h1>
      <div class="flex items-center">
        <h1 class="">Bem vindo <strong>{{ user.name }} {{ user.lastName }}</strong></h1>
        <VButton @click="logout" background-color="5D5FEF" icon="fa-arrow-right-to-bracket" />
      </div>
    </header>
</template>