<template>
  <v-flex xs12 sm6 md4>
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
        <v-btn v-if="user.id!=keep.userId" @click="likeKeep(keep)" small fab flat color="purple accent-2"><i class="material-icons">thumb_up</i></v-btn>
        <v-btn v-if="keep.isPrivate" @click="makePublic(keep)" small fab flat color="purple accent-2"><i class="material-icons">public</i></v-btn>
        <v-btn v-if="!keep.isPrivate" @click="smVisible=!smVisible" small fab flat color="purple accent-2"><i class="material-icons">share</i></v-btn>
        <v-btn v-if="user.id==keep.userId && keep.isPrivate==1" @click="deleteKeep(keep)" small fab flat color="purple accent-2"><i
            class="material-icons">delete_forever</i></v-btn>
        <v-btn @click="removeKeepFromVault(keep.id)" small fab flat color="purple accent-2"><i class="material-icons clickable">remove_circle_outline</i></v-btn>
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
    name: "VKView",
    data() {
      return {
        smVisible: false
      };
    },
    props: ["keep", "user", "activeVaultId"],
    methods: {
      deleteKeep(keep) {
        this.$store.dispatch("deleteKeep", {
          Id: keep.id,
          UserId: keep.userId,
          IsPrivate: keep.isPrivate
        });
      },

      removeKeepFromVault(keepId) {
        this.$store.dispatch("removeKeepFromVault", {
          VaultId: this.activeVaultId,
          KeepId: keepId,
          UserId: this.user.id
        });
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

  #action-bar {
    display: flex;
    justify-content: space-between;
  }

  .large-font {
    font-size: 2em;
  }
</style>