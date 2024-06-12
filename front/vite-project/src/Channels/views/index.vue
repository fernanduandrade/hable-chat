<script lang="ts" setup>
import VInput from '../../common/components/VInput.vue'
import { markRaw, ref, watch, computed, onMounted } from 'vue' 
import useApplicationStore from '../../stores/index'
import { storeToRefs } from 'pinia'
import { Channel, Message } from '../../types';
import useModal from '../../composables/useModal';
import CreateForm from '../../Channels/components/CreateForm.vue';
import { HubConnectionBuilder } from '@microsoft/signalr';

const user = JSON.parse(sessionStorage.getItem('user') || '')
const appStore = useApplicationStore()
const modal = useModal()
const showContextDelete = ref(false)
const { channels, messages } = storeToRefs(appStore)
const { modalEmitValue } = storeToRefs(modal)
const search = ref('')
async function getChannels() {
  const guildId = appStore.selectedGuild
  const response = await fetch(`https://localhost:7266/api/guilds/${guildId}/channels`, {
      headers: {
          Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
      }
  })

    const channels = await response.json() as Channel[]
    appStore.setChannel(channels) 
}

const searchChannels = computed(() => {
  return channels.value.filter(x => x.name.toLowerCase().includes(search.value.toLowerCase()))
})

async function getChannelMessages() {
  const currentChannel = appStore.selectedChannel
  const response = await fetch(`https://localhost:7266/api/channels/${currentChannel}/messages`, {
      headers: {
          Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
      }
  })

    const messages = await response.json() as Message[]
    appStore.setMessages(messages)
}

async function joinChannel(userName: string, channelId: number, userId: string) {
    const connection = new HubConnectionBuilder()
        .withUrl("https://localhost:7266/messages", { withCredentials: false })
        .build();

        connection.on("RecieveSpecificMessage", (username: any, message: any, user: any) => {
          console.log(username, message, user)
            messages.value.push({ id: Math.floor(Math.random() * 9998), user, content: message, created: new Date().toString() })
            const messageContainer = document.getElementById('messageContainer')
            messageContainer!.scrollTop = messageContainer!.scrollHeight;
        });

        await connection.start()
        await connection.invoke('JoinSpecificChannel', { userName, channelId: channelId.toString(), userId })
        appStore.setConnection(connection)
}

async function leaveChannel(channelId: number) {
  appStore.hubConnection?.invoke('LeaveChannel', { userName: '', channelId: channelId.toString(), userId: ''})
}


function createChannel() {
    modal.open({ component: markRaw(CreateForm), props: { guildId: appStore.selectedGuild },  title: 'Crie um canal' })
}

async function selectChannel(channel: Channel) {
  if(appStore.selectedChannel > 0 && appStore.selectedChannel !== channel.id) {
    leaveChannel(channel.id)
  }
  appStore.setChannelId(channel.id)
  appStore.setChannelName(channel.name)
  await joinChannel(user.name, channel.id, user.id)
  await getChannelMessages()
  const messageContainer = document.getElementById('messageContainer')
    messageContainer!.scrollTop = messageContainer!.scrollHeight
}

watch(modalEmitValue, async _ => {
  await getChannels()
})

const elementTopPosition = ref(0)
const deleteChannelId = ref(0)

function openDeleteContext(evt: PointerEvent, channelId: number) {
    evt.preventDefault()
    showContextDelete.value = !showContextDelete.value
    elementTopPosition.value = evt.clientY + 15
    deleteChannelId.value = channelId
    
}

function onMouseLeave() {
  showContextDelete.value = false
}

onMounted(() => {
  const contextmenu = document.getElementById('contextmenu-channels')
    const scope = document.querySelector("body");
    scope!.addEventListener('click', (evt: any) => {
        evt.target.offsetParent != contextmenu
        showContextDelete.value = false
    })
})
async function deleteChannel() {
  await fetch(`https://localhost:7266/api/channels/${deleteChannelId.value}`, {
        headers: {
            Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
        },
        method: 'DELETE'
    })
  await getChannels()
}


</script>

<template>
    <section class="boxes hable__channels">
        <span class="font-semibold">Canais</span>
        <hr />
        <div class="flex items-center gap-4 w-full">
          <div class="w-[70%]">
            <VInput type="text" placeholder="Pesquisar canais" v-model="search" />
          </div>
          <span @click="createChannel" class="p-1 rounded-md text-[#5D5FEF] hover:bg-[#E1E2FF] font-semibold hover:cursor-pointer">
            Canais &plus;
          </span>
        </div>

        <div class="text-black font-semibold flex flex-col p2 gap-3">
          <span
            :oncontextmenu="(evt: PointerEvent) => openDeleteContext(evt, channel.id)"
            @click="selectChannel(channel)"
            v-for="channel in searchChannels"
            :key="channel.id"
            class="p-1 rounded-sm hover:bg-gray-100 hover:cursor-pointer"
            :class="[{active: channel.id === appStore.selectedChannel}]"
          >
            #{{ channel.name }}
          </span>
        </div>
        <div
            @mouseleave="onMouseLeave"
            v-if="showContextDelete"
            class="bg-white hover:bg-red-300 font-semibold p-2 absolute rounded-lg hover:cursor-pointer shadow-md z-10"
            :style="{top: `${elementTopPosition}px`}"
            @click="deleteChannel"
            id="contextmenu-channels"
        >
            <span class="text-red-600 text-center">Deletar o canal</span>
        </div>

      </section>
</template>

<style scoped>
.active {
  @apply bg-gray-100
}
</style>