<template>
  <div class="container">
    <form @submit.prevent="register">
      <h2 class="mb-3">Cadastro</h2>
      <div class="input">
        <label for="email">Nome*</label>
        <input required class="form-control" type="text" name="name" />
      </div>
      <div class="input">
        <label for="email">Email*</label>
        <input required class="form-control" type="email" name="email" />
        <small>Use um email válido, enviaremos código de confirmação</small>
      </div>
      <div class="input">
        <label for="password">Senha*</label>
        <input required class="form-control" type="password" name="password" />
        <div class="alternative-option" style="font-size: 10px; margin: 5px 0">
          <span @click="passwordGenerate">Gerar senha forte</span>
          <span style="margin-left: 20px" @click="showPassword">Ver senha</span>
        </div>
      </div>
      <div class="input">
        <label for="password">Confirmação da Senha*</label>
        <input
          required
          class="form-control"
          type="password"
          name="confirm_password"
        />
      </div>

      <div class="alternative-option mt-4">
        Já possui conta? <span @click="moveToLogin">Login</span>
      </div>

      <button type="submit" id="register_button" class="mt-4 btn-pers">
        Cadastrar
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
      name: "",
      email: "",
      password: "",
      confirm_password: "",
    };
  },
  methods: {
    async register(submitEvent) {
      this.name = submitEvent.target.elements.name.value;
      this.email = submitEvent.target.elements.email.value;
      this.password = submitEvent.target.elements.password.value;
      this.confirm_password =
        submitEvent.target.elements.confirm_password.value;

      if (!this.validations()) return;

      const result = await Api.createUser({
        name: this.name,
        email: this.email,
        password: this.password,
      });

      if (result.status === 204) {
        alert("Acesse seu email para validar o usuário");
        this.moveToLogin();
      } else if (result.status === 422) {
        showAlert("alert", "Email já cadastrado");
      } else {
        showAlert("alert", "Erro ao cadastrar usuário");
      }
    },
    moveToLogin() {
      this.$router.push("/");
    },
    validations() {
      if (!this.name || !this.email || !this.password) {
        showAlert("alert", "Preencha todos os campos");
        return false;
      }

      if (this.password !== this.confirm_password) {
        showAlert("alert", "As senhas não coincidem");
        return false;
      }

      if (!this.validatePassword(this.password)) {
        showAlert(
          "alert",
          "A senha deve conter no mínimo 8 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caractere especial"
        );
        return false;
      }

      return true;
    },
    validatePassword(password) {
      const regex =
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
      return regex.test(password);
    },
    passwordGenerate() {
      const _passwordGenerate = () => {
        const alphabet = "abcdefghijklmnopqrstuvwxyz";
        const upperCaseAlphabet = alphabet.toUpperCase();
        const numbers = "0123456789";
        const specialCharacters = "!@#$%*&";

        const _password = Array(12)
          .fill()
          .map(() => {
            const random = Math.floor(Math.random() * 4);
            switch (random) {
              case 0:
                return alphabet[Math.floor(Math.random() * alphabet.length)];
              case 1:
                return upperCaseAlphabet[
                  Math.floor(Math.random() * upperCaseAlphabet.length)
                ];
              case 2:
                return numbers[Math.floor(Math.random() * numbers.length)];
              case 3:
                return specialCharacters[
                  Math.floor(Math.random() * specialCharacters.length)
                ];
            }
          })
          .join("");
        return _password;
      };

      let password = "";
      while (!this.validatePassword(password)) {
        password = _passwordGenerate();
      }

      this.password = password;
      this.confirm_password = password;

      document.querySelector("input[name=password]").value = password;
      document.querySelector("input[name=confirm_password]").value = password;
    },
    showPassword() {
      const passwordInput = document.querySelector("input[name=password]");
      const confirm_passwordInput = document.querySelector(
        "input[name=confirm_password]"
      );

      if (passwordInput.type === "password") {
        passwordInput.type = "text";
        confirm_passwordInput.type = "text";
      } else {
        passwordInput.type = "password";
        confirm_passwordInput.type = "password";
      }
    },
  },
};
</script>
