import { ref, computed } from "vue";
import { defineStore } from "pinia";
import { jwtDecode } from "jwt-decode";

interface DecodedJwt {
  [key: string]: unknown;
}

export const useUserDataStore = defineStore("userData", () => {
  const jwt = ref<string | null>(null);
  const refreshToken = ref<string | null>(null);
  const roles        = ref<string[]>([]);

  const email = ref<string | null>(null);
  const username = ref<string | null>(null);
  const firstName = ref<string | null>(null);
  const lastName = ref<string | null>(null);
  const userId = ref<string | null>(null); // UUS

  return { jwt, refreshToken, email, username, firstName, lastName, roles, userId };
});
