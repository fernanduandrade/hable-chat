<script lang="ts" setup>
import { markRaw, onMounted, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import useApplicationStore from '../../stores/index'
import useModal from '../../composables/useModal'
import CreateForm from '../components/CreateForm.vue'
import { Channel, Guild } from '../../types'
import useFetch from '../../composables/useFetch'

const modal = useModal()
const appStore = useApplicationStore()
const { modalEmitValue } = storeToRefs(modal)
const { guilds } = storeToRefs(appStore)
const showContextDelete = ref(false)
const deleteGuildId = ref(0)
const getGuilds = async() => {
    const { data, fetchData } = useFetch<Array<Guild>>('https://localhost:7266/api/guilds')
    await fetchData() 

    const result = data.value!
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

const elementTopPosition = ref(0)

async function getChannels() {
    const guildId = appStore.selectedGuild
    const { data, fetchData } = useFetch<Array<Channel>>(`https://localhost:7266/api/guilds/${guildId}/channels`)
    await fetchData()

    const channels = data.value!
    appStore.setChannel(channels)
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

function onMouseLeave() {
    showContextDelete.value = false
    deleteGuildId.value = 0
}
function openDeleteContext(evt: PointerEvent, guildId: number) {
    evt.preventDefault()
    showContextDelete.value = !showContextDelete.value
    elementTopPosition.value = evt.clientY + 15
    deleteGuildId.value = guildId
    
}

async function deleteGuild() {
    const { fetchData } = useFetch(`https://localhost:7266/api/guilds/${deleteGuildId.value}`, {
        method: 'DELETE'
    })
    await fetchData()
    await getGuilds()
}

onMounted(() => {
    const contextmenu = document.getElementById('contextmenu-guilds')
    const scope = document.querySelector("body");
    scope!.addEventListener('click', (evt: any) => {
        evt.target.offsetParent != contextmenu
        showContextDelete.value = false
    })
})

</script>

<template>
    <section class="h-full hable__servers flex flex-col gap-2 text-[#5D5FEF] p-2 font-semibold overflow-y-auto">
        <span @click="createGuild" class="p-1 rounded-md text-[#5D5FEF] hover:bg-[#E1E2FF] font-semibold hover:cursor-pointer">
        Server &plus;
        </span>
        <hr />
        
        <span
            :oncontextmenu="(evt: PointerEvent) => openDeleteContext(evt, guild.id)"
            @click="selectGuild(guild.id)"
            :data-id="guild.id"
            class="p-1 rounded-sm hover:bg-[#E1E2FF] hover:cursor-pointer"
            :class="{active: guild.id === appStore.selectedGuild}"
            v-for="guild in guilds"
            :key="guild.id"
        >
            {{ guild.name }}
        </span>
        <div
            @mouseleave="onMouseLeave"
            v-if="showContextDelete"
            class="bg-white hover:bg-red-300 font-semibold p-2 absolute rounded-lg hover:cursor-pointer shadow-md z-10"
            :style="{top: `${elementTopPosition}px`}"
            @click="deleteGuild"
            id="contextmenu-guilds"
        >
            <span class="text-red-600 text-center">Deletar o servidor</span>
        </div>
      </section>
</template>

<style scoped>
.active {
    @apply bg-[#E1E2FF]
}
</style>