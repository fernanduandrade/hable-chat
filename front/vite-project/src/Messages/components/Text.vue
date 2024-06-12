<script lang="ts" setup>
import { PropType, computed, onMounted, ref } from 'vue';
import Avatar from '../../common/components/Avatar.vue'
import { Message } from '../../types/index'
import useApplicationStore from '../../stores/index'

const appStore = useApplicationStore()
const props = defineProps({
    message: {
        type: Object as PropType<Message>,
        required: true
    },
    owner: {
        type: Boolean,
        required: true
    }
})

const classes = computed(() => {
    return {
        'default': !props.owner,
        'owner': props.owner
    }
})

const showContextDelete = ref(false)
const elementTopPosition = ref(0)
const deleteMessageId = ref(0)

function openDeleteContext(evt: PointerEvent, messageId: number) {
  evt.preventDefault()
  console.log('aassa')
  showContextDelete.value = !showContextDelete.value
  elementTopPosition.value = evt.clientY + 15
  deleteMessageId.value = messageId

}

function onMouseLeave() {
  showContextDelete.value = false
}

onMounted(() => {
  const contextmenu = document.getElementById('contextmenu-messages')
  const scope = document.querySelector("body");
  scope!.addEventListener('click', (evt: any) => {
    evt.target.offsetParent != contextmenu
    showContextDelete.value = false
  })
})
async function deleteMessage() {
  await fetch(`https://localhost:7266/api/channels/${appStore.selectedChannel}/messages/${props.message.id}`, {
    headers: {
      Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
    },
    method: 'DELETE'
  })
}

</script>

<template>
    <div class="flex item-center gap-5" :class="{ 'flex-row-reverse': owner }">
        <span>
            <Avatar :name="message.user.name" :last-name="message.user.lastName" />
        </span>
        <div class="flex flex-col gap-2">
            <span class="block rounded-md w-fit border p-2" :class="[classes]"
                :oncontextmenu="(evt: PointerEvent) => openDeleteContext(evt, message.id)"
            >
                {{ message.content }}
            </span>
            <div @mouseleave="onMouseLeave" v-if="showContextDelete"
            class="bg-white hover:bg-red-300 font-semibold p-2 absolute rounded-lg hover:cursor-pointer shadow-md z-10"
            :style="{ top: `${elementTopPosition}px` }" @click="deleteMessage" id="contextmenu-messages">
            <span class="text-red-600 text-center">Deletar message</span>
            </div>
            <span class="text-[#333333] text-sm"> {{ new Date(message.created).toLocaleTimeString() }}</span>
        </div>

    </div>
</template>

<style scoped>
.default {
    @apply border-indigo-400 text-[#5D5FEF]
}

.owner {
    @apply bg-[#5D5FEF] text-white
}
</style>