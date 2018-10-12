<template>
  <v-app id="app">
    <router-view />
    <v-footer app color="blue accent-2">
      <v-layout justify-end>
        <h4 class="white--text" id="tagline"><i class="fas fa-at"></i>D2dev</h4>
      </v-layout>
    </v-footer>
  </v-app>
</template>

<style>
  #app {
    font-family: "Avenir", Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;

  }

  #nav {
    padding: 30px;
  }

  #nav a {
    font-weight: bold;
    color: #2c3e50;
  }

  #nav a.router-link-exact-active {
    color: #42b983;
  }
</style>