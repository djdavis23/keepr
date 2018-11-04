<template>
  <div id="login">
    <!-- Toolbar header -->
    <v-toolbar fixed color="blue accent-2" class="white--text">
      <v-toolbar-title><i class="fab fa-kickstarter"></i> {{user.username}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items id="tool-bar">
        <v-btn dark small flat v-if="search" @click="search=!search">Clear Search</v-btn>
        <v-form @submit.prevent="searchKeeps" v-if="!search">
          <v-text-field v-model="searchTerm" clearable dark prepend-icon="fas fa-search" placeholder="Search by topic"><i
              class="fas fa-search"></i></v-text-field>
        </v-form>
        <v-tooltip bottom>
          <v-btn dark small fab flat slot="activator" @click="keepForm=!keepForm"><i class="material-icons">add_circle</i></v-btn>
          <span>New Keep</span>
        </v-tooltip>
        <v-btn small flat dark @click="logout">Logout</v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <!-- Main display -->
    <v-container fluid mt-5 mb-4 grid-list-sm align-content-center>
      <v-layout row wrap>
        <!-- Display user vaults on left side -->
        <v-flex xs12 md3>
          <Vaults :user="user" :vaults="vaults" />
        </v-flex>
        <!-- Display keeps on right side -->
        <v-flex xs12 md9>
          <v-layout>
            <v-flex xs12 md8 offset-md2>
              <!-- Display form for creating new keep -->
              <v-card v-if="keepForm" color="grey lighten-3">
                <v-toolbar class="grey darken-3 white--text">
                  <v-toolbar-title>Add a new keep:</v-toolbar-title>
                </v-toolbar>
                <v-form ref="form" v-model="valid" @submit.prevent="createKeep" class="ml-5 mr-5">
                  <v-text-field counter=30 :rules="nameRules" v-model="newKeep.Name" label="Name" required><i class=" material-icons">email</i></v-text-field>
                  <v-text-field counter=255 :rules="descriptionRules" v-model="newKeep.Description" label="Description"
                    required></v-text-field>
                  <v-text-field counter=255 :rules="imgRules" v-model="newKeep.Img" label="Image URL" required></v-text-field>
                  <v-checkbox v-model="newKeep.IsPrivate" label="Make keep public?" value="0" required></v-checkbox>
                  <v-btn round small dark color="blue accent-2" :disabled="!valid" type="submit">Create Keep</v-btn>
                </v-form>
              </v-card>
            </v-flex>
          </v-layout>
          <v-layout row wrap>
            <!-- view all keeps -->
            <KeepView v-if="!search" v-for="keep in keeps" :key="keep.id" :keep="keep" :user="user" :vaults="vaults" />
            <!-- view keeps based on search parameter -->
            <KeepView v-if="search" v-for="keep in searchResults" :key="keep.id" :keep="keep" :user="user" :vaults="vaults" />
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
        this.$store.dispatch('getVaultKeeps');
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
        },
        nameRules: [
          v => !!v || "Name is required",
          v => (v && v.length <= 30) || "Name cannot exceed 30 characters"
        ],
        descriptionRules: [
          v => (v && v.length <= 255) || "Description cannot exceed 255 characters"
        ],
        imgRules: [
          v => (v.length <= 255) || "Image URL cannot exceed 255 characters"
        ],
        valid: true,
        search: false,
        searchTerm: ""
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
        if (this.$refs.form.validate()) {
          this.newKeep.UserId = this.user.id;
          this.newKeep.IsPrivate = parseInt(this.newKeep.IsPrivate)
          console.log(this.newKeep)
          this.$store.dispatch('createKeep', this.newKeep)
          this.newKeep = {
            Name: "",
            Description: "",
            UserId: "",
            Img: "",
            IsPrivate: 1
          }
          this.keepForm = false;
        }
      },

      searchKeeps() {
        let searchPattern = '%' + this.searchTerm + '%'
        this.$store.dispatch("getKeepsByTopic", searchPattern)
        this.searchTerm = ""
        this.search = true
      }
    },

    computed: {
      user() {
        return this.$store.state.user
      },
      keeps() {
        return this.$store.state.keeps
      },
      vaults() {
        return this.$store.state.vaults
      },
      searchResults() {
        return this.$store.state.searchKeeps
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

  #inline-form {
    display: inline;
  }
</style>