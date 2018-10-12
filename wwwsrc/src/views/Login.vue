<template>
  <div id="login">
    <v-toolbar color="blue accent-2" class="white--text">
      <v-toolbar-title><i class="fab fa-kickstarter"></i> KeeprD2</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-btn small flat dark @click="loginForm = !loginForm">Login</v-btn>
        <v-btn small flat dark @click="registerForm = !registerForm">Register</v-btn>
        <v-btn small flat dark @click="browseKeeps">Browse</v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <v-container fluid grid-list-sm align-content-center class="main">
      <v-layout row wrap>
        <v-flex xs12 sm6 offset-sm3>
          <v-card color="grey lighten-3">
            <v-form pl-5 pr-5 ref="logform" v-model="logValid" v-if="loginForm" @submit.prevent="loginUser">
              <v-text-field :rules="emailRules" type="email" v-model="creds.email" label="Email" required><i class=" material-icons">email</i></v-text-field>
              <v-text-field :rules="passwordRules" type="password" v-model="creds.password" label="Password" required></v-text-field>
              <v-btn round small dark color="blue accent-2" :disabled="!logValid" type="submit">Login</v-btn>
            </v-form>
          </v-card>
        </v-flex>
        <v-flex xs12 sm10 offset-sm1>
          <v-card color="grey lighten-3">
            <v-form ref="regform" v-model="regValid" v-if="registerForm" @submit.prevent="register">
              <v-text-field counter=20 :rules="nameRules" type="text" v-model="newUser.username" label="Username"
                required></v-text-field>
              <v-text-field :rules="emailRules" type="email" v-model="newUser.email" label="Email" required></v-text-field>
              <v-text-field :rules="passwordRules" type="password" v-model="newUser.password" label="Password" required></v-text-field>
              <v-btn round small dark color="blue accent-2" :disabled="!regValid" type="submit">Register</v-btn>
            </v-form>
          </v-card>
        </v-flex>
      </v-layout>
      <v-layout row wrap>
        <KeepView v-for="keep in keeps" :key="keep.id" :keep="keep" />
      </v-layout>
    </v-container>

  </div>
</template>

<script>
  import KeepView from "@/components/KeepView.vue";

  export default {
    name: "login",
    mounted() {
      //checks for valid session
      this.$store.dispatch("authenticate");
    },

    components: {
      KeepView
    },

    data() {
      return {
        loginForm: false,
        registerForm: false,
        creds: {
          email: "",
          password: ""
        },
        newUser: {
          email: "",
          password: "",
          username: ""
        },
        regValid: true,
        logValid: true,
        nameRules: [
          v => !!v || "Username is required",
          v => (v && v.length <= 20) || "Username cannot exceed 20 characters"
        ],

        emailRules: [
          v => !!v || "Email is required",
          v => /.+@.+/.test(v) || "Not a valid email address"
        ],

        passwordRules: [
          v => !!v || "Password is required",
          v => v && (v.length >= 6) || "Password must be at least 6 characters"
        ]
      }
    },

    methods: {
      register() {
        if (this.$refs.regform.validate()) {
          this.$store.dispatch("register", this.newUser);
        }
      },
      loginUser() {
        if (this.$refs.logform.validate()) {
          this.$store.dispatch("login", this.creds);
        }
      },
      browseKeeps() {
        this.$store.dispatch("getKeeps");
      }
    },

    computed: {
      keeps() {
        return this.$store.state.keeps;
      },
      userId() {
        return this.$store.state.user.userId;
      }
    }
  };
</script>
<style>

</style>