<script setup lang="ts">
import { RouterLink, useRouter } from "vue-router";
import { useUserDataStore } from "@/stores/userDataStore.ts";
import { computed, ref, onMounted, onUnmounted } from "vue";
import { jwtDecode } from "jwt-decode";
import { switchLanguage } from '@/Helpers/i18nHelper';

const router = useRouter();
const userStore = useUserDataStore();

const fullName = computed(() => userStore.firstName ?? "Kasutaja");

const logout = () => {
  userStore.jwt = null;
  userStore.refreshToken = null;
  router.push("/");
};

const isAdmin = computed(() => {
  if (!userStore.jwt) return false;
  try {
    const decoded: any = jwtDecode(userStore.jwt);
    const roleClaim = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return Array.isArray(roleClaim)
      ? roleClaim.some(r => r === "admin" || r === "manager")
      : roleClaim === "admin" || roleClaim === "manager";
  } catch {
    return false;
  }
});

const dropdownOpen = ref(false);

const toggleDropdown = () => {
  dropdownOpen.value = !dropdownOpen.value;
};

const handleClickOutside = (event: MouseEvent) => {
  const dropdown = document.querySelector(".user-dropdown");
  if (dropdown && !dropdown.contains(event.target as Node)) {
    dropdownOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener("click", handleClickOutside);
});
onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});
</script>

<template>
  <header class="header">
    <div class="header-left">
      <RouterLink to="/home" class="logo">Apollo</RouterLink>
    </div>

    <div class="header-right">
      <div class="controls">
        <div class="lang-switch">
          <button @click="switchLanguage('en')">EN</button>
          <button @click="switchLanguage('et')">ET</button>
        </div>

        <div v-if="userStore.jwt" class="user-menu">
          <div class="user-dropdown" @click="toggleDropdown">
            <i class="bi bi-person-circle user-icon"></i>
            <span class="user-name">{{ fullName }}</span>

            <transition name="fade-slide">
              <ul v-show="dropdownOpen" class="dropdown">
                <li><RouterLink to="/account">{{ $t('Edit Account') }}</RouterLink></li>
                <li><a href="#" @click.prevent="logout">{{ $t('Logout') }}</a></li>
                <template v-if="isAdmin">
                  <li><RouterLink to="/register">{{ $t('Register') }}</RouterLink></li>
                  <li><RouterLink to="/createRole">{{ $t('Create role') }}</RouterLink></li>
                  <li><RouterLink to="/assignRole">{{ $t('Assign role') }}</RouterLink></li>
                  <li><RouterLink to="/getRole">{{ $t('Get role') }}</RouterLink></li>
                  <li><RouterLink to="/userRoles">{{ $t('Users and roles') }}</RouterLink></li>
                </template>
              </ul>
            </transition>
          </div>
        </div>

        <div v-else>
          <RouterLink to="/" class="login-link">{{ $t('Login') }}</RouterLink>
        </div>
      </div>
    </div>
  </header>
</template>

<style scoped>
.header {
  background-color: #111;
  border-bottom: 1px solid #2a2a2a;
  padding: 0.75rem 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: white;
  height: 56px;
  z-index: 999;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.3);
}

.logo {
  color: orange;
  font-size: 1.4rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 1px;
  text-decoration: none;
}

.logo:hover {
  color: #ffa500;
}

.header-right .controls {
  display: flex;
  align-items: center;
  gap: 1.2rem;
}

.lang-switch {
  display: flex;
  gap: 0.4rem;
}

.lang-switch button {
  background-color: #1a1a1a;
  border: 1px solid #333;
  color: #ccc;
  padding: 0.25rem 0.7rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 500;
  transition: background 0.2s ease, color 0.2s ease;
}

.lang-switch button:hover {
  background-color: orange;
  color: black;
}

.user-menu {
  position: relative;
}

.user-dropdown {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  position: relative;
  cursor: pointer;
}

.user-icon {
  font-size: 1.3rem;
  color: white;
}

.user-name {
  font-size: 0.9rem;
  color: #eee;
  font-weight: 500;
}

.dropdown {
  position: absolute;
  right: 0;
  top: 140%;
  background-color: #1a1a1a;
  border: 1px solid #2a2a2a;
  border-radius: 6px;
  list-style: none;
  padding: 0.5rem 0;
  min-width: 180px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
  z-index: 1001;
}

.dropdown li a,
.dropdown li RouterLink {
  color: white;
  text-decoration: none;
  padding: 0.5rem 1rem;
  display: block;
  transition: background 0.2s ease;
}

.dropdown li:hover {
  background-color: rgba(255, 165, 0, 0.1);
}

.login-link {
  color: orange;
  font-weight: 500;
  text-decoration: none;
  padding: 0.5rem 1rem;
  transition: color 0.2s ease;
}

.login-link:hover {
  color: #ffa500;
}

/* dropdown transition */
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.2s ease;
}
.fade-slide-enter-from,
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-5px);
}
</style>
