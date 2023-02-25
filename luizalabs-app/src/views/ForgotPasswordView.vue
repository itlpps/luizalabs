<template>
  <div class="container">
    <form @submit.prevent="sendLink">
      <h2 class="mb-3">Esqueci a senha</h2>
      <p>Você receberá em seu email um link para alterar a senha</p>
      <div class="input">
        <label for="token">Email</label>
        <input class="form-control" type="text" name="email" required />
      </div>
      <button type="submit" class="mt-4 btn-pers" id="send_button">
        Enviar link
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
    };
  },
  methods: {
    async sendLink(submitEvent) {
      this.email = submitEvent.target.elements.email.value;

      const result = await Api.forgotPassword(this.email);
      if (result.status === 200) {
        alert("Verifique seu email para alterar a senha");
        this.$router.push("/");
      } else {
        showAlert("alert", "Email não cadastrado");
      }
    },
  },
};
</script>
