<script setup lang="ts">
import Header from "@/navigation/Header.vue";
import Sidebar from "@/navigation/Sidebar.vue";
</script>

<template>
  <div class="app-layout">
    <Header />
    <div class="content-area">
      <Sidebar />
      <main class="main-content">
        <div class="content-card">
          <router-view />
        </div>
      </main>
    </div>
  </div>
</template>

<style scoped>
.app-layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: hidden;
  font-family: 'Segoe UI', sans-serif;
  background: url('@/assets/apollo-pattern.png') no-repeat center center fixed;
  background-size: cover;
}

.content-area {
  display: flex;
  flex: 1;
  height: calc(100vh - 56px); /* Headeri kõrgus */
  overflow: hidden;
}

.main-content {
  flex: 1;
  overflow-y: auto;
  padding: 2rem;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  scrollbar-width: thin;
  scrollbar-color: #302f2f #1a1a1a;
}

/* Chrome, Edge, Safari scrollbar */
.main-content::-webkit-scrollbar {
  width: 8px;
}

.main-content::-webkit-scrollbar-track {
  background: #1a1a1a;
}

.main-content::-webkit-scrollbar-thumb {
  background-color: orange;
  border-radius: 10px;
  border: 2px solid #1a1a1a;
}

.content-card {
  width: 100%;
  max-width: 1500px;
  height: 100%;
}
</style>
