<template>
  <v-flex xs12 sm6 md3>
    <v-card>
      <v-img :src="keep.img"></v-img>
      <v-card-text class="grey lighten-3">
        <i class="fas fa-thumbs-up"></i> {{keep.views}}&nbsp&nbsp&nbsp&nbsp
        <i class="fas fa-plus-circle"></i> {{keep.keeps}}&nbsp&nbsp&nbsp&nbsp
        <i class="fas fa-share-alt"></i> {{keep.shares}}
      </v-card-text>
      <v-card-title primary-title>
        <div class="text-box">
          <h3 class="headline mb-0">{{keep.name}}</h3>
          <div>{{keep.description}}</div>
        </div>
      </v-card-title>
      <v-card-actions class="grey lighten-3" id="action-bar">
        <v-btn small fab flat color="purple accent-2"><i class="material-icons">thumb_up</i></v-btn>
        <v-menu offset-y>
          <v-btn slot="activator" small fab flat color="purple accent-2"><i class="material-icons">add_circle</i></v-btn>
          <v-list>
            <v-list-tile v-for="vault in vaults" :key="vault.id" @click="addKeepToVault(keep.id, vault.id)">
              <v-list-tile-title>{{vault.name}}</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
        <v-btn v-if="keep.isPrivate" small fab flat color="purple accent-2"><i class="material-icons">share</i></v-btn>
        <v-btn v-if="user.id==keep.userId && keep.isPrivate==1" @click="deleteKeep(keep)" small fab flat color="purple accent-2"><i
            class="material-icons">delete_forever</i></v-btn>
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script>
  export default {
    name: "KeepView",
    data() {
      return {};

    },
    props: ["keep", "user", "vaults"],
    methods: {
      deleteKeep(keep) {
        this.$store.dispatch('deleteKeep', { Id: keep.id, UserId: keep.userId, IsPrivate: keep.isPrivate });
      },

      addKeepToVault(keepId, vaultId) {
        this.$store.dispatch("addKeepToVault", { VaultId: vaultId, KeepId: keepId, UserId: this.user.id })
      }
    },
    computed: {}
  };
</script>
<style>
  .text-box {
    text-align: left;
  }

  #action-bar {
    display: flex;
    justify-content: space-between;
  }
</style>