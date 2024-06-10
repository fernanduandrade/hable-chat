<script setup lang="ts">
import VModal from './common/components/VModal.vue'
</script>

<template>
  <router-view />
  <VModal />
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=Nunito:ital,wght@0,200..1000;1,200..1000&display=swap');
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


@tailwind base;
@tailwind utilities;


:root {
  font-synthesis: none;
  text-rendering: optimizeLegibility;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.auth__container { 
    background-color: #F9FAFE;
    width: 100vw;
    height: 100vh;
    @apply grid place-content-center
}

body {
  background-color: #FFFFFF;
  margin: 0;
  padding: 0;
  min-height: 100vh;
  font-family: 'Nunito', sans-serif;
}

.header {
  background-color: #5D5FEF;
  @apply bg-indigo-500 h-[56px] flex items-center p-2 shadow-sm text-white
}

.error-message {
  @apply text-red-600 font-bold text-xl
}

.boxes {
  @apply bg-[#FFFFF] border border-gray-200 p-2 shadow-sm rounded-2xl h-full flex flex-col gap-3 overflow-y-auto
}
</style>