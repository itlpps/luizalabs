<template>
  <div>
    <div v-if="userConfirmed === 0">
      <h3>Verificando usuário...</h3>
    </div>
    <div v-if="userConfirmed === 1">
      <img
        style="margin: 0 auto; display: block"
        width="100"
        height="100"
        src="https://www.iconarchive.com/download/i103471/paomedia/small-n-flat/sign-check.ico"
      />
      <h3>Usuário validado com sucesso!</h3>
      <div class="alternative-option mt-4">
        <span @click="moveToLogin">Login</span>
      </div>
    </div>
    <div v-if="userConfirmed === 2">
      <img
        style="margin: 0 auto; display: block"
        width="100"
        height="100"
        src="https://www.iconarchive.com/download/i103471/paomedia/small-n-flat/sign-error.ico"
      />
      <h3>Erro ao validar usuário!</h3>
      <div class="alternative-option mt-4">
        <span @click="moveToLogin">Login</span>
      </div>
    </div>
  </div>
</template>

<script>
import Api from "@/service/api";

export default {
  async beforeMount() {
    const userId = this.$route.params.userId;
    const token = this.$route.params.token;

    const result = await Api.confirmUser(userId, token);
    if (result.status === 204) {
      localStorage.setItem("userId", userId);
      this.userConfirmed = 1;
    } else {
      this.userConfirmed = 2;
    }
  },
  data() {
    return {
      userConfirmed: 0,
    };
  },
  methods: {
    moveToLogin() {
      this.$router.push("/");
    },
  },
};
</script>
