<template>
  <div id="login">
    <v-toolbar fixed color="blue accent-2" class="white--text">
      <v-toolbar-title><i class="fab fa-kickstarter"></i> {{user.username}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items id="tool-bar">
        <v-form @submit="searchKeeps">
          <v-text-field clearable dark prepend-icon="fas fa-search"><i class="fas fa-search"></i></v-text-field>
        </v-form>
        <v-btn dark small fab flat @click="keepForm=!keepForm"><i class="material-icons">add_circle</i></v-btn>
        <v-btn small flat dark @click="logout">Logout</v-btn>
      </v-toolbar-items>
    </v-toolbar>
    <v-container fluid mt-5 mb-4 grid-list-sm align-content-center>
      <v-layout row wrap>
        <v-flex xs12 md3>
          <Vaults :user="user" />
        </v-flex>
        <v-flex xs12 md9>
          <v-layout>
            <v-flex xs12 md8 offset-md2>
              <v-card color="grey lighten-3">
                <v-form v-if="keepForm" @submit.prevent="createKeep">
                  <v-text-field v-model="newKeep.Name" label="Name" required><i class=" material-icons">email</i></v-text-field>
                  <v-text-field v-model="newKeep.Description" label="Description" required></v-text-field>
                  <v-text-field v-model="newKeep.Img" label="Image URL" required></v-text-field>
                  <v-checkbox v-model="newKeep.IsPrivate" label="Make keep public?" value="0" required></v-checkbox>
                  <v-btn round small dark color="blue accent-2" type="submit">Create Keep</v-btn>
                </v-form>
              </v-card>
            </v-flex>
          </v-layout>
          <v-layout row wrap>
            <KeepView v-for="keep in keeps" :key="keep.id" :keep="keep" :user="user" />
          </v-layout>
        </v-flex>
      </v-layout>
    </v-container>

  </div>
</template>

<script>

  import KeepView from "@/components/KeepView.vue";
  import Vaults from "@/components/Vaults.vue";

  export default {
    name: "home",
    mounted() {
      //blocks users not logged in
      if (!this.$store.state.user.id) {
        this.$router.push({ name: "login" });
      }
      else {
        this.$store.dispatch('getKeeps');
        this.$store.dispatch('getVaults');
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
          IsPrivate: 1
        }
      }
    },

    components: {
      KeepView,
      Vaults
    },

    methods: {
      logout() {
        this.$store.dispatch('logout')
      },
      createKeep() {
        this.newKeep.UserId = this.user.id;
        this.newKeep.IsPrivate = parseInt(this.newKeep.IsPrivate)
        console.log(this.newKeep)
        this.$store.dispatch('createKeep', this.newKeep)
        this.newKeep = {
          Name: "",
          Description: "",
          UserId: "",
          Img: "",
          IsPrivate: 0
        }
        this.keepForm = false;
      },

      searchKeeps() {
        alert("This feature coming soon!")
      }
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