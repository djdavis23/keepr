<template>
  <v-layout row wrap class="blue accent-2">
    <v-content>
      <h1 class="mt-3 white--text">Welcome {{user.username}}</h1>
    </v-content>
    <v-spacer></v-spacer>
    <v-spacer></v-spacer>
    <v-spacer></v-spacer>
    <v-spacer></v-spacer>
    <v-spacer></v-spacer>
    <v-content>
      <v-btn round color="red" @click="logout" class="mt-3 white--text">Logout</v-btn>
    </v-content>
  </v-layout>

</template>

<script>
  export default {
    name: "home",
    mounted() {
      //blocks users not logged in
      if (!this.$store.state.user.id) {
        this.$router.push({ name: "login" });
      }
    },
    methods: {
      logout() {
        this.$store.dispatch('logout')
      }
    },
    computed: {
      user() {
        return this.$store.state.user
      }
    }
  };
</script>