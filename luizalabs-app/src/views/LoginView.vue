<template>
  <div class="container">
    <form @submit.prevent="login">
      <h2 class="mb-3">Login</h2>
      <div class="input">
        <label for="email">Email</label>
        <input class="form-control" type="text" name="email" required />
      </div>
      <div class="input">
        <label for="password">Senha</label>
        <input class="form-control" type="password" name="password" required />
      </div>
      <div class="alternative-option mt-4">
        Não possui conta? <span @click="moveToRegister">Criar conta</span>
      </div>
      <div class="alternative-option mt-4">
        <span @click="forgotPassword">Esqueci a senha</span>
      </div>
      <button type="submit" class="mt-4 btn-pers" id="login_button">
        Login
      </button>
      <div
        class="alert alert-warning alert-dismissible fade show mt-5 d-none"
        role="alert"
        id="alert"
      >
        <button
          type="button"
          class="btn-close"
          data-bs-dismiss="alert"
          aria-label="Close"
        ></button>
      </div>
    </form>
  </div>
</template>

<script>
import Api from "@/service/api";
import showAlert from "@/helper/alert";

export default {
  data() {
    return {
      email: "",
      password: "",
    };
  },
  methods: {
    async login(submitEvent) {
      this.email = submitEvent.target.elements.email.value;
      this.password = submitEvent.target.elements.password.value;

      const result = await Api.loginUser({
        email: this.email,
        password: this.password,
      });
      if (result.status === 200) {
        localStorage.setItem("userId", result.data.id);
        this.$router.push("/two-factor");
      } else {
        showAlert("alert", "Usuário ou senha inválidos!");
      }
    },
    moveToRegister() {
      this.$router.push("/register");
    },
    forgotPassword() {
      this.$router.push("/forgot-password");
    },
  },
};
</script>
