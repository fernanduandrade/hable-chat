<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import VInput from '../../common/components/VInput.vue'
import VButton from '../../common/components/VButton.vue'
import userApplicationStore from '../../stores/index'
import useFetch from '../../composables/useFetch'
const form = reactive({
    email: '',
    password: ''
})

const appStore = userApplicationStore()
const router = useRouter()
const loginErrorMessage = ref('')
const login = async () => {
    const { data, fetchData } = useFetch<any>('https://localhost:7266/api/auth/login', { method: 'POST', body: JSON.stringify(form)})
    await fetchData()

    const result = data.value
    sessionStorage.setItem('token', JSON.stringify(result.token))
    sessionStorage.setItem('refreshToken', JSON.stringify(result.refreshToken))
    sessionStorage.setItem('user', JSON.stringify(result.user))
    appStore.setUser(result.user)
    loginErrorMessage.value = ''
    router.push({name: 'index'})
}

function goToRegister() {
    router.push({ name: 'register' })
}

</script>

<template>
    <main class="auth__container">
        <div class="wrapper-form">
            <h1 class="uppercase text-4xl font-bold">HABLE CHAT</h1>
            <span class="text-[#696868]">Insira seus dados</span>
            <div class="mt-4 flex flex-col gap-4">
                <div>
                    <label class="font-bold">Email</label>
                    <VInput type="text" v-model="form.email" placeholder="Email" label="Email" />
                </div>
                
                <div>
                    <label class="font-bold">Senha</label>
                    <VInput type="password" v-model="form.password" placeholder="Senha" label="Senha" />
                </div>

                <div class="w-full">
                    <VButton @click="login">Login</VButton>
                </div>

                <span v-if="loginErrorMessage" class="error-message">{{ loginErrorMessage }}</span>
                
                <div class="tracking-normal before:content-['-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-'] after:content-['-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-'] font-bold flex justify-center items-center">
                    <span class=" text-blue-600 text-xl font-semibold hover:cursor-pointer" @click="goToRegister">Cadastra-se aqui</span>
                </div>
                
            </div>
            
        
        </div>
    </main>
</template>

<style scoped>


.wrapper-form {
    min-width: 520px;
    min-height: 400px;
    @apply flex flex-col shadow-lg bg-white p-4 rounded-lg
}

</style>