<script setup lang="ts">
import { onMounted, ref } from "vue";
import { RoleService } from "@/services/RoleService";
import type { AppRole } from "@/domain/logic/AppRole";

const roleService = new RoleService();
const roles = ref<AppRole[]>([]);
const error = ref("");
const success = ref("");
const newRoleName = ref("");

const fetchRoles = async () => {
  try {
    roles.value = await roleService.getAllRoles();
  } catch (e: any) {
    error.value = e.message;
  }
};

const createRole = async () => {
  error.value = "";
  success.value = "";

  if (!newRoleName.value.trim()) {
    error.value = "Rolli nimi ei tohi olla tühi";
    return;
  }

  const result = await roleService.createRole(newRoleName.value.trim());
  if (result.errors!.length) {
    error.value = result.errors!.join(", ");
  } else {
    success.value = result.data!;
    newRoleName.value = "";
    await fetchRoles();
  }
};

onMounted(fetchRoles);
</script>

<template>
  <main class="role-wrapper">
    <section class="role-header">
      <h1 class="page-title">👥 Roles</h1>
      <p class="subtitle">Manage access roles for your system</p>
    </section>

    <div class="role-form-bar">
      <form @submit.prevent="createRole" class="role-form">
        <input
          v-model="newRoleName"
          type="text"
          placeholder="Enter new role name"
          class="role-input"
        />
        <button type="submit" class="role-create-btn">+ Create</button>
      </form>
      <p v-if="error" class="alert error-msg">{{ error }}</p>
      <p v-if="success" class="alert success-msg">{{ success }}</p>
    </div>

    <div class="role-table-box">
      <table class="role-table">
        <thead>
        <tr>
          <th>ID</th>
          <th>Role Name</th>
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
  </main>
</template>

<style scoped>
.role-wrapper {
  padding: 2rem;
  max-width: 1000px;
  margin: 0 auto;
  font-family: 'Inter', sans-serif;
  color: white;
}

.role-header {
  margin-bottom: 2rem;
}

.page-title {
  font-size: 2.6rem;
  font-weight: 800;
  color: #ffaa33;
  text-shadow: 0 0 10px rgba(255, 170, 51, 0.25);
  margin-bottom: 0.3rem;
}

.subtitle {
  font-size: 1rem;
  color: #bbb;
  opacity: 0.85;
}

.role-form-bar {
  background: rgba(30, 30, 30, 0.6);
  backdrop-filter: blur(8px);
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 0 12px rgba(255, 170, 51, 0.05);
  margin-bottom: 2rem;
}

.role-form {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
}

.role-input {
  flex: 1;
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border-radius: 10px;
  border: 1px solid #ffaa33;
  background-color: rgba(43, 43, 43, 0.6);
  color: white;
}

.role-input::placeholder {
  color: #ccc;
}

.role-create-btn {
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  color: black;
  font-weight: 700;
  font-size: 1rem;
  border-radius: 10px;
  padding: 0.6rem 1.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
}

.role-create-btn:hover {
  background: linear-gradient(to right, #ffc56e, #ffa726);
}

.alert {
  margin-top: 1rem;
  font-size: 0.95rem;
  font-weight: 500;
  padding: 0.5rem 1rem;
  border-radius: 8px;
}

.error-msg {
  background: rgba(255, 80, 80, 0.15);
  border: 1px solid rgba(255, 80, 80, 0.6);
  color: #ff5f5f;
}

.success-msg {
  background: rgba(0, 255, 100, 0.1);
  border: 1px solid rgba(0, 255, 100, 0.4);
  color: #9effb1;
}

.role-table-box {
  background: rgba(20, 20, 20, 0.5);
  border-radius: 16px;
  padding: 1rem;
  backdrop-filter: blur(6px);
  box-shadow: inset 0 0 20px rgba(255, 165, 0, 0.05);
}

.role-table {
  width: 100%;
  border-collapse: collapse;
  color: white;
}

.role-table th,
.role-table td {
  padding: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  text-align: left;
}

.role-table thead {
  background-color: #ffaa33;
  color: black;
}

.role-table tbody tr:hover {
  background: rgba(255, 170, 51, 0.1);
}
</style>
