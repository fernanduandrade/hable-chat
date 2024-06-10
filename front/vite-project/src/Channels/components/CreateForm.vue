<script lang="ts" setup>
import { reactive } from 'vue';
import useModal from '../../composables/useModal'
import VButton from '../../common/components/VButton.vue'
import VInput from '../../common/components/VInput.vue'
const modal = useModal()

const props = defineProps({
    guildId: {
        type: Number,
        required: true
    }
})
const form = reactive({
    name: ''
})

async function createChannel() {
    const response = await fetch(`https://localhost:7266/api/guilds/${props.guildId}/channels`, {
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${JSON.parse(sessionStorage.getItem('token') || '')}`
        },
        method: 'POST',
        body: JSON.stringify(form)
    })
    const guild = await response.json()
    modal.setFormValue(guild)
    modal.close()
}

</script>

<template>
  <div class="wrapper">
    <form class="form">
      <div>
        <VInput v-model="form.name" type="text" placeholder="Nome do canal" />
      </div>
    </form>
    <div class="flex items-center gap-2 self-center">
      <VButton :transparent="true" @click="modal.close">
        Cancelar
      </VButton>
      <VButton  @click="createChannel">
        Cadastrar
      </VButton>
    </div>
  </div>
</template>

<style scoped>

.wrapper {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

</style>