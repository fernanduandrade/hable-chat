<script lang="ts" setup>
import { computed } from 'vue'
const props = defineProps({
    name: {
        type: String,
        required: true
    },
    lastName: {
        type: String,
        required: true,
        default: ''
    }
})

const initials = computed(() => {
    const initialName = props.name.substring(0, 1)
    const initialLastName = props.lastName.substring(0, 1)

    return `${initialName} ${initialLastName}`
})
</script>


<template>
    <main class="w-[45px] h-[45px] rounded-full select-none bg-slate-500 flex justify-center items-center">
        <span class="text-white uppercase font-bold"> {{ initials }}</span>
    </main>
</template>