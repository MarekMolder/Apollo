<script setup lang="ts">
import { ref, onMounted } from "vue";
import { RoleService } from "@/services/RoleService";
import { IdentityService } from "@/services/IdentityService";
import type { AppRole } from "@/domain/logic/AppRole";
import type { AssignRoleDto } from "@/types/AssignRoleDto";

const roleService = new RoleService();
const identityService = new IdentityService();

const users = ref<{ id: string; firstName: string; lastName: string }[]>([]);
const roles = ref<AppRole[]>([]);
const selectedUserId = ref("");
const selectedRoleId = ref("");
const message = ref("");
const error = ref("");

const assign = async () => {
  const dto: AssignRoleDto = {
    userId: selectedUserId.value,
    roleId: selectedRoleId.value
  };
  const res = await roleService.assignRoleToUser(dto);
  if (res.errors!.length) {
    error.value = res.errors![0];
    message.value = "";
  } else {
    message.value = res.data!;
    error.value = "";
  }
};

onMounted(async () => {
  users.value = await identityService.getAllUsers();
  roles.value = await roleService.getAllRoles();
});
</script>

<template>
  <main class="assign-role-wrapper">
    <div class="assign-box">
      <h1 class="page-title">🎭 {{ $t('Assign role to user') }}</h1>

      <select v-model="selectedUserId" class="assign-select">
        <option disabled value="">Select user</option>
        <option v-for="u in users" :key="u.id" :value="u.id">
          {{ u.firstName }} {{ u.lastName }}
        </option>
      </select>

      <select v-model="selectedRoleId" class="assign-select">
        <option disabled value="">{{ $t('Select role') }}</option>
        <option v-for="r in roles" :key="r.id" :value="r.id">{{ r.name }}</option>
      </select>

      <button class="assign-button" @click="assign">{{ $t('Assign') }}</button>

      <p v-if="message" class="alert success-msg">{{ message }}</p>
      <p v-if="error" class="alert error-msg">{{ error }}</p>
    </div>
  </main>
</template>

<style scoped>
.assign-role-wrapper {
  display: flex;
  justify-content: center;
  padding: 3rem 1rem;
  font-family: 'Inter', sans-serif;
  color: white;
}

.assign-box {
  background: rgba(20, 20, 20, 0.85);
  backdrop-filter: blur(10px);
  border-radius: 16px;
  padding: 2rem;
  width: 100%;
  max-width: 600px;
  box-shadow: 0 0 16px rgba(255, 165, 0, 0.1);
  border: 1px solid rgba(255, 170, 51, 0.15);
}

.page-title {
  text-align: center;
  font-size: 2.4rem;
  font-weight: 800;
  color: #ffaa33;
  margin-bottom: 2rem;
  text-shadow: 0 0 10px rgba(255, 170, 51, 0.2);
}

.assign-select {
  width: 100%;
  margin-bottom: 1.2rem;
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border-radius: 10px;
  background: rgba(43, 43, 43, 0.6);
  color: white;
  border: 1px solid #ffaa33;
  transition: all 0.2s ease;
}

.assign-select:focus {
  outline: none;
  background: rgba(60, 60, 60, 0.8);
  border-color: #ffc266;
}

.assign-button {
  width: 100%;
  background: linear-gradient(to right, #ffaa33, #ff8c00);
  padding: 0.75rem;
  font-size: 1rem;
  font-weight: bold;
  border: none;
  border-radius: 12px;
  color: black;
  cursor: pointer;
  transition: background 0.2s ease;
  box-shadow: 0 3px 10px rgba(255, 165, 0, 0.2);
}

.assign-button:hover {
  background: linear-gradient(to right, #ffc56e, #ffa726);
}

.alert {
  margin-top: 1rem;
  font-size: 0.95rem;
  padding: 0.6rem 1rem;
  border-radius: 10px;
  text-align: center;
  font-weight: 500;
}

.success-msg {
  background: rgba(0, 255, 100, 0.1);
  border: 1px solid rgba(0, 255, 100, 0.4);
  color: #9effb1;
}

.error-msg {
  background: rgba(255, 80, 80, 0.15);
  border: 1px solid rgba(255, 80, 80, 0.6);
  color: #ff5f5f;
}
</style>
