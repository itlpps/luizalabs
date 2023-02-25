<template>
  <div>
    <div class="text-center">
      Bem vindo <span>{{ this.name }}</span
      >, Você está logado!
    </div>
    <div id="username_display" class="display-6">{{ this.email }}</div>
    <button id="sign_out" class="mt-4 btn btn-danger" @click="signOut">
      Logout
    </button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      email: localStorage.getItem("email"),
      name: localStorage.getItem("name"),
    };
  },
  methods: {
    signOut() {
      localStorage.clear();
      this.$router.push("/");
    },
  },
};
</script>
