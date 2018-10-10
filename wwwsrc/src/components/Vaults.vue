<template>
  <div class="grey lighten-3 blue--text">
    <h3>My Vaults</h3>
    <v-expansion-panel popout>
      <v-expansion-panel-content v-for="vault in vaults" :key="vault.id">
        <div slot="header">{{vault.name}}</div>
        <v-card>
          <v-card-text>{{vault.description}}</v-card-text>
        </v-card>
      </v-expansion-panel-content>
    </v-expansion-panel>
    <h3><i class="material-icons" @click="vaultForm=!vaultForm">add_circle</i></h3>
    <v-card v-if="vaultForm" color="grey lighten-3">
      <v-card-title>Add New Vault:</v-card-title>
      <v-form @submit.prevent="createVault">
        <v-text-field v-model="newVault.Name" label="Name" required></v-text-field>
        <v-text-field v-model="newVault.Description" label="Description" required></v-text-field>
        <v-btn round small dark color="blue accent-2" type="submit">Create</v-btn>
      </v-form>
    </v-card>
  </div>
</template>

<script>

  export default {
    name: 'Vaults',

    props: ['user'],

    data() {
      return {
        vaultForm: false,
        newVault: {
          Name: "",
          Description: "",
          UserId: ""
        }
      }
    },

    methods: {

      createVault() {
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

    computed: {
      vaults() {
        return this.$store.state.vaults;
      }
    }

  }
</script>
<style>
  .blue-text {
    color: #448AFF;
  }
</style>