<template>
  <div id="login">
    <v-toolbar color="blue accent-2" class="white--text">
      <v-toolbar-title><i class="fab fa-kickstarter"></i> {{user.username}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items id="tool-bar">
        <v-text-field dark prepend-icon="fas fa-search"><i class="fas fa-search"></i></v-text-field>
        <v-btn dark small fab flat @click="keepForm=!keepForm"><i class="material-icons">add_circle</i></v-btn>
        <v-btn small flat dark @click="logout">Logout</v-btn>
      </v-toolbar-items>
    </v-toolbar>
    <v-container fluid grid-list-sm align-content-center>
      <v-layout row wrap>
        <v-flex xs12 md3></v-flex>
        <v-flex xs12 md9>
          <v-layout>
            <v-flex xs12 md8 offset-md2>
              <v-card color="grey lighten-3">
                <v-form v-if="keepForm" @submit.prevent="createKeep">
                  <v-text-field v-model="newKeep.Name" label="Name" required><i class=" material-icons">email</i></v-text-field>
                  <v-text-field v-model="newKeep.Description" label="Description" required></v-text-field>
                  <v-text-field v-model="newKeep.Img" label="Image URL" required></v-text-field>
                  <v-checkbox v-model="newKeep.IsPrivate" label="Private Keep?" value=1 required></v-checkbox>
                  <v-btn round small dark color="blue accent-2" type="submit">Create</v-btn>
                </v-form>
              </v-card>
            </v-flex>
          </v-layout>
          <v-layout row wrap>
            <KeepView v-for="keep in keeps" :key="keep.id" :keep="keep" />
          </v-layout>
        </v-flex>
      </v-layout>
    </v-container>

  </div>
</template>

<script>

  import KeepView from "@/components/KeepView.vue";

  export default {
    name: "home",
    mounted() {
      //blocks users not logged in
      if (!this.$store.state.user.id) {
        this.$router.push({ name: "login" });
      }
      else {
        this.$store.dispatch('getKeeps');
      }
    },

    data() {
      return {
        keepForm: false,
        newKeep: {
          Name: "",
          Description: "",
          UserId: "",
          Img: "",
          isPrivate: 0
        }
      }
    },

    components: {
      KeepView
    },

    methods: {
      logout() {
        this.$store.dispatch('logout')
      },
      createKeep() {
        this.newKeep.UserId = this.user.userId;
        this.$store.dispatch('createKeep', this.newKeep)
      },
    },

    computed: {
      user() {
        return this.$store.state.user
      },
      keeps() {
        return this.$store.state.keeps
      }

    }
  };
</script>
<style>
  #tool-bar {
    max-width: 55vw;
  }

  #tagline {
    margin-right: 5px;
  }
</style>