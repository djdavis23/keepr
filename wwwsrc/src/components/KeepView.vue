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
        <v-tooltip bottom v-if="keep.userId != user.id">
          <v-btn slot="activator" @click="likeKeep(keep)" small fab flat color="purple accent-2"><i class="material-icons">thumb_up</i></v-btn>
          <span>Like Keep</span>
        </v-tooltip>
        <v-tooltip bottom v-if="keep.isPrivate">
          <v-btn slot="activator" @click="makePublic(keep)" small fab flat color="purple accent-2"><i class="material-icons">public</i></v-btn>
          <span>Make keep public</span>
        </v-tooltip>
        <v-tooltip bottom v-if="!keep.isPrivate">
          <v-btn slot="activator" @click="smVisible=!smVisible" small fab flat color="purple accent-2"><i class="material-icons">share</i></v-btn>
          <span>Share Keep</span>
        </v-tooltip>
        <v-tooltip bottom v-if="user.id==keep.userId && keep.isPrivate==1">
          <v-btn slot="activator" @click="deleteKeep(keep)" small fab flat color="purple accent-2"><i class="material-icons">delete_forever</i></v-btn>
          <span>Delete Keep</span>
        </v-tooltip>
        <v-menu offset-y>
          <v-btn slot="activator" small fab flat color="purple accent-2"><i class="material-icons">add_circle</i></v-btn>
          <v-list class="grey lighten-3">
            <v-list-tile class="blue--text font-weight-bold">
              <v-list-tile-title>Add to Vault:</v-list-tile-title>
            </v-list-tile>
            <hr />
            <v-list-tile v-for="vault in vaults" :key="vault.id" @click="addKeepToVault(keep, vault.id)">
              <v-list-tile-title>{{vault.name}}</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-card-actions>
      <v-card-actions v-if="smVisible" class="grey lighten-3" id="media-bar">
        <v-btn @click="shareKeep(keep, 'facebook')" small fab flat color="purple accent-2"><i class="fab fa-facebook-square large-font"></i></v-btn>
        <v-btn @click="shareKeep(keep, 'instagram')" small fab flat color="purple accent-2"><i class="fab fa-instagram large-font"></i></v-btn>
        <v-btn @click="shareKeep(keep, 'email')" small fab flat color="purple accent-2"><i class="far fa-envelope large-font"></i></i></v-btn>
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script>
  export default {
    name: "KeepView",
    data() {
      return {
        smVisible: false
      };

    },
    props: ["keep", "user", "vaults"],
    methods: {
      deleteKeep(keep) {
        this.$store.dispatch('deleteKeep', { Id: keep.id, UserId: keep.userId, IsPrivate: keep.isPrivate });
      },

      addKeepToVault(keep, vaultId) {
        this.$store.dispatch("addKeepToVault", {
          vaultAdd: { VaultId: vaultId, KeepId: keep.id, UserId: this.user.id },
          keepUpdate: keep
        })

      },

      makePublic(keep) {
        keep.isPrivate = 0;
        this.$store.dispatch('updateKeep', keep)
      },

      likeKeep(keep) {
        keep.views++
        this.$store.dispatch('updateKeep', keep)
      },

      shareKeep(keep, mediaType) {
        //to be further developed
        keep.shares++
        this.$store.dispatch('updateKeep', keep)
        this.smVisible = false;
      }
    },
    computed: {}
  };
</script>
<style>
  .text-box {
    text-align: left;
  }

  #action-bar,
  #media-bar {
    display: flex;
    justify-content: space-between;
  }

  .large-font {
    font-size: 2em;
  }
</style>