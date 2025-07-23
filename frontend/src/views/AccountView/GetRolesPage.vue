<script setup lang="ts">
import { onMounted, ref } from "vue";
import { RoleService } from "@/services/RoleService";
import type { AppRole } from "@/domain/logic/AppRole";
import { useUserDataStore } from "@/stores/userDataStore"; // ← lisa see

const roleService = new RoleService();
const roles = ref<AppRole[]>([]);
const error = ref("");
const store = useUserDataStore();

const fetchRoles = async () => {
  console.log("JWT in store:", store.jwt);
  try {
    roles.value = await roleService.getAllRoles();
  } catch (e: any) {
    error.value = e.message;
  }
};

onMounted(fetchRoles);
</script>

<template>
  <main class="roles-wrapper">
    <section class="roles-box">
      <h1 class="page-title">🎬 {{ $t('All roles') }}</h1>

      <p v-if="error" class="alert error-msg">{{ error }}</p>

      <div class="table-container">
        <table class="role-table">
          <thead>
          <tr>
            <th>ID</th>
            <th>{{ $t('Name') }}</th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="role in roles" :key="role.id">
            <td>{{ role.id }}</td>
            <td>{{ role.name }}</td>
          </tr>
          </tbody>
        </table>
      </div>
    </section>
  </main>
</template>

<style scoped>
.roles-wrapper {
  padding: 2rem;
  display: flex;
  justify-content: center;
  font-family: 'Inter', sans-serif;
  color: white;
}

.roles-box {
  width: 100%;
  max-width: 900px;
  background: rgba(20, 20, 20, 0.85);
  backdrop-filter: blur(10px);
  border-radius: 16px;
  padding: 2rem;
  box-shadow: 0 0 16px rgba(255, 165, 0, 0.1);
  border: 1px solid rgba(255, 170, 51, 0.15);
}

.page-title {
  text-align: center;
  font-size: 2.4rem;
  font-weight: 800;
  color: #ffaa33;
  margin-bottom: 1.5rem;
  text-shadow: 0 0 10px rgba(255, 170, 51, 0.2);
}

.alert {
  font-size: 1rem;
  padding: 0.8rem 1rem;
  border-radius: 10px;
  text-align: center;
  font-weight: 500;
  margin-bottom: 1.5rem;
}

.error-msg {
  background: rgba(255, 80, 80, 0.15);
  border: 1px solid rgba(255, 80, 80, 0.6);
  color: #ff5f5f;
}

.table-container {
  overflow-x: auto;
  border-radius: 12px;
  background: rgba(20, 20, 20, 0.5);
  box-shadow: inset 0 0 10px rgba(255, 170, 51, 0.05);
}

.role-table {
  width: 100%;
  border-collapse: collapse;
  color: white;
  font-size: 1rem;
  min-width: 400px;
}

.role-table th,
.role-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.role-table thead {
  background: #ffaa33;
  color: black;
}

.role-table tbody tr:hover {
  background-color: rgba(255, 170, 51, 0.08);
}
</style>
