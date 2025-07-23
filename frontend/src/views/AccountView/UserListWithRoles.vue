<script setup lang="ts">
import { onMounted, ref } from "vue";
import { RoleService } from "@/services/RoleService";
import type { UserWithRolesDto } from "@/types/UserWithRolesDto";
import type { AppRole } from "@/domain/logic/AppRole";

const roleService = new RoleService();
const users = ref<UserWithRolesDto[]>([]);
const allRoles = ref<AppRole[]>([]);
const error = ref("");

const fetchUsers = async () => {
  try {
    users.value = await roleService.getAllUsersWithRoles();
  } catch (e: any) {
    error.value = e.message || "Viga kasutajate laadimisel";
  }
};

const fetchAllRoles = async () => {
  try {
    allRoles.value = await roleService.getAllRoles();
  } catch (e: any) {
    error.value = e.message || "Viga rollide laadimisel";
  }
};

const removeRole = async (userId: string, roleName: string) => {
  const role = allRoles.value.find(r => r.name === roleName);
  if (!role) {
    error.value = `Rolli '${roleName}' ei leitud`;
    return;
  }

  const result = await roleService.removeRoleFromUser(userId, role.id);
  if (!result.errors!.length) {
    await fetchUsers();
  } else {
    error.value = result.errors!.join(", ");
  }
};

onMounted(async () => {
  await fetchAllRoles();
  await fetchUsers();
});
</script>

<template>
  <main class="users-wrapper">
    <section class="users-box">
      <h1 class="page-title">👤 {{ $t('Users and roles') }}</h1>

      <p v-if="error" class="alert error-msg">{{ error }}</p>

      <div class="table-container">
        <table class="user-table">
          <thead>
          <tr>
            <th>{{ $t('Email') }}</th>
            <th>{{ $t('Full name') }}</th>
            <th>{{ $t('Roles') }}</th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="user in users" :key="user.id">
            <td>{{ user.email }}</td>
            <td>{{ user.firstName }} {{ user.lastName }}</td>
            <td>
                <span
                  class="role-badge"
                  v-for="role in user.roles"
                  :key="role"
                >
                  {{ role }}
                  <button
                    class="remove-btn"
                    @click="removeRole(user.id, role)"
                    title="Remove role"
                  >
                    ×
                  </button>
                </span>
            </td>
          </tr>
          </tbody>
        </table>
      </div>
    </section>
  </main>
</template>

<style scoped>
.users-wrapper {
  padding: 2rem;
  display: flex;
  justify-content: center;
  font-family: 'Inter', sans-serif;
  color: white;
}

.users-box {
  width: 100%;
  max-width: 1100px;
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

.user-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 1rem;
  min-width: 600px;
}

.user-table th,
.user-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  vertical-align: top;
}

.user-table thead {
  background: #ffaa33;
  color: black;
}

.user-table tbody tr:hover {
  background-color: rgba(255, 170, 51, 0.08);
}

.role-badge {
  display: inline-flex;
  align-items: center;
  background: rgba(255, 165, 0, 0.1);
  color: #ffaa33;
  padding: 0.4rem 0.8rem;
  margin: 0.2rem;
  border-radius: 999px;
  font-size: 0.9rem;
  font-weight: 600;
  border: 1px solid rgba(255, 170, 51, 0.3);
}

.remove-btn {
  margin-left: 0.5rem;
  background: transparent;
  border: none;
  color: #ff5f5f;
  cursor: pointer;
  font-size: 1.1rem;
  font-weight: bold;
  padding: 0 0.2rem;
  border-radius: 50%;
  transition: background 0.2s ease;
}

.remove-btn:hover {
  background: rgba(255, 80, 80, 0.15);
}
</style>
