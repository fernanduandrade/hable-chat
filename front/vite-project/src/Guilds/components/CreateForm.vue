<script lang="ts" setup>
import { reactive } from 'vue';
import useModal from '../../composables/useModal'
import VButton from '../../common/components/VButton.vue'
import VInput from '../../common/components/VInput.vue'
import useFetch from '../../composables/useFetch';
const modal = useModal()

const form = reactive({
  name: ''
})

async function createGuild() {
  const { data, fetchData } = useFetch('https://localhost:7266/api/guilds', {
    method: 'POST',
    body: JSON.stringify(form)
  })
  await fetchData()
  const guild = data.value
  modal.setFormValue(guild)
  modal.close()
}

</script>

<template>
  <div class="wrapper">
    <form class="form">
      <div>
        <VInput v-model="form.name" type="text" placeholder="Nome do servidor" />
      </div>
    </form>
    <div class="flex items-center gap-2 self-center">
      <VButton :transparent="true" @click="modal.close">
        Cancelar
      </VButton>
      <VButton @click="createGuild">
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