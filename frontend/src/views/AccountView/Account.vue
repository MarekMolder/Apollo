<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useUserDataStore } from '@/stores/userDataStore';
import { IdentityService } from '@/services/IdentityService';
import { RouterLink } from 'vue-router';

// Services
const identityService = new IdentityService();

//??
const firstName = ref('');
const lastName = ref('');
const userName = ref('');
const message = ref('');

// Store
const store = useUserDataStore();

// Get account info
onMounted(() => {
  firstName.value = store.firstName || '';
  lastName.value = store.lastName || '';
  userName.value = store.username || '';
});

// Account edit function
const editAccount = async () => {
  const result = await identityService.updateUserFields({
    email: store.email!,
    userName: userName.value,
    firstName: firstName.value,
    lastName: lastName.value,
  });

  message.value = result.errors ? `Viga: ${result.errors.join(', ')}` : '✅ Konto uuendatud!';
};
</script>

<template>
  <main
    class="flex flex-wrap justify-center items-start gap-12 px-6 py-16 sm:px-10 sm:py-20 md:px-28 md:py-28 text-white font-['Segoe_UI']"
  >
    <!-- Account Info Card -->
    <div
      class="flex-1 max-w-[500px] bg-[rgba(20,20,20,0.85)] rounded-[16px] p-6 sm:p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md"
    >
      <div class="flex flex-col items-center gap-4 mb-6">
        <div
          class="w-[90px] h-[90px] bg-[#ffaa33] rounded-full flex items-center justify-center text-[2.8rem] shadow-[0_4px_12px_rgba(255,170,51,0.4)]"
        >
          👤
        </div>
        <h2 class="text-xl font-semibold">{{ $t('My account') }}</h2>
      </div>
      <div
        class="bg-[rgba(255,255,255,0.05)] p-3.5 sm:p-6 rounded-xl text-sm flex flex-col gap-2"
      >
        <p class="py-1 border-b border-white/10">
          <label>{{ $t('Firstname') }}:</label> {{ store.firstName }}
        </p>
        <p class="py-1 border-b border-white/10">
          <label>{{ $t('Lastname') }}:</label> {{ store.lastName }}
        </p>
        <p class="py-1 border-b border-white/10">
          <label>{{ $t('Username') }}:</label> {{ store.username }}
        </p>
        <p class="py-1 border-b border-white/10">
          <label>{{ $t('Email') }}:</label> {{ store.email }}
        </p>
      </div>
    </div>

    <!-- Edit Form Card -->
    <div
      class="flex-1 max-w-[500px] bg-[rgba(20,20,20,0.85)] rounded-[16px] p-6 sm:p-8 shadow-[0_0_16px_rgba(255,165,0,0.2)] backdrop-blur-md"
    >
      <h2
        class="p-1.5 text-[#ffaa33] text-center text-xl sm:text-2xl font-semibold mb-6"
      >
        {{ $t('Edit Profile') }}
      </h2>
      <form @submit.prevent="editAccount" class="flex flex-col gap-4">
        <div class="p-1">
          <label class="font-semibold block mb-1"></label>
          <input
            v-model="firstName"
            type="text"
            placeholder="Firstname"
            class="w-full px-3 py-[0.5rem] sm:py-[0.6rem] rounded-lg bg-[#2a2a2a] text-white text-base border-none focus:outline-none focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <div class="p-1">
          <label class="font-semibold block mb-1"></label>
          <input
            v-model="lastName"
            type="text"
            placeholder="Lastname"
            class="w-full px-3 py-[0.5rem] sm:py-[0.6rem] rounded-lg bg-[#2a2a2a] text-white text-base border-none focus:outline-none focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <div class="p-1">
          <label class="font-semibold block mb-1"></label>
          <input
            v-model="userName"
            type="text"
            placeholder="Username"
            class="w-full px-3 py-[0.5rem] sm:py-[0.6rem] rounded-lg bg-[#2a2a2a] text-white text-base border-none focus:outline-none focus:bg-[#1a1a1a] focus:border-orange-400"
          />
        </div>

        <div
          class="flex flex-col sm:flex-row justify-between items-center mt-4 gap-4 sm:gap-0"
        >
          <button
            type="submit"
            class="w-full sm:w-auto bg-gradient-to-r from-[#ff8c00] to-[#ffa500] text-white font-bold py-2 px-5 rounded-lg transition hover:from-[#ffa500] hover:to-[#ffcc00]"
          >
            {{ $t('Save data') }}
          </button>
          <RouterLink
            to="/changepassword"
            class="w-full sm:w-auto text-center text-[#ffaa33] underline"
          >
            {{ $t('Change password') }}
          </RouterLink>
        </div>

        <p class="text-center font-bold text-green-400 mt-3">{{ message }}</p>
      </form>
    </div>
  </main>
</template>


