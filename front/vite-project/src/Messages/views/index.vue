<script lang="ts" setup>
import { ref } from 'vue'
import VInput from '../../common/components/VInput.vue'
import VButton from '../../common/components/VButton.vue'
import Text from '../components/Text.vue';
import { storeToRefs } from 'pinia'
import useApplicationStore from '../../stores/index'
import useFetch from '../../composables/useFetch';

const user = JSON.parse(sessionStorage.getItem('user') || '')
const appStore = useApplicationStore()
const text = ref('')
const { messages, hubConnection, selectedChannel, channelName } = storeToRefs(appStore)

async function sendMessage(selectedChannel: number, content: string) {
  if (!content)
    return

  const tempId = Math.floor(Math.random() * 9999)
  messages.value.push({ id: tempId, user, content, created: new Date().toString() })
  const { fetchData } = useFetch(`https://localhost:7266/api/channels/${selectedChannel}/messages`, {
    method: 'POST',
    body: JSON.stringify({ content })
  })
  await fetchData()
  const messageIndex = messages.value.findIndex(x => x.id === tempId)
  messages.value.splice(messageIndex, 0)
  hubConnection.value!.invoke('SendMessage', content)
}

function clearInput(evt: string) {
  text.value = evt
}

</script>

<template>
  <section
    class="bg-[#FFFFF] border border-gray-200 p-4 shadow-sm rounded-2xl h-full flex flex-col gap-3 hable__messages">
    <div class="h-[64px] w-full">
      <span class="text-lg font-semibold flex items-center">{{ channelName }}</span>
    </div>
    <hr />

    <block class="h-[70%] flex flex-col overflow-y-auto gap-8" id="messageContainer">
      <Text v-for="message in messages" :key="message.id" :message="message" :owner="message.user.id === user.id"/>
    </block>

    <div class="w-full self-end flex gap-3 items-center">
      <VInput type="text" @on-enter-event="clearInput" :on-enter-args="[selectedChannel, text]" :on-enter="sendMessage"
        placeholder="Digite a mensagem" v-model="text" />
      <VButton background-color="#5D5FEF" icon="fa-greater-than"
        @click="sendMessage(selectedChannel, text); text = ''" />
    </div>

  </section>
</template>