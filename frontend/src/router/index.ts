import { createRouter, createWebHistory } from 'vue-router';
import Login from "@/views/AccountView/Login.vue";
import Account from "@/views/AccountView/Account.vue";
import ChangePassword from "@/views/AccountView/ChangePassword.vue";
import CreateAction from "@/views/ActionViews/CreateAction.vue";
import ActionRequest from "@/views/ActionViews/ActionRequest.vue";
import Products from "@/views/ProductView/Products.vue";
import SpecificProduct from "@/views/ProductView/SpecificProduct.vue";
import CreateProduct from "@/views/ProductView/CreateProduct.vue";
import EditProduct from "@/views/ProductView/EditProduct.vue";
import Suppliers from "@/views/SupplierView/Suppliers.vue";
import CreateSupplier from "@/views/SupplierView/CreateSupplier.vue";
import EditSupplier from "@/views/SupplierView/EditSupplier.vue";
import StorageRooms from "@/views/InventoryView/StorageRooms.vue";
import CurrentStock from "@/views/InventoryView/CurrentStock.vue";
import EditCurrentstock from "@/views/InventoryView/EditCurrentstock.vue";
import CreateCurrentStock from "@/views/InventoryView/CreateCurrentStock.vue";
import UserDetailsView from "@/views/AccountView/UserDetailsView.vue";
import { useUserDataStore } from "@/stores/userDataStore";
import RegisterAccount from "@/views/AccountView/RegisterAccount.vue";
import GetRolesPage from "@/views/AccountView/GetRolesPage.vue";
import AssignRoleToUserPage from "@/views/AccountView/AssignRoleToUserPage.vue";
import CreateRolePage from "@/views/AccountView/CreateRolePage.vue";
import UserListWithRoles from "@/views/AccountView/UserListWithRoles.vue";
import Home from "@/views/Home.vue";
import {User} from "lucide-vue-next";

const routes = [
  { path: "/home", name: "Home", component: Home },
  { path: "/", name: "Login", component: Login },
  { path: "/account", name: "Account", component: Account },
  { path: "/changepassword", name: "ChangePassword", component: ChangePassword },
  { path: "/actionrequest", name: "ActionRequest", component: ActionRequest },
  { path: "/createaction", name: "CreateAction", component: CreateAction },
  { path: "/products", name: "Products", component: Products },
  { path: "/product/:id", name: "SpecificProduct", component: SpecificProduct },
  { path: "/createproduct", name: "CreateProduct", component: CreateProduct },
  { path: "/editproduct/:id", name: "EditProduct", component: EditProduct },
  { path: "/suppliers", name: "Suppliers", component: Suppliers },
  { path: "/createsupplier", name: "CreateSupplier", component: CreateSupplier },
  { path: "/editsupplier/:id", name: "EditSupplier", component: EditSupplier },
  { path: "/storagerooms", name: "StorageRooms", component: StorageRooms },
  { path: "/currentstock/:storageRoomId", name: "CurrentStock", component: CurrentStock },
  { path: "/editcurrentstock/:id", name: "EditCurrentStock", component: EditCurrentstock },
  { path: "/createcurrentStock", name: "CreateCurrentStock", component: CreateCurrentStock },
  { path: "/users/:id", name: "UserDetails", component: UserDetailsView },
  { path: "/register", name: "Register", component: RegisterAccount },
  { path: "/getRole", name: "getRole", component: GetRolesPage },
  { path: "/assignRole", name: "assignRole", component: AssignRoleToUserPage },
  { path: "/createRole", name: "createRole", component: CreateRolePage },
  { path: "/userRoles", name: "userRoles", component: UserListWithRoles },
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
    "/createproduct",
    "/editproduct/:id",
    "/product/:id",
    "/suppliers",
    "/createsupplier",
    "/editsupplier/:id",
    "/storagerooms",
    "/currentstock/:storageRoomId",
    "/editcurrentstock/:id",
    "/createcurrentStock",
    "/users/:id",
    "/register",
    "/createRole",
    "/assignRole",
    "/getRole",
    "/userRoles",
  ];

  const isRestricted = adminOnlyRoutes.some((path) => {
    const regex = new RegExp("^" + path.replace(/:\w+/g, "[^/]+") + "$");
    return regex.test(to.path);
  });

  const userRoles = store.roles ?? [];        // eeldame arrayʼd
  const allowed   = ["admin", "manager"];

  console.log(userRoles)

  if (isRestricted && allowed.every(r => !userRoles.includes(r))) {
    next("/createaction");
  } else {
    next();
  }
});

export default router;
