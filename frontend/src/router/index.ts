import { createRouter, createWebHistory } from 'vue-router';
import Login from "@/views/AccountView/Login.vue";
import Account from "@/views/AccountView/Account.vue";
import ChangePassword from "@/views/AccountView/ChangePassword.vue";
import CreateAction from "@/views/ActionViews/CreateAction.vue";
import ActionRequest from "@/views/ActionViews/ActionRequest.vue";
import Products from "@/views/ProductView/Products.vue";
import Suppliers from "@/views/SupplierView/Suppliers.vue";
import StorageRooms from "@/views/InventoryView/StorageRooms.vue";
import UserDetailsView from "@/views/AccountView/UserDetailsView.vue";
import { useUserDataStore } from "@/stores/userDataStore";
import RegisterAccount from "@/views/AccountView/RegisterAccount.vue";
import AssignRoleToUserPage from "@/views/AccountView/AssignRoleToUserPage.vue";
import CreateRolePage from "@/views/AccountView/CreateRolePage.vue";
import UserListWithRoles from "@/views/AccountView/UserListWithRoles.vue";
import Home from "@/views/Home.vue";
import MonthlyStatistics from "@/views/InventoryView/MonthlyStatistics.vue";
import CrudSettings from "@/views/CrudSettings/CrudSettings.vue";

const routes = [
  { path: "/home", name: "Home", component: Home },
  { path: "/", name: "Login", component: Login },
  { path: "/account", name: "Account", component: Account },
  { path: "/changepassword", name: "ChangePassword", component: ChangePassword },
  { path: "/actionrequest", name: "ActionRequest", component: ActionRequest },
  { path: "/createaction", name: "CreateAction", component: CreateAction },
  { path: "/products", name: "Products", component: Products },
  { path: "/suppliers", name: "Suppliers", component: Suppliers },
  { path: "/storagerooms", name: "StorageRooms", component: StorageRooms },
  { path: "/monthlyStatistics/:storageRoomId", name: "MonthlyStatistics", component: MonthlyStatistics },
  { path: "/users/:id", name: "UserDetails", component: UserDetailsView },
  { path: "/register", name: "Register", component: RegisterAccount },
  { path: "/assignRole", name: "assignRole", component: AssignRoleToUserPage },
  { path: "/createRole", name: "createRole", component: CreateRolePage },
  { path: "/userRoles", name: "userRoles", component: UserListWithRoles },

  { path: "/crudSettings", name: "crudSettings", component: CrudSettings },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const store = useUserDataStore();

  const adminOnlyRoutes = [
    "/home",
    "/actionrequest",
    "/products",
    "/product/:id",
    "/suppliers",
    "/storagerooms",
    "/monthlyStatistics/:storageRoomId",
    "/users/:id",
    "/register",
    "/createRole",
    "/assignRole",
    "/getRole",
    "/userRoles",
    "/crudSettings"
  ];

  const isRestricted = adminOnlyRoutes.some((path) => {
    const regex = new RegExp("^" + path.replace(/:\w+/g, "[^/]+") + "$");
    return regex.test(to.path);
  });

  const userRoles = store.roles ?? [];        // eeldame arrayʼd
  const allowed   = ["admin", "juhataja"];

  console.log(userRoles)

  if (isRestricted && allowed.every(r => !userRoles.includes(r))) {
    next("/createaction");
  } else {
    next();
  }
});

export default router;
