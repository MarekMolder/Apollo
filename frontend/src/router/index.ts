import { createRouter, createWebHistory } from 'vue-router';
import Login from "@/views/AccountView/Login.vue";
import Account from "@/views/AccountView/Account.vue";
import ChangePassword from "@/views/AccountView/ChangePassword.vue";
import CreateAction from "@/views/ActionViews/CreateAction.vue";
import ActionRequest from "@/views/ActionViews/ActionRequest.vue";
import Products from "@/views/ProductView/Products.vue";
import SpecificProduct from "@/views/ProductView/SpecificProduct.vue";
import Suppliers from "@/views/SupplierView/Suppliers.vue";
import StorageRooms from "@/views/InventoryView/StorageRooms.vue";
import UserDetailsView from "@/views/AccountView/UserDetailsView.vue";
import { useUserDataStore } from "@/stores/userDataStore";
import RegisterAccount from "@/views/AccountView/RegisterAccount.vue";
import GetRolesPage from "@/views/AccountView/GetRolesPage.vue";
import AssignRoleToUserPage from "@/views/AccountView/AssignRoleToUserPage.vue";
import CreateRolePage from "@/views/AccountView/CreateRolePage.vue";
import UserListWithRoles from "@/views/AccountView/UserListWithRoles.vue";
import Home from "@/views/Home.vue";
import MonthlyStatistics from "@/views/InventoryView/MonthlyStatistics.vue";
import ActionType from "@/views/ActionTypeView/ActionType.vue";
import Address from "@/views/AddressView/Address.vue";
import ProductCategory from "@/views/ProductCategoryView/ProductCategory.vue";
import Reason from "@/views/ReasonView/Reason.vue";
import RecipeComponent from "@/views/RecipeComponentView/RecipeComponent.vue";

const routes = [
  { path: "/home", name: "Home", component: Home },
  { path: "/", name: "Login", component: Login },
  { path: "/account", name: "Account", component: Account },
  { path: "/changepassword", name: "ChangePassword", component: ChangePassword },
  { path: "/actionrequest", name: "ActionRequest", component: ActionRequest },
  { path: "/createaction", name: "CreateAction", component: CreateAction },
  { path: "/products", name: "Products", component: Products },
  { path: "/product/:id", name: "SpecificProduct", component: SpecificProduct },
  { path: "/suppliers", name: "Suppliers", component: Suppliers },
  { path: "/storagerooms", name: "StorageRooms", component: StorageRooms },
  { path: "/monthlyStatistics/:storageRoomId", name: "MonthlyStatistics", component: MonthlyStatistics },
  { path: "/users/:id", name: "UserDetails", component: UserDetailsView },
  { path: "/register", name: "Register", component: RegisterAccount },
  { path: "/getRole", name: "getRole", component: GetRolesPage },
  { path: "/assignRole", name: "assignRole", component: AssignRoleToUserPage },
  { path: "/createRole", name: "createRole", component: CreateRolePage },
  { path: "/userRoles", name: "userRoles", component: UserListWithRoles },

  { path: "/actionType", name: "actionType", component: ActionType },
  { path: "/address", name: "address", component: Address },
  { path: "/productCategory", name: "productCategory", component: ProductCategory },
  { path: "/reason", name: "reason", component: Reason },
  { path: "/recipeComponent", name: "recipeComponent", component: RecipeComponent },
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
    "/actionType",
    "/address",
    "/productCategory",
    "/reason",
    "/recipeComponent"
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
