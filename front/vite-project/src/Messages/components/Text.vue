<script lang="ts" setup>
import { PropType, computed } from 'vue';
import Avatar from '../../common/components/Avatar.vue'
import { Message } from '../../types/index'

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
</script>

<template>
    <div class="flex item-center gap-5" :class="{'flex-row-reverse' : owner} ">
        <span>
            <Avatar :name="message.user.name" :last-name="message.user.lastName" />
        </span>
        <div class="flex flex-col gap-2">
            <span class="block rounded-md w-[70%] w-fit border p-2" :class="[classes]">
                {{ message.content}}
            </span>
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