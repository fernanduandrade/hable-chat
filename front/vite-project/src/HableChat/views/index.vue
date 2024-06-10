<script setup lang="ts">
import TheHeader from '../../common/components/TheHeader.vue'
import Guilds from '../../Guilds/views/index.vue'
import Channels from '../../Channels/views/index.vue'
import Messages from '../../Messages/views/index.vue'
import useModal from '../../composables/useModal'
import useApplicationStore from '../../stores/index'
import { computed } from 'vue'

const modal = useModal()
const appStore = useApplicationStore()

const cssClasses = computed(() => {
  const classes = {
    'open-modal': modal.modalState.opened,
  }

  return classes
})
</script>

<template>
  <main :class="[cssClasses]">
    <TheHeader />
    <div class="flex p-6 h-[560px] gap-4">

      <Guilds />
      <div v-if="!appStore.selectedGuild" class="boxes flex-[70%] font-bold flex justify-center items-center">
        <h1 class="text-2xl">Selecione um servidor 😄</h1>
      </div>
      <Channels v-else-if="appStore.selectedGuild" />
      <div v-if="!appStore.selectedChannel && appStore.selectedGuild" class="boxes flex-[70%] font-bold flex justify-center items-center">
        <h1 class="text-2xl">Agora selecione um canal 😉</h1>
      </div>
      <Messages v-else-if="appStore.selectedChannel > 0"  />

    </div>
  </main>
</template>

<style>
.open-modal {
  filter: blur(4px);
}

::-webkit-scrollbar {
  width: 4px;
}

/* Track */
::-webkit-scrollbar-track {
  background: #f1f1f1;
}

/* Handle */
::-webkit-scrollbar-thumb {
  @apply bg-indigo-600
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  @apply bg-indigo-500
}

.hable__servers {
  flex: 12%
}

.hable__channels {
  flex: 25%
}

.hable__messages {
  flex: 60%
}
</style>