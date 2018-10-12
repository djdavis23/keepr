<template>
  <div class="grey lighten-3 blue--text">
    <h3>My Vaults</h3>
    <v-expansion-panel popout>
      <v-expansion-panel-content v-for="vault in vaults" :key="vault.id">
        <div slot="header" class="blue--text">{{vault.name}}</div>
        <v-card>
          <hr />
          <v-card-text>{{vault.description}}</v-card-text>
          <v-card-actions class="grey lighten-3" id="action-bar">

            <!-- dialog window to view keeps within a vault -->
            <v-dialog v-model="vault.dialog" width="700">
              <v-btn slot="activator" @click="setActiveVault(vault.id)" small fab flat color="purple accent-2"><i class="material-icons">expand_more</i></v-btn>
              <v-card>
                <v-card-title primary-title class="blue accent-2 white--text">
                  <h2>{{vault.name}}</h2>
                  <v-spacer></v-spacer><i @click="vault.dialog = false" class="material-icons clickable">close</i>
                </v-card-title>
                <v-container grid-list-sm>
                  <v-layout row wrap justify-center>
                    <VKView v-for="keep in vaultKeeps" :key=keep.id :keep="keep" :user="user" :activeVaultId="activeVaultId" />
                  </v-layout>
                </v-container>
              </v-card>
            </v-dialog>
            <v-btn @click="deleteVault(vault)" small fab flat color="purple accent-2"><i class="material-icons">delete_forever</i></v-btn>
          </v-card-actions>
        </v-card>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <h3><i class="material-icons clickable" @click="vaultForm=!vaultForm">add_circle</i></h3>
    <v-card v-if="vaultForm" color="grey lighten-3">
      <v-card-title>Add New Vault:</v-card-title>
      <v-form ref="vform" v-model="vvalid" @submit.prevent="createVault">
        <v-text-field counter=30 v-model="newVault.Name" :rules="nameRules" label="Name" required></v-text-field>
        <v-text-field counter=255 v-model="newVault.Description" :rules="descriptionRules" label="Description" required></v-text-field>
        <v-btn round small dark :disabled="!vvalid" color="blue accent-2" type="submit">Create</v-btn>
      </v-form>
    </v-card>
  </div>
</template>

<script>

  import VKView from './VKView.vue';

  export default {
    name: 'Vaults',

    props: ['user', 'vaults'],

    components: {
      VKView
    },

    data() {
      return {
        vaultForm: false,
        newVault: {
          Name: "",
          Description: "",
          UserId: ""
        },
        vvalid: true,

        nameRules: [
          v => !!v || "Name is required",
          v => (v && v.length <= 30) || "Name cannot exceed 30 characters"
        ],
        descriptionRules: [
          v => (v.length <= 255) || "Description cannot exceed 255 characters"
        ]
      }
    },

    methods: {

      createVault() {
        if (this.$refs.vform.validate()) {
          this.newVault.UserId = this.user.id;
          this.$store.dispatch('addVault', this.newVault);
          this.newVault = {
            Name: "",
            Description: "",
            UserId: ""
          }
          this.vaultForm = false;
        }
      },

      deleteVault(vault) {
        this.$store.dispatch('deleteVault', { Id: vault.id, UserId: vault.userId })
      },

      setActiveVault(vaultId) {
        if (this.activeVaultId == vaultId) { return }
        this.$store.dispatch("setActiveVault", vaultId)

      }
    },

    computed: {
      vaultKeeps() {
        return this.$store.state.activeVaultKeeps
      },

      activeVaultId() {
        return this.$store.state.activeVaultId
      }

    }

  }
</script>
<style>
  .clickable:hover {
    cursor: pointer;
  }
</style>