import axios from "axios";

const BASE_URL = "http://localhost:5015";

class Api {
  static async createUser(user) {
    try {
      const response = await axios.post(`${BASE_URL}/api/user/`, user);
      return { data: response.data, status: response.status };
    } catch (err) {
      return { data: err.response.data, status: err.response.status };
    }
  }

  static async confirmUser(userId, token) {
    try {
      const response = await axios.post(
        `${BASE_URL}/api/user/${userId}/confirm/${token}`
      );
      return { data: response.data, status: response.status };
    } catch (err) {
      return { data: err.response.data, status: err.response.status };
    }
  }

  static async loginUser(user) {
    try {
      const response = await axios.post(`${BASE_URL}/api/auth/login`, user);
      return { data: response.data, status: response.status };
    } catch (err) {
      return { data: err.response.data, status: err.response.status };
    }
  }

  static async twoFactorAuth(user) {
    try {
      const response = await axios.post(`${BASE_URL}/api/auth/2fa`, user);
      return { data: response.data, status: response.status };
    } catch (err) {
      return { data: err.response.data, status: err.response.status };
    }
  }

  static async forgotPassword(email) {
    try {
      const response = await axios.post(
        `${BASE_URL}/api/user/password/forgot`,
        { email }
      );
      return { data: response.data, status: response.status };
    } catch (err) {
      return { data: err.response.data, status: err.response.status };
    }
  }

  static async resetPassword({ userId, token, password }) {
    try {
      const response = await axios.post(`${BASE_URL}/api/user/password/reset`, {
        userId,
        token,
        password,
      });
      return { data: response.data, status: response.status };
    } catch (err) {
      return { data: err.response.data, status: err.response.status };
    }
  }
}

export default Api;
