import { createRouter, createWebHistory } from "vue-router";
import LoginView from "../views/LoginView.vue";

const routes = [
  {
    path: "/",
    name: "login",
    component: LoginView,
  },
  {
    path: "/register",
    name: "register",
    component: () => import("../views/RegisterView.vue"),
  },
  {
    path: "/user/:userId/confirm/:token",
    name: "confirm",
    component: () => import("../views/UserConfirmView.vue"),
  },
  {
    path: "/two-factor",
    name: "two-factor",
    component: () => import("../views/TwoFactorAuthView.vue"),
  },
  {
    path: "/forgot-password",
    name: "forgot-password",
    component: () => import("../views/ForgotPasswordView.vue"),
  },
  {
    path: "/user/:userId/password/reset/:token",
    name: "reset-password",
    component: () => import("../views/ResetPasswordView.vue"),
  },
  {
    path: "/dashboard",
    name: "dashboard",
    component: () => import("../views/DashboardView.vue"),
    meta: {
      authRequired: true,
    },
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const auth = !!localStorage.getItem("token");

  if (to.matched.some((record) => record.meta.authRequired && !auth)) {
    alert("Faça login para acessar essa página");
    router.push("/");
  } else if (to.name === "login" && auth) {
    router.push("/dashboard");
  } else {
    next();
  }
});

export default router;
