<template>
  <div class="container">
    <form @submit.prevent="validate">
      <h2 class="mb-3">Autenticação em 2 fatores</h2>
      <p>Um código foi enviado para seu email, use-o para fazer o login</p>
      <div class="input">
        <label for="token">Código</label>
        <input class="form-control" type="text" name="token" required />
      </div>
      <button type="submit" class="mt-4 btn-pers" id="validate_button">
        Validar
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
      token: "",
    };
  },
  methods: {
    async validate(submitEvent) {
      this.token = submitEvent.target.elements.token.value;

      const result = await Api.twoFactorAuth({
        userId: localStorage.getItem("userId"),
        token: this.token,
      });
      if (result.status === 200) {
        localStorage.removeItem("userId");
        localStorage.setItem("token", result.data.token);
        localStorage.setItem("name", result.data.name);
        localStorage.setItem("email", result.data.email);
        this.$router.push("/dashboard");
      } else {
        showAlert("alert", "Código inválido!");
      }
    },
  },
};
</script>
