<script lang="ts" setup>
import { markRaw, onMounted, watch } from 'vue'
import { storeToRefs } from 'pinia'
import useApplicationStore from '../../stores/index'
import useModal from '../../composables/useModal'
import CreateForm from '../components/CreateForm.vue'
import { Channel } from '../../types'

const modal = useModal()
const appStore = useApplicationStore()
const { modalEmitValue } = storeToRefs(modal)
const { guilds } = storeToRefs(appStore)
const getGuilds = async() => {
    const response = await fetch('https://localhost:7266/api/guilds', {
        headers: {
            Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
        }
    })

    const result = await response.json()
    appStore.setGuilds(result)
}

function createGuild() {
    modal.open({ component: markRaw(CreateForm), title: 'Crie um novo servidor' })
}

watch(modalEmitValue, async _ => {
    await getGuilds()
})

onMounted(async() =>{
    await getGuilds()
})

async function getChannels() {
    const guildId = appStore.selectedGuild
    const response = await fetch(`https://localhost:7266/api/guilds/${guildId}/channels`, {
        headers: {
            Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
        }
    })

    const channels = await response.json()
    appStore.setChannel(channels as Channel[])
}

function resetGuild() {
    appStore.setChannel([])
    appStore.setMessages([])
    appStore.setChannelId(0)
}

async function selectGuild(id: number) {
    resetGuild()
    appStore.setGuildId(id)
    await getChannels()
}
</script>

<template>
    <section class="h-full hable__servers flex flex-col gap-2 text-[#5D5FEF] p-2 font-semibold overflow-y-auto">
        <span @click="createGuild" class="p-1 rounded-md text-[#5D5FEF] hover:bg-[#E1E2FF] font-semibold hover:cursor-pointer">
        Server &plus;
        </span>
        <hr />
        
        <span
            @click="selectGuild(guild.id)"
            class="p-1 rounded-sm hover:bg-[#E1E2FF] hover:cursor-pointer"
            :class="{active: guild.id === appStore.selectedGuild}"
            v-for="guild in guilds"
            :key="guild.id"
        >
            {{ guild.name }}
        </span>
      </section>
</template>

<style scoped>
.active {
    @apply bg-[#E1E2FF]
}
</style>