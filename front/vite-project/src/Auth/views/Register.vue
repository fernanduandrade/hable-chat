<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import VInput from '../../common/components/VInput.vue'
import VButton from '../../common/components/VButton.vue'
import useFetch from '../../composables/useFetch';

const form = reactive({
    email: '',
    password: '',
    userName: '',
    name: '',
    lastName: ''
})

const router = useRouter()
const registerError = ref('')
const register = async () => {
    const { fetchData } = useFetch<any>('https://localhost:7266/api/auth/login', { method: 'POST', body: JSON.stringify(form)})
    await fetchData()

    
    router.push({ name: 'login' })
    
    
}

function goToLogin() {
    router.push({ 'name': 'login' })
}

</script>

<template>
    <main class="auth__container">
        <div class="wrapper-form flex flex-col gap-2">
            <span class="text-sm text-blue-600 hover:cursor-pointer" @click="goToLogin">&#x2190; Voltar</span>
            <span class="text-[#696868]">Insira seus dados abaixo</span>
            
            <div class="mt-4 flex flex-col gap-4">
                <div>
                    <label class="font-bold">Nome de usuário</label>
                    <VInput type="text" v-model="form.userName" placeholder="Nome de usuário" />
                </div>
                <div class="flex gap-5 justify-between">
                    <div class="w-full">
                        <label class="font-bold">Nome</label>
                        <VInput type="text" v-model="form.name" placeholder="Nome" />
                    </div>

                    <div class="w-full">
                      <label class="font-bold">Sobrenome</label>
                      <VInput type="text" v-model="form.lastName" placeholder="Sobrenome" />
                    </div>
                </div>
                <div>
                    <label class="font-bold">Email</label>
                    <VInput type="email" v-model="form.email" placeholder="Email" />
                </div>
                <div>
                    <label class="font-bold">Senha</label>
                    <VInput type="password" v-model="form.password" placeholder="Senha" />
                </div>
            </div>
            <VButton @click="register">Cadastrar</VButton>
        </div>
        <span v-if="registerError" class="text-red-600"> {{ registerError }}</span>
        

    </main>
</template>

<style scoped>


.wrapper-form {
    min-width: 520px;
    min-height: 400px;
    @apply flex flex-col shadow-lg bg-white p-4 rounded-lg
}

</style>
